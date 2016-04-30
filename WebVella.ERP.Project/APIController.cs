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

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-plugin/api/")]
		public IActionResult Index()
		{
			return Json(new { Message = "This is a sample JSON response from webvella erp sample plugin controller." });
		}

		[AcceptVerbs(new[] { "GET" }, Route = "/plugins/webvella-plugin/api/project/list/my-projects")]
		public IActionResult MyProjects(string listName = null, string entityName = null, int page = 0)
		{
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
			var listFields = new List<string>();
			listFields.Add("id");
			foreach (var column in list.Columns)
			{
				if (!column.DataName.Contains("$") && column.DataName != "id")
				{
					listFields.Add(column.DataName);
				}
			}
			listFields.Add("$user_1_n_project.id");
			listFields.Add("$role_n_n_project_staff.id");
			listFields.Add("$role_n_n_project_customer.id");
			var requestedFields = string.Join(",", listFields);
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
				return Json(responseData); //return empty object
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
					userIsPM = ((List<EntityRecord>)record["$user_1_n_project"]).Any(z => (Guid)z["id"] == user.Id);
					userIsStaff = ((List<EntityRecord>)record["$role_n_n_project_staff"]).Any(z => (Guid)z["id"] == userRole.Id);
					userIsCustomer = ((List<EntityRecord>)record["$role_n_n_project_customer"]).Any(z => (Guid)z["id"] == userRole.Id);
				}
				#endregion
				if (userIsPM || userIsStaff || userIsCustomer)
				{
					var recordObj = new EntityRecord();
					recordObj["id"] = record["id"];
					foreach (var column in list.Columns)
					{
						if (!column.DataName.Contains("$") && column.DataName != "id")
						{
							recordObj[column.DataName] = record[column.DataName];
						}
					}
					resultRecordsList.Add(recordObj);
				}
			}
			#endregion
			var skipRecords = list.PageSize*(page-1);
			if(page != 0) {
				resultRecordsList = resultRecordsList.Skip(skipRecords).Take(page).ToList();			
			}
			
			return Json(resultRecordsList);
		}
	}
}
