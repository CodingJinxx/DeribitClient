using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeribitClient.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using DeribitClient.Core;


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

        public bool Connected
        {
            get
            {
                if (this._webSocket is null) return false ;
                return this._webSocket.State == WebSocketState.Open;
            }
        }

        private bool _connectionState;
        public event EventHandler<bool> OnConnectionChanged;

        public ChanneledConnection(IOptions<ConnectionSettings> settings)
        {
            this._settings = settings.Value;
            this._connectionState = false;
        }
        private void MonitorConnection()
        {
            if (this._connectionState != this.Connected)
            {
                this._connectionState = this.Connected;
                this.OnConnectionChanged?.Invoke(this, this._connectionState);
            }
        }

        public async Task Connect()
        {
            this._webSocket = new ClientWebSocket();
            this._tokenSource = new CancellationTokenSource();
            this.Incoming = new XChannel<string>();
            this.Outgoing = new XChannel<string>();
            await this._webSocket.ConnectAsync(new Uri(this._settings.ServerAddress), this._tokenSource.Token);
            this._incomingWebsocketConsumer(this._tokenSource.Token);
            this._outgoingChannelConsumer(this._tokenSource.Token);
            this.MonitorConnection();
        }

        public async Task Disconnect()
        {
            this.Incoming.CancelChannel();
            this.Outgoing.CancelChannel();

            await this._webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed connection", this._tokenSource.Token);
            this._tokenSource.Cancel();
            this._webSocket.Dispose();
            this.MonitorConnection();
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
                    this.MonitorConnection();
                    if (this.Connected)
                    {
                        var buffer = new ArraySegment<byte>(new byte[2048]);
                        string response = "";
                        do
                        {
                            WebSocketReceiveResult result;
                            using (var ms = new MemoryStream())
                            {
                                do
                                {
                                    result = await this._webSocket.ReceiveAsync(buffer, CancellationToken.None);
                                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                                } while (!result.EndOfMessage);

                                if (result.MessageType == WebSocketMessageType.Close)
                                    break;

                                ms.Seek(0, SeekOrigin.Begin);
                                using (var reader = new StreamReader(ms, Encoding.UTF8))
                                {
                                    response = await reader.ReadToEndAsync();
                                    break;
                                }
                            }
                        } while (true);

                        if (response == "")
                        {
                            throw new Exception("Empty Response Received");
                        }

                        this.Incoming.Write(response);
                    }
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
                    this.MonitorConnection();
                    if (this.Connected)
                    {
                        var message = await this.Outgoing.Read();
                        ArraySegment<byte> encodedMessage = Encoding.UTF8.GetBytes(message);
                        this._webSocket.SendAsync(encodedMessage, WebSocketMessageType.Text, true, token);
                    }
                }
            }
        }

       
    }
}