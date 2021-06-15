using Hangfire.Common;
using Hangfire.Dashboard.Extensions.Extensions;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;

namespace Hangfire.Dashboard.Extensions
{
    public static class PeriodicJobAgent
    {
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

        public static void StartPeriodicJob(string JobId)
        {
            using (var connection = JobStorage.Current.GetConnection())
            using (var transaction = connection.CreateWriteTransaction())
            {
                transaction.RemoveFromSet(tagStopJob, JobId);
                transaction.AddToSet(tagRecurringJobs, JobId);
                transaction.Commit();
            }
        }
        public static void StopPeriodicJob(string JobId)
        {
            using (var connection = JobStorage.Current.GetConnection())
            using (var transaction = connection.CreateWriteTransaction())
            {
                transaction.RemoveFromSet(tagRecurringJobs, JobId);
                transaction.AddToSet($"{tagStopJob}", JobId);
                transaction.Commit();
            }
        }

        public static List<PeriodicJobModel> GetStoppedPeriodicJobs()
        {
            var outPut = new List<PeriodicJobModel>();
            using (var connection = JobStorage.Current.GetConnection())
            {
                var allJobStopped = connection.GetAllItemsFromSet(tagStopJob);

                allJobStopped.ToList().ForEach(jobId =>
                {
                    var dto = new PeriodicJobModel();

                    var dataJob = connection.GetAllEntriesFromHash($"{tagRecurringJob}:{jobId}");
                    dto.Id = jobId;
                    dto.TimeZoneId = "UTC"; // Default

                    try
                    {
                        if (dataJob.TryGetValue("Job", out var payload) && !String.IsNullOrWhiteSpace(payload))
                        {
                            var invocationData = InvocationData.DeserializePayload(payload);
                            var job = invocationData.DeserializeJob();
                            dto.Method = job.Method.Name;
                            dto.Class = job.Type.Name;
                        }
                    }
                    catch (JobLoadException ex)
                    {
                        dto.Error = ex.Message;
                    }

                    if (dataJob.ContainsKey("TimeZoneId"))
                    {
                        dto.TimeZoneId = dataJob["TimeZoneId"];
                    }

                    if (dataJob.ContainsKey("NextExecution"))
                    {
                        var tempNextExecution = JobHelper.DeserializeNullableDateTime(dataJob["NextExecution"]);

                        dto.NextExecution = tempNextExecution.HasValue ? tempNextExecution.Value.ConvertTimeZone(dto.TimeZoneId).ToString("G") : "N/A";
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

                    if (dataJob.ContainsKey("LastExecution"))
                    {

                        var tempLastExecution = JobHelper.DeserializeNullableDateTime(dataJob["LastExecution"]);

                        dto.LastExecution = tempLastExecution.HasValue ? tempLastExecution.Value.ConvertTimeZone(dto.TimeZoneId).ToString("G") : "N/A";
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

                    dto.Removed = false;
                    dto.JobState = "Stopped";

                    outPut.Add(dto);

                });
            }
            return outPut;
        }

        public static bool IsValidJobId(string jobId, string tag = tagRecurringJob)
        {
            var result = false;
            using (var connection = JobStorage.Current.GetConnection())
            {
                var job = connection.GetAllEntriesFromHash($"{tag}:{jobId}");

                result = job != null;
            }
            return result;
        }


    }
}
