using System;
using WebVella.ERP.Database;
using WebVella.ERP.Jobs;

namespace WebVella.ERP.Project
{
    public class StartupExtensions
    {
        public static void SetSchedulePlans()
        {
            DateTime utcNow = DateTime.UtcNow;

			#region << Search Index >>
			{
				Guid searchSchedulePlanId = new Guid("5E170FB5-5DE8-4984-909A-24F4ED6AC091");
				SchedulePlan checkBotSchedulePlan = ScheduleManager.Current.GetSchedulePlan(searchSchedulePlanId);

				if (checkBotSchedulePlan == null)
				{
					checkBotSchedulePlan = new SchedulePlan();
					checkBotSchedulePlan.Id = searchSchedulePlanId;
					checkBotSchedulePlan.Name = "Search Index";
					checkBotSchedulePlan.Type = SchedulePlanType.Interval;
					checkBotSchedulePlan.StartDate = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 2, DateTimeKind.Utc);
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
					checkBotSchedulePlan.IntervalInMinutes = 60;
					checkBotSchedulePlan.StartTimespan = 0;
					checkBotSchedulePlan.EndTimespan = 0;
					checkBotSchedulePlan.JobTypeId = new Guid("4E02D675-D02A-4211-837E-46C99A4CDE07");
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
