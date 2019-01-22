using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Components
{
	public class ScreenMessageViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
			// because of https://github.com/aspnet/Mvc/issues/6711 use upper line of code instead of
			ViewBag.ScreenMessage = TempData.Get<ScreenMessage>("ScreenMessage");

			return await Task.FromResult<IViewComponentResult>(View("Default"));
        }
    }
}
