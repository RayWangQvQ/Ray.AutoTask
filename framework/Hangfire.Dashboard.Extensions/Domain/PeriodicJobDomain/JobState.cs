using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Dashboard.Extensions.Models
{
    public enum JobState
    {
        [Description("已停止")]
        Stoped = 0,

        [Description("运行中")]
        Running = 1
    }
}
