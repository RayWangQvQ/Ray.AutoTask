using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ray.BiliBiliTool.Application.Contracts;
using Serilog;

namespace Ray.BiliBiliTool.OpenApi.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMyTasks(this IApplicationBuilder app, IBackgroundJobClient backgroundJobs)
        {
            var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();

            app.UseHangfireDashboard();

            try
            {
                backgroundJobs.Enqueue(() => Log.Information("Hello world from Hangfire!"));
                //backgroundJobs.Enqueue<ITestAppService>(t => t.DoTask());
                RecurringJob.AddOrUpdate<IDailyTaskAppService>(t => t.DoTask(), "5 13 * * *", TimeZoneInfo.Local, typeof(IDailyTaskAppService).Description().ToLower());
                RecurringJob.AddOrUpdate<ILiveLotteryTaskAppService>(t => t.DoTask(), "30 */2 * * *", TimeZoneInfo.Local, typeof(ILiveLotteryTaskAppService).Description().ToLower());
                RecurringJob.AddOrUpdate<IUnfollowBatchedTaskAppService>(t => t.DoTask(), "0 0 * * 1", TimeZoneInfo.Local, typeof(IUnfollowBatchedTaskAppService).Description().ToLower());
            }
            catch
            {
                //ignore
            }

            return app;
        }
    }

    public class TestLog
    {
        public void Run()
        {
            Console.WriteLine("testConsole");
            RealRun(null);
        }

        private void RealRun(PerformContext context)
        {
            Console.WriteLine("testConsole");
            context.WriteLine("testHangfire");
        }
    }
}
