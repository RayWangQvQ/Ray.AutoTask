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
using System.Linq;
using System.Reflection;
using Hangfire.States;

namespace Hangfire.Dashboard.Extensions.Dispatchers
{
    internal sealed class PetiodicAddOrUpdateDispatcher : Dashboard.IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;
        private readonly PeriodicJobRepository _periodicJobRepository;

        public PetiodicAddOrUpdateDispatcher()
        {
            _connection = JobStorage.Current.GetConnection();
            _periodicJobRepository = new PeriodicJobRepository();
        }

        public async Task Dispatch([NotNull] Dashboard.DashboardContext context)
        {
            var job = new PeriodicJob();
            job.Id = (await context.Request.GetFormValuesAsync("Id").ConfigureAwait(false)).FirstOrDefault();
            job.Cron = (await context.Request.GetFormValuesAsync("Cron").ConfigureAwait(false)).FirstOrDefault();
            job.Queue = (await context.Request.GetFormValuesAsync("Queue").ConfigureAwait(false)).FirstOrDefault();
            job.TimeZoneId = (await context.Request.GetFormValuesAsync("TimeZoneId").ConfigureAwait(false)).FirstOrDefault();

            job.ClassFullName = (await context.Request.GetFormValuesAsync("ClassFullName").ConfigureAwait(false)).FirstOrDefault();
            job.MethodName = (await context.Request.GetFormValuesAsync("MethodName").ConfigureAwait(false)).FirstOrDefault();

            _periodicJobRepository.AddOrUpdate(job);

            context.Response.StatusCode = (int)HttpStatusCode.OK;
        }
    }
}

