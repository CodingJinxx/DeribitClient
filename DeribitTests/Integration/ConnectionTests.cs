using System;
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
using Deribit.Core.Messages.Trading;
using Deribit.Core.Types;
using Deribit.Core.Validator;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;


namespace DeribitTests.Integration 
{
    [Collection("Integration")]
    public class ConnectionTests : BaseConnectionTest
    {
        public ConnectionTests(ITestOutputHelper output) : base(output)
        {
          
        }

        [Fact]
        public void EstablishConnection()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(server_address, cancellationTokenSource, new TestServerErrorHandler(output));

            Assert.True(connection.Connected);
        }
        
        [Fact]
        public void Authenticate()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(server_address, cancellationTokenSource, new TestServerErrorHandler(output));

            Assert.True(connection.Connected);

            AuthMessage authMessage = new AuthMessage(credentials, GrantType.ClientCredentials);
            Receiver myReceiver = new Receiver();
            connection.Subscribe(myReceiver);
            connection.SendMessage(authMessage);

            SpinWait.SpinUntil(() => myReceiver.Received, 1000);

            var unused = IResponse<AuthenticationResponse>.FromJson(myReceiver.Values.Dequeue());
            connection.SendMessage(new LogoutMessage(unused.result.access_token));
        }
        
        [Fact]
        public async void Logout()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(server_address, cancellationTokenSource, new TestServerErrorHandler(output));

            Assert.True(connection.Connected);

            AuthMessage authMessage = new AuthMessage(credentials, GrantType.ClientCredentials);
            Receiver myReceiver = new Receiver();
            connection.Subscribe(myReceiver);
            connection.SendMessage(authMessage);

            SpinWait.SpinUntil(() => myReceiver.Values.Count > 0);

            var authResponse = IResponse<AuthenticationResponse>.FromJson(myReceiver.Values.Dequeue());
            
            LogoutMessage logoutMessage = new LogoutMessage(authResponse.result.access_token);
            connection.SendMessage(logoutMessage);
            SpinWait.SpinUntil(() => !connection.Connected, 10000);
            Assert.False(connection.Connected);
        }
    }
}