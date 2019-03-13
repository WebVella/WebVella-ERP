using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
	public class PageDataModel : BaseErpPageModel
	{
		public PageDataModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public ErpPage ErpPage { get; set; }

		public List<ModelNode> PageModel { get; set; } = new List<ModelNode>();

		public string ApiUrlRoot { get; set; } = "";

		public string PagePublicUrl { get; set; } = "";

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void PageInit()
		{
			ApiUrlRoot = PageContext.HttpContext.Request.Scheme + "://" + PageContext.HttpContext.Request.Host;

			#region << Init Page >>
			var pageServ = new PageService();
			ErpPage = pageServ.GetPage(RecordId ?? Guid.Empty);
			#endregion

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/objects/page/l/list";

			HeaderToolbar.AddRange(AdminPageUtils.GetPageAdminSubNav(ErpPage, "model"));
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



			PageModel = PageUtils.GetPageSystemModelNodes(ErpPage);

			PageModel = PageModel.OrderBy(x => x.PageDataSourceName).ToList();

			BeforeRender();
			return Page();
		}


		public IActionResult OnPost()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			PageInit();
			if (ErpPage == null)
				return NotFound();

			ErpRequestContext.PageContext = PageContext;

			try
			{
				dynamic formData = ParsePostData();
				if (formData.IsDeleteAction)
				{
					new PageService().DeletePageDataSource(formData.PageDataSourceId);
				}
				else
				{
					Guid id = Guid.NewGuid();
					if (formData.PageDataSourceId == null)
					{
						new PageService().CreatePageDataSource(id, (Guid)formData.PageId, (Guid)formData.DataSourceId,
								 (string)formData.Name, (List<DataSourceParameter>)formData.Parameters);
					}
					else
					{
						id = formData.PageDataSourceId;
						new PageService().UpdatePageDataSource(formData.PageDataSourceId, (Guid)formData.PageId, (Guid)formData.DataSourceId,
									 (string)formData.Name, (List<DataSourceParameter>)formData.Parameters);
					}
				}
				return Redirect($"/sdk/objects/page/r/{ErpPage.Id}/model");
			}
			catch (ValidationException ex)
			{
				Validation.Message = ex.Message;
				Validation.Errors.AddRange(ex.Errors);
			}

			PageModel = PageUtils.GetPageSystemModelNodes(ErpPage);

			PageModel = PageModel.OrderBy(x => x.PageDataSourceName).ToList();

			BeforeRender();
			return Page();
		}

		private dynamic ParsePostData()
		{
			var formData = HttpContext.Request.Form;
			dynamic rec = new ExpandoObject();
			ValidationException validation = new ValidationException();
			DataSourceManager dsMan = new DataSourceManager();
			DataSourceBase dataSource = null;

			rec.IsDeleteAction = false;

			//handle delete
			if (formData.ContainsKey("action") && formData["action"] == "delete")
			{
				rec.IsDeleteAction = true;

				rec.PageDataSourceId = (Guid?)null;
				if (formData.ContainsKey("page_datasource_id") && !string.IsNullOrWhiteSpace(formData["page_datasource_id"]))
				{
					Guid pageDataSourceId;
					if (Guid.TryParse(formData["page_datasource_id"], out pageDataSourceId))
						rec.PageDataSourceId = pageDataSourceId;
					else
						validation.AddError("page_datasource_id", "Specified page data source id is not in valid GUID format.");
				}
				validation.CheckAndThrow();

				return rec;
			}

			//continue with create or update parse
			rec.PageDataSourceId = (Guid?)null;
			if (formData.ContainsKey("page_datasource_id") && !string.IsNullOrWhiteSpace(formData["page_datasource_id"]))
			{
				Guid pageDataSourceId;
				if (Guid.TryParse(formData["page_datasource_id"], out pageDataSourceId))
					rec.PageDataSourceId = pageDataSourceId;
				else
					validation.AddError("page_datasource_id", "Specified page data source id is not in valid GUID format.");
			}

			if (formData.ContainsKey("page_id") && !string.IsNullOrWhiteSpace(formData["page_id"]))
			{
				Guid pageId;
				if (Guid.TryParse(formData["page_id"], out pageId))
					rec.PageId = pageId;
				else
					validation.AddError("page_id", "Specified page id is not in valid GUID format.");
			}
			else
				validation.AddError("page_id", "Page id is not specified.");

			if (formData.ContainsKey("datasource_id") && !string.IsNullOrWhiteSpace(formData["datasource_id"]))
			{
				Guid datasourceId;
				if (Guid.TryParse(formData["datasource_id"], out datasourceId))
				{
					rec.DataSourceId = datasourceId;
					dataSource = dsMan.Get(datasourceId);
					if (dataSource == null)
						validation.AddError("datasource_id", "Specified datasource id is not found in database.");
				}
				else
					validation.AddError("datasource_id", "Specified datasource id is not in valid GUID format.");
			}
			else
				validation.AddError("datasource_id", "Datasource id is not specified.");

			if (formData.ContainsKey("page_datasource_name") && !string.IsNullOrWhiteSpace(formData["page_datasource_name"]))
				rec.Name = formData["page_datasource_name"];
			else
				validation.AddError("page_datasource_name", "Page datasource name is not specified.");

			validation.CheckAndThrow();

			List<DataSourceParameter> parameters = new List<DataSourceParameter>();
			foreach (var par in dataSource.Parameters)
			{
				var htmlKey = "@_" + par.Name;
				if (formData.Keys.Contains(htmlKey) && !string.IsNullOrWhiteSpace(formData[htmlKey]))
				{
					DataSourceParameter parameter = new DataSourceParameter();
					parameter.Name = par.Name;
					parameter.Type = par.Type;
					parameter.Value = formData[htmlKey];
					parameters.Add(parameter);
				}
			}
			rec.Parameters = parameters;

			return rec;
		}


	}
}