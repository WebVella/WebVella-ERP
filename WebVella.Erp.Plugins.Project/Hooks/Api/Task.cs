using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Project.Services;

namespace WebVella.Erp.Plugins.Project.Hooks.Api
{
	[HookAttachment("task")]
	public class Task : IErpPostCreateRecordHook, IErpPostUpdateRecordHook, IErpPreCreateRecordHook, IErpPreUpdateRecordHook
	{
		public void OnPreCreateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			new TaskService().PreCreateRecordPageHookLogic(entityName, record, errors);
		}

		public void OnPostCreateRecord(string entityName, EntityRecord record)
		{
			new TaskService().PostCreateApiHookLogic(entityName, record);
		}

		public void OnPreUpdateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			new TaskService().PostPreUpdateApiHookLogic(entityName, record, errors);
		}

		public void OnPostUpdateRecord(string entityName, EntityRecord record)
		{
			new TaskService().PostUpdateApiHookLogic(entityName, record);
		}
	}
}
