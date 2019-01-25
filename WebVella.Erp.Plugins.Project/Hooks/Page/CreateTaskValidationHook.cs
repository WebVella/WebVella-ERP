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
	[HookAttachment("task")]
	public class CreateTaskValidationHook : IErpPreCreateRecordHook
	{
		public void OnPreCreateRecord(string entityName, EntityRecord record, List<ErrorModel> errors) {
			var projectIdSubmitted = true;
			if (!record.Properties.ContainsKey("$project_nn_task"))
			{
				projectIdSubmitted = false;
			}
			else {
				var projectRecord = (EntityRecord)record["$project_nn_task"];
				if (!projectRecord.Properties.ContainsKey("id"))
				{
					projectIdSubmitted = false;
				}
				else {
					if (projectRecord["id"] == null && !(projectRecord["id"] is Guid)) {
						projectIdSubmitted = false;
					}
				}
			}

			if (!projectIdSubmitted) {
				errors.Add(new ErrorModel() { 
					Key = "$project_nn_task.id",
					Message = "Project is required"
				});
			}
		}
	}

}
