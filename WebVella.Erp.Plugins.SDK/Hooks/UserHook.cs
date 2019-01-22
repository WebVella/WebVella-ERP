using System;
using System.Collections.Generic;
using System.Diagnostics;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Hooks;

namespace WebVella.Erp.Plugins.SDK.Hooks
{
	[HookAttachment("user")]
	public class UserHook : IErpPreCreateRecordHook, IErpPreUpdateRecordHook,IErpPreDeleteRecordHook,	
							IErpPostCreateRecordHook,IErpPostUpdateRecordHook,IErpPostDeleteRecordHook
	{
		public void OnPreCreateRecord(string entityName, EntityRecord data, List<ErrorModel> errors)
		{
			Debug.WriteLine("Test pre-create executed");
		}

		public void OnPreUpdateRecord(string entityName, EntityRecord data, List<ErrorModel> errors)
		{
			Debug.WriteLine("Test pre-update executed");
		}

		public void OnPreDeleteRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			Debug.WriteLine("Test pre-delete executed");
		}

		public void OnPostCreateRecord(string entityName, EntityRecord record)
		{
			Debug.WriteLine("Test post-create executed");
		}

		public void OnPostUpdateRecord(string entityName, EntityRecord record)
		{
			Debug.WriteLine("Test post-update executed");
		}

		public void OnPostDeleteRecord(string entityName, EntityRecord record)
		{
			Debug.WriteLine("Test post-delete executed");
		}

	}
}
