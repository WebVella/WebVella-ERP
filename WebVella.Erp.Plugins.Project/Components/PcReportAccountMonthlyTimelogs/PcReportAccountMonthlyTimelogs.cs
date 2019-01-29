using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.Project.Services;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.Project.Components
{
	[PageComponent(Label = "Report: Monthly timelog for an account", Library = "WebVella", Description = "Reports for the timelog for a selected month and account", Version = "0.0.1", IconClass = "fas fa-calculator")]
	public class PcReportAccountMonthlyTimelogs : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcReportAccountMonthlyTimelogs([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcReportAccountMonthlyTimelogsOptions
		{
			[JsonProperty(PropertyName = "year")]
			public string Year { get; set; } = "";

			[JsonProperty(PropertyName = "month")]
			public string Month { get; set; } = "";

			[JsonProperty(PropertyName = "account_id")]
			public string AccountId { get; set; } = "";
		}

		public async Task<IViewComponentResult> InvokeAsync(PageComponentContext context)
		{
			ErpPage currentPage = null;
			try
			{
				#region << Init >>
				if (context.Node == null)
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: The node Id is required to be set as query parameter 'nid', when requesting this component"));
				}

				var pageFromModel = context.DataModel.GetProperty("Page");
				if (pageFromModel == null)
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: PageModel cannot be null"));
				}
				else if (pageFromModel is ErpPage)
				{
					currentPage = (ErpPage)pageFromModel;
				}
				else
				{
					return await Task.FromResult<IViewComponentResult>(Content("Error: PageModel does not have Page property or it is not from ErpPage Type"));
				}

				var options = new PcReportAccountMonthlyTimelogsOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcReportAccountMonthlyTimelogsOptions>(context.Options.ToString());
				}

				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

				ViewBag.Options = options;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.ComponentContext = context;



				#region << Init Data >>

				if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help)
				{
					var selectedYearString = context.DataModel.GetPropertyValueByDataSource(options.Year) as string;
					if (string.IsNullOrWhiteSpace(selectedYearString))
						selectedYearString = DateTime.Now.Year.ToString();
					if (!int.TryParse(selectedYearString, out int year))
						throw new Exception("Year is not specified.");

					var selectedMonthString = context.DataModel.GetPropertyValueByDataSource(options.Month) as string;
					if (string.IsNullOrWhiteSpace(selectedMonthString))
						selectedMonthString = DateTime.Now.Month.ToString();
					if (!int.TryParse(selectedMonthString, out int month))
						throw new Exception("Month is not specified.");

					Guid? accountId = null;
					var selectedAccountIdString = context.DataModel.GetPropertyValueByDataSource(options.AccountId) as string;
					if (Guid.TryParse(selectedAccountIdString, out Guid selectedAccountId))
						accountId = selectedAccountId;
					else if (!String.IsNullOrWhiteSpace(selectedAccountIdString))
						throw new Exception("Selected account id is not GUID.");

					List<EntityRecord> data = new ReportService().GetTimelogData(year, month, accountId);

					List<EntityRecord> projects = new List<EntityRecord>();
					List<EntityRecord> tasks = new List<EntityRecord>();
					decimal overallBillableMinutes = 0;
					decimal overallNonBillableMinutes = 0;

					foreach (var rec in data)
					{
						if (!projects.Any(x => (Guid)rec["project_id"] == (Guid)x["id"]))
						{
							EntityRecord projRec = new EntityRecord();
							projRec["id"] = rec["project_id"];
							projRec["name"] = rec["project_name"];
							projects.Add(projRec);
						}
						tasks.Add(rec);
						overallBillableMinutes += (decimal)rec["billable_minutes"];
						overallNonBillableMinutes += (decimal)rec["non_billable_minutes"];
					}
					ViewBag.projects = projects;
					ViewBag.tasks = tasks;
					ViewBag.overallBillableMinutes = overallBillableMinutes;
					ViewBag.overallNonBillableMinutes = overallNonBillableMinutes;
				}

				#endregion

				switch (context.Mode)
				{
					case ComponentMode.Display:
						return await Task.FromResult<IViewComponentResult>(View("Display"));
					case ComponentMode.Design:
						return await Task.FromResult<IViewComponentResult>(View("Design"));
					case ComponentMode.Options:
						return await Task.FromResult<IViewComponentResult>(View("Options"));
					case ComponentMode.Help:
						return await Task.FromResult<IViewComponentResult>(View("Help"));
					default:
						ViewBag.Error = new ValidationException()
						{
							Message = "Unknown component mode"
						};
						return await Task.FromResult<IViewComponentResult>(View("Error"));
				}

			}
			catch (ValidationException ex)
			{
				ViewBag.Error = ex;
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
			catch (Exception ex)
			{
				ViewBag.Error = new ValidationException()
				{
					Message = ex.Message
				};
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
		}
	}
}
