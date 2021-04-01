using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeribitClient.Messages;
using DeribitClient.Messages.MarketData;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeribitClient
{
    public class RoutedConnection
    {
        private static long _sequence;

        static RoutedConnection()
        {
            _sequence = 0;
        }

        private static long getSequence()
        {
            return _sequence++;
        }
        
        public bool Connected { get => this._connection.Connected; }
        public XChannel<Dictionary<string, object>> MarketDataMessagesOut { get; private set; }
        
        private ChanneledConnection _connection;
        private ConcurrentDictionary<string, string> _methodMappings;
        private CancellationTokenSource _tokenSource;
        
        public RoutedConnection(IOptions<ConnectionSettings> connectionSettings)
        {
            this._connection = new ChanneledConnection(connectionSettings);
            this._methodMappings = new ConcurrentDictionary<string, string>();
            this._tokenSource = new CancellationTokenSource();
            this.MarketDataMessagesOut = new XChannel<Dictionary<string, object>>();
        }

        public async Task Connect()
        {
            await this._connection.Connect();
            MessageConsumer(this._tokenSource.Token);
        }

        public async Task Disconnect() => await this._connection.Disconnect();

        public void Send(IRequest message)
        {
            var id = RoutedConnection.getSequence().ToString();
            this._connection.Outgoing.Write(message.ToJson(id, out string methodName));
            this._methodMappings.TryAdd(id, methodName);
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
                    var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(msg);

                    var id = response["id"].ToString();
                    this._methodMappings.TryRemove(id, out var T);
                    response.TryAdd("MsgType", T);

                    this.MarketDataMessagesOut.Write(response);
                }
            }
        }
    }
}