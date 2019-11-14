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
			[JsonProperty(PropertyName = "is_visible")]
			public string IsVisible { get; set; } = "";

			[JsonProperty(PropertyName = "color")]
			public string Color { get; set; } = "{\"type\":\"0\",\"string\":\"App.Color\",\"default\":\"\"}";

			[JsonProperty(PropertyName = "icon_color")]
			public string IconColor { get; set; } = "#fff";

			[JsonProperty(PropertyName = "area_label")]
			public string AreaLabel { get; set; } = "{\"type\":\"0\",\"string\":\"Entity.LabelPlural\",\"default\":\"\"}";

			[JsonProperty(PropertyName = "area_sublabel")]
			public string AreaSubLabel { get; set; } = "";

			[JsonProperty(PropertyName = "title")]
			public string Title { get; set; } = "{\"type\":\"0\",\"string\":\"Page.Label\",\"default\":\"\"}";

			[JsonProperty(PropertyName = "subtitle")]
			public string SubTitle { get; set; } = "";

			[JsonProperty(PropertyName = "description")]
			public string Description { get; set; }

			[JsonProperty(PropertyName = "icon_class")]
			public string IconClass { get; set; } = "{\"type\":\"0\",\"string\":\"Entity.IconName\",\"default\":\"\"}";

			[JsonProperty(PropertyName = "return_url")]
			public string ReturnUrl { get; set; } = "";

			[JsonProperty(PropertyName = "show_page_switch")]
			public bool ShowPageSwitch { get; set; } = false;

            [JsonProperty(PropertyName = "fix_on_scroll")]
            public bool FixOnScroll { get; set; } = false;

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

                if (context.Mode != ComponentMode.Options && context.Mode != ComponentMode.Help)
                {
                    var isVisible = true;
                    var isVisibleDS = context.DataModel.GetPropertyValueByDataSource(instanceOptions.IsVisible);
                    if (isVisibleDS is string && !String.IsNullOrWhiteSpace(isVisibleDS.ToString()))
                    {
                        if (Boolean.TryParse(isVisibleDS.ToString(), out bool outBool))
                        {
                            isVisible = outBool;
                        }
                    }
                    else if (isVisibleDS is Boolean)
                    {
                        isVisible = (bool)isVisibleDS;
                    }
                    ViewBag.IsVisible = isVisible;

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
                        switchItemPages = allPages.FindAll(x => x.Weight < 1000 && x.Type == currentPage.Type).ToList();
                    }
                    else if (instanceOptions.ShowPageSwitch && currentPage != null && currentPage.AppId != null && currentPage.Type == PageType.Application)
                    {
                        var allPages = new PageService().GetAppControlledPages(currentPage.AppId.Value);
                        switchItemPages = allPages.FindAll(x => x.Weight < 1000 && x.Type == currentPage.Type && x.AppId == currentApp.Id && x.NodeId == currentPage.NodeId).ToList();
                    }
                    if (instanceOptions.ShowPageSwitch && currentPage != null && currentSitemapNode != null)
                    {
                        var allPossiblePages = new PageService().GetAll().FindAll(x => x.Type == currentPage.Type
                            && x.EntityId == currentSitemapNode.EntityId && (x.AppId == null || x.AppId == currentPage.AppId.Value)).ToList();

                        switch (currentPage.Type)
                        {
                            case PageType.RecordList:
                                if (currentSitemapNode.EntityListPages != null && currentSitemapNode.EntityListPages.Count > 0)
                                    allPossiblePages = allPossiblePages.FindAll(x => currentSitemapNode.EntityListPages.Contains(x.Id) && x.Weight < 1000).ToList();
                                break;
                            case PageType.RecordCreate:
                                if (currentSitemapNode.EntityCreatePages != null && currentSitemapNode.EntityCreatePages.Count > 0)
                                    allPossiblePages = allPossiblePages.FindAll(x => currentSitemapNode.EntityCreatePages.Contains(x.Id) && x.Weight < 1000).ToList();
                                break;
                            case PageType.RecordDetails:
                                if (currentSitemapNode.EntityDetailsPages != null && currentSitemapNode.EntityDetailsPages.Count > 0)
                                    allPossiblePages = allPossiblePages.FindAll(x => currentSitemapNode.EntityDetailsPages.Contains(x.Id) && x.Weight < 1000).ToList();
                                break;
                            case PageType.RecordManage:
                                if (currentSitemapNode.EntityManagePages != null && currentSitemapNode.EntityManagePages.Count > 0)
                                    allPossiblePages = allPossiblePages.FindAll(x => currentSitemapNode.EntityManagePages.Contains(x.Id) && x.Weight < 1000).ToList();
                                break;
                            default:
                                break;
                        }

                        foreach (var page in allPossiblePages)
                        {
                            switchItemPages.Add(page);
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
                    else if (currentApp != null && currentSitemapNode == null)
                    {
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
                        else if (currentPage.Type == PageType.RecordCreate && parentEntity == null)
                        {
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
                        switchItems.Add(new PageSwitchItem()
                        {
                            IsSelected = isSelected,
                            Label = switchPage.Label,
                            Url = currentUrlTemplate.Replace("[[pageName]]", switchPage.Name)
                        });
                    }

                    ViewBag.PageSwitchItems = switchItems;
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
