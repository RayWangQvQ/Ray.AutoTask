using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Annotations;
using Hangfire.Common;

namespace Hangfire.Dashboard.Extensions
{
    public class PeriodicJobManager : IRecurringJobManager
    {
        private readonly RecurringJobManager recurringJobManager;

        public PeriodicJobManager(RecurringJobManager recurringJobManager)
        {
            this.recurringJobManager = recurringJobManager;
        }

        public void AddOrUpdate([NotNull] string recurringJobId, [NotNull] Job job, [NotNull] string cronExpression, [NotNull] RecurringJobOptions options)
        {
            recurringJobManager.AddOrUpdate(recurringJobId, job, cronExpression, options);
        }

        public void RemoveIfExists([NotNull] string recurringJobId)
        {
            recurringJobManager.RemoveIfExists(recurringJobId);
        }

        public void Trigger([NotNull] string recurringJobId)
        {
            recurringJobManager.Trigger(recurringJobId);
        }
    }
}
