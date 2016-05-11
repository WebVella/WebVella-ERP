using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.WebHooks;

namespace WebVella.ERP.Project
{

    public class WebHooks
    {

		RecordManager recMan;
		EntityManager entityManager;
		EntityRelationManager entityRelationManager;
		SecurityManager secMan;

		public WebHooks()
		{
			recMan = new RecordManager();
			secMan = new SecurityManager();
			entityManager = new EntityManager();
			entityRelationManager = new EntityRelationManager();
		}

		#region << Task >>

		#region << Create >>

		//[WebHook("create_record_validation_errors", "wv_task")] //<<<< UNCOMMENT TO HOOK
		public dynamic TaskCreateValidateFilter(dynamic data) {
			var errors = (List<ErrorModel>)data.errors;
			var record = (EntityRecord)data.record;
			var recordId = (Guid)data.recordId;
			var newErrorsList = Validators.ValidateTask(errors,record,recordId);
			data.errors = newErrorsList;
			return data;		
		}

		[WebHook("create_record_pre_save", "wv_task")]
		public dynamic TaskCreateRecordPreSave(dynamic data)
		{
			EntityRecord record = (EntityRecord)data.record;
			record["project_id"] = new Guid((string)record["$project_1_n_task.id"]);
			record.Properties.Remove("$project_1_n_task.id"); 

			#region << Get project owner and set as ticket owner >>
			EntityRecord projectObject = null;
			{
				var filterObj = EntityQuery.QueryEQ("id", (Guid)record["project_id"]);
				var query = new EntityQuery("wv_project", "*", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (!result.Success)
				{
					
					throw new Exception("Error getting the project: " + result.Message);
				}
				else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
				{
					throw new Exception("Project not found");
				}
				projectObject = result.Object.Data[0];
				record["owner_id"] = (Guid)result.Object.Data[0]["owner_id"];
			}
			#endregion

			#region << Increase the project counter >>
			var patchObject = new EntityRecord();
			patchObject["id"] = (Guid)projectObject["id"];
			switch ((string)record["status"])
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
			var updateResponse = recMan.UpdateRecord("wv_project", patchObject);
			if (!updateResponse.Success)
			{
				throw new Exception(updateResponse.Message);
			}

			#endregion

			return data;
		}

		#endregion

		#region << Update >>

		//[WebHook("update_record_validation_errors", "wv_task")] //<<<< UNCOMMENT TO HOOK
		public dynamic TaskUpdateValidateFilter(dynamic data) {
			var errors = (List<ErrorModel>)data.errors;
			var record = (EntityRecord)data.record;
			var recordId = (Guid)data.recordId;
			var newErrorsList = Validators.ValidateTask(errors,record,recordId);
			data.errors = newErrorsList;
			return data;		
		}

		#endregion

		#region << Patch >>
		
		//[WebHook("patch_record_validation_errors", "wv_task")] //<<<< UNCOMMENT TO HOOK
		public dynamic TaskPatchValidateFilter(dynamic data) {
			var errors = (List<ErrorModel>)data.errors;
			var record = (EntityRecord)data.record;
			var recordId = (Guid)data.recordId;
			var newErrorsList = Validators.ValidateTask(errors,record,recordId);
			data.errors = newErrorsList;
			return data;
		}

		#endregion


		#endregion

    }
}
