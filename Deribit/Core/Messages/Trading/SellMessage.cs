using System;
using System.Buffers;
using Deribit.Core.Messages.ResponseObjects;
using Newtonsoft.Json;

namespace Deribit.Core.Messages.Trading
{
    public class SellMessage : IMessage
    {
        [JsonIgnore]
        public string MethodName { get => "/private/sell"; }
        public string instrument_name { get; set; }
        public decimal amount { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public decimal price { get; set; }
        public string time_in_force { get; set; }
        public decimal max_show { get; set; }
        public bool post_only { get; set; }
        public bool reject_post_only { get; set; }
        public bool reduce_only { get; set; }
        public decimal stop_price { get; set; }
        public string trigger { get; set; }
        public string advanced { get; set; }
        public bool mmp { get; set; }

        public void CheckValidity()
        {
            // TODO Implement Check Validity 
        }
    }

    public class SellResponse : IResponse<SellResponse>
    {
        public Order order { get; set; }
        public Trade[] trades { get; set; }
    }
}