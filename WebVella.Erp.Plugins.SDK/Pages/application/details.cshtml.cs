using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.SDK.Pages.Application
{
	public class DetailsModel : BaseErpPageModel
	{
		public DetailsModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public App App { get; set; }

		public string Name { get; set; } = "";

		public string Label { get; set; } = "";

		public string Description { get; set; } = "";

		public string IconClass { get; set; } = "";

		public string Author { get; set; } = "";

		public string Color { get; set; } = "";

		public int Weight { get; set; } = 10;

		public List<string> Access { get; set; } = new List<string>();

		public List<SelectOption> RoleOptions { get; set; } = new List<SelectOption>();

		public List<string> LocalNav { get; set; } = new List<string>();

		public void PageInit() 
		{
			#region << Init App >>
			var appServ = new AppService();
			App = appServ.GetApplication(RecordId ?? Guid.Empty);
			if (App != null)
			{
				Name = App.Name;
				Label = App.Label;
				Description = App.Description;
				IconClass = App.IconClass;
				Author = App.Author;
				Color = App.Color;
				Weight = App.Weight;
				if (App.Access != null && App.Access.Count > 0)
				{
					Access = App.Access.Select(x => x.ToString()).ToList();
				}
			}
			#endregion

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/objects/application/l/list";

			#region << Init User Role Options >>
			var roles = new SecurityManager().GetAllRoles().OrderBy(x => x.Name).ToList();
			foreach (var role in roles)
			{
				RoleOptions.Add(new SelectOption()
				{
					Value = role.Id.ToString(),
					Label = role.Name
				});
			}
			#endregion
			LocalNav = AdminPageUtils.GetAppAdminSubNav(App, "details");
		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			PageInit();
			if (App == null)
				return NotFound();

			ErpRequestContext.PageContext = PageContext;
			BeforeRender();
			return Page();
		}

		public IActionResult OnPost()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			PageInit();
			if (App == null)
				return NotFound();

			var appServ = new AppService();
			try
			{
				appServ.DeleteApplication(App.Id);
				if (!String.IsNullOrWhiteSpace(ReturnUrl))
				{
					return Redirect(ReturnUrl);
				}
				else
				{
					return Redirect($"/sdk/objects/application/l/list");
				}
			}
			catch (ValidationException ex)
			{
				Validation.Message = ex.Message;
				Validation.Errors = ex.Errors;
			}


			ErpRequestContext.PageContext = PageContext;
			BeforeRender();
			return Page();
		}

	}
}