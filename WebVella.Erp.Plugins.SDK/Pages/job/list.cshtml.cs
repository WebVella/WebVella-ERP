using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Jobs;
using WebVella.Erp.Plugins.SDK.Services;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.Job
{
    public class ListModel : BaseErpPageModel
    {
        public ListModel([FromServices] ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

        public List<WvGridColumnMeta> Columns { get; set; } = new List<WvGridColumnMeta>();

        public List<Erp.Jobs.Job> Records { get; set; } = new List<Erp.Jobs.Job>();

        public int PagerSize { get; set; } = 15;

        public int Pager { get; set; } = 1;

        public int TotalCount { get; set; } = 0;

        public string SortBy { get; set; } = "";

        public QuerySortType SortOrder { get; set; } = QuerySortType.Ascending;

        public List<string> HeaderToolbar { get; private set; } = new List<string>();

        public void InitPageData()
        {
            #region << InitPage >>

            HeaderToolbar.AddRange(AdminPageUtils.GetJobAdminSubNav("job"));

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
                    Label = "created on",
                    Name = "created_on",
                    Width="150px"
                },
                new WvGridColumnMeta(){
                    Label = "started on",
                    Name = "started_on",
                    Width="150px"
                },
                new WvGridColumnMeta(){
                    Label = "finished on",
                    Name = "finished_on",
                    Width="150px"
                },
                new WvGridColumnMeta(){
                    Label = "type name",
                    Name = "type_name",
                },
                new WvGridColumnMeta(){
                    Label = "complete class name",
                    Name = "complete_class_name",
                    Width="400px"
                },
                new WvGridColumnMeta(){
                    Label = "status",
                    Name = "status",
                    Width="100px"
                }
            };

            #endregion

            #region << Records >>

            var submittedFilters = PageUtils.GetPageFiltersFromQuery(PageContext.HttpContext);
            string typeName = null;
            Guid? typeId = null;
            if (submittedFilters.Count > 0)
            {
                var whereClauseList = new List<string>();
                foreach (var filter in submittedFilters)
                {
                    switch (filter.Name)
                    {
                        case "type_name":
                            typeName = filter.Value;
                            break;
                        case "type_id":
                            if (Guid.TryParse(filter.Value, out Guid outGuid))
                            {
                                typeId = outGuid;
                            }
                            break;
                    }
                }
            }

            int totalCount;
            Records = JobManager.Current.GetJobs(out totalCount, null, null, null, null, typeName, null, null, typeId, Pager, PagerSize);
            TotalCount = totalCount;

            #endregion
        }


        public IActionResult OnGet()
        {
            var initResult = Init();
            if (initResult != null)
                return initResult;

            InitPageData();

            BeforeRender();
            return Page();
        }

        public IActionResult OnPost()
        {
            var initResult = Init();
            if (initResult != null)
                return initResult;

            new LogService().ClearJobLogs();

            InitPageData();

            BeforeRender();
            return Page();
        }
    }
}
