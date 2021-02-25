namespace Deribit.Core.Validator
{
    public interface IServerErrorHandler
    {
        ServerSideException ValidateJson(string json);
    }
}