using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpEntity
{
	public class FieldsModel : BaseErpPageModel
	{
		public FieldsModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public Entity ErpEntity { get; set; }

		public List<WvGridColumnMeta> Columns { get; set; } = new List<WvGridColumnMeta>();

		public List<EntityRecord> Records { get; set; } = new List<EntityRecord>();

		public int PagerSize { get; set; } = 1000;

		public int Pager { get; set; } = 1;

		public int TotalCount { get; set; } = 0;

		public string SortBy { get; set; } = "";

		public QuerySortType SortOrder { get; set; } = QuerySortType.Ascending;

		private string ReturnUrlEncoded { get; set; } = "";

		public string PageDescription { get; set; } = "";

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

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

			ErpEntity = entMan.ReadEntity(ParentRecordId ?? Guid.Empty).Object;

			if (ErpEntity == null)
			{
				return NotFound();
			}

			if (String.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = $"/sdk/objects/entity/r/{ErpEntity.Id}/";

			var allFields = ErpEntity.Fields;

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
								allFields = allFields.FindAll(x => x.Name.ToLowerInvariant().Contains(filter.Value.ToLowerInvariant())).ToList();
							}
							break;
					}
				}
			}
			#endregion

			allFields = allFields.OrderBy(x => x.Name).ToList();
			TotalCount = allFields.Count;

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
					Label = "Name",
					Name = "name",
					Sortable = true,
					Searchable = true
				},
				new WvGridColumnMeta(){
					Label = "Type",
					Name = "type",
					Width="120px"
				},
				new WvGridColumnMeta(){
					Label = "System",
					Name = "system",
					Width="1%"
				},
				new WvGridColumnMeta(){
					Label = "Required",
					Name = "required",
					Width="1%"
				},
				new WvGridColumnMeta(){
					Label = "Unique",
					Name = "unique",
					Width="80px"
				},
				new WvGridColumnMeta(){
					Label = "Searchable",
					Name = "searchable",
					Width="1%"
				},
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
							allFields = allFields.OrderByDescending(x => x.Name).ToList();
						}
						else
						{
							allFields = allFields.OrderBy(x => x.Name).ToList();
						}
						break;
					default:
						break;
				}
			}

			//Apply pager
			var skipPages = (Pager - 1) * PagerSize;
			allFields = allFields.Skip(skipPages).Take(PagerSize).ToList();

			foreach (var field in allFields)
			{
				var record = new EntityRecord();
				record["action"] = $"<a class='btn btn-sm btn-outline-secondary' title='App details' href='/sdk/objects/entity/r/{ErpEntity.Id}/rl/fields/r/{field.Id}?returnUrl={ReturnUrlEncoded}'><span class='fa fa-eye'></span></a>";
				record["name"] = field.Name;
				record["type"] = field.GetFieldType().ToString();
				record["system"] = field.System;
				record["required"] = field.Required;
				record["unique"] = field.Unique;
				record["searchable"] = field.Searchable;
				Records.Add(record);
			}
			#endregion

			#region << Actions >>
			HeaderActions.AddRange(new List<string>() {
				$"<a href='/sdk/objects/entity/r/{(ErpEntity != null ? ErpEntity.Id : Guid.Empty)}/rl/fields/c/select?returnUrl={ReturnUrlEncoded}' class='btn btn-white btn-sm'><span class='fa fa-plus go-green'></span> Create Field</a>",
				$"<button type='button' onclick='ErpEvent.DISPATCH(\"WebVella.Erp.Web.Components.PcDrawer\",\"open\")' class='btn btn-white btn-sm'><span class='fa fa-search'></span> Search</a>"
			});
			HeaderToolbar.AddRange(AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "fields"));
			#endregion


			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}
	}
}