using System;

namespace HomeEvents.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset TruncateToHour(this DateTimeOffset time)
        {
            return new DateTimeOffset(time.Year, time.Month, time.Day, time.Hour, 0, 0, time.Offset);
        }
        
        public static string ToShortString(this DateTimeOffset time)
        {
            return $"{time.Year}-{time.Month}-{time.Day} {time.Hour}:{time.Minute}";
        }
        
        public static DateTimeOffset TruncateToMinute(this DateTimeOffset time)
        {
            return new DateTimeOffset(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0, time.Offset);
        }
        
        public static DateTimeOffset TruncateToDay(this DateTimeOffset time)
        {
            return new DateTimeOffset(time.Year, time.Month, time.Day, 0, 0, 0, time.Offset);
        }

        public static bool IsCloseTo(this DateTimeOffset a, DateTimeOffset b, TimeSpan? tolerance = null)
        {
            return (a - b).Duration() < (tolerance ?? TimeSpan.FromMilliseconds(1));
        }
    }
}