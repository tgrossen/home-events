using System;
using Microsoft.Extensions.Configuration;

namespace HomeEvents.Settings
{
    public static class ConfigurationExtensions
    {
        public static T GetRequiredSetting<T>(this IConfiguration config, string setting)
        {
            var value = config.GetValue<T>(setting);
            if (value == null)
            {
                throw new MissingConfigurationException(setting);
            }

            return value;
        }
    }

    public class MissingConfigurationException : Exception
    {
        public MissingConfigurationException(string setting)
            : base($"Missing setting: '{setting}'. Be sure to connect credentials as well.") { }
    }
}