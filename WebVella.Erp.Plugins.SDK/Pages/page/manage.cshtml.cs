using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.SDK.Pages.Page
{
	public class ManageModel : BaseErpPageModel
	{
		public ManageModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public ErpPage ErpPage { get; set; }

		[BindProperty]
		public int Weight { get; set; } = 10;

		[BindProperty]
		public string Label { get; set; } = "";

		//[BindProperty]
		public List<TranslationResource> LabelTranslations { get; set; } = new List<TranslationResource>();

		[BindProperty]
		public string Name { get; set; } = "";

		[BindProperty]
		public string IconClass { get; set; } = "";

		[BindProperty]
		public string Description { get; set; } = "";

		[BindProperty]
		public string Color { get; set; } = "";

		[BindProperty]
		public Guid? EntityId { get; set; } = null;

		[BindProperty]
		public Guid? AppId { get; set; } = null;

		[BindProperty]
		public Guid? AreaId { get; set; } = null;

		[BindProperty]
		public Guid? NodeId { get; set; } = null;

		[BindProperty]
		public PageType Type { get; set; } = PageType.Application;

		public List<PageBodyNode> Body { get; set; } = null;

		[BindProperty]
		public string Layout { get; set; } = "";

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		private void InitPage()
		{
			#region << Init Page >>
			var pageServ = new PageService();
			ErpPage = pageServ.GetPage(RecordId ?? Guid.Empty);
			if (ErpPage != null && PageContext.HttpContext.Request.Method == "GET")
			{
				Weight = ErpPage.Weight;
				Label = ErpPage.Label;
				LabelTranslations = ErpPage.LabelTranslations;
				Name = ErpPage.Name;
				IconClass = ErpPage.IconClass;
				Body = ErpPage.Body;
				EntityId = ErpPage.EntityId;
				AppId = ErpPage.AppId;
				AreaId = ErpPage.AreaId;
				NodeId = ErpPage.NodeId;
				Type = ErpPage.Type;
				Layout = ErpPage.Layout;
			}
			if (String.IsNullOrWhiteSpace(ReturnUrl))
			{
				ReturnUrl = $"/sdk/objects/page/r/{ErpPage.Id}/";
			}

			#endregion

			HeaderToolbar.AddRange(AdminPageUtils.GetPageAdminSubNav(ErpPage, "manage"));
		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			InitPage();

			if (ErpPage == null)
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

			if (ErpPage == null)
			{
				return NotFound();
			}

			var pageServ = new PageService();
			try
			{
				pageServ.UpdatePage(ErpPage.Id, Name, Label, LabelTranslations, IconClass, ErpPage.System, Weight, Type,
					AppId, EntityId, NodeId, AreaId, ErpPage.IsRazorBody, ErpPage.RazorBody, Layout);

				if (!String.IsNullOrWhiteSpace(ReturnUrl))
				{
					return Redirect(ReturnUrl);
				}
				else
				{
					return Redirect($"/sdk/objects/page/r/{ErpPage.Id}/");
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