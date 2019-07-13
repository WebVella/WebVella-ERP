using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Pages.Application;


//TODO develop service
namespace WebVella.Erp.Plugins.Project.Services
{
	public class TimeLogService : BaseService
	{
		public void Create(Guid? id = null, Guid? createdBy = null, DateTime? createdOn = null, DateTime? loggedOn = null, int minutes = 0, bool isBillable = true, string body = "",
			List<string> scope = null, List<Guid> relatedRecords = null)
		{
			#region << Init >>
			if (id == null)
				id = Guid.NewGuid();

			if (createdBy == null)
				createdBy = SystemIds.SystemUserId;

			if (createdOn == null)
				createdOn = DateTime.UtcNow;

			if (loggedOn == null)
				loggedOn = DateTime.UtcNow;
			#endregion

			try
			{
				var record = new EntityRecord();
				record["id"] = id;
				record["created_by"] = createdBy;
				record["created_on"] = createdOn;
				record["logged_on"] = loggedOn.ConvertAppDateToUtc();
				record["body"] = body;
				record["minutes"] = minutes;
				record["is_billable"] = isBillable;
				record["l_scope"] = JsonConvert.SerializeObject(scope);
				record["l_related_records"] = JsonConvert.SerializeObject(relatedRecords);

				var response = RecMan.CreateRecord("timelog", record);
				if (!response.Success)
				{
					throw new ValidationException(response.Message);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Delete(Guid recordId)
		{
			//Validate - only authors can start to delete their posts and comments. Moderation will be later added if needed
			{
				var eqlCommand = "SELECT id,created_by FROM timelog WHERE id = @recordId";
				var eqlParams = new List<EqlParameter>() { new EqlParameter("recordId", recordId) };
				var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();
				if (!eqlResult.Any())
					throw new Exception("RecordId not found");
				if ((Guid)eqlResult[0]["created_by"] != SecurityContext.CurrentUser.Id)
					throw new Exception("Only the author can delete its comment");
			}

			var deleteResponse = new RecordManager().DeleteRecord("timelog", recordId);
			if (!deleteResponse.Success)
			{
				throw new Exception(deleteResponse.Message);
			}



		}

		public EntityRecordList GetTimelogsForPeriod(Guid? projectId, Guid? userId, DateTime startDate, DateTime endDate)
		{
			var projectRecord = new EntityRecord();
			var eqlCommand = "SELECT * from timelog WHERE logged_on >= @startDate AND logged_on < @endDate ";
			var eqlParams = new List<EqlParameter>() { new EqlParameter("startDate", startDate), new EqlParameter("endDate", endDate) };

			if (projectId != null)
			{
				eqlCommand += " AND l_related_records CONTAINS @projectId";
				eqlParams.Add(new EqlParameter("projectId", projectId));
			}
			if (userId != null)
			{
				eqlCommand += " AND created_by = @userId";
				eqlParams.Add(new EqlParameter("userId", userId));
			}
			if (userId != null) { }

			var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();

			return eqlResult;
		}

		public IActionResult PostApplicationNodePageHookLogic(ApplicationNodePageModel pageModel)
		{
			Guid TIMETRACK_PAGE_ID = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
			var page = (ErpPage)pageModel.DataModel.GetProperty("Page");

			var currentUser = SecurityContext.CurrentUser;
			if (page != null && page.Id == TIMETRACK_PAGE_ID && currentUser != null)
			{
				var postForm = pageModel.PageContext.HttpContext.Request.Form;
				if (String.IsNullOrWhiteSpace(postForm["task_id"]))
					throw new Exception("Task Id is required");
				if (String.IsNullOrWhiteSpace(postForm["minutes"]))
					throw new Exception("Minutes is required");
				if (String.IsNullOrWhiteSpace(postForm["logged_on"]))
					throw new Exception("Logged on is required");

				Guid taskId = Guid.Empty;
				DateTime loggedOn = DateTime.MinValue;
				string body = !String.IsNullOrWhiteSpace(postForm["body"]) ? postForm["body"].ToString() : "";
				bool isBillable = true;
				int minutes = 0;

				if (Guid.TryParse(postForm["task_id"].ToString(), out Guid outTaskId))
					taskId = outTaskId;
				if (DateTime.TryParse(postForm["logged_on"].ToString(), out DateTime outLoggedOn))
					loggedOn = outLoggedOn;
				if (Boolean.TryParse(postForm["is_billable"].ToString(), out Boolean outBillable))
					isBillable = outBillable;
				if (int.TryParse(postForm["minutes"].ToString(), out int outMinutes))
					minutes = outMinutes;

				var eqlCommand = " SELECT *,$project_nn_task.id from task WHERE id = @taskId";
				var eqlParams = new List<EqlParameter>() { new EqlParameter("taskId", taskId) };
				var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();
				if (!eqlResult.Any())
					throw new Exception("Task with taskId not found");

				var taskRecord = eqlResult[0];
				var scope = new List<string>() { "projects" };
				var relatedRecords = new List<Guid>() { (Guid)taskRecord["id"] };

				if (taskRecord.Properties.ContainsKey("$project_nn_task") && ((List<EntityRecord>)taskRecord["$project_nn_task"]).Any())
				{
					var projectRecord = ((List<EntityRecord>)taskRecord["$project_nn_task"]).First();
					relatedRecords.Add((Guid)projectRecord["id"]);
				}

				using (var connection = DbContext.Current.CreateConnection())
				{
					try
					{
						connection.BeginTransaction();
						new TaskService().StopTaskTimelog(taskId);
						if (postForm["minutes"].ToString() != "0")
						{
							//Zero minutes are not logged
							new TimeLogService().Create(null, currentUser.Id, DateTime.Now, loggedOn, minutes, isBillable, body, scope, relatedRecords);
						}
						connection.CommitTransaction();
						return new RedirectResult("/projects/track-time/track-time/a/track-time");
					}
					catch (Exception ex)
					{
						connection.RollbackTransaction();
						throw ex;
					}
				}

			}

			return null;
		}

		public void PreCreateApiHookLogic(string entityName, EntityRecord record, List<ErrorModel> errors)
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
						var taskEqlCommand = "SELECT *,$project_nn_task.id, $user_nn_task_watchers.id FROM task WHERE ";
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
					catch(Exception ex)
					{
						throw ex;
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

				var taskWatchersList = new List<string>();
				if (((List<EntityRecord>)taskRecord["$user_nn_task_watchers"]).Any())
				{
					taskWatchersList = ((List<EntityRecord>)taskRecord["$user_nn_task_watchers"]).Select(x => ((Guid)x["id"]).ToString()).ToList();
				}

				//Add activity log
				var subject = $"logged {((int)record["minutes"]).ToString("N0")} {loggedTypeString} minutes on <a href=\"/projects/tasks/tasks/r/{taskRecord["id"]}/details\">[{taskRecord["key"]}] {taskRecord["subject"]}</a>";
				var relatedRecords = new List<string>() { taskRecord["id"].ToString(), record["id"].ToString() };
				if (projectId != null)
				{
					relatedRecords.Add(projectId.ToString());
				}
				relatedRecords.AddRange(taskWatchersList);

				var scope = new List<string>() { "projects" };
				var logSnippet = new Web.Services.RenderService().GetSnippetFromHtml((string)record["body"]);
				new FeedItemService().Create(id: Guid.NewGuid(), createdBy: SecurityContext.CurrentUser.Id, subject: subject,
					body: logSnippet, relatedRecords: relatedRecords, scope: scope, type: "timelog");
			}
		}

		public void PreDeleteApiHookLogic(string entityName, EntityRecord record, List<ErrorModel> errors)
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
	}
}
