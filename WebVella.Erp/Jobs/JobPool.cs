using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebVella.Erp.Api;
using WebVella.Erp.Database;
using WebVella.Erp.Diagnostics;
using WebVella.Erp.Utilities.Dynamic;

namespace WebVella.Erp.Jobs
{
	public class JobPool
	{
		private static object lockObj = new Object();

		private int MAX_THREADS_POOL_COUNT = 20;

		public static JobPool Current { get; private set; }

		private static List<JobContext> Pool { get; set; }

		public bool HasFreeThreads
		{
			get
			{
				lock (lockObj)
				{
					return Pool.Count < MAX_THREADS_POOL_COUNT;
				}
			}
		}

		public int FreeThreadsCount
		{
			get
			{
				lock (lockObj)
				{
					return MAX_THREADS_POOL_COUNT - Pool.Count;
				}
			}
		}

		private JobPool()
		{
			Pool = new List<JobContext>();
		}

		public static void Initialize()
		{
			Current = new JobPool();
		}

		public void RunJobAsync(Job job)
		{
			//Get pool count and if it is < of max_thread_pool_count create new context and start execute the job in new thread
			bool allowed = false;
			lock (lockObj)
			{
				allowed = !Pool.Any(p => p.JobId == job.Id) && Pool.Count < MAX_THREADS_POOL_COUNT;
			}
			if (allowed)
			{
				//Job does not exists in the pool and the pool has free threads.

				JobContext context = new JobContext();
				context.JobId = job.Id;
				context.Aborted = false;
				context.Priority = job.Priority;
				context.Attributes = job.Attributes;
				context.Type = job.Type;

				Task.Run(() => Process(context));
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

				lock (lockObj)
				{
					Pool.Add(context);
				}

				var instance = (ErpJob)Activator.CreateInstance(context.Type.ErpJobType);


				try
				{
					DbContext.CreateContext(ErpSettings.ConnectionString);
					using (var secCtx = SecurityContext.OpenSystemScope())
					{
						//execute job method
						instance.Execute(context);
					}
				}
				catch (TargetInvocationException ex)
				{
					throw ex.InnerException;
				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					DbContext.CloseContext();
				}

				if (context.Result != null)
					job.Result = context.Result;

				job.FinishedOn = DateTime.UtcNow;
				job.Status = JobStatus.Finished;
				jobService.UpdateJob(job);
			}
			catch (Exception ex)
			{

				try
				{
					DbContext.CreateContext(ErpSettings.ConnectionString);
					using (var secCtx = SecurityContext.OpenSystemScope())
					{
						Log log = new Log();
						log.Create(LogType.Error, $"JobPool.Process.{context.Type.Name}", ex);

						job.FinishedOn = DateTime.UtcNow;
						job.Status = JobStatus.Failed;
						job.ErrorMessage = ex.Message;
						jobService.UpdateJob(job);
					}
				}
				finally
				{
					DbContext.CloseContext();
				}

			}
			finally
			{
				lock (lockObj)
				{
					Pool.Remove(context);
				}
			}
		}


		public void AbortJob(Guid jobId)
		{
			lock (lockObj)
			{
				var context = Pool.FirstOrDefault(j => j.JobId == jobId);

				if (context != null)
					context.Aborted = true;
			}
		}

		public bool HasJobFromTypeInThePool(Guid typeId)
		{
			lock (lockObj)
			{
				return Pool.Any(c => c.Type.Id == typeId);
			}
		}
	}
}
