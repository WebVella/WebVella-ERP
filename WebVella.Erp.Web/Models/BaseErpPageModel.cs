using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Models
{
	[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
	public class BaseErpPageModel : PageModel
	{
		private ErpUser currentUser = null;
		public ErpUser CurrentUser
		{
			get
			{
				if (currentUser == null)
					currentUser = AuthService.GetUser(User);

				return currentUser;
			}
		}

		[BindProperty(SupportsGet = true)]
		public string AppName { get; set; } = "";

		[BindProperty(SupportsGet = true)]
		public string AreaName { get; set; } = "";

		[BindProperty(SupportsGet = true)]
		public string NodeName { get; set; } = "";

		[BindProperty(SupportsGet = true)]
		public string PageName { get; set; } = "";

		[BindProperty(SupportsGet = true)]
		public Guid? RecordId { get; set; } = null;

		[BindProperty(SupportsGet = true)]
		public Guid? RelationId { get; set; } = null;

		[BindProperty(SupportsGet = true)]
		public Guid? ParentRecordId { get; set; } = null;

		public PageDataModel DataModel { get; protected set; } = null;

		public ErpRequestContext ErpRequestContext { get; protected set; }

		public ErpAppContext ErpAppContext { get; protected set; }

		//public List<string> HeaderActions { get; private set; } = new List<string>(); //Convenience property only

		public List<MenuItem> ToolbarMenu { get; private set; } = new List<MenuItem>();

		public List<MenuItem> SidebarMenu { get; private set; } = new List<MenuItem>();

		public List<MenuItem> SiteMenu { get; private set; } = new List<MenuItem>();

		public List<MenuItem> ApplicationMenu { get; private set; } = new List<MenuItem>();

		public List<MenuItem> UserMenu { get; private set; } = new List<MenuItem>();

		public ValidationException Validation { get; private set; } = new ValidationException();

		[BindProperty(Name = "returnUrl", SupportsGet = true)]
		public string ReturnUrl { get; set; } = "";

		public string CurrentUrl { get; set; } = "";

		public string HookKey
		{
			get
			{
				string hookKey = string.Empty;
				if (PageContext.HttpContext.Request.Query.ContainsKey("hookKey"))
					hookKey = HttpContext.Request.Query["hookKey"].ToString();
				return hookKey;
			}
		}

		public IActionResult Init(string appName = "", string areaName = "", string nodeName = "",
						string pageName = "", Guid? recordId = null, Guid? relationId = null, Guid? parentRecordId = null)
		{
			//Stopwatch sw = new Stopwatch();
			//sw.Start();

			if (String.IsNullOrWhiteSpace(appName)) appName = AppName;
			if (String.IsNullOrWhiteSpace(areaName)) areaName = AreaName;
			if (String.IsNullOrWhiteSpace(nodeName)) nodeName = NodeName;
			if (String.IsNullOrWhiteSpace(pageName)) pageName = PageName;
			if (recordId == null) recordId = RecordId;
			if (relationId == null) relationId = RelationId;
			if (parentRecordId == null) parentRecordId = ParentRecordId;

			var urlInfo = new PageService().GetInfoFromPath(HttpContext.Request.Path);
			if (String.IsNullOrWhiteSpace(appName))
			{
				appName = urlInfo.AppName;
				if (AppName != appName)
					AppName = appName; //When dealing with non standard routing in pages
			}
			if (String.IsNullOrWhiteSpace(areaName))
			{
				areaName = urlInfo.AreaName;
				if (AreaName != areaName)
					AreaName = areaName; //When dealing with non standard routing in pages
			}
			if (String.IsNullOrWhiteSpace(nodeName))
			{
				nodeName = urlInfo.NodeName;
				if (NodeName != nodeName)
					NodeName = nodeName; //When dealing with non standard routing in pages
			}
			if (String.IsNullOrWhiteSpace(pageName))
			{
				pageName = urlInfo.PageName;
				if (PageName != pageName)
					PageName = pageName; //When dealing with non standard routing in pages
			}
			if (recordId == null)
			{
				recordId = urlInfo.RecordId;
				if (RecordId != recordId)
					RecordId = recordId; //When dealing with non standard routing in pages
			}
			if (relationId == null)
			{
				relationId = urlInfo.RelationId;
				if (RelationId != relationId)
					RelationId = relationId; //When dealing with non standard routing in pages
			}
			if (parentRecordId == null)
			{
				parentRecordId = urlInfo.ParentRecordId;
				if (ParentRecordId != parentRecordId)
					ParentRecordId = parentRecordId; //When dealing with non standard routing in pages
			}


			ErpRequestContext.SetCurrentApp(appName, areaName, nodeName);
			ErpRequestContext.SetCurrentPage(PageContext, pageName, appName, areaName, nodeName, recordId, relationId, parentRecordId);

			ErpRequestContext.RecordId = recordId;
			ErpRequestContext.RelationId = relationId;
			ErpRequestContext.ParentRecordId = parentRecordId;
			ErpRequestContext.PageContext = PageContext;

			if (PageContext.HttpContext.Request.Query.ContainsKey("returnUrl"))
			{
				ReturnUrl = HttpUtility.UrlDecode(PageContext.HttpContext.Request.Query["returnUrl"].ToString());
			}
			ErpAppContext = ErpAppContext.Current;
			CurrentUrl = PageUtils.GetCurrentUrl(PageContext.HttpContext);

			#region << Init Navigation >>
			//Application navigation
			if (ErpRequestContext.App != null)
			{
				var sitemap = ErpRequestContext.App.Sitemap;
				var appPages = new PageService().GetAppControlledPages(ErpRequestContext.App.Id);
				//Calculate node Urls
				foreach (var area in sitemap.Areas)
				{
					foreach (var currentNode in area.Nodes)
					{
						switch (currentNode.Type)
						{
							case SitemapNodeType.ApplicationPage:
								var nodePages = appPages.FindAll(x => x.NodeId == currentNode.Id).ToList();
								//Case 1: Node has attached pages
								if (nodePages.Count > 0)
								{
									nodePages = nodePages.OrderBy(x => x.Weight).ToList();
									currentNode.Url = $"/{ErpRequestContext.App.Name}/{area.Name}/{currentNode.Name}/a/{nodePages[0].Name}";
								}
								else
								{
									var firstAppPage = appPages.FindAll(x => x.Type == PageType.Application).OrderBy(x => x.Weight).FirstOrDefault();
									if (firstAppPage == null)
										currentNode.Url = $"/{ErpRequestContext.App.Name}/{area.Name}/{currentNode.Name}/a/";
									else
										currentNode.Url = $"/{ErpRequestContext.App.Name}/{area.Name}/{currentNode.Name}/a/{firstAppPage.Name}";
								}
								break;
							case SitemapNodeType.EntityList:
								var firstListPage = appPages.FindAll(x => x.Type == PageType.RecordList && x.EntityId == currentNode.EntityId).OrderBy(x => x.Weight).FirstOrDefault();
								if (firstListPage == null)
									currentNode.Url = $"/{ErpRequestContext.App.Name}/{area.Name}/{currentNode.Name}/l/";
								else
									currentNode.Url = $"/{ErpRequestContext.App.Name}/{area.Name}/{currentNode.Name}/l/{firstListPage.Name}";
								break;
							case SitemapNodeType.Url:
								//Do nothing
								break;
							default:
								throw new Exception("Type not found");
						}
						continue;
					}
				}
				//Convert to MenuItem
				foreach (var area in sitemap.Areas)
				{
					if (area.Nodes.Count == 0)
						continue;

					var areaMenuItem = new MenuItem();
					if (area.Nodes.Count > 1)
					{
						var areaLink = $"<a href=\"javascript: void(0)\" title=\"{area.Label}\" data-navclick-handler>";
						areaLink += $"<span class=\"menu-label\">{area.Label}</span>";
						areaLink += $"<span class=\"menu-nav-icon fa fa-angle-down nav-caret\"></span>";
						areaLink += $"</a>";
						areaMenuItem = new MenuItem()
						{
							Id = area.Id,
							Content = areaLink
						};

						foreach (var node in area.Nodes)
						{
							var nodeLink = "";
							if(!String.IsNullOrWhiteSpace(node.Url)){
								nodeLink = $"<a class=\"dropdown-item\" href=\"{node.Url}\" title=\"{node.Label}\"><span class=\"{node.IconClass} icon fa-fw\"></span>{node.Label}</a>";
							}
							else{
								nodeLink = $"<a class=\"dropdown-item\" href=\"#\" onclick=\"return false\" title=\"{node.Label}\"><span class=\"{node.IconClass} icon fa-fw\"></span>{node.Label}</a>";
							}
							areaMenuItem.Nodes.Add(new MenuItem()
							{
								Content = nodeLink,
								Id = node.Id,
								ParentId = node.ParentId,
								SortOrder = node.Weight
							});
						}
					}
					else if (area.Nodes.Count == 1)
					{
						var areaLink = $"<a href=\"{area.Nodes[0].Url}\" title=\"{area.Label}\">";
						areaLink += $"<span class=\"menu-label\">{area.Label}</span>";
						areaLink += $"</a>";
						areaMenuItem = new MenuItem()
						{
							Content = areaLink,
							Id = area.Nodes[0].Id,
							ParentId = area.Nodes[0].ParentId,
							SortOrder = area.Nodes[0].Weight
						};
					}

					if (ErpRequestContext.SitemapArea == null && ErpRequestContext.Page != null && ErpRequestContext.Page.Type != PageType.Application)
						return new NotFoundResult();

					if (ErpRequestContext.SitemapArea != null && area.Id == ErpRequestContext.SitemapArea.Id)
						areaMenuItem.Class = "current";

					//Process the an unusual case when the area has a node type URL which has a link to an app Page or a site page.
					//Then there is no SitemapArea in the ErpRequest as the URL does not has the information about one but still it needs to be 
					//marked as current
					if (ErpRequestContext.SitemapArea == null)
					{
						var urlNodes = area.Nodes.FindAll(x => x.Type == SitemapNodeType.Url);
						var path = HttpContext.Request.Path;
						foreach (var urlNode in urlNodes)
						{
							if (path == urlNode.Url)
							{
								areaMenuItem.Class = "current";
							}
						}
					}

					ApplicationMenu.Add(areaMenuItem);
				}


			}

			//Site menu
			var pageSrv = new PageService();
			var sitePages = pageSrv.GetSitePages();
			foreach (var sitePage in sitePages)
			{
				if (sitePage.Weight < 1000)
				{
					SiteMenu.Add(new MenuItem()
					{
						Content = $"<a class=\"dropdown-item\" href=\"/s/{sitePage.Name}\">{sitePage.Label}</a>"
					});
				}
			}


			#endregion


			DataModel = new PageDataModel(this);

			List<Guid> currentUserRoles = new List<Guid>();
			if (CurrentUser != null)
				currentUserRoles.AddRange(CurrentUser.Roles.Select(x => x.Id));

			if (ErpRequestContext.App != null)
			{
				if (ErpRequestContext.App.Access == null || ErpRequestContext.App.Access.Count == 0)
					new LocalRedirectResult("/error?401");

				IEnumerable<Guid> rolesWithAccess = ErpRequestContext.App.Access.Intersect(currentUserRoles);
				if (!rolesWithAccess.Any())
					new LocalRedirectResult("/error?401");
			}

			//Debug.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>> Base page init: " + sw.ElapsedMilliseconds);
			return null;
		}

		protected bool RecordsExists()
		{
			if (RecordId.HasValue && DataModel.GetProperty("Record") == null)
				return false;
			if (ParentRecordId.HasValue && DataModel.GetProperty("ParentRecord") == null)
				return false;

			return true;
		}

		protected void ValidateRecordSubmission(EntityRecord postObject, Entity entity, ValidationException validation)
		{
			if (entity == null || postObject == null || postObject.Properties.Count == 0 || validation == null)
				return;

			foreach (var property in postObject.Properties)
			{
				//TODO relations validation
				if (property.Key.StartsWith("$"))
					continue;

				Field fieldMeta = entity.Fields.FirstOrDefault(x => x.Name == property.Key);
				if (fieldMeta != null)
				{
					switch (fieldMeta.GetFieldType())
					{
						//case FieldType.AutoNumberField:
						//	if (property.Value != null && !String.IsNullOrWhiteSpace(property.Value.ToString()))
						//	{
						//		validation.Errors.Add(new ValidationError(property.Key, "Autonumber field value should be null or empty string"));
						//	}
						//	break;
						default:
							if (fieldMeta.Required &&
								(property.Value == null || String.IsNullOrWhiteSpace(property.Value.ToString())))
							{
								validation.Errors.Add(new ValidationError(property.Key, "Required"));
							}
							break;
					}
				}
			}
		}

		public object TryGetDataSourceProperty(string propertyName)
		{
			if (DataModel == null)
				return null;

			var dataSource = DataModel.GetProperty(propertyName);
			if (dataSource != null)
				return dataSource;

			return null;
		}

		public T TryGetDataSourceProperty<T>(string propertyName)
		{
			if (DataModel == null)
				return default(T);

			var dataSource = DataModel.GetProperty(propertyName);
			if (dataSource != null && dataSource is T)
				return (T)dataSource;

			return default(T);
		}

		public static BaseErpPageModel CreatePageModelSimulation(
			ErpRequestContext erpRequestContext,
			ErpUser currentUser
		)
		{
			var pageModel = new BaseErpPageModel();
			pageModel.ErpRequestContext = erpRequestContext;
			pageModel.currentUser = currentUser;
			pageModel.AppName = erpRequestContext.App != null ? erpRequestContext.App.Name : "";
			pageModel.AreaName = erpRequestContext.SitemapArea != null ? erpRequestContext.SitemapArea.Name : "";
			pageModel.NodeName = erpRequestContext.SitemapNode != null ? erpRequestContext.SitemapNode.Name : "";
			pageModel.PageName = erpRequestContext.Page != null ? erpRequestContext.Page.Name : "";
			pageModel.RecordId = erpRequestContext.RecordId;
			pageModel.DataModel = new PageDataModel(pageModel);
			return pageModel;
		}

		public void AddUserMenu(MenuItem menu)
		{
			UserMenu.Add(menu);
			UserMenu = UserMenu.OrderBy(x => x.SortOrder).ToList();
		}

		public void BeforeRender()
		{

			#region << Set BodyClass >>
			ViewData["BodyBorderColor"] = "#555";
			if (ErpRequestContext.App != null && !String.IsNullOrWhiteSpace(ErpRequestContext.App.Color))
			{
				ViewData["BodyBorderColor"] = ErpRequestContext.App.Color;
			}
			if (ToolbarMenu.Count > 0)
			{
				var bodyClass = ViewData.ContainsKey("BodyClass") ? ViewData["BodyClass"].ToString().ToLowerInvariant() : "";
				if (!bodyClass.Contains("has-toolbar"))
				{
					ViewData["BodyClass"] = bodyClass + " has-toolbar ";
				}
			}
			if (SidebarMenu.Count > 0)
			{
				var bodyClass = ViewData.ContainsKey("BodyClass") ? ViewData["BodyClass"].ToString().ToLowerInvariant() : "";
				var classAddon = "";
				if (!bodyClass.Contains("sidebar-"))
				{
					if (CurrentUser != null && !String.IsNullOrWhiteSpace(CurrentUser.Preferences.SidebarSize))
					{
						if (CurrentUser.Preferences.SidebarSize != "lg")
							CurrentUser.Preferences.SidebarSize = "sm";

						classAddon = $" sidebar-{CurrentUser.Preferences.SidebarSize} ";
					}
					else
					{
						classAddon = " sidebar-sm ";
					}
					ViewData["BodyClass"] = bodyClass + classAddon;
				}
			}
			ViewData["AppName"] = ErpSettings.AppName;
			ViewData["SystemMasterBodyStyle"] = "";
			if (!String.IsNullOrWhiteSpace(ErpSettings.SystemMasterBackgroundImageUrl))
			{
				ViewData["SystemMasterBodyStyle"] = "background-image: url('" + ErpSettings.SystemMasterBackgroundImageUrl + "');background-position: top center;background-repeat: repeat;min-height: 100vh; ";
			}
			#endregion
		}

	}
}
