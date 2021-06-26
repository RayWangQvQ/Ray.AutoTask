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
            var job = new PeriodicJobModel();
            job.Id = context.Request.GetQuery("Id");
            job.Cron = context.Request.GetQuery("Cron");
            job.Queue = context.Request.GetQuery("Queue");
            job.TimeZoneId = context.Request.GetQuery("TimeZoneId");

            var class = context.Request.GetQuery("Class");
            var method = context.Request.GetQuery("Method");

        var timeZone = TimeZoneInfo.Utc;

            /*
            if (!Utility.IsValidSchedule(job.Cron))
            {
                response.Status = false;
                response.Message = "Invalid CRON";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                return;
            }
            */

            try
            {
                if (!string.IsNullOrEmpty(job.TimeZoneId))
                {
                    timeZone = TimeZoneInfo.FindSystemTimeZoneById(job.TimeZoneId);
                }
}
            catch (Exception ex)
{
    //response.Status = false;
    //response.Message = ex.Message;

    //await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

    return;
}


if (!StorageAssemblySingleton.GetInstance().IsValidType(job.Class))
{
    //response.Status = false;
    //response.Message = "The Class not found";

    //await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

    return;
}

if (!StorageAssemblySingleton.GetInstance().IsValidMethod(job.Class, job.Method))
{
    //response.Status = false;
    //response.Message = "The Method not found";

    //await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

    return;
}


MethodInfo? methodInfo = StorageAssemblySingleton.GetInstance()
    .currentAssembly
    .Where(x => x?.GetType(job.Class)?.GetMethod(job.Method) != null)
    .FirstOrDefault()
    .GetType(job.Class)
    .GetMethod(job.Method);

_periodicJobRepository.AddOrUpdate(
          job.Id,
          methodInfo,
          job.Cron,
          timeZone,
          job.Queue ?? EnqueuedState.DefaultQueue);


context.Response.StatusCode = (int)HttpStatusCode.OK;

await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
