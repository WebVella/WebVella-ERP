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
		EntityManager entMan;
		EntityRelationManager relMan;
		SecurityManager secMan;

		public ApiController()
		{
			recMan = new RecordManager();
			secMan = new SecurityManager();
			entMan = new EntityManager();
			relMan = new EntityRelationManager();
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/project/list/my-projects")]
		public IActionResult MyProjects(int page = 0)
		{
			var response = new ResponseModel();
			try {
			//var queryString = HttpContext.Request.QueryString;
			#region << Can user read projects >>
			//Get current user
			ErpUser user = SecurityContext.CurrentUser;
			//Get entity meta
			var entity = entMan.ReadEntity("wv_project").Object;
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
			//This list support filters by name
			var queryString = HttpContext.Request.QueryString.ToString();
			var queryKeyValue = QueryHelpers.ParseQuery(queryString);

			//Get the project name from query if exists
			QueryObject rootFilterSection = null;
			var auxFiltersRuleList = new List<QueryObject>();
			if (queryKeyValue.ContainsKey("name"))
			{
				var projectIdRule = EntityQuery.QueryContains("name", (string)queryKeyValue["name"]);
				auxFiltersRuleList.Add(projectIdRule);
			}
			if(auxFiltersRuleList.Count > 0) {
				rootFilterSection = EntityQuery.QueryAND(auxFiltersRuleList.ToArray());
			}

			EntityQuery resultQuery = new EntityQuery("wv_project", requestedFields, rootFilterSection, null, null, null, null);
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
					#endregion
					resultRecordsList.Add(recordObj);
				}
			}
			#endregion

			//var skipRecords = list.PageSize * (page - 1);
			//if (page != 0)
			//{
			//	resultRecordsList = resultRecordsList.Skip(skipRecords).Take(page).ToList();
			//}

			response.Success = true;
			response.Timestamp = DateTime.UtcNow;
			response.Message = "My projects successfully read";
			response.Object = resultRecordsList;

			return Json(response);
			}
			catch(Exception ex) {
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Error: " + ex.Message;
				response.Object = null;
				return Json(response);	
			}
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/milestone/list/my-milestones")]
		public IActionResult MyMilestones()
		{
			var response = new ResponseModel();
			var resultProjectIdList = new List<Guid>();
			try {
			#region << Get Project Ids >>

			#region << Can user read projects >>
			//Get current user
			ErpUser user = SecurityContext.CurrentUser;
			//Get entity meta
			var entity = entMan.ReadEntity("wv_project").Object;
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
			var milestone = entMan.ReadEntity("wv_milestone").Object;
			//check if user role has permissions
			var canReadMilestone = user.Roles.Any(x => milestone.RecordPermissions.CanRead.Any(z => z == x.Id));
			var canCreateMilestone = user.Roles.Any(x => milestone.RecordPermissions.CanCreate.Any(z => z == x.Id));
			var canUpdateMilestone = user.Roles.Any(x => milestone.RecordPermissions.CanUpdate.Any(z => z == x.Id));
			var canDeleteMilestone = user.Roles.Any(x => milestone.RecordPermissions.CanDelete.Any(z => z == x.Id));

			if (!canReadMilestone)
			{
				response.Success = false;
				response.Message = "You do not have permission to read the milestones in this system";
				response.Timestamp = DateTime.UtcNow;
				return Json(response); //return empty object
			}

			#endregion

			var requestedFields = "id," +
			"$user_1_n_project_owner.id," +
			"$role_n_n_project_team.id,$role_n_n_project_customer.id";
			#region << Query builder >>
			//QueryObject filterObj = EntityQuery.QueryEQ("id", recordId);
			QueryObject filterObj = null;
			EntityQuery resultQuery = new EntityQuery("wv_project", requestedFields, filterObj, null, null, null, null);
			#endregion

			#region << Execute >>
			QueryResponse result = recMan.Find(resultQuery);
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
					resultProjectIdList.Add((Guid)record["id"]);
				}
			}
			#endregion
			#endregion

			if(resultProjectIdList.Count == 0) {
				response.Success = true;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "You do not have access to any project or there are no projects yet";
				response.Object = null;
				return Json(response);	
			}

			#region << Get Milestones >>
			var milestoneFields = "*";

			QueryObject projectIdFilterSection = null;
			#region << project id filters >>
			var projectIdRulesList = new List<QueryObject>();
			foreach (var projectId in resultProjectIdList)
			{
				var projectIdRule = EntityQuery.QueryEQ("project_id", projectId);
				projectIdRulesList.Add(projectIdRule);
			}
			projectIdFilterSection = EntityQuery.QueryOR(projectIdRulesList.ToArray());
			#endregion

			var sortRulesList = new List<QuerySortObject>();
			var defaultSortRule = new QuerySortObject("name",QuerySortType.Ascending);
			sortRulesList.Add(defaultSortRule);

			var milestoneQuery = new EntityQuery("wv_milestone", milestoneFields, projectIdFilterSection, sortRulesList.ToArray(), null, null, null);
			var milestoneQueryResponse = recMan.Find(milestoneQuery);
			if (!milestoneQueryResponse.Success)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = milestoneQueryResponse.Message;
				response.Object = null;
				return Json(response);
			}

			response.Success = true;
			response.Timestamp = DateTime.UtcNow;
			response.Message = "My milestones successfully read";
			response.Object = milestoneQueryResponse.Object.Data;

			return Json(response);

			#endregion

			}
			catch(Exception ex) {
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Error: " + ex.Message;
				response.Object = null;
				return Json(response);	
			}
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/project/milestones-list")]
		public IActionResult ProjectMilestones(int page = 0)
		{
			var response = new ResponseModel();

			#region << Can user read projects >>
			//Get current user
			ErpUser user = SecurityContext.CurrentUser;
			//Get entity meta
			var entity = entMan.ReadEntity("wv_milestone").Object;
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

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/task/list/all")]
		public IActionResult AllTaskUserCanSee(string listName, int page = 0)
		{
			var response = new ResponseModel();
			try
			{
				//var queryString = HttpContext.Request.QueryString;
				#region << Can user read tasks >>
				//Get current user
				ErpUser user = SecurityContext.CurrentUser;
				//Get entity meta
				var entity = entMan.ReadEntity("wv_task").Object;
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
				var taskQueryResponse = new QueryResponse();
				var userCanSeeProjectIds = new List<Guid>();
				#region << Generate list of projects user can see >>
				{
					var requestedFields = "id,$user_1_n_project_owner.id,$role_n_n_project_team.id,$role_n_n_project_customer.id";
					//QueryObject filterObj = EntityQuery.QueryEQ("id", recordId);
					QueryObject filterObj = null;
					EntityQuery resultQuery = new EntityQuery("wv_project", requestedFields, filterObj, null, null, null, null);
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
							userCanSeeProjectIds.Add((Guid)record["id"]);
						}
					}
				}
				#endregion

				#region << Get tasks >>
				{
					var fields = "id,number,subject,start_date,end_date,status,priority,$user_1_n_task_owner.id,$user_1_n_task_owner.image";

					QueryObject rootFilterSection = null;
					QueryObject auxFilterSection = null;
					QueryObject projectIdFilterSection = null;
					
					#region << project id filters >>
					var projectIdRulesList = new List<QueryObject>();
					foreach (var projectId in userCanSeeProjectIds)
					{
						var projectIdRule = EntityQuery.QueryEQ("project_id", projectId);
						projectIdRulesList.Add(projectIdRule);
					}
					projectIdFilterSection = EntityQuery.QueryOR(projectIdRulesList.ToArray());
					#endregion

					#region << Aux filters & Sort>>
					var sortRulesList = new List<QuerySortObject>();
					var queryString = HttpContext.Request.QueryString.ToString();
					var queryKeyValueList = QueryHelpers.ParseQuery(queryString);
					var auxRulesList = new List<QueryObject>();
					var getListObject = entMan.ReadRecordList(entity.Name, listName).Object;
					//Currently we will hardcode the query generation
					//auxFilterSection = RecordListQuery.ConvertQuery(getListObject.Query);		
					QueryObject auxRule = new QueryObject();
					foreach(var query in queryKeyValueList) {
						switch(query.Key.ToLowerInvariant()) {
							case "number":
								auxRule = new QueryObject();
								decimal outDecimal = 0;
								if(decimal.TryParse((string)query.Value,out outDecimal)){
									auxRule = EntityQuery.QueryEQ("number", outDecimal);
									auxRulesList.Add(auxRule);
								}
								break;
							case "subject":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryContains("subject", (string)query.Value);
								auxRulesList.Add(auxRule);
								break;
							case "status":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryEQ("status", (string)query.Value);
								auxRulesList.Add(auxRule);
								break;
							case "priority":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryEQ("priority", (string)query.Value);
								auxRulesList.Add(auxRule);
								break;
							case "sortby":
								var sortRule = new QuerySortObject((string)query.Value,QuerySortType.Descending);
								if(!queryKeyValueList.ContainsKey("sortOrder") || (string)queryKeyValueList["sortOrder"] == "ascending"){
									sortRule = new QuerySortObject((string)query.Value,QuerySortType.Ascending);
								}
								sortRulesList.Add(sortRule);
								break;
						}
					
					
					}			
					auxFilterSection = EntityQuery.QueryAND(auxRulesList.ToArray());
					//Add default sort by created_on
					var defaultSortRule = new QuerySortObject("created_on",QuerySortType.Ascending);
					sortRulesList.Add(defaultSortRule);

					#endregion

					rootFilterSection = EntityQuery.QueryAND(projectIdFilterSection,auxFilterSection);

					//Calculate page
					var pageSize = getListObject.PageSize;
					var skipRecords = (page-1)*pageSize;


					var taskQuery = new EntityQuery("wv_task", fields, rootFilterSection, sortRulesList.ToArray(), skipRecords, pageSize, null);

					taskQueryResponse = recMan.Find(taskQuery);
					if (!taskQueryResponse.Success)
					{
						response.Success = false;
						response.Timestamp = DateTime.UtcNow;
						response.Message = taskQueryResponse.Message;
						response.Object = null;
						return Json(response);
					}
				}
				#endregion
				var taskList = new List<EntityRecord>();
				#region << Post-process >>

				foreach (var task in taskQueryResponse.Object.Data)
				{
					var record = new EntityRecord();
					record["id"] = (Guid)task["id"];
					record["number"] = (decimal)task["number"];
					record["subject"] = (string)task["subject"];
					record["start_date"] = (DateTime)task["start_date"];
					record["end_date"] = (DateTime)task["end_date"];
					record["status"] = (string)task["status"];
					record["priority"] = (string)task["priority"];
					var taskOwnerIdList = new List<Guid>();
					var taskOwnerImageList = new List<string>();
					var taskOwnerId = (Guid)((List<EntityRecord>)task["$user_1_n_task_owner"])[0]["id"];
					var taskOwnerImage = (string)((List<EntityRecord>)task["$user_1_n_task_owner"])[0]["image"];
					taskOwnerIdList.Add(taskOwnerId);
					taskOwnerImageList.Add(taskOwnerImage);
					record["$field$user_1_n_task_owner$id"] = taskOwnerIdList;
					record["$field$user_1_n_task_owner$image"] = taskOwnerImageList;
					taskList.Add(record);
				}
				#endregion

				response.Success = true;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Successful read";
				response.Object = taskList;

				return Json(response);

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = ex.Message;
				response.Object = null;
				return Json(response);
			}
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/bug/list/all")]
		public IActionResult AllBugsUserCanSee(string listName, int page = 0)
		{
			var response = new ResponseModel();
			try
			{
				//var queryString = HttpContext.Request.QueryString;
				#region << Can user read tasks >>
				//Get current user
				ErpUser user = SecurityContext.CurrentUser;
				//Get entity meta
				var entity = entMan.ReadEntity("wv_bug").Object;
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
				var bugQueryResponse = new QueryResponse();
				var userCanSeeProjectIds = new List<Guid>();
				#region << Generate list of projects user can see >>
				{
					var requestedFields = "id,$user_1_n_project_owner.id,$role_n_n_project_team.id,$role_n_n_project_customer.id";
					//QueryObject filterObj = EntityQuery.QueryEQ("id", recordId);
					QueryObject filterObj = null;
					EntityQuery resultQuery = new EntityQuery("wv_project", requestedFields, filterObj, null, null, null, null);
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
							userCanSeeProjectIds.Add((Guid)record["id"]);
						}
					}
				}
				#endregion

				#region << Get bugs >>
				{
					var fields = "id,number,subject,status,priority,$user_1_n_bug_owner.id,$user_1_n_bug_owner.image";

					QueryObject rootFilterSection = null;
					QueryObject auxFilterSection = null;
					QueryObject projectIdFilterSection = null;
					
					#region << project id filters >>
					var projectIdRulesList = new List<QueryObject>();
					foreach (var projectId in userCanSeeProjectIds)
					{
						var projectIdRule = EntityQuery.QueryEQ("project_id", projectId);
						projectIdRulesList.Add(projectIdRule);
					}
					projectIdFilterSection = EntityQuery.QueryOR(projectIdRulesList.ToArray());
					#endregion

					#region << Aux filters & Sort>>
					var sortRulesList = new List<QuerySortObject>();
					var queryString = HttpContext.Request.QueryString.ToString();
					var queryKeyValueList = QueryHelpers.ParseQuery(queryString);
					var auxRulesList = new List<QueryObject>();
					var getListObject = entMan.ReadRecordList(entity.Name, listName).Object;
					//Currently we will hardcode the query generation
					//auxFilterSection = RecordListQuery.ConvertQuery(getListObject.Query);		
					QueryObject auxRule = new QueryObject();
					foreach(var query in queryKeyValueList) {
						switch(query.Key.ToLowerInvariant()) {
							case "number":
								auxRule = new QueryObject();
								decimal outDecimal = 0;
								if(decimal.TryParse((string)query.Value,out outDecimal)){
									auxRule = EntityQuery.QueryEQ("number", outDecimal);
									auxRulesList.Add(auxRule);
								}
								break;
							case "subject":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryContains("subject", (string)query.Value);
								auxRulesList.Add(auxRule);
								break;
							case "status":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryEQ("status", (string)query.Value);
								auxRulesList.Add(auxRule);
								break;
							case "priority":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryEQ("priority", (string)query.Value);
								auxRulesList.Add(auxRule);
								break;
							case "sortby":
								var sortRule = new QuerySortObject((string)query.Value,QuerySortType.Descending);
								if(!queryKeyValueList.ContainsKey("sortOrder") || (string)queryKeyValueList["sortOrder"] == "ascending"){
									sortRule = new QuerySortObject((string)query.Value,QuerySortType.Ascending);
								}
								sortRulesList.Add(sortRule);
								break;
						}
					
					
					}			
					auxFilterSection = EntityQuery.QueryAND(auxRulesList.ToArray());
					//Add default sort by created_on
					var defaultSortRule = new QuerySortObject("created_on",QuerySortType.Ascending);
					sortRulesList.Add(defaultSortRule);

					#endregion

					rootFilterSection = EntityQuery.QueryAND(projectIdFilterSection,auxFilterSection);

					//Calculate page
					var pageSize = getListObject.PageSize;
					var skipRecords = (page-1)*pageSize;


					var bugQuery = new EntityQuery("wv_bug", fields, rootFilterSection, sortRulesList.ToArray(), skipRecords, pageSize, null);

					bugQueryResponse = recMan.Find(bugQuery);
					if (!bugQueryResponse.Success)
					{
						response.Success = false;
						response.Timestamp = DateTime.UtcNow;
						response.Message = bugQueryResponse.Message;
						response.Object = null;
						return Json(response);
					}
				}
				#endregion
				var bugList = new List<EntityRecord>();
				#region << Post-process >>

				foreach (var bug in bugQueryResponse.Object.Data)
				{
					var record = new EntityRecord();
					record["id"] = (Guid)bug["id"];
					record["number"] = (decimal)bug["number"];
					record["subject"] = (string)bug["subject"];
					record["status"] = (string)bug["status"];
					record["priority"] = (string)bug["priority"];
					var bugOwnerIdList = new List<Guid>();
					var bugOwnerImageList = new List<string>();
					var bugOwnerId = (Guid)((List<EntityRecord>)bug["$user_1_n_bug_owner"])[0]["id"];
					var bugOwnerImage = (string)((List<EntityRecord>)bug["$user_1_n_bug_owner"])[0]["image"];
					bugOwnerIdList.Add(bugOwnerId);
					bugOwnerImageList.Add(bugOwnerImage);
					record["$field$user_1_n_bug_owner$id"] = bugOwnerIdList;
					record["$field$user_1_n_bug_owner$image"] = bugOwnerImageList;
					bugList.Add(record);
				}
				#endregion

				response.Success = true;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Successful read";
				response.Object = bugList;

				return Json(response);

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = ex.Message;
				response.Object = null;
				return Json(response);
			}
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/activity/list/all")]
		public IActionResult AllActivitiesUserCanSee(string label = "all", int page = 1)
		{
			var response = new ResponseModel();
			try
			{
				//var queryString = HttpContext.Request.QueryString;
				#region << Can user read activities >>
				//Get current user
				ErpUser user = SecurityContext.CurrentUser;
				//Get entity meta
				var entity = entMan.ReadEntity("wv_project_activity").Object;
				//check if user role has permissions
				var canRead = user.Roles.Any(x => entity.RecordPermissions.CanRead.Any(z => z == x.Id));
				var canCreate = user.Roles.Any(x => entity.RecordPermissions.CanCreate.Any(z => z == x.Id));
				var canUpdate = user.Roles.Any(x => entity.RecordPermissions.CanUpdate.Any(z => z == x.Id));
				var canDelete = user.Roles.Any(x => entity.RecordPermissions.CanDelete.Any(z => z == x.Id));

				if (!canRead)
				{
					response.Success = false;
					response.Message = "You do not have permission to read the activities in this system";
					response.Timestamp = DateTime.UtcNow;
					return Json(response); //return empty object
				}
				#endregion

				var activityQueryResponse = new QueryResponse();
				var userCanSeeProjectIds = new List<Guid>();
				#region << Generate list of projects user can see >>
				{
					var requestedFields = "id,$user_1_n_project_owner.id,$role_n_n_project_team.id,$role_n_n_project_customer.id";
					//QueryObject filterObj = EntityQuery.QueryEQ("id", recordId);
					QueryObject filterObj = null;
					EntityQuery resultQuery = new EntityQuery("wv_project", requestedFields, filterObj, null, null, null, null);
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
							userCanSeeProjectIds.Add((Guid)record["id"]);
						}
					}
				}
				#endregion

				#region << Get activities >>
				{
					var fields = "id,label,created_on,description,subject,"+
					"$user_wv_project_activity_created_by.username,$user_wv_project_activity_created_by.image," +
					"$project_1_n_activity.name";

					QueryObject rootFilterSection = null;
					QueryObject auxFilterSection = null;
					QueryObject projectIdFilterSection = null;
					
					#region << project id filters >>
					var projectIdRulesList = new List<QueryObject>();
					foreach (var projectId in userCanSeeProjectIds)
					{
						var projectIdRule = EntityQuery.QueryEQ("project_id", projectId);
						projectIdRulesList.Add(projectIdRule);
					}
					projectIdFilterSection = EntityQuery.QueryOR(projectIdRulesList.ToArray());
					#endregion

					#region << Aux filters & Sort>>
					var auxRulesList = new List<QueryObject>();
					QueryObject auxRule = new QueryObject();
					if(label != "all") {
						auxRule = EntityQuery.QueryEQ("label", label);
						auxRulesList.Add(auxRule);
					}

					auxFilterSection = EntityQuery.QueryAND(auxRulesList.ToArray());
					//Add default sort by created_on
					var sortRulesList = new List<QuerySortObject>();
					var defaultSortRule = new QuerySortObject("created_on",QuerySortType.Descending);
					sortRulesList.Add(defaultSortRule);

					#endregion

					rootFilterSection = EntityQuery.QueryAND(projectIdFilterSection,auxFilterSection);

					//Calculate page
					var pageSize = 15;
					var skipRecords = (page-1)*pageSize;


					var activityQuery = new EntityQuery("wv_project_activity", fields, rootFilterSection, sortRulesList.ToArray(), skipRecords, pageSize, null);

					activityQueryResponse = recMan.Find(activityQuery);
					if (!activityQueryResponse.Success)
					{
						response.Success = false;
						response.Timestamp = DateTime.UtcNow;
						response.Message = activityQueryResponse.Message;
						response.Object = null;
						return Json(response);
					}
				}
				#endregion

				response.Success = true;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Successful read";
				response.Object = activityQueryResponse.Object.Data;

				return Json(response);

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = ex.Message;
				response.Object = null;
				return Json(response);
			}
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/task/list/last-updated-for-owner")]
		public IActionResult LastUpdatedTasksUserOwns(int page = 1)
		{
			var response = new ResponseModel();
			try
			{
				//var queryString = HttpContext.Request.QueryString;
				#region << Can user read activities >>
				//Get current user
				ErpUser user = SecurityContext.CurrentUser;
				//Get entity meta
				var entity = entMan.ReadEntity("wv_task").Object;
				//check if user role has permissions
				var canRead = user.Roles.Any(x => entity.RecordPermissions.CanRead.Any(z => z == x.Id));
				var canCreate = user.Roles.Any(x => entity.RecordPermissions.CanCreate.Any(z => z == x.Id));
				var canUpdate = user.Roles.Any(x => entity.RecordPermissions.CanUpdate.Any(z => z == x.Id));
				var canDelete = user.Roles.Any(x => entity.RecordPermissions.CanDelete.Any(z => z == x.Id));

				if (!canRead)
				{
					response.Success = false;
					response.Message = "You do not have permission to read the tasks in this system";
					response.Timestamp = DateTime.UtcNow;
					return Json(response); //return empty object
				}
				#endregion

				var taskQueryResponse = new QueryResponse();

				#region << Get activities >>
				{
					var fields = "id,number,subject,last_modified_on,$user_wv_task_modified_by.username";

					//Add default sort by created_on
					var sortRulesList = new List<QuerySortObject>();
					var defaultSortRule = new QuerySortObject("last_modified_on",QuerySortType.Descending);
					sortRulesList.Add(defaultSortRule);

					#endregion
					var ownerFilter = EntityQuery.QueryEQ("owner_id",SecurityContext.CurrentUser.Id);
					var notClosedFilter = EntityQuery.QueryNOT("status","completed");

					var rootFilterSection = EntityQuery.QueryAND(ownerFilter,notClosedFilter);

					//Calculate page
					var pageSize = 5;
					var skipRecords = (page-1)*pageSize;

					var activityQuery = new EntityQuery("wv_task", fields, rootFilterSection, sortRulesList.ToArray(), skipRecords, pageSize, null);

					taskQueryResponse = recMan.Find(activityQuery);
					if (!taskQueryResponse.Success)
					{
						response.Success = false;
						response.Timestamp = DateTime.UtcNow;
						response.Message = taskQueryResponse.Message;
						response.Object = null;
						return Json(response);
					}
				}

				response.Success = true;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Successful read";
				response.Object = taskQueryResponse.Object.Data;

				return Json(response);

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = ex.Message;
				response.Object = null;
				return Json(response);
			}
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/bug/list/last-updated-for-owner")]
		public IActionResult LastUpdatedBugsUserOwns(int page = 1)
		{
			var response = new ResponseModel();
			try
			{
				//var queryString = HttpContext.Request.QueryString;
				#region << Can user read activities >>
				//Get current user
				ErpUser user = SecurityContext.CurrentUser;
				//Get entity meta
				var entity = entMan.ReadEntity("wv_bug").Object;
				//check if user role has permissions
				var canRead = user.Roles.Any(x => entity.RecordPermissions.CanRead.Any(z => z == x.Id));
				var canCreate = user.Roles.Any(x => entity.RecordPermissions.CanCreate.Any(z => z == x.Id));
				var canUpdate = user.Roles.Any(x => entity.RecordPermissions.CanUpdate.Any(z => z == x.Id));
				var canDelete = user.Roles.Any(x => entity.RecordPermissions.CanDelete.Any(z => z == x.Id));

				if (!canRead)
				{
					response.Success = false;
					response.Message = "You do not have permission to read the bugs in this system";
					response.Timestamp = DateTime.UtcNow;
					return Json(response); //return empty object
				}
				#endregion

				var bugQueryResponse = new QueryResponse();

				#region << Get activities >>
				{
					var fields = "id,number,subject,last_modified_on,$user_wv_bug_modified_by.username";

					//Add default sort by created_on
					var sortRulesList = new List<QuerySortObject>();
					var defaultSortRule = new QuerySortObject("last_modified_on",QuerySortType.Descending);
					sortRulesList.Add(defaultSortRule);

					#endregion
					var ownerFilter = EntityQuery.QueryEQ("owner_id",SecurityContext.CurrentUser.Id);
					var notClosedFilter = EntityQuery.QueryNOT("status","closed");

					var rootFilterSection = EntityQuery.QueryAND(ownerFilter,notClosedFilter);

					//Calculate page
					var pageSize = 5;
					var skipRecords = (page-1)*pageSize;

					var activityQuery = new EntityQuery("wv_bug", fields, rootFilterSection, sortRulesList.ToArray(), skipRecords, pageSize, null);

					bugQueryResponse = recMan.Find(activityQuery);
					if (!bugQueryResponse.Success)
					{
						response.Success = false;
						response.Timestamp = DateTime.UtcNow;
						response.Message = bugQueryResponse.Message;
						response.Object = null;
						return Json(response);
					}
				}

				response.Success = true;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Successful read";
				response.Object = bugQueryResponse.Object.Data;

				return Json(response);

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = ex.Message;
				response.Object = null;
				return Json(response);
			}
		}

	}
}
