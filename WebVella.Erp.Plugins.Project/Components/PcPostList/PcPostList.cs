using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.Project.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.Project.Components
{
	[PageComponent(Label = "Project Post list", Library = "WebVella", Description = "Used for comments, feeds, logs etc.", Version = "0.0.1", IconClass = "far fa-comments")]
	public class PcPostList : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcPostList([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcPostListOptions
		{
			[JsonProperty(PropertyName = "records")]
			public string Records { get; set; } = "";

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

				var options = new PcPostListOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcPostListOptions>(context.Options.ToString());
				}

				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

				ViewBag.Options = options;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.ComponentContext = context;
				ViewBag.CurrentUser = SecurityContext.CurrentUser;
				ViewBag.CurrentUserJson = JsonConvert.SerializeObject(SecurityContext.CurrentUser);

				if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help)
				{
					var relatedRecordId = (Guid?)context.DataModel.GetProperty("Record.id");
					if (relatedRecordId == null)
						throw new Exception("Record.id datasource returns null");

					var entityName = (string)context.DataModel.GetProperty("Entity.Name");
					var relatedRecords = new List<Guid>();
					relatedRecords.Add(relatedRecordId.Value);
					switch (entityName)
					{
						case "task":
							{
								var eqlCommand = "SELECT $project_nn_task.id FROM task WHERE id = @recordId";
								var eqlParams = new List<EqlParameter>() { new EqlParameter("recordId", relatedRecordId) };
								var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();
								if (eqlResult.Any())
								{
									var taskRecord = eqlResult[0];
									if (taskRecord.Properties.ContainsKey("$project_nn_task") && taskRecord["$project_nn_task"] != null)
									{
										if (((List<EntityRecord>)taskRecord["$project_nn_task"]).Any())
										{
											var projectRecord = ((List<EntityRecord>)taskRecord["$project_nn_task"])[0];
											if (projectRecord.Properties.ContainsKey("id") && projectRecord["id"] != null)
											{
												relatedRecords.Add((Guid)projectRecord["id"]);
											}
										}
									}
								}
							}
							break;
						default:
							break;
					}

					ViewBag.RelatedRecordId = relatedRecordId;
					ViewBag.RelatedRecordsJson = JsonConvert.SerializeObject(relatedRecords);

					var inputRecords = context.DataModel.GetPropertyValueByDataSource(options.Records) as List<EntityRecord> ?? new List<EntityRecord>();

					var treeRecords = EntityRecordUtils.ConvertRecordListToTree(input: inputRecords, result: new List<EntityRecord>(), parentId: null,
						parentIdFieldName: "parent_id", createdDateFieldName: "created_on", sortOrder: "asc");
					ViewBag.Records = treeRecords;
					ViewBag.RecordsJson = JsonConvert.SerializeObject(treeRecords);
					HttpContext httpContext = null;
					if (ErpRequestContext.PageContext != null)
					{
						httpContext = ErpRequestContext.PageContext.HttpContext;
					}
					ViewBag.SiteRootUrl = UrlUtils.FullyQualifiedApplicationPath(httpContext);
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
