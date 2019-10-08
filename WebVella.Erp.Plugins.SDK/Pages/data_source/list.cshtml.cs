using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpDataSource
{
	public class ListModel : BaseErpPageModel
	{
		public ListModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public List<WvGridColumnMeta> Columns { get; set; } = new List<WvGridColumnMeta>();

		public List<EntityRecord> Records { get; set; } = new List<EntityRecord>();

		public int PageSize { get; set; } = 15;

		public new int Page { get; set; } = 1;

		public int TotalCount { get; set; } = 0;

		public string SortBy { get; set; } = "";

		public QuerySortType SortOrder { get; set; } = QuerySortType.Ascending;

		private string ReturnUrlEncoded { get; set; } = "";

		public string PageDescription { get; set; } = "";

		public List<SelectOption> TypeOptions { get; set; } = new List<SelectOption>() {
				new SelectOption() {Value = ((int)DataSourceType.DATABASE).ToString(), Label = DataSourceType.DATABASE.GetLabel()},
				new SelectOption() {Value = ((int)DataSourceType.CODE).ToString(), Label = DataSourceType.CODE.GetLabel()}};

		public List<FilterType> FilterTypes { get; private set; } = new List<FilterType> { FilterType.CONTAINS };

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			DataSourceManager dsMan = new DataSourceManager();

			int pager = 0;
			string sortBy = "";
			QuerySortType sortOrder = QuerySortType.Ascending;
			PageUtils.GetListQueryParams(PageContext.HttpContext, out pager, out sortBy, out sortOrder);
			Page = pager;
			SortBy = sortBy;
			SortOrder = sortOrder;
			ReturnUrlEncoded = HttpUtility.UrlEncode(PageUtils.GetCurrentUrl(PageContext.HttpContext));

			var allDataSources = dsMan.GetAll().OrderBy(x => x.Name).ToList();
			foreach (var ds in allDataSources)
			{
				if (ds is DatabaseDataSource)
				{
					var record = new EntityRecord();
					var recordId = ds.Id;
					record["action"] = $"<a class='btn btn-sm btn-outline-secondary' title='Data source details' href='/sdk/objects/data_source/r/{recordId}?returnUrl={ReturnUrlEncoded}'><span class='fa fa-eye'></span></a>";
					record["icon"] = PageUtils.GetDataSourceIconBadge(DataSourceType.DATABASE);
					record["name"] = ds.Name;
					record["target"] = ds.EntityName;
					record["type_description"] = DataSourceType.DATABASE.GetLabel();
					record["type"] = DataSourceType.DATABASE;
					record["model"] = ds.ResultModel;
					record["param_count"] = ds.Parameters.Count;
					Records.Add(record);
				}
				else 
				{
					var record = new EntityRecord();
					var recordId = ds.Id;
					record["action"] = $"<a class='btn btn-sm btn-outline-secondary' title='Data source details' href='/sdk/objects/data_source/r/{recordId}?returnUrl={ReturnUrlEncoded}'><span class='fa fa-eye'></span></a>";
					record["icon"] = PageUtils.GetDataSourceIconBadge(DataSourceType.CODE);
					record["name"] = ds.Name;
					record["target"] = ds.GetType().FullName;
					record["type_description"] = DataSourceType.CODE.GetLabel();
					record["type"] = DataSourceType.CODE;
					record["model"] = ds.ResultModel;
					record["param_count"] = 0;
					Records.Add(record);
				}
			}

			TotalCount = Records.Count;
			PageDescription = PageUtils.GenerateListPageDescription(PageContext.HttpContext, "", TotalCount);

			#region << Filters >>

			var submittedFilters = PageUtils.GetPageFiltersFromQuery(PageContext.HttpContext);
			if (submittedFilters.Count > 0)
			{
				foreach (var filter in submittedFilters)
				{
					switch (filter.Name)
					{
						case "name":
							Records = Records.FindAll(x => x["name"].ToString().ToLowerInvariant().Contains(filter.Value.ToLowerInvariant())).ToList();
							break;
						case "model":
							Records = Records.FindAll(x => x["model"].ToString().ToLowerInvariant().Contains(filter.Value.ToLowerInvariant())).ToList();
							break;
						case "target":
							Records = Records.FindAll(x => x["target"].ToString().ToLowerInvariant().Contains(filter.Value.ToLowerInvariant())).ToList();
							break;
						case "type":
								Records = Records.FindAll(x => ((int)x["type"]).ToString().Equals(filter.Value)).ToList();
							break;
					}
				}
			}

			#endregion

			#region << Sort >>

			if (!String.IsNullOrWhiteSpace(SortBy))
			{
				switch (SortBy)
				{
					case "name":
						if (SortOrder == QuerySortType.Descending)
							Records = Records.OrderByDescending(x => x["name"].ToString()).ToList();
						else
							Records = Records.OrderBy(x => x["name"].ToString()).ToList();
						break;
					case "type":
						if (SortOrder == QuerySortType.Descending)
							Records = Records.OrderByDescending(x => x["type"].ToString()).ToList();
						else
							Records = Records.OrderBy(x => x["type"].ToString()).ToList();
						break;
					case "target":
						if (SortOrder == QuerySortType.Descending)
							Records = Records.OrderByDescending(x => x["target"].ToString()).ToList();
						else
							Records = Records.OrderBy(x => x["target"].ToString()).ToList();
						break;
					default:
						break;
				}
			}

			#endregion

			#region << Create Columns >>
			Columns = new List<WvGridColumnMeta>() {
				new WvGridColumnMeta(){
					Label = "",
					Name = "action",
					Width="1%"
				},
				new WvGridColumnMeta(){
					Label = "Icon",
					Name = "icon",
					Width="1%"
				},
				new WvGridColumnMeta(){
					Label = "Name",
					Name = "name",
					Width="220px",
					Sortable = true
				},
				new WvGridColumnMeta(){
					Label = "Type",
					Name = "type",
					Width="120px",
					Sortable = true
				},
				new WvGridColumnMeta(){
					Label = "target",
					Name = "Target",
					Sortable = true
				},
				new WvGridColumnMeta(){
					Label = "Returned Model",
					Name = "model",
					Width="220px"
				},
				new WvGridColumnMeta(){
					Label = "Params",
					Name = "param_count",
					Width="40px"
				},
			};
			#endregion

			#region << Actions >>
			HeaderActions.AddRange(new List<string>() {
				$"<a href='/sdk/objects/data_source/c/create?returnUrl={ReturnUrlEncoded}' class='btn btn-white btn-sm'><span class='fa fa-plus go-green'></span> Add Data Source</a>",
				$"<button type='button' onclick='ErpEvent.DISPATCH(\"WebVella.Erp.Web.Components.PcDrawer\",\"open\")' class='btn btn-white btn-sm'><span class='fa fa-search'></span> Search</a>"
			});

			#endregion

			
			var skipPages = (Page - 1) * PageSize;
			Records = Records.Skip(skipPages).Take(PageSize).ToList();

			
			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}
	}
}