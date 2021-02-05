﻿using System.Text.Json.Serialization;

namespace Deribit.Core.Messages.Trading
{
    public class CancelMessage : IMessage
    {
        [JsonIgnore]
        public string MethodName
        {
            get => "/private/cancel";
        }

        public string order_id { get; set; }
        public void CheckValidity()
        {
            // TODO Implement Check Validity 
        }
    }

    public class CancelResponse : IResponse<CancelResponse>
    {
        public string order_state { get; set; }
        public float max_show { get; set; }
        public bool api { get; set; }
        public float amount { get; set; }
        public bool web { get; set; }
        public string instrument_name { get; set; }
        public string advanced { get; set; }
        public bool block_trade { get; set; }
        public string original_order_type { get; set; }
        public float price { get; set; }
        public string time_in_force { get; set; }
        public bool auto_replaced { get; set; }
        public string stop_order_id { get; set; }
        public long last_update_timestamp { get; set; }
        public bool post_only { get; set; }
        public bool replaced { get; set; }
        public float filled_amount { get; set; }
        public float average_price { get; set; }
        public string order_id { get; set; }
        public bool reduce_only { get; set; }
        public float commission { get; set; }
        public string app_name { get; set; }
        public float stop_price { get; set; }
        public string label { get; set; }
        public long creation_timestamp { get; set; }
        public string direction { get; set; }
        public bool is_liquidation { get; set; }
        public string order_type { get; set; }
        public float usd { get; set; }
        public float profit_loss { get; set; }
        public float implv { get; set; }
        public string trigger { get; set; }
    }
}