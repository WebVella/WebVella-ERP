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
			"$project_1_n_milestone.id,$project_1_n_milestone.name," +
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
					resultRecordsList.Add(recordObj);
				}
			}
			#endregion

			var skipRecords = list.PageSize*(page-1);
			if(page != 0) {
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
