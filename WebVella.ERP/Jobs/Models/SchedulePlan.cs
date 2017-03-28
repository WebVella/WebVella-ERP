using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Jobs
{
	[Serializable]
	public enum SchedulePlanType
	{
		NotSelected = 0,
		Interval = 1,
		Daily = 2,
		Weekly = 3,
		Monthly = 4
	}

	[Serializable]
	public class SchedulePlan
	{
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "type")]
		public SchedulePlanType Type { get; set; }

		[JsonProperty(PropertyName = "start_date")]
		public DateTime? StartDate { get; set; }

		[JsonProperty(PropertyName = "end_date")]
		public DateTime? EndDate { get; set; }

		[JsonProperty(PropertyName = "schedule_days")]
		public SchedulePlanDaysOfWeek ScheduledDays { get; set; }

		[JsonProperty(PropertyName = "interval_in_minutes")]
		public int? IntervalInMinutes { get; set; }

		[JsonProperty(PropertyName = "start_timespan")]
		public int? StartTimespan { get; set; }

		[JsonProperty(PropertyName = "end_timespan")]
		public int? EndTimespan { get; set; }

		[JsonProperty(PropertyName = "last_trigger_time")]
		public DateTime? LastTriggerTime { get; set; }

		[JsonProperty(PropertyName = "next_trigger_time")]
		public DateTime? NextTriggerTime { get; set; }

		[JsonProperty(PropertyName = "job_type_id")]
		public Guid JobTypeId { get; set; }

		[JsonProperty(PropertyName = "job_type")]
		public JobType JobType { get; set; }

		[JsonProperty(PropertyName = "job_attributes")]
		public dynamic JobAttributes { get; set; }

		[JsonProperty(PropertyName = "enabled")]
		public bool Enabled { get; set; }

		[JsonProperty(PropertyName = "last_started_job_id")]
		public Guid? LastStartedJobId { get; set; }

		[JsonProperty(PropertyName = "created_on")]
		public DateTime CreatedOn { get; internal set; }

		[JsonProperty(PropertyName = "last_modified_by")]
		public Guid? LastModifiedBy { get; set; }

		[JsonProperty(PropertyName = "last_modified_on")]
		public DateTime LastModifiedOn { get; internal set; }
	}

	[Serializable]
	public class SchedulePlanDaysOfWeek
	{
		[JsonProperty(PropertyName = "scheduled_on_sunday")]
		public bool ScheduledOnSunday { get; set; }

		[JsonProperty(PropertyName = "scheduled_on_monday")]
		public bool ScheduledOnMonday { get; set; }

		[JsonProperty(PropertyName = "scheduled_on_tuesday")]
		public bool ScheduledOnTuesday { get; set; }

		[JsonProperty(PropertyName = "scheduled_on_wednesday")]
		public bool ScheduledOnWednesday { get; set; }

		[JsonProperty(PropertyName = "scheduled_on_thursday")]
		public bool ScheduledOnThursday { get; set; }

		[JsonProperty(PropertyName = "scheduled_on_friday")]
		public bool ScheduledOnFriday { get; set; }

		[JsonProperty(PropertyName = "scheduled_on_saturday")]
		public bool ScheduledOnSaturday { get; set; }
	}

	//[Serializable]
	//public class DailySchedulePlan : SchedulePlan
	//{
	//	[JsonProperty(PropertyName = "schedule_days")]
	//	public SchedulePlanDaysOfWeek ScheduledDays { get; set; }

	//}

	//[Serializable]
	//public class WeeklySchedulePlan : SchedulePlan
	//{

	//}

	//[Serializable]
	//public class MonthlySchedulePlan : SchedulePlan
	//{

	//}

	//[Serializable]
	//public class IntervalSchedulePlan : SchedulePlan
	//{
	//	public int IntervalInMinutes { get; set; }

	//	public SchedulePlanTimespan Timespan { get; set; }

	//	public SchedulePlanDaysOfWeek ScheduledDays { get; set; }
	//}

	//[Serializable]
	//public class SchedulePlanTimespan
	//{
	//	public int? StartTimespan { get; private set; }

	//	public int? EndTimespan { get; private set; }
	//}
}