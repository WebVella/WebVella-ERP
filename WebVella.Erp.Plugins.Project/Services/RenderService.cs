using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.Project.Services
{
	public class RenderService : BaseService
	{
		public void UserMenuItemsManagement(BaseErpPageModel pageModel) {
			if (pageModel.AppName == "projects")
			{
				var createTask = new MenuItem()
				{
					IsHtml = true,
					Content = "<div class='menu-nav-wrapper'><div class='menu-nav'><a href='/projects/tasks/tasks/c/create'><i class='fa fa-plus'></i> Create Task</a></div></div>",
					isDropdownRight = false,
					RenderWrapper = false,
					SortOrder = 1
				};
				pageModel.AddUserMenu(createTask);
			}
		}
	}
}
