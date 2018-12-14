using System;
using System.Collections.Generic;
using common.Infrastructure;
using System.Data;
using Dapper;
using Newtonsoft.Json;

namespace common.DarkSky {
    public class DarkSkyLogRepository : IInternalRepository<DarkSkyLog> {
        readonly IPostgresConnection postgresConnection;
        public DarkSkyLogRepository(IPostgresConnection postgresConnection)
        {
            this.postgresConnection = postgresConnection;
        }

        public void Save(DarkSkyLog record)
        {
            using (IDbConnection connection = postgresConnection.Connection) {
                string query = $"INSERT INTO dark_sky_log (event_date, property_id, data) VALUES (@eventDate, @propertyId, @data::json)";
                connection.Open();
                connection.Execute(query, new { eventDate = record.EventDate, propertyId = record.PropertyId, data = JsonConvert.SerializeObject(record.Data) });
            }
        }

        public DarkSkyLog GetLatest()
        {
            using (IDbConnection connection = postgresConnection.Connection) {
                connection.Open();
                var result = connection.QuerySingle<RepositoryLogEntity>("SELECT * FROM dark_sky_log ORDER BY event_date DESC LIMIT 1");
                return DarkSkyLog.FromRepositoryLogEntity(result);
            }
        }

        public IList<DarkSkyLog> GetLatestHistoryInRange(TimeSpan duration)
        {
            throw new NotImplementedException();
        }
    }
}