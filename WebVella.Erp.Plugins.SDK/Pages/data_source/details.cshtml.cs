using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpDataSource
{
	public class DetailsModel : BaseErpPageModel
	{
		public DetailsModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		private DataSourceManager dsMan = new DataSourceManager();

		public DataSourceBase DataSourceObject { get; set; }

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public string Eql
		{
			get
			{
				var dbDS = DataSourceObject as DatabaseDataSource;
				if (dbDS != null)
					return dbDS.EqlText;

				//TODO process code

				return string.Empty;
			}
		}

		public string Sql
		{
			get
			{
				var dbDS = DataSourceObject as DatabaseDataSource;
				if (dbDS != null)
					return dbDS.SqlText;

				//TODO process code

				return string.Empty;
			}
		}
		
		public string FullClass
		{
			get
			{
				var dbDS = DataSourceObject as DatabaseDataSource;
				if (dbDS != null)
					return "";

				return "TODO GET VALUE FROM CodeDataSource";
			}
		}

		public void PageInit()
		{
			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = "/sdk/objects/data_source/l/list";

			DataSourceObject = dsMan.Get(RecordId ?? Guid.Empty);
			if (DataSourceObject == null)
				return;

			var returnUrlEncoded = HttpUtility.UrlEncode(PageUtils.GetCurrentUrl(PageContext.HttpContext));

			if (DataSourceObject is DatabaseDataSource)
			{
				var existingPageDataSources = new PageService().GetPageDataSourcesByDataSourceId(DataSourceObject.Id);
				if(existingPageDataSources.Count > 0 )
					HeaderActions.Add($"<button type='button' class='btn btn-white btn-sm disabled' tooltip='There are existing page data sources related'><i class='fa fa-lock'></i> Delete Locked</button>");
				else
					HeaderActions.Add($"<button type='submit' form='DeleteDataSourceForm' onclick='return confirm(\"Are you sure?\")' class='btn btn-white btn-sm'><i class='fa fa-trash-alt go-red'></i> Delete</button>");

				HeaderActions.Add($"<a href='/sdk/objects/data_source/m/{RecordId}/manage?ReturnUrl={returnUrlEncoded}' class='btn btn-white btn-sm'><i class='fa fa-cog go-orange'></i> Manage</a>");
				
			}
			else
			{
				HeaderActions.Add($"<button type='button' class='btn btn-white btn-sm disabled'><i class='fa fa-lock'></i> Manage Locked</a>");
				HeaderActions.Add($"<button type='button' class='btn btn-white btn-sm disabled'><i class='fa fa-lock'></i> Delete Locked</button>");
			}

		}

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			PageInit();

			if (DataSourceObject == null)
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

			if (DataSourceObject == null)
				return NotFound();

			var existingPageDataSources = new PageService().GetPageDataSourcesByDataSourceId(DataSourceObject.Id);
			if (existingPageDataSources.Count > 0)
				return Redirect($"/sdk/objects/data_source/r/{DataSourceObject.Id}");

			try
			{
				var operation = PageContext.HttpContext.Request.Query["op"];

				if (operation == "delete")
					dsMan.Delete(DataSourceObject.Id);
				else
					return NotFound();

				if (!String.IsNullOrWhiteSpace(ReturnUrl))
					return Redirect(ReturnUrl);
				
				return Redirect($"/sdk/objects/data_source/l/list");
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