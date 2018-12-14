using System;
using common.Infrastructure;
using Microsoft.Extensions.Configuration;
using StructureMap;

namespace specs.Integration {
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