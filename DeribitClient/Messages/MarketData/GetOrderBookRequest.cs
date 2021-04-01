using System.Text.Json.Serialization;
using DeribitClient.DeribitTypes;

namespace DeribitClient.Messages.MarketData
{
    [MethodName("/public/get_order_book")]
    public class GetOrderBookRequest : IRequest
    {
        public string instrument_name { get; set; }
        public int depth { get; set; }
    }
}