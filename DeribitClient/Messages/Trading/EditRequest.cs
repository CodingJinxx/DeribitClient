using System;
using DeribitClient.DeribitTypes;
using Newtonsoft.Json;

namespace DeribitClient.Messages.Trading
{
    [MethodName("/private/edit")]
    public class EditRequest : IRequest
    {
        public string order_id { get; set; }
        public decimal amount { get; set; }
        public decimal? price { get; set; }
        public bool? post_only { get; set; }
        public bool? reduce_only { get; set; }
        public bool? reject_post_only { get; set; }
        public string? advanced { get; set; }
        public decimal? stop_price { get; set; }
        public bool? mmp { get; set; }
    }
}