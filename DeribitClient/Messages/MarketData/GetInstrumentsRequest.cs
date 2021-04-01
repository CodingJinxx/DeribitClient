using System.Text.Json.Serialization;

namespace DeribitClient.Messages.MarketData
{
    [MethodName("/public/get_instruments")]
    public class GetInstrumentsRequest : IRequest
    {
        public string currency { get; set; }
        public string? kind { get; set; }
        public bool? expired { get; set; }
    }
}