using System.Diagnostics.CodeAnalysis;

namespace Deribit.Core.Messages
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ResponseBase<T> 
    {
        public string id { get; set; }
        public string jsonrpc { get; set; }
        public T result { get; set; }
    }
}