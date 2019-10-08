using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.Project.Model;
using WebVella.Erp.Plugins.Project.Services;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.Project.Components
{
	[PageComponent(Label = "Project Widget Budget Chart", Library = "WebVella", Description = "Chart presenting the current project budget", Version = "0.0.1", IconClass = "fas fa-chart-pie")]
	public class PcProjectWidgetBudgetChart : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcProjectWidgetBudgetChart([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcProjectWidgetBudgetChartOptions
		{

			[JsonProperty(PropertyName = "project_id")]
			public string ProjectId { get; set; } = null;
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

				var options = new PcProjectWidgetBudgetChartOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcProjectWidgetBudgetChartOptions>(context.Options.ToString());
				}

				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion


				ViewBag.Options = options;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.ComponentContext = context;

				if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help)
				{

					Guid projectId = context.DataModel.GetPropertyValueByDataSource(options.ProjectId) as Guid? ?? Guid.Empty;

					if (projectId == Guid.Empty)
						return await Task.FromResult<IViewComponentResult>(Content("Error: ProjectId is required"));

					var projectSrv = new ProjectService();
					var projectRecord = projectSrv.Get(projectId);
					var projectTasks = new TaskService().GetTaskQueue(projectId,null,TasksDueType.All);
					var projectTimelogs = projectSrv.GetProjectTimelogs(projectId);

					decimal loggedBillableMinutes = 0;
					decimal loggedNonBillableMinutes = 0;
					decimal projectEstimatedMinutes = 0;

					foreach (var timelog in projectTimelogs)
					{
						if ((bool)timelog["is_billable"])
							loggedBillableMinutes += (decimal)timelog["minutes"];
						else
							loggedNonBillableMinutes += (decimal)timelog["minutes"];
					}

					foreach (var task in projectTasks)
					{
						projectEstimatedMinutes += (decimal)task["estimated_minutes"];
					}

					var billedHours = Math.Round(loggedBillableMinutes / 60, 2);
					var nonBilledhours = Math.Round(loggedNonBillableMinutes / 60, 2);
					var estimatedHours = Math.Round(projectEstimatedMinutes / 60, 2);

					var budgetLeft = (decimal)0;
					decimal budgetAmount = projectRecord["budget_amount"] != null ? (decimal)projectRecord["budget_amount"] : 0;
					decimal hourRate = projectRecord["hour_rate"] != null ? (decimal)projectRecord["hour_rate"] : 0;

					if ((string)projectRecord["budget_type"] == "on duration")
					{
						budgetLeft = budgetAmount - billedHours;
					}
					else {
						if (hourRate > 0 && budgetAmount > 0)
						{
							budgetLeft = (budgetAmount / hourRate) - billedHours;
						}
						else {
							budgetLeft = 0;
						}
					}

					var budgetResult = new EntityRecord();
					budgetResult["billed_hours"] = billedHours;
					budgetResult["nonbilled_hours"] = nonBilledhours;
					budgetResult["estimated_hours"] = estimatedHours;
					budgetResult["budget_type"] = (string)projectRecord["budget_type"];
					budgetResult["budget_left"] = budgetLeft;
					ViewBag.BudgetResult = budgetResult;

					var totalLogged = billedHours + nonBilledhours;
					var billedPercentage = 0;
					var nonBilledPercantage = 0;
					if (totalLogged > 0)
					{
						billedPercentage = (int)Math.Round(billedHours * 100 / totalLogged, 0);
						nonBilledPercantage = 100 - billedPercentage;
					}
					var theme = new Theme();
					var chartDatasets = new List<WvChartDataset>() {
						new WvChartDataset(){
							Data = new List<decimal>(){ billedPercentage, nonBilledPercantage },
							BackgroundColor = new List<string>{ theme.GreenColor, theme.LightBlueColor},
							BorderColor = new List<string>{ theme.GreenColor, theme.LightBlueColor}
						}
					};

					ViewBag.Datasets = chartDatasets;
				}
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
