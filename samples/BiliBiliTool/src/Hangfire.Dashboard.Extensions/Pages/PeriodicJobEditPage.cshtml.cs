using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Dashboard.Extensions.Models;
using Hangfire.Dashboard.Extensions.Repositories;

namespace Hangfire.Dashboard.Extensions.Pages
{
    partial class PeriodicJobEditPage
    {
        private readonly PeriodicJobRepository _periodicJobRepository;

        public PeriodicJobEditPage()
        {
            _periodicJobRepository = new PeriodicJobRepository();
        }

        protected string JobId = "";

        protected PeriodicJobModel PeriodicJob = new PeriodicJobModel();

        protected virtual void Init()
        {
            JobId = Query("id");
            PeriodicJob = GetJobById(JobId);
        }

        protected virtual PeriodicJobModel GetJobById(string id)
        {
            return _periodicJobRepository.GetPeriodicJobById(id);
        }
    }
}
