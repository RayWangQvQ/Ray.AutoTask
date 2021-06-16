using Hangfire.Common;
using Hangfire.Dashboard.Extensions.Extensions;
using Hangfire.Dashboard.Extensions.Models;
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

        /// <summary>
        /// 启动周期作业
        /// </summary>
        /// <param name="JobId"></param>
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

        /// <summary>
        /// 停止周期作业
        /// </summary>
        /// <param name="JobId"></param>
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

        /// <summary>
        /// 获取已停止的周期作业
        /// </summary>
        /// <returns></returns>
        public static List<PeriodicJobModel> GetStoppedPeriodicJobs()
        {
            var outPut = new List<PeriodicJobModel>();
            using (var connection = JobStorage.Current.GetConnection())
            {
                var allJobStopped = connection.GetAllItemsFromSet(tagStopJob);

                allJobStopped.ToList().ForEach(jobId =>
                {
                    outPut.Add(PeriodicJobModel.Create(jobId, connection, JobState.Stoped));

                });
            }
            return outPut;
        }

        public static List<PeriodicJobModel> GetRunningPeriodicJobs()
        {
            var outPut = new List<PeriodicJobModel>();
            using (IStorageConnection connection = JobStorage.Current.GetConnection())
            {
                var allJobStopped = connection.GetAllItemsFromSet(tagRecurringJobs);

                allJobStopped.ToList().ForEach(jobId =>
                {
                    outPut.Add(PeriodicJobModel.Create(jobId, connection, JobState.Running));
                });
            }
            return outPut;
        }

        public static List<PeriodicJobModel> GetPeriodicJobs(JobState jobState)
        {
            //获取所有开启的周期作业
            return jobState == JobState.Running
                ? GetRunningPeriodicJobs()
                : GetStoppedPeriodicJobs();
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
