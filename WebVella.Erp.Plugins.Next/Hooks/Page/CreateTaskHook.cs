using Microsoft.AspNetCore.Mvc;
using System;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Plugins.SDK.Hooks.Page
{
	[HookAttachment]
	public class CreateTaskHook : IPageHook
	{
		private Guid CREATE_TASK_PAGE_ID = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");

		public IActionResult OnGet(BaseErpPageModel pageModel)
		{
			var page = (ErpPage)pageModel.DataModel.GetProperty("Page");

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
				
				//Preselect project
				if (pageModel.HttpContext.Request.Query.ContainsKey("projectId")) {
					var projectQueryId = pageModel.HttpContext.Request.Query["projectId"].ToString();
					if (Guid.TryParse(projectQueryId, out Guid outGuid)) {
						record["project_id"] = outGuid;
					}
				}
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
