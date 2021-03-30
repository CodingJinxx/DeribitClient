namespace Deribit.Core.Notifications.NotificationObjects
{
    public class SubscriptionData
    {
        public long timestamp { get; set; }
        public float price { get; set; }
        public string index_name { get; set; }
    }
}