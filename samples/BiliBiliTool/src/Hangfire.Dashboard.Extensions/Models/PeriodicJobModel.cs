using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Hangfire.Common;
using Hangfire.Dashboard.Extensions.Extensions;
using Hangfire.Dashboard.Extensions.Models;
using Hangfire.Storage;

namespace Hangfire.Dashboard.Extensions.Models
{

    /// <summary>
    /// It is used to build <see cref="RecurringJob"/> 
    /// with <see cref="IRecurringJobBuilder.Build(Func{System.Collections.Generic.IEnumerable{RecurringJobInfo}})"/>.
    /// </summary>
    public class PeriodicJobModel
    {
        public string Id { get; set; }
        public string Cron { get; set; }
        public string Queue { get; set; }

        public Hangfire.Common.Job Job { get; set; }
        public string Class => Job.Method.Name;
        public string Method => Job.Type.Name;

        public JobLoadException LoadException { get; set; }

        public string JobState => JobStateEnum.ToString();
        public JobState JobStateEnum { get; set; }

        public DateTime? LastExecution { get; set; }
        public DateTime? NextExecution { get; set; }
        public string LastJobId { get; set; }
        public string LastJobState { get; set; }

        public DateTime? CreatedAt { get; set; }
        public bool Removed { get; set; }
        public string TimeZoneId { get; set; }

        public string Error { get; set; }
        public int RetryAttempt { get; set; }


        /// <summary>
        /// Hash表中周期作业key的前缀
        /// </summary>
        public const string tagRecurringJob = "recurring-job";

        /// <summary>
        /// Set表中周期作业的key
        /// </summary>
        public const string tagRecurringJobs = tagRecurringJob + "s";

        /// <summary>
        /// Set表中已停止的周期作业的key
        /// </summary>
        public const string tagStopJob = "recurring-jobs-stop";

        public static PeriodicJobModel Create(string jobId, IStorageConnection connection, JobState jobState)
        {
            var dto = new PeriodicJobModel();

            Dictionary<string, string> dataJob = connection.GetAllEntriesFromHash($"{tagRecurringJob}:{jobId}");

            dto.Id = jobId;
            dto.Cron = dataJob["Cron"];
            dto.TimeZoneId = "UTC"; // Default

            try
            {
                if (dataJob.TryGetValue("Job", out string str4) && !string.IsNullOrWhiteSpace(str4))
                {
                    dto.Job = InvocationData.DeserializePayload(str4).DeserializeJob();
                }
            }
            catch (JobLoadException ex)
            {
                dto.LoadException = ex;
            }

            if (dataJob.ContainsKey("TimeZoneId"))
            {
                dto.TimeZoneId = dataJob["TimeZoneId"];
            }

            if (dataJob.ContainsKey("NextExecution"))
            {
                dto.NextExecution = JobHelper.DeserializeNullableDateTime(dataJob["NextExecution"]);
            }
            if (dataJob.ContainsKey("LastExecution"))
            {
                dto.LastExecution = JobHelper.DeserializeNullableDateTime(dataJob["LastExecution"]);
            }

            if (dataJob.ContainsKey("LastJobId") && !string.IsNullOrWhiteSpace(dataJob["LastJobId"]))
            {
                dto.LastJobId = dataJob["LastJobId"];

                var stateData = connection.GetStateData(dto.LastJobId);
                if (stateData != null)
                {
                    dto.LastJobState = stateData.Name;
                }
            }

            if (dataJob.ContainsKey("Queue"))
            {
                dto.Queue = dataJob["Queue"];
            }

            if (dataJob.ContainsKey("CreatedAt"))
            {
                dto.CreatedAt = JobHelper.DeserializeNullableDateTime(dataJob["CreatedAt"]);
                dto.CreatedAt = dto.CreatedAt.HasValue ? dto.CreatedAt.Value.ConvertTimeZone(dto.TimeZoneId) : new DateTime();
            }

            if (dataJob.TryGetValue("Error", out var error) && !String.IsNullOrEmpty(error))
            {
                dto.Error = error;
            }

            dto.RetryAttempt = (!dataJob.TryGetValue("RetryAttempt", out string retryAttemptStr) || !int.TryParse(retryAttemptStr, out int retryAttemptInt))
                ? 0
                : retryAttemptInt;

            dto.Removed = false;
            dto.JobStateEnum = jobState;

            return dto;
        }
    }

}
