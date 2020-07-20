using System;
using System.Collections.Generic;
using System.Text;

namespace Deribit.Core.Authentication
{
    interface ICredentials
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public Uri ServerURL { get; set; }
    }
}
