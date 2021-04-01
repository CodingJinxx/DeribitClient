using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeribitClient.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace DeribitClient
{
    public class ChanneledConnection
    {
        private ClientWebSocket _webSocket;
        private ConnectionSettings _settings;
        private CancellationTokenSource _tokenSource;
        
        // From Server to Connection
        public XChannel<string> Incoming { get; private set; }
        // From Connection to Server
        public XChannel<string> Outgoing { get; private set; }
        public bool Connected { get => _webSocket.State == WebSocketState.Open; }

        public ChanneledConnection(IOptions<ConnectionSettings> settings)
        {
            this._settings = settings.Value;
            this._webSocket = new ClientWebSocket();
            this._tokenSource = new CancellationTokenSource();
        }

        public async Task Connect()
        {
            this.Incoming = new XChannel<string>();
            this.Outgoing = new XChannel<string>();
            await _webSocket.ConnectAsync(new Uri(this._settings.ServerAddress), this._tokenSource.Token);
            this._incomingWebsocketConsumer(this._tokenSource.Token);
            this._outgoingChannelConsumer(this._tokenSource.Token);
        }

        public async Task Disconnect()
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed connection", this._tokenSource.Token);
            this._tokenSource.Cancel();
            this.Incoming.CancelChannel();
            this.Outgoing.CancelChannel();
        }

        public void UpdateConnectionSettings(ConnectionSettings settings)
        {
            this._settings = settings;
        }

        private void _incomingWebsocketConsumer(CancellationToken token)
        {
            var factory = new TaskFactory(token);
            factory.StartNew(consumer, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            async void consumer()
            {
                while (!token.IsCancellationRequested)
                {
                    var buffer = new ArraySegment<byte>(new byte[1024]);
                    var result = await this._webSocket.ReceiveAsync(buffer, token);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await this.Disconnect();
                    }
                    string response = Encoding.UTF8.GetString(buffer);
                    Incoming.Write(response);
                }
            }
        }
        
        private void _outgoingChannelConsumer(CancellationToken token)
        {
            var factory = new TaskFactory(token);
            factory.StartNew(consumer, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                
            async void consumer()
            {
                while (!token.IsCancellationRequested)
                {
                    var message = await this.Outgoing.Read();
                    ArraySegment<byte> encodedMessage = Encoding.UTF8.GetBytes(message);
                    this._webSocket.SendAsync(encodedMessage, WebSocketMessageType.Text, true, token);
                }
            }
        }
    }
}