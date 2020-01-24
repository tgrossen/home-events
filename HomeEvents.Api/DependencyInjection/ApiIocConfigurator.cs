using Dapper;
using HomeEvents.DarkSky;
using HomeEvents.Infrastructure;
using HomeEvents.MiniPonics;
using HomeEvents.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeEvents.Api.DependencyInjection
{
    public static class ApiIocConfigurator
    {
        public static void Configure(HostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var settings = new HomeEventsSettings();

            services.Scan(scanner =>
            {
                scanner.FromAssemblyOf<HomeEventsAssemblyLocator>().AddClasses().AsMatchingInterface().WithSingletonLifetime();
                scanner.FromCallingAssembly().AddClasses().AsSelf().AsMatchingInterface().WithTransientLifetime();
            });
            
            services.AddSingleton<IHomeEventsSettings>(settings);

            services.AddSingleton<IInternalRepository<DarkSkyLog>, DarkSkyLogRepository>();
            services.AddSingleton<IInternalRepository<MiniPonicsImageData>, MiniPonicsImageDataRepository>();
        }
    }
}