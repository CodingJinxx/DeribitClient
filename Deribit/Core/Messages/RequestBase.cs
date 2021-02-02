using System.Diagnostics.CodeAnalysis;
using Deribit.Core.Configuration;

namespace Deribit.Core.Messages
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RequestBase<T>
    {
        public string jsonrpc { get; set; } = ApiSettings.JsonRpc;
        public string id { get; set; }
        public string method { get; set; }
        public T @params { get; set; } // Name - params
    }
}
