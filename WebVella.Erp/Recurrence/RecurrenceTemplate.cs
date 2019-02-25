using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Recurrence
{
	public class RecurrenceTemplate
	{
		[JsonProperty(PropertyName = "type")]
		public RecurrenceType Type { get; set; } = RecurrenceType.DoesNotRepeat;

		[JsonProperty(PropertyName = "end_type")]
		public RecurrenceEndType EndType { get; set; } = RecurrenceEndType.Never;

		[JsonProperty(PropertyName = "end_date")]
		public DateTime? EndDate { get; set; } = null;

		[JsonProperty(PropertyName = "interval")]
		public int Interval { get; set; } = 1;

		[JsonProperty(PropertyName = "repeat_period_type")]
		public RecurrencePeriodType RepeatPeriodType { get; set; } = RecurrencePeriodType.Day;

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

		public List<Occurrence> CalculateOccurrences(DateTime startDate, DateTime endDate, DateTime searchStartDate, DateTime searchEndDate)
		{
			List<Occurrence> result = new List<Occurrence>();

			var vEvent = new CalendarEvent { Start = new CalDateTime(startDate), End = new CalDateTime(endDate) };
			RecurrencePattern recurrenceRule = null;

			switch (Type)
			{
				case RecurrenceType.DoesNotRepeat:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.None, interval: 1)
						{
							Until = endDate,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.Daily:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.Daily, interval: 1)
						{
							Until = searchEndDate,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.Weekly:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.Weekly, interval: 1)
						{
							Until = searchEndDate,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.MonthlyByDate:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.Monthly, interval: 1)
						{
							ByMonthDay = new List<int> { startDate.Day },
							Until = searchEndDate,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.MonthlyByWeekDay:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.Monthly, interval: 1)
						{
							ByDay = new List<WeekDay> { new WeekDay { DayOfWeek = startDate.DayOfWeek, Offset = GetWeekOfMonth(startDate) } },
							Until = searchEndDate,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.Annually:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.Yearly, interval: 1)
						{
							Until = searchEndDate,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.EveryWeekDay:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.Weekly, interval: 1)
						{
							ByDay = new List<WeekDay> { new WeekDay { DayOfWeek = startDate.DayOfWeek } },
							Until = searchEndDate,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.Custom:
					{
						switch (RepeatPeriodType)
						{
							case RecurrencePeriodType.Second:
								{
									recurrenceRule = new RecurrencePattern(FrequencyType.Secondly, interval: Interval)
									{
										Until = searchEndDate,
										RestrictionType = RecurrenceRestrictionType.NoRestriction
									};
								}
								break;
							case RecurrencePeriodType.Minute:
								{
									recurrenceRule = new RecurrencePattern(FrequencyType.Minutely, interval: Interval)
									{
										Until = searchEndDate,
										RestrictionType = RecurrenceRestrictionType.NoRestriction
									};
								}
								break;
							case RecurrencePeriodType.Hour:
								{
									recurrenceRule = new RecurrencePattern(FrequencyType.Hourly, interval: Interval)
									{
										Until = searchEndDate,
										RestrictionType = RecurrenceRestrictionType.NoRestriction
									};
								}
								break;
							case RecurrencePeriodType.Day:
								{
									recurrenceRule = new RecurrencePattern(FrequencyType.Daily, interval: Interval)
									{
										Until = searchEndDate,
										RestrictionType = RecurrenceRestrictionType.NoRestriction
									};
								}
								break;
							case RecurrencePeriodType.Week:
								{
									var weekDays = new List<WeekDay>();
									if (AllowMonday) weekDays.Add(new WeekDay { DayOfWeek = DayOfWeek.Monday });
									if (AllowTuesday) weekDays.Add(new WeekDay { DayOfWeek = DayOfWeek.Tuesday });
									if (AllowWednesday) weekDays.Add(new WeekDay { DayOfWeek = DayOfWeek.Wednesday });
									if (AllowThursday) weekDays.Add(new WeekDay { DayOfWeek = DayOfWeek.Thursday });
									if (AllowFriday) weekDays.Add(new WeekDay { DayOfWeek = DayOfWeek.Friday });
									if (AllowSaturday) weekDays.Add(new WeekDay { DayOfWeek = DayOfWeek.Saturday });
									if (AllowSunday) weekDays.Add(new WeekDay { DayOfWeek = DayOfWeek.Sunday });

									recurrenceRule = new RecurrencePattern(FrequencyType.Daily, interval: Interval)
									{
										ByDay = weekDays,
										Until = searchEndDate,
										RestrictionType = RecurrenceRestrictionType.NoRestriction
									};
								}
								break;
							case RecurrencePeriodType.Month:
								{
									if (RepeatMonthByDate == 0) //by date
									{
										recurrenceRule = new RecurrencePattern(FrequencyType.Monthly, interval: Interval)
										{
											ByMonthDay = new List<int> { startDate.Day },
											Until = searchEndDate,
											RestrictionType = RecurrenceRestrictionType.NoRestriction
										};
									}
									else if (RepeatMonthByDate == 1) //by week day
									{
										recurrenceRule = new RecurrencePattern(FrequencyType.Monthly, interval: Interval)
										{
											ByDay = new List<WeekDay> { new WeekDay { DayOfWeek = startDate.DayOfWeek, Offset = GetWeekOfMonth(startDate) } },
											Until = searchEndDate,
											RestrictionType = RecurrenceRestrictionType.NoRestriction
										};
									}
									else
										throw new NotSupportedException("RepeatMonthByDate");
								}
								break;
							case RecurrencePeriodType.Year:
								recurrenceRule = new RecurrencePattern(FrequencyType.Yearly, interval: Interval)
								{
									Until = searchEndDate,
									RestrictionType = RecurrenceRestrictionType.NoRestriction

								};
								break;
						}

					}
					break;
			}

			vEvent.RecurrenceRules = new List<RecurrencePattern> { recurrenceRule };
			var calendar = new Calendar();
			calendar.Events.Add(vEvent);

			foreach (var occurrence in calendar.GetOccurrences(searchStartDate, searchEndDate)) { result.Add(occurrence); }
			return result;
		}

		private static int GetWeekOfMonth(DateTime date, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
		{
			DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

			while (date.Date.AddDays(1).DayOfWeek != firstDayOfWeek)
				date = date.AddDays(1);

			return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
		}
	}

}
