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
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.Page
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

		public string PageDescription { get; set; } = "";

		public QuerySortType SortOrder { get; set; } = QuerySortType.Ascending;

		public string ReturnUrlEncoded { get; set; } = "";

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

			var pageSer = new PageService();
			var entMan = new EntityManager();
			var appServ = new AppService();

			var pages = pageSer.GetAll();
			var entities = entMan.ReadEntities().Object;
			var apps = appServ.GetAllApplications();

			#region << Apply filters >>
			var submittedFilters = PageUtils.GetPageFiltersFromQuery(PageContext.HttpContext);
			foreach (var filter in submittedFilters)
			{
				switch (filter.Name)
				{
					default:
					case "label":
						if (filter.Type == FilterType.CONTAINS)
						{
							pages = pages.FindAll(x => x.Label.ToLowerInvariant().Contains(filter.Value.ToLowerInvariant())).ToList();
						}
						break;
					case "name":
						if (filter.Type == FilterType.CONTAINS)
						{
							pages = pages.FindAll(x => x.Name.ToLowerInvariant().Contains(filter.Value.ToLowerInvariant())).ToList();
						}
						break;
					case "app":
						if (filter.Type == FilterType.EQ)
						{
							var app = apps.FirstOrDefault(x => x.Name.ToLowerInvariant() == filter.Value.ToLowerInvariant());
							if (app != null)
							{
								pages = pages.FindAll(x => x.AppId == app.Id).ToList();
							}
							else
							{
								pages = new List<ErpPage>();
							}
						}
						break;
					case "entity":
						if (filter.Type == FilterType.EQ)
						{
							var entity = entities.FirstOrDefault(x => x.Name.ToLowerInvariant() == filter.Value.ToLowerInvariant());
							if (entity != null)
							{
								pages = pages.FindAll(x => x.EntityId == entity.Id).ToList();
							}
							else
							{
								pages = new List<ErpPage>();
							}
						}
						break;
					case "type":
						if (filter.Type == FilterType.CONTAINS)
						{
							foreach (var typeEnum in Enum.GetValues(typeof(PageType)).Cast<PageType>())
							{
								var enumDescription = typeEnum.GetLabel();
								if (!enumDescription.ToLowerInvariant().Contains(filter.Value.ToLowerInvariant()))
								{
									pages = pages.FindAll(x => x.Type != typeEnum).ToList();
								}
							}
							//pages = pages.FindAll(x => x.Type == entity.Id).ToList();
						}
						break;
					case "system":
						if (filter.Type == FilterType.EQ)
						{
							if (filter.Value == "true")
							{
								pages = pages.FindAll(x => x.System).ToList();
							}
							else if (filter.Value == "false")
							{
								pages = pages.FindAll(x => !x.System).ToList();
							}
						}
						break;
					case "customized":
						if (filter.Type == FilterType.EQ)
						{
							if (filter.Value == "true")
							{
								pages = pages.FindAll(x => x.IsRazorBody).ToList();
							}
							else if (filter.Value == "false")
							{
								pages = pages.FindAll(x => !x.IsRazorBody).ToList();
							}
						}
						break;
				}
			}
			#endregion

			TotalCount = pages.Count;

			ReturnUrlEncoded = HttpUtility.UrlEncode(PageContext.HttpContext.Request.Path + PageContext.HttpContext.Request.QueryString);

			PageDescription = PageUtils.GenerateListPageDescription(PageContext.HttpContext, "", TotalCount);
			#endregion

			#region << Create Columns >>

			Columns = new List<WvGridColumnMeta>() {
				new WvGridColumnMeta(){
					Name = "action",
					Width="1%"
				},
				new WvGridColumnMeta(){
					Label = "Label",
					Name = "label",
					Sortable = true,
					Searchable = true
				},
				new WvGridColumnMeta(){
					Label = "Name",
					Name = "name",
					Sortable = true,
					Searchable = true
				},
				new WvGridColumnMeta(){
					Label = "App",
					Name = "app",
					Width="140px"
				},
				new WvGridColumnMeta(){
					Label = "Entity",
					Name = "entity",
					Width="140px"
				},
				new WvGridColumnMeta(){
					Label = "Type",
					Name = "type",
					Sortable = true,
					Width="120px"
				},
				new WvGridColumnMeta(){
					Label = "system",
					Name = "system",
					Sortable = true,
					Width="80px"
				},
				new WvGridColumnMeta(){
					Label = "Customized",
					Name = "customized",
					Sortable = true,
					Width="80px"
				}
			};

			#endregion

			#region << Records >>

			switch (SortBy)
			{
				default:
				case "label":
					if (SortOrder == QuerySortType.Descending)
						pages = pages.OrderByDescending(x => x.Label).ToList();
					else
						pages = pages.OrderBy(x => x.Label).ToList();
					break;
				case "name":
					if (SortOrder == QuerySortType.Descending)
						pages = pages.OrderByDescending(x => x.Name).ToList();
					else
						pages = pages.OrderBy(x => x.Name).ToList();
					break;
				case "type":
					if (SortOrder == QuerySortType.Descending)
						pages = pages.OrderByDescending(x => x.Type).ToList();
					else
						pages = pages.OrderBy(x => x.Type).ToList();
					break;
				case "system":
					if (SortOrder == QuerySortType.Descending)
						pages = pages.OrderByDescending(x => x.System).ToList();
					else
						pages = pages.OrderBy(x => x.System).ToList();
					break;
				case "customized":
					if (SortOrder == QuerySortType.Descending)
						pages = pages.OrderByDescending(x => x.IsRazorBody).ToList();
					else
						pages = pages.OrderBy(x => x.IsRazorBody).ToList();
					break;
			}

			//Apply pager
			var skipPages = (Pager - 1) * PagerSize;
			pages = pages.Skip(skipPages).Take(PagerSize).ToList();

			foreach (var page in pages)
			{
				//The entity of the page could be deleted. In this case show its id as name
				var pageEntityName = "";
				if (page.EntityId != null)
					pageEntityName = page.EntityId.Value.ToString();

				var pageEntity = entities.FirstOrDefault(x => x.Id == page.EntityId);
				if (pageEntity != null)
					pageEntityName = pageEntity.Name;

				var record = new EntityRecord();
				record["id"] = page.Id;
				record["label"] = page.Label;
				record["name"] = page.Name;
				record["app"] = page.AppId != null ? apps.First(x => x.Id == page.AppId).Name : "";
				record["entity"] = pageEntityName;
				record["type"] = $"{page.Type.GetLabel()}";
				record["system"] = page.System;
				record["customized"] = page.IsRazorBody;
				Records.Add(record);
			}
			#endregion

			BeforeRender();
			return Page();
		}

		public IActionResult OnPost()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			var pageServ = new PageService();

			if (!PageContext.HttpContext.Request.Query.ContainsKey("pageId"))
				return NotFound();

			if (!Guid.TryParse(PageContext.HttpContext.Request.Query["pageId"], out Guid pageId))
				return NotFound();


			pageServ.ClonePage(pageId);
			BeforeRender();
			return Redirect($"/sdk/objects/page/l/list");
		}
	}
}