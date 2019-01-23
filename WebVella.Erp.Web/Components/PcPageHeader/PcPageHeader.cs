using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Components
{
	[PageComponent(Label = "Page header", Library = "WebVella", Description = "Displays the default page header", Version = "0.0.1", IconClass = "fas fa-heading")]
	public class PcPageHeader : PageComponent
	{
		protected ErpRequestContext ErpRequestContext { get; set; }

		public PcPageHeader([FromServices]ErpRequestContext coreReqCtx)
		{
			ErpRequestContext = coreReqCtx;
		}

		public class PcPageHeaderOptions
		{
			[JsonProperty(PropertyName = "color")]
			public string Color { get; set; } = "#bbb";

			[JsonProperty(PropertyName = "icon_color")]
			public string IconColor { get; set; } = "#fff";

			[JsonProperty(PropertyName = "area_label")]
			public string AreaLabel { get; set; } = "Area";

			[JsonProperty(PropertyName = "area_sublabel")]
			public string AreaSubLabel { get; set; } = "";

			[JsonProperty(PropertyName = "title")]
			public string Title { get; set; } = "Page Title";

			[JsonProperty(PropertyName = "subtitle")]
			public string SubTitle { get; set; } = "";

			[JsonProperty(PropertyName = "description")]
			public string Description { get; set; }

			[JsonProperty(PropertyName = "icon_class")]
			public string IconClass { get; set; } = "fa fa-file";

			[JsonProperty(PropertyName = "return_url")]
			public string ReturnUrl { get; set; } = "";

			[JsonProperty(PropertyName = "show_page_switch")]
			public bool ShowPageSwitch { get; set; } = true;

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

				var instanceOptions = new PcPageHeaderOptions();
				if (context.Options != null)
				{
					instanceOptions = JsonConvert.DeserializeObject<PcPageHeaderOptions>(context.Options.ToString());
				}
				var componentMeta = new PageComponentLibraryService().GetComponentMeta(context.Node.ComponentName);
				#endregion


				ViewBag.Options = instanceOptions;
				ViewBag.Node = context.Node;
				ViewBag.ComponentMeta = componentMeta;
				ViewBag.RequestContext = ErpRequestContext;
				ViewBag.AppContext = ErpAppContext.Current;
				ViewBag.InstanceOptions = instanceOptions;
				ViewBag.ComponentContext = context;



				ViewBag.ProccessedTitle = context.DataModel.GetPropertyValueByDataSource(instanceOptions.Title);
				ViewBag.ProccessedSubTitle = context.DataModel.GetPropertyValueByDataSource(instanceOptions.SubTitle);
				ViewBag.ProccessedAreaLabel = context.DataModel.GetPropertyValueByDataSource(instanceOptions.AreaLabel);
				ViewBag.ProccessedAreaSubLabel = context.DataModel.GetPropertyValueByDataSource(instanceOptions.AreaSubLabel);
				ViewBag.ProccessedDescription = context.DataModel.GetPropertyValueByDataSource(instanceOptions.Description);
				ViewBag.ProccessedColor = context.DataModel.GetPropertyValueByDataSource(instanceOptions.Color);
				ViewBag.ProccessedIconColor = context.DataModel.GetPropertyValueByDataSource(instanceOptions.IconColor);
				ViewBag.ProccessedIconClass = context.DataModel.GetPropertyValueByDataSource(instanceOptions.IconClass);

				if (!String.IsNullOrWhiteSpace(instanceOptions.ReturnUrl))
				{
					ViewBag.ProccessedReturnUrl = context.DataModel.GetPropertyValueByDataSource(instanceOptions.ReturnUrl);
				}

				else if (ErpRequestContext != null && ErpRequestContext.PageContext != null && ErpRequestContext.PageContext.HttpContext.Request.Query.ContainsKey("returnUrl")
					&& !String.IsNullOrWhiteSpace(ErpRequestContext.PageContext.HttpContext.Request.Query["returnUrl"]))
				{
					ViewBag.ProccessedReturnUrl = ErpRequestContext.PageContext.HttpContext.Request.Query["returnUrl"].ToString();
				}

				var switchItemPages = new List<ErpPage>();
				var currentSitemapArea = ErpRequestContext.SitemapArea;
				var currentSitemapNode = ErpRequestContext.SitemapNode;
				var currentApp = ErpRequestContext.App;
				if (instanceOptions.ShowPageSwitch && currentPage != null && currentPage.Type == PageType.Site)
				{
					var allPages = new PageService().GetAll();
					switchItemPages = allPages.FindAll(x => x.Weight > 0 && x.Type == currentPage.Type).ToList();
				}
				else if (instanceOptions.ShowPageSwitch && currentPage != null && currentPage.AppId != null && currentPage.Type == PageType.Application)
				{
					var allPages = new PageService().GetAll();
					switchItemPages = allPages.FindAll(x => x.Weight > 0 && x.Type == currentPage.Type && x.AppId == currentApp.Id && x.NodeId == currentPage.NodeId).ToList();
				}
				if (instanceOptions.ShowPageSwitch && currentPage != null && currentSitemapNode != null) {
					var allPages = new PageService().GetAll();
					var sameTypePages = allPages.FindAll(x => x.Weight > 0 && x.Type == currentPage.Type).ToList();
					foreach (var page in sameTypePages)
					{
						if (page.NodeId == context.Node.Id)
						{
							switchItemPages.Add(page);
						}
						else if(currentSitemapNode.Type == SitemapNodeType.EntityList && page.EntityId == currentSitemapNode.EntityId)
						{
							switchItemPages.Add(page);
						}
					}
				}

				switchItemPages = switchItemPages.OrderBy(x => x.Weight).ToList();

				var switchItems = new List<PageSwitchItem>();
				var currentEntity = ErpRequestContext.Entity;
				var parentEntity = ErpRequestContext.ParentEntity;
				var currentUrlTemplate = "/";
				//Site pages
				if (currentPage.Type == PageType.Site)
				{
					currentUrlTemplate = $"/s/[[pageName]]";
				}
				//App pages without nodes
				else if (currentApp != null && currentSitemapNode == null) {
					currentUrlTemplate = $"/{currentApp.Name}/a/[[pageName]]";
				}
				//App pages with sitemap node				
				else if (currentApp != null && currentSitemapArea != null && currentSitemapNode != null)
				{
					//App pages
					if (currentPage.Type == PageType.Application)
					{
						currentUrlTemplate = $"/{currentApp.Name}/{currentSitemapArea.Name}/{currentSitemapNode.Name}/a/[[pageName]]";
					}
					//Record create page, No relation
					else if (currentPage.Type == PageType.RecordCreate && parentEntity == null) {
						currentUrlTemplate = $"/{currentApp.Name}/{currentSitemapArea.Name}/{currentSitemapNode.Name}/c/[[pageName]]";
					}
					//Record create page, With relation
					else if (currentPage.Type == PageType.RecordCreate && parentEntity != null)
					{
						currentUrlTemplate = $"/{currentApp.Name}/{currentSitemapArea.Name}/{currentSitemapNode.Name}/r/{ErpRequestContext.ParentRecordId}/rl/{ErpRequestContext.RelationId}/c/[[pageName]]";
					}
					//Record manage page, No relation
					else if (currentPage.Type == PageType.RecordManage && parentEntity == null)
					{
						currentUrlTemplate = $"/{currentApp.Name}/{currentSitemapArea.Name}/{currentSitemapNode.Name}/m/{ErpRequestContext.RecordId}/[[pageName]]";
					}
					//Record manage page, With relation
					else if (currentPage.Type == PageType.RecordManage && parentEntity != null)
					{
						currentUrlTemplate = $"/{currentApp.Name}/{currentSitemapArea.Name}/{currentSitemapNode.Name}/r/{ErpRequestContext.ParentRecordId}/rl/{ErpRequestContext.RelationId}/m/{ErpRequestContext.RecordId}/[[pageName]]";
					}
					//Record details page, No relation
					else if (currentPage.Type == PageType.RecordDetails && parentEntity == null)
					{
						currentUrlTemplate = $"/{currentApp.Name}/{currentSitemapArea.Name}/{currentSitemapNode.Name}/r/{ErpRequestContext.RecordId}/[[pageName]]";
					}
					//Record details page, With relation
					else if (currentPage.Type == PageType.RecordDetails && parentEntity != null)
					{
						currentUrlTemplate = $"/{currentApp.Name}/{currentSitemapArea.Name}/{currentSitemapNode.Name}/r/{ErpRequestContext.ParentRecordId}/rl/{ErpRequestContext.RelationId}/r/{ErpRequestContext.RecordId}/[[pageName]]";
					}
					//Record list page, No relation
					else if (currentPage.Type == PageType.RecordList && parentEntity == null)
					{
						currentUrlTemplate = $"/{currentApp.Name}/{currentSitemapArea.Name}/{currentSitemapNode.Name}/l/[[pageName]]";
					}
					//Record list page, With relation
					else if (currentPage.Type == PageType.RecordList && parentEntity != null)
					{
						currentUrlTemplate = $"/{currentApp.Name}/{currentSitemapArea.Name}/{currentSitemapNode.Name}/r/{ErpRequestContext.ParentRecordId}/rl/{ErpRequestContext.RelationId}/l/[[pageName]]";
					}
				}

				foreach (var switchPage in switchItemPages)
				{
					var isSelected = false;
					if (currentPage != null && switchPage.Id == currentPage.Id)
						isSelected = true;
					switchItems.Add(new PageSwitchItem() { 
						IsSelected = isSelected,
						Label = switchPage.Label,
						Url = currentUrlTemplate.Replace("[[pageName]]",switchPage.Name)
					});
				}

				ViewBag.PageSwitchItems = switchItems;

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
						ViewBag.Errors = new List<ValidationError>();
						return await Task.FromResult<IViewComponentResult>(View("Error"));
				}
			}
			catch (ValidationException ex)
			{
				ViewBag.ExceptionMessage = ex.Message;
				ViewBag.Errors = new List<ValidationError>();
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
			catch (Exception ex)
			{
				ViewBag.ExceptionMessage = ex.Message;
				ViewBag.Errors = new List<ValidationError>();
				return await Task.FromResult<IViewComponentResult>(View("Error"));
			}
		}
	}
}
