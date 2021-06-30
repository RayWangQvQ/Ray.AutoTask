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
using Hangfire.Dashboard.Extensions.Repositories;

namespace Hangfire.Dashboard.Extensions.Dispatchers
{
    internal sealed class GetPeriodicJobDispatcher : Dashboard.IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;
        private readonly PeriodicJobRepository _periodicJobRepository;

        public GetPeriodicJobDispatcher()
        {
            _connection = JobStorage.Current.GetConnection();
            _periodicJobRepository = new PeriodicJobRepository();
        }

        public async Task Dispatch([NotNull] Dashboard.DashboardContext context)
        {
            if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.StatusCode = 405;

                return;
            }

            var periodicJob = new List<PeriodicJob>();
            periodicJob.AddRange(_periodicJobRepository.GetPeriodicJobs());

            await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
        }
    }
}
