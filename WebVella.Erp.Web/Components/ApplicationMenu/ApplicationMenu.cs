using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Components
{

	public class ApplicationMenu : ViewComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public ApplicationMenu([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public async Task<IViewComponentResult> InvokeAsync(BaseErpPageModel pageModel)
		{
			ViewBag.ApplicationMenu = pageModel.ApplicationMenu;
			return await Task.FromResult<IViewComponentResult>(View("ApplicationMenu"));
		}
	}
}
