using System;

namespace Deribit.Core.Messages
{
    public interface IMessage
    {
        public string MethodName { get; }
        public string GetJson(Guid id);
        public string GetJson();
    }
}
