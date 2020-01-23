using Machine.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace HomeEvents.IntegrationSpecs {
    public abstract class With_an_integration_setup<T>
    {
        protected static TService GetService<TService>()
        {
            return (TService)ServiceProvider.GetService(typeof(TService));
        }

        Establish context = () =>
        {
            ServiceProvider = IntegrationDependencyInjector.GetServiceProvider();
            ClassUnderTest = GetService<T>();
        };

        Cleanup after = () =>
        {
            ServiceProvider.Dispose();
        };

        protected static T ClassUnderTest;
        static ServiceProvider ServiceProvider;
    }
}