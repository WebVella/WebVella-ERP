using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.Project.Components
{

	[RenderHookAttachment("head-top", 10)]
	public class HookMetaHead : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(BaseErpPageModel pageModel)
        {
			ViewBag.LinkTags = new List<LinkTagInclude>();

			#region === <style> ===
			{
				var linkTagsToInclude = new List<LinkTagInclude>();

				//Your includes below >>>>

				#region << style.css >>
				{
					//Always include
					linkTagsToInclude.Add(new LinkTagInclude()
					{
						InlineContent = FileService.GetEmbeddedTextResource("styles.css", "WebVella.Erp.Plugins.Project.Theme", "WebVella.Erp.Plugins.Project")
				});
				}
				#endregion

				//<<<< Your includes up
				ViewBag.LinkTags = linkTagsToInclude;
			}
			#endregion

			return await Task.FromResult<IViewComponentResult>(View("Project_HookMetaHead"));
        }
    }
}
