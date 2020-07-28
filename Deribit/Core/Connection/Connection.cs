using System;
using System.Collections.Generic;
using System.Text;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using Deribit.Core.Authentication;
using Deribit.Core.Messages;

namespace Deribit.Core.Connection
{
    public class Connection : IObservable<string>
    {
        private const int INITIAL_BUFFERSIZE = 1024;
        private const int BUFFERSIZE_INCREMENT = 1024;

        public bool Connected { get; private set; }
        public bool Sending { get; private set; }
        public bool Receiving { get; private set; }

        private ClientWebSocket _webSocket;
        // ReSharper disable once NotAccessedField.Local
        private ICredentials _credentials;
        private List<IObserver<string>> _observers;
        private CancellationTokenSource _tokenSource;
        private Queue<IMessage> _messages;
        private Uri _server_address;

        public Connection(ICredentials credentials, Uri serverAddress, CancellationTokenSource tokenSource)
        {
            this._credentials = credentials;
            this._server_address = serverAddress;
            this._tokenSource = tokenSource;
            this._webSocket = new ClientWebSocket();
            this._observers = new List<IObserver<string>>();
            this._messages = new Queue<IMessage>();

            _establishConnection().Wait();
#pragma warning disable 4014
            _startReceiving();
#pragma warning restore 4014
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber<string>(_observers, observer);
        }

        private async Task _establishConnection()
        {
            await _webSocket.ConnectAsync(_server_address, _tokenSource.Token);

            if(_webSocket.State != WebSocketState.Open)
            {
                throw new ConnectionFailedException(_server_address, _webSocket.State);
            }

            Connected = true;
        }

        private async Task _startReceiving()
        {
            if(Receiving) throw new InternalConnectionErrorException("Already receiving");
            var buffer = new byte[INITIAL_BUFFERSIZE];
            while (!_tokenSource.Token.IsCancellationRequested)
            {
                Receiving = true;
                int free = buffer.Length;
                int offset = 0;
                while (true)
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer, offset, free), _tokenSource.Token);
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
                foreach(var observer in _observers)
                {
                    observer.OnNext(response);
                }
            }
            Receiving = false;
        }

        private async Task _startSending()
        {
            while (!_tokenSource.Token.IsCancellationRequested)
            {
                if(_messages.Count == 0)
                {
                    Sending = false;
                    return;
                }

                string rawMessage = _messages.Dequeue().GetJson();
                ArraySegment<byte> message = Encoding.UTF8.GetBytes(rawMessage);
                Sending = true;
                await _webSocket.SendAsync(message, WebSocketMessageType.Text, true, _tokenSource.Token);
            }
        }

        public async void SendMessage(IMessage message)
        {
            _messages.Enqueue(message);
            if (!Sending)
            {
                await _startSending();
            }
        }
    }
}
