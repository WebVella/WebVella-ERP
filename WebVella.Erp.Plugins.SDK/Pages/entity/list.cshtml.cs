using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models.AutoMapper;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;
using WebVella.Erp.Web.Services;
using System.Web;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpEntity
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

		private string ReturnUrlEncoded { get; set; } = "";

		public string PageDescription { get; set; } = "";

        public string SearchString {get;set;} = "";

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			var entMan = new EntityManager();

			#region << InitPage >>
			int pager = 0;
			string sortBy = "";
			QuerySortType sortOrder = QuerySortType.Ascending;
			PageUtils.GetListQueryParams(PageContext.HttpContext, out pager, out sortBy, out sortOrder);
			Pager = pager;
			SortBy = sortBy;
			SortOrder = sortOrder;

			var allEntities = entMan.ReadEntities().Object;
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
								allEntities = allEntities.FindAll(x => x.Name.ToLowerInvariant().Contains(filter.Value.ToLowerInvariant())).ToList();
							}
							break;
					}
				}
			}
			#endregion
			allEntities = allEntities.OrderBy(x => x.Name).ToList();

			TotalCount = allEntities.Count;

			ReturnUrlEncoded = HttpUtility.UrlEncode(PageUtils.GetCurrentUrl(PageContext.HttpContext));

			PageDescription = PageUtils.GenerateListPageDescription(PageContext.HttpContext, "", TotalCount);

            var searchKey = "q_name_v";
			if (HttpContext.Request.Query.ContainsKey(searchKey) && !String.IsNullOrWhiteSpace(HttpContext.Request.Query[searchKey]))
			{
				SearchString = (string)HttpContext.Request.Query[searchKey];
			}
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
					Label = "# fields",
					Name = "fields",
					Width="80px"
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
							allEntities = allEntities.OrderByDescending(x => x.Name).ToList();
						}
						else
						{
							allEntities = allEntities.OrderBy(x => x.Name).ToList();
						}
						break;
					default:
						break;
				}
			}

			//Apply pager
			var skipPages = (Pager - 1) * PagerSize;
			allEntities = allEntities.Skip(skipPages).Take(PagerSize).ToList();

			foreach (var entity in allEntities)
			{
				var record = new EntityRecord();
				record["action"] = $"<a class='btn btn-sm btn-outline-secondary' title='Entity details' href='/sdk/objects/entity/r/{entity.Id}?returnUrl={ReturnUrlEncoded}'><span class='fa fa-eye'></span></a>";
				record["icon"] = $"<div class='badge badge-pill' style='font-size:18px;color:{(String.IsNullOrWhiteSpace(entity.Color) ? "#999999" : entity.Color)};'><span class='{entity.IconName}'></span></div>";
				record["name"] = entity.Name;
				record["fields"] = entity.Fields.Count.ToString();
				Records.Add(record);
			}
			#endregion

			#region << Actions >>
			HeaderActions.AddRange(new List<string>() {
				$"<a href='/sdk/objects/entity/c?returnUrl={ReturnUrlEncoded}' class='btn btn-white btn-sm'><span class='fa fa-plus go-green'></span> Create Entity</a>"
			});

			#endregion

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}
	}
}
