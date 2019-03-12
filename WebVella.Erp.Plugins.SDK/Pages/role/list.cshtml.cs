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

namespace WebVella.Erp.Plugins.SDK.Pages.Role
{
	public class ListModel : BaseErpPageModel
	{
		public ListModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public List<GridColumn> Columns { get; set; } = new List<GridColumn>();

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

			//var appServ = new AppService();

			//var apps = appServ.GetAllApplications();
			//TotalCount = apps.Count;

			//ReturnUrlEncoded = HttpUtility.UrlEncode(PageUtils.GetCurrentUrl(PageContext.HttpContext));
			#endregion

			#region << Create Columns >>

			Columns = new List<GridColumn>() {
				new GridColumn(){
					Name = "action",
					Width="1%"
				},
				new GridColumn(){
					Label = "name",
					Name = "name",
					Width="200px"
				},
				new GridColumn(){
					Label = "description",
					Name = "description"
				}
			};

			#endregion

			#region << Records >>
			var eql = " SELECT * FROM role ";
			List<EqlParameter> eqlParams = new List<EqlParameter>();
			Records = new EqlCommand(eql, eqlParams).Execute();
			TotalCount = Records.TotalCount;
			#endregion

			BeforeRender();
			return Page();
		}
	}
}