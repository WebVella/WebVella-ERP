using System.ComponentModel;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum LabelRenderMode
	{
		[SelectOption(Label = "Inherit")]
		Undefined = 0,
		[SelectOption(Label = "Stacked")]
		Stacked = 1,
		[SelectOption(Label = "Horizontal")]
		Horizontal = 2,
		[SelectOption(Label = "Hidden")]
		Hidden = 3
	}
}
