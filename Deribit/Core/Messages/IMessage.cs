namespace Deribit.Core.Messages
{
    public interface IMessage
    {
        public string MethodName { get; }
        public string GetJson();
    }
}
