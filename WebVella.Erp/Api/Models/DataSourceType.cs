using System.ComponentModel;

namespace WebVella.Erp.Api.Models
{
	public enum DataSourceType
	{
		[SelectOption(Label = "database")]
		DATABASE = 0,
		[SelectOption(Label = "code")]
		CODE = 1
	}
}
