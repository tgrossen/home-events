using System;
using NUnit.Framework;
using StructureMap;

namespace specs.Integration {
    [TestFixture]
    public class With_an_integration_setup<T> : IDisposable {
        protected static T ClassUnderTest;
        protected static Container Container;

        public With_an_integration_setup()
        {
            Container = new Container(c => {
                c.AddRegistry(new IntegrationRegistry());
            });
            ClassUnderTest = Container.GetInstance<T>();
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}