using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Repositories;

namespace WebVella.Erp.Web.Services
{
	public class AppService : BaseService
	{
		const string CACHE_KEY = "WebVella.Erp.Web.Services.AppService";
		private string connectionString = ErpSettings.ConnectionString;
		private AppRepository repository;

		public AppService( string conString = null )
		{
			if (conString != null)
				connectionString = conString;

			repository = new AppRepository(connectionString);
		}

		#region <--- Application --->

		/// <summary>
		/// Get all applications
		/// </summary>
		/// <returns></returns>
		public List<App> GetAllApplications( bool useCache = true)
		{
			var apps = repository.GetAllCompleteAppJson().MapTo<App>();
			if (useCache)
			{
				foreach (var app in apps)
					ErpAppContext.Current.Cache.Put($"{CACHE_KEY}-{app.Id}", app);
			}
			return apps;
		}

		/// <summary>
		/// Get application by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public App GetApplication(Guid id)
		{
			App app = ErpAppContext.Current.Cache.Get<App>($"{CACHE_KEY}-{id}");
			if (app == null)
			{
				JToken data = repository.GetCompleteAppJson(id);
				if (data == null)
					return null;

				app = data.MapToSingleObject<App>();

				if( app != null)
					ErpAppContext.Current.Cache.Put($"{CACHE_KEY}-{id}", app);
			}

			return app;
		}

		/// <summary>
		/// Get application by name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public App GetApplication(string name)
		{
			Guid? appId = repository.GetAppIdByName(name);
			if (appId == null)
				return null;

			return GetApplication(appId.Value);
		}

		/// <summary>
		/// Creates new application
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="description"></param>
		/// <param name="iconClass"></param>
		/// <param name="author"></param>
		/// <param name="color"></param>
		/// <param name="weight"></param>
		/// <param name="access"></param>
		/// <param name="transaction"></param>
		public void CreateApplication(Guid id, string name, string label, string description,
			string iconClass, string author, string color, int weight, List<Guid> access, NpgsqlTransaction transaction = null)
		{
			ValidationException ex = new ValidationException();

			var app = repository.GetById(id, transaction);
			if (app != null)
				ex.AddError("id", "There is an existing application with same identifier.");

			if (string.IsNullOrWhiteSpace(name))
				ex.AddError("name", "Application name is not specified.");
			else
			{
				app = repository.GetByName(name, transaction);
				if (app != null)
					ex.AddError("name", "There is an existing application with same name. Application name has to be unique.");
			}

			if (string.IsNullOrWhiteSpace(label))
				ex.AddError("label", "Application label is not specified.");

			ex.CheckAndThrow();

			repository.InsertApplication(id, name, label, description, iconClass, author, color, weight, access, transaction);
		}

		/// <summary>
		/// Updates existing application 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="description"></param>
		/// <param name="iconClass"></param>
		/// <param name="author"></param>
		/// <param name="color"></param>
		/// <param name="weight"></param>
		/// <param name="access"></param>
		/// <param name="transaction"></param>
		public void UpdateApplication(Guid id, string name, string label, string description,
			string iconClass, string author, string color, int weight, List<Guid> access, NpgsqlTransaction transaction = null)
		{
			ValidationException ex = new ValidationException();

			var app = repository.GetById(id, transaction);
			if (app == null)
				ex.AddError("id", "There is no application with specified identifier.");

			if (string.IsNullOrWhiteSpace(name))
				ex.AddError("name", "Application name is not specified.");
			else
			{
				app = repository.GetByName(name, transaction);
				if (app != null && ((Guid)app["id"]) != id)
					ex.AddError("name", "There is an existing application with same name. Application name has to be unique.");
			}

			if (string.IsNullOrWhiteSpace(label))
				ex.AddError("label", "Application label is not specified.");

			ex.CheckAndThrow();

			repository.UpdateApplication(id, name, label, description, iconClass, author, color, weight, access, transaction);

			ClearAppCache(id);
		}

		/// <summary>
		/// Deletes application with all related data
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		public void DeleteApplication(Guid id, NpgsqlTransaction transaction = null, bool cascade = true )
		{
			ValidationException vex = new ValidationException();

			var app = repository.GetById(id, transaction);
			if (app == null)
				vex.AddError("id", "There is no application with specified identifier.");
			
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

						DeleteApplicationInternal(id, cascade, trans);

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
				DeleteApplicationInternal(id, cascade, transaction);
			}
		}

		/// <summary>
		/// Deletes application - wrap transactional code 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		private void DeleteApplicationInternal(Guid id, bool cascade, NpgsqlTransaction transaction)
		{
			if (cascade)
			{
				//delete application pages
				PageService pageService = new PageService(connectionString);
				DataTable appPages = new ErpPageRepository(connectionString).GetApplicationPages(id, transaction);
				foreach (DataRow pageRow in appPages.Rows)
					pageService.DeletePage((Guid)pageRow["id"], transaction);

				//delete application areas
				DataTable appAreas = new SitemapAreaRepository(connectionString).GetApplicationAreas(id, transaction);
				foreach (DataRow entRow in appAreas.Rows)
					DeleteArea((Guid)entRow["id"], transaction);
			}

			//delete application
			repository.DeleteApplication(id, transaction);

			ClearAppCache(id);
		}

		/// <summary>
		/// Clears application related cache
		/// </summary>
		/// <param name="appId"></param>
		public void ClearAppCache(Guid appId)
		{
			ErpAppContext.Current.Cache.Remove($"{CACHE_KEY}-{appId}");
		}

		/// <summary>
		/// Clears all application related cache
		/// </summary>
		/// <param name="appId"></param>
		public void ClearAllAppCache()
		{
			List<App> apps = GetAllApplications();
			foreach(var app in apps )
				ErpAppContext.Current.Cache.Remove($"{CACHE_KEY}-{app.Id}");
		}

		#endregion

		#region <--- Application Sitemap Area --->

		/// <summary>
		/// Creates new sitemap area
		/// </summary>
		/// <param name="id"></param>
		/// <param name="appId"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="description"></param>
		/// <param name="descriptionTranslations"></param>
		/// <param name="iconClass"></param>
		/// <param name="color"></param>
		/// <param name="weight"></param>
		/// <param name="showGroupNames"></param>
		/// <param name="accessRoles"></param>
		/// <param name="transaction"></param>
		public void CreateArea(Guid id, Guid appId, string name, string label, List<TranslationResource> labelTranslations,
			string description, List<TranslationResource> descriptionTranslations, string iconClass, string color,
			int weight, bool showGroupNames, List<Guid> accessRoles, NpgsqlTransaction transaction = null)
		{
			// VALIDATION

			string lblTr = null;
			if (labelTranslations != null)
				lblTr = JsonConvert.SerializeObject(labelTranslations);

			string descTr = null;
			if (descriptionTranslations != null)
				descTr = JsonConvert.SerializeObject(descriptionTranslations);

			new SitemapAreaRepository(connectionString).Insert(id, appId, name, label, lblTr, description,
				descTr, iconClass, color, weight, showGroupNames, accessRoles, transaction);

			ClearAppCache(appId);
		}

		/// <summary>
		/// Updates existing sitemap area
		/// </summary>
		/// <param name="id"></param>
		/// <param name="appId"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="description"></param> 
		/// <param name="descriptionTranslations"></param>
		/// <param name="iconClass"></param>
		/// <param name="color"></param>
		/// <param name="weight"></param>
		/// <param name="showGroupNames"></param>
		/// <param name="accessRoles"></param>
		/// <param name="transaction"></param>
		public void UpdateArea(Guid id, Guid appId, string name, string label, List<TranslationResource> labelTranslations,
			string description, List<TranslationResource> descriptionTranslations, string iconClass, string color,
			int weight, bool showGroupNames, List<Guid> accessRoles, NpgsqlTransaction transaction = null)
		{
			// VALIDATION

			string lblTr = null;
			if (labelTranslations != null)
				lblTr = JsonConvert.SerializeObject(labelTranslations);

			string descTr = null;
			if (descriptionTranslations != null)
				descTr = JsonConvert.SerializeObject(descriptionTranslations);

			new SitemapAreaRepository(connectionString).Update(id, appId, name, label, lblTr, description,
				descTr, iconClass, color, weight, showGroupNames, accessRoles, transaction);

			ClearAppCache(appId);
		}

		/// <summary>
		/// Deletes sitemap area
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		public void DeleteArea(Guid id, NpgsqlTransaction transaction = null, bool cascade = true )
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

						Guid? appId = new SitemapAreaRepository(connectionString).GetAppIdByAreaId(id, trans);
						if (appId == null)
							throw new Exception("Sitemap area is not found");

						DeleteAreaInternal(id, appId.Value, cascade, trans);

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
				Guid? appId = new SitemapAreaRepository(connectionString).GetAppIdByAreaId(id, transaction);
				if (appId == null)
					throw new Exception("Sitemap area is not found");

				DeleteAreaInternal(id, appId.Value, cascade, transaction);
			}
		}

		public void DeleteAreaInternal(Guid id, Guid appId, bool cascade, NpgsqlTransaction transaction = null)
		{
			if (cascade)
			{
				SitemapAreaGroupRepository groupsRep = new SitemapAreaGroupRepository(connectionString);
				DataTable areaGroups = groupsRep.GetAreaGroups(id, transaction);
				foreach (DataRow entRow in areaGroups.Rows)
					groupsRep.Delete((Guid)entRow["id"], transaction);

				SitemapAreaNodeRepository nodesRep = new SitemapAreaNodeRepository(connectionString);
				DataTable areaNodes = nodesRep.GetAreaNodes(id, transaction);
				foreach (DataRow entRow in areaNodes.Rows)
					DeleteAreaNode((Guid)entRow["id"], transaction);
			}

			new PageService(connectionString).UnbindPagesFromSitemapArea(id,transaction);
			new SitemapAreaRepository(connectionString).Delete(id, transaction);

			ClearAppCache(appId);
		}

		#endregion

		#region <--- Application Sitemap Area Group --->

		/// <summary>
		/// Creates new sitemap area group
		/// </summary>
		/// <param name="id"></param>
		/// <param name="areaId"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="weight"></param>
		/// <param name="renderRoles"></param>
		/// <param name="transaction"></param>
		public void CreateAreaGroup(Guid id, Guid areaId, string name, string label, List<TranslationResource> labelTranslations,
			int weight, List<Guid> renderRoles, NpgsqlTransaction transaction = null)
		{
			//VALIDATION

			string lblTr = null;
			if (labelTranslations != null)
				lblTr = JsonConvert.SerializeObject(labelTranslations);

			new SitemapAreaGroupRepository(connectionString).Insert(id, areaId, name, label, lblTr, weight, renderRoles, transaction);

			Guid? appId = new SitemapAreaRepository(connectionString).GetAppIdByAreaId(areaId, transaction);
			ClearAppCache(appId.Value);
		}

		/// <summary>
		/// Updates existing sitemap area group
		/// </summary>
		/// <param name="id"></param>
		/// <param name="areaId"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="weight"></param>
		/// <param name="renderRoles"></param>
		/// <param name="transaction"></param>
		public void UpdateAreaGroup(Guid id, Guid areaId, string name, string label, List<TranslationResource> labelTranslations,
			int weight, List<Guid> renderRoles, NpgsqlTransaction transaction = null)
		{
			//VALIDATION

			string lblTr = null;
			if (labelTranslations != null)
				lblTr = JsonConvert.SerializeObject(labelTranslations);

			new SitemapAreaGroupRepository(connectionString).Update(id, areaId, name, label, lblTr, weight, renderRoles, transaction);

			Guid? appId = new SitemapAreaRepository(connectionString).GetAppIdByAreaId(areaId, transaction);
			ClearAppCache(appId.Value);
		}

		/// <summary>
		/// Delete sitemap area group
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		public void DeleteAreaGroup(Guid id, NpgsqlTransaction transaction = null)
		{
			Guid? areaId = new SitemapAreaGroupRepository(connectionString).GetAreaIdByGroupId(id, transaction);
			Guid? appId = new SitemapAreaRepository(connectionString).GetAppIdByAreaId(areaId.Value, transaction);

			new SitemapAreaGroupRepository(connectionString).Delete(id, transaction);
			
			ClearAppCache(appId.Value);
		}

		#endregion

		#region <--- Application Sitemap Area Node --->

		/// <summary>
		/// Creates new sitemap area node
		/// </summary>
		/// <param name="id"></param>
		/// <param name="areaId"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="iconClass"></param>
		/// <param name="url"></param>
		/// <param name="type"></param>
		/// <param name="entityId"></param>
		/// <param name="weight"></param>
		/// <param name="accessRoles"></param>
		/// <param name="transaction"></param>
		public void CreateAreaNode(Guid id, Guid areaId, string name, string label, List<TranslationResource> labelTranslations,
			string iconClass, string url, int type, Guid? entityId, int weight,
			List<Guid> accessRoles, List<Guid> entityListPages = null, List<Guid> entityCreatePages = null, 
            List<Guid> entityDetailsPages = null, List<Guid> entityManagePages = null, NpgsqlTransaction transaction = null, Guid? parentId = null)
		{
            if (entityListPages == null)
                entityListPages = new List<Guid>();

            if (entityCreatePages == null)
                entityCreatePages = new List<Guid>();

            if (entityDetailsPages == null)
                entityDetailsPages = new List<Guid>();

            if (entityManagePages == null)
                entityManagePages = new List<Guid>();

            //VALIDATION

            string lblTr = null;
			if (labelTranslations != null)
				lblTr = JsonConvert.SerializeObject(labelTranslations);

			new SitemapAreaNodeRepository(connectionString).Insert(id, areaId, name, label, lblTr, iconClass, url, type,
						entityId, weight, accessRoles, entityListPages, entityCreatePages, entityDetailsPages, entityManagePages, transaction,parentId);

			Guid? appId = new SitemapAreaRepository(connectionString).GetAppIdByAreaId(areaId, transaction);
			ClearAppCache(appId.Value);
		}

		/// <summary>
		/// Updates sitemap area node
		/// </summary>
		/// <param name="id"></param>
		/// <param name="areaId"></param>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="labelTranslations"></param>
		/// <param name="iconClass"></param>
		/// <param name="url"></param>
		/// <param name="type"></param>
		/// <param name="entityId"></param>
		/// <param name="weight"></param>
		/// <param name="accessRoles"></param>
		/// <param name="transaction"></param>
		public void UpdateAreaNode(Guid id, Guid areaId, string name, string label, List<TranslationResource> labelTranslations,
			string iconClass, string url, int type, Guid? entityId, int weight,
			List<Guid> accessRoles, List<Guid> entityListPages = null, List<Guid> entityCreatePages = null,
            List<Guid> entityDetailsPages = null, List<Guid> entityManagePages = null, NpgsqlTransaction transaction = null, Guid? parentId = null)
		{
            if (entityListPages == null)
                entityListPages = new List<Guid>();

            if (entityCreatePages == null)
                entityCreatePages = new List<Guid>();

            if (entityDetailsPages == null)
                entityDetailsPages = new List<Guid>();

            if (entityManagePages == null)
                entityManagePages = new List<Guid>();


            //VALIDATION

            string lblTr = null;
			if (labelTranslations != null)
				lblTr = JsonConvert.SerializeObject(labelTranslations);

			new SitemapAreaNodeRepository(connectionString).Update(id, areaId, name, label, lblTr, iconClass, url, type,
						entityId, weight, accessRoles, entityListPages, entityCreatePages, entityDetailsPages, entityManagePages, transaction,parentId);

			Guid? appId = new SitemapAreaRepository(connectionString).GetAppIdByAreaId(areaId, transaction);
			ClearAppCache(appId.Value);
		}

		/// <summary>
		/// Deletes sitemap area node
		/// </summary>
		/// <param name="id"></param>
		/// <param name="transaction"></param>
		public void DeleteAreaNode(Guid id, NpgsqlTransaction transaction = null)
		{
			Guid? areaId = new SitemapAreaNodeRepository(connectionString).GetAreaIdByNodeId(id, transaction);
			Guid? appId = new SitemapAreaRepository(connectionString).GetAppIdByAreaId(areaId.Value, transaction);

			new PageService(connectionString).UnbindPagesFromSitemapNode(id, transaction);
			new SitemapAreaNodeRepository(connectionString).Delete(id, transaction);
			
			ClearAppCache(appId.Value);
		}

		public Sitemap OrderSitemap(Sitemap sitemap)
		{
			sitemap.Areas.ForEach(x => { x.Nodes = x.Nodes.OrderBy(t => t.Weight).ThenBy(y => y.Name).ToList(); });
			sitemap.Areas = sitemap.Areas.OrderBy(x => x.Weight).ThenBy(x=>x.Name).ToList();
			
			return sitemap;
		}

		#endregion
	}
}
