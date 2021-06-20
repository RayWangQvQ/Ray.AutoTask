using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Annotations;
using Hangfire.Client;
using Hangfire.Common;

namespace Hangfire.Dashboard.Extensions
{
    public class PeriodicJobManager : RecurringJobManager
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(15);

        private readonly JobStorage _storage;
        private readonly IBackgroundJobFactory _factory;
        private readonly Func<DateTime> _nowFactory;
        private readonly ITimeZoneResolver _timeZoneResolver;

        public PeriodicJobManager()
            : this(JobStorage.Current)
        {
        }

        public PeriodicJobManager([NotNull] JobStorage storage)
            : this(storage, JobFilterProviders.Providers)
        {
        }

        public PeriodicJobManager([NotNull] JobStorage storage, [NotNull] IJobFilterProvider filterProvider)
            : this(storage, filterProvider, new DefaultTimeZoneResolver())
        {
        }

        public PeriodicJobManager(
            [NotNull] JobStorage storage,
            [NotNull] IJobFilterProvider filterProvider,
            [NotNull] ITimeZoneResolver timeZoneResolver)
            : this(storage, filterProvider, timeZoneResolver, () => DateTime.UtcNow)
        {
        }

        public PeriodicJobManager(
            [NotNull] JobStorage storage,
            [NotNull] IJobFilterProvider filterProvider,
            [NotNull] ITimeZoneResolver timeZoneResolver,
            [NotNull] Func<DateTime> nowFactory)
            : this(storage, new BackgroundJobFactory(filterProvider), timeZoneResolver, nowFactory)
        {
        }

        public PeriodicJobManager([NotNull] JobStorage storage, [NotNull] IBackgroundJobFactory factory)
            : this(storage, factory, new DefaultTimeZoneResolver())
        {
        }

        public PeriodicJobManager([NotNull] JobStorage storage, [NotNull] IBackgroundJobFactory factory, [NotNull] ITimeZoneResolver timeZoneResolver)
            : this(storage, factory, timeZoneResolver, () => DateTime.UtcNow)
        {
        }

        internal PeriodicJobManager(
    [NotNull] JobStorage storage,
    [NotNull] IBackgroundJobFactory factory,
    [NotNull] ITimeZoneResolver timeZoneResolver,
    [NotNull] Func<DateTime> nowFactory)
            : base(storage, factory, timeZoneResolver)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _timeZoneResolver = timeZoneResolver ?? throw new ArgumentNullException(nameof(timeZoneResolver));
            _nowFactory = nowFactory ?? throw new ArgumentNullException(nameof(nowFactory));
        }


        public new void RemoveIfExists(string recurringJobId)
        {
            /*
            if (recurringJobId == null) throw new ArgumentNullException(nameof(recurringJobId));

            using (var connection = _storage.GetConnection())
            using (connection.AcquireDistributedRecurringJobLock(recurringJobId, DefaultTimeout))
            using (var transaction = connection.CreateWriteTransaction())
            {
                transaction.RemoveHash($"recurring-job:{recurringJobId}");
                transaction.RemoveFromSet("recurring-jobs", recurringJobId);
                transaction.RemoveFromSet("recurring-jobs-job", recurringJobId);

                transaction.Commit();
            }
            */
        }
    }
}
