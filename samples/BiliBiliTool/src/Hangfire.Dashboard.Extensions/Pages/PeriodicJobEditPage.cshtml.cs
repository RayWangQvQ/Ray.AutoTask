using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Dashboard.Extensions.Models;
using Hangfire.Dashboard.Extensions.Repositories;
using Hangfire.Dashboard.Extensions.Resources;
using Hangfire.Dashboard.Resources;

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

        public string Title { get; set; }

        protected virtual void Init()
        {
            JobId = "";
            PeriodicJob = new PeriodicJobModel();

            JobId = Query("id");
            if (string.IsNullOrWhiteSpace(JobId))
            {
                Title = Strings.Common_Created;
            }
            else
            {
                Title = RayStrings.PeriodicJobsPage_Edit;
                PeriodicJob = GetJobById(JobId);
            }
        }

        protected virtual PeriodicJobModel GetJobById(string id)
        {
            var result = _periodicJobRepository.GetPeriodicJobById(id);
            if (result == null) throw new Exception("JobId不存在");
            return result;
        }
    }
}
