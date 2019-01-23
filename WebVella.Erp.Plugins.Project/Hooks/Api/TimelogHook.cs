using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Project.Services;

namespace WebVella.Erp.Plugins.Project.Hooks.Api
{
	[HookAttachment("timelog")]
	public class TimelogHook : IErpPreCreateRecordHook, IErpPreUpdateRecordHook, IErpPreDeleteRecordHook,
							IErpPostCreateRecordHook, IErpPostUpdateRecordHook, IErpPostDeleteRecordHook
	{

		public void OnPreCreateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			if (!record.Properties.ContainsKey("id"))
				throw new Exception("Hook exception: timelog id field not found in record");

			Guid recordId = (Guid)record["id"];
			var isProjectTimeLog = false;
			var relatedTaskRecords = new EntityRecordList();
			//Get timelog
			if (record["l_scope"] != null && ((string)record["l_scope"]).Contains("projects"))
			{
				isProjectTimeLog = true;
			}

			if (isProjectTimeLog)
			{

				if (record["l_related_records"] != null && (string)record["l_related_records"] != "")
				{
					try
					{
						var relatedRecordGuid = JsonConvert.DeserializeObject<List<Guid>>((string)record["l_related_records"]);
						var taskEqlCommand = "SELECT *,$project_nn_task.id from task WHERE ";
						var filterStringList = new List<string>();
						var taskEqlParams = new List<EqlParameter>();
						var index = 1;
						foreach (var taskGuid in relatedRecordGuid)
						{
							var paramName = "taskId" + index;
							filterStringList.Add($" id = @{paramName} ");
							taskEqlParams.Add(new EqlParameter(paramName, taskGuid));
							index++;
						}
						taskEqlCommand += String.Join(" OR ", filterStringList);
						relatedTaskRecords = new EqlCommand(taskEqlCommand, taskEqlParams).Execute();
					}
					catch
					{
						//Do nothing
					}
				}

				if (!relatedTaskRecords.Any())
					throw new Exception("Hook exception: This timelog does not have an existing taskId");

				var taskRecord = relatedTaskRecords[0]; //Currently should be related only to 1 task in projects
				var patchRecord = new EntityRecord();
				patchRecord["id"] = (Guid)taskRecord["id"];
				var loggedTypeString = "billable";
				if ((bool)record["is_billable"])
				{
					patchRecord["x_billable_minutes"] = (decimal)taskRecord["x_billable_minutes"] + (int)record["minutes"];
				}
				else
				{
					patchRecord["x_nonbillable_minutes"] = (decimal)taskRecord["x_nonbillable_minutes"] + (int)record["minutes"];
					loggedTypeString = "non-billable";
				}
				//Null timelog_started_on
				patchRecord["timelog_started_on"] = null;

				var updateResponse = new RecordManager(executeHooks: false).UpdateRecord("task", patchRecord);
				if (!updateResponse.Success)
					throw new Exception(updateResponse.Message);



				//Add feed record - include all taskIds and related Project ids in the field
				Guid? projectId = null;
				if (((List<EntityRecord>)taskRecord["$project_nn_task"]).Any())
				{
					var projectRecord = ((List<EntityRecord>)taskRecord["$project_nn_task"]).First();
					if (projectRecord != null)
					{
						projectId = (Guid)projectRecord["id"];
					}
				}

				//Add activity log
				var subject = $"logged {((int)record["minutes"]).ToString("N0")} {loggedTypeString} minutes on <a href=\"/projects/tasks/tasks/r/{taskRecord["id"]}/details\">[{taskRecord["key"]}] {taskRecord["subject"]}</a>";
				var relatedRecords = new List<string>() { taskRecord["id"].ToString(), record["id"].ToString() };
				if (projectId != null)
				{
					relatedRecords.Add(projectId.ToString());
				}
				var scope = new List<string>() { "projects" };
				new FeedItemService().Create(id: Guid.NewGuid(), createdBy: SecurityContext.CurrentUser.Id, subject: subject,
					relatedRecords: relatedRecords, scope: scope, type: "timelog");
			}
		}

		public void OnPreUpdateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			//Do nothing
		}

		public void OnPreDeleteRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			if (!record.Properties.ContainsKey("id"))
				throw new Exception("Hook exception: timelog id field not found in record");

			Guid recordId = (Guid)record["id"];
			var isProjectTimeLog = false;
			var timelogRecord = new EntityRecord();
			var relatedTaskRecords = new EntityRecordList();
			//Get timelog

			var eqlCommand = "SELECT * from timelog WHERE id = @recordId";
			var eqlParams = new List<EqlParameter>() { new EqlParameter("recordId", recordId) };
			var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();
			if (!eqlResult.Any())
				throw new Exception("Hook exception: timelog with this id was not found");

			timelogRecord = eqlResult[0];
			if (timelogRecord["l_scope"] != null && ((string)timelogRecord["l_scope"]).Contains("projects"))
			{
				isProjectTimeLog = true;
			}

			if (isProjectTimeLog)
			{

				if (timelogRecord["l_related_records"] != null && (string)timelogRecord["l_related_records"] != "")
				{
					try
					{
						var relatedRecordGuid = JsonConvert.DeserializeObject<List<Guid>>((string)timelogRecord["l_related_records"]);
						var taskEqlCommand = "SELECT *,$project_nn_task.id from task WHERE ";
						var filterStringList = new List<string>();
						var taskEqlParams = new List<EqlParameter>();
						var index = 1;
						foreach (var taskGuid in relatedRecordGuid)
						{
							var paramName = "taskId" + index;
							filterStringList.Add($" id = @{paramName} ");
							taskEqlParams.Add(new EqlParameter(paramName, taskGuid));
							index++;
						}
						taskEqlCommand += String.Join(" OR ", filterStringList);
						relatedTaskRecords = new EqlCommand(taskEqlCommand, taskEqlParams).Execute();
					}
					catch
					{
						//Do nothing
					}
				}

				var taskRecord = relatedTaskRecords[0]; //Currently should be related only to 1 task in projects
				var patchRecord = new EntityRecord();
				patchRecord["id"] = (Guid)taskRecord["id"];
				if ((bool)timelogRecord["is_billable"])
				{
					var result = Math.Round((decimal)taskRecord["x_billable_minutes"] - (decimal)timelogRecord["minutes"]);
					if (result > 0)
						patchRecord["x_billable_minutes"] = result;
					else
						patchRecord["x_billable_minutes"] = 0;
				}
				else
				{
					var result = Math.Round((decimal)taskRecord["x_nonbillable_minutes"] - (decimal)timelogRecord["minutes"]);
					if (result > 0)
						patchRecord["x_nonbillable_minutes"] = result;
					else
						patchRecord["x_nonbillable_minutes"] = 0;
				}
				var updateResponse = new RecordManager(executeHooks: false).UpdateRecord("task", patchRecord);
				if (!updateResponse.Success)
					throw new Exception(updateResponse.Message);

				//Delete feeds that related to this timelog
				var feedEqlCommand = "SELECT id FROM feed_item WHERE l_related_records CONTAINS @recordId";
				var feedEqlParams = new List<EqlParameter>() { new EqlParameter("recordId", recordId) };
				var feedEqlResult = new EqlCommand(feedEqlCommand, feedEqlParams).Execute();
				foreach (var feedId in feedEqlResult)
				{
					var deleteResponse = new RecordManager(executeHooks: false).DeleteRecord("feed_item", (Guid)feedId["id"]);
					if (!deleteResponse.Success)
						throw new Exception(deleteResponse.Message);
				}

			}
		}

		public void OnPostCreateRecord(string entityName, EntityRecord record)
		{
			//Do nothing
		}

		public void OnPostUpdateRecord(string entityName, EntityRecord record)
		{
			//No timelog update implemented yet
		}

		public void OnPostDeleteRecord(string entityName, EntityRecord record)
		{
			//Do nothing
		}

	}
}
