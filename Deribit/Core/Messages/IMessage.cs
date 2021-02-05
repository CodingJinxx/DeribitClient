using System;
using Newtonsoft.Json;

namespace Deribit.Core.Messages
{
    public interface IMessage
    {
        [JsonIgnore]
        public string MethodName { get; }
        public void CheckValidity();

        public static string GetJson<T>(Guid id, T message) where T : IMessage
        {
            message.CheckValidity();
            
            RequestBase<T> request = new RequestBase<T>();
            request.method = message.MethodName;
            request.@params = message;

            if (id != Guid.Empty)
            {
                request.id = id.ToString();
            }
            
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(request, settings);
        }

        public static string GetJson<T>(T message) where T : IMessage
        {
            return GetJson(Guid.Empty, message);
        }
    }
}
