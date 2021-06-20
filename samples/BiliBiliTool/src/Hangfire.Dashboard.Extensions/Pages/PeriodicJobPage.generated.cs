﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hangfire.Dashboard.Extensions.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Cronos;
    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.Dashboard.Extensions;
    using Hangfire.Dashboard.Extensions.Models;
    using Hangfire.Dashboard.Extensions.Resources;
    using Hangfire.Dashboard.Pages;
    using Hangfire.Dashboard.Resources;
    using Hangfire.States;
    using Hangfire.Storage;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    internal partial class PeriodicJobPage : RazorPage
    {
#line hidden

        public override void Execute()
        {


WriteLiteral("\r\n");
















  
    Init();


WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"col-md-12\">\r\n        <h1 class=\"page-header\"" +
">");


                           Write(Strings.RecurringJobsPage_Title);

WriteLiteral("</h1>\r\n\r\n        <div class=\"js-jobs-list\">\r\n            <div class=\"btn-toolbar " +
"btn-toolbar-top\">\r\n                ");



WriteLiteral("\r\n");


                 if (!IsReadOnly)
                {

WriteLiteral("                    <button class=\"js-jobs-list-command btn btn-sm btn-primary\"\r\n" +
"                        data-url=\"");


                             Write(Url.To("/recurring/trigger"));

WriteLiteral("\"\r\n                        data-loading-text=\"");


                                      Write(Strings.RecurringJobsPage_Triggering);

WriteLiteral("\"\r\n                        disabled=\"disabled\">\r\n                        <span cl" +
"ass=\"glyphicon glyphicon-play-circle\"></span>\r\n                        ");


                   Write(Strings.RecurringJobsPage_TriggerNow);

WriteLiteral("\r\n                    </button>\r\n");


                }
                

                      


                 if (!IsReadOnly)
                {

WriteLiteral("                    <button class=\"js-jobs-list-command btn btn-sm btn-default\"\r\n" +
"                        data-url=\"");


                             Write(Url.To("/periodic/remove"));

WriteLiteral("\"\r\n                        data-loading-text=\"");


                                      Write(Strings.Common_Deleting);

WriteLiteral("\"\r\n                        data-confirm=\"");


                                 Write(Strings.Common_DeleteConfirm);

WriteLiteral("\"\r\n                        disabled=\"disabled\">\r\n                        <span cl" +
"ass=\"glyphicon glyphicon-remove\"></span>\r\n                        ");


                   Write(Strings.Common_Delete);

WriteLiteral("\r\n                    </button>\r\n");


                }


                 if (pager != null)
                {

WriteLiteral("                    ");

WriteLiteral(" ");


                  Write(Html.PerPageSelector(pager));

WriteLiteral("\r\n");


                }

WriteLiteral("            </div>\r\n\r\n            <div class=\"table-responsive\">\r\n               " +
" <table class=\"table\">\r\n                    <thead>\r\n                        <tr" +
">\r\n");


                             if (!IsReadOnly)
                            {

WriteLiteral("                                <th class=\"min-width\">\r\n                         " +
"           <input type=\"checkbox\" class=\"js-jobs-list-select-all\" />\r\n          " +
"                      </th>\r\n");


                            }

WriteLiteral("                            <th>");


                           Write(Strings.Common_Id);

WriteLiteral("</th>\r\n                            <th class=\"min-width\">");


                                             Write(Strings.RecurringJobsPage_Table_Cron);

WriteLiteral("</th>\r\n                            <th>");


                           Write(Strings.RecurringJobsPage_Table_TimeZone);

WriteLiteral("</th>\r\n                            <th>");


                           Write(RayStrings.PeriodicJobsPage_Table_State);

WriteLiteral("</th>\r\n                            <th>");


                           Write(Strings.Common_Job);

WriteLiteral("</th>\r\n                            <th class=\"align-right min-width\">");


                                                         Write(Strings.RecurringJobsPage_Table_NextExecution);

WriteLiteral("</th>\r\n                            <th class=\"align-right min-width\">");


                                                         Write(Strings.RecurringJobsPage_Table_LastExecution);

WriteLiteral("</th>\r\n                            <th class=\"align-right min-width\">");


                                                         Write(Strings.Common_Created);

WriteLiteral("</th>\r\n                            <th class=\"align-right min-width\">");


                                                         Write(RayStrings.PeriodicJobsPage_Table_Operation);

WriteLiteral("</th>\r\n                        </tr>\r\n                    </thead>\r\n             " +
"       <tbody>\r\n");


                         foreach (var job in periodicJobs)
                        {

WriteLiteral("                            <tr class=\"js-jobs-list-rows\">\r\n                     " +
"           ");



WriteLiteral("\r\n");


                                 if (!IsReadOnly)
                                {

WriteLiteral("                                    <td rowspan=\"");


                                             Write(job.Error != null ? "2" : "1");

WriteLiteral("\">\r\n                                        <input type=\"checkbox\" class=\"js-jobs" +
"-list-checkbox\" name=\"jobs[]\" value=\"");


                                                                                                             Write(job.Id);

WriteLiteral("\" />\r\n                                    </td>\r\n");


                                }
                                

                                      

WriteLiteral("                                <td class=\"word-break width-15\">");


                                                           Write(job.Id);

WriteLiteral("</td>\r\n                                ");



WriteLiteral("\r\n                                <td style=\"min-width: 125px\" class=\"min-width\">" +
"\r\n                                    ");



WriteLiteral("\r\n");


                                      
                                        string cronDescription = null;
                                        bool cronError = false;

                                        if (!String.IsNullOrEmpty(job.Cron))
                                        {
                                            try
                                            {
                                                //RecurringJobEntity.ParseCronExpression(job.Cron);
                                                //通过反射调用
                                                Assembly asm = Assembly.GetAssembly(typeof(Hangfire.RecurringJob));
                                                MethodInfo? mf = asm?.GetType("Hangfire.RecurringJobEntity")
                                                ?.GetMethod("ParseCronExpression", BindingFlags.NonPublic | BindingFlags.Static);
                                                //CronExpression saf = (CronExpression)mf.Invoke(null, null);
                                                mf?.Invoke(null, new[] { job.Cron });
                                            }
                                            catch (Exception ex)
                                            {
                                                cronDescription = ex.Message;
                                                cronError = true;
                                            }

                                            if (cronDescription == null)
                                            {
#if FEATURE_CRONDESCRIPTOR
#endif
                                            }
                                        }
                                    

WriteLiteral("\r\n");


                                     if (cronDescription != null)
                                    {

WriteLiteral("                                        <code title=\"");


                                                Write(cronDescription);

WriteLiteral("\" class=\"cron-badge\">\r\n");


                                             if (cronError)
                                            {

WriteLiteral("                                                <span class=\"glyphicon glyphicon-" +
"exclamation-sign\"></span>\r\n");


                                            }

WriteLiteral("                                            ");


                                       Write(job.Cron);

WriteLiteral("\r\n                                        </code>\r\n");


                                    }
                                    else
                                    {

WriteLiteral("                                        <code class=\"cron-badge\">");


                                                            Write(job.Cron);

WriteLiteral("</code>\r\n");


                                    }

WriteLiteral("                                </td>\r\n                                ");



WriteLiteral("\r\n                                <td>\r\n");


                                     if (!String.IsNullOrWhiteSpace(job.TimeZoneId))
                                    {
                                        string displayName;
                                        Exception exception = null;

                                        try
                                        {
                                            var resolver = DashboardOptions.TimeZoneResolver ?? new DefaultTimeZoneResolver();
                                            displayName = resolver.GetTimeZoneById(job.TimeZoneId).DisplayName;
                                        }
                                        catch (Exception ex)
                                        {
                                            displayName = null;
                                            exception = ex;
                                        }


WriteLiteral("                                        <span title=\"");


                                                Write(displayName);

WriteLiteral("\" data-container=\"body\">\r\n                                            ");


                                       Write(job.TimeZoneId);

WriteLiteral("\r\n");


                                             if (exception != null)
                                            {

WriteLiteral("                                                <span class=\"glyphicon glyphicon-" +
"exclamation-sign\" title=\"");


                                                                                                     Write(exception.Message);

WriteLiteral("\"></span>\r\n");


                                            }

WriteLiteral("                                        </span>\r\n");


                                    }
                                    else
                                    {

WriteLiteral("                                        ");

WriteLiteral(" UTC\r\n");


                                    }

WriteLiteral("                                </td>\r\n                                ");



WriteLiteral("\r\n                                <td>\r\n");


                                     if (job.JobState == "Running")
                                    {

WriteLiteral("                                        <span class=\"label label-success\">");


                                                                     Write(job.JobState);

WriteLiteral("</span>\r\n");


                                    }
                                    else
                                    {

WriteLiteral("                                        <span class=\"label label-danger\">");


                                                                    Write(job.JobState);

WriteLiteral("</span>\r\n");


                                    }

WriteLiteral("                                </td>\r\n                                ");



WriteLiteral("\r\n                                <td class=\"word-break width-30\">\r\n");


                                     if (job.Job != null)
                                    {

WriteLiteral("                                        ");

WriteLiteral(" ");


                                      Write(Html.JobName(job.Job));

WriteLiteral("\r\n");


                                    }
                                    else if (job.LoadException != null && job.LoadException.InnerException != null)
                                    {

WriteLiteral("                                        <em>");


                                       Write(job.LoadException.InnerException.Message);

WriteLiteral("</em>\r\n");


                                    }
                                    else if (job.LoadException != null)
                                    {

WriteLiteral("                                        <em>");


                                       Write(job.LoadException.Message);

WriteLiteral("</em>\r\n");


                                    }
                                    else
                                    {

WriteLiteral("                                        <em>");


                                       Write(Strings.Common_NotAvailable);

WriteLiteral("</em>\r\n");


                                    }

WriteLiteral("                                </td>\r\n                                ");



WriteLiteral("\r\n                                <td class=\"align-right min-width\">\r\n");


                                     if (!job.NextExecution.HasValue)
                                    {
                                        if (job.Error != null)
                                        {

WriteLiteral("                                            <span class=\"label label-danger text-" +
"uppercase\">");


                                                                                       Write(Strings.Common_Error);

WriteLiteral("</span>\r\n");


                                        }
                                        else
                                        {

WriteLiteral("                                            <span class=\"label label-default text" +
"-uppercase\" title=\"");


                                                                                               Write(Strings.RecurringJobsPage_RecurringJobDisabled_Tooltip);

WriteLiteral("\">");


                                                                                                                                                        Write(Strings.Common_Disabled);

WriteLiteral("</span>\r\n");


                                        }

                                    }
                                    else if (job.RetryAttempt > 0)
                                    {

WriteLiteral("                                        <span class=\"label label-warning\">");


                                                                     Write(Html.RelativeTime(job.NextExecution.Value));

WriteLiteral("</span>\r\n");


                                    }
                                    else
                                    {
                                        
                                   Write(Html.RelativeTime(job.NextExecution.Value));

                                                                                   
                                    }

WriteLiteral("                                </td>\r\n                                ");



WriteLiteral("\r\n                                <td class=\"align-right min-width\">\r\n");


                                     if (job.LastExecution != null)
                                    {
                                        if (!String.IsNullOrEmpty(job.LastJobId))
                                        {

WriteLiteral("                                            <a href=\"");


                                                Write(Url.JobDetails(job.LastJobId));

WriteLiteral("\" style=\"text-decoration: none\">\r\n                                               " +
" <span class=\"label label-default label-hover\" style=\"");


                                                                                                 Write($"background-color: {JobHistoryRenderer.GetForegroundStateColor(job.LastJobState ?? EnqueuedState.StateName)};");

WriteLiteral("\">\r\n                                                    ");


                                               Write(Html.RelativeTime(job.LastExecution.Value));

WriteLiteral("\r\n                                                </span>\r\n                      " +
"                      </a>\r\n");


                                        }
                                        else
                                        {

WriteLiteral("                                            <em>\r\n                               " +
"                 ");


                                           Write(Strings.RecurringJobsPage_Canceled);

WriteLiteral(" ");


                                                                               Write(Html.RelativeTime(job.LastExecution.Value));

WriteLiteral("\r\n                                            </em>\r\n");


                                        }
                                    }
                                    else
                                    {

WriteLiteral("                                        <em>");


                                       Write(Strings.Common_NotAvailable);

WriteLiteral("</em>\r\n");


                                    }

WriteLiteral("                                </td>\r\n                                ");



WriteLiteral("\r\n                                <td class=\"align-right min-width\">\r\n");


                                     if (job.CreatedAt != null)
                                    {
                                        
                                   Write(Html.RelativeTime(job.CreatedAt.Value));

                                                                               
                                    }
                                    else
                                    {

WriteLiteral("                                        <em>N/A</em>\r\n");


                                    }

WriteLiteral("                                </td>\r\n                                ");



WriteLiteral("\r\n                                <td style=\"min-width:200px\" class=\"align-right\"" +
">\r\n                                    <button class=\"btn btn-info btn-xs\">Edit<" +
"/button>\r\n");


                                     if (job.JobStateEnum == JobState.Running)
                                    {

WriteLiteral("                                        <button type=\"button\" class=\"js-period-jo" +
"bs-list-command btn btn-warning btn-xs\"\r\n                                       " +
" data-url=\"");


                                             Write(Url.To($"/RecurringJobManage/Stop?jobId={job.Id}"));

WriteLiteral("\">\r\n                                            Stop\r\n                           " +
"             </button>\r\n");



WriteLiteral("                                        <button class=\"js-period-jobs-list-comman" +
"d btn btn-primary btn-xs\">\r\n                                            Excute\r\n" +
"                                        </button>\r\n");



WriteLiteral("                                        <button class=\"btn btn-danger btn-xs\">Del" +
"ete</button>\r\n");


                                    }


                                     if (job.JobStateEnum == JobState.Stoped)
                                    {

WriteLiteral("                                        <button class=\"js-period-jobs-list-comman" +
"d btn btn-success btn-xs\"\r\n                                        data-url=\"");


                                             Write(Url.To($"/RecurringJobManage/Start?jobId={job.Id}"));

WriteLiteral("\">\r\n                                            Start\r\n                          " +
"              </button>\r\n");


                                    }

WriteLiteral("                                </td>\r\n");


                                 if (job.Error != null)
                                {


WriteLiteral("                                <tr>\r\n                                    <td col" +
"span=\"");


                                             Write(IsReadOnly ? "6" : "7");

WriteLiteral("\" class=\"failed-job-details\">\r\n                                        <pre class" +
"=\"stack-trace\"><code>");


                                                                  Write(Html.StackTrace(job.Error));

WriteLiteral("</code></pre>\r\n                                    </td>\r\n                       " +
"         </tr>\r\n");


                                }

WriteLiteral("                            </tr>\r\n");


                        }

WriteLiteral("                    </tbody>\r\n                </table>\r\n            </div>\r\n\r\n");


             if (pager != null)
            {

WriteLiteral("                ");

WriteLiteral(" ");


              Write(Html.Paginator(pager));

WriteLiteral("\r\n");


            }

WriteLiteral("        </div>\r\n        }\r\n    </div>\r\n</div>\r\n\r\n");



WriteLiteral(@"
<script src=""js0""></script>

<script>
    $('.js-jobs-list').each(function () {
        var container = this;

        $(this).on('click', '.js-period-jobs-list-command', function (e) {
            var $this = $(this);

            $this.prop('disabled');
            var loadingDelay = setTimeout(function () {
                $this.button('loading');
            }, 100);

            $.post($this.data('url'), {}, function () {
                clearTimeout(loadingDelay);
                window.location.reload();
            });

            e.preventDefault();
        });
    })

</script>
");


        }
    }
}
#pragma warning restore 1591
