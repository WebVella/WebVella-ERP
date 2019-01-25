using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Project.Services;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Pages.Application;

namespace WebVella.Erp.Plugins.Project.Hooks.Page
{
	[HookAttachment]
	public class UserMenuHook : IPageHook
	{
		public IActionResult OnGet(BaseErpPageModel pageModel)
		{
			if (pageModel.AppName == "projects")
			{
				var createTask = new MenuItem()
				{
					IsHtml = true,
					Content = "<div class'menu-nav-wrapper'><div class='menu-nav'><a href='/projects/tasks/tasks/c/create'><i class='fa fa-plus'></i> Create Task</a></div></div>",
					isDropdownRight = false,
					RenderWrapper = false,
					SortOrder = 1
				};
				pageModel.AddUserMenu(createTask);
			}
			return null;
		}

		public IActionResult OnPost(BaseErpPageModel pageModel)
		{
			if (pageModel.AppName == "projects")
			{
				var createTask = new MenuItem()
				{
					IsHtml = true,
					Content = "<div class'menu-nav-wrapper'><div class='menu-nav'><a href='/projects/tasks/tasks/c/create'><i class='fa fa-plus'></i> Create Task</a></div></div>",
					isDropdownRight = false,
					RenderWrapper = false,
					SortOrder = 1
				};
				pageModel.AddUserMenu(createTask);
			}
			return null;
		}
	}

}
