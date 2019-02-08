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
			Name = "WvProjectTrackTimeTasks";
			Description = "Current User's open tasks and track time info";
			ResultModel = "List<EntityRecord>";

			Parameters.Add(new DataSourceParameter { Name = "search_query", Type = "text", Value = "" });
		}

		public override object Execute(Dictionary<string, object> arguments)
		{
			var currentUser = SecurityContext.CurrentUser;
			if (currentUser == null)
				return null;

			var userOpenTasks = new TaskService().GetTaskQueue(null,currentUser.Id,Model.TasksDueType.StartTimeDue, includeProjectData: true);

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
				task["is_billable"] = true;
				if (task.Properties.ContainsKey("$project_nn_task") && task["$project_nn_task"] != null && task["$project_nn_task"] is List<EntityRecord>
					&& ((List<EntityRecord>)task["$project_nn_task"]).Any()) {

					var firstRecord = ((List<EntityRecord>)task["$project_nn_task"])[0];
					if (firstRecord.Properties.ContainsKey("is_billable") && firstRecord["is_billable"] != null && firstRecord["is_billable"] is Boolean) {
						task["is_billable"] = (bool)firstRecord["is_billable"];
					}
				}
			}

			return userOpenTasks.OrderByDescending(x => (DateTime?)x["created_on"]).ToList();
		}
	}
}
