using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Jobs;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.Job
{
	public class PlanModel : BaseErpPageModel
	{
		public PlanModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public List<WvGridColumnMeta> Columns { get; set; } = new List<WvGridColumnMeta>();

		public List<OutputSchedulePlan> Records { get; set; } = new List<OutputSchedulePlan>();

		[BindProperty]
		public Guid? TriggerPlanId { get; set; } = null;

		public int PagerSize { get; set; } = 15;

		public int Pager { get; set; } = 1;

		public int TotalCount { get; set; } = 0;

		public string SortBy { get; set; } = "";

		public QuerySortType SortOrder { get; set; } = QuerySortType.Ascending;

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;


			#region << InitPage >>

			HeaderToolbar.AddRange(AdminPageUtils.GetJobAdminSubNav("plan"));

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
					Width="140px"
				},
				new WvGridColumnMeta(){
					Label = "",
					Name = "status",
					Width="30px"
				},
				new WvGridColumnMeta(){
					Label = "name",
					Name = "name"
				},
				new WvGridColumnMeta(){
					Label = "type",
					Name = "type",
					Width="100px"
				},
				new WvGridColumnMeta(){
					Label = "last trigger",
					Name = "last_trigger",
					Width="140px"
				},
				new WvGridColumnMeta(){
					Label = "next trigger",
					Name = "next_trigger",
					Width="140px"
				},
			};

			#endregion

			#region << Records >>
			Records = ScheduleManager.Current.GetSchedulePlans().MapTo<OutputSchedulePlan>();
			Records = Records.OrderBy(x => x.Name).ThenByDescending(x => x.CreatedOn).ToList();

			#region << Apply Filters >>
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
								Records = Records.FindAll(x => x.Name.ToLowerInvariant().Contains(filter.Value.ToLowerInvariant())).ToList();
							}
							break;
						default:
							break;
					}
				}
			}
			#endregion

			TotalCount = Records.Count;
			Records = Records.Skip((Pager - 1) * PagerSize).Take(PagerSize).ToList();
			#endregion

			BeforeRender();
			return Page();
		}

		public IActionResult OnPost()
		{
			if (TriggerPlanId == null)
				return NotFound();

			var schedulePlan = ScheduleManager.Current.GetSchedulePlan(TriggerPlanId.Value);

			if (schedulePlan == null)
				return NotFound();

			ScheduleManager.Current.TriggerNowSchedulePlan(schedulePlan);

			BeforeRender();
			return Redirect(PageContext.HttpContext.Request.Path + PageContext.HttpContext.Request.QueryString.ToUriComponent());

		}
	}
}