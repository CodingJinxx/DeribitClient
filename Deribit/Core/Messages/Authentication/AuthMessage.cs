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

        public void CheckValidity()
        {
            if (!GrantType.Contains(this.grant_type))
            {
                throw new InvalidParameterException(this.GetType().Name, this.grant_type, nameof(grant_type));
            }
            if (this.grant_type == GrantType.ClientCredentials || this.grant_type == GrantType.ClientSignature)
            {
                if (this.client_id == null)
                {
                    throw new MissingParameterException(this.GetType().Name, nameof(client_id));
                }
            }
            if (this.grant_type == GrantType.ClientCredentials)
            {
                if (this.client_secret == null)
                {
                    throw new MissingParameterException(this.GetType().Name, nameof(client_secret));
                }
            }
            if (this.grant_type == GrantType.RefreshToken)
            {
                if (this.refresh_token == null)
                {
                    throw new MissingParameterException(this.GetType().Name, nameof(refresh_token));
                }
            }
            if (this.grant_type == GrantType.ClientSignature)
            {
                if (this.signature == null)
                {
                    throw new MissingParameterException(this.GetType().Name, nameof(refresh_token));
                }
            }
            if (this.grant_type != GrantType.ClientSignature)
            {
                if (this.nonce != null)
                {
                    throw new ExcessiveParameterException(this.GetType().Name, nameof(nonce));
                }
                if (this.data != null)
                {
                    throw new ExcessiveParameterException(this.GetType().Name, nameof(data));
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
