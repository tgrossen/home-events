using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using api.Configuration;

namespace api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ApiConfigurationBuilder().Build();
            var host = CreateWebHostBuilder(args)
                .UseUrls(configuration.GetSection("urls").Value)
                .Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
