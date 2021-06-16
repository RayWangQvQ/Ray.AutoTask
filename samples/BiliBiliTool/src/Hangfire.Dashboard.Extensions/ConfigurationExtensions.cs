using System;
using System.Reflection;
using Hangfire.Annotations;
using Hangfire.Dashboard.Extensions.Pages;
using Hangfire.Dashboard.Extensions.Resources;

namespace Hangfire.Dashboard.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assemblies"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseDashboardExtensions(this IGlobalConfiguration config, [NotNull] params string[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            //StorageAssemblySingleton.GetInstance().SetCurrentAssembly(assemblies: assemblies.Select(x => Type.GetType(x).Assembly).ToArray());
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

            //StorageAssemblySingleton.GetInstance().SetCurrentAssembly(includeReferences, assemblies.Select(x => Type.GetType(x).Assembly).ToArray());
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

            //StorageAssemblySingleton.GetInstance().SetCurrentAssembly(assemblies: assemblies);
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

            //StorageAssemblySingleton.GetInstance().SetCurrentAssembly(includeReferences, assemblies);
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
            DashboardRoutes.Routes.AddRazorPage(RecurringJobExtensionsPage.PageRoute, x => new RecurringJobExtensionsPage());

            NavigationMenu.Items.Add(page =>
            //new MenuItem(RecurringJobExtensionsPage.Title, page.Url.To(RecurringJobExtensionsPage.PageRoute))
            new MenuItem(RayStrings.NavigationMenu_PeriodicJobs, page.Url.To(RecurringJobExtensionsPage.PageRoute))
            {
                Active = page.RequestPath.StartsWith(RecurringJobExtensionsPage.PageRoute),
                Metric = DashboardMetrics.RecurringJobCount
            });
        }
    }
}
