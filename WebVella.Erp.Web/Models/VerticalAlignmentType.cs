using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum VerticalAlignmentType
	{
		[SelectOption(Label = "None")]
		None = 1,
		[SelectOption(Label = "Top")]
		Top = 2,
		[SelectOption(Label = "Middle")]
		Middle = 3,
		[SelectOption(Label = "Bottom")]
		Bottom = 4
	}
}
