using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        protected PeriodicJob PeriodicJob;

        public string Title { get; set; }

        protected bool IsEdit => !string.IsNullOrWhiteSpace(JobId);

        /// <summary>
        /// 时区下拉列表
        /// </summary>
        protected Dictionary<string, string> TimeZones { get; set; }

        protected virtual string DefaultTimeZoneId => "UTC";

        protected virtual void Init()
        {
            JobId = "";
            PeriodicJob = null;

            TimeZones = GetTimeZones();

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

        protected virtual PeriodicJob GetJobById(string id)
        {
            var result = _periodicJobRepository.GetPeriodicJobById(id);
            if (result == null) throw new Exception("JobId不存在");
            return result;
        }

        protected virtual Dictionary<string, string> GetTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones().OrderBy(x => x.DisplayName).ToDictionary(k => k.Id, v => v.DisplayName);
        }
    }
}
