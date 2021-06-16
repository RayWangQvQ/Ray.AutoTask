using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Dashboard.Extensions.Extensions;
using Hangfire.Dashboard.Extensions.Models;

namespace Hangfire.Dashboard.Extensions.Dispatchers
{
    internal sealed class GetPeriodicJobDispatcher //: Dashboard.IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;

        public GetPeriodicJobDispatcher()
        {
            _connection = JobStorage.Current.GetConnection();
        }

        public async Task Dispatch([NotNull] Dashboard.DashboardContext context)
        {
            if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.StatusCode = 405;

                return;
            }

            var periodicJob = new List<PeriodicJobModel>();
            List<PeriodicJobModel> running = PeriodicJobAgent.GetPeriodicJobs(JobState.Running);
            List<PeriodicJobModel> stoped = PeriodicJobAgent.GetPeriodicJobs(JobState.Stoped);
            periodicJob.AddRange(running);
            periodicJob.AddRange(stoped);

            await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
        }
    }
}
