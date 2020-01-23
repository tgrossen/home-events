using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeEvents.Infrastructure;
using HomeEvents.Postgres;
using Newtonsoft.Json;

namespace HomeEvents.DarkSky {
    public class DarkSkyLogRepository : IInternalRepository<DarkSkyLog> {
        
        readonly IPostgresConnection postgresConnection;
        
        public DarkSkyLogRepository(IPostgresConnection postgresConnection)
        {
            this.postgresConnection = postgresConnection;
        }

        public async Task Save(DarkSkyLog record)
        {
            var query = $"INSERT INTO dark_sky_log (event_date, property_id, data) VALUES (@eventDate, @propertyId, @data::json)";
            await postgresConnection.ExecuteAsync(query, new { eventDate = record.EventDate, propertyId = record.PropertyId, data = JsonConvert.SerializeObject(record.Data) });
        }

        public async Task<DarkSkyLog> GetLatest()
        {
            var result = await postgresConnection.QuerySingleAsync<RepositoryLogEntity>("SELECT * FROM dark_sky_log ORDER BY event_date DESC LIMIT 1");
            return DarkSkyLog.FromRepositoryLogEntity(result);
        }

        public Task<IList<DarkSkyLog>> GetLatestHistoryInRange(TimeSpan duration)
        {
            throw new NotImplementedException();
        }
    }
}