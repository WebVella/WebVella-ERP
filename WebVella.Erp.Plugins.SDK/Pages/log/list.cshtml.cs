using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.Log
{
	public class ListModel : BaseErpPageModel
	{
		public ListModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public List<WvGridColumnMeta> Columns { get; set; } = new List<WvGridColumnMeta>();

		public EntityRecordList Records { get; set; } = new EntityRecordList();

		public int PagerSize { get; set; } = 15;

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
					Label = "date",
					Name = "Date",
					Width="150px"
				},
				new WvGridColumnMeta(){
					Label = "type",
					Name = "Type",
					Width="40px"
				},
				new WvGridColumnMeta(){
					Label = "source",
					Name = "Source"
				},
				new WvGridColumnMeta(){
					Label = "message",
					Name = "Message"
				},
				new WvGridColumnMeta(){
					Label = "status",
					Name = "Status",
					Width= "40px"
				},
			};

			#endregion

			#region << Records >>

			string querySource = null;
			string queryMessage = null;

			var submittedFilters = PageUtils.GetPageFiltersFromQuery(PageContext.HttpContext);
			if (submittedFilters.Count > 0)
			{
				var whereClauseList = new List<string>();
				foreach (var filter in submittedFilters)
				{
					switch (filter.Name)
					{
						case "source":
							querySource = filter.Value;
							break;
						case "message":
							queryMessage = filter.Value;
							break;
					}
				}
			}

			Records = new Diagnostics.Log().GetLogs(Pager, PagerSize, querySource, queryMessage);
			TotalCount = Records.TotalCount;
			#endregion
			BeforeRender();
			return Page();
		}
	}
}