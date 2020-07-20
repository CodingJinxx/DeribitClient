using System;
using System.Collections.Generic;
using System.Text;

namespace Deribit.Core.Messages
{
    public interface IResponse<T>
    {
        public T FromJson(string json);
    }
}
