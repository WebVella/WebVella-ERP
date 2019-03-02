using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Database;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.SDK
{
	public partial class SdkPlugin : ErpPlugin
	{
		private static void Patch20181215(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			var appSrv = new AppService();
			#region << Create SDK App >>
			var sdkApp = new App()
			{
				Id = WEBVELLA_SDK_APP_ID,
				Name = WEBVELLA_SDK_APP_NAME,
				Label = "Software Development Kit",
				Description = "SDK & Development Tools",
				IconClass = "fa fa-cogs",
				Author = "WebVella",
				Weight = 1000,
				Color = "#dc3545",
				Access = new List<Guid>() { SystemIds.AdministratorRoleId }
			};
			appSrv.CreateApplication(sdkApp.Id, sdkApp.Name, sdkApp.Label, sdkApp.Description, sdkApp.IconClass, sdkApp.Author, sdkApp.Color,
						sdkApp.Weight, sdkApp.Access, DbContext.Current.Transaction);

			#endregion

			#region << Create App Sitemap Areas>>
			{
				var appArea = new SitemapArea()
				{
					Id = WEBVELLA_SDK_APP_AREA_DESIGN_ID,
					Name = "objects",
					Label = "Objects",
					Description = "Schema and Layout management",
					IconClass = "fas fa-pencil-ruler",
					Weight = 1,
					Color = "#2196F3"
				};
				appSrv.CreateArea(appArea.Id, WEBVELLA_SDK_APP_ID, appArea.Name, appArea.Label, appArea.LabelTranslations, appArea.Description, appArea.DescriptionTranslations,
						appArea.IconClass, appArea.Color, appArea.Weight, appArea.ShowGroupNames, appArea.Access, DbContext.Current.Transaction);
			}
			{
				var appArea = new SitemapArea()
				{
					Id = WEBVELLA_SDK_APP_AREA_ACCESS_ID,
					Name = "access",
					Label = "Access",
					Description = "Manage users and roles",
					IconClass = "fa fa-key",
					Weight = 2,
					Color = "#673AB7"
				};
				appSrv.CreateArea(appArea.Id, WEBVELLA_SDK_APP_ID, appArea.Name, appArea.Label, appArea.LabelTranslations, appArea.Description, appArea.DescriptionTranslations,
						appArea.IconClass, appArea.Color, appArea.Weight, appArea.ShowGroupNames, appArea.Access, DbContext.Current.Transaction);
			}
			{
				var appArea = new SitemapArea()
				{
					Id = WEBVELLA_SDK_APP_AREA_SERVER_ID,
					Name = "server",
					Label = "Server",
					Description = "Background jobs and maintenance",
					IconClass = "fa fa-database",
					Weight = 3,
					Color = "#F44336"
				};
				appSrv.CreateArea(appArea.Id, WEBVELLA_SDK_APP_ID, appArea.Name, appArea.Label, appArea.LabelTranslations, appArea.Description, appArea.DescriptionTranslations,
						appArea.IconClass, appArea.Color, appArea.Weight, appArea.ShowGroupNames, appArea.Access, DbContext.Current.Transaction);
			}
			#endregion

			#region Create Sitemap Nodes >>
			//Design
			{
				var appNode = new SitemapNode()
				{
					Id = new Guid("5b132ac0-703e-4342-a13d-c7ff93d07a4f"),
					Name = "page",
					Label = "Pages",
					IconClass = "fa fa-file",
					Url = "/sdk/objects/page/l",
					Weight = 1,
					Type = SitemapNodeType.Url
				};
				appSrv.CreateAreaNode(appNode.Id, WEBVELLA_SDK_APP_AREA_DESIGN_ID, appNode.Name, appNode.Label, appNode.LabelTranslations, appNode.IconClass, appNode.Url,
					(int)appNode.Type, appNode.EntityId, appNode.Weight, appNode.Access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), DbContext.Current.Transaction);
			}
			{
				var appNode = new SitemapNode()
				{
					Id = new Guid("9b30bf96-67d9-4d20-bf07-e6ef1c44d553"),
					Name = "data_source",
					Label = "Data sources",
					IconClass = "fa fa-cloud-download-alt",
					Url = "/sdk/objects/data_source/l/list",
					Weight = 2,
					Type = SitemapNodeType.Url
				};
				appSrv.CreateAreaNode(appNode.Id, WEBVELLA_SDK_APP_AREA_DESIGN_ID, appNode.Name, appNode.Label, appNode.LabelTranslations, appNode.IconClass, appNode.Url,
					(int)appNode.Type, appNode.EntityId, appNode.Weight, appNode.Access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), DbContext.Current.Transaction);
			}
			{
				var appNode = new SitemapNode()
				{
					Id = new Guid("02d75ea5-8fc6-4f95-9933-0eed6b36ca49"),
					Name = "application",
					Label = "Applications",
					IconClass = "fa fa-th",
					Url = "/sdk/objects/application/l/list",
					Weight = 3,
					Type = SitemapNodeType.Url
				};
				appSrv.CreateAreaNode(appNode.Id, WEBVELLA_SDK_APP_AREA_DESIGN_ID, appNode.Name, appNode.Label, appNode.LabelTranslations, appNode.IconClass, appNode.Url,
					(int)appNode.Type, appNode.EntityId, appNode.Weight, appNode.Access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), DbContext.Current.Transaction);
			}
			{
				var appNode = new SitemapNode()
				{
					Id = new Guid("dfa7ec55-b55b-404f-b251-889f1d81df29"),
					Name = "entity",
					Label = "Entities",
					IconClass = "fa fa-database",
					Url = "/sdk/objects/entity/l",
					Weight = 4,
					Type = SitemapNodeType.Url
				};
				appSrv.CreateAreaNode(appNode.Id, WEBVELLA_SDK_APP_AREA_DESIGN_ID, appNode.Name, appNode.Label, appNode.LabelTranslations, appNode.IconClass, appNode.Url,
					(int)appNode.Type, appNode.EntityId, appNode.Weight, appNode.Access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), DbContext.Current.Transaction);
			}
			{
				var appNode = new SitemapNode()
				{
					Id = new Guid("4571de62-a817-4a94-8b49-4b230cc0d2ad"),
					Name = "codegen",
					Label = "Code generation",
					IconClass = "fa fa-code",
					Url = "/sdk/objects/codegen/a/codegen",
					Weight = 10,
					Type = SitemapNodeType.Url
				};
				appSrv.CreateAreaNode(appNode.Id, WEBVELLA_SDK_APP_AREA_DESIGN_ID, appNode.Name, appNode.Label, appNode.LabelTranslations, appNode.IconClass, appNode.Url,
					(int)appNode.Type, appNode.EntityId, appNode.Weight, appNode.Access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), DbContext.Current.Transaction);
			}

			//Access
			{
				var appNode = new SitemapNode()
				{
					Id = new Guid("ff578868-817e-433d-988f-bb8d4e9baa0d"),
					Name = "user",
					Label = "Users",
					IconClass = "fa fa-user",
					Url = "/sdk/access/user/l/list",
					Weight = 1,
					Type = SitemapNodeType.Url
				};
				appSrv.CreateAreaNode(appNode.Id, WEBVELLA_SDK_APP_AREA_ACCESS_ID, appNode.Name, appNode.Label, appNode.LabelTranslations, appNode.IconClass, appNode.Url,
					(int)appNode.Type, appNode.EntityId, appNode.Weight, appNode.Access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), DbContext.Current.Transaction);
			}
			{
				var appNode = new SitemapNode()
				{
					Id = new Guid("75567fc4-70e1-41a9-9e32-2e5b62636598"),
					Name = "role",
					Label = "Roles",
					IconClass = "fa fa-key",
					Url = "/sdk/access/role/l/list",
					Weight = 2,
					Type = SitemapNodeType.Url
				};
				appSrv.CreateAreaNode(appNode.Id, WEBVELLA_SDK_APP_AREA_ACCESS_ID, appNode.Name, appNode.Label, appNode.LabelTranslations, appNode.IconClass, appNode.Url,
					(int)appNode.Type, appNode.EntityId, appNode.Weight, appNode.Access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), DbContext.Current.Transaction);
			}

			//Server
			{
				var appNode = new SitemapNode()
				{
					Id = new Guid("396ec481-3b2e-461c-b514-743fb3252003"),
					Name = "job",
					Label = "Background jobs",
					IconClass = "fa fa-cogs",
					Url = "/sdk/server/job/l/plan",
					Weight = 1,
					Type = SitemapNodeType.Url
				};
				appSrv.CreateAreaNode(appNode.Id, WEBVELLA_SDK_APP_AREA_SERVER_ID, appNode.Name, appNode.Label, appNode.LabelTranslations, appNode.IconClass, appNode.Url,
					(int)appNode.Type, appNode.EntityId, appNode.Weight, appNode.Access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), DbContext.Current.Transaction);
			}
			{
				var appNode = new SitemapNode()
				{
					Id = new Guid("78a29ac8-d2aa-4379-b990-08f7f164a895"),
					Name = "log",
					Label = "Logs",
					IconClass = "far fa-sticky-note",
					Url = "/sdk/server/log/l/list",
					Weight = 2,
					Type = SitemapNodeType.Url
				};
				appSrv.CreateAreaNode(appNode.Id, WEBVELLA_SDK_APP_AREA_SERVER_ID, appNode.Name, appNode.Label, appNode.LabelTranslations, appNode.IconClass, appNode.Url,
					(int)appNode.Type, appNode.EntityId, appNode.Weight, appNode.Access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), DbContext.Current.Transaction);
			}
			#endregion
		}
	}
}
