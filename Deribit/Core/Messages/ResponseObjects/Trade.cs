namespace Deribit.Core.Messages.ResponseObjects
{
    public class Trade
    {
        public decimal amount { get; set; }
        public string block_trade_id { get; set; }
        public string direction { get; set; }
        public decimal fee { get; set; }
        public string fee_currency { get; set; }
        public decimal index_price { get; set; }
        public string instrument_name { get; set; }
        public decimal iv { get; set; }
        public string label { get; set; }
        public string liquidation { get; set; }
        public string liquidity { get; set; }
        public decimal mark_price { get; set; }
        public string matching_id { get; set; }
        public string order_id { get; set; }
        public string order_type { get; set; }
        public string post_only { get; set; }
        public decimal price { get; set; }
        public decimal profit_loss { get; set; }
        public string reduce_only { get; set; }
        public bool self_trade { get; set; }
        public string state { get; set; }
        public int tick_direction { get; set; }
        public long timestamp { get; set; }
        public string trade_id { get; set; }
        public int trade_seq { get; set; }
        public decimal underlying_price { get; set; }
    }
}