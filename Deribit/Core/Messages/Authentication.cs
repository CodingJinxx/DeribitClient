using System;
using System.Collections.Generic;
using System.Text;

using Deribit.Core.Types;

using Newtonsoft.Json;

namespace Deribit.Core.Messages
{
    public class AuthenticationMessage : IMessage
    {
        [Newtonsoft.Json.JsonIgnore]
        public string MethodName { get => "/public/auth"; }
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string refresh_token { get; set; }
        public int timestamp { get; set; }
        public string signature { get; set; }
        public string nonce { get; set; }
        public string data { get; set; }
        public string state { get; set; }
        public string scope { get; set; }



        public string GetJson()
        {
            if (!GrantType.Contains(grant_type))
            {
                throw new InvalidParameterException(this.GetType().Name, grant_type, nameof(grant_type));
            }
            if(grant_type == GrantType.ClientCredentials || grant_type == GrantType.ClientSignature)
            {
                if(client_id == null)
                {
                    throw new MissingParameterException(this.GetType().Name, nameof(client_id));
                }
            }
            if(grant_type == GrantType.ClientCredentials)
            {
                if(client_secret == null)
                {
                    throw new MissingParameterException(this.GetType().Name, nameof(client_secret));
                }
            }
            if(grant_type == GrantType.RefreshToken)
            {
                if(refresh_token == null)
                {
                    throw new MissingParameterException(this.GetType().Name, nameof(refresh_token));
                }
            }
            if(grant_type == GrantType.ClientSignature)
            {
                if(signature == null)
                {
                    throw new MissingParameterException(this.GetType().Name , nameof(refresh_token));
                }
            }
            if(grant_type != GrantType.ClientSignature)
            {
                if(nonce != null)
                {
                    throw new ExcessiveParameterException(this.GetType().Name, nameof(nonce));
                }
                if(data != null)
                {
                    throw new ExcessiveParameterException(this.GetType().Name, nameof(data));
                }
            }

            RequestBase<AuthenticationMessage> request = new RequestBase<AuthenticationMessage>();

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(request, settings);
        }
    }

    public class AuthenticationResponse : IResponse<AuthenticationResponse>
    {
        public string access_token { get; private set; }
        public string refresh_token { get; private set; }
        public string scope { get; private set; }
        public string state { get; private set; }
        public string token_type { get; private set; }
        public static ResponseBase<AuthenticationResponse> FromJson(string json)
        {
            ResponseBase<AuthenticationResponse> response = JsonConvert.DeserializeObject<ResponseBase<AuthenticationResponse>>(json);
            return response;
        }
    }
}
