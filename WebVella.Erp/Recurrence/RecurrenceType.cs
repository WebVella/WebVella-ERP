using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Recurrence
{
	public enum RecurrenceType
	{
		[SelectOption(Label = "does not repeat")]
		DoesNotRepeat = 0,
		[SelectOption(Label = "daily, same hour")]
		Daily = 1,
		[SelectOption(Label = "weekly, same day and hour")]
		Weekly = 2,
		[SelectOption(Label = "monthly, same date (eg. each 10th date)")]
		MonthlyByDate = 3,
		[SelectOption(Label = "monthly, same week day occurrence (eg. First Monday)")]
		MonthlyByWeekDay = 4,
		[SelectOption(Label = "annually, same date and hour")]
		Annually = 5,
		[SelectOption(Label = "every weekday, same hour")]
		EveryWeekDay = 6,
		[SelectOption(Label = "custom")]
		Custom = 7
	}
}
