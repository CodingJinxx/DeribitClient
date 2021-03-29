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
        public float? ask_iv { get; set; }
        public float[][] asks { get; set; }
        public float best_ask_amount { get; set; }
        public float? best_ask_price { get; set; }
        public float best_bid_amount { get; set; }
        public float? best_bid_price { get; set; }
        public float? bid_iv { get; set; }
        public float[][] bids { get; set; }
        public float? current_funding { get; set; }
        public float? delivery_price { get; set; }
        public float? funding_8h { get; set; }
        public Greeks? greeks { get; set; }
        public float index_price { get; set; }
        public string instrument_name { get; set; }
        public float? interest_rate { get; set; }
        public float last_price { get; set; }
        public float? mark_iv { get; set; }
        public float mark_price { get; set; }
        public float max_price { get; set; }
        public float min_price { get; set; }
        public float open_price { get; set; }
        public float? settlement_price { get; set; }
        public string state { get; set; }
        public Stats stats { get; set; }
        public long timestamp { get; set; }
        public float? underlying_index { get; set; }
        public float? underlying_price { get; set; }
        public long change_id { get; set; }
    }
}