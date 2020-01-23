using System.IO;
using Microsoft.Extensions.Configuration;

namespace HomeEvents.Api.Configuration
{
    public class ApiConfigurationBuilder
    {
        public IConfiguration Build()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false)
                .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}