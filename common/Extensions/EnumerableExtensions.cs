using System;
using System.Collections.Generic;

namespace common.Extensions {
    public static class EnumerableExceptions {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }
    }
}