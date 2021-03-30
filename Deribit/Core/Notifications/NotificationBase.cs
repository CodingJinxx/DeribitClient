using Newtonsoft.Json;

namespace Deribit.Core.Notifications
{
    [JsonObject(ItemRequired = Required.Always)]
    public class NotificationBase<T>
    {
        public string jsonrpc { get; set; }
        public string method { get; set; }
        public T @params { get; set; }
    }
}