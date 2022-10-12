using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebVella.Erp.Api;
using WebVella.Erp.Database;
using WebVella.Erp.Diagnostics;
using WebVella.Erp.Exceptions;

namespace WebVella.Erp.Jobs
{
	public class ScheduleManager
	{
		public static ScheduleManager Current { get; private set; }

		private static JobManagerSettings Settings { get; set; }

		private JobDataService JobService { get; set; }

		private ScheduleManager()
		{
		}

		private ScheduleManager(JobManagerSettings settings)
		{
			Settings = settings;
			JobService = new JobDataService(Settings);
		}

		public static void Initialize(JobManagerSettings settings)
		{
			Current = new ScheduleManager(settings);
		}

		public bool CreateSchedulePlan(SchedulePlan schedulePlan)
		{
			if (schedulePlan.Id == Guid.Empty)
				schedulePlan.Id = Guid.NewGuid();

			schedulePlan.NextTriggerTime = FindSchedulePlanNextTriggerDate(schedulePlan);

			return JobService.CreateSchedule(schedulePlan);
		}

		public bool UpdateSchedulePlan(SchedulePlan schedulePlan)
		{
			return JobService.UpdateSchedule(schedulePlan);
		}

		private bool UpdateSchedulePlanShort(SchedulePlan schedulePlan)
		{
			return JobService.UpdateSchedule(schedulePlan.Id, schedulePlan.LastTriggerTime, schedulePlan.NextTriggerTime,
												schedulePlan.LastModifiedBy, schedulePlan.LastStartedJobId);
		}

		public SchedulePlan GetSchedulePlan(Guid id)
		{
			return JobService.GetSchedulePlan(id);
		}

		public List<SchedulePlan> GetSchedulePlans()
		{
			return JobService.GetSchedulePlans();
		}

		public void TriggerNowSchedulePlan(SchedulePlan schedulePlan)
		{
			schedulePlan.NextTriggerTime = DateTime.UtcNow.AddMinutes(1);
			UpdateSchedulePlanShort(schedulePlan);
		}

		public void ProcessSchedulesAsync()
		{
			Task.Run(() => Process());
		}

		public void Process()
		{
			if (!Settings.Enabled)
				return;

			//Thread.Sleep(120000); //Initial sleep time

			while (true)
			{
				try
				{
					//Get ready for execution schedules
					List<SchedulePlan> schedulePlans = JobService.GetReadyForExecutionScheduledPlans();

					//foreach schedule if it's time create a job and save it to db
					foreach (var schedulePlan in schedulePlans)
					{
						if (schedulePlan is null || schedulePlan.JobType is null)
							continue;

						//run new job if last one is finished or canceled
						bool startNewJob = true;
						if (schedulePlan.LastStartedJobId.HasValue)
							startNewJob = JobService.IsJobFinished(schedulePlan.LastStartedJobId.Value);

						//calculate next schedule run time and update
						switch (schedulePlan.Type)
						{
							case SchedulePlanType.Interval:
								{
									if (startNewJob)
										schedulePlan.LastTriggerTime = DateTime.UtcNow;

									DateTime startDate = DateTime.UtcNow;

									DateTime? nextActivation = FindIntervalSchedulePlanNextTriggerDate(schedulePlan, startDate, schedulePlan.LastTriggerTime);
									if (nextActivation.HasValue)
									{
										schedulePlan.NextTriggerTime = nextActivation.Value;
									}
									else
									{
										schedulePlan.NextTriggerTime = null;
									}

									break;
								}
							case SchedulePlanType.Daily:
								{
									if (startNewJob)
										schedulePlan.LastTriggerTime = DateTime.UtcNow;

									DateTime startDate = DateTime.UtcNow;

									DateTime? nextActivation = FindDailySchedulePlanNextTriggerDate(schedulePlan, startDate.AddMinutes(1),
											schedulePlan.StartDate.HasValue ? schedulePlan.StartDate.Value : startDate);
									if (nextActivation.HasValue)
									{
										schedulePlan.NextTriggerTime = nextActivation.Value;
									}
									else
									{
										schedulePlan.NextTriggerTime = null;
									}

									break;
								}
							case SchedulePlanType.Weekly:
								{
									if (startNewJob)
										schedulePlan.LastTriggerTime = DateTime.UtcNow;

									DateTime nextActivation = schedulePlan.LastTriggerTime.HasValue
																  ? schedulePlan.LastTriggerTime.Value.AddDays(7)
																  : DateTime.UtcNow.AddDays(7);

									if ((!schedulePlan.EndDate.HasValue) || (nextActivation < schedulePlan.EndDate.Value))
									{
										schedulePlan.NextTriggerTime = nextActivation;
									} 
									else
									{
										schedulePlan.NextTriggerTime = null;
									}

									break;
								}
							case SchedulePlanType.Monthly:
								{
									if (startNewJob)
										schedulePlan.LastTriggerTime = DateTime.UtcNow;

									DateTime nextActivation = schedulePlan.LastTriggerTime.HasValue
																  ? schedulePlan.LastTriggerTime.Value.AddMonths(1)
																  : DateTime.UtcNow.AddMonths(1);
									if ((!schedulePlan.EndDate.HasValue) || (nextActivation < schedulePlan.EndDate.Value))
									{
										schedulePlan.NextTriggerTime = nextActivation;
									}
									else
									{
										schedulePlan.NextTriggerTime = null;
									}
									break;
								}
						}

						if (startNewJob)
						{
							try
							{
								Job job = JobManager.Current.CreateJob(schedulePlan.JobType.Id, schedulePlan.JobAttributes, schedulePlanId: schedulePlan.Id);
								schedulePlan.LastStartedJobId = job.Id;
							}
							catch(Exception scex)
							{
								throw new Exception($"Schedule plan '{schedulePlan.Name}' failed to create job.", scex);
							}
						}
						UpdateSchedulePlanShort(schedulePlan);
					}

				}
				catch (Exception ex)
				{

					try
					{
						DbContext.CreateContext(Erp.ErpSettings.ConnectionString);

						using (var secCtx = SecurityContext.OpenSystemScope())
						{
							Log log = new Log();
							log.Create(LogType.Error, "ScheduleManager.Process", ex);
						}
					}
					finally
					{
						DbContext.CloseContext();
					}

				}
				finally
				{
					Thread.Sleep(12000);
				}
			}
		}

		internal async void ProcessSchedulesAsync(CancellationToken stoppingToken)
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
					//Get ready for execution schedules
					List<SchedulePlan> schedulePlans = JobService.GetReadyForExecutionScheduledPlans();

					//foreach schedule if it's time create a job and save it to db
					foreach (var schedulePlan in schedulePlans)
					{
						//run new job if last one is finished or canceled
						bool startNewJob = true;
						if (schedulePlan.LastStartedJobId.HasValue)
							startNewJob = JobService.IsJobFinished(schedulePlan.LastStartedJobId.Value);

						//calculate next schedule run time and update
						switch (schedulePlan.Type)
						{
							case SchedulePlanType.Interval:
								{
									if (startNewJob)
										schedulePlan.LastTriggerTime = DateTime.UtcNow;

									DateTime startDate = DateTime.UtcNow;

									DateTime? nextActivation = FindIntervalSchedulePlanNextTriggerDate(schedulePlan, startDate, schedulePlan.LastTriggerTime);
									if (nextActivation.HasValue)
									{
										schedulePlan.NextTriggerTime = nextActivation.Value;
									}
									else
									{
										schedulePlan.NextTriggerTime = null;
									}

									break;
								}
							case SchedulePlanType.Daily:
								{
									if (startNewJob)
										schedulePlan.LastTriggerTime = DateTime.UtcNow;

									DateTime startDate = DateTime.UtcNow;

									DateTime? nextActivation = FindDailySchedulePlanNextTriggerDate(schedulePlan, startDate.AddMinutes(1),
											schedulePlan.StartDate.HasValue ? schedulePlan.StartDate.Value : startDate);
									if (nextActivation.HasValue)
									{
										schedulePlan.NextTriggerTime = nextActivation.Value;
									}
									else
									{
										schedulePlan.NextTriggerTime = null;
									}

									break;
								}
							case SchedulePlanType.Weekly:
								{
									if (startNewJob)
										schedulePlan.LastTriggerTime = DateTime.UtcNow;

									DateTime nextActivation = schedulePlan.LastTriggerTime.HasValue
																  ? schedulePlan.LastTriggerTime.Value.AddDays(7)
																  : DateTime.UtcNow.AddDays(7);

									if ((!schedulePlan.EndDate.HasValue) || (nextActivation < schedulePlan.EndDate.Value))
									{
										schedulePlan.NextTriggerTime = nextActivation;
									}
									else
									{
										schedulePlan.NextTriggerTime = null;
									}

									break;
								}
							case SchedulePlanType.Monthly:
								{
									if (startNewJob)
										schedulePlan.LastTriggerTime = DateTime.UtcNow;

									DateTime nextActivation = schedulePlan.LastTriggerTime.HasValue
																  ? schedulePlan.LastTriggerTime.Value.AddMonths(1)
																  : DateTime.UtcNow.AddMonths(1);
									if ((!schedulePlan.EndDate.HasValue) || (nextActivation < schedulePlan.EndDate.Value))
									{
										schedulePlan.NextTriggerTime = nextActivation;
									}
									else
									{
										schedulePlan.NextTriggerTime = null;
									}
									break;
								}
						}

						if (startNewJob)
						{
							try
							{
								Job job = JobManager.Current.CreateJob(schedulePlan.JobType.Id, schedulePlan.JobAttributes, schedulePlanId: schedulePlan.Id);
								schedulePlan.LastStartedJobId = job.Id;
							}
							catch (Exception scex)
							{
								throw new Exception($"Schedule plan '{schedulePlan.Name}' failed to create job.", scex);
							}
						}
						UpdateSchedulePlanShort(schedulePlan);
					}

				}
				catch (Exception ex)
				{

					try
					{
						DbContext.CreateContext(Erp.ErpSettings.ConnectionString);

						using (var secCtx = SecurityContext.OpenSystemScope())
						{
							Log log = new Log();
							log.Create(LogType.Error, "ScheduleManager.Process", ex);
						}
					}
					finally
					{
						DbContext.CloseContext();
					}

				}
				finally
				{
					try { await Task.Delay(10000, stoppingToken); } catch (TaskCanceledException) { };
				}
			}
		}

		public DateTime? FindSchedulePlanNextTriggerDate(SchedulePlan schedulePlan)
		{
			SchedulePlanType planType = schedulePlan.Type;
			DateTime nowDateTime = DateTime.UtcNow;//.AddMinutes(1);
			DateTime startingDate;
			if (schedulePlan.StartDate.HasValue)//if day is selected then 
			{
				startingDate = schedulePlan.StartDate.Value;
			}
			else
			{
				startingDate = nowDateTime;
			}
			switch (planType)
			{
				case SchedulePlanType.Interval:
					{
						return FindIntervalSchedulePlanNextTriggerDate(schedulePlan, nowDateTime, schedulePlan.LastTriggerTime);
					}
				case SchedulePlanType.Daily:
					{
						return FindDailySchedulePlanNextTriggerDate(schedulePlan, nowDateTime, startingDate);
					}
				case SchedulePlanType.Weekly:
					{
						return FindWeeklySchedulePlanNextTriggerDate(schedulePlan, nowDateTime, startingDate);
					}
				case SchedulePlanType.Monthly:
					{
						return FindMonthlySchedulePlanNextTriggerDate(schedulePlan, nowDateTime, startingDate);
					}
			}
			return null;
		}

		private DateTime? FindIntervalSchedulePlanNextTriggerDate(SchedulePlan intervalPlan, DateTime nowDateTime, DateTime? lastExecution)
		{
			if (intervalPlan.ScheduledDays == null)
				intervalPlan.ScheduledDays = new SchedulePlanDaysOfWeek();

			var daysOfWeek = intervalPlan.ScheduledDays;

			//if interval is <=0 then can't find match
			if (intervalPlan.IntervalInMinutes <= 0)
			{
				return null;
			}

			DateTime startingDate = lastExecution.HasValue ? lastExecution.Value.AddMinutes(intervalPlan.IntervalInMinutes.Value) : DateTime.UtcNow;
			try
			{
				while (true)
				{
					//check for expired interval
					if (intervalPlan.EndDate.HasValue)
					{
						if (intervalPlan.EndDate.Value < startingDate)
						{
							return null;
						}
					}

					int timeAsInt = startingDate.Hour * 60 + startingDate.Minute;
					bool isIntervalConnectedToFirstDay = false;
					if (intervalPlan.StartTimespan.HasValue)
					{
						isIntervalConnectedToFirstDay = ((intervalPlan.StartTimespan.Value > intervalPlan.EndTimespan.Value) &&
															   ((0 < timeAsInt) && (timeAsInt <= intervalPlan.EndTimespan.Value)));
					}

					if (IsTimeInTimespanInterval(startingDate, intervalPlan.StartTimespan, intervalPlan.EndTimespan) )
					{
						if (IsDayUsedInSchedulePlan(startingDate, daysOfWeek, isIntervalConnectedToFirstDay))
							return startingDate;
						else
							startingDate = startingDate.AddDays(1);
						
						continue;
					}
					else //step
					{
						DateTime startTimespan = new DateTime(startingDate.Year, startingDate.Month, startingDate.Day, 0, 0, 0, DateTimeKind.Utc);
						startTimespan = startTimespan.AddMinutes(intervalPlan.StartTimespan.Value);

						if (nowDateTime <= startTimespan && startingDate <= startTimespan)
						{
							startingDate = startTimespan;
						}
						else
						{
							startingDate = startTimespan.AddDays(1);
						}
					}
				}
			}
			catch
			{
				return null;
			}
		}

		//private DateTime? FindIntervalSchedulePlanNextTriggerDate(SchedulePlan intervalPlan, DateTime nowDateTime, DateTime startDate)
		//{
		//	if (intervalPlan.ScheduledDays == null)
		//		intervalPlan.ScheduledDays = new SchedulePlanDaysOfWeek();

		//	var daysOfWeek = intervalPlan.ScheduledDays;

		//	//if interval is <=0 then can't find match
		//	if (intervalPlan.IntervalInMinutes <= 0)
		//	{
		//		return null;
		//	}

		//	DateTime startingDate = startDate;
		//	try
		//	{
		//		while (true)
		//		{
		//			//check for expired interval
		//			if (intervalPlan.EndDate.HasValue)
		//			{
		//				if (intervalPlan.EndDate.Value < startingDate)
		//				{
		//					return null;
		//				}
		//			}
		//			int timeAsInt = startingDate.Hour * 60 + startingDate.Minute;
		//			bool isIntervalConnectedToFirstDay = false;
		//			if (intervalPlan.StartTimespan.HasValue)
		//			{
		//				isIntervalConnectedToFirstDay = ((intervalPlan.StartTimespan.Value > intervalPlan.EndTimespan.Value) &&
		//													   ((0 < timeAsInt) && (timeAsInt <= intervalPlan.EndTimespan.Value)));
		//			}

		//			DateTime movedTime = startingDate.AddSeconds(10);
		//			if (movedTime >= nowDateTime && IsTimeInTimespanInterval(startingDate, intervalPlan.StartTimespan, intervalPlan.EndTimespan) &&
		//				IsDayUsedInSchedulePlan(startingDate, daysOfWeek, isIntervalConnectedToFirstDay))
		//			{
		//				return startingDate;
		//			}
		//			else //step
		//			{
		//				startingDate = startingDate.AddMinutes(intervalPlan.IntervalInMinutes.Value);
		//			}
		//		}
		//	}
		//	catch
		//	{
		//		return null;
		//	}
		//}

		private DateTime? FindDailySchedulePlanNextTriggerDate(SchedulePlan dailyPlan, DateTime nowDateTime, DateTime startDate)
		{
			if (dailyPlan.ScheduledDays == null)
				dailyPlan.ScheduledDays = new SchedulePlanDaysOfWeek();

			var daysOfWeek = dailyPlan.ScheduledDays;

			DateTime startingDate = startDate;
			try
			{
				while (true)
				{
					//check for expired interval
					if (dailyPlan.EndDate.HasValue)
					{
						if (dailyPlan.EndDate.Value < startingDate)
						{
							return null;
						}
					}

					DateTime movedTime = startingDate.AddSeconds(10);
					if (movedTime >= nowDateTime && IsDayUsedInSchedulePlan(startingDate, daysOfWeek, false))
					{
						return startingDate;
					}
					else //step
					{
						startingDate = startingDate.AddDays(1);
					}
				}
			}
			catch
			{
				return null;
			}
		}

		private DateTime? FindWeeklySchedulePlanNextTriggerDate(SchedulePlan weeklyPlan, DateTime nowDateTime, DateTime startDate)
		{
			DateTime startingDate = startDate;
			try
			{
				while (true)
				{
					//check for expired interval
					if (weeklyPlan.EndDate.HasValue)
					{
						if (weeklyPlan.EndDate.Value < startingDate)
						{
							return null;
						}
					}
					DateTime movedTime = startingDate.AddSeconds(10);
					if (movedTime >= nowDateTime)
					{
						return startingDate;
					}
					else //step
					{
						startingDate = startingDate.AddDays(7);
					}
				}
			}
			catch
			{
				return null;
			}
		}

		private DateTime? FindMonthlySchedulePlanNextTriggerDate(SchedulePlan monthlyPlan, DateTime nowDateTime, DateTime startDate)
		{
			DateTime startingDate = startDate;
			try
			{
				while (true)
				{
					//check for expired interval
					if (monthlyPlan.EndDate.HasValue)
					{
						if (monthlyPlan.EndDate.Value < startingDate)
						{
							return null;
						}
					}
					DateTime movedTime = startingDate.AddSeconds(10);
					if (movedTime >= nowDateTime)
					{
						return startingDate;
					}
					else //step
					{
						startingDate = startingDate.AddMonths(1);
					}
				}
			}
			catch
			{
				return null;
			}
		}

		private bool IsDayUsedInSchedulePlan(DateTime checkedDay, SchedulePlanDaysOfWeek selectedDays, bool isTimeConnectedToFirstDay)
		{
			DateTime dayToCheck = checkedDay;
			DayOfWeek dayOfWeek = dayToCheck.DayOfWeek;
			if (isTimeConnectedToFirstDay)
			{
				dayToCheck = dayToCheck.AddDays(-1);
				dayOfWeek = dayToCheck.DayOfWeek;
			}
			switch (dayOfWeek)
			{
				case DayOfWeek.Sunday:
					{
						if (selectedDays.ScheduledOnSunday)
						{
							return true;
						}
						break;
					}
				case DayOfWeek.Monday:
					{
						if (selectedDays.ScheduledOnMonday)
						{
							return true;
						}
						break;
					}
				case DayOfWeek.Tuesday:
					{
						if (selectedDays.ScheduledOnTuesday)
						{
							return true;
						}
						break;
					}
				case DayOfWeek.Wednesday:
					{
						if (selectedDays.ScheduledOnWednesday)
						{
							return true;
						}
						break;
					}
				case DayOfWeek.Thursday:
					{
						if (selectedDays.ScheduledOnThursday)
						{
							return true;
						}
						break;
					}
				case DayOfWeek.Friday:
					{
						if (selectedDays.ScheduledOnFriday)
						{
							return true;
						}
						break;
					}
				case DayOfWeek.Saturday:
					{
						if (selectedDays.ScheduledOnSaturday)
						{
							return true;
						}
						break;
					}
			}

			return false;
		}

		private bool IsTimeInTimespanInterval(DateTime date, int? startTimespan, int? endTimespan)
		{
			int timeAsInt = date.Hour * 60 + date.Minute;
			//if no time span interval then everything is ok
			if (!startTimespan.HasValue)
			{
				return true;
			}

			if (startTimespan < endTimespan) //normal situation start(200) - end(1000)
			{
				return ((startTimespan <= timeAsInt) && (timeAsInt <= endTimespan));
			}
			else //day overlap start(1000) - end(200)
			{
				return (((startTimespan <= timeAsInt) && (timeAsInt <= 1440)) ||
						 ((0 < timeAsInt) && (timeAsInt <= endTimespan)));
			}
		}
	}
}
