using System.Threading;
using Deribit.Core;
using Deribit.Core.Connection;
using Deribit.Core.Messages;
using Deribit.Core.Messages.Authentication;
using Deribit.Core.Messages.Trading;
using Deribit.Core.Types;
using Deribit.Core.Validator;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DeribitTests.Integration
{
    [Collection("Integration")]
    public class IdentifierTests : BaseConnectionTest
    {
        public IdentifierTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TwoReceiverSeperateMessages()
        {
            Receiver receiverOne = new Receiver();
            Receiver receiverTwo = new Receiver();

            Connection connection = new Connection(server_address,new CancellationTokenSource(), new TestServerErrorHandler(output));
            connection.Connect();

            Assert.True(connection.Connected);

            connection.Subscribe(receiverOne);
            connection.Subscribe(receiverOne, receiverOne.Id);
            connection.Subscribe(receiverTwo, receiverTwo.Id);
            
            connection.SendMessage(new AuthMessage(credentials, GrantType.ClientCredentials));

            SpinWait.SpinUntil(() => receiverOne.Values.Count > 0, 10000);
            var authResponse = IResponse<AuthenticationResponse>.FromJson(receiverOne.Values.Dequeue());


            BuyMessage buyMessage = new BuyMessage()
            {
                instrument_name = "BTC-PERPETUAL",
                amount = 1000.0m,
                price = 20000.0m
            };

            SellMessage sellMessage = new SellMessage()
            {
                instrument_name = "ETH-PERPETUAL",
                amount = 500.0m,
                price = 1000.0m
            };

            
            connection.SendMessage(buyMessage, receiverOne.Id);
            SpinWait.SpinUntil(() => receiverOne.Values.Count > 0, 10000);
            Assert.True(receiverOne.Values.Count == 1);
            Assert.True(receiverTwo.Values.Count == 0);
            var responseOne = IResponse<BuyResponse>.FromJson(receiverOne.Values.Dequeue());
            Assert.True(responseOne.result.order.amount == 1000.0m);
            Assert.True(responseOne.result.order.instrument_name == "BTC-PERPETUAL");
            
            connection.SendMessage(sellMessage, receiverTwo.Id);
            SpinWait.SpinUntil(() => receiverTwo.Values.Count > 0, 10000);
            Assert.True(receiverOne.Values.Count == 0);
            Assert.True(receiverTwo.Values.Count == 1);
            var responseTwo = IResponse<BuyResponse>.FromJson(receiverTwo.Values.Dequeue());
            Assert.True(responseTwo.result.order.amount == 500.0m);
            Assert.True(responseTwo.result.order.instrument_name == "ETH-PERPETUAL");
            connection.SendMessage(new LogoutMessage());
        }
    }
}