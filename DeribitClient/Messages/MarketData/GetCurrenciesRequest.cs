using System.Text.Json.Serialization;
using DeribitClient.DeribitTypes;

namespace DeribitClient.Messages.MarketData
{
    [MethodName("/public/get_currencies")]
    public class GetCurrenciesRequest : IRequest
    {
    }
}