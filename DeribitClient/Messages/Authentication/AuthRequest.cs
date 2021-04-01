using System;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json;

namespace DeribitClient.Messages.Authentication
{
    [MethodName("/public/auth")]
    public class AuthRequest : IRequest
    {
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
    }
}
