namespace Deribit.Core.Messages.SubscriptionManagement
{
    public class SubscribeMessage : IMessage
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string MethodName { get => "private/subscribe"; }

        public string[] channels { get; set; }
        public void CheckValidity()
        {
            // TODO Implement Check Validity 
        }
    }

    public class SubscribeResponse : IResponse<string[]>
    {
        
    }
}