using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Dashboard.Extensions.Dispatchers
{
    /// <summary>
    /// [调度器] 组合多个调度器的输出
    /// 用于 <see cref="RouteCollectionExtensions.Append"/>.
    /// </summary>
    internal class CompositeDispatcher : IDashboardDispatcher
    {
        private readonly List<IDashboardDispatcher> _dispatchers;

        public CompositeDispatcher(params IDashboardDispatcher[] dispatchers)
        {
            _dispatchers = new List<IDashboardDispatcher>(dispatchers);
        }

        public void AddDispatcher(IDashboardDispatcher dispatcher)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));

            _dispatchers.Add(dispatcher);
        }

        public async Task Dispatch(DashboardContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (_dispatchers.Count == 0)
                throw new InvalidOperationException("CompositeDispatcher should contain at least one dispatcher");

            foreach (var dispatcher in _dispatchers)
            {
                await dispatcher.Dispatch(context);
            }
        }
    }
}
