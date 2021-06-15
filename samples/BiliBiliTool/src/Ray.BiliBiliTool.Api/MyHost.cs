﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Ray.BiliBiliTool.Agent.Extensions;
using Ray.BiliBiliTool.Application.Extensions;
using Ray.BiliBiliTool.Config.Extensions;
using Ray.BiliBiliTool.DomainService.Extensions;
using Ray.BiliBiliTool.Infrastructure;
using Serilog;
using Serilog.Debugging;

namespace Ray.BiliBiliTool.Api
{
    public class MyHost
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder hostBuilder = new HostBuilder();

            //var contentRoot1 = Directory.GetCurrentDirectory();
            //var contentRoot2 = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //var contentRoot3 = AppContext.BaseDirectory;
            var contentRoot = AppDomain.CurrentDomain.BaseDirectory;
            hostBuilder.UseContentRoot(contentRoot);

            //承载系统自身的配置：
            hostBuilder.ConfigureHostConfiguration(hostConfigurationBuilder =>
            {
                hostConfigurationBuilder.AddJsonFile("commandLineMappings.json", false, false);

                Environment.SetEnvironmentVariable(HostDefaults.EnvironmentKey, Environment.GetEnvironmentVariable(Global.EnvironmentKey));
                hostConfigurationBuilder.AddEnvironmentVariables();
            });

            //应用配置:
            hostBuilder.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
            {
                Global.HostingEnvironment = hostBuilderContext.HostingEnvironment;

                //json文件：
                configurationBuilder.AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddJsonFile("exp.json", false, true)
                    .AddJsonFile("donateCoinCanContinueStatus.json", false, true);

                //用户机密：
                if (hostBuilderContext.HostingEnvironment.IsDevelopment())
                {
                    //Assembly assembly = Assembly.Load(new AssemblyName(hostBuilderContext.HostingEnvironment.ApplicationName));
                    Assembly assembly = typeof(MyHost).Assembly;
                    if (assembly != null)
                        configurationBuilder.AddUserSecrets(assembly, true);
                }

                //环境变量：
                configurationBuilder.AddExcludeEmptyEnvironmentVariables("Ray_");

                //命令行：
                if (args != null && args.Length > 0)
                {
                    configurationBuilder.AddCommandLine(args, hostBuilderContext.Configuration
                        .GetSection("CommandLineMappings")
                        .Get<Dictionary<string, string>>());
                }
            });

            //日志:
            hostBuilder.ConfigureLogging((hostBuilderContext, loggingBuilder) =>
            {
                Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(hostBuilderContext.Configuration)
                //.WriteTo.Console()
                .CreateLogger();
                SelfLog.Enable(x => System.Console.WriteLine(x ?? ""));
            }).UseSerilog();

            //DI容器:
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                Global.ConfigurationRoot = (IConfigurationRoot)hostContext.Configuration;

                //HostedService：
                //services.AddHostedService<LogAppInfoHostedService>();
                //services.AddHostedService<PreCheckHostedService>();
                //services.AddHostedService<RandomSleepHostedService>();
                //services.AddHostedService<BiliBiliToolHostedService>();

                services.AddBiliBiliConfigs(hostContext.Configuration);
                services.AddBiliBiliClientApi(hostContext.Configuration);
                services.AddDomainServices();
                services.AddAppServices();
            });

            return hostBuilder;
        }
    }
}
