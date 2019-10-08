using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Application list", Library = "WebVella", Description = "List of all available application", Version = "0.0.1", IconClass = "fas fa-th")]
	public class PcApplications : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcApplications([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcApplicationsOptions
		{

			//[JsonProperty(PropertyName = "is_visible")]
			//public string IsVisible { get; set; } = "";
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

				var instanceOptions = new PcApplicationsOptions();
				if (context.Options != null)
				{
					instanceOptions = JsonConvert.DeserializeObject<PcApplicationsOptions>(context.Options.ToString());
				}

				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion

				ViewBag.Options = instanceOptions;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.ComponentContext = context;

				if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help) {
                    var currentUser = AuthService.GetUser(HttpContext.User);

                    var currentUserRoles = currentUser.Roles.Select(x => x.Id);
                    var apps = new AppService().GetAllApplications().OrderBy(x => x.Weight).ToList();
                    var allowedApps = new List<App>();
                    if (apps != null)
                    {
                        foreach (var app in apps)
                        {
                            if (app.Access == null || app.Access.Count == 0)
                                continue;

                            IEnumerable<Guid> accessRoles = app.Access.Intersect(currentUserRoles);
                            if (accessRoles.Any())
                                allowedApps.Add(app);
                        }
                    }
                    //Generate Url
                    foreach (var app in allowedApps)
					{
						app.HomePages = app.HomePages.FindAll(x => x.Weight < 1000).OrderBy(x => x.Weight).ToList();
						foreach (var area in app.Sitemap.Areas)
						{
							foreach (var node in area.Nodes)
							{
								node.Url = PageUtils.GenerateSitemapNodeUrl(node, area, app);
							}
						}
						app.Sitemap = new AppService().OrderSitemap(app.Sitemap);
					}
					ViewBag.Apps = allowedApps;
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
