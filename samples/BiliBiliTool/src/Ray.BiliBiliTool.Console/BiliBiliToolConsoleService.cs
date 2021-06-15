using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ray.BiliBiliTool.Application.Contracts;
using Ray.BiliBiliTool.Config;
using Ray.BiliBiliTool.Config.Options;
using Ray.BiliBiliTool.Infrastructure;
using Ray.BiliBiliTool.Infrastructure.Helpers;

namespace Ray.BiliBiliTool.Console
{
    public class BiliBiliToolConsoleService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly ILogger<BiliBiliToolConsoleService> _logger;
        private readonly IConfiguration _configuration;
        private readonly SecurityOptions _securityOptions;
        private readonly CookieStrFactory _cookieStrFactory;

        public BiliBiliToolConsoleService(
            IServiceProvider serviceProvider,
            IHostApplicationLifetime applicationLifetime,
            ILogger<BiliBiliToolConsoleService> logger,
            IConfiguration configuration,
            IOptionsMonitor<SecurityOptions> securityOptions,
            CookieStrFactory cookieStrFactory
            )
        {
            _serviceProvider = serviceProvider;
            _applicationLifetime = applicationLifetime;
            _logger = logger;
            _configuration = configuration;
            _securityOptions = securityOptions.CurrentValue;
            _cookieStrFactory = cookieStrFactory;
        }

        public void Run()
        {
            LogAppInfo();
            PreCheck();
            RandomSleep();
            RealRun();
        }

        public void LogAppInfo()
        {
            _logger.LogInformation(
                "【版本号】Ray.BiliBiliTool-v{version}",
                typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    ?.InformationalVersion);
            _logger.LogInformation("【当前IP】{ip} ", IpHelper.GetIp());
            //_logger.LogInformation("当前环境：{env}", Global.HostingEnvironment.EnvironmentName);
            _logger.LogInformation("【开源地址】 {url}" + Environment.NewLine, Constants.SourceCodeUrl);
        }

        public void PreCheck()
        {
            //目标任务
            var tasks = _configuration["RunTasks"]
                .Split("&", options: StringSplitOptions.RemoveEmptyEntries);
            _logger.LogInformation("【目标任务】{tasks}", _configuration["RunTasks"]);
            if (!tasks.Any()) ;

            //Cookie
            _logger.LogInformation("【账号】{count}个" + Environment.NewLine, _cookieStrFactory.Count);
            if (_cookieStrFactory.Count == 0) ;

            //是否跳过
            if (CheckSkip()) return;

            return;
        }

        public void RandomSleep()
        {
            if (_securityOptions.RandomSleepMaxMin > 0)
            {
                int randomMin = new Random().Next(1, ++_securityOptions.RandomSleepMaxMin);
                _logger.LogInformation("随机休眠{min}分钟" + Environment.NewLine, randomMin);
                Thread.Sleep(randomMin * 1000 * 60);
            }
        }

        public void RealRun()
        {
            try
            {
                var tasks = _configuration["RunTasks"]
                    .Split("&", options: StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < _cookieStrFactory.Count; i++)
                {
                    _cookieStrFactory.CurrentNum = i + 1;
                    _logger.LogInformation("账号 {num} ：" + Environment.NewLine, _cookieStrFactory.CurrentNum);

                    try
                    {
                        DoTasks(tasks);
                    }
                    catch (Exception e)
                    {
                        //ignore
                        _logger.LogWarning("异常：{msg}", e);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("程序异常终止，原因：{msg}", ex.Message);
                throw;
            }
            finally
            {
                _logger.LogInformation("开始推送");

                if (Global.ConfigurationRoot["CloseConsoleWhenEnd"] == "1")
                {
                    _logger.LogInformation("正在自动关闭应用...");
                    _applicationLifetime.StopApplication();
                }
            }
        }

        private void DoTasks(string[] tasks)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                foreach (var task in tasks)
                {
                    var type = TaskTypeFactory.Create(task);
                    if (type == null) _logger.LogWarning("任务不存在：{task}", task);

                    var appService = (IAutoTask)scope.ServiceProvider.GetRequiredService(type);
                    appService?.DoTask();
                }
            }
        }


        public bool CheckSkip()
        {
            if (_securityOptions.IsSkipDailyTask)
            {
                _logger.LogWarning("已配置为跳过任务" + Environment.NewLine);
                return true;
            }

            return false;
        }
    }
}
