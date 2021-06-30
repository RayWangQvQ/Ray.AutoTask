using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Annotations;

namespace Hangfire.Dashboard.Extensions.Dispatchers
{
    /// <summary>
    /// [调度器]获取嵌入资源
    /// </summary>
    public class EmbeddedResourceDispatcher : IDashboardDispatcher
    {
        private readonly Assembly _assembly;
        private readonly string _resourceName;
        private readonly string _contentType;

        public EmbeddedResourceDispatcher(
            [NotNull] string contentType,
            [NotNull] Assembly assembly,
            string resourceName)
        {
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            _assembly = assembly;
            _resourceName = resourceName;
            _contentType = contentType;
        }

        public async Task Dispatch(DashboardContext context)
        {
            /*
            context.Response.ContentType = _contentType;
            context.Response.SetExpire(DateTimeOffset.Now.AddYears(1));
            */

            /*
             * 在第一次调用 response.Body.WriteAsync 方法时，HasStarted 属性便会被设置为 True
             * 一旦 HasStarted 设置为 true 后，便不能再修改响应头
             * 因为此时 content-length 的值已经确定，继续写入可能会造成协议冲突
            */

            if (!string.IsNullOrEmpty(_contentType))
            {
                var contentType = context.Response.ContentType;

                // content type 还为设置：
                if (string.IsNullOrEmpty(contentType))
                {
                    context.Response.ContentType = _contentType;
                }
                // content type 已设置，但是与当前不一致：
                else if (contentType != _contentType)
                {
                    throw new InvalidOperationException($"ContentType '{_contentType}' conflicts with '{context.Response.ContentType}'");
                }
            }

            await WriteResponse(context.Response).ConfigureAwait(false);
        }

        protected virtual Task WriteResponse(DashboardResponse response)
        {
            return WriteResource(response, _assembly, _resourceName);
        }

        protected async Task WriteResource(DashboardResponse response, Assembly assembly, string resourceName)
        {
            using (var inputStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (inputStream == null)
                {
                    throw new ArgumentException($@"Resource with name {resourceName} not found in assembly {assembly}.");
                }

                await inputStream.CopyToAsync(response.Body).ConfigureAwait(false);
            }
        }
    }
}
