using System.Text.Json.Serialization;

namespace Deribit.Core.Messages.MarketData
{
    public class GetInstrumentMessage : IMessage
    {
        [JsonIgnore]
        public string MethodName { get => "/public/get_instrument";}
        public string instrument_name { get; set; }
        public void CheckValidity()
        {
            // TODO Check Validity
        }
    }

    public class GetInstrumentResponse : IResponse<GetInstrumentResponse>
    {
        public string base_currency { get; set; }
        public float block_trade_commission { get; set; }
        public float contract_size { get; set; }
        public long creation_timestamp { get; set; }
        public long expiration_timestamp { get; set; }
        public string instrument_name { get; set; }
        public bool is_active { get; set; }
        public string kind { get; set; }
        public int? leverage { get; set; }
        public float maker_commission { get; set; }
        public float? min_trade_amount { get; set; }
        public string? option_type { get; set; }
        public string quote_currency { get; set; }
        public string settlement_period { get; set; }
        public float? strike { get; set; }
        public float taker_commission { get; set; }
        public float tick_size { get; set; }
    }
}