using System.Text.Json.Serialization;

namespace DeribitClient.Messages.MarketData
{
    [MethodName("/public/get_instrument")]
    public class GetInstrumentRequest : IRequest
    {
        public string instrument_name { get; set; }
    }
}