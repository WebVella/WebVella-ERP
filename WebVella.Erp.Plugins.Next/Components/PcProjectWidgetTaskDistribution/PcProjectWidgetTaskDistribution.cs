using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.Next.Services;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.Next.Components
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
					return await Task.FromResult<IViewComponentResult>(Content("Error: The node Id is required to be set as query param 'nid', when requesting this component"));
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
					var projectTasks = new TaskService().GetTasks(projectId,null);
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

					var users = new UserService().GetAll();
					var userDict = new Dictionary<Guid, EntityRecord>();

					foreach (var task in projectTasks)
					{
						var ownerId = (Guid)task["owner_id"];
						var taskStatus = (Guid)task["status_id"];
						var targetDate = (DateTime?)task["target_date"];

						if (!userDict.ContainsKey(ownerId)) {
							var userRecord = new EntityRecord();
							userRecord["overdue"] = (int)0;
							userRecord["today"] = (int)0;
							userRecord["open"] = (int)0;
							userRecord["all"] = (int)0;
							userDict[ownerId] = userRecord;
						}

						var currentRecord = userDict[ownerId];
						currentRecord["all"] = ((int)currentRecord["all"])+1;
						if (!closedStatusHashset.Contains(taskStatus))
							currentRecord["open"] = ((int)currentRecord["open"]) + 1;

						if (targetDate != null) {
							var erpTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ErpSettings.TimeZoneName);
							targetDate = TimeZoneInfo.ConvertTimeFromUtc(targetDate.Value, erpTimeZone);
							if (targetDate.Value.Date < DateTime.Now.Date)
								currentRecord["overdue"] = ((int)currentRecord["overdue"]) + 1;
							else if (targetDate.Value.Date == DateTime.Now.Date)
								currentRecord["today"] = ((int)currentRecord["today"]) + 1;
						}
						userDict[ownerId] = currentRecord;
					}

					var records = new List<EntityRecord>();
					foreach (var key in userDict.Keys)
					{
						var user = users.First(x => (Guid)x["id"] == key);
						var statRecord = userDict[key];
						var row = new EntityRecord();
						var imagePath = "/assets/avatar.png";
						if (user["image"] != null && (string)user["image"] != "")
							imagePath = "/fs" + (string)user["image"];

						row["user"] = $"<img src=\"{imagePath}\" class=\"rounded-circle\" width=\"24\"> {(string)user["username"]}";
						row["overdue"] = statRecord["overdue"];
						row["today"] = statRecord["today"];
						row["open"] = statRecord["open"];
						row["all"] = statRecord["all"];
						records.Add(row);
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
