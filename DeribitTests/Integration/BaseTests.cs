using System;
using System.IO;
using Deribit.Core.Authentication;
using Deribit.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace DeribitTests.Integration
{
    public class BaseTests
    {
        protected Credentials credentials;
        protected Uri server_address;
        protected readonly ITestOutputHelper output;
        
        public BaseTests(ITestOutputHelper output)
        {
            this.output = output;
            var isRunningInsideAction = Environment.GetEnvironmentVariable("CI") == "true";

            IConfigurationRoot config = null;
            string clientId, clientSecret;
            if (isRunningInsideAction)
            {
                clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
                clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
                server_address = new Uri(Environment.GetEnvironmentVariable("SERVER_ADDRESS"));
                ApiSettings.JsonRpc = Environment.GetEnvironmentVariable("JSON_RPC");
            }
            else
            {
                string basePath = Directory.GetCurrentDirectory();
                IConfigurationBuilder builder = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("testsettings.json", false)
                    .AddJsonFile("usersettings.json", false);

                config = builder.Build();

                clientId = config.GetSection("UserSettings").GetSection("Client_Id").Value;
                clientSecret = config.GetSection("UserSettings").GetSection("Client_Secret").Value;
                ApiSettings.JsonRpc = config.GetSection("ApiSettings").GetSection("JSON_RPC").Value;
            }

            if (config != null)
                this.server_address = new Uri(config.GetSection("ApiSettings").GetSection("Server_URL").Value);
            this.credentials = new Credentials(clientId, clientSecret);
        }

    }
}