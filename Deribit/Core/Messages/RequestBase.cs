using System;
using System.Collections.Generic;
using System.Text;

namespace Deribit.Core.Messages
{
    public class RequestBase<T>
    {
        public string jsonrpc { get; set; }
        public string id { get; set; }
        public string method { get; set; }
        public T @params { get; set; } // Name - params
    }
}
