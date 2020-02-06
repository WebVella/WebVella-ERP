using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Plugins.SDK.Model;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Controllers
{
	[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
	public class AdminController : Controller
	{
		private const char RELATION_SEPARATOR = '.';
		private const char RELATION_NAME_RESULT_SEPARATOR = '$';

		RecordManager recMan;
		EntityManager entMan;
		EntityRelationManager relMan;
		SecurityManager secMan;
		IErpService erpService;

		public AdminController(IErpService erpService)
		{
			recMan = new RecordManager();
			secMan = new SecurityManager();
			entMan = new EntityManager();
			relMan = new EntityRelationManager();
			this.erpService = erpService;
		}

		#region << Data Source >>
		//[AllowAnonymous] //Just for webcomponent dev
		[Route("api/v3.0/p/sdk/datasource/list")]
		[HttpGet]
		public ActionResult DataSourceAction()
		{
			DataSourceManager dataSourceManager = new DataSourceManager();
			var dsList = dataSourceManager.GetAll();
			dsList = dsList.OrderBy(x => x.Name).ToList();
			return Json(dsList);
		}


		#endregion

		#region << Sitemap Area >>
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3.0/p/sdk/sitemap/area")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult CreateSitemapArea([FromBody]SitemapArea area, [FromQuery]Guid? appId = null)
		{
			var response = new ResponseModel();
			response.Message = "Success";
			response.Success = true;

			if (area == null)
			{
				response.Message = "Wrong object model submitted. Could not restore!";
				response.Success = false;
				return Json(response);
			}
			if (appId == null)
			{
				response.Message = "Application Id needs to be submitted as 'appId' query string";
				response.Success = false;
				return Json(response);
			}

			if (area.Id == Guid.Empty)
			{
				area.Id = Guid.NewGuid();
			}

			var appSrv = new AppService();

			try
			{
				appSrv.CreateArea(area.Id, appId ?? Guid.Empty, area.Name, area.Label, area.LabelTranslations, area.Description, area.DescriptionTranslations,
					area.IconClass, area.Color, area.Weight, area.ShowGroupNames, area.Access);
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.Success = false;
				return Json(response);
			}

			var newSitemap = appSrv.GetApplication(appId ?? Guid.Empty).Sitemap;
			var initData = new EntityRecord();
			initData["sitemap"] = appSrv.OrderSitemap(newSitemap);
			initData["node_page_dict"] = PageUtils.GetNodePageDictionary(appId);
			response.Object = initData;

			return Json(response);
		}

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3.0/p/sdk/sitemap/area/{areaId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UpdateSitemapArea([FromBody]SitemapArea area, [FromQuery]Guid? appId = null)
		{
			var response = new ResponseModel();
			response.Message = "Success";
			response.Success = true;

			if (area == null)
			{
				response.Message = "Wrong object model submitted. Could not restore!";
				response.Success = false;
				return Json(response);
			}
			if (appId == null)
			{
				response.Message = "Application Id needs to be submitted as 'appId' query string";
				response.Success = false;
				return Json(response);
			}

			if (area.Id == null || area.Id == Guid.Empty)
			{
				response.Message = "Area Id needs to be submitted";
				response.Success = false;
				return Json(response);
			}

			var appSrv = new AppService();

			try
			{
				appSrv.UpdateArea(area.Id, appId ?? Guid.Empty, area.Name, area.Label, area.LabelTranslations, area.Description, area.DescriptionTranslations,
					area.IconClass, area.Color, area.Weight, area.ShowGroupNames, area.Access);
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.Success = false;
				return Json(response);
			}

			var newSitemap = appSrv.GetApplication(appId ?? Guid.Empty).Sitemap;
			var initData = new EntityRecord();
			initData["sitemap"] = appSrv.OrderSitemap(newSitemap);
			initData["node_page_dict"] = PageUtils.GetNodePageDictionary(appId);
			response.Object = initData;

			return Json(response);
		}

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3.0/p/sdk/sitemap/area/{areaId}/delete")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult DeleteSitemapArea(Guid areaId, [FromQuery]Guid? appId = null)
		{
			var response = new ResponseModel();
			response.Message = "Success";
			response.Success = true;

			if (areaId == null || areaId == Guid.Empty)
			{
				response.Message = "Area Id needs to be submitted";
				response.Success = false;
				return Json(response);
			}

			if (appId == null)
			{
				response.Message = "Application Id needs to be submitted as 'appId' query string";
				response.Success = false;
				return Json(response);
			}

			var appSrv = new AppService();

			try
			{
				appSrv.DeleteArea(areaId);
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.Success = false;
				return Json(response);
			}

			var newSitemap = appSrv.GetApplication(appId ?? Guid.Empty).Sitemap;
			var initData = new EntityRecord();
			initData["sitemap"] = appSrv.OrderSitemap(newSitemap);
			initData["node_page_dict"] = PageUtils.GetNodePageDictionary(appId);
			response.Object = initData;

			return Json(response);
		}
		#endregion

		#region << Sitemap Node >>
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3.0/p/sdk/sitemap/node")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult CreateSitemapNode([FromBody]SitemapNodeSubmit node, [FromQuery]Guid? appId = null, [FromQuery]Guid? areaId = null)
		{
			var response = new ResponseModel();
			response.Message = "Success";
			response.Success = true;

			if (node == null)
			{
				response.Message = "Wrong object model submitted. Could not restore!";
				response.Success = false;
				return Json(response);
			}
			if (appId == null)
			{
				response.Message = "Application Id needs to be submitted as 'appId' query string";
				response.Success = false;
				return Json(response);
			}
			if (areaId == null)
			{
				response.Message = "Area Id needs to be submitted as 'areaId' query string";
				response.Success = false;
				return Json(response);
			}

			if (node.Id == Guid.Empty)
			{
				node.Id = Guid.NewGuid();
			}

			var appSrv = new AppService();
			var pageSrv = new PageService();
			try
			{
				appSrv.CreateAreaNode(node.Id, areaId ?? Guid.Empty, node.Name, node.Label, node.LabelTranslations,
					node.IconClass, node.Url, (int)node.Type, node.EntityId, node.Weight, node.Access, node.EntityListPages, 
					node.EntityCreatePages, node.EntityDetailsPages, node.EntityManagePages,null,node.ParentId);
				if (node.Pages == null)
				{
					node.Pages = new List<Guid>();
				}
				foreach (var pageId in node.Pages)
				{
					var page = pageSrv.GetPage(pageId);
					if (page == null)
					{
						throw new Exception("Page not found");
					}
					pageSrv.UpdatePage(page.Id, page.Name, page.Label, page.LabelTranslations, page.IconClass, page.System, page.Weight,
						page.Type, page.AppId, page.EntityId, node.Id, areaId, page.IsRazorBody, page.RazorBody, page.Layout);
				}
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.Success = false;
				return Json(response);
			}

			var newSitemap = appSrv.GetApplication(appId ?? Guid.Empty).Sitemap;
			var initData = new EntityRecord();
			initData["sitemap"] = appSrv.OrderSitemap(newSitemap);
			initData["node_page_dict"] = PageUtils.GetNodePageDictionary(appId);
			response.Object = initData;

			return Json(response);
		}

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3.0/p/sdk/sitemap/node/{nodeId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UpdateSitemapNode([FromBody]SitemapNodeSubmit node, [FromQuery]Guid? appId = null, [FromQuery]Guid? areaId = null)
		{
			var response = new ResponseModel();
			response.Message = "Success";
			response.Success = true;

			if (node == null)
			{
				response.Message = "Wrong object model submitted. Could not restore!";
				response.Success = false;
				return Json(response);
			}
			if (appId == null)
			{
				response.Message = "Application Id needs to be submitted as 'appId' query string";
				response.Success = false;
				return Json(response);
			}

			if (areaId == null)
			{
				response.Message = "Area Id needs to be submitted as 'areaId' query string";
				response.Success = false;
				return Json(response);
			}

			if (node.Id == null || node.Id == Guid.Empty)
			{
				response.Message = "Node Id needs to be submitted";
				response.Success = false;
				return Json(response);
			}

			var appSrv = new AppService();
			var pageSrv = new PageService();
			try
			{
				appSrv.UpdateAreaNode(node.Id, areaId ?? Guid.Empty, node.Name, node.Label, node.LabelTranslations,
					node.IconClass, node.Url, (int)node.Type, node.EntityId, node.Weight, node.Access, 
					node.EntityListPages, node.EntityCreatePages, node.EntityDetailsPages, node.EntityManagePages,
					null,node.ParentId);

				var allAppPages = pageSrv.GetAppControlledPages(appId ?? Guid.Empty);

				var currentAttachedNodePages = allAppPages.FindAll(x => x.NodeId == node.Id).Select(x => x.Id).ToList();
				var currentAttachedPagesHashSet = new HashSet<Guid>();
				foreach (var pageId in currentAttachedNodePages)
				{
					currentAttachedPagesHashSet.Add(pageId);
				}

				//Process submitted page Ids
				if (node.Pages == null)
					node.Pages = new List<Guid>();
				foreach (var pageId in node.Pages)
				{
					var page = pageSrv.GetPage(pageId);
					if (page == null)
					{
						throw new Exception("Page not found");
					}
					if (page.NodeId == null)
					{
						pageSrv.UpdatePage(page.Id, page.Name, page.Label, page.LabelTranslations, page.IconClass, page.System, page.Weight,
							page.Type, page.AppId, page.EntityId, node.Id, areaId, page.IsRazorBody, page.RazorBody, page.Layout);
					}
					else if (page.NodeId == node.Id)
					{
						currentAttachedPagesHashSet.Remove(page.Id);
					}
				}

				//Detach pages that were not submitted
				foreach (var pageId in currentAttachedPagesHashSet)
				{
					var page = pageSrv.GetPage(pageId);
					if (page == null)
					{
						throw new Exception("Page not found");
					}
					pageSrv.UpdatePage(page.Id, page.Name, page.Label, page.LabelTranslations, page.IconClass, page.System, page.Weight,
						page.Type, page.AppId, page.EntityId, null, null, page.IsRazorBody, page.RazorBody, page.Layout);
				}

			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.Success = false;
				return Json(response);
			}

			var newSitemap = appSrv.GetApplication(appId ?? Guid.Empty).Sitemap;
			var initData = new EntityRecord();
			initData["sitemap"] = appSrv.OrderSitemap(newSitemap);
			initData["node_page_dict"] = PageUtils.GetNodePageDictionary(appId);
			response.Object = initData;

			return Json(response);
		}

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3.0/p/sdk/sitemap/node/{nodeId}/delete")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult DeleteSitemapNode(Guid nodeId, [FromQuery]Guid? appId = null)
		{
			var response = new ResponseModel();
			response.Message = "Success";
			response.Success = true;

			if (nodeId == null || nodeId == Guid.Empty)
			{
				response.Message = "Node Id needs to be submitted";
				response.Success = false;
				return Json(response);
			}

			if (appId == null)
			{
				response.Message = "Application Id needs to be submitted as 'appId' query string";
				response.Success = false;
				return Json(response);
			}

			var appSrv = new AppService();

			try
			{
				appSrv.DeleteAreaNode(nodeId);
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.Success = false;
				return Json(response);
			}

			var newSitemap = appSrv.GetApplication(appId ?? Guid.Empty).Sitemap;
			var initData = new EntityRecord();
			initData["sitemap"] = appSrv.OrderSitemap(newSitemap);
			initData["node_page_dict"] = PageUtils.GetNodePageDictionary(appId);
			response.Object = initData;

			return Json(response);
		}

		//[AllowAnonymous] //Needed only when webcomponent development
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3.0/p/sdk/sitemap/node/get-aux-info")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetNodeAuxData([FromQuery]Guid? appId = null)
		{
			var response = new ResponseModel();
			response.Message = "Success";
			response.Success = true;

			if (appId == null)
			{
				response.Message = "Application Id needs to be submitted as 'appId' query string";
				response.Success = false;
				return Json(response);
			}

			try
			{
				var responseObject = new EntityRecord();
				var entitiesSelectOptions = new List<SelectOption>();
				var appPageRecords = new List<EntityRecord>();
				var allEntityPageRecords = new List<EntityRecord>();
				var typesSelectOptions = new List<SelectOption>();
				var entities = new EntityManager().ReadEntities().Object;
				foreach (var entity in entities)
				{
					var selectOption = new SelectOption()
					{
						Value = entity.Id.ToString(),
						Label = entity.Name
					};
					entitiesSelectOptions.Add(selectOption);
				}
				entitiesSelectOptions = entitiesSelectOptions.OrderBy(x => x.Label).ToList();

				entitiesSelectOptions.Insert(0, new SelectOption() { Value = "", Label = "not attached" });
				responseObject["all_entities"] = entitiesSelectOptions;



				foreach (var typeEnum in Enum.GetValues(typeof(SitemapNodeType)).Cast<SitemapNodeType>())
				{
					var selectOption = new SelectOption()
					{
						Value = ((int)typeEnum).ToString(),
						Label = typeEnum.GetLabel()
					};
					typesSelectOptions.Add(selectOption);
				}
				responseObject["node_types"] = typesSelectOptions.OrderBy(x => x.Label).ToList();

				var pageService = new PageService();
				//Get App pages
				var appPages = pageService.GetAppControlledPages(appId.Value);
				var allAppPagesWithoutNodes = appPages.FindAll(x => x.NodeId == null && x.Type == PageType.Application).OrderBy(x => x.Name).ToList();
				foreach (var page in allAppPagesWithoutNodes)
				{
					var selectOption = new EntityRecord();
					selectOption["page_id"] = page.Id.ToString();
					selectOption["page_name"] = page.Name;
					selectOption["node_id"] = page.NodeId != null ? (page.NodeId ?? Guid.Empty).ToString() : "";
					appPageRecords.Add(selectOption);
				}
				responseObject["app_pages"] = appPageRecords.OrderBy(x => (string)x["page_name"]).ToList();

				//Get EntityPages
				var allEntityPages = pageService.GetAll();
				foreach (var page in allEntityPages)
				{
					if (page.EntityId != null && page.AppId == appId.Value)
					{
						var selectOption = new EntityRecord();
						selectOption["page_id"] = page.Id.ToString();
						selectOption["page_name"] = page.Name;
						selectOption["entity_id"] = page.EntityId;
						selectOption["type"] = ((int)page.Type).ToString();
						selectOption["node_id"] = page.NodeId != null ? (page.NodeId ?? Guid.Empty).ToString() : "";
						allEntityPageRecords.Add(selectOption);
					}
				}
				responseObject["all_entity_pages"] = allEntityPageRecords.OrderBy(x => (string)x["page_name"]).ToList();

				response.Object = responseObject;
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.Success = false;
				return Json(response);
			}

			return Json(response);
		}


		#endregion
	}


}