using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.Project.Services;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.Project.Components
{
	[PageComponent(Label = "Project Widget Tasks", Library = "WebVella", Description = "Chart presenting the current project tasks", Version = "0.0.1", IconClass = "fas fa-chart-pie")]
	public class PcProjectWidgetTasks : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcProjectWidgetTasks([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcProjectWidgetTasksOptions
		{

			[JsonProperty(PropertyName = "project_id")]
			public string ProjectId { get; set; } = null;

			[JsonProperty(PropertyName = "user_id")]
			public string UserId { get; set; } = null;
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

				var options = new PcProjectWidgetTasksOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcProjectWidgetTasksOptions>(context.Options.ToString());
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

					Guid? projectId = context.DataModel.GetPropertyValueByDataSource(options.ProjectId) as Guid?;
					Guid? userId = context.DataModel.GetPropertyValueByDataSource(options.UserId) as Guid?;


					var projectTasks = new TaskService().GetTasks(projectId, userId);
					int openTasks = 0;
					int closedTasks = 0;

					var taskStatuses = new TaskService().GetTaskStatuses();
					var closedStatusHashset = new HashSet<Guid>();
					foreach (var taskStatus in taskStatuses)
					{
						if ((bool)taskStatus["is_closed"]) {
							closedStatusHashset.Add((Guid)taskStatus["id"]);
						}
					}
					var overdueTasks = (int)0;
					var dueTodayTasks = (int)0;

					foreach (var task in projectTasks)
					{
						var taskStatus = (Guid)task["status_id"];
						var targetDate = (DateTime?)task["target_date"];
						if (closedStatusHashset.Contains(taskStatus))
							closedTasks++;
						else
							openTasks++;

						if (targetDate != null)
						{
							var erpTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ErpSettings.TimeZoneName);
							targetDate = TimeZoneInfo.ConvertTimeFromUtc(targetDate.Value, erpTimeZone);
							if (targetDate.Value.Date < DateTime.Now.Date)
								overdueTasks++;
							else if (targetDate.Value.Date == DateTime.Now.Date)
								dueTodayTasks++;
						}
					}


					var theme = new Theme();
					var chartDatasets = new List<ErpChartDataset>() {
						new ErpChartDataset(){
							Data = new List<decimal>(){ openTasks, closedTasks },
							BackgroundColor = new List<string>{ theme.PurpleColor, theme.TealColor},
							BorderColor = new List<string>{ theme.PurpleColor, theme.TealColor }
						}
					};

					ViewBag.OpenTasks = openTasks;
					ViewBag.ClosedTasks = closedTasks;
					ViewBag.Datasets = chartDatasets;
					ViewBag.OverdueTasks = overdueTasks;
					ViewBag.DueTodayTasks = dueTodayTasks;
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
						ViewBag.ExceptionMessage = "Unknown component mode";
						return await Task.FromResult<IViewComponentResult>(View("Error"));
				}
			}
			catch (ValidationException ex)
			{
				ViewBag.ExceptionMessage = ex.Message;
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
			catch (Exception ex)
			{
				ViewBag.ExceptionMessage = ex.Message;
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
		}
	}
}
