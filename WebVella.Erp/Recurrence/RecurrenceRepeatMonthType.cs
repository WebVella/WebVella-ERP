using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Recurrence
{
	public enum RecurrenceRepeatMonthType
	{
		[SelectOption(Label = "by day")]
		ByDate = 0,
		[SelectOption(Label = "by week day")]
		ByWeekDay = 1
	}
}
