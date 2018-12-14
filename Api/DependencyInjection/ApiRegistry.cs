using System;
using StructureMap;
using Microsoft.Extensions.Configuration;

using Api.Configuration;
using Common.Infrastructure;
using Common.DarkSky;

namespace Api.DependencyInjection {
    public class ApiRegistry : Registry {
        public ApiRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
                scan.RegisterConcreteTypesAgainstTheFirstInterface();
            });

            For<IConfiguration>().Use(new ApiConfigurationBuilder().Build());
            For<IPostgresConnection>().Use<PostgresConnection>();
            For<IInternalRepository<DarkSkyLog>>().Use<DarkSkyLogRepository>();
        }
    }
}