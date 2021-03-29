using System.Threading;
using Deribit.Core;
using Deribit.Core.Connection;
using Deribit.Core.Messages;
using Deribit.Core.Messages.MarketData;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace DeribitTests.Integration
{
    public class MarketDataConnectionTest : BaseConnectionTest
    {
        public MarketDataConnectionTest(ITestOutputHelper output) : base(output)
        {
            
        }

        [Fact]
        public void BookSummaryDataTests()
        {
            Connection connection = new Connection(this.credentials, this.server_address, new CancellationTokenSource());
            Assert.True(connection.Connected);

            BookSummaryByCurrencyMessage message = new BookSummaryByCurrencyMessage
            {
                currency = "BTC",
                kind = "future"
            };

            Receiver receiver = new Receiver();
            connection.Subscribe(receiver);
            connection.SendMessage(message);

            SpinWait.SpinUntil(() => receiver.Received == true);

            var response = IResponse<BookSummaryByCurrencyResponse[]>.FromJson(receiver.Values.Dequeue());
            Assert.True(response.result[0].ask_price != null);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            output.WriteLine(JsonConvert.SerializeObject(response, settings));
        }

        [Fact]
        public void OrderbookDataTest()
        {
            Connection connection = new Connection(this.credentials, this.server_address, new CancellationTokenSource());
            Assert.True(connection.Connected);

            var orderbookMessage = new GetOrderBookMessage()
            {
                depth = 5,
                instrument_name = "BTC-PERPETUAL"
            };

            Receiver receiver = new Receiver();
            connection.Subscribe(receiver);
            connection.SendMessage(orderbookMessage);
            
            SpinWait.SpinUntil(() => receiver.Received == true);

            var response = IResponse<GetOrderBookResponse>.FromJson(receiver.Values.Dequeue());
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            output.WriteLine(JsonConvert.SerializeObject(response, settings));
        }
    }
}