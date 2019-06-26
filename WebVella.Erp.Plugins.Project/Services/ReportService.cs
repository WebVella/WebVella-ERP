using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;

namespace WebVella.Erp.Plugins.Project.Services
{
	public class ReportService
	{
		public List<EntityRecord> GetTimelogData(int year, int month, Guid? accountId)
		{
			ValidationException valEx = new ValidationException();

			if (month > 12 || month <= 0)
				valEx.AddError("month", "Invalid month.");

			if (year <= 0)
				valEx.AddError("year", "Invalid year.");

			List<EqlParameter> eqlParams;
			if (accountId.HasValue)
			{
				eqlParams = new List<EqlParameter>() { new EqlParameter("id", accountId.Value) };
				var eqlResult = new EqlCommand("SELECT * FROM account WHERE id = @id ", eqlParams).Execute();
				if (!eqlResult.Any())
					valEx.AddError("accountId", $"Account with ID:{accountId} not found.");
			}

			valEx.CheckAndThrow();

			/// load timelog records from database
			DateTime fromDate = new DateTime(year, month, 1);
			DateTime toDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
			eqlParams = new List<EqlParameter>() {
				new EqlParameter("from_date", fromDate),
				new EqlParameter("to_date", toDate),
				new EqlParameter("scope", "projects" ),
			};
			string eql = @"SELECT id,is_billable,l_related_records,minutes FROM timelog 
						   WHERE logged_on >= @from_date AND 
							  logged_on <= @to_date AND
							  l_scope CONTAINS @scope";
			var timelogRecords = new EqlCommand(eql, eqlParams).Execute();

			HashSet<Guid> setOfTasksWithTimelog = new HashSet<Guid>();
			foreach(var timelog in timelogRecords )
			{
				List<Guid> ids = JsonConvert.DeserializeObject<List<Guid>>((string)timelog["l_related_records"]);
				Guid taskId = ids[0];
				timelog["task_id"] = taskId;
				if (!setOfTasksWithTimelog.Contains(taskId))
					setOfTasksWithTimelog.Add(taskId);
			}

			//load all tasks 
			eqlParams = new List<EqlParameter>();
			eql = @"SELECT id,subject, $project_nn_task.id, $project_nn_task.name,$project_nn_task.account_id, $task_type_1n_task.label FROM task";
			var tasks = new EqlCommand(eql, eqlParams).Execute();

			//process tasks - split records for projects - filter by account - filter by timelog
			EntityRecordList processedTasks = new EntityRecordList();
			foreach (var task in tasks)
			{
				//skip task that has no timelog record
				if (!setOfTasksWithTimelog.Contains((Guid)task["id"]))
					continue;

				List<EntityRecord> taskProjects = (List<EntityRecord>)task["$project_nn_task"];
				//skip tasks with no project
				if (taskProjects.Count == 0)
					continue;

				//split tasks to projects if more than one project is related to task
				foreach (var project in taskProjects)
				{
					if (accountId != null )
					{
						if(project["account_id"] == null){
							throw new Exception("There is a project without an account");
						}

						if ((Guid)project["account_id"] == accountId)
						{
							task["project"] = project;
							processedTasks.Add(task);
						}
					}
					else
					{
						task["project"] = project;
						processedTasks.Add(task);
					}
				}
			}
			tasks = processedTasks;
			tasks.TotalCount = processedTasks.Count;



			List<EntityRecord> result = new List<EntityRecord>();
			foreach(var task in tasks )
			{
				EntityRecord rec = new EntityRecord();
				rec["task_id"] = task["id"];
				rec["project_id"] = ((EntityRecord)task["project"])["id"];
				rec["task_subject"] = task["subject"];
				rec["project_name"] = ((EntityRecord)task["project"])["name"];
				rec["task_type"] = ((List<EntityRecord>)task["$task_type_1n_task"])[0]["label"];
				rec["billable_minutes"] = (decimal)0;
				rec["non_billable_minutes"] = (decimal)0;


				//during development
				//rec["task"] = task;
				//rec["project"] = (EntityRecord)task["project"];
				
				foreach (var timelog in timelogRecords)
				{
					if ((Guid)timelog["task_id"] != (Guid)task["id"])
						continue;

					if ((bool)timelog["is_billable"])
						rec["billable_minutes"] = (decimal)rec["billable_minutes"] + (decimal)timelog["minutes"];
					else
						rec["non_billable_minutes"] = (decimal)rec["non_billable_minutes"] + (decimal)timelog["minutes"];
				}

				result.Add(rec);
			}

			return result;
		}
	}
}
