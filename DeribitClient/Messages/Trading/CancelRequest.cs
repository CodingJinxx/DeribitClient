using System.Text.Json.Serialization;

namespace DeribitClient.Messages.Trading
{
    [MethodName("/private/cancel")]
    public class CancelRequest : IRequest
    {
        public string order_id { get; set; }
    }
}