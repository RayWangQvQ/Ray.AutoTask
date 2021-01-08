using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ray.AutoTask.Enums;
using Ray.AutoTask.Extensions;
using Ray.AutoTask.Options;
using Ray.Infrastructure.Extensions;

namespace Ray.AutoTask.Tasks
{
    public abstract class BaseTask
    {
        private readonly ILogger _logger;

        public BaseTask(ILogger logger, IOptionsMonitor<TaskOptions> options)
        {
            _logger = logger;
            TaskOptions = options.CurrentValue;

            Client = new HttpClient();
        }

        public HttpClient Client { get; set; }

        public TaskOptions TaskOptions { get; set; }

        public virtual void DoTask(TaskInfo taskInfo)
        {
            if (!taskInfo.Open) return;

            _logger.LogInformation("---开始【{name}】---", taskInfo.Name);

            Uri apiUri = new Uri(taskInfo.Api);

            //头
            BuildHeaders(taskInfo);

            //cookie
            BuildCookies(taskInfo);

            HttpResponseMessage httpResponseMessage = null;

            if (taskInfo.HttpMethod == HttpMethod.Get)
            {
                httpResponseMessage = Client.GetAsync(apiUri).GetAwaiter().GetResult();
            }

            else if (taskInfo.HttpMethod == HttpMethod.Post)
            {
                var content = BuildContent(taskInfo);

                httpResponseMessage = Client.PostAsync(apiUri, content).GetAwaiter().GetResult();
            }
            else
            {
                throw new Exception("暂不支持");
            }


            var re = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            _logger.LogInformation("返回：{re}", re);

            _logger.LogInformation("---【{name}】结束---", taskInfo.Name);
        }

        /// <summary>
        /// 请求头
        /// </summary>
        public virtual void BuildHeaders(TaskInfo taskInfo)
        {
            if (taskInfo.Headers.Remove.RemoveAll) return;
            Client.DefaultRequestHeaders.AddHeaders(TaskOptions.DefaultHeaders, taskInfo.Headers?.Add, taskInfo.Headers?.Remove?.List);
        }

        /// <summary>
        /// Cookie
        /// </summary>
        public virtual void BuildCookies(TaskInfo taskInfo)
        {
            if (taskInfo.Cookies.Remove.RemoveAll) return;

            var dic = TaskOptions.DefaultCookies;
            dic.Merge(taskInfo.Cookies?.Add, taskInfo.Cookies?.Remove?.List);

            string cookieStr = "";
            foreach (var item in dic)
            {
                cookieStr += $"{item.Key}={item.Value}; ";
            }
            Client.DefaultRequestHeaders.Add("Cookie", cookieStr.Trim());
        }

        /// <summary>
        /// Content
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        public virtual HttpContent BuildContent(TaskInfo taskInfo)
        {
            HttpContent httpContent = null;
            switch (taskInfo.Content.Type)
            {
                case MyContentType.StringContent:
                    httpContent = new StringContent(taskInfo.Content.StringContent.Content);
                    break;
                case MyContentType.FormUrlEncodedContent:
                    var dic = taskInfo.Content.FormUrlEncodedContent.Dic;
                    httpContent = new FormUrlEncodedContent(dic);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return httpContent;
        }
    }
}
