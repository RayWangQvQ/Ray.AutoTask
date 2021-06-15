using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Hangfire.Dashboard.Extensions.Resources;
using Hangfire.Dashboard.Pages;
using Hangfire.Dashboard.Resources;
using Hangfire.Storage;

namespace Hangfire.Dashboard.Extensions.Pages
{
    partial class RecurringJobExtensionsPage : IExtensionsPageInfo
    {
        public static string Title => RayStrings.NavigationMenu_RecurringJobsEx;

        public static string PageRoute => "/RecurringJobManage";

        public RecurringJobExtensionsPage()
        {

        }
    }
}
