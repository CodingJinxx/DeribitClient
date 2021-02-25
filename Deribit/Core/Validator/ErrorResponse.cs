namespace Deribit.Core.Validator
{
    public class ErrorResponse
    {
        public Error? error { get; set; }
    }

    public class Error
    {
        public int code { get; set; }
        public string message { get; set; }
        public object? data { get; set; }
    }
}