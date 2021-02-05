using System.Diagnostics.CodeAnalysis;
using Deribit.Core.Configuration;

namespace Deribit.Core.Messages
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ResponseBase<T> 
    {
        public string id { get; set; }
        public string jsonrpc { get; set; }
        public T result { get; set; }
        public long usIn { get; set; } 
        public long usOut { get; set; } 
        public int usDiff { get; set; } 
        public bool testnet { get; set; } 
    }
}