using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ray.BiliBiliTool.Application.Contracts;

namespace Ray.BiliBiliTool.Application
{
    public abstract class AutoTaskAppService : AppService, IAutoTask
    {

        public AutoTaskAppService(
            ILogger logger,
            int? randomSleepMaxMinSelf,
            int randomSleepMaxMinGlobal
            ) : base(logger)
        {
            if (randomSleepMaxMinSelf.HasValue) this.RandomSleepMaxMin = randomSleepMaxMinSelf.Value;
            else this.RandomSleepMaxMin = randomSleepMaxMinGlobal;
        }

        public int RandomSleepMaxMin { get; }

        public abstract void DoTask();

        public virtual Task DoTaskAsync()
        {
            DoTask();
            return Task.CompletedTask;
        }

        protected void RandomSleep()
        {
            if (this.RandomSleepMaxMin <= 0) return;

            int random = new Random().Next(0, RandomSleepMaxMin + 1);
            Logger.LogInformation("随机睡眠{random分钟...", random);
            Task.Delay(random).Wait();
        }
    }
}
