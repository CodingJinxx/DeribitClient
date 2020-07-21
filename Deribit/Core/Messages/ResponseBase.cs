namespace Deribit.Core.Messages
{
    public class ResponseBase<T>
    {
        public int id { get; set; }
        public string jsonrpc { get; set; }
        public T result { get; set; }
    }
}