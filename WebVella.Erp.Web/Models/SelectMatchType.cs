using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum SelectMatchType
	{
		[SelectOption(Label = "contains")]
		Contains = 0,
		[SelectOption(Label = "starts with")]
		StartsWith = 1
	}
}
