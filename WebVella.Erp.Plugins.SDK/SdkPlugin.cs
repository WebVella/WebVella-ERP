using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Jobs;
using WebVella.Erp.Plugins.SDK.Jobs;

namespace WebVella.Erp.Plugins.SDK
{
	public partial class SdkPlugin : ErpPlugin
	{
		[JsonProperty(PropertyName = "name")]
		public override string Name { get; protected set; } = "sdk";

		public override void Initialize(IServiceProvider serviceProvider)
		{
			using (var ctx = SecurityContext.OpenSystemScope())
			{
				SetSchedulePlans();
				ProcessPatches();
			}
		}

		public override IEnumerable<Type> GetJobTypes()
		{
			List<Type> jobTypes = new List<Type>();
			jobTypes.Add(typeof(SampleJob));
			jobTypes.Add(typeof(ClearJobAndErrorLogsJob));
			return jobTypes;
		}

		public void SetSchedulePlans() {
			DateTime utcNow = DateTime.UtcNow;

			#region << Sample Job Plan >>
			//{
			//	Guid checkBotSchedulePlanId = new Guid("AC3D460F-77BD-44B6-A7C5-B52A37A0C846");
			//	string planName = "Sample Job Plan";
			//	SchedulePlan checkBotSchedulePlan = ScheduleManager.Current.GetSchedulePlan(checkBotSchedulePlanId);

			//	if (checkBotSchedulePlan == null)
			//	{
			//		checkBotSchedulePlan = new SchedulePlan();
			//		checkBotSchedulePlan.Id = checkBotSchedulePlanId;
			//		checkBotSchedulePlan.Name = planName;
			//		checkBotSchedulePlan.Type = SchedulePlanType.Daily;
			//		checkBotSchedulePlan.StartDate = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 2, DateTimeKind.Utc);
			//		checkBotSchedulePlan.EndDate = null;
			//		checkBotSchedulePlan.ScheduledDays = new SchedulePlanDaysOfWeek()
			//		{
			//			ScheduledOnMonday = true,
			//			ScheduledOnTuesday = true,
			//			ScheduledOnWednesday = true,
			//			ScheduledOnThursday = true,
			//			ScheduledOnFriday = true,
			//			ScheduledOnSaturday = true,
			//			ScheduledOnSunday = true
			//		};
			//		checkBotSchedulePlan.IntervalInMinutes = 1440;
			//		checkBotSchedulePlan.StartTimespan = 0;
			//		checkBotSchedulePlan.EndTimespan = 1440;
			//		checkBotSchedulePlan.JobTypeId = new Guid("559c557a-0fd3-4235-b061-117197154ca5");
			//		checkBotSchedulePlan.JobAttributes = null;
			//		checkBotSchedulePlan.Enabled = true;
			//		checkBotSchedulePlan.LastModifiedBy = null;

			//		ScheduleManager.Current.CreateSchedulePlan(checkBotSchedulePlan);
			//	}
			//}
			#endregion

			#region << Clear job and error logs Job Plan>>
			{
				Guid logsSchedulePlanId = new Guid("8CC1DF20-0967-4635-B44A-45FD90819105");
				SchedulePlan logsSchedulePlan = ScheduleManager.Current.GetSchedulePlan(logsSchedulePlanId);

				if (logsSchedulePlan == null)
				{
					logsSchedulePlan = new SchedulePlan();
					logsSchedulePlan.Id = logsSchedulePlanId;
					logsSchedulePlan.Name = "Clear job and error logs.";
					logsSchedulePlan.Type = SchedulePlanType.Daily;
					logsSchedulePlan.StartDate = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 2, DateTimeKind.Utc);
					logsSchedulePlan.EndDate = null;
					logsSchedulePlan.ScheduledDays = new SchedulePlanDaysOfWeek()
					{
						ScheduledOnMonday = true,
						ScheduledOnTuesday = true,
						ScheduledOnWednesday = true,
						ScheduledOnThursday = true,
						ScheduledOnFriday = true,
						ScheduledOnSaturday = true,
						ScheduledOnSunday = true
					};
					logsSchedulePlan.IntervalInMinutes = 1440;
					logsSchedulePlan.StartTimespan = 0;
					logsSchedulePlan.EndTimespan = 1440;
					logsSchedulePlan.JobTypeId = new Guid("99D9A8BB-31E6-4436-B0C2-20BD6AA23786");
					logsSchedulePlan.JobAttributes = null;
					logsSchedulePlan.Enabled = true;
					logsSchedulePlan.LastModifiedBy = null;

					ScheduleManager.Current.CreateSchedulePlan(logsSchedulePlan);
				}
			}
			#endregion
		}
	}
}
