namespace DeribitClient.Messages.SubscriptionManagement
{
    [MethodName("private/subscribe")]
    public class SubscribeRequest : IRequest
    {
        public string[] channels { get; set; }
    }
}