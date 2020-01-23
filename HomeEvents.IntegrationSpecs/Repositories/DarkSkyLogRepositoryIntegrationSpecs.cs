using System;
using DarkSky.Models;
using HomeEvents.DarkSky;
using HomeEvents.TestingUtilities.Extensions;
using Machine.Specifications;

namespace HomeEvents.IntegrationSpecs.Repositories
{
    public class When_round_tripping_dark_sky_logs : With_an_integration_setup<DarkSkyLogRepository>
    {
        Establish context = () =>
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
        };

        Because of = () =>
        {
            ClassUnderTest.Save(darkSkyLog).Wait();
            savedDarkSkyLog = ClassUnderTest.GetLatest().GetAwaiter().GetResult();
        };

        It should_have_saved_the_log = () => savedDarkSkyLog.ShouldDeepEqualWithDatesCloseTo(darkSkyLog, TimeSpan.FromSeconds(1));

        static DarkSkyLog darkSkyLog;
        static DarkSkyLog savedDarkSkyLog;
    }
}