using System.Diagnostics.CodeAnalysis;
using Deribit.Core.Configuration;

namespace Deribit.Core.Messages
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ResponseBase<T> 
    {
        public string id { get; set; }
        public string jsonrpc { get; set; } = ApiSettings.JsonRpc;
        public T result { get; set; }
    }
}