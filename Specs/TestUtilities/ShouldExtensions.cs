using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Specs.TestUtilities
{
    public static class ShouldExtensions
    {
        public static void ShouldDeepEqual<T>(this T actual, T comp)
        {
            Assert.That(JsonConvert.SerializeObject(actual) == JsonConvert.SerializeObject(comp), Is.True);
        }

        public static void ShouldBeCloseTo(this DateTimeOffset self, DateTimeOffset other, TimeSpan tolerance)
        {
            Assert.That(self, Is.EqualTo(other).Within(tolerance));
        }

        public static void ShouldDeepEqualWithDatesCloseTo<T>(this T actual, T expected, TimeSpan tolerance)
        {
            var t = actual.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>();
            
            if (!t.IsInterface)
                props = t.GetProperties();
            else props = (new Type[] { t })
                .Concat(t.GetInterfaces())
                .SelectMany(i => i.GetProperties()).ToList();

            foreach (var prop in props)
            {
                if (prop.GetValue(actual).GetType() == typeof(DateTimeOffset))
                {
                    Assert.That(prop.GetValue(actual), Is.EqualTo(prop.GetValue(expected)).Within(tolerance));
                }
                else
                {
                    Assert.That(JsonConvert.SerializeObject(prop.GetValue(actual)) == JsonConvert.SerializeObject(prop.GetValue(expected)), Is.True);
                }
            }
        }
    }
}