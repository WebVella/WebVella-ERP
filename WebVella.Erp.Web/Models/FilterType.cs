using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum FilterType
	{
		Undefined = 0,
		[SelectOption(Label = "Starts with")]
		STARTSWITH = 1,
		[SelectOption(Label = "Contains")]
		CONTAINS = 2,
		[SelectOption(Label = "Equals")]
		EQ = 3,
		[SelectOption(Label = "Does not equal")]
		NOT = 4,
		[SelectOption(Label = "Less than")]
		LT = 5,
		[SelectOption(Label = "Less than or equal to")]
		LTE = 6,
		[SelectOption(Label = "Greater than")]
		GT = 7,
		[SelectOption(Label = "Greater than or equal to")]
		GTE = 8,
		[SelectOption(Label = "Matches RegEx")]
		REGEX = 9,
		[SelectOption(Label = "Full text search")]
		FTS = 10,
		[SelectOption(Label = "Between")]
		BETWEEN = 11,
		[SelectOption(Label = "Not Between")]
		NOTBETWEEN = 12,
	}
}
