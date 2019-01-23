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
	[PageComponent(Label = "Project Widget Overdue Tasks", Library = "WebVella", Description = "overdue tasks for a project", Version = "0.0.1", IconClass = "fas fa-chart-pie")]
	public class PcProjectWidgetOverdueTasks : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcProjectWidgetOverdueTasks([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcProjectWidgetOverdueTasksOptions
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

				var options = new PcProjectWidgetOverdueTasksOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcProjectWidgetOverdueTasksOptions>(context.Options.ToString());
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

					var projectRecord = new ProjectService().Get(projectId);
					var projectTasks = new TaskService().GetTasks(projectId,null);

					var overdueTasks = new List<EntityRecord>();
					var users = new UserService().GetAll();

					foreach (var task in projectTasks)
					{
						var targetDate = (DateTime?)task["target_date"];

						if (targetDate != null)
						{
							var erpTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ErpSettings.TimeZoneName);
							targetDate = TimeZoneInfo.ConvertTimeFromUtc(targetDate.Value, erpTimeZone);
							if (targetDate.Value.Date < DateTime.Now.Date)
							{
								var user = users.First(x => (Guid)x["id"] == (Guid)task["owner_id"]);
								var imagePath = "/assets/avatar.png";
								if (user["image"] != null && (string)user["image"] != "")
									imagePath = "/fs" + (string)user["image"];

								string iconClass = "";
								string color = "";
								new TaskService().GetTaskIconAndColor((string)task["priority"],out iconClass, out color);

								var row = new EntityRecord();
								row["task"] = $"<i class='{iconClass}' style='color:{color}'></i> <a target=\"_blank\" href=\"/projects/tasks/tasks/r/{(Guid)task["id"]}/details\">[{task["key"]}] {task["subject"]}</a>";
								row["user"] = $"<img src=\"{imagePath}\" class=\"rounded-circle\" width=\"24\"> {(string)user["username"]}";
								row["date"] = targetDate;
								overdueTasks.Add(row);
							}
						}
					}
					ViewBag.Records = overdueTasks;
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
