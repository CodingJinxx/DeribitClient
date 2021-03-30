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
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace DeribitTests.Integration
{
    public class TradeConnectionTest : BaseConnectionTest
    {
        public TradeConnectionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Buy()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(server_address, cancellationTokenSource, new TestServerErrorHandler(output));
            connection.Connect();


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
                amount = 40.0m,
                type = OrderType.Limit,
                price = 35000.0m,
                label = "market04022021"
            };
            connection.SendMessage(buyMessage);

            SpinWait.SpinUntil(() => myReceiver.Values.Count > 0);

            var buyResponse = IResponse<BuyResponse>.FromJson(myReceiver.Values.Dequeue());

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            output.WriteLine(JsonConvert.SerializeObject(buyResponse, settings));
        }

        [Fact]
        public void Sell()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(server_address, cancellationTokenSource, new TestServerErrorHandler(output));
            connection.Connect();


            Assert.True(connection.Connected);

            AuthMessage authMessage = new AuthMessage(credentials, GrantType.ClientCredentials);
            Receiver myReceiver = new Receiver();
            connection.Subscribe(myReceiver);
            connection.SendMessage(authMessage);

            SpinWait.SpinUntil(() => myReceiver.Values.Count > 0);

            var AuthResponse = IResponse<AuthenticationResponse>.FromJson(myReceiver.Values.Dequeue());

            SellMessage sellMessage = new SellMessage
            {
                instrument_name = "BTC-PERPETUAL",
                amount = 40.0m,
                type = OrderType.Limit,
                price = 35000.0m,
                label = "market04022021"
            };
            connection.SendMessage(sellMessage);

            SpinWait.SpinUntil(() => myReceiver.Values.Count > 0);

            var sellResponse = IResponse<SellResponse>.FromJson(myReceiver.Values.Dequeue());

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            output.WriteLine(JsonConvert.SerializeObject(sellResponse, settings));
        }

        [Fact]
        public void Edit()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(server_address, cancellationTokenSource, new TestServerErrorHandler(output));
            connection.Connect();


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
                amount = 30.0m,
                type = OrderType.Limit,
                price = 35000.0m,
                label = "market04022021"
            };
            connection.SendMessage(buyMessage);

            SpinWait.SpinUntil(() => myReceiver.Values.Count > 0);

            var buyResponse = IResponse<BuyResponse>.FromJson(myReceiver.Values.Dequeue());

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            output.WriteLine(JsonConvert.SerializeObject(buyResponse, settings));

            EditMessage editMessage = new EditMessage
            {
                order_id = buyResponse.result.order.order_id,
                amount = 10.0m,
            };

            connection.SendMessage(editMessage);

            SpinWait.SpinUntil(() => myReceiver.Values.Count > 0);

            var editResponse = IResponse<EditResponse>.FromJson(myReceiver.Values.Dequeue());
            output.WriteLine(JsonConvert.SerializeObject(editResponse, settings));
        }

        [Fact]
        public void Cancel()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Connection connection = new Connection(server_address, cancellationTokenSource, new TestServerErrorHandler(output));
            connection.Connect();


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
                amount = 40.0m,
                type = OrderType.Limit,
                price = 35000.0m,
                label = "market04022021"
            };
            connection.SendMessage(buyMessage);

            SpinWait.SpinUntil(() => myReceiver.Values.Count > 0);

            var buyResponse = IResponse<BuyResponse>.FromJson(myReceiver.Values.Dequeue());

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            output.WriteLine(JsonConvert.SerializeObject(buyResponse, settings));

            var cancelRequest = new CancelMessage
            {
                order_id = buyResponse.result.order.order_id
            };

            connection.SendMessage(cancelRequest);

            SpinWait.SpinUntil(() => myReceiver.Values.Count > 0);

            var cancelResponse = IResponse<CancelResponse>.FromJson(myReceiver.Values.Dequeue());
            output.WriteLine(JsonConvert.SerializeObject(cancelResponse, settings));
        }
    }
}