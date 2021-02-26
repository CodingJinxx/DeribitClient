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

            Connection connection = new Connection(credentials, server_address,new CancellationTokenSource(), new TestServerErrorHandler(output));
            Assert.True(connection.Connected);

            connection.Subscribe(receiverOne, receiverOne.Id);
            connection.Subscribe(receiverTwo, receiverTwo.Id);
            
            connection.SendMessage(new AuthMessage(credentials, GrantType.ClientCredentials));


            BuyMessage buyMessage = new BuyMessage()
            {
                instrument_name = "BTC-PERPETUAL",
                amount = 80.0f
            };

            SellMessage sellMessage = new SellMessage()
            {
                instrument_name = "ETH-PERPETUAL",
                amount = 40.0f
            };

            
            connection.SendMessage(buyMessage, receiverOne.Id);
            SpinWait.SpinUntil(() => receiverOne.Values.Count > 0, 10000);
            Assert.True(receiverOne.Values.Count == 1);
            Assert.True(receiverTwo.Values.Count == 0);
            var responseOne = IResponse<BuyResponse>.FromJson(receiverOne.Values.Dequeue());
            Assert.True(responseOne.result.order.amount == 80.0f);
            Assert.True(responseOne.result.order.instrument_name == "BTC-PERPETUAL");
            
            connection.SendMessage(sellMessage, receiverTwo.Id);
            SpinWait.SpinUntil(() => receiverTwo.Values.Count > 0, 10000);
            Assert.True(receiverOne.Values.Count == 0);
            Assert.True(receiverTwo.Values.Count == 1);
            var responseTwo = IResponse<BuyResponse>.FromJson(receiverOne.Values.Dequeue());
            Assert.True(responseTwo.result.order.amount == 40.0f);
            Assert.True(responseTwo.result.order.instrument_name == "ETH-PERPETUAL");

        }
    }
}