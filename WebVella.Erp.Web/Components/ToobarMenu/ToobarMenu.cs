using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using Newtonsoft.Json;

namespace WebVella.Erp.Web.Components
{

	public class ToobarMenu : ViewComponent
    {
		protected ErpRequestContext ErpRequestContext { get; set; }

		public ToobarMenu([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public async Task<IViewComponentResult> InvokeAsync(BaseErpPageModel pageModel)
        {
			ViewBag.ToolbarMenu = pageModel.ToolbarMenu;
			return await Task.FromResult<IViewComponentResult>(View("ToobarMenu"));
        }
    }
}
