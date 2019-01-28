using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Jobs;
using WebVella.Erp.Plugins.Project.Jobs;

namespace WebVella.Erp.Plugins.Project
{
	public partial class ProjectPlugin : ErpPlugin
	{
		[JsonProperty(PropertyName = "name")]
		public override string Name { get; protected set; } = "project";

		public override void Initialize(IServiceProvider serviceProvider)
		{
			using (var ctx = SecurityContext.OpenSystemScope())
			{
				ProcessPatches();
				SetSchedulePlans();
			}
		}

		public override IEnumerable<Type> GetJobTypes()
		{
			List<Type> jobTypes = new List<Type>();
			jobTypes.Add(typeof(StartTasksOnStartDate));
			return jobTypes;
		}

		public void SetSchedulePlans()
		{
			DateTime utcNow = DateTime.UtcNow;

			#region << StartTasksOnStartDate >>
			{
				Guid checkBotSchedulePlanId = new Guid("6765D758-FB63-478F-B714-5B153AB9A758");
				string planName = "Start tasks on start_date";
				SchedulePlan checkBotSchedulePlan = ScheduleManager.Current.GetSchedulePlan(checkBotSchedulePlanId);

				if (checkBotSchedulePlan == null)
				{
					checkBotSchedulePlan = new SchedulePlan();
					checkBotSchedulePlan.Id = checkBotSchedulePlanId;
					checkBotSchedulePlan.Name = planName;
					checkBotSchedulePlan.Type = SchedulePlanType.Daily;
					checkBotSchedulePlan.StartDate = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 10, 0, DateTimeKind.Utc);
					checkBotSchedulePlan.EndDate = null;
					checkBotSchedulePlan.ScheduledDays = new SchedulePlanDaysOfWeek()
					{
						ScheduledOnMonday = true,
						ScheduledOnTuesday = true,
						ScheduledOnWednesday = true,
						ScheduledOnThursday = true,
						ScheduledOnFriday = true,
						ScheduledOnSaturday = true,
						ScheduledOnSunday = true
					};
					checkBotSchedulePlan.IntervalInMinutes = 1440;
					checkBotSchedulePlan.StartTimespan = 0;
					checkBotSchedulePlan.EndTimespan = 1440;
					checkBotSchedulePlan.JobTypeId = new Guid("3D18B8D8-74B8-45B1-B121-9582F7B8A4F4");
					checkBotSchedulePlan.JobAttributes = null;
					checkBotSchedulePlan.Enabled = true;
					checkBotSchedulePlan.LastModifiedBy = null;

					ScheduleManager.Current.CreateSchedulePlan(checkBotSchedulePlan);
				}
			}
			#endregion

		}
	}
}
