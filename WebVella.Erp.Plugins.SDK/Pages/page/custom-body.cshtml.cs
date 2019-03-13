using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.Page
{
	public class CustomBodyModel : BaseErpPageModel
	{
		public CustomBodyModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public ErpPage ErpPage { get; set; }

		public bool IsRazorBody { get; set; } = false;

		public string RazorBody { get; set; } = "";

		public string PagePublicUrl { get; set; } = "";

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void PageInit()
		{
			#region << Init Page >>
			var pageServ = new PageService();
			ErpPage = pageServ.GetPage(RecordId ?? Guid.Empty);
			if (ErpPage != null)
			{
				IsRazorBody = ErpPage.IsRazorBody;
				RazorBody = ErpPage.RazorBody;
			}
			#endregion

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/objects/page/l/list";

			HeaderToolbar.AddRange(AdminPageUtils.GetPageAdminSubNav(ErpPage, "custom-body"));

			PagePublicUrl = PageUtils.CalculatePageUrl(ErpPage.Id);

		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			PageInit();
			if (ErpPage == null)
				return NotFound();

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}



	}
}