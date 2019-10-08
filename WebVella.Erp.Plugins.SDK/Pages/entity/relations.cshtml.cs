using Microsoft.AspNetCore.Mvc;
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
	public class RelationsModel : BaseErpPageModel
	{
		public RelationsModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public Entity ErpEntity { get; set; }

		public List<WvGridColumnMeta> Columns { get; set; } = new List<WvGridColumnMeta>();

		public List<EntityRecord> Records { get; set; } = new List<EntityRecord>();

		public int PagerSize { get; set; } = 1000;

		public int Pager { get; set; } = 1;

		public int TotalCount { get; set; } = 0;

		public string SortBy { get; set; } = "";

		public QuerySortType SortOrder { get; set; } = QuerySortType.Ascending;

		public string ReturnUrlEncoded { get; set; } = "";

		public string PageDescription { get; set; } = "";

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			var entMan = new EntityManager();
			var relMan = new EntityRelationManager();

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
				return NotFound();

			if (string.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = $"/sdk/objects/entity/r/{ErpEntity.Id}/";

			var entityRelations = relMan.Read().Object.Where(x => x.TargetEntityId == ErpEntity.Id || x.OriginEntityId == ErpEntity.Id).ToList();

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
								entityRelations = entityRelations.FindAll(x => x.Name.ToLowerInvariant().Contains(filter.Value.ToLowerInvariant())).ToList();
							}
							break;
					}
				}
			}
			#endregion

			entityRelations = entityRelations.OrderBy(x => x.Name).ToList();
			TotalCount = entityRelations.Count;

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
					Label = "Origin",
					Name = "origin",
					Sortable = false,
					Searchable = false,
					Width="25%"
				},
				new WvGridColumnMeta(){
					Label = "Target",
					Name = "target",
					Sortable = false,
					Searchable = false,
					Width="25%",
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
							entityRelations = entityRelations.OrderByDescending(x => x.Name).ToList();
						else
							entityRelations = entityRelations.OrderBy(x => x.Name).ToList();
						break;
					default:
						break;
				}
			}

			//Apply pager
			var skipPages = (Pager - 1) * PagerSize;
			entityRelations = entityRelations.Skip(skipPages).Take(PagerSize).ToList();

			const string OneToOne = " <span class='badge badge-primary badge-inverse' title='One to One' style='margin-left:5px;'>1 : 1</span>";
			const string OneToMany = " <span class='badge badge-primary badge-inverse' title='One to Many' style='margin-left:5px;'>1 : N</span>";
			const string ManyToMany = " <span class='badge badge-primary badge-inverse' title='Many to Many' style='margin-left:5px;'>N : N</span>";

			foreach (var relation in entityRelations)
			{
				var nameColumnText = relation.Name + (relation.System ? " <i class='fa fa-fw fa-lock'></i>" : "");
				switch (relation.RelationType)
				{
					case EntityRelationType.OneToOne:
						nameColumnText = nameColumnText + OneToOne;
						break;
					case EntityRelationType.OneToMany:
						nameColumnText = nameColumnText + OneToMany;
						break;
					case EntityRelationType.ManyToMany:
						nameColumnText = nameColumnText + ManyToMany;
						break;
				}


				var originColumnText = $"<div><span class='go-gray'>Entity: </span> {relation.OriginEntityName}</div><div><span class='go-gray'>Field: </span> {relation.OriginFieldName}</div>";
				var targetColumnText = $"<div><span class='go-gray'>Entity: </span> {relation.TargetEntityName}</div><div><span class='go-gray'>Field: </span> {relation.TargetFieldName}</div>";

				var record = new EntityRecord();
				record["action"] = $"<a class='btn btn-sm btn-outline-secondary' title='view relation details' href='/sdk/objects/entity/r/{ErpEntity.Id}/rl/relations/r/{relation.Id}?returnUrl={ReturnUrlEncoded}'><span class='fa fa-eye'></span></a>";
				record["name"] = nameColumnText;
				record["origin"] = originColumnText;
				record["target"] = targetColumnText;
				Records.Add(record);
			}
			#endregion

			HeaderToolbar.AddRange(AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "relations"));

			ErpRequestContext.PageContext = PageContext;

			BeforeRender();
			return Page();
		}
	}
}