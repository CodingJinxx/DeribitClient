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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;


namespace DeribitTests.Integration
{
    public class TradeTests
    {
        private Credentials credentials;
        private Uri server_address;
        private readonly ITestOutputHelper output;
        
        public TradeTests(ITestOutputHelper output)
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
        public void Buy()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(credentials, server_address, cancellationTokenSource);
            
            Assert.True(connection.Connected);
            
            AuthMessage authMessage = new AuthMessage(credentials, GrantType.ClientCredentials);
            Receiver myReceiver = new Receiver();
            connection.Subscribe(myReceiver);
            connection.SendMessage(authMessage);

            SpinWait.SpinUntil(() => myReceiver.Values.Count > 0);

            var AuthResponse = IResponse<AuthenticationResponse>.FromJson(myReceiver.Values.Dequeue());

            BuyMessage buyMessage = new BuyMessage
            {
                
                instrument_name = "BTC-PERPETUAL",
                amount = 40.0f,
                type = OrderType.Limit,
                price = 35000.0f,
                label = "market04022021"
            };
            connection.SendMessage(buyMessage);
            
            SpinWait.SpinUntil(() => myReceiver.Values.Count > 0);

            var buyResponse = IResponse<BuyResponse>.FromJson(myReceiver.Values.Dequeue());
            
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            output.WriteLine(JsonConvert.SerializeObject(buyResponse, settings));
        }
    }
}