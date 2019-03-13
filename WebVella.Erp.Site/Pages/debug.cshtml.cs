using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Pages
{
	[Authorize]
	public class DebugModel : BaseErpPageModel
	{
		public AppService appService = new AppService();

		public DebugModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }


		public IActionResult OnGet()
        {
			//var list = new PageComponentLibraryService().GetPageComponentsList();

			//ViewBag.LibraryJson = JsonConvert.SerializeObject(new PageComponentLibraryService().GetPageComponentsList());
			//ViewBag.PageNodeListJson = JsonConvert.SerializeObject(new PageService().GetPageNodes(new System.Guid("129937b1-7cbe-42a0-b699-e61bebd28619")));
			var initResult = Init();
			if (initResult != null)
				return initResult;

			return Page();
		}
    }
}

/*
 * system actions: none
 * custom actions: none
 */
