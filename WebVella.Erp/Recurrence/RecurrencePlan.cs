using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Recurrence
{
	public class RecurrencePlan
	{
		private const int CALCULATION_EXTEND_YEARS = 5;

		[JsonProperty(PropertyName = "type")]
		public RecurrenceType Type { get; set; } = RecurrenceType.DoesNotRepeat;

		[JsonProperty(PropertyName = "end_type")]
		public RecurrenceEndType EndType { get; set; } = RecurrenceEndType.Never;

		[JsonProperty(PropertyName = "end_date")]
		public DateTime? EndDate { get; set; } = null; //used when EndType is Date

		[JsonProperty(PropertyName = "occurrences_count")]
		public int OccurrencesCount { get; set; } = 0; //used when EndType is Occurrences

		[JsonProperty(PropertyName = "interval")]
		public int Interval { get; set; } = 1; //used when Type is Custom to specify interval in sec,min,day....

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
		public RecurrenceRepeatMonthType RepeatMonthType { get; set; } = RecurrenceRepeatMonthType.ByDate;

		public List<Occurrence> CalculateOccurrences(DateTime startTime, DateTime endTime)
		{
			DateTime calculationStartTime = DateTime.Today;
			DateTime calculationEndTime = DateTime.Today;


			switch (EndType)
			{
				case RecurrenceEndType.Never:
					{
						//initialiy we set end date CALCULATION_EXTEND_YEARS years in the future
						//at later stage there will be background task which will extend these occurences
						calculationStartTime = startTime;
						calculationEndTime = startTime.AddYears(CALCULATION_EXTEND_YEARS);
					}
					break;
				case RecurrenceEndType.Date:
					{
						calculationStartTime = startTime;
						if (EndDate == null)
							throw new Exception("When EndType is date, EndDate is required");

						calculationEndTime = EndDate.Value;
					}
					break;
				case RecurrenceEndType.Occurrences:
					{
						//initialiy we set end date CALCULATION_EXTEND_YEARS years in the future, but later if occurences count is less than requestes, 
						//we will increase it by CALCULATION_EXTEND_YEARS years step
						calculationStartTime = startTime;
						calculationEndTime = startTime.AddYears(CALCULATION_EXTEND_YEARS);
					}
					break;
			}


			List<Occurrence> result = new List<Occurrence>();

			var vEvent = new CalendarEvent { Start = new CalDateTime(startTime), End = new CalDateTime(endTime) };
			RecurrencePattern recurrenceRule = null;

			switch (Type)
			{
				case RecurrenceType.DoesNotRepeat:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.None, interval: 1)
						{
							Until = endTime,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.Daily:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.Daily, interval: 1)
						{
							Until = calculationEndTime,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.Weekly:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.Weekly, interval: 1)
						{
							Until = calculationEndTime,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.MonthlyByDate:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.Monthly, interval: 1)
						{
							ByMonthDay = new List<int> { startTime.Day },
							Until = calculationEndTime,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.MonthlyByWeekDay:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.Monthly, interval: 1)
						{
							ByDay = new List<WeekDay> { new WeekDay { DayOfWeek = startTime.DayOfWeek, Offset = GetWeekOfMonth(startTime) } },
							Until = calculationEndTime,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.Annually:
					{
						recurrenceRule = new RecurrencePattern(FrequencyType.Yearly, interval: 1)
						{
							Until = calculationEndTime,
							RestrictionType = RecurrenceRestrictionType.NoRestriction
						};
					}
					break;
				case RecurrenceType.EveryWeekDay:
					{
						var weekDays = new List<WeekDay>();
						weekDays.Add(new WeekDay { DayOfWeek = DayOfWeek.Monday });
						weekDays.Add(new WeekDay { DayOfWeek = DayOfWeek.Tuesday });
						weekDays.Add(new WeekDay { DayOfWeek = DayOfWeek.Wednesday });
						weekDays.Add(new WeekDay { DayOfWeek = DayOfWeek.Thursday });
						weekDays.Add(new WeekDay { DayOfWeek = DayOfWeek.Friday });
						recurrenceRule = new RecurrencePattern(FrequencyType.Daily, interval: 1)
						{
							ByDay = weekDays,
							Until = calculationEndTime,
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
										Until = calculationEndTime,
										RestrictionType = RecurrenceRestrictionType.NoRestriction
									};
								}
								break;
							case RecurrencePeriodType.Minute:
								{
									recurrenceRule = new RecurrencePattern(FrequencyType.Minutely, interval: Interval)
									{
										Until = calculationEndTime,
										RestrictionType = RecurrenceRestrictionType.NoRestriction
									};
								}
								break;
							case RecurrencePeriodType.Hour:
								{
									recurrenceRule = new RecurrencePattern(FrequencyType.Hourly, interval: Interval)
									{
										Until = calculationEndTime,
										RestrictionType = RecurrenceRestrictionType.NoRestriction
									};
								}
								break;
							case RecurrencePeriodType.Day:
								{
									recurrenceRule = new RecurrencePattern(FrequencyType.Daily, interval: Interval)
									{
										Until = calculationEndTime,
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
										Until = calculationEndTime,
										RestrictionType = RecurrenceRestrictionType.NoRestriction
									};
								}
								break;
							case RecurrencePeriodType.Month:
								{
									if (RepeatMonthType == RecurrenceRepeatMonthType.ByDate)
									{
										recurrenceRule = new RecurrencePattern(FrequencyType.Monthly, interval: Interval)
										{
											ByMonthDay = new List<int> { startTime.Day },
											Until = calculationEndTime,
											RestrictionType = RecurrenceRestrictionType.NoRestriction
										};
									}
									else if (RepeatMonthType == RecurrenceRepeatMonthType.ByWeekDay)
									{
										recurrenceRule = new RecurrencePattern(FrequencyType.Monthly, interval: Interval)
										{
											ByDay = new List<WeekDay> { new WeekDay { DayOfWeek = startTime.DayOfWeek, Offset = GetWeekOfMonth(startTime) } },
											Until = calculationEndTime,
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
									Until = calculationEndTime,
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

			foreach (var occurrence in calendar.GetOccurrences(calculationStartTime, calculationEndTime)) { result.Add(occurrence); }

			if(EndType == RecurrenceEndType.Occurrences && result.Count < OccurrencesCount )
			{
				while(true)
				{
					calculationEndTime = calculationEndTime.AddYears(CALCULATION_EXTEND_YEARS);
					recurrenceRule.Until = calculationEndTime;
					HashSet<Occurrence> occurences = calendar.GetOccurrences(calculationStartTime, calculationEndTime);
					if (occurences.Count < OccurrencesCount)
						continue;

					result = new List<Occurrence>();
					int count = 0;
					foreach (var occurrence in occurences ) 
					{
						count++;
						result.Add(occurrence);
						if (count == OccurrencesCount)
							break;
					}
					break;
				}
			}
			if (EndType == RecurrenceEndType.Occurrences && result.Count > OccurrencesCount)
			{
				result.RemoveRange(OccurrencesCount, result.Count - OccurrencesCount);
			}
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
