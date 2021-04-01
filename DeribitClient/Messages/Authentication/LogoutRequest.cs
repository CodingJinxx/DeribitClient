using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace DeribitClient.Messages.Authentication
{
    [MethodName("/private/logout")]
    public class LogoutRequest : IRequest
    {
    }
}