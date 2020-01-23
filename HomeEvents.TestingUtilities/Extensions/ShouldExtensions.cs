using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HomeEvents.Extensions;
using Machine.Specifications;
using Machine.Specifications.Annotations;
using Newtonsoft.Json;
using NUnit.Framework;

namespace HomeEvents.TestingUtilities.Extensions
{
    public static class ShouldExtensions
    {
        public static bool IsWithin(this DateTimeOffset? self, DateTimeOffset? other, TimeSpan tolerance)
        {
            if (self == other)
            {
                return true;
            }
            if (self == null)
            {
                return false;
            }

            var minRange = self.Value.Year == DateTimeOffset.MinValue.Year ? self.Value : self.Value.Subtract(tolerance);
            var maxRange = self.Value == DateTimeOffset.MaxValue ? self.Value : self.Value.Add(tolerance);
            return other <= maxRange && other >= minRange;
        }

        [AssertionMethod]
        public static void ShouldBeType<T>(this Exception self)
        {
            if (self.GetBaseException().GetType() != typeof(T))
            {
                throw new SpecificationException($"Types are not equal.");
            }
        }

        [AssertionMethod]
        public static void ShouldBeCloseTo(this DateTimeOffset self, DateTimeOffset other, TimeSpan tolerance)
        {
            if (!IsWithin(self, other, tolerance))
            {
                throw new SpecificationException($"DateTimeOffset {self} and {other} are not within tolerance {tolerance}");
            }
        }

        [AssertionMethod]
        public static void ShouldBeCloseTo(this DateTimeOffset? self, DateTimeOffset? other, TimeSpan tolerance)
        {
            if (!IsWithin(self, other, tolerance))
            {
                throw new SpecificationException($"DateTimeOffset {self} and {other} are not within tolerance {tolerance}");
            }
        }

        [AssertionMethod]
        public static void ShouldBeSequenceEqual<T>(this IEnumerable<T> actual, params T[] expected)
        {
            ShouldBeSequenceEqual(actual, (IEnumerable<T>)expected);
        }

        [AssertionMethod]
        public static void ShouldBeSequenceEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            ShouldBeSequenceEqual(actual, expected, EqualityComparer<T>.Default);
        }

        [AssertionMethod]
        public static void ShouldBeSequenceDeepEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            ShouldBeSequenceEqual(actual, expected, new DeepEqualityComparer<T>());
        }

        [AssertionMethod]
        public static void ShouldBeEqualIgnoringOrder<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            CollectionAssert.AreEquivalent(actual, expected);
        }

        [AssertionMethod]
        public static void ShouldDeepEqual<T>(this T actual, T expected)
        {
            var serializedActual = JsonConvert.SerializeObject(actual);
            var serializedExpected = JsonConvert.SerializeObject(expected);
            if (serializedActual != serializedExpected)
            {
                throw new SpecificationException($"These do not deep equal: {Environment.NewLine} actual: {serializedActual} {Environment.NewLine} expected: {serializedExpected}");
            }
        }


        [AssertionMethod]
        public static void ShouldDeepEqualWithProperties<T>(this T actual, T expected, TimeSpan? tolerance = null)
        {
            var span = tolerance ?? TimeSpan.FromSeconds(1);
            var t = actual.GetType();
            if (t.GetTypeInfo().IsClass)
            {
                IList<PropertyInfo> props = t.GetProperties().ToList();

                props.ForEach(prop =>
                {
                    if (prop.PropertyType.Name == nameof(DateTimeOffset) || Nullable.GetUnderlyingType(prop.PropertyType)?.Name == nameof(DateTimeOffset))
                    {
                        ShouldBeCloseTo((DateTimeOffset?)prop.GetValue(actual), (DateTimeOffset?)prop.GetValue(expected), span);
                    }
                    else if (typeof(IEnumerable<string>).IsAssignableFrom(prop.PropertyType))
                    {
                        var actualJson = JsonConvert.SerializeObject(((IEnumerable<string>)prop.GetValue(actual)).OrderBy(x => x));
                        var expectedJson = JsonConvert.SerializeObject(((IEnumerable<string>)prop.GetValue(expected)).OrderBy(x => x));
                        ShouldDeepEqual(actualJson, expectedJson);
                    }
                    else
                    {
                        var actualJson = JsonConvert.SerializeObject(prop.GetValue(actual));
                        var expectedJson = JsonConvert.SerializeObject(prop.GetValue(expected));
                        ShouldDeepEqual(actualJson, expectedJson);
                    }
                });
            }
            else
            {
                var serializedActual = JsonConvert.SerializeObject(actual);
                var serializedExpected = JsonConvert.SerializeObject(expected);
                if (serializedActual != serializedExpected)
                {
                    throw new SpecificationException(
                        $"These do not deep equal: {Environment.NewLine} actual: {serializedActual} {Environment.NewLine} expected: {serializedExpected}");
                }
            }
        }


        [AssertionMethod]
        public static void ShouldDeepEqualIgnoringOrder<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            ShouldDeepEqual(actual.OrderBy(x => x), expected.OrderBy(x => x));
        }

        [AssertionMethod]
        public static void ShouldDictionaryDeepEqual(this IDictionary<string, object> actual, IDictionary<string, object> expected)
        {
            if (actual.Keys.Count != expected.Keys.Count)
            {
                throw new SpecificationException("Dictionaries are not equal. Key count is different.");
            }
            var errors = new List<string>();
            foreach (var key in expected.Keys)
            {
                if (actual.TryGetValue(key, out var resultValue))
                {
                    if (!IsDeepEqual(expected[key], actual[key]))
                    {
                        var actualType = actual[key].GetType().Name;
                        var expectedType = expected[key].GetType().Name;

                        errors.Add($"'{key}' does not equal the expected value." + Environment.NewLine +
                            $"    Actual: {actual[key]}" + Environment.NewLine +
                            $"    Expected: {expected[key]}" + Environment.NewLine +
                            (actualType != expectedType ? $"    Type mismatch: {actualType} != {expectedType}" + Environment.NewLine : ""));
                    }
                }
                else
                {
                    errors.Add($"{key} does not exist." + Environment.NewLine);
                }
            }

            if (errors.Count > 0)
            {
                throw new SpecificationException(string.Join(Environment.NewLine, errors));
            }
        }

        [AssertionMethod]
        public static void ShouldDeepEqualIgnoringTimezone<T>(this T actual, T expected)
        {
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
            };
            var serializedActual = JsonConvert.SerializeObject(actual, settings);
            var serializedExpected = JsonConvert.SerializeObject(expected, settings);
            if (!Equals(serializedActual, serializedExpected))
            {
                throw new SpecificationException($"{serializedActual} does not equal {serializedExpected}");
            }
        }

        [AssertionMethod]
        public static bool IsDeepEqual<T>(this T actual, T expected)
        {
            return JsonConvert.SerializeObject(actual) == JsonConvert.SerializeObject(expected);
        }

        [AssertionMethod]
        public static void ShouldBeSequenceEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T> equalityComparer)
        {
            T[] actualArray = actual.ToArray();
            T[] expectedArray = expected.ToArray();

            if (actualArray.Count() != expectedArray.Count())
                throw new Exception($"Should have {expectedArray.Count()} elements but has {actualArray.Count()}");

            for (int i = 0; i < actual.Count(); ++i)
            {
                T actualItem = actualArray[i];
                T expectedItem = expectedArray[i];

                if (null == actualItem)
                {
                    if (null != expectedItem)
                        throw new Exception(
                            $"Mismatch in item {i}.\nExpected {expectedItem}\nActual: null");
                }
                else if (!equalityComparer.Equals(actualItem, expectedItem))
                {
                    throw new Exception($"Mismatch in item {i}.\nExpected: {expectedItem}\nActual: {actualItem}");
                }
            }
        }

        [AssertionMethod]
        public static void ShouldEqualWithDatesCloseTo<T>(this IEnumerable<T> actual, IEnumerable<T> expected, Func<T, IComparable> getKeyProp, TimeSpan tolerance)
        {
            var actualArray = actual.OrderBy(getKeyProp).ToArray();
            var expectedArray = expected.OrderBy(getKeyProp).ToArray();

            if (actualArray.Count() != expectedArray.Count())
                throw new Exception($"Should have {expectedArray.Count()} elements but has {actualArray.Count()}");

            for (var i = 0; i < actualArray.Length; i++)
            {
                ShouldEqualWithDatesCloseTo(actualArray[i], expectedArray[i], tolerance);
            }
        }

        [AssertionMethod]
        public static void ShouldEqualWithDatesCloseTo<T>(this T actualItem, T expectedItem, TimeSpan tolerance, IList<string> ignoredProps = null)
        {
            var t = actualItem.GetType();
            IList<PropertyInfo> props = t.GetProperties();

            props.Where(prop => ignoredProps != null && !ignoredProps.Contains(prop.Name)).ForEach(prop =>
            {
                if (prop.PropertyType.Name == nameof(DateTimeOffset))
                {
                    ShouldBeCloseTo((DateTimeOffset?)prop.GetValue(actualItem), (DateTimeOffset?)prop.GetValue(expectedItem), tolerance);
                }
                else
                {
                    ShouldDeepEqual(prop.GetValue(actualItem), prop.GetValue(expectedItem));
                }
            });
        }

        [AssertionMethod]
        public static void ShouldDeepEqualWithDatesCloseTo<T>(this IEnumerable<T> actual, IEnumerable<T> expected, Func<T, IComparable> getKeyProp, TimeSpan tolerance, IList<string> ignoredProps = null)
        {
            var actualArray = actual.OrderByDescending(getKeyProp).ToArray();
            var expectedArray = expected.OrderByDescending(getKeyProp).ToArray();

            if (actualArray.Length != expectedArray.Length)
                throw new Exception($"Should have {expectedArray.Count()} elements but has {actualArray.Count()}");

            for (var i = 0; i < actualArray.Length; i++)
            {
                ShouldDeepEqualWithDatesCloseTo(actualArray[i], expectedArray[i], tolerance, ignoredProps);
            }
        }

        [AssertionMethod]
        public static void ShouldDeepEqualWithDatesCloseTo<T>(this T actualItem, T expectedItem, TimeSpan tolerance, IList<string> ignoredProps = null)
        {
            if (typeof(T).GetInterface("IEnumerable") != null)
            {
                throw new Exception("Do not use this for enumerables. Use the other one by passing getKeyProp");
            }
            var t = actualItem.GetType();
            IList<PropertyInfo> props = t.GetProperties().ToList();

            var filteredProps = ignoredProps == null ? props : props.Where(prop => !ignoredProps.Contains(prop.Name)).ToList();

            filteredProps.ForEach(prop =>
            {

                if (prop.PropertyType.Name == nameof(DateTimeOffset) || Nullable.GetUnderlyingType(prop.PropertyType)?.Name == nameof(DateTimeOffset))
                {
                    ShouldBeCloseTo((DateTimeOffset?)prop.GetValue(actualItem), (DateTimeOffset?)prop.GetValue(expectedItem), tolerance);
                }
                else if (typeof(IEnumerable<string>).IsAssignableFrom(prop.PropertyType))
                {
                    var actualJson = JsonConvert.SerializeObject(((IEnumerable<string>)prop.GetValue(actualItem)).OrderBy(x => x));
                    var expectedJson = JsonConvert.SerializeObject(((IEnumerable<string>)prop.GetValue(expectedItem)).OrderBy(x => x));
                    ShouldDeepEqual(actualJson, expectedJson);
                }
                else
                {
                    var actualJson = JsonConvert.SerializeObject(prop.GetValue(actualItem));
                    var expectedJson = JsonConvert.SerializeObject(prop.GetValue(expectedItem));
                    ShouldDeepEqual(actualJson, expectedJson);
                }
            });
        }
    }
}