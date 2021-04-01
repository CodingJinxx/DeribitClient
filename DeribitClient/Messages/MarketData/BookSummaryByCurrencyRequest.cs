using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DeribitClient.Messages.MarketData
{
    [MethodName("/public/get_book_summary_by_currency")]
    public class BookSummaryByCurrencyRequest : IRequest
    {
        public string currency { get; set; }
        public string kind { get; set; }
    }
}