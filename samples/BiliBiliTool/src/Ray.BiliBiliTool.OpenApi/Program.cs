using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ray.BiliBiliTool.Api;
using Ray.BiliBiliTool.Infrastructure;

namespace Ray.BiliBiliTool.OpenApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            Global.ServiceProviderRoot = host.Services;

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            MyHost.CreateHostBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
