namespace Deribit.Core.Messages.SubscriptionManagement
{
    public class UnsubscribeMessage : IMessage
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string MethodName { get => "private/unsubscribe"; }

        public string[] channels { get; set; }
        public void CheckValidity()
        {
            // TODO Implement Check Validity 
        }
    }

    public class Unsubscribe : IResponse<string[]>
    {
        
    }
}