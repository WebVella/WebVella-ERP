using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum HorizontalAlignmentType
	{
		[SelectOption(Label = "None")]
		None = 1,
		[SelectOption(Label = "left")]
		Left = 2,
		[SelectOption(Label = "center")]
		Center = 3,
		[SelectOption(Label = "right")]
		Right = 4
	}
}
