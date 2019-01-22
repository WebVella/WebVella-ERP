using System.ComponentModel;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum TabNavRenderType
	{
		[SelectOption(Label = "Tabs")]
		TABS = 1,
		[SelectOption(Label = "Pills")]
		PILLS = 2
	}
}
