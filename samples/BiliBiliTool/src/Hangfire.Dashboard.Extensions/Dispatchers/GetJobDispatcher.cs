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

namespace Hangfire.Dashboard.Extensions.Dispatchers
{
    internal sealed class GetPeriodicJobDispatcher : Dashboard.IDashboardDispatcher
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

            var recurringJob = _connection.GetRecurringJobs();
            var periodicJob = new List<PeriodicJobModel>();

            if (recurringJob.Count > 0)
            {
                recurringJob.ForEach((x) =>
                {
                    periodicJob.Add(new PeriodicJobModel
                    {
                        Id = x.Id,
                        Cron = x.Cron,
                        CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.Value.ConvertTimeZone(x.TimeZoneId) : new DateTime(),
                        Error = x.Error,
                        LastExecution = x.LastExecution.HasValue ? x.LastExecution.Value.ConvertTimeZone(x.TimeZoneId).ToString("G") : "N/A",
                        Method = x.Job.Method.Name,
                        JobState = "Running",
                        Class = x.Job.Type.Name,
                        Queue = x.Queue,
                        LastJobId = x.LastJobId,
                        LastJobState = x.LastJobState,
                        NextExecution = x.NextExecution.HasValue ? x.NextExecution.Value.ConvertTimeZone(x.TimeZoneId).ToString("G") : "N/A",
                        Removed = x.Removed,
                        TimeZoneId = x.TimeZoneId
                    });
                });
            }

            //Add job was stopped:
            periodicJob.AddRange(PeriodicJobAgent.GetStoppedPeriodicJobs());

            await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
        }
    }
}
