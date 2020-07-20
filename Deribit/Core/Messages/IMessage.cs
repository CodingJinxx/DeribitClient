using System;
using System.Collections.Generic;
using System.Text;

namespace Deribit.Core.Messages
{
    public interface IMessage
    {
        public string GetJson();
    }
}
