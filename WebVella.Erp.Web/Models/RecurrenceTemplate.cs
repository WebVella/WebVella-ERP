using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Web.Models
{
	public class RecurrenceTemplate
	{
		[JsonProperty(PropertyName = "type")]
		public RecurrenceType Type { get; set; } = RecurrenceType.DoesNotRepeat;

		[JsonProperty(PropertyName = "end_type")]
		public RecurrenceEndType EndType { get; set; } = RecurrenceEndType.Never;

		[JsonProperty(PropertyName = "end_date")]
		public DateTime? EndDate { get; set; } = null;

		[JsonProperty(PropertyName = "end_count")]
		public int? EndCount { get; set; } = 1;

		[JsonProperty(PropertyName = "repeat_period")]
		public PeriodType RepeatPeriod { get; set; } = PeriodType.Day;

		[JsonProperty(PropertyName = "repeat_count")]
		public int? RepeatCount { get; set; } = 1;

		[JsonProperty(PropertyName = "timespan_start")]
		public int? TimeSpanStart { get; set; } = null;

		[JsonProperty(PropertyName = "timespan_end")]
		public int? TimeSpanEnd { get; set; } = null;

		[JsonProperty(PropertyName = "allow_monday")]
		public bool AllowMonday { get; set; } = true;

		[JsonProperty(PropertyName = "allow_tuesday")]
		public bool AllowTuesday { get; set; } = true;

		[JsonProperty(PropertyName = "allow_wednesday")]
		public bool AllowWednesday { get; set; } = true;

		[JsonProperty(PropertyName = "allow_thursday")]
		public bool AllowThursday { get; set; } = true;

		[JsonProperty(PropertyName = "allow_friday")]
		public bool AllowFriday { get; set; } = true;

		[JsonProperty(PropertyName = "allow_saturday")]
		public bool AllowSaturday { get; set; } = true;

		[JsonProperty(PropertyName = "allow_sunday")]
		public bool AllowSunday { get; set; } = true;

		[JsonProperty(PropertyName = "repeat_month_type")]
		public int RepeatMonthByDate { get; set; } = 0; // 0 - by date, 1 - by week day occurrence 

		[JsonProperty(PropertyName = "recurrence_change_type")]
		public RecurrenceChangeType RecurrenceChangeType { get; set; } = RecurrenceChangeType.OnlyThis;
	}

}
