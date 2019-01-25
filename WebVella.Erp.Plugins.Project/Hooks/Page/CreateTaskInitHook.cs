using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Plugins.Project.Hooks.Page
{
	[HookAttachment]
	public class CreateTaskInitHook : IPageHook
	{
		private Guid CREATE_TASK_PAGE_ID = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");

		public IActionResult OnGet(BaseErpPageModel pageModel)
		{
			var page = pageModel.ErpRequestContext.Page;

			if (page != null && page.Id == CREATE_TASK_PAGE_ID)
			{
				EntityRecord record = (EntityRecord)pageModel.DataModel.GetProperty("Record");

				#region << Init fields >>
				if (record == null)
					record = new EntityRecord();
				
				//Preselect owner
				ErpUser currentUser = (ErpUser)pageModel.DataModel.GetProperty("CurrentUser");
				if(currentUser != null)
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
						var projectRecord = new EntityRecord();
						projectRecord["id"] = outGuid;
						var recordsList = new List<EntityRecord>() { projectRecord };
						record["$project_nn_task"] = recordsList;
					}
				}
				else {
					var eqlCommand = "SELECT created_on,type_id,$project_nn_task.id FROM task WHERE created_by = @currentUserId ORDER BY created_on PAGE 1 PAGESIZE 1";
					var eqlParams = new List<EqlParameter>() { new EqlParameter("currentUserId", currentUser.Id) };
					var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();
					if (eqlResult != null && eqlResult is EntityRecordList && eqlResult.Count > 0) {
						var relatedProjects = (List<EntityRecord>)eqlResult[0]["$project_nn_task"];
						if (relatedProjects.Count > 0)
						{
							var projectIdList = new List<Guid>();
							projectIdList.Add((Guid)relatedProjects[0]["id"]);
							record["$project_nn_task.id"] = projectIdList;

							var projectRecord = new EntityRecord();
							projectRecord["id"] = (Guid)relatedProjects[0]["id"];
							var recordsList = new List<EntityRecord>() { projectRecord };
							record["$project_nn_task"] = recordsList;
						}
						record["type_id"] = (Guid?)eqlResult[0]["type_id"];
					}
				}

				//Preset start date
				record["start_date"] = DateTime.Now;

				#endregion

				pageModel.DataModel.SetRecord(record);
			}
			return null;
		}

		public IActionResult OnPost(BaseErpPageModel pageModel)
		{
			return null;
		}

	}

}
