using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Dashboard.Extensions.Pages
{
    public interface IExtensionsPageInfo
    {
        /// <summary>
        /// 标题
        /// </summary>
        static string Title { get; }

        /// <summary>
        /// 路由地址
        /// </summary>
        static string PageRoute { get; }
    }
}
