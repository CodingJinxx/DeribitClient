using System;
using System.IO;
using System.Threading;
using Deribit.Core.Authentication;
using Deribit.Core.Messages;
using Deribit.Core.Connection;
using Deribit.Core.Types;
using Microsoft.Extensions.Configuration;
using Xunit;


namespace DeribitTests.Core
{
    public class ConnectionTests
    {
        private Credentials credentials;
        private Uri server_address;
        
        public ConnectionTests()
        {
            var isRunningInsideAction = Environment.GetEnvironmentVariable("CI") == "true";

            IConfigurationRoot config = null;
            string clientId, clientSecret;
            if (isRunningInsideAction)
            {
                clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
                clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
            }
            else
            {
                string basePath = Directory.GetCurrentDirectory();
                IConfigurationBuilder builder = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("testsettings.json", false)
                    .AddJsonFile("usersettings.json", false);

                config = builder.Build();

                clientId = config.GetSection("UserSettings").GetSection("Client_Id").Value;
                clientSecret = config.GetSection("UserSettings").GetSection("Client_Secret").Value;
            }

            if (config != null)
                this.server_address = new Uri(config.GetSection("ApiSettings").GetSection("Server_URL").Value);
            this.credentials = new Credentials(clientId, clientSecret);
        }

        [Fact]
        public void EstablishConnection()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(credentials, server_address, cancellationTokenSource);

            Assert.True(connection.Connected);
        }
        [Fact]
        public void Authenticate()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(credentials, server_address, cancellationTokenSource);

            Assert.True(connection.Connected);

            AuthenticationMessage authMessage = new AuthenticationMessage(credentials, GrantType.ClientCredentials);
            Receiver myReceiver = new Receiver();
            connection.Subscribe(myReceiver);
            connection.SendMessage(authMessage);

            SpinWait.SpinUntil(() => myReceiver.Received);

            var unused = IResponse<AuthenticationResponse>.FromJson(myReceiver.Value);
        }

        private class Receiver : IObserver<string>
        {
            public string Value;
            // ReSharper disable once NotAccessedField.Local
            public bool Done;
            public bool Received;
            // ReSharper disable once NotAccessedField.Local
            public string Error;

            public Receiver()
            {
                Value = "";
                Done = false;
                Error = "";
                Received = false;
            }
            public void OnCompleted()
            {
                Done = true;
            }

            public void OnError(Exception error)
            {
                this.Error = error.Message;
            }

            public void OnNext(string value)
            {
                this.Value = value;
                this.Received = true;
            }
        }
    }
}