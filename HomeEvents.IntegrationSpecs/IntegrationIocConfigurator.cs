using Dapper;
using HomeEvents.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace HomeEvents.IntegrationSpecs
{
    public static class IntegrationDependencyInjector
    {
        public static ServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var settings = new HomeEventsSettings("appsettings_integration.json");

            services.Scan(scanner =>
            {
                scanner.FromAssemblyOf<HomeEventsAssemblyLocator>().AddClasses().AsSelfWithInterfaces().AsMatchingInterface().WithSingletonLifetime();

                scanner.FromCallingAssembly().AddClasses().AsSelf().AsMatchingInterface().WithTransientLifetime();
            });

            services.AddSingleton<IHomeEventsSettings>(settings);

            return services.BuildServiceProvider();
        }
    }
}