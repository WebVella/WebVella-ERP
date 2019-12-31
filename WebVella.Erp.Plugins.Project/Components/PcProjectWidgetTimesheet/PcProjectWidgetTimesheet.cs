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
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.Project.Components
{
	[PageComponent(Label = "Project Widget Timesheet", Library = "WebVella", Description = "Timesheet for the past 7 days in a project", Version = "0.0.1", IconClass = "fas fa-chart-pie")]
	public class PcProjectWidgetTimesheet : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcProjectWidgetTimesheet([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcProjectWidgetTimesheetOptions
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

				var options = new PcProjectWidgetTimesheetOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcProjectWidgetTimesheetOptions>(context.Options.ToString());
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

					var nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Local);
					List<DateTime> last7Days = Enumerable.Range(0, 7).Select(i => nowDate.AddDays(-6).Date.AddDays(i)).ToList();
					var startDate = new DateTime(last7Days[0].Year, last7Days[0].Month, last7Days[0].Day, 0, 0, 0, DateTimeKind.Local);
					var endDate = nowDate.AddDays(1); //Get End of current date

					var projectTimelogs = new TimeLogService().GetTimelogsForPeriod(projectId,userId,startDate,endDate);

					var users = new UserService().GetAll();

					#region << Generate Grid Columns >>
					var gridColumns = new List<WvGridColumnMeta>() { new WvGridColumnMeta() };
					foreach (var date in last7Days)
					{
						gridColumns.Add(new WvGridColumnMeta() {
							Label = date.ToString("dd MMM"),
							Width = "10%",
							Class= "text-right"
						});
					}
					gridColumns.Add(new WvGridColumnMeta()
					{
						Label = "Total",
						Width = "10%",
						Class = "font-weight-bold text-right"
					});
					ViewBag.GridColumns = gridColumns;
					#endregion

					var records = new List<EntityRecord>(); //id and other fields
					#region << Init Rows >>
					{
						var billableRow = new EntityRecord();
						billableRow["id"] = "billable";
						billableRow["label"] = "Billable";
						billableRow["total"] = (decimal)0;
						records.Add(billableRow);
						var nonbillableRow = new EntityRecord();
						nonbillableRow["id"] = "nonbillable";
						nonbillableRow["label"] = "Non-Billable";
						nonbillableRow["total"] = (decimal)0;
						records.Add(nonbillableRow);
						var totalRow = new EntityRecord();
						totalRow["id"] = "total";
						totalRow["label"] = "Total";
						totalRow["total"] = (decimal)0;
						records.Add(totalRow);
					}
					#endregion

					var timelogsGroupByDate = projectTimelogs.GroupBy(x => (((DateTime?)x["logged_on"]).ConvertToAppDate() ?? DateTime.Now).ToString("dd-MM")).ToList();

					for (int i = 0; i < 7; i++)
					{
						var billableRow = records.First(x => (string)x["id"] == "billable");
						var nonbillableRow = records.First(x => (string)x["id"] == "nonbillable");
						var totalRow = records.First(x => (string)x["id"] == "total");

						billableRow.Properties.Add("day" + (i + 1), (decimal)0);
						nonbillableRow.Properties.Add("day" + (i + 1), (decimal)0);
						totalRow.Properties.Add("day" + (i + 1), (decimal)0);

						var dateString = last7Days[i].ToString("dd-MM");
						var dateLogGroup = timelogsGroupByDate.FirstOrDefault(x => x.Key == dateString);
						if (dateLogGroup != null) {
							var dateLogs = dateLogGroup.ToList();
							foreach (var timelog in dateLogs)
							{
								totalRow["day" + (i + 1)] = (decimal)totalRow["day" + (i + 1)] + (decimal)timelog["minutes"];
								if ((bool)timelog["is_billable"])
								{
									billableRow["day" + (i + 1)] = (decimal)billableRow["day" + (i + 1)] + (decimal)timelog["minutes"];
									billableRow["total"] = (decimal)billableRow["total"] + (decimal)timelog["minutes"];
								}
								else
								{
									nonbillableRow["day" + (i + 1)] = (decimal)nonbillableRow["day" + (i + 1)] + (decimal)timelog["minutes"];
									nonbillableRow["total"] = (decimal)nonbillableRow["total"] + (decimal)timelog["minutes"];
								}
								totalRow["total"] = (decimal)totalRow["total"] + (decimal)timelog["minutes"];
							}
						}
					}

					if (userId == null)
					{
						var timelogsGroupByCreator = projectTimelogs.GroupBy(x => (Guid)x["created_by"]).ToList();
						foreach (var userGroup in timelogsGroupByCreator)
						{
							var user = users.First(x => (Guid)x["id"] == userGroup.Key);
							var imagePath = "/_content/WebVella.Erp.Web/assets/avatar.png";
							if (user["image"] != null && (string)user["image"] != "")
								imagePath = "/fs" + (string)user["image"];

							var userTimelogs = userGroup.ToList();
							var userTimelogsGroupByDate = userTimelogs.GroupBy(x => (((DateTime?)x["logged_on"]).ConvertToAppDate() ?? DateTime.Now).ToString("dd-MM")).ToList();
							var userRow = new EntityRecord();
							userRow["id"] = (string)user["username"];
							userRow["label"] = $"<img src=\"{imagePath}\" class=\"rounded-circle\" width=\"24\"> {(string)user["username"]}";
							userRow["total"] = (decimal)0;

							for (int i = 0; i < 7; i++)
							{
								userRow.Properties.Add("day" + (i + 1), (decimal)0);
								var dateString = last7Days[i].ToString("dd-MM");
								var dateLogGroup = userTimelogsGroupByDate.FirstOrDefault(x => x.Key == dateString);
								if (dateLogGroup != null)
								{
									var dateLogs = dateLogGroup.ToList();
									foreach (var timelog in dateLogs)
									{
										userRow["day" + (i + 1)] = (decimal)userRow["day" + (i + 1)] + (decimal)timelog["minutes"];
										userRow["total"] = (decimal)userRow["total"] + (decimal)timelog["minutes"];
									}
								}
							}
							records.Add(userRow);
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
