using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Hangfire.Dashboard.Extensions.Models;
using Hangfire.Dashboard.Extensions.Resources;
using Hangfire.Dashboard.Pages;
using Hangfire.Dashboard.Resources;
using Hangfire.Storage;

namespace Hangfire.Dashboard.Extensions.Pages
{
    partial class PeriodicJobPage : RazorPage, IExtensionsPageInfo
    {
        public static string Title => RayStrings.NavigationMenu_PeriodicJobs;

        public static string PageRoute => "/RecurringJobManage";

        public PeriodicJobPage()
        {
            Layout = new LayoutPage(Strings.RecurringJobsPage_Title);
        }

        public void Init()
        {
            pager = null;
            int.TryParse(Query("from"), out from);
            int.TryParse(Query("count"), out perPage);

            using (var connection = Storage.GetConnection())
            {
                var storageConnection = connection as JobStorageConnection;
                if (storageConnection != null)
                {
                    List<PeriodicJobModel> running = PeriodicJobAgent.GetPeriodicJobs(JobState.Running);
                    List<PeriodicJobModel> stoped = PeriodicJobAgent.GetPeriodicJobs(JobState.Stoped);
                    periodicJobs.AddRange(running);
                    periodicJobs.AddRange(stoped);
                }
                else
                {
                    var recurringJobs = connection.GetRecurringJobs();
                }
            }
        }

        public List<PeriodicJobModel> periodicJobs = new List<PeriodicJobModel>();

        public int from;
        public int perPage;

        public Pager pager;
    }
}
