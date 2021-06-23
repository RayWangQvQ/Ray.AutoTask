using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Dashboard.Extensions.Models;
using Hangfire.Storage;

namespace Hangfire.Dashboard.Extensions.Repositories
{
    public partial class PeriodicJobRepository
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

        public List<PeriodicJobModel> GetPeriodicJobs(JobState? jobState = null)
        {
            return jobState switch
            {
                JobState.Stoped => GetStoppedPeriodicJobs(),
                JobState.Running => GetRunningPeriodicJobs(),
                _ => GetAllPeriodicJobs(),
            };
        }

        public PeriodicJobModel GetPeriodicJobById(string id)
        {
            List<PeriodicJobModel> list = GetPeriodicJobs();
            return list.FirstOrDefault(x => x.Id == id);
        }

        #region 私有方法
        private static IStorageConnection GetConnection() => JobStorage.Current.GetConnection();

        /// <summary>
        /// 获取已停止的周期作业
        /// </summary>
        /// <returns></returns>
        private List<PeriodicJobModel> GetStoppedPeriodicJobs()
        {
            var outPut = new List<PeriodicJobModel>();
            using (IStorageConnection connection = GetConnection())
            {
                HashSet<string> allJobStopped = connection.GetAllItemsFromSet(tagStopJob);

                allJobStopped.ToList().ForEach(jobId =>
                {
                    outPut.Add(PeriodicJobModel.Create(jobId, connection, JobState.Stoped));

                });
            }
            return outPut;
        }

        private List<PeriodicJobModel> GetRunningPeriodicJobs()
        {
            var outPut = new List<PeriodicJobModel>();
            using (IStorageConnection connection = GetConnection())
            {
                HashSet<string> allJobStopped = connection.GetAllItemsFromSet(tagRecurringJobs);

                allJobStopped.ToList().ForEach(jobId =>
                {
                    outPut.Add(PeriodicJobModel.Create(jobId, connection, JobState.Running));
                });
            }
            return outPut;
        }

        private List<PeriodicJobModel> GetAllPeriodicJobs()
        {
            var result = new List<PeriodicJobModel>();

            result.AddRange(this.GetRunningPeriodicJobs());
            result.AddRange(this.GetStoppedPeriodicJobs());

            return result;
        }
        #endregion
    }

    public partial class PeriodicJobRepository
    {
        /// <summary>
        /// 启动周期作业
        /// </summary>
        /// <param name="jobId"></param>
        public void StartPeriodicJob(string jobId)
        {
            using (IStorageConnection connection = GetConnection())
            using (IWriteOnlyTransaction transaction = connection.CreateWriteTransaction())
            {
                transaction.RemoveFromSet(tagStopJob, jobId);
                transaction.AddToSet(tagRecurringJobs, jobId);
                transaction.Commit();
            }
        }

        /// <summary>
        /// 停止周期作业
        /// </summary>
        /// <param name="JobId"></param>
        public void StopPeriodicJob(string JobId)
        {
            using (IStorageConnection connection = GetConnection())
            using (IWriteOnlyTransaction transaction = connection.CreateWriteTransaction())
            {
                transaction.RemoveFromSet(tagRecurringJobs, JobId);
                transaction.AddToSet($"{tagStopJob}", JobId);
                transaction.Commit();
            }
        }
    }
}
