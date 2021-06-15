using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ray.BiliBiliTool.Api;
using Ray.BiliBiliTool.Infrastructure;
using Serilog;

namespace Ray.BiliBiliTool.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHost(args);

            Global.ServiceProviderRoot = host.Services;

            try
            {
                //host.Run();
                using (var scope = host.Services.CreateScope())
                {
                    var bilibili = scope.ServiceProvider.GetRequiredService<BiliBiliToolConsoleService>();
                    bilibili.Run();
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
            }
            finally
            {
                Log.CloseAndFlush();
                if (Global.ConfigurationRoot["CloseConsoleWhenEnd"] != "1")
                    Task.Delay(10 * 1000 * 60).Wait();//停留10分钟，用于查看console窗口的异常信息
            }
        }

        public static IHost CreateHost(string[] args)
        {
            var hostBuilder = MyHost.CreateHostBuilder(args);

            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<BiliBiliToolConsoleService>();
            });

            IHost host = hostBuilder
                .UseConsoleLifetime()
                .Build();
            return host;
        }
    }
}
