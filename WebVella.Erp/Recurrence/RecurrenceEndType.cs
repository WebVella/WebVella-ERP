using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Recurrence
{
	public enum RecurrenceEndType
	{
		[SelectOption(Label = "never")]
		Never = 0,
		[SelectOption(Label = "date")]
		Date = 1,
		[SelectOption(Label = "occurrences")]
		Occurrences = 2
	}
}
