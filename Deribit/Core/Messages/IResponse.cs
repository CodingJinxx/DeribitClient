using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Deribit.Core.Messages
{
    public interface IResponse<T>
    {
        public static ResponseBase<T> FromJson(string json)
        {
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            var response = JsonConvert.DeserializeObject<ResponseBase<T>>(json, settings);
            

            return response;
        }
    }
}
