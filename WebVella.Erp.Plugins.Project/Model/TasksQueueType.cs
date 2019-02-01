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
		[SelectOption(Label = "end time overdue")]
		EndTimeOverdue = 1,
		[SelectOption(Label = "end time due today")]
		EndTimeDueToday = 2,
		[SelectOption(Label = "end time not due")]
		EndTimeNotDue = 3,
		[SelectOption(Label = "start time due")]
		StartTimeDue = 4,
		[SelectOption(Label = "start time not due")]
		StartTimeNotDue = 5,
	}
}
