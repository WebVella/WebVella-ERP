using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Plugins.Project.Model
{
	public enum TasksDueType
	{
		[SelectOption(Label = "all")]
		All = 0,
		[SelectOption(Label = "target date overdue")]
		TargetDateOverdue = 1,
		[SelectOption(Label = "target date due today")]
		TargetDateDueToday = 2,
		[SelectOption(Label = "target date not due")]
		TargetDateNotDue = 3,
		[SelectOption(Label = "start date due")]
		StartDateDue = 4,
		[SelectOption(Label = "start date not due")]
		StartDateNotDue = 5,
	}
}
