using DarkSky.Models;
using HomeEvents.Infrastructure;
using Newtonsoft.Json;

namespace HomeEvents.DarkSky {
    public class DarkSkyLog : InternalObject {
        public Forecast Data { get; set; }
        public static DarkSkyLog FromRepositoryLogEntity(RepositoryLogEntity repositoryLogEntity) {
            return new DarkSkyLog {
                EventDate = repositoryLogEntity.EventDate,
                PropertyId = repositoryLogEntity.PropertyId,
                Data = JsonConvert.DeserializeObject<Forecast>(repositoryLogEntity.Data)
            };
        }
    }
}