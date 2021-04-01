namespace DeribitClient.Validator
{
    public interface IServerErrorHandler
    {
        ServerSideException ValidateJson(string json);
    }
}