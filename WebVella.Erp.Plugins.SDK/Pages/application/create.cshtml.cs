using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.SDK.Pages.Application
{
	public class CreateModel : BaseErpPageModel
	{
		public CreateModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

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
		public string Color { get; set; } = "#2196F3";

		[BindProperty]
		public int Weight { get; set; } = 10;

		[BindProperty]
		public List<string> Access { get; set; } = new List<string>();

		public List<SelectOption> RoleOptions { get; set; } = new List<SelectOption>();

		public List<string> HeaderActions { get; private set; } = new List<string>();

		private void InitPage()
		{
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
			HeaderActions.AddRange( new List<string>() {
				$"<button type='submit' form='CreateRecord' onclick='return confirm('Are you sure ?')' class='btn btn-green btn-sm'><span class='fa fa-save go-white'></span> Create App</button>",
				$"<a href='{ReturnUrl}' class='btn btn-white btn-sm'>Cancel</a>"
			});

			#endregion
		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitPage();

			ErpRequestContext.PageContext = PageContext;
            //Add Admin as the default roles
            Access.Add(SystemIds.AdministratorRoleId.ToString());


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

			var appServ = new AppService();
			try
			{
				var appId = Guid.NewGuid();
				appServ.CreateApplication(appId, Name, Label, Description, IconClass, Author, Color, Weight, Access.Select(x => Guid.Parse(x)).ToList());
				return Redirect($"/sdk/objects/application/r/{appId}/");
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