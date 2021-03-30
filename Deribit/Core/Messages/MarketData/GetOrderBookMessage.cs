using System.Text.Json.Serialization;
using Deribit.Core.Messages.ResponseObjects;

namespace Deribit.Core.Messages.MarketData
{
    public class GetOrderBookMessage : IMessage
    {
        [JsonIgnore]
        public string MethodName { get => "/public/get_order_book"; }

        public string instrument_name { get; set; }
        public int depth { get; set; }
        public void CheckValidity()
        {
            // TODO Check Validation
        }
    }

    public class GetOrderBookResponse : IResponse<GetOrderBookResponse>
    {
        public decimal? ask_iv { get; set; }
        public decimal[][] asks { get; set; }
        public decimal best_ask_amount { get; set; }
        public decimal? best_ask_price { get; set; }
        public decimal best_bid_amount { get; set; }
        public decimal? best_bid_price { get; set; }
        public decimal? bid_iv { get; set; }
        public decimal[][] bids { get; set; }
        public decimal? current_funding { get; set; }
        public decimal? delivery_price { get; set; }
        public decimal? funding_8h { get; set; }
        public Greeks? greeks { get; set; }
        public decimal index_price { get; set; }
        public string instrument_name { get; set; }
        public decimal? interest_rate { get; set; }
        public decimal last_price { get; set; }
        public decimal? mark_iv { get; set; }
        public decimal mark_price { get; set; }
        public decimal max_price { get; set; }
        public decimal min_price { get; set; }
        public decimal open_price { get; set; }
        public decimal? settlement_price { get; set; }
        public string state { get; set; }
        public Stats stats { get; set; }
        public long timestamp { get; set; }
        public decimal? underlying_index { get; set; }
        public decimal? underlying_price { get; set; }
        public long change_id { get; set; }
    }
}