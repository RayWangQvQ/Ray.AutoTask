using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cronos;
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

        public List<PeriodicJob> GetPeriodicJobs(JobState? jobState = null)
        {
            return jobState switch
            {
                JobState.Stoped => GetStoppedPeriodicJobs(),
                JobState.Running => GetRunningPeriodicJobs(),
                _ => GetAllPeriodicJobs(),
            };
        }

        public PeriodicJob GetPeriodicJobById(string id)
        {
            List<PeriodicJob> list = GetPeriodicJobs();
            return list.FirstOrDefault(x => x.Id == id);
        }

        #region 私有方法
        private static IStorageConnection GetConnection() => JobStorage.Current.GetConnection();

        /// <summary>
        /// 获取已停止的周期作业
        /// </summary>
        /// <returns></returns>
        private List<PeriodicJob> GetStoppedPeriodicJobs()
        {
            var outPut = new List<PeriodicJob>();
            using (IStorageConnection connection = GetConnection())
            {
                HashSet<string> allJobStopped = connection.GetAllItemsFromSet(tagStopJob);

                allJobStopped.ToList().ForEach(jobId =>
                {
                    outPut.Add(PeriodicJob.Create(jobId, connection, JobState.Stoped));

                });
            }

            //排序
            outPut = outPut.OrderByDescending(x => x.CreatedAt).ToList();

            return outPut;
        }

        private List<PeriodicJob> GetRunningPeriodicJobs()
        {
            var outPut = new List<PeriodicJob>();
            using (IStorageConnection connection = GetConnection())
            {
                HashSet<string> allJobStopped = connection.GetAllItemsFromSet(tagRecurringJobs);

                allJobStopped.ToList().ForEach(jobId =>
                {
                    outPut.Add(PeriodicJob.Create(jobId, connection, JobState.Running));
                });
            }

            //排序
            outPut = outPut.OrderByDescending(x => x.CreatedAt).ToList();

            return outPut;
        }

        private List<PeriodicJob> GetAllPeriodicJobs()
        {
            var result = new List<PeriodicJob>();

            result.AddRange(this.GetRunningPeriodicJobs());
            result.AddRange(this.GetStoppedPeriodicJobs());

            //排序
            result = result.OrderByDescending(x => x.CreatedAt).ToList();

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

        public void AddOrUpdate(PeriodicJob periodicJob)
        {
            var job = periodicJob;
            var timeZone = TimeZoneInfo.Utc;

            //验证Cron
            CronExpression.Parse(periodicJob.Cron);

            //验证时区
            if (!string.IsNullOrEmpty(job.TimeZoneId))
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(job.TimeZoneId);
            }

            //验证ClassName
            var classType = StorageAssemblySingleton.GetInstance().GetClassType(job.ClassFullName);
            if (classType == null)
            {
                throw new Exception($"Class Name :{job.ClassFullName} 不存在");
            }

            //验证MethodName
            var methodInfo = StorageAssemblySingleton.GetInstance().GetMethodInfo(job.ClassFullName, job.MethodName);
            if (methodInfo == null)
            {
                throw new Exception($"Method Name :{job.MethodName} 不存在");
            }

            AddOrUpdate(
                      job.Id,
                      classType,
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
        public void AddOrUpdate(string jobId, Type classType, MethodInfo method, string cron, TimeZoneInfo timeZone, string queue)
        {
            if (jobId == null) throw new ArgumentNullException(nameof(jobId));
            if (classType == null) throw new ArgumentNullException(nameof(classType));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (cron == null) throw new ArgumentNullException(nameof(cron));
            if (timeZone == null) throw new ArgumentNullException(nameof(timeZone));
            if (queue == null) throw new ArgumentNullException(nameof(queue));

            ParameterInfo[] parameters = method.GetParameters();
            Expression[] args = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = Expression.Default(parameters[i].ParameterType);
            }

            ParameterExpression parameterExpression = Expression.Parameter(classType, "parameterExpression");

            MethodCallExpression methodCall = method.IsStatic
                ? Expression.Call(method, args)
                : Expression.Call(parameterExpression, method, args);

            MethodCallExpression addOrUpdate = Expression.Call(
                type: typeof(RecurringJob),
                methodName: nameof(RecurringJob.AddOrUpdate),
                //typeArguments: new Type[] { method.DeclaringType },
                typeArguments: new Type[] { classType },
                arguments: new Expression[]
                {
                    Expression.Constant(jobId),
                    Expression.Lambda(methodCall, parameterExpression),
                    Expression.Constant(cron),
                    Expression.Constant(timeZone),
                    Expression.Constant(queue)
                });

            Expression.Lambda(addOrUpdate).Compile().DynamicInvoke();
        }
    }
}
