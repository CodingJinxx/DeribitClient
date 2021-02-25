﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using Deribit.Core.Authentication;
using Deribit.Core.Messages;
using Deribit.Core.Validator;

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
        private Queue<Tuple<Guid, IMessage>> _messages; 
        private Uri _server_address;
        private IServerErrorHandler _errorHandler;

        public Connection(ICredentials credentials, Uri serverAddress, IServerErrorHandler handler, CancellationTokenSource tokenSource)
        {
            this._credentials = credentials;
            this._server_address = serverAddress;
            this._tokenSource = tokenSource;
            this._webSocket = new ClientWebSocket();
            this._observers = new List<IObserver<string>>();
            this._messages = new Queue<Tuple<Guid, IMessage>>();
            this._errorHandler = handler;

            _establishConnection().Wait();
            _startReceiving();
        }

        public Connection(ICredentials credentials, Uri serverAddress, CancellationTokenSource tokenSource) : this(
            credentials, serverAddress, new ServerErrorHandler(), tokenSource)
        {
            
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
                var error = _errorHandler.ValidateJson(response);
                if (error is not null)
                {
                    foreach (var observer in _observers)
                    {
                        observer.OnError(error);
                    }
                }
                else
                {
                    foreach(var observer in _observers)
                    {
                        observer.OnNext(response);
                    }
                }
                buffer = new byte[INITIAL_BUFFERSIZE];
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

                var messageTuple = _messages.Dequeue();
                string rawMessage = IMessage.GetJson(messageTuple.Item1, messageTuple.Item2);
                ArraySegment<byte> message = Encoding.UTF8.GetBytes(rawMessage);
                Sending = true;
                await _webSocket.SendAsync(message, WebSocketMessageType.Text, true, _tokenSource.Token);
            }
        }

        public async Task<Guid> SendMessage(IMessage message)
        {
            Guid messageGuid = Guid.NewGuid();
            _messages.Enqueue(new Tuple<Guid, IMessage>(messageGuid, message));
            if (!Sending)
            {
#pragma warning disable 4014
                Task.Factory.StartNew(_startSending);
#pragma warning restore 4014
            }

            return messageGuid;
        }
    }
}
