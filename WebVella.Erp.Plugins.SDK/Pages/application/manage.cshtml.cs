using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.SDK.Pages.Application
{
	public class ManageModel : BaseErpPageModel
	{
		public ManageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public App App { get; set; }

		[BindProperty]
		public string Name { get; set; } = "";

		[BindProperty]
		public string Label { get; set; } = "";

		[BindProperty]
		public string Description { get; set; } = "";

		[BindProperty]
		public string IconClass { get; set; } = "";

		[BindProperty]
		public string Author { get; set; } = "";

		[BindProperty]
		public string Color { get; set; } = "";

		[BindProperty]
		public int Weight { get; set; } = 10;

		[BindProperty]
		public List<string> Access { get; set; } = new List<string>();

		public List<SelectOption> RoleOptions { get; set; } = new List<SelectOption>();

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		private void InitPage()
		{
			#region << Init App >>
			var appServ = new AppService();
			App = appServ.GetApplication(RecordId ?? Guid.Empty);
			if (App != null && PageContext.HttpContext.Request.Method == "GET")
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

			#region << Init Roles >>
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

			#region << Actions >>
			HeaderActions.AddRange(new List<string>() {
				$"<button type='submit' form='ManageRecord' class='btn btn-blue btn-sm'><span class='fa fa-save go-white'></span> Save App</button>",
				$"<a href='{ReturnUrl}' class='btn btn-white btn-sm'>Cancel</a>"
			});

			HeaderToolbar.AddRange(AdminPageUtils.GetAppAdminSubNav(App, "manage"));

			#endregion

		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitPage();

			if (App == null)
			{
				return NotFound();
			}

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}

		public IActionResult OnPost()
		{
			if (!ModelState.IsValid) throw new Exception("Antiforgery check failed.");

			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitPage();

			if (App == null)
			{
				return NotFound();
			}

			var appServ = new AppService();
			try
			{
				appServ.UpdateApplication(App.Id, Name, Label, Description, IconClass, Author, Color, Weight, Access.Select(x => Guid.Parse(x)).ToList());
				if (!String.IsNullOrWhiteSpace(ReturnUrl))
				{
					return Redirect(ReturnUrl);
				}
				else
				{
					return Redirect($"/sdk/objects/application/r/{App.Id}/");
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