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
	[PageComponent(Label = "Project Widget Task Distribution", Library = "WebVella", Description = "Chart presenting the current project opened tasks by user", Version = "0.0.1", IconClass = "fas fa-chart-pie")]
	public class PcProjectWidgetTaskDistribution : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcProjectWidgetTaskDistribution([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcProjectWidgetTaskDistributionOptions
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

				var options = new PcProjectWidgetTaskDistributionOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcProjectWidgetTaskDistributionOptions>(context.Options.ToString());
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
					var projectTasks = new TaskService().GetTaskQueue(projectId,null);

					var users = new UserService().GetAll();
					var userDict = new Dictionary<Guid, EntityRecord>();

					foreach (var task in projectTasks)
					{
						var ownerId = (Guid?)task["owner_id"];
						var taskStatus = (Guid)task["status_id"];
						var endTime = (DateTime?)task["end_time"];


						var userRecord = new EntityRecord();
						userRecord["overdue"] = (int)0;
						userRecord["today"] = (int)0;
						userRecord["other"] = (int)0;
						if (ownerId == null && !userDict.ContainsKey(Guid.Empty)) 
							userDict[Guid.Empty] = userRecord;
						else if (!userDict.ContainsKey(ownerId.Value))
							userDict[ownerId.Value] = userRecord;

						var currentRecord = userDict[ownerId != null ? ownerId.Value : Guid.Empty];

						if (endTime != null)
						{
							if (endTime.Value.AddDays(1) < DateTime.Now.Date)
								currentRecord["overdue"] = ((int)currentRecord["overdue"]) + 1;
							else if (endTime.Value >= DateTime.Now.Date && endTime.Value < DateTime.Now.Date.AddDays(1))
								currentRecord["today"] = ((int)currentRecord["today"]) + 1;
							else
								currentRecord["other"] = ((int)currentRecord["other"]) + 1;
						}
						else {
							currentRecord["other"] = ((int)currentRecord["other"]) + 1;
						}
						userDict[ownerId != null ? ownerId.Value : Guid.Empty] = currentRecord;
					}

					var records = new List<EntityRecord>();
					foreach (var key in userDict.Keys)
					{
						if (key == Guid.Empty)
						{
							var statRecord = userDict[key];
							var row = new EntityRecord();
							var imagePath = "/_content/WebVella.Erp.Web/assets/avatar.png";

							row["user"] = $"<img src=\"{imagePath}\" class=\"rounded-circle\" width=\"24\"> No owner";
							row["overdue"] = statRecord["overdue"];
							row["today"] = statRecord["today"];
							row["other"] = statRecord["other"];
							records.Add(row);
						}
						else
						{
							var user = users.First(x => (Guid)x["id"] == key);
							var statRecord = userDict[key];
							var row = new EntityRecord();
							var imagePath = "/_content/WebVella.Erp.Web/assets/avatar.png";
							if (user["image"] != null && (string)user["image"] != "")
								imagePath = "/fs" + (string)user["image"];

							row["user"] = $"<img src=\"{imagePath}\" class=\"rounded-circle\" width=\"24\"> {(string)user["username"]}";
							row["overdue"] = statRecord["overdue"];
							row["today"] = statRecord["today"];
							row["other"] = statRecord["other"];
							records.Add(row);
						}
					}
					ViewBag.Records = records;
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
