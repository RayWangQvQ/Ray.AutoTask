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
    internal partial class PeriodicJobEditPage : RazorPage
    {
#line hidden

        public override void Execute()
        {


WriteLiteral("\r\n");














WriteLiteral("\r\n");


WriteLiteral("\r\n");


  
    //Layout = new LayoutPage("test");
    this.Init();


WriteLiteral("\r\n<div class=\"modal-content\">\r\n    <div class=\"modal-header\">\r\n        <button ty" +
"pe=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hid" +
"den=\"true\">&times;</span></button>\r\n        <h4 class=\"modal-title\" id=\"myModalL" +
"abel\">");


                                             Write(Title);

WriteLiteral("</h4>\r\n    </div>\r\n    <div class=\"modal-body\">\r\n        <form action=\"\" class=\"f" +
"orm-horizontal\">\r\n\r\n            ");



WriteLiteral("\r\n            <div class=\"form-group\">\r\n                <label for=\"\" class=\"col-" +
"sm-2 control-label\">");


                                                        Write(Strings.Common_Id);

WriteLiteral("</label>\r\n                <div class=\"col-sm-9\">\r\n                    <input type" +
"=\"text\" class=\"form-control\"\r\n                           disabled=\"disabled\"\r\n  " +
"                         id=\"modal_Id\"\r\n                           value=\"");


                             Write(this.PeriodicJob.Id);

WriteLiteral("\" />\r\n                </div>\r\n            </div>\r\n\r\n            ");



WriteLiteral("\r\n            <div class=\"form-group\">\r\n                <label for=\"\" class=\"col-" +
"sm-2 control-label\">");


                                                        Write(Strings.RecurringJobsPage_Table_Cron);

WriteLiteral("</label>\r\n                <div class=\"col-sm-9\">\r\n                    <input type" +
"=\"text\" class=\"form-control\"\r\n                           id=\"modal_Cron\"\r\n      " +
"                     value=\"");


                             Write(this.PeriodicJob.Cron);

WriteLiteral("\">\r\n                </div>\r\n            </div>\r\n\r\n            ");



WriteLiteral("\r\n            <div class=\"form-group\">\r\n                <label for=\"\" class=\"col-" +
"sm-2 control-label\">");


                                                        Write(Strings.RecurringJobsPage_Table_TimeZone);

WriteLiteral("</label>\r\n                <div class=\"col-sm-9\">\r\n                    <input type" +
"=\"text\" class=\"form-control\"\r\n                           id=\"modal_TimeZoneId\"\r\n" +
"                           value=\"");


                             Write(PeriodicJob.TimeZoneId);

WriteLiteral("\">\r\n                </div>\r\n            </div>\r\n\r\n            ");



WriteLiteral("\r\n            <div class=\"form-group\">\r\n                <label for=\"\" class=\"col-" +
"sm-2 control-label\">");


                                                        Write(Strings.Common_Enqueued);

WriteLiteral("</label>\r\n                <div class=\"col-sm-9\">\r\n                    <input type" +
"=\"text\" class=\"form-control\"\r\n                           id=\"modal_Queue\"\r\n     " +
"                      value=\"");


                             Write(PeriodicJob.Queue);

WriteLiteral(@""">
                </div>
            </div>

            <div class=""form-group"">
                <label for="""" class=""col-sm-2 control-label"">Class</label>
                <div class=""col-sm-9"">
                    <input type=""text"" class=""form-control""
                           id=""modal_Class""
                           value=""");


                             Write(PeriodicJob.Class);

WriteLiteral(@""">
                </div>
            </div>
            <div class=""form-group"">
                <label for="""" class=""col-sm-2 control-label"">Method</label>
                <div class=""col-sm-9"">
                    <input type=""text"" class=""form-control""
                           id=""modal_Method""
                           value=""");


                             Write(PeriodicJob.Method);

WriteLiteral(@""">
                </div>
            </div>
        </form>
    </div>
    <div class=""modal-footer"">
        <button type=""button"" class=""btn btn-default"" data-dismiss=""modal"">Close</button>
        <button type=""button"" class=""btn btn-primary"">Save changes</button>
    </div>
</div>


<script src=""");


        Write(Url.To("/js0"));

WriteLiteral("\"></script>\r\n<script src=\"");


        Write(Url.To("/js-ext0"));

WriteLiteral("\"></script>\r\n");


        }
    }
}
#pragma warning restore 1591