﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Threading;
using Deribit.Core;
using Deribit.Core.Authentication;
using Deribit.Core.Configuration;
using Deribit.Core.Messages.Authentication;
using Deribit.Core.Messages;
using Deribit.Core.Connection;
using Deribit.Core.Types;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;


namespace DeribitTests.Core
{
    public class ConnectionTests
    {
        private Credentials credentials;
        private Uri server_address;
        private readonly ITestOutputHelper output;
        
        public ConnectionTests(ITestOutputHelper output)
        {
            this.output = output;
            var isRunningInsideAction = Environment.GetEnvironmentVariable("CI") == "true";

            IConfigurationRoot config = null;
            string clientId, clientSecret;
            if (isRunningInsideAction)
            {
                clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
                clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
                server_address = new Uri(Environment.GetEnvironmentVariable("SERVER_ADDRESS"));
                ApiSettings.JsonRpc = Environment.GetEnvironmentVariable("JSON_RPC");
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
                ApiSettings.JsonRpc = config.GetSection("ApiSettings").GetSection("JSON_RPC").Value;
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

            AuthMessage authMessage = new AuthMessage(credentials, GrantType.ClientCredentials);
            Receiver myReceiver = new Receiver();
            connection.Subscribe(myReceiver);
            connection.SendMessage(authMessage);

            SpinWait.SpinUntil(() => myReceiver.Received);

            var unused = IResponse<AuthenticationResponse>.FromJson(myReceiver.Values.Dequeue());
        }

        [Fact]
        public async void MessageDifferentiation()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(credentials, server_address, cancellationTokenSource);

            Assert.True(connection.Connected);

            AuthMessage authMessage = new AuthMessage(credentials, GrantType.ClientCredentials);
            Receiver myReceiver = new Receiver();
            connection.Subscribe(myReceiver);

            Guid id1 = await connection.SendMessage(authMessage);
            Guid id2 = await connection.SendMessage(authMessage);

            SpinWait.SpinUntil(() => myReceiver.Values.Count == 2);

            var message1 = IResponse<AuthenticationResponse>.FromJson(myReceiver.Values.Dequeue());
            var message2 = IResponse<AuthenticationResponse>.FromJson(myReceiver.Values.Dequeue());

            Assert.True((message1.id == id1.ToString() || message1.id == id2.ToString()) && (message2.id == id1.ToString() || message2.id == id2.ToString()));
        }

        [Fact]
        public async void Logout()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(credentials, server_address, cancellationTokenSource);

            Assert.True(connection.Connected);

            AuthMessage authMessage = new AuthMessage(credentials, GrantType.ClientCredentials);
            Receiver myReceiver = new Receiver();
            connection.Subscribe(myReceiver);
            connection.SendMessage(authMessage);

            SpinWait.SpinUntil(() => myReceiver.Values.Count > 0);

            var authResponse = IResponse<AuthenticationResponse>.FromJson(myReceiver.Values.Dequeue());
            
            LogoutMessage logoutMessage = new LogoutMessage(authResponse.result.access_token);
            connection.SendMessage(logoutMessage);
            
            SpinWait.SpinUntil(() => myReceiver.Values.Count > 0);
            var logoutResponse = IResponse<EmptyResponse>.FromJson(myReceiver.Values.Dequeue());
        }
    }
}