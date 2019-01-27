using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Plugins.Project.Services;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.Project.DataSource
{
	public class TrackTimeTasks : CodeDataSource
	{
		public TrackTimeTasks() : base()
		{
			Id = new Guid("473EE9B6-2131-4164-B5FE-D9B3073E9178");
			Name = "TrackTimeTasks";
			Description = "Current User's open tasks and track time info";
			ResultModel = "List<EntityRecord>";

			Parameters.Add(new DataSourceParameter { Name = "search_query", Type = "text", Value = "" });
		}

		public override object Execute(Dictionary<string, object> arguments)
		{
			var currentUser = SecurityContext.CurrentUser;
			if (currentUser == null)
				return null;

			var userOpenTasks = new TaskService().GetTaskQueue(null,currentUser.Id,Model.TasksDueType.StartDateDue);

			var userTimelogs = new List<EntityRecord>();

			{
				var eqlCommand = "SELECT * from timelog WHERE created_by = @userId";
				var eqlParams = new List<EqlParameter>() { new EqlParameter("userId", currentUser.Id) };
				userTimelogs = new EqlCommand(eqlCommand, eqlParams).Execute();
			}

			foreach (var task in userOpenTasks)
			{
				task.Properties.Add("logged_minutes", (int)0);
				task.Properties.Add("last_logged_on", (DateTime)DateTime.MinValue);
				var taskTimelogs = userTimelogs.FindAll(x => (x["l_related_records"] == null ? "" : (string)x["l_related_records"]).Contains(task["id"].ToString())).ToList();
				if (taskTimelogs.Count > 0)
				{
					task["logged_minutes"] = (int)(taskTimelogs.Select(x => (decimal)x["minutes"]).Aggregate(func: (aggrResult, x) => aggrResult + x));
					var lastLog = taskTimelogs.OrderByDescending(x => (DateTime)x["created_on"]).FirstOrDefault();
					if (lastLog != null)
						task["last_logged_on"] = (DateTime)lastLog["created_on"];
				}
			}

			return userOpenTasks.OrderByDescending(x => (DateTime?)x["timelog_started_on"]).ThenByDescending(x => (DateTime)x["last_logged_on"]).ToList();
		}
	}
}
