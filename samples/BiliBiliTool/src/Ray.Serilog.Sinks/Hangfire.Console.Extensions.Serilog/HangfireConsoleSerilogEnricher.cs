﻿using System;
using Serilog.Core;
using Serilog.Events;

namespace Hangfire.Console.Extensions.Serilog
{
    class HangfireConsoleSerilogEnricher : ILogEventEnricher
    {
        private readonly AsyncLocalLogFilter asyncLocalLogFilter = new AsyncLocalLogFilter();

        public HangfireConsoleSerilogEnricher()
        {
        }       

        /// <inheritdoc />
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var context = asyncLocalLogFilter.Get();
            if (context == null)
                return;
            // Create property value with PerformContext and put as "PerformContext"
            var property = new LogEventProperty("HangFireJob", new PerformingContextStructureValue(context));
            logEvent.AddOrUpdateProperty(property);
        }
    }
}
