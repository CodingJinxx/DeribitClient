using System.Threading;
using System.Threading.Tasks;
using Deribit.Core;
using Deribit.Core.Connection;
using Deribit.Core.Messages.SessionManagement;
using Xunit;
using Xunit.Abstractions;

namespace DeribitTests.Integration
{
    [Collection("Integration")]
    public class SessionManagementTests : BaseConnectionTest
    {
        public SessionManagementTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async void HeartbeatTest()
        {
            Connection connection = new Connection(server_address, new CancellationTokenSource(), new TestServerErrorHandler(output));
            Receiver normalReceiver = new Receiver();
            Receiver notificationReceiver = new Receiver();
            connection.Subscribe(normalReceiver);
            connection.SubscribeNotifications(notificationReceiver);
            connection.Connect();
            Assert.True(connection.Connected);

            var setHeartbeat = new SetHeartbeatMessage()
            {
                interval = 10
            };
            connection.SendMessage(setHeartbeat);

            ManualResetEvent resetEvent = new ManualResetEvent(false);
            resetEvent.WaitOne(60000);
            
            Assert.True(connection.Connected);
            Assert.True(notificationReceiver.Received == false);
        }
    }
}