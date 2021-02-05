using System;
using Deribit.Core.Messages.Exceptions;
using Deribit.Core.Messages.ResponseObjects;
using Newtonsoft.Json;

namespace Deribit.Core.Messages.Trading
{
    public class BuyMessage : IMessage
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string MethodName { get => "/private/buy"; }
        public string instrument_name { get; set; }
        public float amount { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public float price { get; set; }
        public string time_in_force { get; set; }
        public float max_show { get; set; }
        public bool post_only { get; set; }
        public bool reject_post_only { get; set; }
        public bool reduce_only { get; set; }
        public float stop_price { get; set; }
        public string trigger { get; set; }
        public string advanced { get; set; }
        public bool mmp { get; set; }

        public BuyMessage()
        {
        }

        public static void CheckValidity(BuyMessage message)
        {
            // TODO Check Validity on the Buy Message
        }
        public string GetJson(Guid id)
        {
            CheckValidity(this);
            
            RequestBase<BuyMessage> request = new RequestBase<BuyMessage>();
            request.method = MethodName;
            request.@params = this;

            if (id != Guid.Empty)
            {
                request.id = id.ToString();
            }
            
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(request, settings);
        }

        public string GetJson()
        {
            return GetJson(Guid.Empty);
        }
    }

    public class BuyResponse : IResponse<BuyResponse>
    {
        public Order order { get; set; }
        public Trades[] trades { get; set; }
    }
}