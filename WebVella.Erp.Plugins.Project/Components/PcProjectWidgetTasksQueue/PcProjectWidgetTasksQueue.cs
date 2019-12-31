using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.Project.Model;
using WebVella.Erp.Plugins.Project.Services;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.Project.Components
{
	[PageComponent(Label = "Project Widget Tasks Queue", Library = "WebVella", Description = "tasks queue for a project or an user", Version = "0.0.1", IconClass = "fas fa-chart-pie")]
	public class PcProjectWidgetTasksQueue : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcProjectWidgetTasksQueue([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcProjectWidgetTasksQueueOptions
		{

			[JsonProperty(PropertyName = "project_id")]
			public string ProjectId { get; set; } = null;

			[JsonProperty(PropertyName = "user_id")]
			public string UserId { get; set; } = null;

			[JsonProperty(PropertyName = "type")]
			public TasksDueType Type { get; set; } = TasksDueType.All;
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

				var options = new PcProjectWidgetTasksQueueOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcProjectWidgetTasksQueueOptions>(context.Options.ToString());
				}

				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion


				ViewBag.Options = options;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.ComponentContext = context;
				ViewBag.TypeOptions = ModelExtensions.GetEnumAsSelectOptions<TasksDueType>();

				if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help)
				{

					Guid? projectId = context.DataModel.GetPropertyValueByDataSource(options.ProjectId) as Guid?;

					Guid? userId = context.DataModel.GetPropertyValueByDataSource(options.UserId) as Guid?;
					var limit = options.Type == TasksDueType.EndTimeNotDue ? 10 : 50;
					var taskQueue = new TaskService().GetTaskQueue(projectId, userId, options.Type, limit);

					var users = new UserService().GetAll();

					var resultRecords = new List<EntityRecord>();

					foreach (var task in taskQueue)
					{
						var imagePath = "/_content/WebVella.Erp.Web/assets/avatar.png";
						var user = new EntityRecord();
						user["username"] = "No Owner";
						if (task["owner_id"] != null)
						{
							user = users.First(x => (Guid)x["id"] == (Guid)task["owner_id"]);
							if (user["image"] != null && (string)user["image"] != "")
								imagePath = "/fs" + (string)user["image"];
						}
						string iconClass = "";
						string color = "";
						new TaskService().GetTaskIconAndColor((string)task["priority"],out iconClass, out color);

						var row = new EntityRecord();
						row["task"] = $"<i class='{iconClass}' style='color:{color}'></i> <a href=\"/projects/tasks/tasks/r/{(Guid)task["id"]}/details\">[{task["key"]}] {task["subject"]}</a>";
						row["user"] = $"<img src=\"{imagePath}\" class=\"rounded-circle\" width=\"24\"> {(string)user["username"]}";
						row["date"] = ((DateTime?)task["end_time"]).ConvertToAppDate();
						resultRecords.Add(row);
					}
					ViewBag.Records = resultRecords;
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
