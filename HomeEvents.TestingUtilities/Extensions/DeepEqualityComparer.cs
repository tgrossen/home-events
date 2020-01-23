using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeEvents.TestingUtilities.Extensions
{
    public class DeepEqualityComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            var serializedX = JsonConvert.SerializeObject(x);
            var serializedY = JsonConvert.SerializeObject(y);
            return serializedX == serializedY;
        }

        public int GetHashCode(T obj)
        {
            throw new System.NotImplementedException();
        }
    }
}