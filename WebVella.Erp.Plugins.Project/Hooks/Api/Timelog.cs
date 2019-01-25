using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Project.Services;

namespace WebVella.Erp.Plugins.Project.Hooks.Api
{
	[HookAttachment("timelog")]
	public class Timelog : IErpPreCreateRecordHook, IErpPreDeleteRecordHook
	{

		public void OnPreCreateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			new TimeLogService().PreCreateApiHookLogic(entityName, record, errors);
		}

		public void OnPreDeleteRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			new TimeLogService().PreDeleteApiHookLogic(entityName, record, errors);
		}

	}
}
