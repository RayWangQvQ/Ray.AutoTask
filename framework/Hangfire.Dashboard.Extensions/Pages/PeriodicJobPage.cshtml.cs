using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Hangfire.Dashboard.Extensions.Models;
using Hangfire.Dashboard.Extensions.Repositories;
using Hangfire.Dashboard.Extensions.Resources;
using Hangfire.Dashboard.Pages;
using Hangfire.Dashboard.Resources;
using Hangfire.Storage;

namespace Hangfire.Dashboard.Extensions.Pages
{
    partial class PeriodicJobPage : RazorPage, IExtensionsPageInfo
    {
        public static string Title => RayStrings.NavigationMenu_PeriodicJobs;

        public static string PageRoute => "/periodic";


        private readonly PeriodicJobRepository _periodicJobRepository;

        public PeriodicJobPage()
        {
            Layout = new LayoutPage(Strings.RecurringJobsPage_Title);
            _periodicJobRepository = new PeriodicJobRepository();
        }

        public void Init()
        {
            pager = null;
            int.TryParse(Query("from"), out from);
            int.TryParse(Query("count"), out perPage);

            periodicJobs.AddRange(_periodicJobRepository.GetPeriodicJobs());
        }

        public List<PeriodicJob> periodicJobs = new List<PeriodicJob>();

        public int from;
        public int perPage;

        public Pager pager;

        public PeriodicJob PeriodicModel = new PeriodicJob();
    }
}
