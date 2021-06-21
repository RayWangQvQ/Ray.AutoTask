﻿using Hangfire.Annotations;
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
    internal sealed class PetiodicStartDispatcher : Dashboard.IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;

        public PetiodicStartDispatcher()
        {
            _connection = JobStorage.Current.GetConnection();
        }

        public Task Dispatch([NotNull] Dashboard.DashboardContext context)
        {
            var jobId = context.Request.GetQuery("jobId");

            PeriodicJobAgent.StartPeriodicJob(jobId);

            return Task.CompletedTask;
        }
    }
}