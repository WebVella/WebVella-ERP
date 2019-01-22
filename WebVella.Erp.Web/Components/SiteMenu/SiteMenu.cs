using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Components
{

	public class SiteMenu : ViewComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public SiteMenu([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public async Task<IViewComponentResult> InvokeAsync(BaseErpPageModel pageModel)
		{
			ViewBag.SiteMenu = pageModel.SiteMenu;
			return await Task.FromResult<IViewComponentResult>(View("SiteMenu"));
		}
	}
}
