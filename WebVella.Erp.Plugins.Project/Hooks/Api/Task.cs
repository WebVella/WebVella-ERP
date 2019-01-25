using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Project.Services;

namespace WebVella.Erp.Plugins.Project.Hooks.Api
{
	[HookAttachment("task")]
	public class Task : IErpPostCreateRecordHook, IErpPostUpdateRecordHook
	{

		public void OnPostCreateRecord(string entityName, EntityRecord record)
		{
			new TaskService().PostCreateApiHookLogic(entityName, record);
		}

		public void OnPostUpdateRecord(string entityName, EntityRecord record)
		{
			new TaskService().PostUpdateApiHookLogic(entityName, record);
		}

	}
}
