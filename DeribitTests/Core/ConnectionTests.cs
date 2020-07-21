using System;
using System.Threading;
using System.Threading.Tasks;
using Deribit.Core;
using Deribit.Core.Authentication;
using Deribit.Core.Messages;
using Deribit.Core.Connection;
using Deribit.Core.Types;
using DeribitTests.Core.Configuration;
using Microsoft.Extensions.Configuration;

using Xunit;
using Xunit.Sdk;


namespace DeribitTests.Core
{
    public class ConnectionTests
    {
        private Startup settings;
        private Credentials credentials;

        public ConnectionTests()
        {
            this.settings = Startup.GetInstance();
            string client_id = settings.Configuration.GetSection("UserSettings").GetSection("Client_Id").Value;
            string client_secret = settings.Configuration.GetSection("UserSettings").GetSection("Client_Secret").Value;
            Uri server_url = new Uri(settings.Configuration.GetSection("ApiSettings").GetSection("Server_URL").Value);
            this.credentials = new Credentials(client_id, client_secret, server_url);
        }

        [Fact]
        public void EstablishConnection()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(credentials, cancellationTokenSource);
            
            Assert.True(connection.Connected);
        }
        [Fact]
        public void Authenticate()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(credentials, cancellationTokenSource);

            Assert.True(connection.Connected);

            AuthenticationMessage authMessage = new AuthenticationMessage();
            authMessage.client_id = credentials.ClientID;
            authMessage.client_secret = credentials.ClientSecret;
            authMessage.grant_type = GrantType.ClientCredentials;
            Reciever myReciever = new Reciever();
            connection.Subscribe(myReciever);
            connection.SendMessage(authMessage);

            while (!myReciever.recieved)
            {
                
            }

            var result = AuthenticationResponse.FromJson(myReciever.value);
        }

        private class Reciever : IObserver<string>
        {
            public string value;
            public bool done;
            public bool recieved;
            public string error;

            public Reciever()
            {
                value = "";
                done = false;
                error = "";
                recieved = false;
            }
            public void OnCompleted()
            {
                done = true;
            }

            public void OnError(Exception error)
            {
                this.error = error.Message;
            }

            public void OnNext(string value)
            {
                this.value = value;
                this.recieved = true;
            }
        }
    }
}