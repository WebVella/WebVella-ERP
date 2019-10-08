using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.Application
{
	public class PagesModel : BaseErpPageModel
	{
		public PagesModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public App App { get; set; }

		public List<EntityRecord> Records { get; set; }

		public List<WvGridColumnMeta> Columns { get; set; }

		public int PagerSize { get; set; } = 0; //All

		public int Pager { get; set; } = 1;

		public int TotalCount { get; set; } = 0;

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void PageInit()
		{
			#region << Init App >>
			var appServ = new AppService();
			App = appServ.GetApplication(RecordId ?? Guid.Empty);
			#endregion

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/objects/application/l/list";

			#region << Actions >>
			HeaderActions.Add($"<a href='/sdk/objects/page/c/create?returnUrl={HttpUtility.UrlEncode(CurrentUrl)}&Type=Application&AppId={(App != null ? App.Id.ToString() : "")}' class='btn btn-white btn-sm'><i class='fa fa-plus go-green'></i> CreatePage</a>");

			#endregion

			#region << Actions >>
			HeaderToolbar.AddRange(AdminPageUtils.GetAppAdminSubNav(App, "pages"));
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

			ErpRequestContext.PageContext = PageContext;
			var appSrv = new AppService();
			Records = appSrv.GetApplication(App.Id).HomePages.OrderBy(x => x.Weight).ToList().MapTo<EntityRecord>();

			Columns = new List<WvGridColumnMeta>() {
				new WvGridColumnMeta(){
					Label = "",
					Name = "action",
					Width = "50px"
				},
				new WvGridColumnMeta(){
					Label = "weight",
					Name = "weight",
					Width = "80px"
				},
				new WvGridColumnMeta(){
					Label = "name",
					Name = "name",
					Width = ""
				},
			};
			BeforeRender();
			return Page();
		}
	}
}