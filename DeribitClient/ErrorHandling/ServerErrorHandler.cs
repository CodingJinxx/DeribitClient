using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DeribitClient.Validator
{
    public class ServerErrorHandler : IServerErrorHandler
    {
        private JsonSerializerSettings settings;
        public ServerErrorHandler()
        {
            this.settings = new JsonSerializerSettings();
            this.settings.NullValueHandling = NullValueHandling.Ignore;
        }

        public virtual ServerSideException ValidateJson(string json)
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

        public virtual ServerSideException ValidateDictionary(Dictionary<string, object> response)
        {
            if (response.TryGetValue("error", out var error))
            {
                var convertedError = error as Dictionary<string, object>;
                return new ServerSideException(Convert.ToInt32(convertedError["code"]),
                    Convert.ToString(convertedError["message"]),
                    ErrorMessages.CodeToMessage[Convert.ToInt32(convertedError["code"])]);
            }

            return null;
        }
    }
}