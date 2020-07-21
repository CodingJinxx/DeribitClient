using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace Deribit.Core.Configuration
{
    public class ApiSettings
    {
        public const string SectionName = nameof(ApiSettings);
        public string JSON_RPC { get; set; }
        public string Server_URL { get; set; }
    }
}