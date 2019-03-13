using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.Application
{
	public class SitemapModel : BaseErpPageModel
	{
		public SitemapModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public App App { get; set; }

		public string ComponentInitData { get; set; }

		public string ApiUrlRoot { get; set; } = "";

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void PageInit()
		{
			ApiUrlRoot = PageContext.HttpContext.Request.Scheme + "://" + PageContext.HttpContext.Request.Host;

			#region << Init App >>
			var appServ = new AppService();
			App = appServ.GetApplication(RecordId ?? Guid.Empty);
			#endregion

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/objects/application/l/list";

			#region << Actions >>

			HeaderToolbar.AddRange(AdminPageUtils.GetAppAdminSubNav(App, "sitemap"));

			#endregion

		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			PageInit();
			if (App == null)
				return NotFound();

			var initData = new EntityRecord();
			initData["sitemap"] = new AppService().OrderSitemap(App.Sitemap);
			initData["node_page_dict"] = PageUtils.GetNodePageDictionary(App.Id);

			ComponentInitData = JsonConvert.SerializeObject(initData);

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}



	}
}