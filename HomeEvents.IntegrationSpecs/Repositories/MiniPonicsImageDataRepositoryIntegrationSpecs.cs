using System;
using HomeEvents.MiniPonics;
using HomeEvents.TestingUtilities.Extensions;
using Machine.Specifications;

namespace HomeEvents.IntegrationSpecs.Repositories
{
    [Subject(typeof(MiniPonicsImageDataRepository))]
    public class When_round_tripping_mini_ponics_image_data : With_an_integration_setup<MiniPonicsImageDataRepository>
    {
        Establish context = () =>
        {
            miniPonicsImageData = new MiniPonicsImageData
            {
                EventDate = DateTimeOffset.UtcNow,
                PropertyId = 0,
                ImageFilePath = "some/path/" + Guid.NewGuid()
            };
        };

        Because of = () =>
        {
            ClassUnderTest.Save(miniPonicsImageData).Wait();
            savedMiniPonicsImageData = ClassUnderTest.GetLatest().GetAwaiter().GetResult();
        };

        It should_have_saved_the_image_data = () => savedMiniPonicsImageData.ShouldDeepEqualWithDatesCloseTo(miniPonicsImageData, TimeSpan.FromSeconds(1));

        static MiniPonicsImageData miniPonicsImageData;
        static MiniPonicsImageData savedMiniPonicsImageData;
    }
}