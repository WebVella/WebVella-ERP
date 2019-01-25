using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Project.Services;

namespace WebVella.Erp.Plugins.Project.Hooks.Api
{
	[HookAttachment("comment")]
	public class Comment : IErpPreCreateRecordHook
	{

		public void OnPreCreateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			new CommentService().PreCreateApiHookLogic(entityName, record, errors);
		}

	}
}
