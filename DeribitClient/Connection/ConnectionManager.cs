using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using DeribitClient.Messages;
using DeribitClient.Messages.MarketData;
using DeribitClient.Validator;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DeribitClient.Core;
using DeribitClient.Messages.SessionManagement;
using DeribitClient.Messages.Supporting;
using Microsoft.Extensions.Logging;

namespace DeribitClient
{
    public class ConnectionManager
    {
        private static long _sequence;

        static ConnectionManager()
        {
            _sequence = 1;
        }

        private static long getSequence()
        {
            return _sequence++;
        }

        public bool Connected
        {
            get => this._connection.Connected;
        }

        public event EventHandler<bool> OnConnectionChange;
        public event EventHandler<ServerSideException> OnServerError;
        public XChannel<Dictionary<string, object>> Incoming { get; private set; }

        private ChanneledConnection _connection;
        private ConcurrentDictionary<string, string> _methodMappings;
        private CancellationTokenSource _tokenSource;
        private IServerErrorHandler? _serverErrorHandler;
        private ILogger _logger;

        public ConnectionManager(IOptions<ConnectionSettings> connectionSettings, IServerErrorHandler? errorHandler,
            ILogger<ConnectionManager> log)
        {
            this._connection = new ChanneledConnection(connectionSettings);
            this._methodMappings = new ConcurrentDictionary<string, string>();
            this._tokenSource = new CancellationTokenSource();
            this._serverErrorHandler = errorHandler;
            this._logger = log;
            this.Incoming = new XChannel<Dictionary<string, object>>();
            this._connection.OnConnectionChanged += (sender, b) => this.OnConnectionChange?.Invoke(sender, b);
        }

        public async Task Connect()
        {
            this._logger.LogInformation("Connecting");
            await this._connection.Connect();
            this.MessageConsumer(this._tokenSource.Token);
        }

        public async Task Disconnect()
        {
            await this._connection.Disconnect();
            this._logger.LogInformation("Disconnecting");
        }


        public void Send(IRequest message)
        {
            var id = getSequence().ToString();
            this._connection.Outgoing.Write(message.ToJson(id, out string methodName));
            this._methodMappings.TryAdd(id, methodName);
            this._logger.LogInformation($"Send - {id} {methodName}", id, methodName);
        }

        private void InternalSend(IRequest message)
        {
            var id = (getSequence() * -1).ToString();
            this._connection.Outgoing.Write(message.ToJson(id, out string methodName));
            this._methodMappings.TryAdd(id, methodName);
            this._logger.LogInformation($"Internal Send - {id} {methodName}", id, methodName);
        }

        public void SetHeartbeat(int interval)
        {
            if (interval < 10)
            {
                throw new ArgumentException("Interval may not be less 10");
            }

            var setHeartbeatMsg = new SetHeartbeatRequest()
            {
                interval = interval
            };
            this.InternalSend(setHeartbeatMsg);
            this._logger.LogInformation($"SetHeartbeat - {interval}", interval);
        }

        private void MessageConsumer(CancellationToken token)
        {
            TaskFactory factory = new TaskFactory();
            factory.StartNew(consumer, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            async void consumer()
            {
                while (!token.IsCancellationRequested)
                {
                    var msg = await this._connection.Incoming.Read();
                    this._logger.LogInformation("Received Message");
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    var responseObject = JsonConvert.DeserializeObject(msg, settings);
                    Dictionary<string, object> response =
                        Converter.ToSystemObject(responseObject) as Dictionary<string, object>;

                    if (response.TryGetValue("id", out object id))
                    {
                        this._methodMappings.TryRemove(id as string, out var T);
                        response.TryAdd("MsgType", T);
                    }

                    bool forwardToIncoming = this.Process(response);

                    if (forwardToIncoming)
                        this.Incoming.Write(response);
                    this._logger.LogInformation($"Message - Id: {id} Forward: {forwardToIncoming}", id,
                        forwardToIncoming);
                }
            }
        }

        private bool Process(Dictionary<string, object> response)
        {
            bool fwd = true;
            // Handle Errors -> Handle Heartbeat Requests
            fwd &= this.HandleErrors(response);
            fwd &= this.HandleHeartbeats(response);
            fwd &= this.HandleInternals(response);

            return fwd;
        }

        private bool HandleInternals(Dictionary<string, object> response)
        {
            this._logger.LogInformation("HandleInternals");
            if (response.TryGetValue("id", out object id))
            {
                if (Convert.ToInt32(response["id"]) < 0)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        private bool HandleErrors(Dictionary<string, object> response)
        {
            this._logger.LogInformation("HandleErrors");

            var error = this._serverErrorHandler.ValidateDictionary(response);
            if (error is not null)
            {
                this.OnServerError?.Invoke(this, error);
                return false;
            }

            return true;
        }

        private bool HandleHeartbeats(Dictionary<string, object> response)
        {
            this._logger.LogInformation("HandleHeartbeats");

            bool fwd = true;
            if (response.Count == 3)
            {
                bool isNotification = response.ContainsKey("jsonrpc") && response.ContainsKey("method") &&
                                      response.ContainsKey("params");

                if (isNotification && (response["method"] as string) == "heartbeat")
                {
                    fwd = false;
                    if ((response["params"] as Dictionary<string, object>)["type"] as string == "test_request")
                    {
                        var msg = new TestRequest();
                        this.InternalSend(msg);
                        this._logger.LogInformation("Sent Test Request");
                    }
                }
            }

            return fwd;
        }
    }
}