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
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.Project.Components
{
	[PageComponent(Label = "Feed list", Library = "WebVella", Description = "Used for feeds", Version = "0.0.1", IconClass = "fas fa-rss")]
	public class PcFeedList : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcFeedList([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcFeedListOptions
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

				var options = new PcFeedListOptions();
				if (context.Options != null)
				{
					options = JsonConvert.DeserializeObject<PcFeedListOptions>(context.Options.ToString());
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
					var inputRecords = context.DataModel.GetPropertyValueByDataSource(options.Records) as List<EntityRecord> ?? new List<EntityRecord>();
					var groupedRecords = inputRecords.GroupBy(x => ((DateTime)x["created_on"]).ToString("dd MMMM")).ToList();
					var groupedFeedList = new EntityRecord();
					foreach (var feedDate in groupedRecords)
					{
						groupedFeedList[feedDate.Key] = feedDate;
					}
					ViewBag.RecordsJson = JsonConvert.SerializeObject(groupedFeedList);
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
