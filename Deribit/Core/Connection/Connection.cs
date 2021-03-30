using System;
using System.Collections.Generic;
using System.Text;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using Deribit.Core.Authentication;
using Deribit.Core.Messages;
using Deribit.Core.Messages.Authentication;
using Deribit.Core.Messages.Supporting;
using Deribit.Core.Notifications;
using Deribit.Core.Validator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Deribit.Core.Connection
{
    public class Connection : IObservable<string>
    {
        private const int INITIAL_BUFFERSIZE = 1024;
        private const int BUFFERSIZE_INCREMENT = 1024;

        public bool Connected
        {
            get => _webSocket.State == WebSocketState.Open;
        }

        public bool Sending { get; private set; }
        public bool Receiving { get; private set; }

        private ClientWebSocket _webSocket;
        private Dictionary<Guid, IObserver<string>> _identifiedObservers;
        private List<IObserver<string>> _observers;
        private List<IObserver<string>> _notificationObservers;
        private CancellationTokenSource _tokenSource;
        private Queue<Tuple<Guid, IMessage>> _messages;
        private Uri _server_address;
        private IServerErrorHandler _errorHandler;

        public Connection(Uri serverAddress, CancellationTokenSource tokenSource, IServerErrorHandler handler)
        {
            this._server_address = serverAddress;
            this._tokenSource = tokenSource;
            this._identifiedObservers = new Dictionary<Guid, IObserver<string>>();
            this._notificationObservers = new List<IObserver<string>>();
            this._observers = new List<IObserver<string>>();
            this._messages = new Queue<Tuple<Guid, IMessage>>();
            this._errorHandler = handler;
        }

        public void Connect()
        {
            this._webSocket = new ClientWebSocket();

            _establishConnection().Wait();
            _startReceiving();
        }

        public void Disconnect()
        {
            var logoutRequest = new LogoutMessage();
            this.SendMessage(logoutRequest).Wait();
            SpinWait.SpinUntil(() => this.Sending == false);
            this._webSocket.CloseAsync(WebSocketCloseStatus.Empty, "", this._tokenSource.Token);
        }

        public Connection(Uri serverAddress, CancellationTokenSource tokenSource) : this(serverAddress, tokenSource,
            new ServerErrorHandler())
        {
        }

        public IDisposable Subscribe(IObserver<string> observer, Guid observerId)
        {
            if (observerId == Guid.Empty)
            {
                if (!_observers.Contains(observer))
                {
                    _observers.Add(observer);
                    return new Unsubscriber<string, List<IObserver<string>>>(_observers, observer, observerId);
                }
            }
            else if (!_identifiedObservers.ContainsKey(observerId) && !_identifiedObservers.ContainsValue(observer))
            {
                _identifiedObservers.TryAdd(observerId, observer);
                return new Unsubscriber<string, Dictionary<Guid, IObserver<string>>>(_identifiedObservers, observer,
                    observerId);
            }

            throw new Exception("Could not add Observer");
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            return Subscribe(observer, Guid.Empty);
        }

        private async Task _establishConnection()
        {
            await _webSocket.ConnectAsync(_server_address, _tokenSource.Token);

            if (_webSocket.State != WebSocketState.Open)
            {
                throw new ConnectionFailedException(_server_address, _webSocket.State);
            }
        }

        private async Task _startReceiving()
        {
            if (Receiving) throw new InternalConnectionErrorException("Already receiving");
            var buffer = new byte[INITIAL_BUFFERSIZE];
            while (!_tokenSource.Token.IsCancellationRequested)
            {
                Receiving = true;
                int free = buffer.Length;
                int offset = 0;
                while (true)
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer, offset, free),
                        _tokenSource.Token);
                    offset += result.Count;
                    free -= result.Count;
                    if (result.EndOfMessage)
                    {
                        break;
                    }

                    if (free == 0)
                    {
                        var newSize = buffer.Length + BUFFERSIZE_INCREMENT;

                        var newBuffer = new byte[newSize];
                        Array.Copy(buffer, newBuffer, offset);
                        buffer = newBuffer;
                        free = buffer.Length - offset;
                    }
                }

                string response = Encoding.UTF8.GetString(buffer);
                buffer = new byte[INITIAL_BUFFERSIZE];
                var id = _extractId(response);
                var error = _errorHandler.ValidateJson(response);
                if (error is not null)
                {
                    if (id == Guid.Empty)
                    {
                        foreach (var observer in _observers)
                        {
                            observer.OnError(error);
                        }
                    }
                    else
                    {
                        _identifiedObservers[id].OnError(error);
                    }
                }
                else
                {
                    if (ParseNotification(response, out NotificationBase<object> notification))
                    {
                        HeartbeatNotification heartbeat = null;
                        try
                        {
                            heartbeat = (notification.@params as JObject).ToObject<HeartbeatNotification>();
                        }
                        catch (Exception e)
                        {
                         
                        }
                        if (heartbeat is null)
                        {
                            foreach (var observer in _notificationObservers)
                            {
                                observer.OnNext(response);
                            }
                        }
                        else
                        {
                            HandleHeartbeat(heartbeat);
                        }
                    }
                    else if (id == Guid.Empty)
                    {
                        foreach (var observer in _observers)
                        {
                            observer.OnNext(response);
                        }
                    }
                    else
                    {
                        if (_identifiedObservers.ContainsKey(id))
                        {
                            _identifiedObservers[id].OnNext(response);
                        }
                    }
                }
            }

            Receiving = false;
        }

        public bool ParseNotification(string json, out NotificationBase<object> notification)
        {
            try
            {
                notification = JsonConvert.DeserializeObject<NotificationBase<object>>(json);
                return true;
            }
            catch (Exception e)
            {
                notification = null;
                return false;
            }
        }

        public void HandleHeartbeat(HeartbeatNotification heartbeat)
        {
            if (heartbeat.type == "test_request")
            {
                var responseMessage = new TestMessage();
                SendMessage(responseMessage);
            }
        }

        public IDisposable SubscribeNotifications(IObserver<string> observer)
        {
            if (!_notificationObservers.Contains(observer))
            {
                _notificationObservers.Add(observer);
                return new Unsubscriber<string, List<IObserver<string>>>(_observers, observer, Guid.Empty);
            }

            throw new Exception("Could not add Observer");
        }

        private async Task _startSending()
        {
            while (!_tokenSource.Token.IsCancellationRequested)
            {
                if (_messages.Count == 0)
                {
                    Sending = false;
                    return;
                }

                var messageTuple = _messages.Dequeue();
                string rawMessage = IMessage.GetJson(messageTuple.Item1, messageTuple.Item2);
                ArraySegment<byte> message = Encoding.UTF8.GetBytes(rawMessage);
                Sending = true;
                await _webSocket.SendAsync(message, WebSocketMessageType.Text, true, _tokenSource.Token);
            }
        }

        private Guid _extractId(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, settings);
            if (jsonObj.ContainsKey("id"))
            {
                return new Guid(jsonObj["id"] as string);
            }

            return Guid.Empty;
        }

        public async Task<Guid> SendMessage(IMessage message)
        {
            Guid messageGuid = Guid.Empty;
            _messages.Enqueue(new Tuple<Guid, IMessage>(messageGuid, message));
            if (!Sending)
            {
#pragma warning disable 4014
                Task.Factory.StartNew(_startSending);
#pragma warning restore 4014
            }

            return messageGuid;
        }

        public async Task<Guid> SendMessage(IMessage message, Guid observerId)
        {
            _messages.Enqueue(new Tuple<Guid, IMessage>(observerId, message));
            if (!Sending)
            {
#pragma warning disable 4014
                Task.Factory.StartNew(_startSending);
#pragma warning restore 4014
            }

            return observerId;
        }
    }
}