using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeEvents.Infrastructure;
using HomeEvents.Postgres;

namespace HomeEvents.MiniPonics
{
    public class MiniPonicsImageDataRepository : IInternalRepository<MiniPonicsImageData>
    {
        readonly IPostgresConnection postgresConnection;

        public MiniPonicsImageDataRepository(IPostgresConnection postgresConnection)
        {
            this.postgresConnection = postgresConnection;
        }

        public async Task Save(MiniPonicsImageData record)
        {
            await postgresConnection.ExecuteAsync("INSERT INTO mini_ponics_image_data (event_date, property_id, image_file_path) VALUES (@eventDate, @propertyId, @imageFilePath);", new
            {
                eventDate = record.EventDate,
                propertyId = record.PropertyId,
                imageFilePath = record.ImageFilePath
            });
        }

        public async Task<MiniPonicsImageData> GetLatest()
        {
            var result = await postgresConnection.QuerySingleAsync<MiniPonicsImageData>("SELECT * FROM mini_ponics_image_data ORDER BY event_date DESC LIMIT 1");
            return result;
        }

        public Task<IList<MiniPonicsImageData>> GetLatestInRange(TimeSpan duration)
        {
            throw new NotImplementedException();
        }
    }
}