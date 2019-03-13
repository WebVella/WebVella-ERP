using Microsoft.AspNetCore.Mvc;
using System;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Exceptions;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using System.Linq;
using WebVella.Erp.Api;

namespace WebVella.Erp.Plugins.SDK.Pages.User
{
	public class CreateModel : BaseErpPageModel
	{
		public CreateModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		[BindProperty]
		public string UserName { get; set; } = "";

		[BindProperty]
		public string Email { get; set; } = "";

		[BindProperty]
		public string Password { get; set; } = "";

		[BindProperty]
		public string Image { get; set; } = "";

		[BindProperty]
		public string FirstName { get; set; } = "";

		[BindProperty]
		public string LastName { get; set; } = "";

		[BindProperty]
		public bool Enabled { get; set; } = true;

		[BindProperty]
		public bool Verified { get; set; } = true;

		[BindProperty]
		public List<string> Roles { get; set; } = new List<string>() { "f16ec6db-626d-4c27-8de0-3e7ce542c55f" }; //regular by default

		public List<SelectOption> RoleOptions { get; set; } = new List<SelectOption>();

		private void InitPage()
		{
			var roles = new EqlCommand("select * from role").Execute();
			foreach (var role in roles)
			{
				if ((string)role["name"] != "guest")
					RoleOptions.Add(new SelectOption() { Value = role["id"].ToString(), Label = role["name"].ToString() });
			}

			RoleOptions = RoleOptions.OrderBy(x => x.Label).ToList();

			if (string.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/access/user/l/list";

		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitPage();
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
				var secMan = new SecurityManager();
				var allRoles = secMan.GetAllRoles();
				ErpUser newUser = new ErpUser();
				newUser.Id = Guid.NewGuid();
				newUser.Username = UserName;
				newUser.FirstName = FirstName;
				newUser.LastName = LastName;
				newUser.Email = Email;
				newUser.Password = Password;
				newUser.Image = Image;
				newUser.Enabled = Enabled;
				newUser.Verified = Verified;
				newUser.Preferences = new ErpUserPreferences();
				foreach (var roleId in Roles)
				{
					var role = allRoles.Single(x => x.Id == new Guid(roleId));
					newUser.Roles.Add(role);
				}
				secMan.SaveUser(newUser);
				BeforeRender();
				return Redirect(ReturnUrl);
			}
			catch (ValidationException ex)
			{
				Validation.Message = ex.Message;
				Validation.Errors = ex.Errors;
				BeforeRender();
				return Page();
			}
			catch (Exception ex)
			{
				Validation.Message = ex.Message;
				Validation.Errors.Add(new ValidationError("", ex.Message, isSystem: true));
				BeforeRender();
				return Page();
			}
		}
	}
}