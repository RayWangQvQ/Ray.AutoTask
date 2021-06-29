using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ray.AutoTask.Options;

namespace Ray.AutoTask.Tasks
{
    public class SignInTask : BaseTask
    {
        public SignInTask(ILogger<SignInTask> logger, IOptionsMonitor<TaskOptions> options)
            : base(logger, options)
        {
        }

        public override HttpContent BuildContent(TaskInfo taskInfo)
        {
            var d = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var t = d.TotalMilliseconds.ToString();
            taskInfo.Content.FormUrlEncodedContent.Dic["t"] = t;

            return base.BuildContent(taskInfo);
        }
    }
}
