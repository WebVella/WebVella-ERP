using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.SDK.Pages.User
{
	public class ManageModel : BaseErpPageModel
	{
		public ManageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

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
		public List<string> Roles { get; set; } = new List<string>(); //regular by default

		public List<SelectOption> RoleOptions { get; set; } = new List<SelectOption>();

		private EntityRecord UserRecord { get; set; } = null;

		private void InitPage()
		{
			var roles = new EqlCommand("select * from role").Execute();
			foreach (var role in roles)
			{
				if ((string)role["name"] != "guest")
					RoleOptions.Add(new SelectOption() { Value = role["id"].ToString(), Label = role["name"].ToString() });
			}

			RoleOptions = RoleOptions.OrderBy(x => x.Label).ToList();

			if (RecordId != null)
			{
				var resultRecords = new EqlCommand($"select *,$user_role.id from user where id = '{RecordId}'").Execute();
				if (resultRecords.Any())
					UserRecord = resultRecords.First();

			}

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/access/user/l/list";
		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitPage();

			if (UserRecord == null)
				return NotFound();

			UserName = (string)UserRecord["username"];
			Email = (string)UserRecord["email"];
			Image = (string)UserRecord["image"];
			FirstName = (string)UserRecord["first_name"];
			LastName = (string)UserRecord["last_name"];
			Enabled = (bool)UserRecord["enabled"];
			Verified = (bool)UserRecord["verified"];

			foreach (var role in (List<EntityRecord>)UserRecord["$user_role"])
			{
				Roles.Add(role["id"].ToString());
			}

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

			if (UserRecord == null)
				return NotFound();

			try
			{

				var secMan = new SecurityManager();
				var allRoles = secMan.GetAllRoles();

				ErpUser user = new ErpUser();
				user.Id = (Guid)UserRecord["id"];
				user.Username = UserName;
				user.FirstName = FirstName;
				user.LastName = LastName;
				user.Email = Email;
				user.Password = Password;
				user.Image = Image;
				user.Enabled = Enabled;
				user.Verified = Verified;
				user.Preferences = new ErpUserPreferences();
				foreach (var roleId in Roles)
				{
					var role = allRoles.Single(x => x.Id == new Guid(roleId));
					user.Roles.Add(role);
				}

				secMan.SaveUser(user);
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