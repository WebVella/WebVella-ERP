using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum CssSize
	{
		[SelectOption(Label = "normal")]
		Normal = 0,
		[SelectOption(Label = "small")]
		Small = 1,
		[SelectOption(Label = "large")]
		Large = 2,
		[SelectOption(Label = "inherit")]
		Inherit = 3,
	}
}
