using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Deribit.Core.Messages
{
    public interface IMessage
    {
        public string MethodName { get; }
        public string GetJson();
    }
}
