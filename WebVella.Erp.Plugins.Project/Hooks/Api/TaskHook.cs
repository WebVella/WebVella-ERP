using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Project.Services;

namespace WebVella.Erp.Plugins.Project.Hooks.Api
{
	[HookAttachment("task")]
	public class TaskHook : IErpPreCreateRecordHook, IErpPreUpdateRecordHook, IErpPreDeleteRecordHook,
							IErpPostCreateRecordHook, IErpPostUpdateRecordHook, IErpPostDeleteRecordHook
	{

		public void OnPreCreateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
		}

		public void OnPreUpdateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{

		}

		public void OnPreDeleteRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{

		}

		public void OnPostCreateRecord(string entityName, EntityRecord record)
		{
			//Update key and search fields
			Guid projectId = Guid.Empty;
			string taskSubject = "";
			var patchRecord = new TaskService().SetCalculationFields((Guid)record["id"], subject: out taskSubject, projectId: out projectId);
			var updateResponse = new RecordManager(executeHooks: false).UpdateRecord("task", patchRecord);
			if (!updateResponse.Success)
				throw new Exception(updateResponse.Message);


			//Add activity log
			var subject = $"created <a href=\"/projects/tasks/tasks/r/{patchRecord["id"]}/details\">[{patchRecord["key"]}] {taskSubject}</a>";
			var relatedRecords = new List<string>() { patchRecord["id"].ToString(), projectId.ToString() };
			var scope = new List<string>() { "projects" };
			new FeedItemService().Create(id: Guid.NewGuid(), createdBy: SecurityContext.CurrentUser.Id, subject: subject,
				relatedRecords: relatedRecords, scope: scope, type: "task");
		}

		public void OnPostUpdateRecord(string entityName, EntityRecord record)
		{
			//Update key and search fields
			Guid projectId = Guid.Empty;
			string taskSubject = "";
			var patchRecord = new TaskService().SetCalculationFields((Guid)record["id"], subject: out taskSubject, projectId: out projectId);
			var updateResponse = new RecordManager(executeHooks: false).UpdateRecord("task", patchRecord);
			if (!updateResponse.Success)
				throw new Exception(updateResponse.Message);
		}

		public void OnPostDeleteRecord(string entityName, EntityRecord record)
		{

		}

	}
}
