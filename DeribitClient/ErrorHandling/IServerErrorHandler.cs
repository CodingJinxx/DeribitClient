using System.Collections.Generic;

namespace DeribitClient.Validator
{
    public interface IServerErrorHandler
    {
        ServerSideException ValidateJson(string json);
        ServerSideException ValidateDictionary(Dictionary<string, object> response);
    }
}