using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Next;
using WebVella.Erp.Plugins.Next.Services;

namespace WebVella.Erp.Plugins.Next.Hooks.Api
{
	[HookAttachment("account",int.MinValue)]
	public class AccountHook : IErpPostCreateRecordHook, IErpPostUpdateRecordHook
	{
		public void OnPostCreateRecord(string entityName, EntityRecord record)
		{
			new SearchService().RegenSearchField(entityName,record, Configuration.AccountSearchIndexFields);
		}

		public void OnPostUpdateRecord(string entityName, EntityRecord record)
		{
			new SearchService().RegenSearchField(entityName,record, Configuration.AccountSearchIndexFields);
		}
	}
}
