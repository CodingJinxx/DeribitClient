using System.Collections.Generic;
using Newtonsoft.Json;

namespace Deribit.Core.Validator
{
    public class ServerErrorHandler : IServerErrorHandler
    {
        private JsonSerializerSettings settings;
        public ServerErrorHandler()
        {
            settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
        }

        public ServerSideException ValidateJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;

            var jsonObj = JsonConvert.DeserializeObject<ErrorResponse>(json);
            if (jsonObj.error is not null)
            {
                return new ServerSideException(jsonObj.error.code, jsonObj.error.message,
                    ErrorMessages.CodeToMessage[jsonObj.error.code]);
            }

            return null;
        }
    }
}