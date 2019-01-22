using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Components
{

	public class SearchNavViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync( )
        {
			return await Task.FromResult<IViewComponentResult>(View("SearchNav.Default"));
        }
    }
}
