using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Project.Services;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Plugins.Project.Hooks.Page
{
	[HookAttachment(key: "TimeTrackCreateLog")]
	public class TimeTrackCreateLog : IPageHook
	{
		private Guid TIMETRACK_PAGE_ID = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");

		public IActionResult OnGet(BaseErpPageModel pageModel)
		{
			return null;
		}

		public IActionResult OnPost(BaseErpPageModel pageModel)
		{
			var page = (ErpPage)pageModel.DataModel.GetProperty("Page");

			var currentUser = SecurityContext.CurrentUser;
			if (page != null && page.Id == TIMETRACK_PAGE_ID && currentUser != null)
			{
				var postForm = pageModel.PageContext.HttpContext.Request.Form;
				if (String.IsNullOrWhiteSpace(postForm["task_id"]))
					throw new Exception("Task Id is required");
				if (String.IsNullOrWhiteSpace(postForm["minutes"]))
					throw new Exception("Minutes is required");
				if (String.IsNullOrWhiteSpace(postForm["logged_on"]))
					throw new Exception("Logged on is required");

				Guid taskId = Guid.Empty;
				DateTime loggedOn = DateTime.MinValue;
				string body = !String.IsNullOrWhiteSpace(postForm["body"]) ? postForm["body"].ToString() : "";
				bool isBillable = true;
				int minutes = 0;

				if (Guid.TryParse(postForm["task_id"].ToString(), out Guid outTaskId))
					taskId = outTaskId;
				if (DateTime.TryParse(postForm["logged_on"].ToString(), out DateTime outLoggedOn))
					loggedOn = outLoggedOn;
				if (Boolean.TryParse(postForm["is_billable"].ToString(), out Boolean outBillable))
					isBillable = outBillable;
				if (int.TryParse(postForm["minutes"].ToString(), out int outMinutes))
					minutes = outMinutes;

				var eqlCommand = " SELECT *,$project_nn_task.id from task WHERE id = @taskId";
				var eqlParams = new List<EqlParameter>() { new EqlParameter("taskId", taskId) };
				var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();
				if (!eqlResult.Any())
					throw new Exception("Task with taskId not found");

				var taskRecord = eqlResult[0];
				var scope = new List<string>() { "projects" };
				var relatedRecords = new List<Guid>() { (Guid)taskRecord["id"] };

				if (taskRecord.Properties.ContainsKey("$project_nn_task") && ((List<EntityRecord>)taskRecord["$project_nn_task"]).Any()) {
					var projectRecord = ((List<EntityRecord>)taskRecord["$project_nn_task"]).First();
					relatedRecords.Add((Guid)projectRecord["id"]);
				}

				using (var connection = DbContext.Current.CreateConnection()){
					try
					{
						connection.BeginTransaction();
						new TimeLogService().Create(null, currentUser.Id, DateTime.Now, loggedOn, minutes, isBillable, body, scope, relatedRecords);
						connection.CommitTransaction();
						return new RedirectResult("/projects/track-time/track-time/a/track-time");
					}
					catch (Exception ex)
					{
						connection.RollbackTransaction();
						throw ex;
					}
				}
				
			}
			return null;
		}
	}

}
