using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Database;
using WebVella.ERP.Diagnostics;
using WebVella.ERP.Utilities.Dynamic;

namespace WebVella.ERP.Jobs
{
	public class JobPool
	{
		private int MAX_THREADS_POOL_COUNT = 20;

		public static JobPool Current { get; private set; }

		public static List<JobContext> Pool { get; private set; }

		public bool HasFreeThreads { get { return Pool.Count < MAX_THREADS_POOL_COUNT; } }

		public int FreeThreadsCount { get { return MAX_THREADS_POOL_COUNT - Pool.Count; } }

		private JobPool()
		{
			Pool = new List<JobContext>();
		}

		public static void Initialize()
		{
			Current = new JobPool();
		}

		public async void RunJobAsync(Job job)
		{
			//Get pool count and if it is < of max_thread_pool_count create new context and start execute the job in new thread

			if (!Pool.Any(p => p.JobId == job.Id) && Pool.Count < MAX_THREADS_POOL_COUNT)
			{
				//Job does not exists in the pool and the pool has free threads.

				JobContext context = new JobContext();
				context.JobId = job.Id;
				context.Aborted = false;
				context.Priority = job.Priority;
				context.Attributes = job.Attributes;
				context.Type = job.Type;

				await Task.Run(() => Process(context));
			}
		}

		public void Process(JobContext context)
		{
			JobDataService jobService = new JobDataService(JobManager.Settings);
			Job job = new Job();
			job.Id = context.JobId;
			job.StartedOn = DateTime.UtcNow;
			job.Status = JobStatus.Running;

			try
			{
				jobService.UpdateJob(job);
				Pool.Add(context);

				var assemblies = AppDomain.CurrentDomain.GetAssemblies();
				Assembly assembly = assemblies.FirstOrDefault(a => a.GetName().Name == context.Type.Assembly);

				if (assembly == null)
				{
					//log error
					throw new Exception("Assembly can not be found!");
				}

				Type type = assembly.GetType(context.Type.CompleteClassName);
				if (type == null)
					throw new Exception($"Type with name '{context.Type.CompleteClassName}' does not exist in assembly {assembly.FullName}");

				var method = type.GetMethod(context.Type.MethodName);
				if (method == null)
					throw new Exception($"Method with name '{context.Type.MethodName}' does not exist in assembly {assembly.FullName}");

				using (var secCtx = SecurityContext.OpenSystemScope())
				{
					try
					{
						DbContext.CreateContext(Settings.ConnectionString);
						//execute job method
						method.Invoke(new DynamicObjectCreater(type).CreateInstance(), new object[] { context });
					}
					finally
					{
						DbContext.CloseContext();
					}
				}

				job.FinishedOn = DateTime.UtcNow;
				job.Status = JobStatus.Finished;
				jobService.UpdateJob(job);
			}
			catch (Exception ex)
			{
				using (var secCtx = SecurityContext.OpenSystemScope())
				{
					try
					{
						DbContext.CreateContext(Settings.ConnectionString);

						Log log = new Log();
						string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
						log.Create(LogType.Error, "Background job process", message, ex.StackTrace);

						job.FinishedOn = DateTime.UtcNow;
						job.Status = JobStatus.Failed;
						job.ErrorMessage = message;
						jobService.UpdateJob(job);
					}
					finally
					{
						DbContext.CloseContext();
					}
				}
			}
			finally
			{
				Pool.Remove(context);
			}
		}

		public void AbortJob(Guid jobId)
		{
			var context = Pool.FirstOrDefault(j => j.JobId == jobId);

			if (context != null)
				context.Aborted = true;
		}
	}
}
