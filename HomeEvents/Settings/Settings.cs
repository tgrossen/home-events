using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace HomeEvents.Settings
{
    public interface IHomeEventsSettings
    {
        string PostgresHost { get; }
        int PostgresPort { get; }
        string PostgresUsername { get; }
        string PostgresPassword { get; }
        string PostgresDatabase { get; }
    }

    public class HomeEventsSettings : IHomeEventsSettings
    {
        readonly IConfigurationRoot config;

        public HomeEventsSettings(string fileName = "appsettings.json")
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(fileName, false)
                .SetBasePath(Directory.GetCurrentDirectory());

            config = builder.Build();
        }

        public string PostgresHost => config.GetSection("Postgres").GetValue<string>("Host");
        public int PostgresPort => config.GetSection("Postgres").GetValue<int>("Port");
        public string PostgresUsername => config.GetSection("Postgres").GetValue<string>("Username");
        public string PostgresPassword => config.GetSection("Postgres").GetValue<string>("Password");
        public string PostgresDatabase => config.GetSection("Postgres").GetValue<string>("Database");
    }
}