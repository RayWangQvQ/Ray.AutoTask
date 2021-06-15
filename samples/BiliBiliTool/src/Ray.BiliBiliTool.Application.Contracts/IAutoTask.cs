using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ray.BiliBiliTool.Application.Contracts
{
    public interface IAutoTask
    {
        public int RandomSleepMaxMin { get; }

        public void DoTask();

        public Task DoTaskAsync();
    }
}
