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

        public Task Dispatch([NotNull] Dashboard.DashboardContext context)
        {
            var job = new PeriodicJobModel();
            job.Id = context.Request.GetFormValuesAsync("Id").Result.FirstOrDefault();
            job.Cron = context.Request.GetFormValuesAsync("Cron").Result.FirstOrDefault();
            job.Queue = context.Request.GetFormValuesAsync("Queue").Result.FirstOrDefault();
            job.TimeZoneId = context.Request.GetFormValuesAsync("TimeZoneId").Result.FirstOrDefault();

            job.ClassFullName = context.Request.GetFormValuesAsync("ClassFullName").Result.FirstOrDefault();
            job.MethodName = context.Request.GetFormValuesAsync("MethodName").Result.FirstOrDefault();

            _periodicJobRepository.AddOrUpdate(job);

            context.Response.StatusCode = (int)HttpStatusCode.OK;

            return Task.CompletedTask;
        }
    }
}

