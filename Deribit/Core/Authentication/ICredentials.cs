using System;
using System.Collections.Generic;
using System.Text;

namespace Deribit.Core.Authentication
{
    public interface ICredentials
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public Uri ServerURL { get; set; }
    }
}
