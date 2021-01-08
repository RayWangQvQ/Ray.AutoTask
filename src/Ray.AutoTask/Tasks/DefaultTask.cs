using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ray.AutoTask.Options;

namespace Ray.AutoTask.Tasks
{
    public class DefaultTask : BaseTask
    {
        public DefaultTask(ILogger<SignInTask> logger, IOptionsMonitor<TaskOptions> options)
            : base(logger, options)
        {
        }
    }
}
