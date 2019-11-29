using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpEntity
{
	public class EntityDataModel : BaseErpPageModel
	{
		public EntityDataModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public Entity ErpEntity { get; set; }

		public List<WvGridColumnMeta> Columns { get; set; } = new List<WvGridColumnMeta>();

		public List<EntityRecord> Records { get; set; } = new List<EntityRecord>();

		public int PagerSize { get; set; } = 15;

		public int Pager { get; set; } = 1;

		public int TotalCount { get; set; } = 0;

		public string SortBy { get; set; } = "";

		public QuerySortType SortOrder { get; set; } = QuerySortType.Ascending;

		public string ReturnUrlEncoded { get; set; } = "";

		public string PageDescription { get; set; } = "";

		public List<dynamic> Filters { get; set; } = new List<dynamic>();

		public List<Field> Fields { get; set; } = new List<Field>();

		public bool ReadAccess { get; set; } = false;

		public bool CreateAccess { get; set; } = false;

		public bool UpdateAccess { get; set; } = false;

		public bool DeleteAccess { get; set; } = false;

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			var entMan = new EntityManager();
			var recMan = new RecordManager();

			#region << InitPage >>
			int pager = 1;
			string sortBy = "";
			QuerySortType sortOrder = QuerySortType.Ascending;
			PageUtils.GetListQueryParams(PageContext.HttpContext, out pager, out sortBy, out sortOrder);
			Pager = pager;
			SortBy = sortBy;
			SortOrder = sortOrder;

			ErpEntity = entMan.ReadEntity(ParentRecordId ?? Guid.Empty).Object;

			if (ErpEntity == null)
				return NotFound();

			ReadAccess = SecurityContext.HasEntityPermission(EntityPermission.Read, ErpEntity);
			CreateAccess = SecurityContext.HasEntityPermission(EntityPermission.Create, ErpEntity);
			UpdateAccess = SecurityContext.HasEntityPermission(EntityPermission.Update, ErpEntity);
			DeleteAccess = SecurityContext.HasEntityPermission(EntityPermission.Delete, ErpEntity);

			if (string.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = $"/sdk/objects/entity/r/{ErpEntity.Id}/";

			Fields = ErpEntity.Fields.OrderBy(x => x.Name).ToList();
			var idField = Fields.Single(x => x.Name == "id");
			Fields.Remove(idField);
			Fields = Fields.OrderBy(x => x.Name).ToList();
			Fields.Insert(0, idField);

			List<QueryObject> filterQueries = new List<QueryObject>();

			#region << Process filters >>

			var submittedFilters = PageUtils.GetPageFiltersFromQuery(PageContext.HttpContext);
			if (submittedFilters.Count > 0)
			{
				foreach (var filter in submittedFilters)
				{
					if (!ErpEntity.Fields.Any(x => x.Name == filter.Name))
						continue;

					switch (filter.Type)
					{
						case FilterType.EQ:
							filterQueries.Add(EntityQuery.QueryEQ(filter.Name, filter.Value));
							break;
						case FilterType.CONTAINS:
							filterQueries.Add(EntityQuery.QueryContains(filter.Name, filter.Value));
							break;
						case FilterType.FTS:
							filterQueries.Add(EntityQuery.QueryFTS(filter.Name, filter.Value));
							break;
						case FilterType.STARTSWITH:
							filterQueries.Add(EntityQuery.QueryStartsWith(filter.Name, filter.Value));
							break;
						case FilterType.REGEX:
							filterQueries.Add(EntityQuery.QueryRegex(filter.Name, filter.Value));
							break;
					}
				}
			}
			#endregion


			#region << Process Sort >>

			var sort = new List<QuerySortObject> { new QuerySortObject("id", QuerySortType.Ascending) };
			if (!String.IsNullOrWhiteSpace(SortBy) && Fields.Any(x => x.Name == SortBy))
				sort = new List<QuerySortObject> { new QuerySortObject(SortBy, SortOrder == QuerySortType.Ascending ? QuerySortType.Ascending : QuerySortType.Descending) };

			#endregion

			if (!ReadAccess)
			{
				Records = new List<EntityRecord>();
				TotalCount = 0;

				Validation.Message = "You have no permissions to view records.";
				Validation.Errors.Add(new ValidationError("", "You have no permissions to view records."));
			}
			else
			{
				EntityQuery enQuery = null;

				if (filterQueries.Any())
					enQuery = new EntityQuery(ErpEntity.Name, query: EntityQuery.QueryAND(filterQueries.ToArray()), sort: sort.ToArray(), skip: (pager - 1) * PagerSize, limit: PagerSize);
				else
					enQuery = new EntityQuery(ErpEntity.Name, sort: sort.ToArray(), skip: (pager - 1) * PagerSize, limit: PagerSize);

				var queryResponse = recMan.Find(enQuery);
				if (!queryResponse.Success)
					throw new Exception(queryResponse.Message);

				Records = queryResponse.Object.Data;
				TotalCount = (int)recMan.Count(enQuery).Object;
			}

			ReturnUrlEncoded = HttpUtility.UrlEncode(PageUtils.GetCurrentUrl(PageContext.HttpContext));
			PageDescription = PageUtils.GenerateListPageDescription(PageContext.HttpContext, "", TotalCount);
			HeaderToolbar.AddRange(AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "data"));

			#endregion

			#region << Create Columns >>

			Columns.Add(new WvGridColumnMeta()
			{
				Name = "",
				Label = "",
				Width = "90px",
				Sortable = false,
				Searchable = false
			});

			foreach (var field in Fields)
			{
				var fieldAccess = GetFieldAccess(field);
				var searchAndSortAvailable = field.Searchable && (fieldAccess == WvFieldAccess.Full || fieldAccess == WvFieldAccess.ReadOnly);
				var column = new WvGridColumnMeta()
				{
					Name = field.Name,
					Label = field.Name, //should present just the name not to confuse
					Sortable = searchAndSortAvailable,
					Searchable = searchAndSortAvailable
				};

				if (field.GetFieldType() == FieldType.GuidField)
					column.Width = "1%";

				Columns.Add(column);
			}

			#endregion

			#region << Filters >>

			foreach (var field in Fields)
			{
				//remove fields with no access from search
				var fieldAccess = GetFieldAccess(field);
				var searchable = field.Searchable && (fieldAccess == WvFieldAccess.Full || fieldAccess == WvFieldAccess.ReadOnly);
				if (!searchable)
					continue;

				if (field.Name == "id")
				{
					dynamic filterObj = new ExpandoObject();
					filterObj.Name = field.Name;
					filterObj.Label = field.Label;
					filterObj.Type = FilterType.EQ;
					filterObj.AllowedTypes = new List<FilterType> { FilterType.EQ };
					Filters.Add(filterObj);
				}
				else if (field.Searchable)
				{
					dynamic filterObj = new ExpandoObject();
					filterObj.Name = field.Name;
					filterObj.Label = field.Label;
					filterObj.Type = FilterType.EQ;
					filterObj.AllowedTypes = new List<FilterType> { FilterType.EQ, FilterType.NOT };

					var fieldType = field.GetFieldType();
					if (fieldType == FieldType.TextField || fieldType == FieldType.MultiLineTextField ||
						 fieldType == FieldType.UrlField || fieldType == FieldType.EmailField || fieldType == FieldType.HtmlField ||
						 fieldType == FieldType.SelectField || fieldType == FieldType.MultiSelectField)
					{
						filterObj.AllowedTypes.Add(FilterType.REGEX);
						filterObj.AllowedTypes.Add(FilterType.FTS);
						filterObj.AllowedTypes.Add(FilterType.STARTSWITH);
						filterObj.AllowedTypes.Add(FilterType.CONTAINS);
					}

					Filters.Add(filterObj);
				}


			}

			#endregion

			BeforeRender();
			return Page();
		}

		public IActionResult OnPost()
		{
			var result = OnGet();
			if (result is NotFoundResult)
				return result;

			if (!PageContext.HttpContext.Request.Query.ContainsKey("recordId"))
				return NotFound();

			if (!Guid.TryParse(PageContext.HttpContext.Request.Query["recordId"], out Guid recordId))
				return NotFound();

			ErpEntity = new EntityManager().ReadEntity(ParentRecordId ?? Guid.Empty).Object;

			if (ErpEntity == null)
				return NotFound();

			try
			{
				var recMan = new RecordManager();
				var response = recMan.DeleteRecord(ErpEntity, recordId);
				if (!response.Success)
				{
					var exception = new ValidationException(response.Message);
					exception.Errors = response.Errors.MapTo<ValidationError>();
					throw exception;
				}
				return Redirect($"/sdk/objects/entity/r/{ParentRecordId}/rl/data/l");
			}
			catch (ValidationException ex)
			{
				Validation.Message = ex.Message;
				Validation.Errors = ex.Errors;
			}

			BeforeRender();
			return Page();
		}

		public WvFieldAccess GetFieldAccess(Field entityField)
		{
			var canRead = false;
			var canUpdate = false;

			if (entityField.EnableSecurity)
			{
				if (CurrentUser != null)
				{
					foreach (var role in CurrentUser.Roles)
					{
						if (entityField.Permissions.CanRead.Any(x => x == role.Id))
							canRead = true;
						if (entityField.Permissions.CanUpdate.Any(x => x == role.Id))
							canUpdate = true;
					}
				}
				else
				{
					canRead = entityField.Permissions.CanRead.Any(x => x == SystemIds.GuestRoleId);
					canUpdate = entityField.Permissions.CanRead.Any(x => x == SystemIds.GuestRoleId);
				}
			}
			else
				return WvFieldAccess.Full;


			if (canUpdate)
				return WvFieldAccess.Full;
			else if (canRead)
				return WvFieldAccess.ReadOnly;
			else
				return WvFieldAccess.Forbidden;
		}

	}
}
