using System;
using System.Collections.Generic;
using System.Text;
using Deribit.Core.Configuration;
using Newtonsoft.Json;

namespace Deribit.Core.Messages
{
    public class RequestBase<T>
    {
        public RequestBase()
        {
            //Startup startup = Startup.GetInstance();
            //var apiSettings = JsonConvert.DeserializeObject<ApiSettings>(startup.Configuration.GetSection(ApiSettings.SectionName).Value);
        }
        public string jsonrpc { get; set; }
        public string id { get; set; }
        public string method { get; set; }
        public T @params { get; set; } // Name - params
    }
}
