using System;
using System.IO;
using Deribit.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;


namespace DeribitTests.Core.Configuration
{
    public class Startup
    {
        private bool IsCI = false;

        public IConfigurationRoot Configuration { get; private set; }

        public static Startup GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Startup();
            }

            return _instance;
        }

        private static Startup _instance;

        private Startup()
        {
            IsCI = Environment.GetEnvironmentVariable("CI") == "true";

            Configuration = BuildConfiguration();
        }

        private IConfigurationRoot BuildConfiguration()
        {
            string basepath = Directory.GetCurrentDirectory();
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(basepath)
                .AddJsonFile("testsettings.json", false)
                .AddJsonFile("usersettings.json", IsCI);

            return builder.Build();
        }
    }
}