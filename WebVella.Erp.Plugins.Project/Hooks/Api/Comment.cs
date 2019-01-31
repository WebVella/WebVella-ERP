using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Project.Services;

namespace WebVella.Erp.Plugins.Project.Hooks.Api
{
	[HookAttachment("comment")]
	public class Comment : IErpPreCreateRecordHook, IErpPostCreateRecordHook
	{

		public void OnPreCreateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			new CommentService().PreCreateApiHookLogic(entityName, record, errors);
		}

		public void OnPostCreateRecord(string entityName, EntityRecord record)
		{
			new CommentService().PostCreateApiHookLogic(entityName, record);
		}

	}
}
