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
        public decimal? ask_price { get; set; }
        public string base_currency { get; set; }
        public decimal? bid_price { get; set; }
        public long creation_timestamp { get; set; }
        public decimal? current_funding { get; set; }
        public decimal? estimated_delivery_price { get; set; }
        public decimal funding_8h { get; set; }
        public decimal high { get; set; }
        public string instrument_name { get; set; }
        public decimal interest_rate { get; set; }
        public decimal? last { get; set; }
        public decimal? low { get; set; }
        public decimal mark_price { get; set; }
        public decimal mid_price { get; set; }
        public decimal open_interest { get; set; }
        public decimal? price_change { get; set; }
        public string quote_currency { get; set; }
        public string? underlying_index { get; set; }
        public decimal? underlying_price { get; set; }
        public decimal volume { get; set; }
        public decimal? volume_usd { get; set; }
    }
}