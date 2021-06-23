using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Dashboard.Extensions.Models;

namespace Hangfire.Dashboard.Extensions.Pages
{
    public partial class PeriodicJobEditPage
    {
        protected string JobId = "";

        protected PeriodicJobModel PeriodicJob = new PeriodicJobModel();

        protected virtual void Init()
        {
            JobId = Query("id");
            PeriodicJob = GetJobById(JobId);
        }

        protected virtual PeriodicJobModel GetJobById(string id)
        {
            return new PeriodicJobModel
            {
                Id = id
            };
        }
    }
}
