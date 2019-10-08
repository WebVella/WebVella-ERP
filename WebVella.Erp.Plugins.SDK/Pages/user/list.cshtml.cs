using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.User
{
	public class ListModel : BaseErpPageModel
	{
		public ListModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public List<WvGridColumnMeta> Columns { get; set; } = new List<WvGridColumnMeta>();

		public EntityRecordList Records { get; set; } = new EntityRecordList();

		public int PagerSize { get; set; } = 10;

		public int Pager { get; set; } = 1;

		public int TotalCount { get; set; } = 0;

		public string SortBy { get; set; } = "";

		public QuerySortType SortOrder { get; set; } = QuerySortType.Ascending;

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

			#endregion

			#region << Create Columns >>

			Columns = new List<WvGridColumnMeta>() {
				new WvGridColumnMeta(){
					Name = "action",
					Width="1%"
				},
				new WvGridColumnMeta(){
					Label = "email",
					Name = "email",
					Sortable = true,
					Width = "120px"
				},
				new WvGridColumnMeta(){
					Label = "username",
					Name = "username",
					Sortable = true
				},
				new WvGridColumnMeta(){
					Label = "role",
					Name = "role",
					Sortable = false
				}
			};

			#endregion

			#region << Records >>
			var eql = " SELECT id,email,username,$user_role.name FROM user ";
			List<EqlParameter> eqlParams = new List<EqlParameter>();

			//Apply sort
			if (!String.IsNullOrWhiteSpace(SortBy) && (new List<string>() { "email", "username" }).Contains(SortBy))
			{
				eqlParams.Add(new EqlParameter("@sortBy", SortBy));
				if (SortOrder == QuerySortType.Descending)
				{
					eqlParams.Add(new EqlParameter("@sortOrder", "Desc"));
				}
				else
				{
					eqlParams.Add(new EqlParameter("@sortOrder", "Asc"));
				}
				eql += " ORDER BY @sortBy @sortOrder ";
			}
			else {
				eql += " ORDER BY email Asc ";
			}
			eql += " PAGE @page ";
			eql += " PAGESIZE @pageSize ";
			eqlParams.Add(new EqlParameter("@page", Pager));
			eqlParams.Add(new EqlParameter("@pageSize", PagerSize));
			
			Records = new EqlCommand(eql, eqlParams).Execute();
			TotalCount = Records.TotalCount;
			#endregion

			BeforeRender();
			return Page();
		}
	}
}