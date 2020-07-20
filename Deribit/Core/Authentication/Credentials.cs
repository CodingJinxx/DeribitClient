using System;
using System.Collections.Generic;
using System.Text;

namespace Deribit.Core.Authentication
{
    public class Credentials : ICredentials
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public Uri ServerURL { get; set; }

        public Credentials(string clientID, string clientSecret, Uri serverURL)
        {
            this.ClientID = clientID;
            this.ClientSecret = clientSecret;
            this.ServerURL = serverURL;
        }
    }
}
