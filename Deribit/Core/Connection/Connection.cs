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
        private ICredentials _credentials;
        private List<IObserver<string>> _observers;
        private CancellationTokenSource _tokenSource;
        private Queue<IMessage> _messages;

        public Connection(ICredentials credentials, CancellationTokenSource tokenSource)
        {
            this._credentials = credentials;
            this._tokenSource = tokenSource;
            _webSocket = new ClientWebSocket();
            _observers = new List<IObserver<string>>();
            _messages = new Queue<IMessage>();

            _establishConnection().Wait();
            _startReceiving();
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
            await _webSocket.ConnectAsync(_credentials.ServerURL, _tokenSource.Token);

            if(_webSocket.State != WebSocketState.Open)
            {
                throw new ConnectionFailedException(_credentials.ServerURL, _webSocket.State);
            }

            Connected = true;
        }

        private async Task _startReceiving()
        {
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

                ArraySegment<byte> message = Encoding.UTF8.GetBytes(_messages.Dequeue().GetJson());
                Sending = true;
                await _webSocket.SendAsync(message, WebSocketMessageType.Text, true, _tokenSource.Token);
            }
        }

        public void SendMessage(IMessage message)
        {
            _messages.Enqueue(message);
            if (!Sending)
            {
                _startSending();
            }
        }
    }
}
