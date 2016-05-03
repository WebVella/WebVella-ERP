using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
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

			#region << Init >>
			var responseData = new List<EntityRecord>();
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
				return Json(responseData); //return empty object
			}
			#endregion

			#region << Init fields >>
			var requestedFields = "id,name,start_date,end_date," +
			"$user_1_n_project_owner.id,$user_1_n_project_owner.image,$user_1_n_project_owner.username," +
			"$project_1_n_milestone.id,$project_1_n_milestone.status," +
			"$project_1_n_task.id,$project_1_n_task.status," +
			"$project_1_n_bug.id,$project_1_n_bug.status," +
			"$role_n_n_project_team.id,$role_n_n_project_customer.id";
			#endregion

			#region << Query builder >>
			//QueryObject filterObj = EntityQuery.QueryEQ("id", recordId);
			QueryObject filterObj = null;
			EntityQuery resultQuery = new EntityQuery(entityName, requestedFields, filterObj, null, null, null, null);
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
					var milestonesOpened = 0;
					var milestonesCompleted = 0;
					foreach (var item in (List<EntityRecord>)record["$project_1_n_milestone"])
					{
						switch ((string)item["status"])
						{
							case "opened":
								milestonesOpened++;
								break;
							case "completed":
								milestonesCompleted++;
								break;
						}
					}

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
					var tasksNotStarted = 0;
					var tasksInProgress = 0;
					var tasksCompleted = 0;
					foreach (var item in (List<EntityRecord>)record["$project_1_n_task"])
					{
						switch ((string)item["status"])
						{
							case "not started":
								tasksNotStarted++;
								break;
							case "in progress":
								tasksInProgress++;
								break;
							case "completed":
								tasksCompleted++;
								break;
						}
					}

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
					var bugsOpened = 0;
					var bugsReOpened = 0;
					var bugsClosed = 0;
					foreach (var item in (List<EntityRecord>)record["$project_1_n_bug"])
					{
						switch ((string)item["status"])
						{
							case "opened":
								bugsOpened++;
								break;
							case "reopened":
								bugsReOpened++;
								break;
							case "closed":
								bugsClosed++;
								break;
						}
					}


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

	}
}
