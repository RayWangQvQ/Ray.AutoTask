using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Dashboard.Extensions.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ConvertTimeZone(this DateTime dateTime, string timeZoneId)
            => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, timeZoneId);
    }
}
