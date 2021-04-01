namespace DeribitClient.Messages.SubscriptionManagement
{
    [MethodName("private/unsubscribe")]
    public class UnsubscribeRequest : IRequest
    {
        public string[] channels { get; set; }
    }
}