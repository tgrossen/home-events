using System;
using Common.Infrastructure;
using Microsoft.Extensions.Configuration;
using StructureMap;

namespace Specs.Integration {
    public class IntegrationRegistry : Registry
    {
        public IntegrationRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });

            For<IConfiguration>().Use(new IntegrationConfigurationBuilder().Build());
            For<IPostgresConnection>().Use<PostgresConnection>();
        }
    }
}