using System.ComponentModel;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum FieldRenderMode
	{
		[SelectOption(Label = "Inherit")]
		Undefined = 0,
		[SelectOption(Label = "Form")]
		Form = 1,
		[SelectOption(Label = "Display")]
		Display = 2,
		[SelectOption(Label = "InlineEdit")]
		InlineEdit = 3,
		[SelectOption(Label = "Simple")]
		Simple = 4
	}
}
