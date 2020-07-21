using System.IO;
using Deribit.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace DeribitTests.Core.Configuration
{
    public class Startup
    {
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
            Configuration = BuildConfiguration();
        }

        private IConfigurationRoot BuildConfiguration()
        {
            string basepath = Directory.GetCurrentDirectory();
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(basepath)
                .AddJsonFile("testsettings.json", false)
                .AddJsonFile("usersettings.json", false);

            return builder.Build();
        }
    }
}