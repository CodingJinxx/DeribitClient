using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DeribitClient.Core
{
    public class Converter
    {
        public static object ToSystemObject(object requestObject)
        {
            switch (requestObject)
            {
                case JObject jObject: // objects become Dictionary<string,object>
                    return ((IEnumerable<KeyValuePair<string, JToken>>) jObject).ToDictionary(j => j.Key, j => ToSystemObject(j.Value));
                case JArray jArray: // arrays become List<object>
                    return jArray.Select(ToSystemObject).ToList();
                case JValue jValue: // values just become the value
                    return jValue.Value;
                default: // don't know what to do here
                    throw new Exception($"Unsupported type: {requestObject.GetType()}");
            }
        }
    }
}