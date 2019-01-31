using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Plugins.Project.Model;
using WebVella.Erp.Web.Models;


//TODO develop service
namespace WebVella.Erp.Plugins.Project.Services
{
	public class TaskService : BaseService
	{
		/// <summary>
		/// Calculates the key and x_search contents and updates the task
		/// </summary>
		/// <param name="data"></param>
		public EntityRecord SetCalculationFields(Guid taskId, out string subject, out Guid projectId, out Guid? projectOwnerId)
		{
			subject = "";
			projectId = Guid.Empty;
			projectOwnerId = null;

			EntityRecord patchRecord = new EntityRecord();

			var getTaskResponse = new RecordManager().Find(new EntityQuery("task", "*,$task_type_1n_task.label,$task_status_1n_task.label,$project_nn_task.abbr,$project_nn_task.id, $project_nn_task.owner_id", EntityQuery.QueryEQ("id", taskId)));
			if (!getTaskResponse.Success)
				throw new Exception(getTaskResponse.Message);
			if (!getTaskResponse.Object.Data.Any())
				throw new Exception("Task with this Id was not found");

			var taskRecord = getTaskResponse.Object.Data.First();
			subject = (string)taskRecord["subject"];
			var projectAbbr = "";
			var status = "";
			var type = "";
			if (((List<EntityRecord>)taskRecord["$project_nn_task"]).Any())
			{
				var projectRecord = ((List<EntityRecord>)taskRecord["$project_nn_task"]).First();
				if (projectRecord != null && projectRecord.Properties.ContainsKey("abbr"))
				{
					projectAbbr = (string)projectRecord["abbr"];
				}
				if (projectRecord != null) {
					projectId = (Guid)projectRecord["id"];
					projectOwnerId = (Guid?)projectRecord["owner_id"];
				}
			}
			if (((List<EntityRecord>)taskRecord["$task_status_1n_task"]).Any())
			{
				var statusRecord = ((List<EntityRecord>)taskRecord["$task_status_1n_task"]).First();
				if (statusRecord != null && statusRecord.Properties.ContainsKey("label"))
				{
					status = (string)statusRecord["label"];
				}
			}
			if (((List<EntityRecord>)taskRecord["$task_type_1n_task"]).Any())
			{
				var typeRecord = ((List<EntityRecord>)taskRecord["$task_type_1n_task"]).First();
				if (typeRecord != null && typeRecord.Properties.ContainsKey("label"))
				{
					type = (string)typeRecord["label"];
				}
			}

			patchRecord["id"] = taskId;
			patchRecord["key"] = projectAbbr + "-" + taskRecord["number"];
			var searchIndex = ""; // FTS is included to be used as a default string when you need all records to be returned
			searchIndex += $" {patchRecord["key"]} ";
			searchIndex += $" {taskRecord["subject"]} ";
			searchIndex += $" {taskRecord["body"]} ";
			searchIndex += $" {taskRecord["priority"]} ";


			searchIndex += $" {status} ";
			searchIndex += $" {type} ";


			//TODO Add Comments

			patchRecord["x_search"] = searchIndex;

			return patchRecord;
		}

		public EntityRecordList GetTaskStatuses() {
			var projectRecord = new EntityRecord();
			var eqlCommand = "SELECT * from task_status";
			var eqlParams = new List<EqlParameter>();
			var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();
			if (!eqlResult.Any())
				throw new Exception("Error: No task statuses found");

			return eqlResult;
		}

		public EntityRecord GetTask(Guid taskId)
		{
			var projectRecord = new EntityRecord();
			var eqlCommand = " SELECT * from task WHERE id = @taskId";
			var eqlParams = new List<EqlParameter>() { new EqlParameter("taskId", taskId) };

			var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();
			if (!eqlResult.Any()) 
				return null;
			else 
				return eqlResult[0];
		}

		public EntityRecordList GetTaskQueue(Guid? projectId, Guid? userId, TasksDueType type = TasksDueType.All, int? limit = null, bool includeProjectData = false)
		{
			var selectedFields = "*";
			if (includeProjectData) {
				selectedFields += ",$project_nn_task.is_billable";
			}

			var eqlCommand = $"SELECT {selectedFields} from task ";
			var eqlParams = new List<EqlParameter>();
			eqlParams.Add(new EqlParameter("currentDate", DateTime.Now.Date));

			var whereFilters = new List<string>();

			// Start date
			if (type == TasksDueType.StartDateDue)
				whereFilters.Add("(start_date <= @currentDate OR start_date = null)");
			if (type == TasksDueType.StartDateNotDue)
				whereFilters.Add("start_date > @currentDate");

			// Target date
			if (type == TasksDueType.TargetDateOverdue)
				whereFilters.Add("target_date < @currentDate");
			if (type == TasksDueType.TargetDateDueToday)
				whereFilters.Add("target_date = @currentDate");
			if (type == TasksDueType.TargetDateNotDue)
				whereFilters.Add("(target_date > @currentDate OR target_date = null)");
			
			// Project and user
			if (projectId != null && userId != null)
			{
				whereFilters.Add("$project_nn_task.id = @projectId AND owner_id = @userId");
				eqlParams.Add(new EqlParameter("projectId", projectId));
				eqlParams.Add(new EqlParameter("userId", userId));
			}
			else if (projectId != null)
			{
				whereFilters.Add("$project_nn_task.id = @projectId");
				eqlParams.Add(new EqlParameter("projectId", projectId));
			}
			else if (userId != null) {
				whereFilters.Add("owner_id = @userId");
				eqlParams.Add(new EqlParameter("userId", userId));
			}

			//Status open
			if (type != TasksDueType.All)
			{
				var taskStatuses = new TaskService().GetTaskStatuses();
				var closedStatusHashset = new HashSet<Guid>();
				foreach (var taskStatus in taskStatuses)
				{
					if ((bool)taskStatus["is_closed"])
					{
						closedStatusHashset.Add((Guid)taskStatus["id"]);
					}
				}
				var index = 1;
				foreach (var key in closedStatusHashset)
				{
					var paramName = "status" + index;
					whereFilters.Add($"status_id <> @{paramName}");
					eqlParams.Add(new EqlParameter(paramName, key));
					index++;
				}
			}

			if (whereFilters.Count > 0)
			{
				eqlCommand += " WHERE " + string.Join(" AND ", whereFilters);
			}


			//Order
			switch (type) {
				case TasksDueType.All:
					// No sort for optimization purposes
					break;
				case TasksDueType.TargetDateOverdue:
					eqlCommand += $" ORDER BY target_date ASC, priority DESC";
					break;
				case TasksDueType.TargetDateDueToday:
					eqlCommand += $" ORDER BY priority DESC";
					break;
				case TasksDueType.TargetDateNotDue:
					eqlCommand += $" ORDER BY target_date ASC, priority DESC";
					break;
				case TasksDueType.StartDateDue:
					eqlCommand += $" ORDER BY target_date ASC, priority DESC";
					break;
				case TasksDueType.StartDateNotDue:
					eqlCommand += $" ORDER BY target_date ASC, priority DESC";
					break;
				default:
					throw new Exception("Unknown TasksDueType");
			}


			//Limit
			if(limit != null)
				eqlCommand += $" PAGE 1 PAGESIZE {limit} ";


			var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();

			return eqlResult;
		}

		public void GetTaskIconAndColor(string priorityValue, out string iconClass, out string color) {
			iconClass = "";
			color = "#fff";

			var priorityOptions = ((SelectField)new EntityManager().ReadEntity("task").Object.Fields.First(x => x.Name == "priority")).Options;
			var recordPriority = priorityOptions.FirstOrDefault(x => x.Value == priorityValue);
			if (recordPriority != null)
			{
				iconClass = recordPriority.IconClass;
				color = recordPriority.Color;
			}

		}

		public void StartTaskTimelog(Guid taskId) {
			var patchRecord = new EntityRecord();
			patchRecord["id"] = taskId;
			patchRecord["timelog_started_on"] = DateTime.Now;
			var updateResponse = new RecordManager().UpdateRecord("task", patchRecord);
			if (!updateResponse.Success)
				throw new Exception(updateResponse.Message);
		}

		public void StopTaskTimelog(Guid taskId)
		{
			//Create transaction
			var patchRecord = new EntityRecord();
			patchRecord["id"] = taskId;
			patchRecord["timelog_started_on"] = null;
			var updateResponse = new RecordManager().UpdateRecord("task", patchRecord);
			if (!updateResponse.Success)
				throw new Exception(updateResponse.Message);
		}

		public EntityRecord GetPageHookLogic(BaseErpPageModel pageModel, EntityRecord record) {

			if (record == null)
				record = new EntityRecord();

			//Preselect owner
			ErpUser currentUser = (ErpUser)pageModel.DataModel.GetProperty("CurrentUser");
			if (currentUser != null)
				record["owner_id"] = currentUser.Id;
			//$project_nn_task.id
			//Preselect project
			if (pageModel.HttpContext.Request.Query.ContainsKey("projectId"))
			{
				var projectQueryId = pageModel.HttpContext.Request.Query["projectId"].ToString();
				if (Guid.TryParse(projectQueryId, out Guid outGuid))
				{
					var projectIdList = new List<Guid>();
					projectIdList.Add(outGuid);
					record["$project_nn_task.id"] = projectIdList;
				}
			}
			else
			{
				var eqlCommand = "SELECT created_on,type_id,$project_nn_task.id FROM task WHERE created_by = @currentUserId ORDER BY created_on PAGE 1 PAGESIZE 1";
				var eqlParams = new List<EqlParameter>() { new EqlParameter("currentUserId", currentUser.Id) };
				var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();
				if (eqlResult != null && eqlResult is EntityRecordList && eqlResult.Count > 0)
				{
					var relatedProjects = (List<EntityRecord>)eqlResult[0]["$project_nn_task"];
					if (relatedProjects.Count > 0)
					{
						var projectIdList = new List<Guid>();
						projectIdList.Add((Guid)relatedProjects[0]["id"]);
						record["$project_nn_task.id"] = projectIdList;
					}
					record["type_id"] = (Guid?)eqlResult[0]["type_id"];
				}
			}

			//Preset start date
			record["start_date"] = DateTime.Now;
			record["target_date"] = DateTime.Now.AddDays(1);
			return record;
		}


		public void PreCreateRecordPageHookLogic(string entityName, EntityRecord record, List<ErrorModel> errors) 
		{
			if (!record.Properties.ContainsKey("$project_nn_task.id"))
			{
				errors.Add(new ErrorModel()
				{
					Key = "$project_nn_task.id",
					Message = "Project is not specified."
				});
			}
			else
			{
				var projectRecord = (List<Guid>)record["$project_nn_task.id"];
				if (projectRecord.Count == 0)
				{
					errors.Add(new ErrorModel()
					{
						Key = "$project_nn_task.id",
						Message = "Project is not specified."
					});
				}
				else if (projectRecord.Count > 1)
				{
					errors.Add(new ErrorModel()
					{
						Key = "$project_nn_task.id",
						Message = "More than one project is selected."
					});
				}
			}
		}

		public void PostCreateApiHookLogic(string entityName, EntityRecord record) {
			//Update key and search fields
			Guid projectId = Guid.Empty;
			Guid? projectOwnerId = null;
			string taskSubject = "";
			var patchRecord = new TaskService().SetCalculationFields((Guid)record["id"], subject: out taskSubject, projectId: out projectId, projectOwnerId: out projectOwnerId);
			var updateResponse = new RecordManager(executeHooks: false).UpdateRecord("task", patchRecord);
			if (!updateResponse.Success)
				throw new Exception(updateResponse.Message);

			//Set the initial watchers list - project lead, creator, owner
			var watchers = new List<Guid>();
			{
				var fieldName = "owner_id";
				if (record.Properties.ContainsKey(fieldName) && record[fieldName] != null) {
					var userId = (Guid)record[fieldName];
					if (!watchers.Contains(userId))
						watchers.Add(userId);
				}
			}
			{
				var fieldName = "created_by";
				if (record.Properties.ContainsKey(fieldName) && record[fieldName] != null)
				{
					var userId = (Guid)record[fieldName];
					if (!watchers.Contains(userId))
						watchers.Add(userId);
				}
			}
			if (projectOwnerId != null) {
				if (!watchers.Contains(projectOwnerId.Value))
					watchers.Add(projectOwnerId.Value);
			}

			//Create relations
			var watchRelation = new EntityRelationManager().Read("user_nn_task_watchers").Object;
			if (watchRelation == null)
				throw new Exception("Watch relation not found");

			foreach (var userId in watchers)
			{
				var createRelResponse = new RecordManager().CreateRelationManyToManyRecord(watchRelation.Id, userId, (Guid)record["id"]);
				if (!createRelResponse.Success)
					throw new Exception(createRelResponse.Message);
			}


			//Add activity log
			var subject = $"created <a href=\"/projects/tasks/tasks/r/{patchRecord["id"]}/details\">[{patchRecord["key"]}] {taskSubject}</a>";
			var relatedRecords = new List<string>() { patchRecord["id"].ToString(), projectId.ToString() };
			var scope = new List<string>() { "projects" };
			//Add watchers as scope
			foreach (var userId in watchers)
			{
				relatedRecords.Add(userId.ToString());
			}
			var taskSnippet = new Web.Services.RenderService().GetSnippetFromHtml((string)record["body"]);
			new FeedItemService().Create(id: Guid.NewGuid(), createdBy: SecurityContext.CurrentUser.Id, subject: subject,
				body: taskSnippet, relatedRecords: relatedRecords, scope: scope, type: "task");
		}

		public void PostUpdateApiHookLogic(string entityName, EntityRecord record)
		{
			//Update key and search fields
			Guid projectId = Guid.Empty;
			Guid? projectOwnerId = null;
			string taskSubject = "";
			var patchRecord = new TaskService().SetCalculationFields((Guid)record["id"], subject: out taskSubject, projectId: out projectId, projectOwnerId: out projectOwnerId);
			var updateResponse = new RecordManager(executeHooks: false).UpdateRecord("task", patchRecord);
			if (!updateResponse.Success)
				throw new Exception(updateResponse.Message);
		}

		public void SetStatus(Guid taskId, Guid statusId)
		{
			var patchRecord = new EntityRecord();
			patchRecord["id"] = taskId;
			patchRecord["status_id"] = statusId;
			var updateResponse = new RecordManager().UpdateRecord("task", patchRecord);
			if (!updateResponse.Success)
				throw new Exception(updateResponse.Message);
		}

		public List<EntityRecord> GetTasksThatNeedStarting() {
			var eqlCommand = "SELECT id FROM task WHERE status_id = @notStartedStatusId AND start_date <= @currentDate";
			var eqlParams = new List<EqlParameter>() {
				new EqlParameter("notStartedStatusId", new Guid("f3fdd750-0c16-4215-93b3-5373bd528d1f")),
				new EqlParameter("currentDate", DateTime.Now.Date),
			};

			return new EqlCommand(eqlCommand, eqlParams).Execute();
		}
	}
}
