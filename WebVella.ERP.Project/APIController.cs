using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.WebUtilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Database;
using WebVella.ERP.Web.Security;

namespace WebVella.ERP.Project
{
	[Authorize]
	public class ApiController : Controller
	{
		RecordManager recMan;
		EntityManager entityManager;
		EntityRelationManager entityRelationManager;
		SecurityManager secMan;

		public ApiController()
		{
			recMan = new RecordManager();
			secMan = new SecurityManager();
			entityManager = new EntityManager();
			entityRelationManager = new EntityRelationManager();
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/project/list/my-projects")]
		public IActionResult MyProjects(string listName = null, string entityName = null, int page = 0)
		{
			var response = new ResponseModel();
			//var queryString = HttpContext.Request.QueryString;
			#region << Can user read projects >>
			//Get current user
			ErpUser user = SecurityContext.CurrentUser;
			//Get entity meta
			var entity = entityManager.ReadEntity(entityName).Object;
			//Get list meta
			var list = entityManager.ReadRecordList(entity.Name, listName).Object;
			//check if user role has permissions
			var canRead = user.Roles.Any(x => entity.RecordPermissions.CanRead.Any(z => z == x.Id));
			var canCreate = user.Roles.Any(x => entity.RecordPermissions.CanCreate.Any(z => z == x.Id));
			var canUpdate = user.Roles.Any(x => entity.RecordPermissions.CanUpdate.Any(z => z == x.Id));
			var canDelete = user.Roles.Any(x => entity.RecordPermissions.CanDelete.Any(z => z == x.Id));

			if (!canRead)
			{
				response.Success = false;
				response.Message = "You do not have permission to read the projects in this system";
				response.Timestamp = DateTime.UtcNow;
				return Json(response); //return empty object
			}
			#endregion

			#region << Init fields >>
			var requestedFields = "id,name,start_date,end_date," +
			"x_milestones_opened,x_milestones_completed,x_tasks_not_started,x_tasks_in_progress,x_tasks_completed,x_bugs_opened,x_bugs_reopened,x_bugs_closed," +
			"$user_1_n_project_owner.id,$user_1_n_project_owner.image,$user_1_n_project_owner.username," +
			"$role_n_n_project_team.id,$role_n_n_project_customer.id";
			#endregion

			#region << Query builder >>
			//QueryObject filterObj = EntityQuery.QueryEQ("id", recordId);
			QueryObject filterObj = null;
			EntityQuery resultQuery = new EntityQuery("wv_project", requestedFields, filterObj, null, null, null, null);
			#endregion

			#region << Sort >>

			#endregion

			#region << Execute >>
			QueryResponse result = recMan.Find(resultQuery);
			var resultRecordsList = new List<EntityRecord>();
			if (!result.Success)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = result.Message;
				response.Object = null;
				return Json(response);
			}
			foreach (var record in result.Object.Data)
			{
				//Check if user can view the object
				var userIsPM = false;
				var userIsStaff = false;
				var userIsCustomer = false;
				#region << Check user roles >>
				foreach (var userRole in user.Roles)
				{
					userIsPM = ((List<EntityRecord>)record["$user_1_n_project_owner"]).Any(z => (Guid)z["id"] == user.Id);
					userIsStaff = ((List<EntityRecord>)record["$role_n_n_project_team"]).Any(z => (Guid)z["id"] == userRole.Id);
					userIsCustomer = ((List<EntityRecord>)record["$role_n_n_project_customer"]).Any(z => (Guid)z["id"] == userRole.Id);
				}
				#endregion

				if (userIsPM || userIsStaff || userIsCustomer)
				{
					var recordObj = new EntityRecord();
					recordObj["id"] = record["id"];
					recordObj["name"] = record["name"];
					recordObj["start_date"] = record["start_date"];
					recordObj["end_date"] = record["end_date"];
					recordObj["owner_image"] = ((List<EntityRecord>)record["$user_1_n_project_owner"])[0]["image"];
					recordObj["owner_username"] = ((List<EntityRecord>)record["$user_1_n_project_owner"])[0]["username"];

					#region << milestones Count "opened" vs "completed" >>
					var milestonesOpened = (decimal)record["x_milestones_opened"];
					var milestonesCompleted = (decimal)record["x_milestones_completed"];
					recordObj["milestones_opened_count"] = milestonesOpened;
					recordObj["milestones_completed_count"] = milestonesCompleted;
					if (milestonesOpened + milestonesCompleted > 0)
					{
						recordObj["milestones_opened_percentage"] = Math.Round((decimal)(milestonesOpened * 100) / (milestonesOpened + milestonesCompleted));
						recordObj["milestones_completed_percentage"] = 100 - Math.Round((decimal)(milestonesOpened * 100) / (milestonesOpened + milestonesCompleted));
					}
					else
					{
						recordObj["milestones_opened_percentage"] = 0;
						recordObj["milestones_completed_percentage"] = 0;
					}

					#endregion

					#region << tasks Count "not started" vs "in progress" vs "completed" >>
					var tasksNotStarted = (decimal)record["x_tasks_not_started"];
					var tasksInProgress = (decimal)record["x_tasks_in_progress"];
					var tasksCompleted = (decimal)record["x_tasks_completed"];

					recordObj["tasks_not_started_count"] = tasksNotStarted;
					recordObj["tasks_in_progress_count"] = tasksInProgress;
					recordObj["tasks_completed_count"] = tasksCompleted;
					if (tasksNotStarted + tasksInProgress + tasksCompleted > 0)
					{
						recordObj["tasks_not_started_percentage"] = Math.Round((decimal)(tasksNotStarted * 100) / (tasksNotStarted + tasksInProgress + tasksCompleted));
						recordObj["tasks_in_progress_percentage"] = Math.Round((decimal)(tasksInProgress * 100) / (tasksNotStarted + tasksInProgress + tasksCompleted));
						recordObj["tasks_completed_percentage"] = 100 - Math.Round((decimal)(tasksNotStarted * 100) / (tasksNotStarted + tasksInProgress + tasksCompleted)) - Math.Round((decimal)(tasksInProgress * 100) / (tasksNotStarted + tasksInProgress + tasksCompleted));
					}
					else
					{
						recordObj["tasks_not_started_percentage"] = 0;
						recordObj["tasks_in_progress_percentage"] = 0;
						recordObj["tasks_completed_percentage"] = 0;
					}
					#endregion

					#region << bugs Count "opened" & "reopened" vs "closed" >>
					var bugsOpened = (decimal)record["x_bugs_opened"];
					var bugsReOpened = (decimal)record["x_bugs_reopened"];
					var bugsClosed = (decimal)record["x_bugs_closed"];

					recordObj["bugs_opened_count"] = bugsOpened;
					recordObj["bugs_reopened_count"] = bugsReOpened;
					recordObj["bugs_closed_count"] = bugsClosed;
					if (bugsOpened + bugsReOpened + bugsClosed > 0)
					{
						recordObj["bugs_opened_percentage"] = Math.Round((decimal)(bugsOpened * 100) / (bugsOpened + bugsReOpened + bugsClosed));
						recordObj["bugs_reopened_percentage"] = Math.Round((decimal)(bugsReOpened * 100) / (bugsOpened + bugsReOpened + bugsClosed));
						recordObj["bugs_closed_percentage"] = 100 - Math.Round((decimal)(bugsOpened * 100) / (bugsOpened + bugsReOpened + bugsClosed)) - Math.Round((decimal)(bugsReOpened * 100) / (bugsOpened + bugsReOpened + bugsClosed));
					}
					else
					{
						recordObj["bugs_opened_percentage"] = 0;
						recordObj["bugs_reopened_percentage"] = 0;
						recordObj["bugs_closed_percentage"] = 0;
					}
					resultRecordsList.Add(recordObj);
					#endregion

				}
			}
			#endregion

			var skipRecords = list.PageSize * (page - 1);
			if (page != 0)
			{
				resultRecordsList = resultRecordsList.Skip(skipRecords).Take(page).ToList();
			}

			response.Success = true;
			response.Timestamp = DateTime.UtcNow;
			response.Message = "My projects successfully read";
			response.Object = resultRecordsList;

			return Json(response);
		}


		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/project/milestones-list")]
		public IActionResult ProjectMilestones(string listName = null, string entityName = null, int page = 0)
		{
			var response = new ResponseModel();

			#region << Can user read projects >>
			//Get current user
			ErpUser user = SecurityContext.CurrentUser;
			//Get entity meta
			var entity = entityManager.ReadEntity(entityName).Object;
			//Get list meta
			var list = entityManager.ReadRecordList(entity.Name, listName).Object;
			//check if user role has permissions
			var canRead = user.Roles.Any(x => entity.RecordPermissions.CanRead.Any(z => z == x.Id));
			var canCreate = user.Roles.Any(x => entity.RecordPermissions.CanCreate.Any(z => z == x.Id));
			var canUpdate = user.Roles.Any(x => entity.RecordPermissions.CanUpdate.Any(z => z == x.Id));
			var canDelete = user.Roles.Any(x => entity.RecordPermissions.CanDelete.Any(z => z == x.Id));

			if (!canRead)
			{
				response.Success = false;
				response.Message = "You do not have permission to read the projects in this system";
				response.Timestamp = DateTime.UtcNow;
				return Json(response); //return empty object
			}
			#endregion

			#region << Get the project id >>
			var queryString = HttpContext.Request.QueryString.ToString();
			var queryKeyValue = QueryHelpers.ParseQuery(queryString);
			var projectId = new Guid();
			if (queryKeyValue.ContainsKey("recordId") && Guid.TryParse(queryKeyValue["recordId"], out projectId))
			{

			}
			else
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Project Id either not found or not a GUID";
				response.Object = null;
			}
			#endregion

			#region << Get milestone data >>
			var requestedFields = "id,name,start_date,end_date,x_tasks_not_started,x_tasks_in_progress,x_tasks_completed,x_bugs_opened,x_bugs_reopened,x_bugs_closed";

			QueryObject filterObj = EntityQuery.QueryEQ("project_id", projectId);

			var sortList = new List<QuerySortObject>();
			sortList.Add(new QuerySortObject("end_date", QuerySortType.Descending));
			EntityQuery resultQuery = new EntityQuery("wv_milestone", requestedFields, filterObj, sortList.ToArray(), null, null, null);

			QueryResponse result = recMan.Find(resultQuery);
			if (!result.Success)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = result.Message;
				response.Object = null;
				return Json(response);
			}
			#endregion
			var resultRecordsList = new List<EntityRecord>();
			foreach (var record in result.Object.Data)
			{
				var recordObj = new EntityRecord();
				recordObj["id"] = record["id"];
				recordObj["name"] = record["name"];
				recordObj["start_date"] = record["start_date"];
				recordObj["end_date"] = record["end_date"];

				#region << tasks Count "not started" vs "in progress" vs "completed" >>
				var tasksNotStarted = (decimal)record["x_tasks_not_started"];
				var tasksInProgress = (decimal)record["x_tasks_in_progress"];
				var tasksCompleted = (decimal)record["x_tasks_completed"];

				recordObj["tasks_not_started_count"] = tasksNotStarted;
				recordObj["tasks_in_progress_count"] = tasksInProgress;
				recordObj["tasks_completed_count"] = tasksCompleted;
				if (tasksNotStarted + tasksInProgress + tasksCompleted > 0)
				{
					recordObj["tasks_not_started_percentage"] = Math.Round((decimal)(tasksNotStarted * 100) / (tasksNotStarted + tasksInProgress + tasksCompleted));
					recordObj["tasks_in_progress_percentage"] = Math.Round((decimal)(tasksInProgress * 100) / (tasksNotStarted + tasksInProgress + tasksCompleted));
					recordObj["tasks_completed_percentage"] = 100 - Math.Round((decimal)(tasksNotStarted * 100) / (tasksNotStarted + tasksInProgress + tasksCompleted)) - Math.Round((decimal)(tasksInProgress * 100) / (tasksNotStarted + tasksInProgress + tasksCompleted));
				}
				else
				{
					recordObj["tasks_not_started_percentage"] = 0;
					recordObj["tasks_in_progress_percentage"] = 0;
					recordObj["tasks_completed_percentage"] = 0;
				}
				#endregion

				#region << bugs Count "opened" & "reopened" vs "closed" >>
				var bugsOpened = (decimal)record["x_bugs_opened"];
				var bugsReOpened = (decimal)record["x_bugs_reopened"];
				var bugsClosed = (decimal)record["x_bugs_closed"];

				recordObj["bugs_opened_count"] = bugsOpened;
				recordObj["bugs_reopened_count"] = bugsReOpened;
				recordObj["bugs_closed_count"] = bugsClosed;
				if (bugsOpened + bugsReOpened + bugsClosed > 0)
				{
					recordObj["bugs_opened_percentage"] = Math.Round((decimal)(bugsOpened * 100) / (bugsOpened + bugsReOpened + bugsClosed));
					recordObj["bugs_reopened_percentage"] = Math.Round((decimal)(bugsReOpened * 100) / (bugsOpened + bugsReOpened + bugsClosed));
					recordObj["bugs_closed_percentage"] = 100 - Math.Round((decimal)(bugsOpened * 100) / (bugsOpened + bugsReOpened + bugsClosed)) - Math.Round((decimal)(bugsReOpened * 100) / (bugsOpened + bugsReOpened + bugsClosed));
				}
				else
				{
					recordObj["bugs_opened_percentage"] = 0;
					recordObj["bugs_reopened_percentage"] = 0;
					recordObj["bugs_closed_percentage"] = 0;
				}

				#endregion
				resultRecordsList.Add(recordObj);
			}

			response.Success = true;
			response.Timestamp = DateTime.UtcNow;
			response.Message = "My projects successfully read";
			response.Object = resultRecordsList;

			return Json(response);
		}

		[AcceptVerbs(new[] { "POST" }, Route = "/plugins/webvella-projects/api/task")]
		public IActionResult CreateTask([FromBody]JObject submitObj)
		{
			var response = new ResponseModel();
			var task = new EntityRecord();
			task["id"] = Guid.NewGuid();
			#region << ConvertObject >>

			try
			{
				foreach (var prop in submitObj.Properties())
				{
					switch (prop.Name.ToLower())
					{
						case "subject":
							task["subject"] = (string)prop.Value;
							break;
						case "description":
							task["description"] = (string)prop.Value;
							break;
						case "$project_1_n_task.id":
							task["project_id"] = (Guid)prop.Value;
							break;
						case "start_date":
							task["start_date"] = ((DateTime)prop.Value).ToUniversalTime();
							break;
						case "end_date":
							task["end_date"] = ((DateTime)prop.Value).ToUniversalTime();
							break;
						case "priority":
							task["priority"] = (string)prop.Value;
							break;
						case "status":
							task["status"] = (string)prop.Value;
							break;
						case "owner_id":
							//will be init in the below
							break;
					}
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Validation error : " + ex.Message;
				response.Object = null;
			}
			#endregion

			#region << Get project owner and set as ticket owner >>
			EntityRecord projectObject = null;
			{
				var filterObj = EntityQuery.QueryEQ("id", (Guid)task["project_id"]);
				var query = new EntityQuery("wv_project", "*", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (!result.Success)
				{
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					response.Message = "Error getting the project: " + result.Message;
					response.Object = null;
				}
				else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
				{
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					response.Message = "Project not found";
					response.Object = null;
				}
				projectObject = result.Object.Data[0];
				task["owner_id"] = (Guid)result.Object.Data[0]["owner_id"];
			}
			#endregion

			//Create transaction
			using (var connection = DbContext.Current.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();
					//Update project tasks counters
					{
						var patchObject = new EntityRecord();
						patchObject["id"] = (Guid)projectObject["id"];
						switch ((string)task["status"])
						{
							case "not started":
								patchObject["x_tasks_not_started"] = (decimal)projectObject["x_tasks_not_started"] + 1;
								break;
							case "in progress":
								patchObject["x_tasks_in_progress"] = (decimal)projectObject["x_tasks_in_progress"] + 1;
								break;
							case "completed":
								patchObject["x_tasks_completed"] = (decimal)projectObject["x_tasks_completed"] + 1;
								break;
						}
						var updateResponse = recMan.UpdateRecord("wv_project",patchObject);
						if(!updateResponse.Success) {
							throw new Exception(updateResponse.Message);
						}
					}
					//Create task
					{
						var createResponse = recMan.CreateRecord("wv_task",task);
						if(!createResponse.Success) {
							throw new Exception(createResponse.Message);
						}
					}
					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					response.Message = "Error saving the task: " + ex.Message;
					response.Object = null;
				}

			}
			response.Success = true;
			response.Timestamp = DateTime.UtcNow;
			response.Message = "My projects successfully read";
			response.Object = null;

			return Json(response);
		}

	}
}
