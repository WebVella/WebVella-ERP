using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
			try
			{
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
				var requestedFields = "id,name,code,start_date,end_date," +
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
				if (auxFiltersRuleList.Count > 0)
				{
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
						if (!userIsPM)
						{
							userIsPM = ((List<EntityRecord>)record["$user_1_n_project_owner"]).Any(z => (Guid)z["id"] == user.Id);
						}
						if (!userIsStaff)
						{
							userIsStaff = ((List<EntityRecord>)record["$role_n_n_project_team"]).Any(z => (Guid)z["id"] == userRole.Id);
						}
						if (!userIsCustomer)
						{
							userIsCustomer = ((List<EntityRecord>)record["$role_n_n_project_customer"]).Any(z => (Guid)z["id"] == userRole.Id);
						}
					}
					#endregion

					if (userIsPM || userIsStaff || userIsCustomer)
					{
						var recordObj = new EntityRecord();
						recordObj["id"] = record["id"];
						recordObj["name"] = record["name"];
						recordObj["code"] = record["code"];
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
			catch (Exception ex)
			{
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
			try
			{
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
						if (!userIsPM)
						{
							userIsPM = ((List<EntityRecord>)record["$user_1_n_project_owner"]).Any(z => (Guid)z["id"] == user.Id);
						}
						if (!userIsStaff)
						{
							userIsStaff = ((List<EntityRecord>)record["$role_n_n_project_team"]).Any(z => (Guid)z["id"] == userRole.Id);
						}
						if (!userIsCustomer)
						{
							userIsCustomer = ((List<EntityRecord>)record["$role_n_n_project_customer"]).Any(z => (Guid)z["id"] == userRole.Id);
						}
					}
					#endregion

					if (userIsPM || userIsStaff || userIsCustomer)
					{
						resultProjectIdList.Add((Guid)record["id"]);
					}
				}
				#endregion
				#endregion

				if (resultProjectIdList.Count == 0)
				{
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
				var defaultSortRule = new QuerySortObject("name", QuerySortType.Ascending);
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
			catch (Exception ex)
			{
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
							if (!userIsPM)
							{
								userIsPM = ((List<EntityRecord>)record["$user_1_n_project_owner"]).Any(z => (Guid)z["id"] == user.Id);
							}
							if (!userIsStaff)
							{
								userIsStaff = ((List<EntityRecord>)record["$role_n_n_project_team"]).Any(z => (Guid)z["id"] == userRole.Id);
							}
							if (!userIsCustomer)
							{
								userIsCustomer = ((List<EntityRecord>)record["$role_n_n_project_customer"]).Any(z => (Guid)z["id"] == userRole.Id);
							}
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
					var fields = "id,code,number,subject,start_date,end_date,status,priority,$user_1_n_task_owner.id,$user_1_n_task_owner.username";

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
					foreach (var query in queryKeyValueList)
					{
						switch (query.Key.ToLowerInvariant())
						{
							case "code":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryContains("code", (string)query.Value);
								auxRulesList.Add(auxRule);
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
							case "$user_1_n_task_owner.username":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryContains("$user_1_n_task_owner.username", (string)query.Value);
								auxRulesList.Add(auxRule);
								break;
							case "priority":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryEQ("priority", (string)query.Value);
								auxRulesList.Add(auxRule);
								break;
							case "fts":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryFTS("fts", (string)query.Value);
								auxRulesList.Add(auxRule);
								break;
							case "sortby":
								var sortRule = new QuerySortObject((string)query.Value, QuerySortType.Descending);
								if (!queryKeyValueList.ContainsKey("sortOrder") || (string)queryKeyValueList["sortOrder"] == "ascending")
								{
									sortRule = new QuerySortObject((string)query.Value, QuerySortType.Ascending);
								}
								sortRulesList.Add(sortRule);
								break;
						}


					}
					auxFilterSection = EntityQuery.QueryAND(auxRulesList.ToArray());
					//Add default sort by created_on
					var defaultSortRule = new QuerySortObject("created_on", QuerySortType.Ascending);
					sortRulesList.Add(defaultSortRule);

					#endregion

					rootFilterSection = EntityQuery.QueryAND(projectIdFilterSection, auxFilterSection);

					//Calculate page
					var pageSize = getListObject.PageSize;
					var skipRecords = (page - 1) * pageSize;


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
					record["code"] = (string)task["code"];
					record["subject"] = (string)task["subject"];
					record["start_date"] = (DateTime)task["start_date"];
					record["end_date"] = (DateTime)task["end_date"];
					record["status"] = (string)task["status"];
					record["priority"] = (string)task["priority"];
					var taskOwnerIdList = new List<Guid>();
					var taskUsernameImageList = new List<string>();
					var taskOwnerId = (Guid)((List<EntityRecord>)task["$user_1_n_task_owner"])[0]["id"];
					var taskOwnerUsername = (string)((List<EntityRecord>)task["$user_1_n_task_owner"])[0]["username"];
					taskOwnerIdList.Add(taskOwnerId);
					taskUsernameImageList.Add(taskOwnerUsername);
					record["$field$user_1_n_task_owner$id"] = taskOwnerIdList;
					record["$field$user_1_n_task_owner$username"] = taskUsernameImageList;
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
							if (!userIsPM)
							{
								userIsPM = ((List<EntityRecord>)record["$user_1_n_project_owner"]).Any(z => (Guid)z["id"] == user.Id);
							}
							if (!userIsStaff)
							{
								userIsStaff = ((List<EntityRecord>)record["$role_n_n_project_team"]).Any(z => (Guid)z["id"] == userRole.Id);
							}
							if (!userIsCustomer)
							{
								userIsCustomer = ((List<EntityRecord>)record["$role_n_n_project_customer"]).Any(z => (Guid)z["id"] == userRole.Id);
							}
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
					var fields = "id,code,number,subject,status,priority,$user_1_n_bug_owner.id,$user_1_n_bug_owner.username";

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
					foreach (var query in queryKeyValueList)
					{
						switch (query.Key.ToLowerInvariant())
						{
							case "code":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryContains("code", (string)query.Value);
								auxRulesList.Add(auxRule);
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
							case "$user_1_n_bug_owner.username":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryContains("$user_1_n_bug_owner.username", (string)query.Value);
								auxRulesList.Add(auxRule);
								break;
							case "priority":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryEQ("priority", (string)query.Value);
								auxRulesList.Add(auxRule);
								break;
							case "fts":
								auxRule = new QueryObject();
								auxRule = EntityQuery.QueryFTS("fts", (string)query.Value);
								auxRulesList.Add(auxRule);
								break;
							case "sortby":
								var sortRule = new QuerySortObject((string)query.Value, QuerySortType.Descending);
								if (!queryKeyValueList.ContainsKey("sortOrder") || (string)queryKeyValueList["sortOrder"] == "ascending")
								{
									sortRule = new QuerySortObject((string)query.Value, QuerySortType.Ascending);
								}
								sortRulesList.Add(sortRule);
								break;
						}


					}
					auxFilterSection = EntityQuery.QueryAND(auxRulesList.ToArray());
					//Add default sort by created_on
					var defaultSortRule = new QuerySortObject("created_on", QuerySortType.Ascending);
					sortRulesList.Add(defaultSortRule);

					#endregion

					rootFilterSection = EntityQuery.QueryAND(projectIdFilterSection, auxFilterSection);

					//Calculate page
					var pageSize = getListObject.PageSize;
					var skipRecords = (page - 1) * pageSize;


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
					record["code"] = (string)bug["code"];
					record["subject"] = (string)bug["subject"];
					record["status"] = (string)bug["status"];
					record["priority"] = (string)bug["priority"];
					var bugOwnerIdList = new List<Guid>();
					var bugUsernameImageList = new List<string>();
					var bugOwnerId = (Guid)((List<EntityRecord>)bug["$user_1_n_bug_owner"])[0]["id"];
					var bugOwnerUsername = (string)((List<EntityRecord>)bug["$user_1_n_bug_owner"])[0]["username"];
					bugOwnerIdList.Add(bugOwnerId);
					bugUsernameImageList.Add(bugOwnerUsername);
					record["$field$user_1_n_bug_owner$id"] = bugOwnerIdList;
					record["$field$user_1_n_bug_owner$username"] = bugUsernameImageList;
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
							if (!userIsPM)
							{
								userIsPM = ((List<EntityRecord>)record["$user_1_n_project_owner"]).Any(z => (Guid)z["id"] == user.Id);
							}
							if (!userIsStaff)
							{
								userIsStaff = ((List<EntityRecord>)record["$role_n_n_project_team"]).Any(z => (Guid)z["id"] == userRole.Id);
							}
							if (!userIsCustomer)
							{
								userIsCustomer = ((List<EntityRecord>)record["$role_n_n_project_customer"]).Any(z => (Guid)z["id"] == userRole.Id);
							}
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
					var fields = "id,label,created_on,description,subject," +
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
					if (label != "all")
					{
						auxRule = EntityQuery.QueryEQ("label", label);
						auxRulesList.Add(auxRule);
					}

					auxFilterSection = EntityQuery.QueryAND(auxRulesList.ToArray());
					//Add default sort by created_on
					var sortRulesList = new List<QuerySortObject>();
					var defaultSortRule = new QuerySortObject("created_on", QuerySortType.Descending);
					sortRulesList.Add(defaultSortRule);

					#endregion

					rootFilterSection = EntityQuery.QueryAND(projectIdFilterSection, auxFilterSection);

					//Calculate page
					var pageSize = 10;
					var skipRecords = (page - 1) * pageSize;


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

				#region << Get tasks >>
				{
					var fields = "id,number,subject,code,priority,last_modified_on,$user_wv_task_modified_by.username";

					//Add default sort by created_on
					var sortRulesList = new List<QuerySortObject>();
					var defaultSortRule = new QuerySortObject("last_modified_on", QuerySortType.Descending);
					sortRulesList.Add(defaultSortRule);

					#endregion
					var ownerFilter = EntityQuery.QueryEQ("owner_id", SecurityContext.CurrentUser.Id);
					var notClosedFilter = EntityQuery.QueryNOT("status", "completed");

					var rootFilterSection = EntityQuery.QueryAND(ownerFilter, notClosedFilter);

					//Calculate page
					var pageSize = 5;
					var skipRecords = (page - 1) * pageSize;

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

				#region << Get bugs >>
				{
					var fields = "id,number,code,subject,priority,last_modified_on,$user_wv_bug_modified_by.username";

					//Add default sort by created_on
					var sortRulesList = new List<QuerySortObject>();
					var defaultSortRule = new QuerySortObject("last_modified_on", QuerySortType.Descending);
					sortRulesList.Add(defaultSortRule);

					#endregion
					var ownerFilter = EntityQuery.QueryEQ("owner_id", SecurityContext.CurrentUser.Id);
					var notClosedFilter = EntityQuery.QueryNOT("status", "closed");

					var rootFilterSection = EntityQuery.QueryAND(ownerFilter, notClosedFilter);

					//Calculate page
					var pageSize = 5;
					var skipRecords = (page - 1) * pageSize;

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

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/report/project-timelog")]
		public IActionResult ProjectTimelogReport(int year = 0, int month = 0)
		{
			var response = new ResponseModel();
			if (year == 0 || month == 0)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Month or year parameter is missing";
				response.Object = null;
				return Json(response);
			}
			try
			{
				var timelogs = new List<EntityRecord>();
				var projects = new List<EntityRecord>();
				var projectsWithTimelogs = new List<EntityRecord>();
				var projectTaskBugsDict = new Dictionary<Guid, List<EntityRecord>>();
				var workedTaskIds = new List<Guid>();
				var workedBugIds = new List<Guid>();
				var workedTasks = new List<EntityRecord>();
				var workedBugs = new List<EntityRecord>();
				var taskTimelogs = new Dictionary<Guid, List<EntityRecord>>();
				var bugTimelogs = new Dictionary<Guid, List<EntityRecord>>();

				#region << Get timelogs from the period >>
				var startDate = new DateTime(year, month, 1);
				var endDate = startDate.AddMonths(1);
				{
					var requestedFields = "billable,hours," +
					"$$bug_1_n_time_log.id,$$task_1_n_time_log.id";
					var filterObj = EntityQuery.QueryAND(EntityQuery.QueryGTE("log_date", startDate), EntityQuery.QueryLT("log_date", endDate));
					EntityQuery resultQuery = new EntityQuery("wv_timelog", requestedFields, filterObj);
					QueryResponse result = recMan.Find(resultQuery);
					if (!result.Success)
					{
						response.Success = false;
						response.Timestamp = DateTime.UtcNow;
						response.Message = result.Message;
						response.Object = null;
						return Json(response);
					}

					timelogs = result.Object.Data;
					var addedTasks = new Dictionary<Guid, bool>();
					var addedBugs = new Dictionary<Guid, bool>();
					foreach (var timelog in timelogs)
					{

						if (((List<EntityRecord>)timelog["$task_1_n_time_log"]).Any())
						{
							var task = ((List<EntityRecord>)timelog["$task_1_n_time_log"])[0];
							var taskId = (Guid)task["id"];
							if (!addedTasks.ContainsKey(taskId))
							{
								addedTasks[taskId] = true;
								workedTaskIds.Add(taskId);
							}
							var timelogList = new List<EntityRecord>();
							if (taskTimelogs.ContainsKey(taskId))
							{
								timelogList = taskTimelogs[taskId];
								timelogList.Add(timelog);
							}
							else
							{
								timelogList.Add(timelog);
							}
							taskTimelogs[taskId] = timelogList;
						}

						if (((List<EntityRecord>)timelog["$bug_1_n_time_log"]).Any())
						{
							var bug = ((List<EntityRecord>)timelog["$bug_1_n_time_log"])[0];
							var bugId = (Guid)bug["id"];
							if (!addedBugs.ContainsKey(bugId))
							{
								addedBugs[bugId] = true;
								workedBugIds.Add(bugId);
							}
							var timelogList = new List<EntityRecord>();
							if (bugTimelogs.ContainsKey(bugId))
							{
								timelogList = bugTimelogs[bugId];
								timelogList.Add(timelog);
							}
							else
							{
								timelogList.Add(timelog);
							}
							bugTimelogs[bugId] = timelogList;
						}
					}

				}
				#endregion

				#region << Get tasks >>
				{
					if (workedTaskIds.Count() > 0)
					{
						var requestedFields = "id,subject,project_id";
						var queryList = new List<QueryObject>();
						foreach (var taskId in workedTaskIds)
						{
							var query = EntityQuery.QueryEQ("id", taskId);
							queryList.Add(query);
						}

						var filterObj = EntityQuery.QueryOR(queryList.ToArray());
						var sortRulesList = new List<QuerySortObject>();
						var sortRule = new QuerySortObject("created_on", QuerySortType.Ascending);
						sortRulesList.Add(sortRule);
						EntityQuery resultQuery = new EntityQuery("wv_task", requestedFields, filterObj, sortRulesList.ToArray());
						QueryResponse result = recMan.Find(resultQuery);
						if (!result.Success)
						{
							response.Success = false;
							response.Timestamp = DateTime.UtcNow;
							response.Message = result.Message;
							response.Object = null;
							return Json(response);
						}

						workedTasks = result.Object.Data;
					}
				}
				#endregion

				#region << Get bugs >>
				{
					if (workedBugIds.Count() > 0)
					{
						var requestedFields = "id,subject,project_id";
						var queryList = new List<QueryObject>();
						foreach (var bugId in workedBugIds)
						{
							var query = EntityQuery.QueryEQ("id", bugId);
							queryList.Add(query);
						}
						var filterObj = EntityQuery.QueryOR(queryList.ToArray());
						var sortRulesList = new List<QuerySortObject>();
						var sortRule = new QuerySortObject("created_on", QuerySortType.Ascending);
						sortRulesList.Add(sortRule);
						EntityQuery resultQuery = new EntityQuery("wv_bug", requestedFields, filterObj, sortRulesList.ToArray());
						QueryResponse result = recMan.Find(resultQuery);
						if (!result.Success)
						{
							response.Success = false;
							response.Timestamp = DateTime.UtcNow;
							response.Message = result.Message;
							response.Object = null;
							return Json(response);
						}

						workedBugs = result.Object.Data;
					}
				}
				#endregion

				#region << Generate project task & bugs dict >>
				foreach (var task in workedTasks)
				{
					var taskId = (Guid)task["id"];
					var taskProjectId = (Guid)task["project_id"];
					var taskBillable = (decimal)0;
					var taskNotBillable = (decimal)0;
					var taskTimeLogList = taskTimelogs[taskId];
					foreach (var timelog in taskTimeLogList)
					{
						if ((bool)timelog["billable"])
						{
							taskBillable += (decimal)timelog["hours"];
						}
						else
						{
							taskNotBillable += (decimal)timelog["hours"];
						}
					}
					task["billable"] = taskBillable;
					task["not_billable"] = taskNotBillable;
					task["type"] = "task";
					var projectBugAndTasks = new List<EntityRecord>();
					if (projectTaskBugsDict.ContainsKey(taskProjectId))
					{
						projectBugAndTasks = projectTaskBugsDict[taskProjectId];
					}
					projectBugAndTasks.Add(task);
					projectTaskBugsDict[taskProjectId] = projectBugAndTasks;
				}
				foreach (var bug in workedBugs)
				{
					var bugId = (Guid)bug["id"];
					var bugProjectId = (Guid)bug["project_id"];
					var bugBillable = (decimal)0;
					var bugNotBillable = (decimal)0;
					var bugTimeLogList = bugTimelogs[bugId];
					foreach (var timelog in bugTimeLogList)
					{
						if ((bool)timelog["billable"])
						{
							bugBillable += (decimal)timelog["hours"];
						}
						else
						{
							bugNotBillable += (decimal)timelog["hours"];
						}
					}
					bug["billable"] = bugBillable;
					bug["not_billable"] = bugNotBillable;
					bug["type"] = "bug";
					var projectBugAndTasks = new List<EntityRecord>();
					if (projectTaskBugsDict.ContainsKey(bugProjectId))
					{
						projectBugAndTasks = projectTaskBugsDict[bugProjectId];
					}
					projectBugAndTasks.Add(bug);
					projectTaskBugsDict[bugProjectId] = projectBugAndTasks;
				}
				#endregion

				#region << Get all projects >>
				{
					var requestedFields = "id,name";
					var queryList = new List<QueryObject>();
					foreach (var taskId in workedTaskIds)
					{
						var query = EntityQuery.QueryEQ("id", taskId);
						queryList.Add(query);
					}

					QueryObject filterObj = null;
					var sortRulesList = new List<QuerySortObject>();
					var sortRule = new QuerySortObject("created_on", QuerySortType.Ascending);
					sortRulesList.Add(sortRule);
					EntityQuery resultQuery = new EntityQuery("wv_project", requestedFields, filterObj, sortRulesList.ToArray());
					QueryResponse result = recMan.Find(resultQuery);
					if (!result.Success)
					{
						response.Success = false;
						response.Timestamp = DateTime.UtcNow;
						response.Message = result.Message;
						response.Object = null;
						return Json(response);
					}

					projects = result.Object.Data;

				}
				#endregion

				#region << Generate project with logs >>
				var totalBillable = (decimal)0;
				var totalNotBillable = (decimal)0;
				foreach (var project in projects)
				{
					if (projectTaskBugsDict.ContainsKey((Guid)project["id"]))
					{
						var projectEntries = (List<EntityRecord>)projectTaskBugsDict[(Guid)project["id"]];
						project["entries"] = projectEntries;
						project["billable"] = (decimal)0;
						project["not_billable"] = (decimal)0;
						foreach (var entry in projectEntries)
						{
							project["billable"] = (decimal)project["billable"] + (decimal)entry["billable"];
							project["not_billable"] = (decimal)project["not_billable"] + (decimal)entry["not_billable"];
						}
						totalBillable += (decimal)project["billable"];
						totalNotBillable += (decimal)project["not_billable"];
						projectsWithTimelogs.Add(project);
					}
				}

				#endregion


				#region
				var responseObject = new EntityRecord();
				responseObject["projects"] = projectsWithTimelogs;
				responseObject["billable"] = totalBillable;
				responseObject["not_billable"] = totalNotBillable;
				response.Success = true;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Report successfully generated";
				response.Object = responseObject;

				return Json(response);

				#endregion

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Error: " + ex.Message;
				response.Object = null;
				return Json(response);
			}
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/sprint/list")]
		public IActionResult GetSprintList(int page = 1, int pageSize = 1)
		{
			var response = new ResponseModel();
			var sprintList = new List<EntityRecord>();
			var skipPages = (page - 1) * pageSize;
			try
			{
				QueryObject queryFilter = null;
				var sortRulesList = new List<QuerySortObject>();
				var sortRule = new QuerySortObject("start_date", QuerySortType.Descending);
				sortRulesList.Add(sortRule);
				var query = new EntityQuery("wv_sprint", "*", queryFilter, sortRulesList.ToArray(), skipPages, pageSize);
				var queryResponse = recMan.Find(query);
				if (!queryResponse.Success)
				{
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					response.Message = "Error: " + queryResponse.Message;
					response.Object = null;
					return Json(response);
				}
				if (queryResponse.Object.Data.Any())
				{
					sprintList = queryResponse.Object.Data;
				}
				else
				{
					response.Success = true;
					response.Timestamp = DateTime.UtcNow;
					response.Message = "No sprints found!";
					response.Object = null;
					return Json(response);
				}

				response.Success = true;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Sprints successfully read";
				response.Object = sprintList;

				return Json(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Error: " + ex.Message;
				response.Object = null;
				return Json(response);
			}
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/sprint/{sprintId?}")]
		public IActionResult SprintDetails(Guid? sprintId = null, string scope = "user")
		{
			var response = new ResponseModel();
			var runningSprints = new List<EntityRecord>();
			var currentSprint = new EntityRecord();
			var userDictionary = new Dictionary<Guid, EntityRecord>();
			try
			{
				Guid? currentSprintId = null;

				#region << Get all users >>
				{
					var query = new EntityQuery("user");
					var queryResponse = recMan.Find(query);
					if (!queryResponse.Success)
					{
						response.Success = false;
						response.Timestamp = DateTime.UtcNow;
						response.Message = "Error: " + queryResponse.Message;
						response.Object = null;
						return Json(response);
					}
					foreach (var user in queryResponse.Object.Data)
					{
						userDictionary[(Guid)user["id"]] = user;
					}
				}

				#endregion

				#region << Init the current sprint id >>
				if (sprintId == null)
				{
					//Get all current sprints
					{
						var sprintFields = "id";
						var queryFilter = EntityQuery.QueryAND(EntityQuery.QueryLTE("start_date", DateTime.UtcNow), EntityQuery.QueryGTE("end_date", DateTime.UtcNow.AddDays(-1)));
						var sortRulesList = new List<QuerySortObject>();
						var sortRule = new QuerySortObject("start_date", QuerySortType.Ascending);
						sortRulesList.Add(sortRule);
						var query = new EntityQuery("wv_sprint", sprintFields, queryFilter, sortRulesList.ToArray());
						var queryResponse = recMan.Find(query);
						if (!queryResponse.Success)
						{
							response.Success = false;
							response.Timestamp = DateTime.UtcNow;
							response.Message = "Error: " + queryResponse.Message;
							response.Object = null;
							return Json(response);
						}
						if (queryResponse.Object.Data.Any())
						{
							runningSprints = queryResponse.Object.Data;
						}
						else
						{
							response.Success = true;
							response.Timestamp = DateTime.UtcNow;
							response.Message = "Sprints successfully read, but no current sprints found!";
							response.Object = null;
							return Json(response);
						}
					}

					//Find the first sprint that is current to the current data
					currentSprintId = (Guid)runningSprints[0]["id"];
				}
				else
				{
					currentSprintId = sprintId;
				}

				#endregion

				#region << Get current sprint details >>
				{
					var fields = "id,start_date,end_date,name," +
					"$wv_sprint_n_n_wv_task.id,$wv_sprint_n_n_wv_task.code,$wv_sprint_n_n_wv_task.owner_id, $wv_sprint_n_n_wv_task.priority,$wv_sprint_n_n_wv_task.status,$wv_sprint_n_n_wv_task.subject," +
					"$wv_sprint_n_n_wv_task.x_billable_hours,$wv_sprint_n_n_wv_task.x_nonbillable_hours,$wv_sprint_n_n_wv_task.estimation," +
					"$$role_n_n_wv_sprint.id";
					var queryFilter = EntityQuery.QueryEQ("id", currentSprintId);
					var query = new EntityQuery("wv_sprint", fields, queryFilter);
					var queryResponse = recMan.Find(query);
					if (!queryResponse.Success)
					{
						response.Success = false;
						response.Timestamp = DateTime.UtcNow;
						response.Message = "Error: " + queryResponse.Message;
						response.Object = null;
						return Json(response);
					}
					if (!queryResponse.Object.Data.Any())
					{
						response.Success = false;
						response.Timestamp = DateTime.UtcNow;
						response.Message = "There is no sprint with this Id";
						response.Object = null;
						return Json(response);
					}

					currentSprint = queryResponse.Object.Data[0];
					var sprintRoleRecords = (List<EntityRecord>)currentSprint["$role_n_n_wv_sprint"];
					//Check if the current user can see the sprint
					var userRoles = SecurityContext.CurrentUser.Roles;
					var userCanAccessSprint = false;
					foreach(var role in sprintRoleRecords) {
						if(userRoles.Any(x => x.Id == (Guid)role["id"])) {
							userCanAccessSprint = true;
							break;
						}
					}
					if(!userCanAccessSprint) {
						response.Success = true;
						response.Timestamp = DateTime.UtcNow;
						response.Message = "no access";
						response.Object = null;	
						return Json(response);					
					}

				}
				#endregion

				var processedSprintObject = new EntityRecord();
				processedSprintObject["id"] = (Guid)currentSprint["id"];
				processedSprintObject["start_date"] = (DateTime)currentSprint["start_date"];
				processedSprintObject["end_date"] = (DateTime)currentSprint["end_date"];
				processedSprintObject["name"] = (string)currentSprint["name"];
				processedSprintObject["estimation"] = (decimal)0;
				processedSprintObject["logged"] = (decimal)0;
				processedSprintObject["tasks_not_started"] = new List<EntityRecord>();
				processedSprintObject["tasks_in_progress"] = new List<EntityRecord>();
				processedSprintObject["tasks_completed"] = new List<EntityRecord>();
				foreach (var task in (List<EntityRecord>)currentSprint["$wv_sprint_n_n_wv_task"])
				{
					if ((scope == "user" && (Guid)task["owner_id"] == SecurityContext.CurrentUser.Id) || scope != "user")
					{
						var proccessedTask = new EntityRecord();
						proccessedTask["id"] = (Guid)task["id"];
						proccessedTask["code"] = (string)task["code"];
						proccessedTask["priority"] = (string)task["priority"];
						proccessedTask["status"] = (string)task["status"];
						proccessedTask["subject"] = (string)task["subject"];
						proccessedTask["estimation"] = (decimal)task["estimation"];
						proccessedTask["logged"] = (decimal)task["x_nonbillable_hours"] + (decimal)task["x_billable_hours"];
						proccessedTask["owner_username"] = (string)userDictionary[(Guid)task["owner_id"]]["username"];
						proccessedTask["owner_image"] = (string)userDictionary[(Guid)task["owner_id"]]["image"];
						switch ((string)task["status"])
						{
							case "not started":
								((List<EntityRecord>)processedSprintObject["tasks_not_started"]).Add(proccessedTask);
								break;
							case "in progress":
								((List<EntityRecord>)processedSprintObject["tasks_in_progress"]).Add(proccessedTask);
								break;
							case "completed":
								((List<EntityRecord>)processedSprintObject["tasks_completed"]).Add(proccessedTask);
								break;
						}
						processedSprintObject["estimation"] = (decimal)processedSprintObject["estimation"] + (decimal)task["estimation"];
						processedSprintObject["logged"] = (decimal)processedSprintObject["logged"] + (decimal)proccessedTask["logged"];
					}
				}
				processedSprintObject["estimation"] = Math.Round((decimal)processedSprintObject["estimation"],2);
				processedSprintObject["logged"] = Math.Round((decimal)processedSprintObject["logged"],2);
				response.Success = true;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "My projects successfully read";
				response.Object = processedSprintObject;

				return Json(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Error: " + ex.Message;
				response.Object = null;
				return Json(response);
			}
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/sprint/{sprintId}/available-tasks")]
		public IActionResult GetSprintTasksList(Guid sprintId, string status = "not started", string scope = "user", int page = 1, int pageSize = 1)
		{
			var response = new ResponseModel();
			var taskList = new List<EntityRecord>();
			var processedTaskList = new List<EntityRecord>();
			var userDictionary = new Dictionary<Guid, EntityRecord>();
			var skipPages = (page - 1) * pageSize;
			try
			{

				#region << Get all users >>
				{
					var query = new EntityQuery("user");
					var queryResponse = recMan.Find(query);
					if (!queryResponse.Success)
					{
						response.Success = false;
						response.Timestamp = DateTime.UtcNow;
						response.Message = "Error: " + queryResponse.Message;
						response.Object = null;
						return Json(response);
					}
					foreach (var user in queryResponse.Object.Data)
					{
						userDictionary[(Guid)user["id"]] = user;
					}
				}

				#endregion

				#region << Get tasks >>
				{
					QueryObject queryFilter = null;
					var queryRulesList = new List<QueryObject>();
					//Only not completed tasks
					queryRulesList.Add(EntityQuery.QueryNOT("status", "completed"));
					if (scope == "user")
					{
						queryRulesList.Add(EntityQuery.QueryEQ("owner_id", SecurityContext.CurrentUser.Id));
					}
					queryRulesList.Add(EntityQuery.QueryEQ("status", status));

					queryFilter = EntityQuery.QueryAND(queryRulesList.ToArray());

					var sortRulesList = new List<QuerySortObject>();
					var sortRule = new QuerySortObject("created_on", QuerySortType.Descending);
					sortRulesList.Add(sortRule);
					var queryFields = "id,code,owner_id, priority,status,subject," +
						"x_billable_hours,x_nonbillable_hours,estimation";

					var query = new EntityQuery("wv_task", queryFields, queryFilter, sortRulesList.ToArray(), skipPages, pageSize);
					var queryResponse = recMan.Find(query);
					if (!queryResponse.Success)
					{
						response.Success = false;
						response.Timestamp = DateTime.UtcNow;
						response.Message = "Error: " + queryResponse.Message;
						response.Object = null;
						return Json(response);
					}
					if (queryResponse.Object.Data.Any())
					{
						taskList = queryResponse.Object.Data;
					}
					else
					{
						response.Success = true;
						response.Timestamp = DateTime.UtcNow;
						response.Message = "No tasks found!";
						response.Object = new List<EntityRecord>();
						return Json(response);
					}
				}
				#endregion

				#region << process Tasks >>
				foreach (var task in taskList)
				{
					var proccessedTask = new EntityRecord();
					proccessedTask["id"] = (Guid)task["id"];
					proccessedTask["code"] = (string)task["code"];
					proccessedTask["priority"] = (string)task["priority"];
					proccessedTask["status"] = (string)task["status"];
					proccessedTask["subject"] = (string)task["subject"];
					proccessedTask["estimation"] = (decimal)task["estimation"];
					proccessedTask["logged"] = (decimal)task["x_nonbillable_hours"] + (decimal)task["x_billable_hours"];
					proccessedTask["owner_username"] = (string)userDictionary[(Guid)task["owner_id"]]["username"];
					proccessedTask["owner_image"] = (string)userDictionary[(Guid)task["owner_id"]]["image"];
					processedTaskList.Add(proccessedTask);
				}
				#endregion

				response.Success = true;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Tasks successfully read";
				response.Object = processedTaskList;

				return Json(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Error: " + ex.Message;
				response.Object = null;
				return Json(response);
			}
		}


		[AcceptVerbs(new[] { "POST" }, Route = "/plugins/webvella-projects/api/comment/upsert")]
		public IActionResult UpsertComment([FromBody]JObject submitObj)
		{
			var response = new ResponseModel();
			var submitRecord = new EntityRecord();
			var postRecord = new EntityRecord();
			postRecord["task_id"] = null;
			postRecord["bug_id"] = null;
			var recepients = new List<string>();
			var task = new EntityRecord();
			var bug = new EntityRecord();
			var upsertResultRecord = new EntityRecord();
			#region << Init Object >>
			var outGuid = Guid.Empty;
			foreach (var prop in submitObj.Properties())
			{
				switch (prop.Name.ToLower())
				{
					case "id":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
						{
							if (Guid.TryParse((string)prop.Value, out outGuid))
							{
								submitRecord["id"] = outGuid;
							}
							else
							{
								//throw error
								response.Success = false;
								response.Message = "Comment Id is not a valid Guid";
								return Json(response);
							}
						}
						else
						{
							submitRecord["id"] = null;
						}
						break;
					case "task_id":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
						{
							if (Guid.TryParse((string)prop.Value, out outGuid))
							{
								submitRecord["task_id"] = outGuid;
							}
							else
							{
								//throw error
								response.Success = false;
								response.Message = "Task Id is not a valid Guid";
								return Json(response);
							}
						}
						else
						{
							submitRecord["task_id"] = null;
						}
						break;
					case "bug_id":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
						{
							if (Guid.TryParse((string)prop.Value, out outGuid))
							{
								submitRecord["bug_id"] = outGuid;
							}
							else
							{
								//throw error
								response.Success = false;
								response.Message = "Bug Id is not a valid Guid";
								return Json(response);
							}
						}
						else
						{
							submitRecord["bug_id"] = null;
						}
						break;
					case "new_owner_id":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
						{
							if (Guid.TryParse((string)prop.Value, out outGuid))
							{
								submitRecord["new_owner_id"] = outGuid;
							}
							else
							{
								//throw error
								response.Success = false;
								response.Message = "New Owner Id is not a valid Guid";
								return Json(response);
							}
						}
						else
						{
							submitRecord["new_owner_id"] = null;
						}
						break;
					case "content":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
						{
							submitRecord["content"] = prop.Value.ToString();
						}
						else
						{
							//throw error
							response.Success = false;
							response.Message = "Content is required";
							return Json(response);
						}
						break;
				}
			}

			#endregion

			if(submitRecord.Properties.ContainsKey("id") && submitRecord["id"] != null) {
				postRecord["id"] = (Guid)submitRecord["id"];
			}else {
				postRecord["id"] = Guid.NewGuid();			
			}
			if(submitRecord.Properties.ContainsKey("task_id") && submitRecord["task_id"] != null) {
				postRecord["task_id"] = (Guid)submitRecord["task_id"];
			}
			if(submitRecord.Properties.ContainsKey("bug_id") && submitRecord["bug_id"] != null) {
				postRecord["bug_id"] = (Guid)submitRecord["bug_id"];
			}
			postRecord["content"] = (string)submitRecord["content"];
			using (var connection = DbContext.Current.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();
					//Upset comment
					if(submitRecord["id"] == null) {
						//create
						var createResponse = recMan.CreateRecord("wv_project_comment",postRecord);
						if(!createResponse.Success) {
							throw new Exception(createResponse.Message);
						}
						response.Message = "Comment successfully created";
						upsertResultRecord = createResponse.Object.Data.First();
					}
					else {
						//update
						var updateResponse = recMan.UpdateRecord("wv_project_comment",postRecord);
						if(!updateResponse.Success) {
							throw new Exception(updateResponse.Message);
						}
						response.Message = "Comment successfully updated";
						upsertResultRecord = updateResponse.Object.Data.First();
					}

					#region << New owner >>
					if (submitRecord.Properties.ContainsKey("new_owner_id") && submitRecord["new_owner_id"] != null) {
						//Check the new owner id
						{
							var query = EntityQuery.QueryEQ("id",(Guid)submitRecord["new_owner_id"]);
							var queryResponse = recMan.Find(new EntityQuery("user","*",query));
							if(!queryResponse.Success) {
								throw new Exception(queryResponse.Message);
							}
							if(queryResponse.Object.Data == null || !queryResponse.Object.Data.Any()) {
								throw new Exception("cannot find user with the provided new owner id");
							}						
						}
						//Change task/bug owner if needed
						if(submitRecord.Properties.ContainsKey("task_id") && submitRecord["task_id"] != null) {
							var patchRecord = new EntityRecord();
							var query = EntityQuery.QueryEQ("id",(Guid)submitRecord["task_id"]);
							var queryResponse = recMan.Find(new EntityQuery("wv_task","*",query));
							if(!queryResponse.Success) {
								throw new Exception(queryResponse.Message);
							}
							if(queryResponse.Object.Data == null || !queryResponse.Object.Data.Any()) {
								throw new Exception("cannot find task with the provided id");
							}
							patchRecord["id"] = (Guid)submitRecord["task_id"];
							patchRecord["owner_id"] = (Guid)submitRecord["new_owner_id"];
							var patchRecordResponse = recMan.UpdateRecord("wv_task",patchRecord);
							if(!patchRecordResponse.Success) {
								throw new Exception(patchRecordResponse.Message);
							}
						}
						else if(submitRecord.Properties.ContainsKey("bug_id") && submitRecord["bug_id"] != null) {
							var patchRecord = new EntityRecord();
							var query = EntityQuery.QueryEQ("id",(Guid)submitRecord["bug_id"]);
							var queryResponse = recMan.Find(new EntityQuery("wv_bug","*",query));
							if(!queryResponse.Success) {
								throw new Exception(queryResponse.Message);
							}
							if(queryResponse.Object.Data == null || !queryResponse.Object.Data.Any()) {
								throw new Exception("cannot find bug with the provided id");
							}
							patchRecord["id"] = (Guid)submitRecord["bug_id"];
							patchRecord["owner_id"] = (Guid)submitRecord["new_owner_id"];
							var patchRecordResponse = recMan.UpdateRecord("wv_bug",patchRecord);
							if(!patchRecordResponse.Success) {
								throw new Exception(patchRecordResponse.Message);
							}				
						}
					}
					#endregion

					#region << Get newly created record >>
					if (postRecord["task_id"] != null)
					{
						var filterObj = EntityQuery.QueryEQ("id", (Guid)postRecord["task_id"]);
						var query = new EntityQuery("wv_task", "id,code,subject,status,description,project_id,$$user_n_n_task_watchers.id,$$user_n_n_task_watchers.email,$$project_1_n_task.code", filterObj, null, null, null);
						var result = recMan.Find(query);
						if (result.Success)
						{
							task = result.Object.Data[0];
							Utils.CreateActivity(recMan, "commented", "created a <i class='fa fa-fw fa-comment-o go-blue'></i> comment for task [" + task["code"] + "] <a href='/#/areas/projects/wv_task/view-general/sb/general/" + task["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)task["subject"]) + "</a>", null, (Guid)task["project_id"], (Guid)task["id"], null);
							//If status was completed turn it back to in progress
							if((string)task["status"] == "completed") {
								var newTask = new EntityRecord();
								newTask["id"] = (Guid)task["id"];
								newTask["status"] = "in progress";
								var updateResult = recMan.UpdateRecord("wv_task",newTask,true);
								if(!updateResult.Success) {
									throw new Exception(result.Message);
								}
							}
						}
						else
						{
							throw new Exception(result.Message);
						}
					}
					else if (postRecord["bug_id"] != null)
					{
						var filterObj = EntityQuery.QueryEQ("id", (Guid)postRecord["bug_id"]);
						var query = new EntityQuery("wv_bug", "id,code,subject,status,description,project_id,$$user_n_n_bug_watchers.id,$$user_n_n_bug_watchers.email,$$project_1_n_bug.code", filterObj, null, null, null);
						var result = recMan.Find(query);
						if (result.Success)
						{
							bug = result.Object.Data[0];
							Utils.CreateActivity(recMan, "commented", "created a <i class='fa fa-fw fa-comment-o go-blue'></i> comment for bug [" + bug["code"] + "] <a href='/#/areas/projects/wv_bug/view-general/sb/general/" + bug["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)bug["subject"]) + "</a>", null, (Guid)bug["project_id"], null, (Guid)bug["id"]);
							//If status was closed turn it back to reopened
							if((string)bug["status"] == "closed") {
								var newBug = new EntityRecord();
								newBug["id"] = (Guid)bug["id"];
								newBug["status"] = "reopened";
								var updateResult = recMan.UpdateRecord("wv_bug",newBug,true);
								if(!updateResult.Success) {
									throw new Exception(result.Message);
								}
							}
						}
						else
						{
							throw new Exception(result.Message);
						}
					}
					#endregion

					var commentatorUsername = "";
					#region << Get username of the commentator>>
					{
						EntityQuery query = new EntityQuery("user", "username", EntityQuery.QueryEQ("id", (Guid)upsertResultRecord["created_by"]), null, null, null);
						QueryResponse result = recMan.Find(query);
						if (!result.Success)
						{
							throw new Exception("Cannot get the username of the commentator");
						}
						commentatorUsername = (string)result.Object.Data[0]["username"];
					}
					#endregion

					#region << Add the comment creator to the watch list if he is not there, Generate recipients list >>
					{
						if (postRecord.Properties.ContainsKey("task_id") &&  postRecord["task_id"] != null)
						{
							var isCommentatorInWatchList = false;
							#region << Check if is in watch list already >>
							foreach (var watcher in (List<EntityRecord>)task["$user_n_n_task_watchers"])
							{
								if ((Guid)watcher["id"] == (Guid)upsertResultRecord["created_by"])
								{
									isCommentatorInWatchList = true;
								}
								else
								{
									recepients.Add((string)watcher["email"]);
								}
							}
							#endregion
							if (!isCommentatorInWatchList)
							{
								var targetRelation = relMan.Read("user_n_n_task_watchers").Object;
								var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(targetRelation.Id, (Guid)upsertResultRecord["created_by"], new Guid(postRecord["task_id"].ToString()));
								if (!createRelationNtoNResponse.Success)
								{
									throw new Exception("Could not create watch relation" + createRelationNtoNResponse.Message);
								}
							}
						}
						else if (postRecord.Properties.ContainsKey("bug_id") &&  postRecord["bug_id"] != null)
						{
							var isCommentatorInWatchList = false;
							#region << Check if is in watch list already >>
							foreach (var watcher in (List<EntityRecord>)bug["$user_n_n_bug_watchers"])
							{
								if ((Guid)watcher["id"] == (Guid)upsertResultRecord["created_by"])
								{
									isCommentatorInWatchList = true;
								}
								else
								{
									recepients.Add((string)watcher["email"]);
								}
							}
							#endregion
							if (!isCommentatorInWatchList)
							{
								var targetRelation = relMan.Read("user_n_n_bug_watchers").Object;
								var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(targetRelation.Id, (Guid)upsertResultRecord["created_by"], new Guid(postRecord["bug_id"].ToString()));
								if (!createRelationNtoNResponse.Success)
								{
									throw new Exception("Could not create watch relation" + createRelationNtoNResponse.Message);
								}
							}
						}
					}
					#endregion
					
					#region << Generate notifications to watch list>>
					var emailService = new EmailService();
					if (recepients.Count > 0)
					{
						try
						{
							if (postRecord["task_id"] != null)
							{
								var emailSubjectParameters = new Dictionary<string, string>();
								emailSubjectParameters["code"] = (string)task["code"];
								emailSubjectParameters["subject"] = (string)task["subject"];

								var subject = Regex.Replace(EmailTemplates.NewCommentNotificationSubject, @"\{(.+?)\}", m => emailSubjectParameters[m.Groups[1].Value]);

								var emailContentParameters = new Dictionary<string, string>();
								emailContentParameters["baseUrl"] = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
								emailContentParameters["subject"] = subject;
								emailContentParameters["type"] = "task";
								emailContentParameters["creator"] = commentatorUsername;
								emailContentParameters["taskOrBugUrl"] = emailContentParameters["baseUrl"] + "/#/areas/projects/wv_task/view-general/sb/general/" + postRecord["task_id"];
								emailContentParameters["commentContent"] = (string)postRecord["content"];
								emailContentParameters["taskOrBugDescription"] = (string)task["description"];

								var content = Regex.Replace(EmailTemplates.NewCommentNotificationContent, @"\{(.+?)\}", m => emailContentParameters[m.Groups[1].Value]);

								emailService.SendEmail(recepients.ToArray(), subject, content);
							}
							else if (postRecord["bug_id"] != null)
							{
								var emailSubjectParameters = new Dictionary<string, string>();
								emailSubjectParameters["code"] = (string)bug["code"];
								emailSubjectParameters["subject"] = (string)bug["subject"];

								var subject = Regex.Replace(EmailTemplates.NewCommentNotificationSubject, @"\{(.+?)\}", m => emailSubjectParameters[m.Groups[1].Value]);

								var emailContentParameters = new Dictionary<string, string>();
								emailContentParameters["baseUrl"] = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
								emailContentParameters["subject"] = subject;
								emailContentParameters["type"] = "bug";
								emailContentParameters["creator"] = commentatorUsername;
								emailContentParameters["taskOrBugUrl"] = emailContentParameters["baseUrl"] + "/#/areas/projects/wv_bug/view-general/sb/general/" + postRecord["bug_id"];
								emailContentParameters["commentContent"] = (string)postRecord["content"];
								emailContentParameters["taskOrBugDescription"] = (string)bug["description"];

								var content = Regex.Replace(EmailTemplates.NewCommentNotificationContent, @"\{(.+?)\}", m => emailContentParameters[m.Groups[1].Value]);

								emailService.SendEmail(recepients.ToArray(), subject, content);
							}
						}
						catch (Exception ex)
						{
							throw ex;
						}
					}

					#endregion

					#region << Regen FTS >>
					var regenRecord = new EntityRecord();
					if(submitRecord.Properties.ContainsKey("task_id") && submitRecord["task_id"] != null) {
						regenRecord["id"] = new Guid(submitRecord["task_id"].ToString());
						Utils.RegenAndUpdateTaskFts((EntityRecord)regenRecord,recMan);
					}
					else if(submitRecord.Properties.ContainsKey("bug_id") && submitRecord["bug_id"] != null) {
						regenRecord["id"] = new Guid(submitRecord["bug_id"].ToString());
						Utils.RegenAndUpdateBugFts((EntityRecord)regenRecord,recMan);
					}
					#endregion
					connection.CommitTransaction();
					response.Success = true;
					response.Timestamp = DateTime.UtcNow;
					return Json(response);
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					response.Success = false;
					response.Timestamp = DateTime.UtcNow;
					response.Message = "Error: " + ex.Message;
					response.Object = null;
					return Json(response);
				}
			}
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-projects/api/fts/regen")]
		public IActionResult RegenBugAndTaskFTS()
		{
			var response = new ResponseModel();
			try
			{
				var tasks = new List<EntityRecord>();
				#region << get tasks >>
				{
					var query = new EntityQuery("wv_task", "*,$task_1_n_comment.content,$$project_1_n_task.name,$$project_1_n_task.code,$$user_1_n_task_owner.username,$$user_wv_task_created_by.username");
					var result = recMan.Find(query);
					if (!result.Success)
					{

						throw new Exception("Error getting the updated tasks: " + result.Message);
					}
					else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
					{
						throw new Exception("Bug not found");
					}
					tasks = result.Object.Data;
				}
				#endregion

				var bugs = new List<EntityRecord>();
				#region << get bugs >>
				{
					var query = new EntityQuery("wv_bug", "*,$bug_1_n_comment.content,$$project_1_n_bug.name,$$project_1_n_bug.code,$$user_1_n_bug_owner.username,$$user_wv_bug_created_by.username");
					var result = recMan.Find(query);
					if (!result.Success)
					{

						throw new Exception("Error getting the updated bugs: " + result.Message);
					}
					else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
					{
						throw new Exception("Bug not found");
					}
					bugs = result.Object.Data;
				}
				#endregion

				foreach (var task in tasks)
				{
					Utils.RegenAndUpdateTaskFts(task,recMan);
				}

				foreach (var bug in bugs)
				{
					Utils.RegenAndUpdateBugFts(bug,recMan);
				}

				response.Success = true;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "FTS regeneration success";
				response.Object = null;
				return Json(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = "Error: " + ex.Message;
				response.Object = null;
				return Json(response);
			}
		}
	}
}
