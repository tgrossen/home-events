using System;

namespace HomeEvents.Postgres
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PostgresJsonAttribute : Attribute { }
}