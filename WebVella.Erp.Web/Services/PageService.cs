using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Repositories;
using static WebVella.Erp.Web.Components.PcFieldHtml;
using static WebVella.Erp.Web.Components.PcHtmlBlock;

namespace WebVella.Erp.Web.Services
{
	public class PageService : BaseService
	{
		private string connectionString = ErpSettings.ConnectionString;
		const string CACHE_KEY = "WebVella.Erp.Web.Services.PageService-ALL-PAGES";

		public PageService(string conString = null)
		{
			if (conString != null)
				connectionString = conString;
		}


		#region <--- Page Methods --->

		/// <summary>
		/// Gets all pages list
		/// </summary>
		/// <returns></returns>
		public List<ErpPage> GetAll(NpgsqlTransaction transaction = null, bool useCache = true)
		{
			List<ErpPage> pages;
			if (useCache && ErpAppContext.Current != null)
			{
				pages = ErpAppContext.Current.Cache.Get<List<ErpPage>>(CACHE_KEY);
				if (pages != null)
					return pages;
			}

			pages = new ErpPageRepository(connectionString).GetAllPages(transaction).MapTo<ErpPage>().OrderBy(x => x.Weight).ToList();

			if (useCache && ErpAppContext.Current != null)
				ErpAppContext.Current.Cache.Put(CACHE_KEY, pages);
			return pages;
		}

		/// <summary>
		/// Gets page by for specified page identifier
		/// </summary>
		/// <param name="pageId"></param>
		/// <returns></returns>
		public ErpPage GetPage(Guid pageId, NpgsqlTransaction transaction = null)
		{

			var pages = GetAll(transaction);
			return pages.FirstOrDefault(x => x.Id == pageId);
		}

		/// <summary>
		/// Gets index pages
		/// </summary>
		/// <returns></returns>
		public List<ErpPage> GetIndexPages(NpgsqlTransaction transaction = null)
		{
			var pages = GetAll(transaction);
			return pages.FindAll(x => x.Type == PageType.Home).OrderBy(x => x.Weight).ThenBy(x => x.Label).ToList();
		}

		/// <summary>
		/// Gets site pages
		/// </summary>
		/// <returns></returns>
		public List<ErpPage> GetSitePages(NpgsqlTransaction transaction = null)
		{
			var pages = GetAll(transaction);
			return pages.FindAll(x => x.Type == PageType.Site).OrderBy(x => x.Weight).ThenBy(x => x.Label).ToList();
		}

		/// <summary>
		/// Gets pages for specified application
		/// </summary>
		/// <param name="appId"></param>
		/// <returns></returns>
		public List<ErpPage> GetAppControlledPages(Guid appId, NpgsqlTransaction transaction = null)
		{
			var pages = GetAll(transaction);
			return pages.FindAll(x => x.AppId == appId).OrderBy(x => x.Weight).ThenBy(x => x.Label).ToList();
		}

		/// <summary>
		/// Gets all pages that can be used by an app. This includes pages with the apps ID but also pages 
		/// of entities that are attached to its sitemap nodes and do not have AppId specified
		/// </summary>
		/// <param name="appId"></param>
		/// <returns></returns>
		public List<ErpPage> GetAllPagesAvailableToApp(Guid appId, NpgsqlTransaction transaction = null)
		{
			var app = new AppService(connectionString).GetApplication(appId);

			if (app == null)
			{
				throw new ValidationException() { Message = "App not found" };
			}
			var allPages = GetAll(transaction);
			var appPages = allPages.FindAll(x => x.AppId == appId).ToList();
			var addedEntitiesHashSet = new HashSet<Guid>();
			foreach (var area in app.Sitemap.Areas)
			{
				foreach (var node in area.Nodes)
				{
					if (node.Type == SitemapNodeType.EntityList)
					{
						if (node.EntityId == null)
						{
							throw new ValidationException() { Message = "Node from type SitemapNodeType.EntityList should have an EntityId" };
						}
						if (!addedEntitiesHashSet.Contains(node.EntityId ?? Guid.Empty))
						{
							addedEntitiesHashSet.Add(node.EntityId ?? Guid.Empty);
						}
						var entityPagesWithNoApp = allPages.FindAll(x => x.AppId == null && x.EntityId == node.EntityId).ToList();
						appPages.AddRange(entityPagesWithNoApp);
					}
				}
			}

			appPages = appPages.OrderBy(x => x.Weight).ThenBy(x => x.Label).ToList();
			return appPages;
		}


		/// <summary>
		/// Gets pages for specified entity and page type
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public List<ErpPage> GetEntityPages(Guid entityId, PageType type, NpgsqlTransaction transaction = null)
		{
			if (type == PageType.Application || type == PageType.Site)
				throw new Exception("Wrong page type. Should not be PageType.Application or PageType.Site");

			var pages = GetAll(transaction);
			return pages.FindAll(x => x.EntityId == entityId && x.Type == type).OrderBy(x => x.Weight).ThenBy(x => x.Label).ToList();
		}

		/// <summary>
		/// Gets pages for specified entity and page type
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public List<ErpPage> GetEntityAppPages(Guid entityId, PageType type, Guid? appId, NpgsqlTransaction transaction = null)
		{
			if (type == PageType.Application || type == PageType.Site)
				throw new Exception("Wrong page type. Should not be PageType.Application or PageType.Site");

			var pages = GetAll(transaction);
			return pages.FindAll(x => x.EntityId == entityId && x.Type == type && x.AppId == appId).OrderBy(x => x.Weight).ThenBy(x => x.Label).ToList();
		}


		/// <summary>
		/// Creates new Application Erp Page
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="iconClass"></param>
		/// <param name="system"></param>
		/// <param name="weight"></param>
		/// <param name="type"></param>
		/// <param name="appId"></param>
		/// <param name="entityId"></param>
		/// <param name="nodeId"></param>
		/// <param name="areaId"></param>
		/// <param name="isRazorBody"></param>
		/// <param name="razorBody"></param>
		/// <param name="layout"></param>
		/// <param name="transaction"></param>
		public void CreatePage(Guid id, string name, string label, List<TranslationResource> labelTranslations, string iconClass, bool system, int weight, PageType type,
			Guid? appId, Guid? entityId, Guid? nodeId, Guid? areaId, bool isRazorBody, string razorBody, string layout, NpgsqlTransaction transaction = null)
		{
			var pageRepository = new ErpPageRepository(connectionString);

			ValidationException ex = new ValidationException();

			var page = pageRepository.GetById(id, transaction);
			if (page != null)
				ex.AddError("id", "There is an existing page with specified identifier.");

			if (string.IsNullOrWhiteSpace(name))
				ex.AddError("name", "Page name is not specified.");

			if (string.IsNullOrWhiteSpace(label))
				ex.AddError("label", "Page label is not specified.");

			var homePages = pageRepository.GetByType(PageType.Home);
			if (type == PageType.Home && homePages.Rows.Count > 0)
				ex.AddError("type", "Only one home page can be created");

			ex.CheckAndThrow();

			string lblTr = null;
			if (labelTranslations != null)
				lblTr = JsonConvert.SerializeObject(labelTranslations);

			if (isRazorBody)
				SavePageBodyContentOnFileSystem(id, razorBody ?? string.Empty);

			pageRepository.Insert(id, name, label, lblTr, iconClass, system, weight, (int)type, appId,
				entityId, nodeId, areaId, !string.IsNullOrWhiteSpace(razorBody), razorBody, layout, transaction);

			ClearPagesCache();
		}

		/// <summary>
		/// Updates existing page
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="iconClass"></param>
		/// <param name="system"></param>
		/// <param name="weight"></param>
		/// <param name="type"></param>
		/// <param name="appId"></param>
		/// <param name="entityId"></param>
		/// <param name="nodeId"></param>
		/// <param name="areaId"></param>
		/// <param name="isRazorBody"></param>
		/// <param name="razorBody"></param>
		/// <param name="layout"></param>
		/// <param name="transaction"></param>
		public void UpdatePage(Guid id, string name, string label, List<TranslationResource> labelTranslations, string iconClass, bool system, int weight, PageType type,
			Guid? appId, Guid? entityId, Guid? nodeId, Guid? areaId, bool isRazorBody, string razorBody, string layout, NpgsqlTransaction transaction = null)
		{

			var pageRepository = new ErpPageRepository(connectionString);

			ValidationException ex = new ValidationException();

			var page = pageRepository.GetById(id, transaction);
			if (page == null)
				ex.AddError("id", "There is no page for specified identifier.");

			if (string.IsNullOrWhiteSpace(name))
				ex.AddError("name", "Page name is not specified.");

			if (string.IsNullOrWhiteSpace(label))
				ex.AddError("label", "Page label is not specified.");

			if (isRazorBody && (razorBody == null || !razorBody.Trim().StartsWith("@page")))
			{
				ex.AddError("razorBody", "Body should start with '@page' declaration");
			}

			var homePages = pageRepository.GetByType(PageType.Home);
			if (type == PageType.Home && homePages.Rows.Count > 0 && (Guid)homePages.Rows[0]["id"] != id)
				ex.AddError("type", "Only one home page can be created");

			if (nodeId != null)
			{
				var nodeRow = new SitemapAreaNodeRepository(connectionString).Get(nodeId.Value);
				if (nodeRow == null)
					ex.AddError("node_id", "Specified node cannot be found.");
				else
				{
					//TODO eventually validate type of node and page type matching
					//var node = nodeRow.MapTo<SitemapNode>();

				}
			}

			ex.CheckAndThrow();

			string lblTr = null;
			if (labelTranslations != null)
				lblTr = JsonConvert.SerializeObject(labelTranslations);

			if (isRazorBody)
				SavePageBodyContentOnFileSystem(id, razorBody ?? string.Empty);
			else
				DeletePageBodyContentOnFileSystem(id);

			pageRepository.Update(id, name, label, lblTr, iconClass, system, weight, (int)type, appId,
				entityId, nodeId, areaId, isRazorBody, razorBody, layout, transaction);

			ClearPagesCache();

		}

		/// <summary>
		/// Deletes page
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		/// <param name="cascade"></param>
		public void DeletePage(Guid id, NpgsqlTransaction transaction = null, bool cascade = true)
		{
			var pageRepository = new ErpPageRepository(connectionString);
			ValidationException vex = new ValidationException();

			var page = pageRepository.GetById(id, transaction);
			if (page == null)
				vex.AddError("id", "There is no page for specified identifier.");

			vex.CheckAndThrow();

			if (transaction == null)
			{
				using (NpgsqlConnection con = new NpgsqlConnection(ErpSettings.ConnectionString))
				{
					NpgsqlTransaction trans = null;
					try
					{
						con.Open();
						trans = con.BeginTransaction();

						DeletePageInternal(id, cascade, trans);

						trans.Commit();
					}
					catch (Exception ex)
					{
						if (trans != null)
							trans.Rollback();
						throw ex;
					}
					finally
					{
						con.Close();
					}
				}
			}
			else
			{
				DeletePageInternal(id, cascade, transaction);
			}

			ClearPagesCache();
		}

		/// <summary>
		/// Gets razor body from file system
		/// </summary>
		/// <param name="pageId"></param>
		/// <returns></returns>
		private string GetRazorBodyFromFileSystem(Guid pageId, NpgsqlTransaction transaction = null)
		{
			var pageRepository = new ErpPageRepository(connectionString);
			var pageRow = pageRepository.GetById(pageId, transaction);
			if (pageRow == null)
				throw new ValidationException("Page is not found.");

			var page = pageRow.MapToSingleObject<ErpPage>();

			//Boz - no need for this check as this will alternate both variants
			//if(!page.IsRazorBody)
			//	throw new ValidationException("Page body is not in razor format.");

			var env = ErpAppContext.Current.ServiceProvider.GetService<IWebHostEnvironment>();
			var erpViewsFolderPath = Path.Combine(env.ContentRootPath, "Pages", "WV", "Pages");
			if (!Directory.Exists(erpViewsFolderPath))
				throw new ValidationException("Content folder is not found on file system.");


			var filepath = Path.Combine(erpViewsFolderPath, $"{pageId}.cshtml");
			if (!File.Exists(filepath))
				return "";
			//Just return empty
			//throw new ValidationException("Content file is not found on file system.");

			return File.ReadAllText(filepath);
		}


		/// <summary>
		/// Deletes page and all referenced data 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		private void DeletePageInternal(Guid id, bool cascade, NpgsqlTransaction transaction = null)
		{
			if (cascade)
			{
				DataTable layoutTable = new PageBodyNodeRepository(connectionString).GetPageRootBodyNode(id, transaction);
				foreach (DataRow layoutRow in layoutTable.Rows)
					DeletePageBodyNode((Guid)layoutRow["id"], transaction);

				DataTable pageDataSources = new PageDataSourceRepository(connectionString).GetByPageId(id, transaction);
				foreach (DataRow dr in pageDataSources.Rows)
					DeletePageDataSource((Guid)dr["id"], transaction);
			}

			//delete content if exists
			DeletePageBodyContentOnFileSystem(id);

			new ErpPageRepository(connectionString).Delete(id, transaction);
		}

		/// <summary>
		/// Save page view on file system
		/// </summary>
		/// <param name="pageId"></param>
		/// <param name="content"></param>
		private void SavePageBodyContentOnFileSystem(Guid pageId, string content)
		{
			var env = ErpAppContext.Current.ServiceProvider.GetService<IWebHostEnvironment>();
			var erpViewsFolderPath = Path.Combine(env.ContentRootPath, "Pages", "WV", "Pages");
			if (!Directory.Exists(erpViewsFolderPath))
				Directory.CreateDirectory(erpViewsFolderPath);

			//file is deleted
			var filepath = Path.Combine(erpViewsFolderPath, $"{pageId}.cshtml");
			if (File.Exists(filepath))
				File.Delete(filepath);

			//file is created again if content is provided
			if (!string.IsNullOrWhiteSpace(content))
			{
				using (StreamWriter sw = File.CreateText(filepath))
				{
					sw.Write(content);
					sw.Close();
				}
			}
		}

		/// <summary>
		/// Deletes page view from file system
		/// </summary>
		/// <param name="pageId"></param>
		/// <param name="content"></param>
		private void DeletePageBodyContentOnFileSystem(Guid pageId)
		{
			var env = ErpAppContext.Current.ServiceProvider.GetService<IWebHostEnvironment>();
			var erpViewsFolderPath = Path.Combine(env.ContentRootPath, "Pages", "WV", "Pages");
			var filepath = Path.Combine(erpViewsFolderPath, $"{pageId}.cshtml");
			if (File.Exists(filepath))
				File.Delete(filepath);
		}


		/// <summary>
		/// Clears page related cache
		/// </summary>
		/// <param name="appId"></param>
		public void ClearPagesCache()
		{
			if (ErpAppContext.Current != null)
				ErpAppContext.Current.Cache.Remove(CACHE_KEY);
			//we clear cache for apps also cause pages are loaded into apps 
			new AppService(connectionString).ClearAllAppCache();
		}

		/// <summary>
		/// Unbind sitemap node from all pages
		/// This method is used before page delete
		/// </summary>
		/// <param name="sitemapNodeId"></param>
		/// <param name="transaction"></param>
		public void UnbindPagesFromSitemapNode(Guid sitemapNodeId, NpgsqlTransaction transaction = null)
		{
			new ErpPageRepository(connectionString).UnbindPagesFromSitemapNode(sitemapNodeId, transaction);
		}

		/// <summary>
		/// Unbind sitemap area from all pages
		/// This method is used before page delete
		/// </summary>
		/// <param name="areaId"></param>
		/// <param name="transaction"></param>
		public void UnbindPagesFromSitemapArea(Guid areaId, NpgsqlTransaction transaction = null)
		{
			new ErpPageRepository(connectionString).UnbindPagesFromSitemapArea(areaId, transaction);
		}

		/// <summary>
		/// Clone existing page
		/// if name not specified, it will be generated automatically
		/// </summary>
		/// <param name="pageId"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public ErpPage ClonePage(Guid pageId, string name = null, NpgsqlTransaction transaction = null)
		{
			ErpPage clonedPage = null;
			if (transaction == null)
			{
				using (NpgsqlConnection con = new NpgsqlConnection(ErpSettings.ConnectionString))
				{
					NpgsqlTransaction trans = null;
					try
					{
						con.Open();
						trans = con.BeginTransaction();

						clonedPage = ClonePageInternal(pageId, trans, name);

						trans.Commit();
					}
					catch (Exception ex)
					{
						if (trans != null)
							trans.Rollback();
						throw ex;
					}
					finally
					{
						con.Close();
					}
				}
			}
			else
			{
				clonedPage = ClonePageInternal(pageId, transaction, name);
			}

			ClearPagesCache();
			return clonedPage;
		}

		private ErpPage ClonePageInternal(Guid pageId, NpgsqlTransaction transaction, string name = null)
		{
			ValidationException valEx = new ValidationException();

			var allPages = GetAll(transaction);

			var page = allPages.SingleOrDefault(x => x.Id == pageId);

			if (page == null)
			{
				valEx.AddError("pageId", "Page you try to clone does not exist");
				throw valEx;
			}

			var newPageName = name;
			if (!string.IsNullOrWhiteSpace(name) && allPages.Any(x => x.Name == newPageName))
			{
				valEx.AddError("name", "Page with this name already exists");
				throw valEx;
			}
			else
			{
				int counter = 0;
				newPageName = page.Name + " - Copy";
				do
				{
					if (allPages.Any(x => x.Name == newPageName))
					{
						counter++;
						newPageName = page.Name + " - Copy" + counter;
					}
					else
						break;
				}
				while (true);
			}
			Guid newPageId = Guid.NewGuid();

			CreatePage(newPageId, newPageName, page.Label, page.LabelTranslations, page.IconClass, page.System, page.Weight,
				page.Type, page.AppId, page.EntityId, page.NodeId, page.AreaId, page.IsRazorBody, page.RazorBody, page.Layout, transaction);

			foreach (var node in page.Body)
				ClonePageBodyNodeInternal(newPageId, null, node, transaction);

			var pageDataSources = GetPageDataSources(page.Id);
			foreach (var ds in pageDataSources)
				CreatePageDataSource(Guid.NewGuid(), newPageId, ds.DataSourceId, ds.Name, ds.Parameters, transaction);

			ClearPagesCache();
			return GetPage(newPageId, transaction);
		}

		private void ClonePageBodyNodeInternal(Guid newPageId, Guid? parentNodeId, PageBodyNode node, NpgsqlTransaction transaction)
		{
			Guid newNodeId = Guid.NewGuid();
			CreatePageBodyNode(newNodeId, parentNodeId, newPageId, node.NodeId, node.Weight, node.ComponentName, node.ContainerId, node.Options, transaction);
			foreach (var childNode in node.Nodes)
				ClonePageBodyNodeInternal(newPageId, newNodeId, childNode, transaction);
		}

		#endregion

		#region <--- Page Body --->

		/// <summary>
		/// Gets all body nodes
		/// </summary>
		/// <returns></returns>
		public List<PageBodyNode> GetAllBodyNodes()
		{
			List<PageBodyNode> nodes = new List<PageBodyNode>();
			DataTable dtNodes = new PageBodyNodeRepository(connectionString).GetAllBodyNodes();
			foreach (var dr in dtNodes.Rows)
				nodes.Add(dr.MapTo<PageBodyNode>());

			return nodes;
		}

		/// <summary>
		/// Gets page root body node
		/// </summary>
		/// <param name="pageId"></param>
		/// <returns></returns>
		public List<PageBodyNode> GetPageBody(Guid pageId)
		{
			List<PageBodyNode> nodes = new List<PageBodyNode>();
			DataTable dtNodes = new PageBodyNodeRepository(connectionString).GetPageBodyNodes(pageId);
			foreach (var dr in dtNodes.Rows)
				nodes.Add(dr.MapTo<PageBodyNode>());

			if (nodes.Count == 0)
				return new List<PageBodyNode>();

			Queue<PageBodyNode> processQueue = new Queue<PageBodyNode>();
			var rootNodes = nodes.Where(n => n.ParentId == null).ToList();
			foreach (var rootNode in rootNodes)
				processQueue.Enqueue(rootNode);

			while (processQueue.Count > 0)
			{
				var node = processQueue.Dequeue();
				node.Nodes.AddRange(nodes.Where(n => n.ParentId == node.Id));
				node.Nodes.ForEach(n => processQueue.Enqueue(n));
			}

			return rootNodes;
		}

		/// <summary>
		/// Returns page node by id
		/// </summary>
		/// <param name="nodeId"></param>
		/// <returns></returns>
		public PageBodyNode GetPageNodeById(Guid nodeId)
		{
			PageBodyNodeRepository nodeRep = new PageBodyNodeRepository(connectionString);
			DataRow node = nodeRep.GetById(nodeId);
			if (node != null)
			{
				return node.MapTo<PageBodyNode>();
			}
			else
			{
				throw new Exception("Node you try to get does not exist.");
			}
		}

		/// <summary>
		/// Returns page nodes
		/// </summary>
		/// <param name="pageId"></param>
		/// <returns></returns>
		public List<PageBodyNode> GetPageNodes(Guid pageId)
		{
			List<PageBodyNode> nodes = new List<PageBodyNode>();
			DataTable dtNodes = new PageBodyNodeRepository(connectionString).GetPageBodyNodes(pageId);
			foreach (var dr in dtNodes.Rows)
				nodes.Add(dr.MapTo<PageBodyNode>());

			return nodes;
		}

		/// <summary>
		/// Creates page body node
		/// </summary>
		/// <param name="id"></param>
		/// <param name="parentId"></param>
		/// <param name="pageId"></param>
		/// <param name="nodeId"></param>
		/// <param name="weight"></param>
		/// <param name="componentName"></param>
		/// <param name="containerId"></param>
		/// <param name="options"></param>
		/// <param name="transaction"></param>
		public void CreatePageBodyNode(Guid id, Guid? parentId, Guid pageId, Guid? nodeId, int weight,
			string componentName, string containerId, string options, NpgsqlTransaction transaction = null)
		{
			PageBodyNodeRepository nodeRep = new PageBodyNodeRepository(connectionString);
			DataRow node = nodeRep.GetById(id, transaction);
			if (node != null)
				throw new Exception("Node with same ID already exists.");

			//TODO MORE VALIDATION

			//Problem with serialization of <script> tags
			if(componentName == "PcHtmlBlock"){
				var blockOptions = JsonConvert.DeserializeObject<PcHtmlBlockOptions>(options);
				if(blockOptions.Html.Contains("<script>") || blockOptions.Html.Contains("</script>")){
					throw new Exception("<script> tags are not supported in html block. Please use Javascript block component instead");
				}
			}
			else if(componentName == "PcFieldHtml"){
				var blockOptions = JsonConvert.DeserializeObject<PcFieldHtmlOptions>(options);
				if(blockOptions.Value.Contains("<script>") || blockOptions.Value.Contains("</script>")){
					throw new Exception("<script> tags are not supported in html field. Please use Javascript block component instead");
				}

			}

			new PageBodyNodeRepository(connectionString).Insert(id, parentId, pageId, nodeId, weight, componentName, containerId, options, transaction);

			ClearPagesCache();
		}

		/// <summary>
		/// Updates page body node
		/// </summary>
		/// <param name="id"></param>
		/// <param name="parentId"></param>
		/// <param name="pageId"></param>
		/// <param name="nodeId"></param>
		/// <param name="weight"></param>
		/// <param name="componentName"></param>
		/// <param name="containerId"></param>
		/// <param name="options"></param>
		/// <param name="transaction"></param>
		public void UpdatePageBodyNode(Guid id, Guid? parentId, Guid pageId, Guid? nodeId, int weight,
				string componentName, string containerId, string options, NpgsqlTransaction transaction = null)
		{
			PageBodyNodeRepository nodeRep = new PageBodyNodeRepository(connectionString);
			DataRow node = nodeRep.GetById(id, transaction);
			if (node == null)
				throw new Exception("Node you try to update does not exist.");

			//TODO MORE VALIDATION

			//Problem with serialization of <script> tags
			if(componentName == "PcHtmlBlock"){
				var blockOptions = JsonConvert.DeserializeObject<PcHtmlBlockOptions>(options);
				if(blockOptions.Html.Contains("<script>") || blockOptions.Html.Contains("</script>")){
					throw new Exception("<script> tags are not supported in html block. Please use Javascript block component instead");
				}
			}
			else if(componentName == "PcFieldHtml"){
				var blockOptions = JsonConvert.DeserializeObject<PcFieldHtmlOptions>(options);
				if(blockOptions.Value.Contains("<script>") || blockOptions.Value.Contains("</script>")){
					throw new Exception("<script> tags are not supported in html field. Please use Javascript block component instead");
				}

			}

			nodeRep.Update(id, parentId, pageId, nodeId, weight, componentName, containerId, options, transaction);

			ClearPagesCache();

		}

		/// <summary>
		/// Updates page body node options
		/// </summary>
		/// <param name="id"></param>
		/// <param name="options"></param>
		/// <param name="transaction"></param>
		public void UpdatePageBodyNodeOptions(Guid id, string options, NpgsqlTransaction transaction = null)
		{
			PageBodyNodeRepository nodeRep = new PageBodyNodeRepository(connectionString);
			DataRow node = nodeRep.GetById(id, transaction);
			if (node == null)
				throw new Exception("Node you try to update does not exist.");

			nodeRep.Update(id, options, transaction);

			ClearPagesCache();

		}


		/// <summary>
		/// Deletes page body node
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		/// <param name="cascade"></param>
		public void DeletePageBodyNode(Guid id, NpgsqlTransaction transaction = null, bool cascade = true)
		{
			if (transaction == null)
			{
				using (NpgsqlConnection con = new NpgsqlConnection(ErpSettings.ConnectionString))
				{
					NpgsqlTransaction trans = null;
					try
					{
						con.Open();
						trans = con.BeginTransaction();

						DeletePageBodyNodeInternal(id, cascade, trans);

						trans.Commit();
					}
					catch (Exception ex)
					{
						if (trans != null)
							trans.Rollback();
						throw ex;
					}
					finally
					{
						con.Close();
					}
				}
			}
			else
			{
				DeletePageBodyNodeInternal(id, cascade, transaction);
			}

		}


		public void DeletePageBodyNodeInternal(Guid id, bool cascade, NpgsqlTransaction transaction = null)
		{
			PageBodyNodeRepository nodeRep = new PageBodyNodeRepository(connectionString);

			Stack<Guid> deleteStack = new Stack<Guid>();
			Queue<Guid> processQueue = new Queue<Guid>();

			DataRow pageBodyNode = nodeRep.GetById(id, transaction);
			if (pageBodyNode != null)
			{
				processQueue.Enqueue(id);

				if (cascade)
				{
					// process nodes from parents to children
					while (processQueue.Count > 0)
					{
						Guid nodeId = processQueue.Dequeue();
						deleteStack.Push(nodeId);

						DataTable dtChildNodes = nodeRep.GetPageBodyNodesByParentId(nodeId);
						foreach (DataRow dr in dtChildNodes.Rows)
						{
							Guid childNodeId = (Guid)dr["id"];

							//if deleteStack already contains this nodeId, that mean a cyclic structure exists
							if (deleteStack.Contains(childNodeId))
								throw new Exception($"Cyclic node structure found between: '{id}' and '{childNodeId}' .");

							deleteStack.Push(nodeId);
							processQueue.Enqueue(childNodeId);
						}
					}

					//deletes nodes in order, so we have no problems with references
					while (deleteStack.Count > 0)
					{
						Guid deleteId = deleteStack.Pop();
						nodeRep.Delete(deleteId, transaction);
					}
				}

				nodeRep.Delete(id, transaction);
			}
			else
			{
				throw new Exception("Node you try to delete does not exist.");
			}

			ClearPagesCache();
		}


		#endregion

		#region <--- PageDataSource --->

		static List<string> reservedDataSourcesNames = new List<string> { "CurrentUser", "ReturnUrl", "Record", "RelatedRecord",
		"RecordId", "RelatedRecordId","RelationId","PageContext","Page","ParentPage","Entity","ParentEntity","Message","Validation",
		"Detection","App","SitemapNode","SitemapArea","RowRecord" };

		/// <summary>
		/// Returns all page data sources
		/// </summary>
		/// <param name="pageId"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public List<PageDataSource> GetPageDataSources(Guid pageId, NpgsqlTransaction transaction = null)
		{
			var pageDataSourceRep = new PageDataSourceRepository(connectionString);
			return pageDataSourceRep.GetByPageId(pageId, transaction).Rows.MapTo<PageDataSource>();
		}

		/// <summary>
		/// Gets page data source records for specified data source id
		/// </summary>
		/// <param name="dataSourceId"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public List<PageDataSource> GetPageDataSourcesByDataSourceId(Guid dataSourceId, NpgsqlTransaction transaction = null)
		{
			var pageDataSourceRep = new PageDataSourceRepository(connectionString);
			return pageDataSourceRep.GetByDataSourceId(dataSourceId, transaction).Rows.MapTo<PageDataSource>();
		}

		/// <summary>
		/// Creates new page data source
		/// </summary>
		/// <param name="id"></param>
		/// <param name="pageId"></param>
		/// <param name="dataSourceId"></param>
		/// <param name="name"></param>
		/// <param name="parameters"></param>
		/// <param name="transaction"></param>
		public void CreatePageDataSource(Guid id, Guid pageId, Guid dataSourceId, string name, List<DataSourceParameter> parameters, NpgsqlTransaction transaction = null)
		{
			var pageDataSourceRep = new PageDataSourceRepository(connectionString);
			var existingDataSources = GetPageDataSources(pageId, transaction);
			ValidationException ex = new ValidationException();

			var pds = pageDataSourceRep.GetById(id, transaction);
			if (pds != null)
				ex.AddError("id", "There is an existing page data source with specified identifier.");

			if (string.IsNullOrWhiteSpace(name))
				ex.AddError("name", "Page data source name is not specified.");
			else
			{
				if (existingDataSources.Any(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant()))
					ex.AddError("name", "Page data source with same name already exists.");

				if (reservedDataSourcesNames.Any(x => x.ToLowerInvariant() == name.ToLowerInvariant()))
					ex.AddError("name", "Specified page data source name is reserved.");
			}

			ex.CheckAndThrow();

			string parametersJson = JsonConvert.SerializeObject(parameters ?? new List<DataSourceParameter>());
			pageDataSourceRep.Insert(id, pageId, dataSourceId, name, parametersJson, transaction);
		}

		/// <summary>
		/// Creates new page data source
		/// </summary>
		/// <param name="id"></param>
		/// <param name="pageId"></param>
		/// <param name="dataSourceId"></param>
		/// <param name="name"></param>
		/// <param name="parameters"></param>
		/// <param name="transaction"></param>
		public void CreatePageDataSource(Guid id, Guid pageId, Guid dataSourceId, string name, string parametersJson, NpgsqlTransaction transaction = null)
		{
			var pageDataSourceRep = new PageDataSourceRepository(connectionString);
			var existingDataSources = GetPageDataSources(pageId, transaction);
			ValidationException ex = new ValidationException();

			var pds = pageDataSourceRep.GetById(id, transaction);
			if (pds != null)
				ex.AddError("id", "There is an existing page data source with specified identifier.");

			if (string.IsNullOrWhiteSpace(name))
				ex.AddError("name", "Page data source name is not specified.");
			else
			{
				if (existingDataSources.Any(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant()))
					ex.AddError("name", "Page data source with same name already exists.");

				if (reservedDataSourcesNames.Any(x => x.ToLowerInvariant() == name.ToLowerInvariant()))
					ex.AddError("name", "Specified page data source name is reserved.");
			}

			ex.CheckAndThrow();

			pageDataSourceRep.Insert(id, pageId, dataSourceId, name, parametersJson, transaction);
		}


		/// <summary>
		/// Updates existing page data source
		/// </summary>
		/// <param name="id"></param>
		/// <param name="pageId"></param>
		/// <param name="dataSourceId"></param>
		/// <param name="name"></param>
		/// <param name="parameters"></param>
		/// <param name="transaction"></param>
		public void UpdatePageDataSource(Guid id, Guid pageId, Guid dataSourceId, string name, List<DataSourceParameter> parameters, NpgsqlTransaction transaction = null)
		{
			var pageDataSourceRep = new PageDataSourceRepository(connectionString);
			var existingDataSources = GetPageDataSources(pageId, transaction);
			ValidationException ex = new ValidationException();

			var pds = pageDataSourceRep.GetById(id, transaction);
			if (pds == null)
				ex.AddError("id", "There is no existing page data source with specified identifier.");

			if (string.IsNullOrWhiteSpace(name))
				ex.AddError("name", "Page data source name is not specified.");
			else
			{
				if (existingDataSources.Any(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant() && x.Id != id))
					ex.AddError("name", "Page data source with same name already exists.");

				if (reservedDataSourcesNames.Any(x => x.ToLowerInvariant() == name.ToLowerInvariant()))
					ex.AddError("name", "Specified page data source name is reserved.");
			}
			ex.CheckAndThrow();

			string parametersJson = JsonConvert.SerializeObject(parameters ?? new List<DataSourceParameter>());
			pageDataSourceRep.Update(id, pageId, dataSourceId, name, parametersJson, transaction);
		}

		/// <summary>
		/// Updates existing page data source
		/// </summary>
		/// <param name="id"></param>
		/// <param name="pageId"></param>
		/// <param name="dataSourceId"></param>
		/// <param name="name"></param>
		/// <param name="parametersJson"></param>
		/// <param name="transaction"></param>
		public void UpdatePageDataSource(Guid id, Guid pageId, Guid dataSourceId, string name, string parametersJson, NpgsqlTransaction transaction = null)
		{
			var pageDataSourceRep = new PageDataSourceRepository(connectionString);
			var existingDataSources = GetPageDataSources(pageId, transaction);
			ValidationException ex = new ValidationException();

			var pds = pageDataSourceRep.GetById(id, transaction);
			if (pds == null)
				ex.AddError("id", "There is no existing page data source with specified identifier.");

			if (string.IsNullOrWhiteSpace(name))
				ex.AddError("name", "Page data source name is not specified.");
			else
			{
				if (existingDataSources.Any(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant() && x.Id != id))
					ex.AddError("name", "Page data source with same name already exists.");

				if (reservedDataSourcesNames.Any(x => x.ToLowerInvariant() == name.ToLowerInvariant()))
					ex.AddError("name", "Specified page data source name is reserved.");
			}
			ex.CheckAndThrow();

			pageDataSourceRep.Update(id, pageId, dataSourceId, name, parametersJson, transaction);
		}

		/// <summary>
		/// Deletes page data source
		/// </summary>
		/// <param name="id"></param>
		/// /// <param name="transaction"></param>
		public void DeletePageDataSource(Guid id, NpgsqlTransaction transaction = null)
		{
			PageDataSourceRepository dsRep = new PageDataSourceRepository(connectionString);
			var ds = dsRep.GetById(id, transaction);
			if (ds == null)
				throw new Exception("Page data source you try to delete does not exist.");

			dsRep.Delete(id, transaction);
		}

		#endregion

		public ErpPage GetCurrentPage(PageContext pageContext, string pageName, string appName, string areaName, string nodeName,
			out ErpPage parentPage, Guid? recordId = null, Guid? relationId = null, Guid? parentRecordId = null)
		{
			parentPage = null;

			var pages = GetAll();

			var entMan = new EntityManager();
			var appService = new AppService(connectionString);
			var currentNode = GetCurrentNode(appName, areaName, nodeName);
			var currentApp = appService.GetApplication(appName);
			var urlInfo = GetInfoFromPath(pageContext.HttpContext.Request.Path);
			var hasRelation = urlInfo.HasRelation;
			var pathPageType = urlInfo.PageType;
			if (String.IsNullOrWhiteSpace(appName))
				appName = urlInfo.AppName;
			if (String.IsNullOrWhiteSpace(areaName))
				areaName = urlInfo.AreaName;
			if (String.IsNullOrWhiteSpace(nodeName))
				nodeName = urlInfo.NodeName;
			if (String.IsNullOrWhiteSpace(pageName))
				pageName = urlInfo.PageName;
			if (recordId == null)
				recordId = urlInfo.RecordId;
			if (relationId == null)
				relationId = urlInfo.RelationId;
			if (parentRecordId == null)
				parentRecordId = urlInfo.ParentRecordId;

			Entity currentEntity = null;
			if (currentNode != null && currentNode.Type == SitemapNodeType.EntityList && currentNode.EntityId != null)
			{
				currentEntity = entMan.ReadEntity(currentNode.EntityId ?? Guid.Empty).Object;
			}

			//Record Details, Create, List, Manage priority
			//1. Check Page.NodeId, Page.Name

			#region << Record Pages - WITH Relation >>
			if (hasRelation && parentRecordId != null && relationId != null && currentEntity != null && (pathPageType == PageType.RecordDetails || pathPageType == PageType.RecordCreate
				 || pathPageType == PageType.RecordList || pathPageType == PageType.RecordManage))
			{
				ErpPage resultPage = null;

				if (parentRecordId != null || pathPageType == PageType.RecordList || pathPageType == PageType.RecordCreate)
				{
					//Get Parent page -> Can be the default page or a pageSet in the URL query param "parentPageName"
					#region << Get Parent Page >>
					{
						var parentPageType = PageType.RecordDetails;
						var parentPageName = "";
						if (pageContext.HttpContext.Request.Query.ContainsKey("parentPageName"))
						{
							parentPageName = pageContext.HttpContext.Request.Query["parentPageName"].ToString();
						}

						//Case 1 (named page)
						#region << Case 1 >>>
						if (!String.IsNullOrWhiteSpace(parentPageName))
						{
							// Has Exact Node Match
							parentPage = pages.FirstOrDefault(x => x.Type == parentPageType && x.EntityId == currentEntity.Id
																&& x.NodeId == currentNode.Id && x.Name == parentPageName);
							// No Exact Node Match (general case)
							if (resultPage == null)
							{
								parentPage = pages.FirstOrDefault(x => x.Type == parentPageType && x.EntityId == currentEntity.Id
																	&& x.Name == parentPageName);
							}
						}
						#endregion

						//Case 2 (default entity record details page)
						#region << Case 2 >>>
						else
						{
							// Has Exact Node Match
							parentPage = pages.FirstOrDefault(x => x.Type == parentPageType && x.EntityId == currentEntity.Id
																&& x.NodeId == currentNode.Id);
							// No Exact Node Match (general case)
							if (resultPage == null)
							{
								parentPage = pages.FirstOrDefault(x => x.Type == parentPageType && x.EntityId == currentEntity.Id);
							}
						}
						#endregion
					}
					#endregion

					#region << Get Current Page >>
					{
						var relMan = new EntityRelationManager();
						var currentRelation = relMan.Read(relationId ?? Guid.Empty).Object;
						if (currentRelation != null)
						{
							Entity childEntity = null;
							var childEntityIsOrigin = false;
							if (currentRelation.OriginEntityId == currentEntity.Id)
							{
								childEntity = entMan.ReadEntity(currentRelation.TargetEntityId).Object.MapTo<Entity>();
							}
							else
							{
								childEntityIsOrigin = true;
								childEntity = entMan.ReadEntity(currentRelation.OriginEntityId).Object.MapTo<Entity>();
							}

							//Page should be returned only if the current relation makes sense otherwise throw Error

							if (pathPageType == PageType.RecordCreate && childEntityIsOrigin && currentRelation.RelationType != EntityRelationType.ManyToMany)
							{
								throw new Exception("The child entity can be 'origin' only in n:n relation");
							}
							else if (pathPageType == PageType.RecordCreate && currentRelation.RelationType == EntityRelationType.OneToOne)
							{
								throw new Exception("The relation cannot be from 1:1 type");
							}


							//Case 1 (named page)
							#region << Case 1 >>>
							if (!String.IsNullOrWhiteSpace(pageName))
							{
								resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.EntityId == childEntity.Id
																		&& x.Name == pageName);
							}
							#endregion

							//Case 2 (default entity record details page)
							#region << Case 2 >>>
							else
							{
								resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.EntityId == childEntity.Id);
							}
							#endregion
						}
					}
					#endregion

					if (parentPage != null && resultPage != null)
					{
						return resultPage;
					}
				}
				resultPage = null;
				parentPage = null;
			}
			#endregion

			#region << Record Pages - NO Relation >>
			if (!hasRelation && (pathPageType == PageType.RecordDetails || pathPageType == PageType.RecordCreate
				 || pathPageType == PageType.RecordList || pathPageType == PageType.RecordManage))
			{
				if ((recordId != null || pathPageType == PageType.RecordCreate || pathPageType == PageType.RecordList) && currentEntity != null && currentNode != null)
				{
					ErpPage resultPage = null;

					//Case 1 (named page)
					#region << Case 1 >>>
					if (!String.IsNullOrWhiteSpace(pageName))
					{
						switch (pathPageType)
						{
							case PageType.RecordList:
								if (currentNode.EntityListPages == null || currentNode.EntityListPages.Count == 0)
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
													   && (x.NodeId == currentNode.Id || x.NodeId == null)
													   && x.Name == pageName);
								else
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
									&& (x.NodeId == currentNode.Id || x.NodeId == null)
									&& x.Name == pageName && currentNode.EntityListPages.Contains(x.Id));
								break;
							case PageType.RecordCreate:
								if (currentNode.EntityCreatePages == null || currentNode.EntityCreatePages.Count == 0)
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
													   && (x.NodeId == currentNode.Id || x.NodeId == null)
													   && x.Name == pageName);
								else
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
									&& (x.NodeId == currentNode.Id || x.NodeId == null)
									&& x.Name == pageName && currentNode.EntityCreatePages.Contains(x.Id));
								break;
							case PageType.RecordDetails:
								if (currentNode.EntityDetailsPages == null || currentNode.EntityDetailsPages.Count == 0)
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
													   && (x.NodeId == currentNode.Id || x.NodeId == null)
													   && x.Name == pageName);
								else
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
									&& (x.NodeId == currentNode.Id || x.NodeId == null)
									&& x.Name == pageName && currentNode.EntityDetailsPages.Contains(x.Id));
								break;
							case PageType.RecordManage:
								if (currentNode.EntityManagePages == null || currentNode.EntityManagePages.Count == 0)
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
													   && (x.NodeId == currentNode.Id || x.NodeId == null)
													   && x.Name == pageName);
								else
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
									&& (x.NodeId == currentNode.Id || x.NodeId == null)
									&& x.Name == pageName && currentNode.EntityManagePages.Contains(x.Id));
								break;
							default:
								break;
						}

					}
					#endregion

					//Case 2 (default entity record page) - page Name not given or not found or does not comply to restrictions
					#region << Case 2 >>>
					if (resultPage == null)
					{
						//Check if node has Restrictions and if the page is in these restrictions

						switch (pathPageType)
						{
							case PageType.RecordList:
								if (currentNode.EntityListPages.Count == 0)
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
										&& (x.NodeId == currentNode.Id || x.NodeId == null));
								else
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
										&& (x.NodeId == currentNode.Id || x.NodeId == null)
										&& currentNode.EntityListPages.Contains(x.Id));
								break;
							case PageType.RecordCreate:
								if (currentNode.EntityCreatePages.Count == 0)
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
									   && (x.NodeId == currentNode.Id || x.NodeId == null));
								else
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
										&& (x.NodeId == currentNode.Id || x.NodeId == null)
										&& currentNode.EntityCreatePages.Contains(x.Id));
								break;
							case PageType.RecordDetails:
								if (currentNode.EntityDetailsPages.Count == 0)
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
									   && (x.NodeId == currentNode.Id || x.NodeId == null));
								else
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
										&& (x.NodeId == currentNode.Id || x.NodeId == null)
										&& currentNode.EntityDetailsPages.Contains(x.Id));
								break;
							case PageType.RecordManage:
								if (currentNode.EntityManagePages.Count == 0)
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
									  && (x.NodeId == currentNode.Id || x.NodeId == null));
								else
									resultPage = pages.FirstOrDefault(x => x.Type == pathPageType && x.AppId == currentApp.Id && x.EntityId == currentEntity.Id
									&& (x.NodeId == currentNode.Id || x.NodeId == null)
									&& currentNode.EntityManagePages.Contains(x.Id));
								break;
							default:
								break;
						}
					}
					#endregion

					return resultPage;
				}
			}
			#endregion

			#region << Application Page >>
			if (pathPageType == PageType.Application)
			{
				//Case 1 (named Application Page)
				#region << Case 1 >>
				if (currentNode != null && !String.IsNullOrWhiteSpace(pageName))
				{
					ErpPage resultPage = null;
					//With Exact node match
					resultPage = pages.FirstOrDefault(x => x.Type == PageType.Application && x.AppId == currentApp.Id
												&& x.NodeId == currentNode.Id && x.Name == pageName);
					//No node match
					if (resultPage == null)
					{
						resultPage = pages.FirstOrDefault(x => x.Type == PageType.Application && x.AppId == currentApp.Id
												&& x.Name == pageName);
					}
					return resultPage;
				}
				//default
				else if (currentNode != null && String.IsNullOrWhiteSpace(pageName))
				{
					return pages.FirstOrDefault(x => x.Type == PageType.Application && x.AppId == currentApp.Id
											&& x.NodeId == currentNode.Id);
				}
				#endregion

				//Case 2 (Application Home)
				#region << Case 2 >>
				//named
				if (currentNode == null && !String.IsNullOrWhiteSpace(pageName))
				{
					return pages.FirstOrDefault(x => x.Type == PageType.Application && x.AppId == currentApp.Id
											&& x.Name == pageName);
				}
				//default
				else if (currentNode == null && String.IsNullOrWhiteSpace(pageName))
				{
					return pages.FirstOrDefault(x => x.Type == PageType.Application && x.AppId == currentApp.Id);
				}

				#endregion

			}
			#endregion

			#region << Site Page >>
			if (pathPageType == PageType.Site)
			{
				//Case 1 (SitePage) currentNode:NOT SET, pageName:SET
				if (currentNode == null && !String.IsNullOrWhiteSpace(pageName))
				{
					return pages.FirstOrDefault(x => x.Type == PageType.Site
												&& x.Name == pageName);
				}

				//Case 2 currentNode:NOT SET, pageName:NOT SET
				else if (currentNode == null && String.IsNullOrWhiteSpace(pageName))
				{
					return pages.FirstOrDefault(x => x.Type == PageType.Site);
				}
			}
			#endregion

			#region << Index Page >>
			if (pathPageType == PageType.Home) // Only one home page will be available
			{
				return pages.FirstOrDefault(x => x.Type == PageType.Home);
			}
			#endregion

			return null;
		}

		public EntityRecord ConvertFormPostToEntityRecord(HttpContext httpContext, Guid? recordId, Entity entity = null )
		{
			var resultRecord = new EntityRecord();
			var allEntities = new EntityManager().ReadEntities().Object;

			var fieldTypeDictionary = new Dictionary<string, FieldType>();
			var relationsFieldTypeDictionary = new Dictionary<string, FieldType>();
			var relationsFieldRelationTypeDictionary = new Dictionary<string, EntityRelationType>();

			if (entity == null) return new EntityRecord();

			var entityRelations = new EntityRelationManager().Read().Object.Where(x => (x.OriginEntityId == entity.Id || x.TargetEntityId == entity.Id)).ToList();

			foreach (var field in entity.Fields)
				fieldTypeDictionary[field.Name] = field.GetFieldType();

			foreach (var key in httpContext.Request.Form.Keys)
			{
				if (key == "__RequestVerificationToken")
					continue;

				var postedValue = httpContext.Request.Form[key];

				if (key.StartsWith("$"))
				{
					string[] fieldSect = key.Split(".");
					if (fieldSect.Length < 2)
						continue; //ignore this field

					var relationName = fieldSect[0].Substring(1);
					var relation = entityRelations.SingleOrDefault(x => x.Name.ToLowerInvariant() == relationName.ToLowerInvariant());
					if (relation == null)
						continue; //ignore this field

					Entity relatedEntity = null;
					if (relation.OriginEntityId == entity.Id)
						relatedEntity = allEntities.Single(x => x.Id == relation.TargetEntityId);
					else
						relatedEntity = allEntities.Single(x => x.Id == relation.OriginEntityId);

					Field field = relatedEntity.Fields.SingleOrDefault(x => x.Name == fieldSect[1]);
					if (field == null)
						continue; //ignore this field

					fieldTypeDictionary[key] = FieldType.RelationField;
					relationsFieldTypeDictionary[key] = field.GetFieldType();
					relationsFieldRelationTypeDictionary[key] = relation.RelationType;

				}

				//A new struct Microsoft.Framework.Primitives.StringValues has been introduced to streamline handling of values that may be empty, single strings, or multiple strings. The value is implicitly convertable to and from string and string[], and also provides helpers like Concat and IsNullOrEmpty.
				if (StringValues.IsNullOrEmpty(postedValue))
					resultRecord[key] = "";
				else if (postedValue.Count == 1)
					resultRecord[key] = postedValue.ToString();
				else
					resultRecord[key] = postedValue.ToArray();

			}

			List<string> propertiesToRemove = new List<string>();
			
			var mappedRecord = new EntityRecord();
			foreach (var key in resultRecord.Properties.Keys)
			{
				if (fieldTypeDictionary.ContainsKey(key))
				{
					EntityRelationType? relationType = null;
					FieldType? relatedFieldType = null;
					if (relationsFieldTypeDictionary.ContainsKey(key))
						relatedFieldType = relationsFieldTypeDictionary[key];
					
					if (relationsFieldRelationTypeDictionary.ContainsKey(key))
						relationType = relationsFieldRelationTypeDictionary[key];
					

					try
					{
						MapRecordToModelType(mappedRecord, resultRecord, key, fieldTypeDictionary[key], relatedFieldType, relationType);
					}
					catch(Exception ex)
					{
						//we remove properties (relational type) where value is not from expected type.
						//this is because there are pages with forms which posts custom, non related to entity meta data
						if (ex.Message == "1000")
							propertiesToRemove.Add(key);
						else 
							throw;
					}
				}
			}
			
			foreach (string key in propertiesToRemove)
				resultRecord.Properties.Remove(key);

			if(!resultRecord.Properties.ContainsKey("id") && recordId.HasValue )
				resultRecord["id"] = recordId.Value;

			return mappedRecord;
		}


		public UrlInfo GetInfoFromPath(string path)
		{
			var result = new UrlInfo();
			var pathNodes = path.Split("/");

			if (path == "/")
			{
				//Home	 /
				result.PageType = PageType.Home;
				return result;
			}

			if (pathNodes.Length >= 3 && pathNodes[2].ToLowerInvariant() == "a")
			{
				//Application Home	 /app_name/a/{pageName}
				result.PageType = PageType.Application;
				result.AppName = pathNodes[1].ToLowerInvariant();
				if (pathNodes.Length >= 4)
				{
					result.PageName = pathNodes[3].ToLowerInvariant();
				}
				return result;
			}

			//Site and Plugin pages
			if (pathNodes.Length >= 4 && pathNodes[1].ToLowerInvariant() == "s")
			{
				//Plugin site page /s/{pluginName}/{pageName}
				result.PageType = PageType.Site;
				result.PageName = pathNodes[3].ToLowerInvariant();
				return result;
			}
			else if (pathNodes.Length >= 3 && pathNodes[1].ToLowerInvariant() == "s")
			{
				//Site page	 /s/{pageName}
				result.PageType = PageType.Site;
				result.PageName = pathNodes[2].ToLowerInvariant();
				return result;
			}

			if (pathNodes.Length >= 5)
			{
				if (pathNodes[4].ToLowerInvariant() == "r")
				{
					//Record /app_name/area_name/node_name/r/record_id/pageName? 
					result.AppName = pathNodes[1].ToLowerInvariant();
					result.AreaName = pathNodes[2].ToLowerInvariant();
					result.NodeName = pathNodes[3].ToLowerInvariant();
					result.RecordId = null;
					if (pathNodes.Length >= 6)
					{
						if (Guid.TryParse(pathNodes[5], out Guid outGuid))
						{
							result.RecordId = outGuid;
						}
					}

					//Case 1: Has relation
					if (pathNodes.Length >= 7 && pathNodes[6].ToLowerInvariant() == "rl")
					{
						result.HasRelation = true;
						if (pathNodes.Length >= 7 && Guid.TryParse(pathNodes[7], out Guid outRelationGuid))
							result.RelationId = outRelationGuid;

						if (pathNodes.Length >= 9)
						{
							switch (pathNodes[8].ToLowerInvariant())
							{
								case "l":
									if (pathNodes.Length >= 10)
										result.PageName = pathNodes[9];

									if (pathNodes.Length >= 6 && Guid.TryParse(pathNodes[5], out Guid outParentRecordLGuid))
										result.ParentRecordId = outParentRecordLGuid;
									else
										result.ParentRecordId = null;

									if (pathNodes.Length >= 10 && Guid.TryParse(pathNodes[9], out Guid outRecordLGuid))
										result.RecordId = outRecordLGuid;
									else
										result.RecordId = null;

									result.PageType = PageType.RecordList;
									return result;
								case "c":
									if (pathNodes.Length >= 10)
										result.PageName = pathNodes[9];

									if (pathNodes.Length >= 6 && Guid.TryParse(pathNodes[5], out Guid outParentRecordCGuid))
										result.ParentRecordId = outParentRecordCGuid;
									else
										result.ParentRecordId = null;

									if (pathNodes.Length >= 10 && Guid.TryParse(pathNodes[9], out Guid outRecordCGuid))
										result.RecordId = outRecordCGuid;
									else
										result.RecordId = null;

									result.PageType = PageType.RecordCreate;
									return result;

								case "r":
									if (pathNodes.Length >= 11)
										result.PageName = pathNodes[10];

									if (pathNodes.Length >= 6 && Guid.TryParse(pathNodes[5], out Guid outParentRecordRGuid))
										result.ParentRecordId = outParentRecordRGuid;
									else
										result.ParentRecordId = null;

									if (pathNodes.Length >= 10 && Guid.TryParse(pathNodes[9], out Guid outRecordRGuid))
										result.RecordId = outRecordRGuid;
									else
										result.RecordId = null;

									result.PageType = PageType.RecordDetails;
									return result;

								case "m":
									if (pathNodes.Length >= 11)
										result.PageName = pathNodes[10];

									if (pathNodes.Length >= 6 && Guid.TryParse(pathNodes[5], out Guid outParentRecordMGuid))
										result.ParentRecordId = outParentRecordMGuid;
									else
										result.ParentRecordId = null;

									if (pathNodes.Length >= 10 && Guid.TryParse(pathNodes[9], out Guid outRecordMGuid))
										result.RecordId = outRecordMGuid;
									else
										result.RecordId = null;

									result.PageType = PageType.RecordManage;
									return result;
								default:
									//Unknown relation url structure
									result.PageType = PageType.RecordDetails;
									return result;
							}
						}
						else
						{
							//Unknown relation url structure
							result.PageType = PageType.RecordDetails;
							return result;
						}
					}
					//Case 2: No relation
					else
					{
						if (pathNodes.Length >= 7)
						{
							result.PageName = pathNodes[6];
						}
						result.PageType = PageType.RecordDetails;
						return result;
					}

				}
				else if (pathNodes[4].ToLowerInvariant() == "c")
				{
					//Create /app_name/area_name/node_name/c/pageName?
					result.PageType = PageType.RecordCreate;
					result.AppName = pathNodes[1].ToLowerInvariant();
					result.AreaName = pathNodes[2].ToLowerInvariant();
					result.NodeName = pathNodes[3].ToLowerInvariant();
					if (pathNodes.Length >= 6)
					{
						result.PageName = pathNodes[5].ToLowerInvariant();
					}
				}
				else if (pathNodes[4].ToLowerInvariant() == "m")
				{
					//Manage /app_name/area_name/node_name/m/pageName?
					result.PageType = PageType.RecordManage;
					result.AppName = pathNodes[1].ToLowerInvariant();
					result.AreaName = pathNodes[2].ToLowerInvariant();
					result.NodeName = pathNodes[3].ToLowerInvariant();
					if (pathNodes.Length >= 6)
					{
						result.PageName = pathNodes[5].ToLowerInvariant();
					}
				}
				else if (pathNodes[4].ToLowerInvariant() == "l")
				{
					//List	/app_name/area_name/node_name/l/pageName?
					result.PageType = PageType.RecordList;
					result.AppName = pathNodes[1].ToLowerInvariant();
					result.AreaName = pathNodes[2].ToLowerInvariant();
					result.NodeName = pathNodes[3].ToLowerInvariant();
					if (pathNodes.Length >= 6)
					{
						result.PageName = pathNodes[5].ToLowerInvariant();
					}
				}
				else if (pathNodes[4].ToLowerInvariant() == "a")
				{
					//Application	/app_name/area_name/node_name/a/{pageName}
					result.PageType = PageType.Application;
					result.AppName = pathNodes[1].ToLowerInvariant();
					result.AreaName = pathNodes[2].ToLowerInvariant();
					result.NodeName = pathNodes[3].ToLowerInvariant();
					if (pathNodes.Length >= 6)
					{
						result.PageName = pathNodes[5].ToLowerInvariant();
					}
					return result;
				}
			}


			return result;
		}

		public List<ErpPage> GetCurrentPageSiblings(ErpRequestContext requestContext)
		{
			var result = new List<ErpPage>();
			var currentPage = requestContext.Page;
			if (currentPage != null)
			{
				switch (currentPage.Type)
				{
					case PageType.Home:
						return GetIndexPages().FindAll(x => x.Id != currentPage.Id).ToList();
					case PageType.Site:
						return GetSitePages().FindAll(x => x.Id != currentPage.Id).ToList();
					case PageType.Application:
						{
							var currentApp = requestContext.App;
							if (currentApp != null)
							{
								var appPages = GetAppControlledPages(currentApp.Id);
								appPages = appPages.FindAll(x => x.Id != currentPage.Id && x.Type == PageType.Application).ToList();

								if (currentPage.AreaId != null)
								{
									appPages = appPages.FindAll(x => x.AreaId == currentPage.AreaId).ToList();
								}
								else if (requestContext.SitemapArea != null)
								{
									appPages = appPages.FindAll(x => x.AreaId == requestContext.SitemapArea.Id).ToList();
								}
								else
								{
									appPages = appPages.FindAll(x => x.AreaId == null).ToList();
								}

								if (currentPage.NodeId != null)
								{
									appPages = appPages.FindAll(x => x.NodeId == currentPage.NodeId).ToList();
								}
								else if (requestContext.SitemapNode != null)
								{
									appPages = appPages.FindAll(x => x.NodeId == requestContext.SitemapNode.Id).ToList();
								}
								else
								{
									appPages = appPages.FindAll(x => x.NodeId == null).ToList();
								}

								return appPages;
							}
						}
						break;
					case PageType.RecordList:
						{
							var currentEntity = requestContext.Entity;
							if (currentEntity != null)
							{
								return GetEntityPages(currentEntity.Id, PageType.RecordList).FindAll(x => x.Id != currentPage.Id).ToList();
							}
						}
						break;
					case PageType.RecordDetails:
						{
							var currentEntity = requestContext.Entity;
							if (currentEntity != null)
							{
								return GetEntityPages(currentEntity.Id, PageType.RecordDetails).FindAll(x => x.Id != currentPage.Id).ToList();
							}
						}
						break;
					default:
						break;

				}
			}
			return result;
		}


		#region <--- Private Utility Methods --->

		private void MapRecordToModelType(EntityRecord mappedRecord, EntityRecord resultRecord, string key, FieldType fieldType, FieldType? relatedFieldType, EntityRelationType? relationType )
		{
			switch (fieldType)
			{
				case FieldType.AutoNumberField:
					if (Decimal.TryParse(resultRecord[key].ToString(), out decimal outAutoNumber))
						mappedRecord[key] = outAutoNumber;
					else
						mappedRecord[key] = null;
					break;
				case FieldType.CheckboxField:
					if (Boolean.TryParse(resultRecord[key].ToString(), out bool outCheckbox))
						mappedRecord[key] = outCheckbox;
					else
						mappedRecord[key] = null;
					break;
				case FieldType.CurrencyField:
					if (Decimal.TryParse(resultRecord[key].ToString(), out decimal outCurrency))
						mappedRecord[key] = outCurrency;
					else
						mappedRecord[key] = null;
					break;
				case FieldType.DateField:
					if (DateTime.TryParse(resultRecord[key].ToString(), out DateTime outDate))
						mappedRecord[key] = outDate;
					else
						mappedRecord[key] = null;
					break;
				case FieldType.DateTimeField:
					if (DateTime.TryParse(resultRecord[key].ToString(), out DateTime outDateTime))
						mappedRecord[key] = outDateTime;
					else
						mappedRecord[key] = null;
					break;
				case FieldType.EmailField:
					mappedRecord[key] = resultRecord[key];
					break;
				case FieldType.FileField:
					mappedRecord[key] = resultRecord[key];
					break;
				case FieldType.HtmlField:
					mappedRecord[key] = resultRecord[key];
					break;
				case FieldType.ImageField:
					mappedRecord[key] = resultRecord[key];
					break;
				case FieldType.MultiLineTextField:
					mappedRecord[key] = resultRecord[key];
					break;
				case FieldType.MultiSelectField:
					{
						if (resultRecord[key] is string)
							mappedRecord[key] = new List<object> { resultRecord[key] };
						else if (resultRecord[key] is Array)
							mappedRecord[key] = resultRecord[key];
						else
							mappedRecord[key] = null;
					}
					break;
				case FieldType.NumberField:
					if (Decimal.TryParse(resultRecord[key].ToString(), out decimal outNumber))
						mappedRecord[key] = outNumber;
					else
						mappedRecord[key] = null;
					break;
				case FieldType.PasswordField:
					mappedRecord[key] = resultRecord[key];
					break;
				case FieldType.PercentField:
					if (Decimal.TryParse(resultRecord[key].ToString(), out decimal outPercent))
						mappedRecord[key] = outPercent;
					else
						mappedRecord[key] = null;
					break;
				case FieldType.PhoneField:
					mappedRecord[key] = resultRecord[key];
					break;
				case FieldType.GuidField:
					if (Guid.TryParse(resultRecord[key].ToString(), out Guid outGuid))
						mappedRecord[key] = outGuid;
					else
						mappedRecord[key] = null;
					break;
				case FieldType.SelectField:
					mappedRecord[key] = resultRecord[key];
					break;
				case FieldType.TextField:
					mappedRecord[key] = resultRecord[key];
					break;
				case FieldType.UrlField:
					mappedRecord[key] = resultRecord[key];
					break;
				case FieldType.RelationField:
					{
						if (relatedFieldType == null)
							throw new Exception("1000");

						var value = resultRecord[key];
						bool isArray = value.GetType().IsArray;
						switch (relatedFieldType.Value)
						{
							case FieldType.GuidField:
								{
									if (relationType.HasValue && relationType == EntityRelationType.ManyToMany)
									{
										List<Guid> list = new List<Guid>();
										if (isArray)
										{
											foreach (var guidString in (string[])value)
											{
												if (Guid.TryParse(guidString, out Guid guidValue))
													list.Add(guidValue);
											}
										}
										else
										{
											if (Guid.TryParse(value.ToString(), out Guid guidValue))
												list.Add(guidValue);
										}
										mappedRecord[key] = list;
									}
									else
									{
										if (Guid.TryParse(value.ToString(), out Guid guidValue))
											mappedRecord[key] = guidValue.ToString();
									}
								}
								break;
							default:
								throw new Exception("1000");
						}
					}
					break;
			}
		}

		private SitemapNode GetCurrentNode(string appName, string areaName, string nodeName)
		{
			if (String.IsNullOrWhiteSpace(appName) || String.IsNullOrWhiteSpace(areaName) || String.IsNullOrWhiteSpace(nodeName))
			{
				return null;
			}
			var currentApp = new AppService(connectionString).GetApplication(appName);
			if (currentApp == null) return null;

			var currentArea = currentApp.Sitemap.Areas.FirstOrDefault(x => x.Name == areaName);
			if (currentArea == null) return null;

			var currentNode = currentArea.Nodes.FirstOrDefault(x => x.Name == nodeName);
			return currentNode;
		}

		#endregion



	}
}
