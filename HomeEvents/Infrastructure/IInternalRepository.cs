using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeEvents.DarkSky;

namespace HomeEvents.Infrastructure
{
    public class RepositoryLogEntity : InternalObject {
        public string Data { get; set; }
    }
    public interface IInternalRepository<T> where T : InternalObject
    {
        Task Save(T record);
        Task<DarkSkyLog> GetLatest();
        Task<IList<DarkSkyLog>> GetLatestHistoryInRange(TimeSpan duration);
    }
}
