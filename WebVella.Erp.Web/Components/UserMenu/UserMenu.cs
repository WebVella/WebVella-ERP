using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Components
{

	public class UserMenu : ViewComponent
    {
		protected ErpRequestContext ErpRequestContext { get; set; }

		public UserMenu([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public async Task<IViewComponentResult> InvokeAsync(BaseErpPageModel pageModel)
        {
			ViewBag.UserMenu = pageModel.UserMenu;
			return await Task.FromResult<IViewComponentResult>(View("UserMenu"));

		}
    }
}
