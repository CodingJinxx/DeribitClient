using Deribit.Core.Validator;
using Xunit.Abstractions;

namespace DeribitTests
{
    public class TestServerErrorHandler : ServerErrorHandler
    {
        private ITestOutputHelper output;
        
        public TestServerErrorHandler(ITestOutputHelper output)
        {
            this.output = output;
        }
        public override ServerSideException? ValidateJson(string json)
        {
            ServerSideException? exception = base.ValidateJson(json);
            if (exception is not null)
            {
                output.WriteLine(exception?.Message);
            }
            return exception;
        }
    }
}