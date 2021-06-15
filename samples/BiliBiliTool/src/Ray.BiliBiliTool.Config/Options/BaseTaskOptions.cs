using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ray.BiliBiliTool.Config.Options
{
    public class BaseTaskOptions
    {
        public string Cron { get; set; }

        public int RandomSleepMaxMin { get; set; }
    }
}
