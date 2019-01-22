using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Wangkanai.Detection;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web
{
	public class ErpRequestContext
	{
		public IServiceProvider ServiceProvider { get; private set; }
		
		public IDeviceResolver Detection { get { return ServiceProvider.GetService<IDeviceResolver>(); } }

		public PageContext PageContext { get; set; }
		
		public App App { get; internal set; } = null;

		public SitemapArea SitemapArea { get; internal set; } = null;

		public SitemapNode SitemapNode { get; internal set; } = null;

		public Entity Entity { get; internal set; } = null;

		public Entity ParentEntity { get; internal set; } = null;

		public ErpPage Page { get; internal set; } = null;

		public ErpPage ParentPage { get; internal set; } = null;

		public Guid? RecordId { get; internal set; } = null;

		public Guid? RelationId { get; internal set; } = null;

		public Guid? ParentRecordId { get; internal set; } = null;

		public ErpRequestContext([FromServices]IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}

		public void SetCurrentApp(string appName, string areaName, string nodeName)
		{
			if (!String.IsNullOrWhiteSpace(appName))
			{
				App = new AppService().GetApplication(appName);
			}
			else
			{
				App = null;
			}
			if (App != null && !String.IsNullOrWhiteSpace(areaName))
			{
				if (App.Sitemap.Areas.Count > 0)
				{
					SitemapArea = App.Sitemap.Areas.FirstOrDefault(x => x.Name == areaName);
				}
				else
					SitemapArea = null;

			}
			else
			{
				SitemapArea = null;
			}
			if (App != null && SitemapArea != null && !String.IsNullOrWhiteSpace(nodeName))
			{
				if (SitemapArea.Nodes.Count > 0)
				{
					SitemapNode = SitemapArea.Nodes.FirstOrDefault(x => x.Name == nodeName);
				}
				else
					SitemapNode = null;
			}
			else
			{
				SitemapNode = null;
			}
		}

		public void SetCurrentPage(PageContext pageContext,string pageName, string appName, string areaName, string nodeName, Guid? recordId = null, Guid? relationId = null, Guid? parentRecordId = null)
		{
			ErpPage parentPage = null;
			Page = new PageService().GetCurrentPage(pageContext, pageName, appName, areaName, nodeName, out parentPage, recordId: recordId, relationId: relationId, parentRecordId:parentRecordId );
			if (Page != null && Page.EntityId != null && (Page.Type == PageType.RecordList || Page.Type == PageType.RecordDetails
							|| Page.Type == PageType.RecordCreate || Page.Type == PageType.RecordManage)) {
				Entity = new EntityManager().ReadEntity(Page.EntityId ?? Guid.Empty).Object.MapTo<Entity>();
			}
			if (parentPage != null) {
				ParentPage = parentPage;
				ParentEntity = new EntityManager().ReadEntity(parentPage.EntityId ?? Guid.Empty).Object.MapTo<Entity>();
			}

		}

		public string GenerateCurrentPageBaseUrl()
		{
			var context = this;
			//Case 1. Entity Record Page | App:SET, SitemapArea:SET, SitemapNode: SET, Entity:SET, RecordId: SET
			if (context.App != null && context.SitemapArea != null && context.SitemapNode != null && context.Entity != null && context.RecordId != null)
			{
				//Node.Name should equal the Entity.Name
				return $"/{context.App.Name}/{context.SitemapArea.Name}/{context.SitemapNode.Name}/r/{context.RecordId}/";
			}
			//Case 2. Entity List Page | App:SET, SitemapArea:SET, SitemapNode: SET, Entity:SET, RecordId: NOT SET
			else if (context.App != null && context.SitemapArea != null && context.SitemapNode != null && context.Entity != null && context.RecordId == null)
			{
				//Node.Name should equal the Entity.Name
				return $"/{context.App.Name}/{context.SitemapArea.Name}/{context.SitemapNode.Name}/l/";
			}
			//Case 3. Application page | App:SET, SitemapArea:SET, SitemapNode: SET, Entity:NOT SET, RecordId: NOT SET
			else if (context.App != null && context.SitemapArea != null && context.SitemapNode != null && context.Entity == null && context.RecordId == null)
			{
				return $"/{context.App.Name}/{context.SitemapArea.Name}/{context.SitemapNode.Name}/a/";
			}
			//Case 4. Site Page
			else if (context.App != null && context.SitemapArea != null && context.SitemapNode != null && context.Entity != null && context.RecordId != null)
			{
				return $"/s/";
			}
			//Case 5. Default return "#" so it can be created as anchor
			else
			{
				return "#";
			}
		}

		public string GetCurrentRecordIdentityFieldValue(Guid? recordId = null, string entityName = "")
		{
			var context = this;
			if (recordId == null)
			{
				recordId = context.RecordId;
			}
			if (String.IsNullOrWhiteSpace(entityName) && context.Entity != null)
			{
				entityName = context.Entity.Name;
			}

			if (recordId == null)
			{
				throw new Exception("No suitable record Id found");
			}

			if (String.IsNullOrWhiteSpace(entityName))
			{
				throw new Exception("No suitable entity Name found");
			}


			var currentEntity = new EntityManager().ReadEntity(entityName).Object;
			if (currentEntity != null)
			{
				var identityFieldName = "id";
				if (currentEntity.RecordScreenIdField != null) {
					var screenField = currentEntity.Fields.FirstOrDefault(x => x.Id == currentEntity.RecordScreenIdField);
					if (screenField != null) {
						identityFieldName = screenField.Name;
					}
				}

				var recMan = new RecordManager();
				var queryResult = recMan.Find(new EntityQuery(currentEntity.Name, identityFieldName, EntityQuery.QueryEQ("id", recordId)));
				if (!queryResult.Success)
				{
					throw new Exception(queryResult.Message);
				}
				var queryData = queryResult.Object.Data;
				if (queryData != null && queryData.Any())
				{
					var record = queryResult.Object.Data.First();
					if (record.Properties.ContainsKey(identityFieldName))
					{
						return record[identityFieldName].ToString();
					}
				}
			}
			return "";// "(!) " + recordId.ToString();
		}

		public void SetSimulatedRouteData(Guid? entityId = null, Guid? parentEntityId = null, Guid? pageId = null, Guid? parentPageId = null, Guid? recordId = null, Guid? relationId = null, Guid? parentRecordId = null) {
			
			if (entityId != null)
				Entity = new EntityManager().ReadEntity(entityId ?? Guid.Empty).Object;

			if (parentEntityId != null)
				ParentEntity = new EntityManager().ReadEntity(parentEntityId ?? Guid.Empty).Object;

			if (pageId != null)
				Page = new PageService().GetPage(pageId ?? Guid.Empty);

			if (parentPageId != null)
				ParentPage = new PageService().GetPage(parentPageId ?? Guid.Empty);

			if (recordId != null)
				RecordId = recordId;

			if (relationId != null)
				RelationId = relationId;

			if(parentRecordId != null)
				ParentRecordId = parentRecordId;
		}
	}
}
