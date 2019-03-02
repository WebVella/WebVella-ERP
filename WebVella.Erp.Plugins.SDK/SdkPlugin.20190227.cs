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
		private static void Patch20190227(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			var appSrv = new AppService();

			#region << ***Update app*** App name: sdk >>
			{
				var id = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
				var name = "sdk";
				var label = "Software Development Kit";
				var description = "SDK & Development Tools";
				var iconClass = "fa fa-cogs";
				var author = "WebVella";
				var color = "#dc3545";
				var weight = 1000;
				var access = new List<Guid>();
				access.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));

				new WebVella.Erp.Web.Services.AppService().UpdateApplication(id, name, label, description, iconClass, author, color, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap area*** Sitemap area name: objects >>
			{
				var id = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
				var appId = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
				var name = "objects";
				var label = "Objects";
				var description = @"Schema and Layout management";
				var iconClass = "fa fa-pencil-ruler";
				var color = "#2196F3";
				var weight = 1;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap area*** Sitemap area name: access >>
			{
				var id = new Guid("c5c4cefc-1402-4a8b-9867-7f2a059b745d");
				var appId = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
				var name = "access";
				var label = "Access";
				var description = @"Manage users and roles";
				var iconClass = "fa fa-key";
				var color = "#673AB7";
				var weight = 2;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap area*** Sitemap area name: server >>
			{
				var id = new Guid("fee72214-f1c4-4ed5-8bda-35698dc11528");
				var appId = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
				var name = "server";
				var label = "Server";
				var description = @"Background jobs and maintenance";
				var iconClass = "fa fa-database";
				var color = "#F44336";
				var weight = 3;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: page >>
			{
				var id = new Guid("5b132ac0-703e-4342-a13d-c7ff93d07a4f");
				var areaId = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
				Guid? entityId = null;
				var name = "page";
				var label = "Pages";
				var url = "/sdk/objects/page/l";
				var iconClass = "fa fa-file";
				var weight = 1;
				var type = ((int)3);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: data_source >>
			{
				var id = new Guid("9b30bf96-67d9-4d20-bf07-e6ef1c44d553");
				var areaId = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
				Guid? entityId = null;
				var name = "data_source";
				var label = "Data sources";
				var url = "/sdk/objects/data_source/l/list";
				var iconClass = "fa fa-cloud-download-alt";
				var weight = 2;
				var type = ((int)3);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: application >>
			{
				var id = new Guid("02d75ea5-8fc6-4f95-9933-0eed6b36ca49");
				var areaId = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
				Guid? entityId = null;
				var name = "application";
				var label = "Applications";
				var url = "/sdk/objects/application/l/list";
				var iconClass = "fa fa-th";
				var weight = 3;
				var type = ((int)3);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: entity >>
			{
				var id = new Guid("dfa7ec55-b55b-404f-b251-889f1d81df29");
				var areaId = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
				Guid? entityId = null;
				var name = "entity";
				var label = "Entities";
				var url = "/sdk/objects/entity/l";
				var iconClass = "fa fa-database";
				var weight = 4;
				var type = ((int)3);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: user >>
			{
				var id = new Guid("ff578868-817e-433d-988f-bb8d4e9baa0d");
				var areaId = new Guid("c5c4cefc-1402-4a8b-9867-7f2a059b745d");
				Guid? entityId = null;
				var name = "user";
				var label = "Users";
				var url = "/sdk/access/user/l/list";
				var iconClass = "fa fa-user";
				var weight = 1;
				var type = ((int)3);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: job >>
			{
				var id = new Guid("396ec481-3b2e-461c-b514-743fb3252003");
				var areaId = new Guid("fee72214-f1c4-4ed5-8bda-35698dc11528");
				Guid? entityId = null;
				var name = "job";
				var label = "Background jobs";
				var url = "/sdk/server/job/l/plan";
				var iconClass = "fa fa-cogs";
				var weight = 1;
				var type = ((int)3);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: log >>
			{
				var id = new Guid("78a29ac8-d2aa-4379-b990-08f7f164a895");
				var areaId = new Guid("fee72214-f1c4-4ed5-8bda-35698dc11528");
				Guid? entityId = null;
				var name = "log";
				var label = "Logs";
				var url = "/sdk/server/log/l/list";
				var iconClass = "fas fa-sticky-note";
				var weight = 2;
				var type = ((int)3);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion


		}
	}
}
