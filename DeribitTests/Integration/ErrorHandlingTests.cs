using System;
using System.Threading;
using Deribit.Core;
using Deribit.Core.Authentication;
using Deribit.Core.Connection;
using Deribit.Core.Messages.Authentication;
using Deribit.Core.Messages.Trading;
using Deribit.Core.Types;
using Deribit.Core.Validator;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Xunit;
using Xunit.Abstractions;

namespace DeribitTests.Integration
{
    public class ErrorHandlingTests : BaseConnectionTest
    {
        public ErrorHandlingTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void BadRequestThrowsError()
        {
            Connection connection = new Connection(credentials, server_address, new CancellationTokenSource(), new TestServerErrorHandler(output));
            Assert.True(connection.Connected);

            AuthMessage message = new AuthMessage(new Credentials(clientId: "12dsad", clientSecret: "dmjaiosda"),
                GrantType.ClientCredentials);

            Receiver receiver = new Receiver();
            connection.Subscribe(receiver);
            connection.SendMessage(message);

            SpinWait.SpinUntil(() => receiver.ErrorOcurred, 1000);
            Assert.True(receiver.ErrorOcurred);
            output.WriteLine(receiver.Error);
        }
    }
}