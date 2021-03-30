using System.Collections.Immutable;

namespace Deribit.Core.Messages.ResponseObjects
{
    public class Order
    {
        public string order_state { get; set; }
        public decimal max_show { get; set; }
        public bool api { get; set; }
        public decimal amount { get; set; }
        public bool web { get; set; }
        public string instrument_name { get; set; }
        public string advanced { get; set; }
        public bool triggered { get; set; }
        public bool block_trade { get; set; }
        public string original_order_type { get; set; }
        public decimal price { get; set; }
        public string time_in_force { get; set; }
        public bool auto_replaced { get; set; }
        public string stop_order_id { get; set; }
        public long last_update_timestamp { get; set; }
        public bool post_only { get; set; }
        public bool replaced { get; set; }
        public decimal filled_number { get; set; }
        public decimal average_price { get; set; }
        public string order_id { get; set; }
        public bool reduce_only { get; set; }
        public decimal commission { get; set; }
        public string app_name { get; set; }
        public decimal stop_price { get; set; }
        public string label { get; set; }
        public int creation_stamp { get; set; }
        public string direction { get; set; }
        public bool is_liquidation { get; set; }
        public string order_type { get; set; }
        public decimal usd { get; set; }
        public decimal profit_loss { get; set; }
        public decimal implv { get; set; }
        public string trigger { get; set; }
    }
}