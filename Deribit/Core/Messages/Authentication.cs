using System;
using System.Diagnostics.CodeAnalysis;
using Deribit.Core.Authentication;
using Deribit.Core.Types;

using Newtonsoft.Json;

namespace Deribit.Core.Messages
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AuthenticationMessage : IMessage
    {
        // Think about adding the state property
        public AuthenticationMessage(ICredentials credentials, string grantType)
        {
            if (!GrantType.Contains(grantType))
            {
                throw new InvalidParameterException(grantType.GetType().ToString(), grantType, nameof(grantType));
            }

            this.grant_type = grantType;
            if (grantType == GrantType.ClientCredentials)
            {
                this.client_secret = credentials.ClientSecret;
                this.client_id = credentials.ClientId;
            }
            else if (grantType == GrantType.ClientSignature)
            {
                this.signature = credentials.Signature;
                this.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
            else if(grantType == GrantType.RefreshToken)
            {
                this.refresh_token = credentials.RefreshToken;
            }
            else
            {
                throw new NotImplementedException($"{grantType} is not implemented");
            }
        }

        [JsonIgnore]
        public string MethodName { get => "/public/auth"; }
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string refresh_token { get; set; }
        public long timestamp { get; set; }
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
            request.id = "0";
            request.method = this.MethodName;
            request.jsonrpc = "2.0";
            request.@params = this;

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(request, settings);
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AuthenticationResponse : IResponse<AuthenticationResponse>
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string state { get; set; }
        public string token_type { get; set; }


    }
}
