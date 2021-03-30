using Deribit.Core.Notifications.NotificationObjects;

namespace Deribit.Core.Notifications
{
    public class SubscriptionNotification
    {
        public string channel { get; set; }
        public SubscriptionData data { get; set; }
    }
}