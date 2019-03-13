using Microsoft.AspNetCore.Mvc;
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
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.Page
{
	public class DetailsModel : BaseErpPageModel
	{
		public DetailsModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public ErpPage ErpPage { get; set; }

		[BindProperty]
		public int Weight { get; set; } = 10;

		[BindProperty]
		public string Label { get; set; } = "";

		//[BindProperty]
		public List<TranslationResource> LabelTranslations { get; set; } = new List<TranslationResource>();

		public string Name { get; set; } = "";

		public string IconClass { get; set; } = "";

		public string Description { get; set; } = "";

		public bool System { get; set; } = false;

		public PageType Type { get; set; } = PageType.Application;

		public Guid? AppId { get; set; } = null;

		public Guid? EntityId { get; set; } = null;

		public Guid? AreaId { get; set; } = null;

		public Guid? NodeId { get; set; } = null;

		public bool IsRazorBody { get; set; } = false;

		public string Layout { get; set; } = "";

		public List<PageBodyNode> Body { get; set; } = null;

		public string PagePublicUrl { get; set; } = "";

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public List<string> Access { get; set; } = new List<string>();

		public List<SelectOption> RoleOptions { get; set; } = new List<SelectOption>();

		public void PageInit()
		{
			#region << Init Page >>
			var pageServ = new PageService();
			ErpPage = pageServ.GetPage(RecordId ?? Guid.Empty);
			if (ErpPage != null)
			{
				Weight = ErpPage.Weight;
				Label = ErpPage.Label;
				LabelTranslations = ErpPage.LabelTranslations;
				Name = ErpPage.Name;
				IconClass = ErpPage.IconClass;
				System = ErpPage.System;
				Type = ErpPage.Type;
				AppId = ErpPage.AppId;
				EntityId = ErpPage.EntityId;
				AreaId = ErpPage.AreaId;
				NodeId = ErpPage.NodeId;
				IsRazorBody = ErpPage.IsRazorBody;
				Body = ErpPage.Body;
				Layout = ErpPage.Layout;
			}
			#endregion

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/objects/page/l/list";

			#region << Init App >>
			var appServ = new AppService();
			var app = appServ.GetApplication(ErpPage.AppId ?? Guid.Empty);
			if (app != null)
			{
				if (app.Access != null && app.Access.Count > 0)
				{
					Access = app.Access.Select(x => x.ToString()).ToList();
				}

			}
			#endregion

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

			HeaderToolbar.AddRange(AdminPageUtils.GetPageAdminSubNav(ErpPage, "details"));

			PagePublicUrl = PageUtils.CalculatePageUrl(ErpPage.Id);
		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			PageInit();
			if (ErpPage == null)
				return NotFound();

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}

		public IActionResult OnPost()
		{
			PageInit();
			if (ErpPage == null)
				return NotFound();

			if(!PageContext.HttpContext.Request.Query.ContainsKey("op"))
				return NotFound();

			var operation = PageContext.HttpContext.Request.Query["op"];
			var pageServ = new PageService();
			try
			{
				ErpPage pageCopy = null;

				if (operation == "delete" )
					pageServ.DeletePage(ErpPage.Id);
				else if (operation == "clone")
					pageCopy = pageServ.ClonePage(ErpPage.Id);
				else
					return NotFound();

				if (!String.IsNullOrWhiteSpace(ReturnUrl))
				{
					if (operation == "clone")
						return Redirect($"/sdk/objects/page/r/{pageCopy.Id}");
					else
						return Redirect(ReturnUrl);
				}
				else
				{
					if (operation == "clone")
						return Redirect($"/sdk/objects/page/r/{pageCopy.Id}");
					else
						return Redirect($"/sdk/objects/page/l/list");
				}
			}
			catch (ValidationException ex)
			{
				Validation.Message = ex.Message;
				Validation.Errors = ex.Errors;
			}

			BeforeRender();
			return Page();
		}

	}
}