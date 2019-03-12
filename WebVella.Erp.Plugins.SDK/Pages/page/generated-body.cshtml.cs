using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.Page
{
	public class GeneratedBodyModel : BaseErpPageModel
	{
		public GeneratedBodyModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public ErpPage ErpPage { get; set; }

		public string LibraryJson { get; set; }

		public string PageNodeListJson { get; set; }

		public string ApiUrlRoot { get; set; } = "";

		public string PagePublicUrl { get; set; } = "";

		[BindProperty(SupportsGet = true, Name = "recId")]
		public Guid? SimulationRecordId { get; set; }

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void PageInit()
		{
			ApiUrlRoot = PageContext.HttpContext.Request.Scheme + "://" + PageContext.HttpContext.Request.Host;

			#region << Init Page >>
			var pageServ = new PageService();
			ErpPage = pageServ.GetPage(RecordId ?? Guid.Empty);

			#endregion

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/objects/page/l/list";

			HeaderToolbar.AddRange(AdminPageUtils.GetPageAdminSubNav(ErpPage, "generated-body"));
			if(ErpPage != null)
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


			var componentLibrary = new PageComponentLibraryService().GetPageComponentsList();

			foreach (var component in componentLibrary)
			{
				if (component.Description.Length >= 120)
				{
					component.Description = component.Description.Substring(0, 120) + "...";
				}
				//Apply component Usage
				var userUsage = CurrentUser.Preferences.ComponentUsage.FirstOrDefault(x => x.Name == component.Name);
				if (userUsage != null) {
					component.LastUsedOn = userUsage.SdkUsedOn;
					component.UsageCounter = userUsage.SdkUsed;
				}
			}

			LibraryJson = JsonConvert.SerializeObject(componentLibrary);
			var pageNodes = new PageService().GetPageNodes(ErpPage.Id);
			PageNodeListJson = JsonConvert.SerializeObject(pageNodes);

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}
	}
}