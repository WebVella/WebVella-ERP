using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Database;
using WebVella.ERP.Diagnostics;

namespace WebVella.ERP.Jobs
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
			//Register default job types
			LoadDefaultTypes();

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

		public static void Initialize(JobManagerSettings settings)
		{
			JobTypes = new List<JobType>();
			Current = new JobManager(settings);
			JobPool.Initialize();
		}

		private void LoadDefaultTypes()
		{
			//Test job type
			//JobType sendEmailType = new JobType();
			//sendEmailType.Id = new Guid("70f06b11-2aee-40d5-b8ef-de1a2d8bbb59");
			//sendEmailType.Name = "Email sender";
			//sendEmailType.DefaultPriority = JobPriority.Low;
			//sendEmailType.Assembly = "WebVella.ERP";
			//sendEmailType.CompleteClassName = "WebVella.ERP.Jobs.JobManager";
			//sendEmailType.MethodName = "Test";
			//sendEmailType.AllowSingleInstance = false;

			//RegisterType(sendEmailType);
		}

		public bool RegisterType(JobType type)
		{
			if (JobTypes.Any(t => t.Name.ToLowerInvariant() == type.Name.ToLowerInvariant()))
			{
				Log log = new Log();
				log.Create(LogType.Error, "JobManager.RegisterType", "Register type failed!", $"Type with name '{type.Name}' already exists.", saveDetailsAsJson: true);
				return false;
			}

			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Assembly assembly = assemblies.FirstOrDefault(a => a.GetName().Name == type.Assembly);

			if (assembly == null)
			{
				//log error "Assembly can not be found!"
				Log log = new Log();
				log.Create(LogType.Error, "JobManager.RegisterType", "Register type failed!", $"Assembly with name '{type.Assembly}' can not be found.", saveDetailsAsJson: true);
				return false;
			}

			Type assemblyType = assembly.GetType(type.CompleteClassName);
			if (assemblyType == null)
			{
				Log log = new Log();
				log.Create(LogType.Error, "JobManager.RegisterType", "Register type failed!", $"Type with name '{type.CompleteClassName}' does not exist in assembly {assembly.FullName}.", saveDetailsAsJson: true);
				return false;
			}

			var method = assemblyType.GetMethod(type.MethodName);
			if (method == null)
			{
				Log log = new Log();
				log.Create(LogType.Error, "JobManager.RegisterType", "Register type failed!", $"Method with name '{type.MethodName}' does not exist in assembly {assembly.FullName}.", saveDetailsAsJson: true);
				return false;
			}


			if (!JobTypes.Any(t => t.Id == type.Id))
				JobTypes.Add(type);

			return true;
		}

		public void RegisterTypes(List<JobType> types)
		{
			foreach (var type in types)
				RegisterType(type);
		}

		public Job CreateJob(Guid typeId, dynamic attributes, JobPriority priority = 0, Guid? creatorId = null, Guid? schedulePlanId = null, Guid? jobId = null)
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
			job.Assembly = type.Assembly;
			job.CompleteClassName = type.CompleteClassName;
			job.MethodName = type.MethodName;
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

		public List<Job> GetJobs(DateTime? startFromDate = null, DateTime? startToDate = null, DateTime? finishedFromDate = null, DateTime? finishedToDate = null,
			string typeName = null, int? status = null, int? priority = null, Guid? schedulePlanId = null, int? page = null, int? pageSize = null)
		{
			return JobService.GetJobs(startFromDate, startToDate, finishedFromDate, finishedToDate,
				typeName, status, priority, schedulePlanId, page, pageSize);
		}

		public void ProcessJobsAsync()
		{
			Task.Run(() => Process());
		}

		private void Process()
		{
			if (!Settings.Enabled)
				return;

			Thread.Sleep(120000); //Initial sleep time

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
								using (var secCtx = SecurityContext.OpenSystemScope())
								{
									try
									{
										DbContext.CreateContext(ERP.Settings.ConnectionString);

										Log log = new Log();
										string jobId = job != null ? job.Id.ToString() : "null";
										string jobType = job != null && job.Type != null ? job.Type.Name : "null";
										log.Create(LogType.Error, "JobManager.Process", $"Start job with id[{jobId}] and type [{jobType}] failed! " , ex.Message);
									}
									finally
									{
										DbContext.CloseContext();
									}
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
							DbContext.CreateContext(ERP.Settings.ConnectionString);

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
	}
}
