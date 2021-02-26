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
        public override ServerSideException ValidateJson(string json)
        {
            var exception = base.ValidateJson(json);
            output.WriteLine(exception.Message);
            return exception;
        }
    }
}