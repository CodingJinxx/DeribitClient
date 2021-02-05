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

        public void CheckValidity()
        {
            if (this.access_token is null || this.access_token.Count() < 1)
            {
                throw new InvalidParameterException(this.access_token.GetType().ToString(), this.access_token, nameof(this.access_token));
            }
        }
    }
}