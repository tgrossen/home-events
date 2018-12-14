using System;
using NUnit.Framework;
using DarkSky.Models;
using Should;
using Newtonsoft.Json;

using common.DarkSky;
using specs.TestUtilities;

namespace specs.Integration.Repositories
{
    [TestFixture]
    public class DarkSkyRepositoryIntegrationSpecs : With_an_integration_setup<DarkSkyLogRepository>
    {
        private DarkSkyLog darkSkyLog;
        private DarkSkyLog savedDarkSkyLog;

        [SetUp]
        public void Setup()
        {
            darkSkyLog = new DarkSkyLog
            {
                EventDate = DateTimeOffset.UtcNow,
                PropertyId = 0,
                Data = new Forecast
                {
                    Latitude = 1
                }
            };

            ClassUnderTest.Save(darkSkyLog);
            savedDarkSkyLog = ClassUnderTest.GetLatest();
        }

        [Test]
        public void SavedCorrectLog()
        {
            savedDarkSkyLog.ShouldDeepEqualWithDatesCloseTo(darkSkyLog, TimeSpan.FromSeconds(1));
        }
    }
}