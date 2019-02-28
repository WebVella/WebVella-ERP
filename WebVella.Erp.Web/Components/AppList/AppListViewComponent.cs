using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Components
{

	public class AppListViewComponent : ViewComponent
	{
		private ErpUser currentUser = null;
		public ErpUser CurrentUser
		{
			get
			{
				if (currentUser == null)
					currentUser = AuthService.GetUser(HttpContext.User);

				return currentUser;
			}
		}

		public async Task<IViewComponentResult> InvokeAsync(List<App> apps)
		{
			List<Guid> currentUserRoles = new List<Guid>();
			if (CurrentUser != null)
				currentUserRoles.AddRange(CurrentUser.Roles.Select(x => x.Id));

			ViewBag.AppList = new List<App>();
			if (apps != null)
			{
				foreach (var app in apps)
				{
					if (app.Access == null || app.Access.Count == 0)
						continue;

					IEnumerable<Guid> accessRoles = app.Access.Intersect(currentUserRoles);
					if(accessRoles.Any())
						ViewBag.AppList.Add(app);
				}
			}

			return await Task.FromResult<IViewComponentResult>(View("AppList.Default"));
		}
	}
}
