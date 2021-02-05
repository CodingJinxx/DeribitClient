using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Deribit.Core.Authentication;
using Deribit.Core.Messages.Exceptions;
using Newtonsoft.Json;

namespace Deribit.Core.Messages.Authentication
{
    public class LogoutMessage : IMessage
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string MethodName { get => "/private/logout"; }
        public string access_token { get; set; }

        public LogoutMessage(string accessToken)
        {
            this.access_token = accessToken;
        }
        public string GetJson(Guid id)
        {
            CheckValidity(this);
            
            RequestBase<LogoutMessage> request = new RequestBase<LogoutMessage>();
            request.method = MethodName;
            request.@params = this;

            if (id != Guid.Empty)
            {
                request.id = id.ToString();
            }
            
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(request, settings);

        }

        public string GetJson()
        {
            return GetJson(Guid.Empty);
        }

        public static void CheckValidity(LogoutMessage message)
        {
            if (message.access_token is null || message.access_token.Count() < 1)
            {
                throw new InvalidParameterException(message.access_token.GetType().ToString(), message.access_token, nameof(message.access_token));
            }
        }
    }
}