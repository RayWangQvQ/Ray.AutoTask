using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Dashboard.Extensions.Models;
using Hangfire.States;
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

        public void AddOrUpdate(PeriodicJobModel periodicJob)
        {
            var job = periodicJob;
            var timeZone = TimeZoneInfo.Utc;

            /*
            if (!Utility.IsValidSchedule(job.Cron))
            {
                response.Status = false;
                response.Message = "Invalid CRON";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                return;
            }
            */

            try
            {
                if (!string.IsNullOrEmpty(job.TimeZoneId))
                {
                    timeZone = TimeZoneInfo.FindSystemTimeZoneById(job.TimeZoneId);
                }
            }
            catch (Exception ex)
            {

                //response.Status = false;
                //response.Message = ex.Message;

                //await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                return;
            }


            if (!StorageAssemblySingleton.GetInstance().IsValidType(job.Class))
            {
                //response.Status = false;
                //response.Message = "The Class not found";

                //await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                return;
            }

            if (!StorageAssemblySingleton.GetInstance().IsValidMethod(job.Class, job.Method))
            {
                //response.Status = false;
                //response.Message = "The Method not found";

                //await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                return;
            }


            var methodInfo = StorageAssemblySingleton.GetInstance()
                .currentAssembly
                .Where(x => x?.GetType(job.Class)?.GetMethod(job.Method) != null)
                .FirstOrDefault()
                .GetType(job.Class)
                .GetMethod(job.Method);

            AddOrUpdate(
                      job.Id,
                      methodInfo,
                      job.Cron,
                      timeZone,
                      job.Queue ?? EnqueuedState.DefaultQueue);
        }

        /// <summary>
        /// Register RecurringJob via <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="jobId">The identifier of the RecurringJob</param>
        /// <param name="method">the specified method</param>
        /// <param name="cron">Cron expressions</param>
        /// <param name="timeZone"><see cref="TimeZoneInfo"/></param>
        /// <param name="queue">Queue name</param>
        public void AddOrUpdate(string jobId, MethodInfo method, string cron, TimeZoneInfo timeZone, string queue)
        {
            if (jobId == null) throw new ArgumentNullException(nameof(jobId));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (cron == null) throw new ArgumentNullException(nameof(cron));
            if (timeZone == null) throw new ArgumentNullException(nameof(timeZone));
            if (queue == null) throw new ArgumentNullException(nameof(queue));

            var parameters = method.GetParameters();

            Expression[] args = new Expression[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = Expression.Default(parameters[i].ParameterType);
            }

            var x = Expression.Parameter(method.DeclaringType, "x");

            var methodCall = method.IsStatic ? Expression.Call(method, args) : Expression.Call(x, method, args);

            var addOrUpdate = Expression.Call(
                typeof(RecurringJob),
                nameof(RecurringJob.AddOrUpdate),
                new Type[] { method.DeclaringType },
                new Expression[]
                {
                    Expression.Constant(jobId),
                    Expression.Lambda(methodCall, x),
                    Expression.Constant(cron),
                    Expression.Constant(timeZone),
                    Expression.Constant(queue)
                });

            Expression.Lambda(addOrUpdate).Compile().DynamicInvoke();
        }
    }
}
