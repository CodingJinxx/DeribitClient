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
    public class MarketDataTests : BaseTests
    {
        public MarketDataTests(ITestOutputHelper output) : base(output)
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
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            output.WriteLine(JsonConvert.SerializeObject(response, settings));
        }
    }
}