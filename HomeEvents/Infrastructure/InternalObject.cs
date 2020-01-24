using System;

namespace HomeEvents.Infrastructure
{
    public class InternalObject
    {
        public DateTimeOffset EventDate { get; set; } = DateTimeOffset.Now;
        public PropertyId PropertyId { get; set; } = PropertyId.OliveSpgs;
    }
}
