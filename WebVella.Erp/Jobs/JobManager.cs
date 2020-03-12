using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebVella.Erp.Api;
using WebVella.Erp.Database;
using WebVella.Erp.Diagnostics;

namespace WebVella.Erp.Jobs
{
	public class JobManager
	{
		public static JobManager Current { get; private set; }

		public static List<JobType> JobTypes { get; private set; }

		public static JobManagerSettings Settings { get; set; }

		private JobDataService JobService { get; set; }

		private JobManager()
		{
		}

		private JobManager(JobManagerSettings settings)
		{
			Settings = settings;
			JobService = new JobDataService(Settings);

			//Get all jobs with status running and set them to status abort.
			var runningJobs = JobService.GetRunningJobs();

			foreach (var job in runningJobs)
			{
				job.Status = JobStatus.Aborted;
				job.AbortedBy = Guid.Empty; //by system
				job.FinishedOn = DateTime.UtcNow;
				JobService.UpdateJob(job);
			}
		}

		public static void Initialize(JobManagerSettings settings, List<JobType> additionalJobTypes = null)
		{
			JobTypes = new List<JobType>();
			if (additionalJobTypes != null)
			{
				foreach (JobType jt in additionalJobTypes)
					JobTypes.Add(jt);
			}
			Current = new JobManager(settings);
			JobPool.Initialize();
		}

		public void RegisterJobTypes(ErpService service)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
							.Where(a => !(a.FullName.ToLowerInvariant().StartsWith("microsoft.")
								|| a.FullName.ToLowerInvariant().StartsWith("system.")));
			foreach (var assembly in assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (!type.IsSubclassOf(typeof(ErpJob)))
						continue;

					var attributes = type.GetCustomAttributes(typeof(JobAttribute), true);
					if (attributes.Length != 1)
						continue;

					var attribute = attributes[0] as JobAttribute;
					JobType internalJobType = new JobType();
					internalJobType.Id = attribute.Id;
					internalJobType.Name = attribute.Name;
					internalJobType.DefaultPriority = (JobPriority)((int)attribute.DefaultPriority);
					internalJobType.AllowSingleInstance = attribute.AllowSingleInstance;
					internalJobType.CompleteClassName = type.FullName;
					internalJobType.ErpJobType = type;
					RegisterJobType(internalJobType);
				}
			}
		}

		internal bool RegisterJobType(JobType type)
		{
			if (JobTypes.Any(t => t.Name.ToLowerInvariant() == type.Name.ToLowerInvariant()))
			{
				Log log = new Log();
				log.Create(LogType.Error, "JobManager.RegisterType", "Register type failed!", $"Type with name '{type.Name}' already exists.", saveDetailsAsJson: true);
				return false;
			}

			if (!JobTypes.Any(t => t.Id == type.Id))
				JobTypes.Add(type);

			return true;
		}

		public Job CreateJob(Guid typeId, dynamic attributes = null, JobPriority priority = 0, Guid? creatorId = null, Guid? schedulePlanId = null, Guid? jobId = null)
		{
			JobType type = JobTypes.FirstOrDefault(t => t.Id == typeId);
			if (type == null)
			{
				Log log = new Log();
				log.Create(LogType.Error, "JobManager.CreateJob", "Create job failed!", $"Type with id '{typeId}' can not be found.", saveDetailsAsJson: true);
				return null;
			}

			if (!Enum.IsDefined(typeof(JobPriority), priority))
				priority = type.DefaultPriority;

			Job job = new Job();
			job.Id = jobId.HasValue ? jobId.Value : Guid.NewGuid();
			job.TypeId = type.Id;
			job.Type = type;
			job.TypeName = type.Name;
			job.CompleteClassName = type.CompleteClassName;
			job.Status = JobStatus.Pending;
			job.Priority = priority;
			job.Attributes = attributes;
			job.CreatedBy = creatorId;
			job.LastModifiedBy = creatorId;
			job.SchedulePlanId = schedulePlanId;

			return JobService.CreateJob(job);
		}

		public bool UpdateJob(Job job)
		{
			return JobService.UpdateJob(job);
		}

		public Job GetJob(Guid jobId)
		{
			return JobService.GetJob(jobId);
		}

		public List<Job> GetJobs(out int totalCount, DateTime? startFromDate = null, DateTime? startToDate = null, DateTime? finishedFromDate = null, DateTime? finishedToDate = null,
			string typeName = null, int? status = null, int? priority = null, Guid? schedulePlanId = null, int? page = null, int? pageSize = null)
		{
			totalCount = (int)JobService.GetJobsTotalCount(startFromDate, startToDate, finishedFromDate, finishedToDate, typeName, status, priority, schedulePlanId);
			return JobService.GetJobs(startFromDate, startToDate, finishedFromDate, finishedToDate, typeName, status, priority, schedulePlanId, page, pageSize);
		}

		public void ProcessJobsAsync()
		{
			Task.Run(() => Process());
		}

		private void Process()
		{
			if (!Settings.Enabled)
				return;


#if DEBUG
			Thread.Sleep(10000); //Initial sleep time
#else
			Thread.Sleep(120000); //Initial sleep time
#endif

			while (true)
			{
				try
				{
					//If there are free threads in the pool
					if (JobPool.Current.HasFreeThreads)
					{
						//Get pending jobs (limit the count of the returned jobs to be <= to count of the free threads)
						List<Job> pendingJobs = JobService.GetPendingJobs(JobPool.Current.FreeThreadsCount);

						foreach (var job in pendingJobs)
						{
							try
							{
								if (job.Type.AllowSingleInstance && JobPool.Current.HasJobFromTypeInThePool(job.Type.Id))
									continue;

								JobPool.Current.RunJobAsync(job);
							}
							catch (Exception ex)
							{
								try
								{
									DbContext.CreateContext(ErpSettings.ConnectionString);
									using (var secCtx = SecurityContext.OpenSystemScope())
									{
										Log log = new Log();
										string jobId = job != null ? job.Id.ToString() : "null";
										string jobType = job != null && job.Type != null ? job.Type.Name : "null";
										log.Create(LogType.Error, "JobManager.Process", $"Start job with id[{jobId}] and type [{jobType}] failed! ", ex.Message);
									}
								}
								finally
								{
									DbContext.CloseContext();
								}

							}
						}
					}
				}
				catch (Exception ex)
				{
					using (var secCtx = SecurityContext.OpenSystemScope())
					{
						try
						{
							DbContext.CreateContext(Erp.ErpSettings.ConnectionString);

							Log log = new Log();
							log.Create(LogType.Error, "JobManager.Process", ex);
						}
						finally
						{
							DbContext.CloseContext();
						}
					}
				}
				finally
				{
					Thread.Sleep(12000);
				}
			}
		}

		public async void ProcessJobsAsync(CancellationToken stoppingToken)
		{
			if (!Settings.Enabled)
				return;


#if !DEBUG
			try { await Task.Delay(120000, stoppingToken); } catch (TaskCanceledException) { };
#endif

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					//If there are free threads in the pool
					if (JobPool.Current.HasFreeThreads)
					{
						//Get pending jobs (limit the count of the returned jobs to be <= to count of the free threads)
						List<Job> pendingJobs = JobService.GetPendingJobs(JobPool.Current.FreeThreadsCount);

						foreach (var job in pendingJobs)
						{
							try
							{
								if (job.Type.AllowSingleInstance && JobPool.Current.HasJobFromTypeInThePool(job.Type.Id))
									continue;

								JobPool.Current.RunJobAsync(job);
							}
							catch (Exception ex)
							{
								try
								{
									DbContext.CreateContext(ErpSettings.ConnectionString);
									using (var secCtx = SecurityContext.OpenSystemScope())
									{
										Log log = new Log();
										string jobId = job != null ? job.Id.ToString() : "null";
										string jobType = job != null && job.Type != null ? job.Type.Name : "null";
										log.Create(LogType.Error, "JobManager.Process", $"Start job with id[{jobId}] and type [{jobType}] failed! ", ex.Message);
									}
								}
								finally
								{
									DbContext.CloseContext();
								}

							}
						}
					}
				}
				catch (Exception ex)
				{
					using (var secCtx = SecurityContext.OpenSystemScope())
					{
						try
						{
							DbContext.CreateContext(Erp.ErpSettings.ConnectionString);

							Log log = new Log();
							log.Create(LogType.Error, "JobManager.Process", ex);
						}
						finally
						{
							DbContext.CloseContext();
						}
					}
				}
				finally
				{
					try { await Task.Delay(10000, stoppingToken); } catch (TaskCanceledException) { };
				}
			}
		}
	}
}
