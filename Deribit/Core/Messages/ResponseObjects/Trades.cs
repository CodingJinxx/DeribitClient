namespace Deribit.Core.Messages.ResponseObjects
{
    public class Trades
    {
        public float amount { get; set; }
        public string block_trade_id { get; set; }
        public string direction { get; set; }
        public float fee { get; set; }
        public string fee_currency { get; set; }
        public float index_price { get; set; }
        public string instrument_name { get; set; }
        public float iv { get; set; }
        public string label { get; set; }
        public string liquidation { get; set; }
        public string liquidity { get; set; }
        public float mark_price { get; set; }
        public string matching_id { get; set; }
        public string order_id { get; set; }
        public string order_type { get; set; }
        public string post_only { get; set; }
        public float price { get; set; }
        public float profit_loss { get; set; }
        public string reduce_only { get; set; }
        public bool self_trade { get; set; }
        public string state { get; set; }
        public int tick_direction { get; set; }
        public int timestamp { get; set; }
        public string trade_id { get; set; }
        public int trade_seq { get; set; }
        public float underlying_price { get; set; }
    }
}