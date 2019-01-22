using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.Application
{
	public class EntitiesModel : BaseErpPageModel
	{
		public EntitiesModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public App App { get; set; }

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void PageInit() {
			Init();

			#region << Init App >>
			var appServ = new AppService();
			App = appServ.GetApplication(RecordId ?? Guid.Empty);
			#endregion

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/objects/application/l/list";



		}

		public IActionResult OnGet()
		{
			PageInit();
			if (App == null)
				return NotFound();

			ErpRequestContext.PageContext = PageContext;
			return Page();
		}

		

	}
}