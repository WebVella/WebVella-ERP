using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum PeriodType
	{
		[SelectOption(Label = "second")]
		Second = 0,
		[SelectOption(Label = "minute")]
		Minute = 1,
		[SelectOption(Label = "hour")]
		Hour = 2,
		[SelectOption(Label = "day")]
		Day = 3,
		[SelectOption(Label = "week")]
		Week = 4,
		[SelectOption(Label = "month")]
		Month = 5,
		[SelectOption(Label = "year")]
		Year = 6
	}
}
