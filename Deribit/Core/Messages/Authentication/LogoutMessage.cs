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

        public LogoutMessage()
        {
          
        }

        public void CheckValidity()
        {
          
        }
    }
}