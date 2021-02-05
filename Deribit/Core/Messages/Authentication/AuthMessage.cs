using System;
using System.Diagnostics.CodeAnalysis;
using Deribit.Core.Authentication;
using Deribit.Core.Types;
using Deribit.Core.Messages.Exceptions;

using Newtonsoft.Json;

namespace Deribit.Core.Messages.Authentication
{
    public class AuthMessage : IMessage
    {
        
        // Think about adding the state property
        public AuthMessage(ICredentials credentials, string grantType)
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
            return GetJson(Guid.Empty);
        }

        public string GetJson(Guid id)
        {
            CheckValidity(this);

            RequestBase<AuthMessage> request = new RequestBase<AuthMessage>();
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

        public static void CheckValidity(AuthMessage message)
        {
            if (!GrantType.Contains(message.grant_type))
            {
                throw new InvalidParameterException(message.GetType().Name, message.grant_type, nameof(grant_type));
            }
            if (message.grant_type == GrantType.ClientCredentials || message.grant_type == GrantType.ClientSignature)
            {
                if (message.client_id == null)
                {
                    throw new MissingParameterException(message.GetType().Name, nameof(client_id));
                }
            }
            if (message.grant_type == GrantType.ClientCredentials)
            {
                if (message.client_secret == null)
                {
                    throw new MissingParameterException(message.GetType().Name, nameof(client_secret));
                }
            }
            if (message.grant_type == GrantType.RefreshToken)
            {
                if (message.refresh_token == null)
                {
                    throw new MissingParameterException(message.GetType().Name, nameof(refresh_token));
                }
            }
            if (message.grant_type == GrantType.ClientSignature)
            {
                if (message.signature == null)
                {
                    throw new MissingParameterException(message.GetType().Name, nameof(refresh_token));
                }
            }
            if (message.grant_type != GrantType.ClientSignature)
            {
                if (message.nonce != null)
                {
                    throw new ExcessiveParameterException(message.GetType().Name, nameof(nonce));
                }
                if (message.data != null)
                {
                    throw new ExcessiveParameterException(message.GetType().Name, nameof(data));
                }
            }
        }
    }

    public class AuthenticationResponse : IResponse<AuthenticationResponse>
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string state { get; set; }
        public string token_type { get; set; }
    }
}
