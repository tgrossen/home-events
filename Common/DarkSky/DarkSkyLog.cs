using System;
using Common.Infrastructure;
using DarkSky.Models;
using Newtonsoft.Json;

namespace Common.DarkSky {
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