using System;
using Deribit.Core.Messages.ResponseObjects;
using Newtonsoft.Json;

namespace Deribit.Core.Messages.Trading
{
    public class EditMessage : IMessage
    {
        [JsonIgnore]
        public string MethodName
        {
            get => "/private/edit";
        }

        public string order_id { get; set; }
        public float amount { get; set; }
        public float? price { get; set; }
        public bool? post_only { get; set; }
        public bool? reduce_only { get; set; }
        public bool? reject_post_only { get; set; }
        public string? advanced { get; set; }
        public float? stop_price { get; set; }
        public bool? mmp { get; set; }
        
        public void CheckValidity()
        {
            // TODO Implement Check Validity 
        }
    }

    public class EditResponse : IResponse<EditResponse>
    {
        public Order order { get; set; }
        public Trade[] trades { get; set; }
    }
}