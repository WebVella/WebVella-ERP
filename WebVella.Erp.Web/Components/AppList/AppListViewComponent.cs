using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Components
{

	public class AppListViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync( List<App> apps)
        {
			//TODO: Apply app previligies based on the current user role

			ViewBag.AppList = apps; 

			return await Task.FromResult<IViewComponentResult>(View("AppList.Default"));
        }
    }
}
