using System.Text.Json.Serialization;
using Deribit.Core.Messages.ResponseObjects;

namespace Deribit.Core.Messages.MarketData
{
    public class GetCurrenciesMessage : IMessage
    {
        [JsonIgnore]
        public string MethodName { get => "/public/get_currencies"; }
        public void CheckValidity()
        {
            // TODO Implement Check Validity
        }
    }

    public class GetCurrenciesResponse : IResponse<GetCurrenciesResponse[]>
    {
        public string coin_type { get; set; }
        public string currency { get; set; }
        public string currency_long { get; set; }
        public int fee_precision { get; set; }
        public int min_confirmations { get; set; }
        public float min_withdrawal_fee { get; set; }
        public float withdrawal_fee { get; set; }
        public WithdrawalPriority[] withdrawal_priorities { get; set; }
    }
}