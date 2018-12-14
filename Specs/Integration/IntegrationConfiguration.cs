using System.IO;
using Microsoft.Extensions.Configuration;

namespace Specs.Integration
{
    public class IntegrationConfigurationBuilder
    {
        public IConfiguration Build()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings_integration.json", optional: false)
                .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}