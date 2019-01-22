using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum CssBreakpoint
	{
		[SelectOption(Label = "none")]
		None = 0,
		[SelectOption(Label = "xs")]
		XSmall = 1,
		[SelectOption(Label = "sm")]
		Small = 2,
		[SelectOption(Label = "md")]
		Medium = 3,
		[SelectOption(Label = "lg")]
		Large = 4,
		[SelectOption(Label = "xl")]
		XLarge = 5
	}
}
