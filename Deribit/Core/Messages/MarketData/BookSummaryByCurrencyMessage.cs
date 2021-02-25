using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Deribit.Core.Messages.MarketData
{
    public class BookSummaryByCurrencyMessage : IMessage
    {
        [JsonIgnore]
        public string MethodName { get => "/public/get_book_summary_by_currency"; }

        public BookSummaryByCurrencyMessage()
        {
            
        }
        public void CheckValidity()
        {
            // TODO Implement Check Validity 
        }

        public string currency { get; set; }
        public string kind { get; set; }
    }

    public class BookSummaryByCurrencyResponse : IResponse<BookSummaryByCurrencyResponse>
    {
        public float? ask_price { get; set; }
        public string base_currency { get; set; }
        public float? bid_price { get; set; }
        public long creation_timestamp { get; set; }
        public float? current_funding { get; set; }
        public float? estimated_delivery_price { get; set; }
        public float funding_8h { get; set; }
        public float high { get; set; }
        public string instrument_name { get; set; }
        public float interest_rate { get; set; }
        public float? last { get; set; }
        public float? low { get; set; }
        public float mark_price { get; set; }
        public float mid_price { get; set; }
        public float open_interest { get; set; }
        public float? price_change { get; set; }
        public string quote_currency { get; set; }
        public string? underlying_index { get; set; }
        public float? underlying_price { get; set; }
        public float volume { get; set; }
        public float? volume_usd { get; set; }
    }
}