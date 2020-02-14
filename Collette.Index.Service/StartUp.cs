using Collette.Index.Core;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Collette.Index.Service
{
    public class Startup
    {
        private static IConfiguration configuration;
        public void ConfigureService()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            configuration = builder.Build();

            Application app = new Application(typeof(Startup).Assembly.GetName());

            app.InitializeServices(configuration);
          
        }


    }
}
