using System;
using System.Collections.Generic;
using System.Text;

using Deribit.Core.Types;

using Newtonsoft.Json;

namespace Deribit.Core.Messages
{
    public class AuthenticationMessage : IMessage
    {
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
                    throw new 
                }
            }

            RequestBase<AuthenticationMessage> request = new RequestBase<AuthenticationMessage>();
            request.@params = this;

            return JsonConvert.SerializeObject(request);
        }
    }

    public class AuthenticationResponse : IResponse<AuthenticationResponse>
    {
        public int id { get; private set; }
        public int jsonrpc { get; private set; }
        public int result { get; private set; }
        public string access_token { get; private set; }
        public string refresh_token { get; private set; }
        public string scope { get; private set; }
        public string state { get; private set; }
        public string token_type { get; private set; }
        public AuthenticationResponse FromJson(string json)
        {
            throw new NotImplementedException();
        }
    }
}
