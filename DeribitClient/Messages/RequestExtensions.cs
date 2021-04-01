using System.Globalization;
using System.Runtime.CompilerServices;
using System.Reflection;
using Newtonsoft.Json;

namespace DeribitClient.Messages
{
    public static class RequestExtensions
    {
        private static JsonSerializerSettings _settings;

        static RequestExtensions()
        {
            _settings = new JsonSerializerSettings();
            _settings.NullValueHandling = NullValueHandling.Ignore;
        }
        
        public static string ToJson(this IRequest caller, string id, out string methodName)
        {
            methodName = caller.GetType().GetCustomAttribute<MethodName>().methodName;
            
            RequestBase<object> request = new RequestBase<object>();
            request.id = id;
            request.method = methodName;
            request.@params = caller;

            return JsonConvert.SerializeObject(request, _settings);
        }
        
        public static string ToJson(this IRequest caller)
        {
            var methodName = caller.GetType().GetCustomAttribute<MethodName>().methodName;
            
            RequestBase<object> request = new RequestBase<object>();
            request.id = null;
            request.method = methodName;
            request.@params = caller;

            return JsonConvert.SerializeObject(request, _settings);
        }
    }
}