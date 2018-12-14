using System;
using System.Collections.Generic;

namespace common.Infrastructure
{
    public class RepositoryLogEntity : InternalObject {
        public string Data { get; set; }
    }
    public interface IInternalRepository<T> where T : InternalObject
    {
        void Save(T record);
        T GetLatest();
        IList<T> GetLatestHistoryInRange(TimeSpan duration);
    }
}
