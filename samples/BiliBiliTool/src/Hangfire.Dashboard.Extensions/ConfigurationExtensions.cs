﻿using System;
using System.Linq;
using System.Reflection;
using Hangfire.Annotations;
using Hangfire.Dashboard.Extensions.Dispatchers;
using Hangfire.Dashboard.Extensions.Pages;
using Hangfire.Dashboard.Extensions.Repositories;
using Hangfire.Dashboard.Extensions.Resources;

namespace Hangfire.Dashboard.Extensions
{
    public static class ConfigurationExtensions
    {
        private static readonly string[] Javascripts =
        {
            "hangfire-ext.js"
        };

        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assemblies"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseDashboardExtensions(this IGlobalConfiguration config, [NotNull] params string[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(assemblies: assemblies.Select(x => Type.GetType(x).Assembly).ToArray());
            //PeriodicJobBuilder.GetAllJobs();
            CreateManagmentJob();
            return config;
        }

        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assemblies"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseDashboardExtensions(this IGlobalConfiguration config, bool includeReferences = false, [NotNull] params string[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(includeReferences, assemblies.Select(x => Type.GetType(x).Assembly).ToArray());
            //PeriodicJobBuilder.GetAllJobs();
            CreateManagmentJob();
            return config;
        }

        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assemblies"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseDashboardExtensions(this IGlobalConfiguration config, [NotNull] params Assembly[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(assemblies: assemblies);
            //PeriodicJobBuilder.GetAllJobs();
            CreateManagmentJob();
            return config;
        }

        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assembliess"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseDashboardExtensions(this IGlobalConfiguration config, bool includeReferences = false, [NotNull] params Assembly[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(includeReferences, assemblies);
            //PeriodicJobBuilder.GetAllJobs();
            CreateManagmentJob();
            return config;
        }

        [PublicAPI]
        public static IGlobalConfiguration UseDashboardExtensions(this IGlobalConfiguration config)
        {
            CreateManagmentJob();
            return config;
        }

        private static void CreateManagmentJob()
        {
            //注册js文件路由
            DashboardRoutes.Routes.Add("/js-ext[0-9]+", new CombinedResourceDispatcher(
                "application/javascript",
                GetExecutingAssembly(),
                GetContentFolderNamespace("js"),
                Javascripts));

            //注册页面路由
            DashboardRoutes.Routes.AddRazorPage(PeriodicJobPage.PageRoute, x => new PeriodicJobPage());
            DashboardRoutes.Routes.AddRazorPage($"{PeriodicJobPage.PageRoute}/edit", x => new PeriodicJobEditPage());

            //新增顶部菜单
            NavigationMenu.Items.Add(page =>
            new MenuItem(RayStrings.NavigationMenu_PeriodicJobs, page.Url.To(PeriodicJobPage.PageRoute))
            {
                Active = page.RequestPath.StartsWith(PeriodicJobPage.PageRoute),
                Metric = DashboardMetrics.RecurringJobCount
            });

            //注册api
            DashboardRoutes.Routes.AddBatchCommand($"{PeriodicJobPage.PageRoute}/remove",
                (context, jobId) =>
                {
                    IRecurringJobManager manager = context.GetRecurringJobManager();
                    PeriodicJobRepository periodicJobRepository = new PeriodicJobRepository();
                    var job = periodicJobRepository.GetPeriodicJobById(jobId);

                    if (job.JobStateEnum == Models.JobState.Stoped)
                    {
                        periodicJobRepository.StartPeriodicJob(jobId);
                    }
                    manager.RemoveIfExists(jobId);
                });


            DashboardRoutes.Routes.Add($"{PeriodicJobPage.PageRoute}/stop", new PetiodicStopDispatcher());
            DashboardRoutes.Routes.Add($"{PeriodicJobPage.PageRoute}/start", new PetiodicStartDispatcher());
            DashboardRoutes.Routes.Add($"{PeriodicJobPage.PageRoute}/addOrUpdate", new PetiodicAddOrUpdateDispatcher());


        }

        internal static string GetContentFolderNamespace(string contentFolder)
        {
            return $"{typeof(ConfigurationExtensions).Namespace}.Content.{contentFolder}";
        }

        private static Assembly GetExecutingAssembly()
        {
            return typeof(ConfigurationExtensions).GetTypeInfo().Assembly;
        }
    }
}
