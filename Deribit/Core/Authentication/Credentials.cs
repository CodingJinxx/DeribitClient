﻿namespace Deribit.Core.Authentication
{
    public class Credentials : ICredentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RefreshToken { get; set; }
        public string Signature { get; set; }

        public Credentials(string clientId = null, string clientSecret = null, string refreshToken = null, string signature = null)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.RefreshToken = refreshToken;
            this.Signature = signature;
        }
    }
}
