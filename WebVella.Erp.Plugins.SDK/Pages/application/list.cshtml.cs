using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.Application
{
	public class ListModel : BaseErpPageModel
	{
		public ListModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public List<WvGridColumnMeta> Columns { get; set; } = new List<WvGridColumnMeta>();

		public List<EntityRecord> Records { get; set; } = new List<EntityRecord>();

		public int PagerSize { get; set; } = 15;

		public int Pager { get; set; } = 1;

		public int TotalCount { get; set; } = 0;

		public string SortBy { get; set; } = "";

		public QuerySortType SortOrder { get; set; } = QuerySortType.Ascending;

		public string ReturnUrlEncoded { get; set; } = "";

		public string PageDescription { get; set; } = "";

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			#region << InitPage >>
			int pager = 0;
			string sortBy = "";
			QuerySortType sortOrder = QuerySortType.Ascending;
			PageUtils.GetListQueryParams(PageContext.HttpContext, out pager, out sortBy, out sortOrder);
			Pager = pager;
			SortBy = sortBy;
			SortOrder = sortOrder;

			var appServ = new AppService();

			var apps = appServ.GetAllApplications().OrderBy(x => x.Name).ToList();
			#region << Apply filters >>
			var submittedFilters = PageUtils.GetPageFiltersFromQuery(PageContext.HttpContext);
			if (submittedFilters.Count > 0)
			{
				foreach (var filter in submittedFilters)
				{
					switch (filter.Name)
					{
						case "name":
							if (filter.Type == FilterType.CONTAINS)
							{
								apps = apps.FindAll(x => x.Name.ToLowerInvariant().Contains(filter.Value.ToLowerInvariant())).ToList();
							}
							break;
					}
				}
			}
			#endregion


			TotalCount = apps.Count;

			ReturnUrlEncoded = HttpUtility.UrlEncode(PageUtils.GetCurrentUrl(PageContext.HttpContext));

			PageDescription = PageUtils.GenerateListPageDescription(PageContext.HttpContext, "", TotalCount);
			#endregion

			#region << Create Columns >>

			Columns = new List<WvGridColumnMeta>() {
				new WvGridColumnMeta(){
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
					Sortable = true,
					Searchable = true
				},
				new WvGridColumnMeta(){
					Label = "Label",
					Name = "label",
					Sortable = true,
					Searchable = true
				},
				new WvGridColumnMeta(){
					Label = "Description",
					Name = "description"
				}
			};

			#endregion

			#region << Records >>
			//Apply sort
			if (!String.IsNullOrWhiteSpace(SortBy))
			{
				switch (SortBy)
				{
					case "name":
						if (SortOrder == QuerySortType.Descending)
						{
							apps = apps.OrderByDescending(x => x.Name).ToList();
						}
						else
						{
							apps = apps.OrderBy(x => x.Name).ToList();
						}
						break;
					case "label":
						if (SortOrder == QuerySortType.Descending)
						{
							apps = apps.OrderByDescending(x => x.Label).ToList();
						}
						else
						{
							apps = apps.OrderBy(x => x.Label).ToList();
						}
						break;
					default:
						break;
				}
			}

			//Apply pager
			var skipPages = (Pager - 1) * PagerSize;
			apps = apps.Skip(skipPages).Take(PagerSize).ToList();

			foreach (var app in apps)
			{
				var record = new EntityRecord();
				record["action"] = $"<a class='btn btn-sm btn-outline-secondary' title='App details' href='/sdk/objects/application/r/{app.Id}?returnUrl={ReturnUrlEncoded}'><span class='fa fa-eye'></span></a>";
				record["name"] = app.Name;
				record["label"] = app.Label;
				record["icon"] = $"<div class='badge badge-pill' style='font-size:18px;color:{app.Color};'><span class='{app.IconClass}'></span></div>";
				record["description"] = app.Description;
				Records.Add(record);
			}
			#endregion

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}
	}
}