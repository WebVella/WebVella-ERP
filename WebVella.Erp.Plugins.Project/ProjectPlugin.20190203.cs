using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.Project
{
	public partial class ProjectPlugin : ErpPlugin
	{
		private static void Patch20190203(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Create app*** App name: projects >>
			{
				var id = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "projects";
				var label = "Projects";
				var description = "Project management, task and time accounting";
				var iconClass = "fa fa-code";
				var author = "WebVella";
				var color = "#9c27b0";
				var weight = 1;
				var access = new List<Guid>();
				access.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));

				new WebVella.Erp.Web.Services.AppService().CreateApplication(id, name, label, description, iconClass, author, color, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: dashboard >>
			{
				var id = new Guid("d99e07df-b5f3-4a01-8506-b607c3389308");
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "dashboard";
				var label = "Dashboard";
				var description = @"";
				var iconClass = "fas fa-tachometer-alt";
				var color = "#9C27B0";
				var weight = 1;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: track-time >>
			{
				var id = new Guid("fe9ac91f-a52f-4127-a74b-c4b335930c1d");
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "track-time";
				var label = "Track Time";
				var description = @"User time track";
				var iconClass = "fas fa-clock";
				var color = "#9C27B0";
				var weight = 2;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: feed >>
			{
				var id = new Guid("24028a64-748b-43a2-98ae-47514da142fe");
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "feed";
				var label = "Feed";
				var description = @"";
				var iconClass = "fas fa-rss-square";
				var color = "#9C27B0";
				var weight = 3;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: tasks >>
			{
				var id = new Guid("9aacb1b4-c03d-44bb-8d79-554971f4a25c");
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "tasks";
				var label = "Tasks";
				var description = @"";
				var iconClass = "fas fa-check-double";
				var color = "#9C27B0";
				var weight = 4;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: projects >>
			{
				var id = new Guid("dadd2bb1-459b-48da-a798-f2eea579c4e5");
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "projects";
				var label = "Projects";
				var description = @"";
				var iconClass = "fa fa-cogs";
				var color = "#9C27B0";
				var weight = 5;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: accounts >>
			{
				var id = new Guid("b7ddb30a-0d8b-4d52-a392-5cc6136fb7a4");
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "accounts";
				var label = "Accounts";
				var description = @"list of all accounts in the system";
				var iconClass = "fas fa-user-tie";
				var color = "#9C27B0";
				var weight = 6;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: reports >>
			{
				var id = new Guid("83ebdcfd-a244-4fba-9e25-f96fe27b7d0d");
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "reports";
				var label = "Reports";
				var description = @"";
				var iconClass = "fa fa-database";
				var color = "#9C27B0";
				var weight = 7;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: dashboard >>
			{
				var id = new Guid("3edb7097-a998-4e2e-9ba0-716f0767ce35");
				var areaId = new Guid("d99e07df-b5f3-4a01-8506-b607c3389308");
				Guid? entityId = null;
				var name = "dashboard";
				var label = "Dashboard";
				var url = "/projects/dashboard/dashboard/a";
				var iconClass = "fas fa-tachometer-alt";
				var weight = 1;
				var type = ((int)2);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: track-time >>
			{
				var id = new Guid("8c27983c-d215-48ad-9e73-49fd4e8acdb8");
				var areaId = new Guid("fe9ac91f-a52f-4127-a74b-c4b335930c1d");
				Guid? entityId = null;
				var name = "track-time";
				var label = "Track Time";
				var url = "";
				var iconClass = "fas fa-clock";
				var weight = 1;
				var type = ((int)2);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: feed >>
			{
				var id = new Guid("8950c6c6-7848-4a0b-b260-e8dbedf7486c");
				var areaId = new Guid("24028a64-748b-43a2-98ae-47514da142fe");
				Guid? entityId = null;
				var name = "feed";
				var label = "Feed";
				var url = "/projects/feed/feed/a";
				var iconClass = "fas fa-rss-square";
				var weight = 1;
				var type = ((int)2);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: tasks >>
			{
				var id = new Guid("dda5c020-c2bd-4f1f-9d8d-447659decc15");
				var areaId = new Guid("9aacb1b4-c03d-44bb-8d79-554971f4a25c");
				Guid? entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				var name = "tasks";
				var label = "All tasks";
				var url = "";
				var iconClass = "fas fa-list-ul";
				var weight = 2;
				var type = ((int)1);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: projects >>
			{
				var id = new Guid("48200d8b-6b7d-47b5-931c-17033ad8a679");
				var areaId = new Guid("dadd2bb1-459b-48da-a798-f2eea579c4e5");
				Guid? entityId = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				var name = "projects";
				var label = "All projects";
				var url = "";
				var iconClass = "fas fa-list-ul";
				var weight = 2;
				var type = ((int)1);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: accounts >>
			{
				var id = new Guid("98c2a9bc-5576-4b90-b72c-d2cd7a3da5a5");
				var areaId = new Guid("b7ddb30a-0d8b-4d52-a392-5cc6136fb7a4");
				Guid? entityId = new Guid("2e22b50f-e444-4b62-a171-076e51246939");
				var name = "accounts";
				var label = "Accounts";
				var url = "";
				var iconClass = "fas fa-user-tie";
				var weight = 1;
				var type = ((int)1);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: list >>
			{
				var id = new Guid("f04a7e50-f56a-4c5a-aa82-8b1028a05eeb");
				var areaId = new Guid("83ebdcfd-a244-4fba-9e25-f96fe27b7d0d");
				Guid? entityId = null;
				var name = "list";
				var label = "Report List";
				var url = "";
				var iconClass = "fas fa-list-ul";
				var weight = 1;
				var type = ((int)2);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: list >>
			{
				var id = new Guid("84b892fc-6ca4-4c7e-8b7c-2f2f6954862f");
				var name = @"list";
				var label = "Reports List";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 1;
				var type = (PageType)((int)2);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = null;
				Guid? nodeId = new Guid("f04a7e50-f56a-4c5a-aa82-8b1028a05eeb");
				Guid? areaId = new Guid("83ebdcfd-a244-4fba-9e25-f96fe27b7d0d");
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: open >>
			{
				var id = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var name = @"open";
				var label = "Open tasks";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 5;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: all >>
			{
				var id = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var name = @"all";
				var label = "All Projects";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: create >>
			{
				var id = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var name = @"create";
				var label = "Create Project";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)4);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: dashboard >>
			{
				var id = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var name = @"dashboard";
				var label = "My Dashboard";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)2);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = null;
				Guid? nodeId = new Guid("3edb7097-a998-4e2e-9ba0-716f0767ce35");
				Guid? areaId = new Guid("d99e07df-b5f3-4a01-8506-b607c3389308");
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: dashboard >>
			{
				var id = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var name = @"dashboard";
				var label = "Project dashboard";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: details >>
			{
				var id = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var name = @"details";
				var label = "Project details";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: milestones >>
			{
				var id = new Guid("d07cbf70-09c6-47ee-9a13-80568e43d331");
				var name = @"milestones";
				var label = "Project milestones";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: create >>
			{
				var id = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var name = @"create";
				var label = "Create task";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)4);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: details >>
			{
				var id = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var name = @"details";
				var label = "Task details";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: no-owner >>
			{
				var id = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var name = @"no-owner";
				var label = "Open tasks without owner";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: list >>
			{
				var id = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var name = @"list";
				var label = "Accounts List";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2e22b50f-e444-4b62-a171-076e51246939");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: details >>
			{
				var id = new Guid("80b10445-c850-44cf-9c8c-57daca671dcf");
				var name = @"details";
				var label = "Account details";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2e22b50f-e444-4b62-a171-076e51246939");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: create >>
			{
				var id = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var name = @"create";
				var label = "Create Account";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)4);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2e22b50f-e444-4b62-a171-076e51246939");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: feed >>
			{
				var id = new Guid("acb76466-32b8-428c-81cb-47b6013879e7");
				var name = @"feed";
				var label = "My Watch Feed";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)2);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = null;
				Guid? nodeId = new Guid("8950c6c6-7848-4a0b-b260-e8dbedf7486c");
				Guid? areaId = new Guid("24028a64-748b-43a2-98ae-47514da142fe");
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: track-time >>
			{
				var id = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var name = @"track-time";
				var label = "Track Time";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)2);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = null;
				Guid? nodeId = new Guid("8c27983c-d215-48ad-9e73-49fd4e8acdb8");
				Guid? areaId = new Guid("fe9ac91f-a52f-4127-a74b-c4b335930c1d");
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: all >>
			{
				var id = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var name = @"all";
				var label = "All tasks";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 20;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: feed >>
			{
				var id = new Guid("dfe56667-174d-492d-8f84-b8ab8b70c63f");
				var name = @"feed";
				var label = "Project feed";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 100;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: account-monthly-timelog >>
			{
				var id = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var name = @"account-monthly-timelog";
				var label = "Report: Monthly Timelog for an account";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 1000;
				var type = (PageType)((int)2);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = null;
				Guid? nodeId = new Guid("f04a7e50-f56a-4c5a-aa82-8b1028a05eeb");
				Guid? areaId = new Guid("83ebdcfd-a244-4fba-9e25-f96fe27b7d0d");
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: tasks >>
			{
				var id = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var name = @"tasks";
				var label = "Project tasks";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 1000;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 6bb17b95-258a-4572-99f3-898d1895cfba >>
			{
				var id = new Guid("6bb17b95-258a-4572-99f3-898d1895cfba");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcValidation";
				var containerId = "";
				var options = @"{
  ""validation"": ""{\""type\"":\""0\"",\""string\"":\""Validation\"",\""default\"":\""\""}""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: f4f2b086-1181-4db5-b78f-51d1b41e1611 >>
			{
				var id = new Guid("f4f2b086-1181-4db5-b78f-51d1b41e1611");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcDrawer";
				var containerId = "";
				var options = @"{
  ""title"": ""Search Tasks"",
  ""width"": ""550px"",
  ""class"": """",
  ""body_class"": """",
  ""title_action_html"": """"
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 7590ab09-b749-4051-935a-b51d16d7b76a >>
			{
				var id = new Guid("7590ab09-b749-4051-935a-b51d16d7b76a");
				Guid? parentId = new Guid("f4f2b086-1181-4db5-b78f-51d1b41e1611");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-7590ab09-b749-4051-935a-b51d16d7b76a"",
  ""name"": ""form"",
  ""hook_key"": """",
  ""method"": ""get"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: df667b11-30ac-4b6b-a12d-41e5aaf6cae5 >>
			{
				var id = new Guid("df667b11-30ac-4b6b-a12d-41e5aaf6cae5");
				Guid? parentId = new Guid("7590ab09-b749-4051-935a-b51d16d7b76a");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Search Tasks"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 57789d88-e897-4b7b-9999-239821db4274 >>
			{
				var id = new Guid("57789d88-e897-4b7b-9999-239821db4274");
				Guid? parentId = new Guid("7590ab09-b749-4051-935a-b51d16d7b76a");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcGridFilterField";
				var containerId = "body";
				var options = @"{
  ""label"": ""Task contents"",
  ""name"": ""x_search"",
  ""try_connect_to_entity"": ""true"",
  ""field_type"": ""18"",
  ""query_type"": ""2"",
  ""query_options"": [
    ""2""
  ],
  ""prefix"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: dedd97f6-1b09-4942-aae1-684cdc49a3eb >>
			{
				var id = new Guid("dedd97f6-1b09-4942-aae1-684cdc49a3eb");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 2,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""Accounts\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""true"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No accounts matching your query"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""true"",
  ""container1_label"": """",
  ""container1_width"": ""20px"",
  ""container1_name"": ""action"",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_searchable"": ""false"",
  ""container2_label"": ""name"",
  ""container2_width"": """",
  ""container2_name"": ""name"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_searchable"": ""false"",
  ""container3_label"": ""column3"",
  ""container3_width"": """",
  ""container3_name"": ""column3"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_searchable"": ""false"",
  ""container4_label"": ""column4"",
  ""container4_width"": """",
  ""container4_name"": ""column4"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_searchable"": ""false"",
  ""container5_label"": ""column5"",
  ""container5_width"": """",
  ""container5_name"": ""column5"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_searchable"": ""false"",
  ""container6_label"": ""column6"",
  ""container6_width"": """",
  ""container6_name"": ""column6"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_searchable"": ""false"",
  ""container7_label"": ""column7"",
  ""container7_width"": """",
  ""container7_name"": ""column7"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_searchable"": ""false"",
  ""container8_label"": ""column8"",
  ""container8_width"": """",
  ""container8_name"": ""column8"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_searchable"": ""false"",
  ""container9_label"": ""column9"",
  ""container9_width"": """",
  ""container9_name"": ""column9"",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_searchable"": ""false"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_searchable"": ""false"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_searchable"": ""false"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_searchable"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: 8f61ba2d-9c8a-434d-9f78-d12926cd80ef >>
			{
				var id = new Guid("8f61ba2d-9c8a-434d-9f78-d12926cd80ef");
				Guid? parentId = new Guid("dedd97f6-1b09-4942-aae1-684cdc49a3eb");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.name\"",\""default\"":\""Account Name\""}"",
  ""name"": ""name"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: c0689f85-235d-484e-bea3-e534e6e10094 >>
			{
				var id = new Guid("c0689f85-235d-484e-bea3-e534e6e10094");
				Guid? parentId = new Guid("dedd97f6-1b09-4942-aae1-684cdc49a3eb");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "column1";
				var options = @"{
  ""type"": ""2"",
  ""text"": """",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-eye"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//replace constants with your values\\n\\t\\tconst string DATASOURCE_NAME = \\\""RowRecord.id\\\"";\\n\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar dataSource = pageModel.TryGetDataSourceProperty<Guid>(DATASOURCE_NAME);\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (dataSource == null)\\n\\t\\t\\treturn null;\\n\\n        \\n\\t\\treturn $\\\""/projects/accounts/accounts/r/{dataSource}\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: 13a1d868-93ee-41d1-bb94-231d99899f74 >>
			{
				var id = new Guid("13a1d868-93ee-41d1-bb94-231d99899f74");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": """",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""Entity.IconName\"",\""default\"":\""\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: 77abedcf-4bea-46f3-b50c-340a7aa237d6 >>
			{
				var id = new Guid("77abedcf-4bea-46f3-b50c-340a7aa237d6");
				Guid? parentId = new Guid("13a1d868-93ee-41d1-bb94-231d99899f74");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""New Account"",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-plus go-green"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""/projects/accounts/accounts/c"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: b9258d04-360b-426f-b542-ec458f946edf >>
			{
				var id = new Guid("b9258d04-360b-426f-b542-ec458f946edf");
				Guid? parentId = new Guid("13a1d868-93ee-41d1-bb94-231d99899f74");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""Search"",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-search"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH(\""WebVella.Erp.Web.Components.PcDrawer\"",\""open\"")"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: feed  id: e1e493ac-6b74-490f-a0e3-ffd2f2f71f1b >>
			{
				var id = new Guid("e1e493ac-6b74-490f-a0e3-ffd2f2f71f1b");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("acb76466-32b8-428c-81cb-47b6013879e7");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""App.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fas fa-rss-square"",
  ""return_url"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: a584a5ed-96a2-4a28-95e8-23266bc36926 >>
			{
				var id = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 12,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 6,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 12,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 6,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 63daa5c0-ed7f-432e-bfbb-746b94207146 >>
			{
				var id = new Guid("63daa5c0-ed7f-432e-bfbb-746b94207146");
				Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""My Overdue Tasks"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3 "",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: d4c6bc3b-51d5-4f2d-a329-f02c59250a41 >>
			{
				var id = new Guid("d4c6bc3b-51d5-4f2d-a329-f02c59250a41");
				Guid? parentId = new Guid("63daa5c0-ed7f-432e-bfbb-746b94207146");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetTasksQueue";
				var containerId = "body";
				var options = @"{
  ""project_id"": """",
  ""user_id"": ""{\""type\"":\""0\"",\""string\"":\""CurrentUser.Id\"",\""default\"":\""\""}"",
  ""type"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: ae930e6f-38b5-4c48-a17f-63b0bdf7dab6 >>
			{
				var id = new Guid("ae930e6f-38b5-4c48-a17f-63b0bdf7dab6");
				Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""All Users' Timesheet"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": ""pt-3 pb-3"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 483e09f0-98c4-4e70-ad9a-3a92abebaf74 >>
			{
				var id = new Guid("483e09f0-98c4-4e70-ad9a-3a92abebaf74");
				Guid? parentId = new Guid("ae930e6f-38b5-4c48-a17f-63b0bdf7dab6");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetTimesheet";
				var containerId = "body";
				var options = @"""{}""";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 151e265c-d3d3-4340-92fc-0cace2ca45f9 >>
			{
				var id = new Guid("151e265c-d3d3-4340-92fc-0cace2ca45f9");
				Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column1";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": ""mb-3"",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 47303562-04a3-4935-b228-aaa61527f963 >>
			{
				var id = new Guid("47303562-04a3-4935-b228-aaa61527f963");
				Guid? parentId = new Guid("151e265c-d3d3-4340-92fc-0cace2ca45f9");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""Tasks"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm h-100"",
  ""body_class"": ""p-3 align-center-col"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 5b95ff72-dfc0-4a99-ad3a-6c6107f7bd4c >>
			{
				var id = new Guid("5b95ff72-dfc0-4a99-ad3a-6c6107f7bd4c");
				Guid? parentId = new Guid("47303562-04a3-4935-b228-aaa61527f963");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetTasksChart";
				var containerId = "body";
				var options = @"{
  ""project_id"": """",
  ""user_id"": ""{\""type\"":\""0\"",\""string\"":\""CurrentUser.Id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: be907fa3-0971-45b5-9dcf-fabbb277fe54 >>
			{
				var id = new Guid("be907fa3-0971-45b5-9dcf-fabbb277fe54");
				Guid? parentId = new Guid("151e265c-d3d3-4340-92fc-0cace2ca45f9");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Priority"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm h-100"",
  ""body_class"": ""p-3 align-center-col"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 209d32c9-6c2f-4f45-859a-3ae2718ebf88 >>
			{
				var id = new Guid("209d32c9-6c2f-4f45-859a-3ae2718ebf88");
				Guid? parentId = new Guid("be907fa3-0971-45b5-9dcf-fabbb277fe54");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetTasksPriorityChart";
				var containerId = "body";
				var options = @"{
  ""project_id"": """",
  ""user_id"": ""{\""type\"":\""0\"",\""string\"":\""CurrentUser.Id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 8e533c53-0bf5-4082-ae06-f47f1bd9b3b5 >>
			{
				var id = new Guid("8e533c53-0bf5-4082-ae06-f47f1bd9b3b5");
				Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""My 10 Upcoming Tasks "",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: f35b6e4b-3c81-409c-8f01-18e0d457e9ff >>
			{
				var id = new Guid("f35b6e4b-3c81-409c-8f01-18e0d457e9ff");
				Guid? parentId = new Guid("8e533c53-0bf5-4082-ae06-f47f1bd9b3b5");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetTasksQueue";
				var containerId = "body";
				var options = @"{
  ""project_id"": """",
  ""user_id"": ""{\""type\"":\""0\"",\""string\"":\""CurrentUser.Id\"",\""default\"":\""\""}"",
  ""type"": ""3""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: e49cf2f9-82b0-4988-aa29-427e8d9501d9 >>
			{
				var id = new Guid("e49cf2f9-82b0-4988-aa29-427e8d9501d9");
				Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""My Timesheet"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": ""pt-3 pb-3"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: c510d93d-e3d5-40d2-9655-73a3d2f63020 >>
			{
				var id = new Guid("c510d93d-e3d5-40d2-9655-73a3d2f63020");
				Guid? parentId = new Guid("e49cf2f9-82b0-4988-aa29-427e8d9501d9");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetTimesheet";
				var containerId = "body";
				var options = @"{
  ""project_id"": """",
  ""user_id"": ""{\""type\"":\""0\"",\""string\"":\""CurrentUser.Id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 6ef7bbd7-b96c-45d4-97e1-b8e43f489ed5 >>
			{
				var id = new Guid("6ef7bbd7-b96c-45d4-97e1-b8e43f489ed5");
				Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""My Tasks Due Today"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: ec0da060-7367-4263-b3fa-7c32765c97c5 >>
			{
				var id = new Guid("ec0da060-7367-4263-b3fa-7c32765c97c5");
				Guid? parentId = new Guid("6ef7bbd7-b96c-45d4-97e1-b8e43f489ed5");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetTasksQueue";
				var containerId = "body";
				var options = @"{
  ""project_id"": """",
  ""user_id"": ""{\""type\"":\""0\"",\""string\"":\""CurrentUser.Id\"",\""default\"":\""\""}"",
  ""type"": ""2""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: f68e4fb5-64d1-48ff-8846-e0ec36aa7e69 >>
			{
				var id = new Guid("f68e4fb5-64d1-48ff-8846-e0ec36aa7e69");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""App.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fas fa-tachometer-alt"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: account-monthly-timelog  id: d3501ea7-86f2-4230-8bc5-30ffab78be5e >>
			{
				var id = new Guid("d3501ea7-86f2-4230-8bc5-30ffab78be5e");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcReportAccountMonthlyTimelogs";
				var containerId = "";
				var options = @"{
  ""year"": ""{\""type\"":\""0\"",\""string\"":\""RequestQuery.year\"",\""default\"":\""\""}"",
  ""month"": ""{\""type\"":\""0\"",\""string\"":\""RequestQuery.month\"",\""default\"":\""\""}"",
  ""account_id"": ""{\""type\"":\""0\"",\""string\"":\""RequestQuery.account\"",\""default\"":\""\""}""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: c984f52a-5121-471d-ae66-e8a64de68c3d >>
			{
				var id = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 8,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""AllProjectTasks\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""true"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No records"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""true"",
  ""container1_label"": """",
  ""container1_width"": ""40px"",
  ""container1_name"": """",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_searchable"": ""false"",
  ""container2_label"": ""type"",
  ""container2_width"": ""20px"",
  ""container2_name"": ""type"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_searchable"": ""false"",
  ""container3_label"": ""key"",
  ""container3_width"": ""80px"",
  ""container3_name"": ""key"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_searchable"": ""false"",
  ""container4_label"": ""task"",
  ""container4_width"": """",
  ""container4_name"": ""task"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_searchable"": ""false"",
  ""container5_label"": ""owner"",
  ""container5_width"": ""120px"",
  ""container5_name"": ""owner_id"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_searchable"": ""false"",
  ""container6_label"": ""created by"",
  ""container6_width"": ""120px"",
  ""container6_name"": ""created_by"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_searchable"": ""false"",
  ""container7_label"": ""target date"",
  ""container7_width"": ""120px"",
  ""container7_name"": ""target_date"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_searchable"": ""false"",
  ""container8_label"": ""status"",
  ""container8_width"": ""80px"",
  ""container8_name"": ""status"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_searchable"": ""false"",
  ""container9_label"": ""column9"",
  ""container9_width"": """",
  ""container9_name"": ""column9"",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_searchable"": ""false"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_searchable"": ""false"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_searchable"": ""false"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_searchable"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: d088ba1c-15b8-48b9-8673-a871338cbdea >>
			{
				var id = new Guid("d088ba1c-15b8-48b9-8673-a871338cbdea");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column4";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.subject\"",\""default\"":\""Task subject\""}"",
  ""name"": ""subject"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 064ea82a-c5c2-40dd-96e4-7859aa879b14 >>
			{
				var id = new Guid("064ea82a-c5c2-40dd-96e4-7859aa879b14");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column5";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_task[0].username\"",\""default\"":\""n/a\""}"",
  ""name"": ""owner_id"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: e83f6542-f9f8-4fec-aeb8-48731951f182 >>
			{
				var id = new Guid("e83f6542-f9f8-4fec-aeb8-48731951f182");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column7";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.target_date\"",\""default\"":\""n/a\""}"",
  ""name"": ""target_date"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: cfa8a277-5447-45f2-ad06-26818381b54a >>
			{
				var id = new Guid("cfa8a277-5447-45f2-ad06-26818381b54a");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column3";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.key\"",\""default\"":\""key\""}"",
  ""name"": ""key"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 9cd708aa-60c5-4dfa-b95c-73e5508aec64 >>
			{
				var id = new Guid("9cd708aa-60c5-4dfa-b95c-73e5508aec64");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column8";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$task_status_1n_task[0].label\"",\""default\"":\""n/a\""}"",
  ""name"": ""status_id"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: ad1e60ec-813c-4b1d-aa33-ad76d705d5d9 >>
			{
				var id = new Guid("ad1e60ec-813c-4b1d-aa33-ad76d705d5d9");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column6";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_task_creator[0].username\"",\""default\"":\""n/a\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: a20664ce-a3fe-436a-84f0-42f4e14564c1 >>
			{
				var id = new Guid("a20664ce-a3fe-436a-84f0-42f4e14564c1");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar typeRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord.$task_type_1n_task[0]\\\"");\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (typeRecord == null)\\n\\t\\t\\treturn null;\\n\\n        var iconClass=\\\""fa fa-fw fa-file\\\"";\\n        var color=\\\""#999\\\"";\\n        if(typeRecord[\\\""icon_class\\\""] != null){\\n            iconClass = (string)typeRecord[\\\""icon_class\\\""];\\n        }\\n        if(typeRecord[\\\""color\\\""] != null){\\n            color = (string)typeRecord[\\\""color\\\""];\\n        }\\n\\t\\treturn $\\\""<i class=\\\\\\\""{iconClass}\\\\\\\"" style=\\\\\\\""color:{color};font-size:23px;\\\\\\\"" title=\\\\\\\""{typeRecord[\\\""label\\\""]}\\\\\\\""></i>\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""icon\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 4f298a92-4592-4714-948e-eaebb3962785 >>
			{
				var id = new Guid("4f298a92-4592-4714-948e-eaebb3962785");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "column1";
				var options = @"{
  ""type"": ""2"",
  ""text"": """",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-eye"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar taskId = pageModel.TryGetDataSourceProperty<Guid>(\\\""RowRecord.id\\\"");\\n        var projectId = pageModel.TryGetDataSourceProperty<Guid>(\\\""ParentRecord.id\\\"");\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (taskId == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\treturn $\\\""/projects/tasks/tasks/r/{taskId}/details?returnUrl=/projects/projects/projects/r/{projectId}/rl/b1db4466-7423-44e9-b6b9-3063222c9e15/l/tasks\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: d65f22f5-6644-4ca9-81ce-c3ce5898f8b5 >>
			{
				var id = new Guid("d65f22f5-6644-4ca9-81ce-c3ce5898f8b5");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""App.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""far fa-clock"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: feed  id: 33cba2bb-6070-4b00-ba92-64064077a49b >>
			{
				var id = new Guid("33cba2bb-6070-4b00-ba92-64064077a49b");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("dfe56667-174d-492d-8f84-b8ab8b70c63f");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""Record.abbr\"",\""default\"":\""\""}"",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-rss"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: feed  id: c50ad432-98f2-4140-a40c-3157fc52f93c >>
			{
				var id = new Guid("c50ad432-98f2-4140-a40c-3157fc52f93c");
				Guid? parentId = new Guid("33cba2bb-6070-4b00-ba92-64064077a49b");
				Guid? nodeId = null;
				var pageId = new Guid("dfe56667-174d-492d-8f84-b8ab8b70c63f");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "toolbar";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar projectId = pageModel.TryGetDataSourceProperty<Guid>(\\\""Record.id\\\"");\\n        var pageName = pageModel.TryGetDataSourceProperty<string>(\\\""Page.Name\\\"");\\n\\n\\t\\tif (projectId == null || pageName == null)\\n\\t\\t\\treturn null;\\n\\n        var result = $\\\""<a href='/projects/projects/projects/r/{projectId}/dashboard' class='btn btn-link btn-sm {(pageName == \\\""dashboard\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Dashboard</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/feed' class='btn btn-link btn-sm {(pageName == \\\""feed\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Feed</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/b1db4466-7423-44e9-b6b9-3063222c9e15/l/tasks' class='btn btn-link btn-sm {(pageName == \\\""tasks\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Tasks</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/55c8d6e2-f26d-4689-9d1b-a8c1b9de1672/l/milestones' class='btn btn-link btn-sm {(pageName == \\\""milestones\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Milestones</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/details' class='btn btn-link btn-sm {(pageName == \\\""details\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Details</a>\\\"";\\n\\t\\treturn result;\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: e84c527a-4feb-4d60-ab91-4b1ecd89b39c >>
			{
				var id = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 4,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""TrackTimeTasks\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""false"",
  ""bordered"": ""true"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No records"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""false"",
  ""container1_label"": ""my tasks "",
  ""container1_width"": """",
  ""container1_name"": ""task"",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_class"": """",
  ""container1_vertical_align"": ""3"",
  ""container1_horizontal_align"": ""1"",
  ""container2_label"": ""logged"",
  ""container2_width"": ""120px"",
  ""container2_name"": ""logged"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_class"": ""timer-td"",
  ""container2_vertical_align"": ""3"",
  ""container2_horizontal_align"": ""1"",
  ""container3_label"": ""timelog"",
  ""container3_width"": ""150px"",
  ""container3_name"": """",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_class"": """",
  ""container3_vertical_align"": ""3"",
  ""container3_horizontal_align"": ""1"",
  ""container4_label"": ""action"",
  ""container4_width"": ""100px"",
  ""container4_name"": """",
  ""container4_nowrap"": ""true"",
  ""container4_sortable"": ""false"",
  ""container4_class"": """",
  ""container4_vertical_align"": ""1"",
  ""container4_horizontal_align"": ""1"",
  ""container5_label"": ""column5"",
  ""container5_width"": """",
  ""container5_name"": ""column5"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_class"": """",
  ""container5_vertical_align"": ""1"",
  ""container5_horizontal_align"": ""1"",
  ""container6_label"": ""column6"",
  ""container6_width"": """",
  ""container6_name"": ""column6"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_class"": """",
  ""container6_vertical_align"": ""1"",
  ""container6_horizontal_align"": ""1"",
  ""container7_label"": ""column7"",
  ""container7_width"": """",
  ""container7_name"": ""column7"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_class"": """",
  ""container7_vertical_align"": ""1"",
  ""container7_horizontal_align"": ""1"",
  ""container8_label"": ""column8"",
  ""container8_width"": """",
  ""container8_name"": ""column8"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_class"": """",
  ""container8_vertical_align"": ""1"",
  ""container8_horizontal_align"": ""1"",
  ""container9_label"": ""column9"",
  ""container9_width"": """",
  ""container9_name"": ""column9"",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_class"": """",
  ""container9_vertical_align"": ""1"",
  ""container9_horizontal_align"": ""1"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_class"": """",
  ""container10_vertical_align"": ""1"",
  ""container10_horizontal_align"": ""1"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_class"": """",
  ""container11_vertical_align"": ""1"",
  ""container11_horizontal_align"": ""1"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_class"": """",
  ""container12_vertical_align"": ""1"",
  ""container12_horizontal_align"": ""1""
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 3d0eb8a7-1182-4974-a039-433954aa8d7c >>
			{
				var id = new Guid("3d0eb8a7-1182-4974-a039-433954aa8d7c");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "column3";
				var options = @"{
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.id\"",\""default\"":\""\""}"",
  ""name"": ""task_id"",
  ""try_connect_to_entity"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 46657d5a-0102-43b7-9ca3-9259953d37b6 >>
			{
				var id = new Guid("46657d5a-0102-43b7-9ca3-9259953d37b6");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcBtnGroup";
				var containerId = "column3";
				var options = @"{
  ""size"": ""3"",
  ""is_vertical"": ""false"",
  ""class"": ""d-none stop-log-group w-100"",
  ""id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 700954cc-7407-4b20-81de-a882380e5d4d >>
			{
				var id = new Guid("700954cc-7407-4b20-81de-a882380e5d4d");
				Guid? parentId = new Guid("46657d5a-0102-43b7-9ca3-9259953d37b6");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""stop logging"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": ""btn-block stop-log"",
  ""id"": """",
  ""icon_class"": ""fas fa-fw fa-square go-red"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: c57d94a6-9c90-4071-b54b-2c05b79aa522 >>
			{
				var id = new Guid("c57d94a6-9c90-4071-b54b-2c05b79aa522");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry{\\n\\t\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\t\\tif (pageModel == null)\\n\\t\\t\\t\\treturn null;\\n\\t\\n\\t\\t\\t//try read data source by name and get result as specified type object\\n\\t\\t\\tvar dataSource = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord\\\"");\\n\\t\\n\\t\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\t\\tif (dataSource == null)\\n\\t\\t\\t\\treturn null;\\n\\t        var loggedSeconds = ((int)dataSource[\\\""logged_minutes\\\""])*60;\\n\\t        var logStartedOn = (DateTime?)dataSource[\\\""timelog_started_on\\\""];\\n\\t        var logStartString = \\\""\\\"";\\n\\t        if(logStartedOn != null){\\n\\t            loggedSeconds = loggedSeconds + (int)((DateTime.UtcNow - logStartedOn.Value).TotalSeconds);\\n\\t            logStartString = logStartedOn.Value.ToString(\\\""o\\\"");\\n\\t        }\\n\\n\\t        var hours = (int)(loggedSeconds/3600);\\n\\t        var loggedSecondsLeft = loggedSeconds - hours*3600;\\n\\t        var hoursString = \\\""00\\\"";\\n\\t        if(hours < 10)\\n\\t            hoursString = \\\""0\\\"" + hours;\\n            else\\n                hoursString = hours.ToString();\\n\\t            \\n\\t        var minutes = (int)(loggedSecondsLeft/60);\\n\\t        var minutesString = \\\""00\\\"";\\n\\t        if(minutes < 10)\\n\\t            minutesString = \\\""0\\\"" + minutes;\\n            else\\n                minutesString = minutes.ToString();\\t        \\n                \\n            var seconds =  loggedSecondsLeft -  minutes*60;\\n\\t        var secondsString = \\\""00\\\"";\\n\\t        if(seconds < 10)\\n\\t            secondsString = \\\""0\\\"" + seconds;\\n            else\\n                secondsString = seconds.ToString();\\t                    \\n            \\n            var result = $\\\""<span class='go-gray wv-timer' style='font-size:16px;'>{hoursString + \\\"" : \\\"" + minutesString + \\\"" : \\\"" + secondsString}</span>\\\\n\\\"";\\n            result += $\\\""<input type='hidden' name='timelog_total_seconds' value='{loggedSeconds}'/>\\\\n\\\"";\\n            result += $\\\""<input type='hidden' name='timelog_started_on' value='{logStartString}'/>\\\"";\\n            return result;\\n\\t\\t}\\n\\t\\tcatch(Exception ex){\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\\n\"",\""default\"":\""<span class=\\\""go-gray\\\"" style='font-size:16px;'>00 : 00 : 00</span>\""}"",
  ""name"": ""field"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 6b80c95e-a06d-4ad3-ae19-dfdc9fecf6ed >>
			{
				var id = new Guid("6b80c95e-a06d-4ad3-ae19-dfdc9fecf6ed");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "column4";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""set complete"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": ""set-completed"",
  ""id"": """",
  ""icon_class"": ""fa fa-check"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: c0962d97-a609-498b-9b0c-7c0dbfae8b73 >>
			{
				var id = new Guid("c0962d97-a609-498b-9b0c-7c0dbfae8b73");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "column3";
				var options = @"{
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.is_billable\"",\""default\"":\""\""}"",
  ""name"": ""is_billable""
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: b2baa937-e32a-4a06-8b9b-404f89e539c0 >>
			{
				var id = new Guid("b2baa937-e32a-4a06-8b9b-404f89e539c0");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing System.Diagnostics;\\nusing WebVella.Erp.Plugins.Project.Services;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar taskRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord\\\"");\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (taskRecord == null)\\n\\t\\t\\treturn null;\\n\\t\\t\\t\\n        var iconClass = \\\""\\\"";\\n        var color = \\\""\\\"";\\n        new TaskService().GetTaskIconAndColor((string)taskRecord[\\\""priority\\\""], out iconClass, out color);\\n\\n\\t\\treturn $\\\""<i class='{iconClass}' style='color:{color}'></i> <a href='/projects/tasks/tasks/r/{(Guid)taskRecord[\\\""id\\\""]}/details'>[{(string)taskRecord[\\\""key\\\""]}] {taskRecord[\\\""subject\\\""]}</a>\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""Task name\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 278b4db1-b310-416a-9f32-66ecd3475ba8 >>
			{
				var id = new Guid("278b4db1-b310-416a-9f32-66ecd3475ba8");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcBtnGroup";
				var containerId = "column3";
				var options = @"{
  ""size"": ""1"",
  ""is_vertical"": ""false"",
  ""class"": ""start-log-group w-100 d-none"",
  ""id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 55ff1b2f-43d4-4bde-818c-fd139d799261 >>
			{
				var id = new Guid("55ff1b2f-43d4-4bde-818c-fd139d799261");
				Guid? parentId = new Guid("278b4db1-b310-416a-9f32-66ecd3475ba8");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""0"",
  ""text"": """",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": ""manual-log"",
  ""id"": """",
  ""icon_class"": ""fa fa-plus"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 4603466b-422c-4666-9f05-aae386569590 >>
			{
				var id = new Guid("4603466b-422c-4666-9f05-aae386569590");
				Guid? parentId = new Guid("278b4db1-b310-416a-9f32-66ecd3475ba8");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""start log"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": ""start-log"",
  ""id"": """",
  ""icon_class"": ""fa fa-fw fa-play"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: d32f39bb-8ad4-438d-a8d1-7abca6f5e6b4 >>
			{
				var id = new Guid("d32f39bb-8ad4-438d-a8d1-7abca6f5e6b4");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""Projects\""}"",
  ""area_sublabel"": """",
  ""title"": ""Create account"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""App.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-plus"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: aeacecd8-8b3e-4cdb-84f6-114a2fb3c06d >>
			{
				var id = new Guid("aeacecd8-8b3e-4cdb-84f6-114a2fb3c06d");
				Guid? parentId = new Guid("d32f39bb-8ad4-438d-a8d1-7abca6f5e6b4");
				Guid? nodeId = null;
				var pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Create Account"",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-fw fa-plus go-green"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": ""CreateRecord""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: b7b8ed33-910f-4d28-bbe8-48c0799b00b5 >>
			{
				var id = new Guid("b7b8ed33-910f-4d28-bbe8-48c0799b00b5");
				Guid? parentId = new Guid("d32f39bb-8ad4-438d-a8d1-7abca6f5e6b4");
				Guid? nodeId = null;
				var pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""Cancel"",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""0\"",\""string\"":\""ReturnUrl\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 037ee1a4-e26c-4cd1-91ca-0e626c2995ed >>
			{
				var id = new Guid("037ee1a4-e26c-4cd1-91ca-0e626c2995ed");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "";
				var options = @"{
  ""id"": ""CreateRecord"",
  ""name"": ""CreateAccount"",
  ""hook_key"": """",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 0fb05f08-6066-4de8-8452-c8b3c7306ff9 >>
			{
				var id = new Guid("0fb05f08-6066-4de8-8452-c8b3c7306ff9");
				Guid? parentId = new Guid("037ee1a4-e26c-4cd1-91ca-0e626c2995ed");
				Guid? nodeId = null;
				var pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"""{}""";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 5ecb652c-e474-4700-bc32-5173d2fdad00 >>
			{
				var id = new Guid("5ecb652c-e474-4700-bc32-5173d2fdad00");
				Guid? parentId = new Guid("0fb05f08-6066-4de8-8452-c8b3c7306ff9");
				Guid? nodeId = null;
				var pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Name"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""name"": ""name"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 03d2ed0f-33ed-4b7d-84fb-102f4b7452a8 >>
			{
				var id = new Guid("03d2ed0f-33ed-4b7d-84fb-102f4b7452a8");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("80b10445-c850-44cf-9c8c-57daca671dcf");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"""{}""";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 7eb7af4f-bdd3-410a-b3c4-71e620b627c5 >>
			{
				var id = new Guid("7eb7af4f-bdd3-410a-b3c4-71e620b627c5");
				Guid? parentId = new Guid("03d2ed0f-33ed-4b7d-84fb-102f4b7452a8");
				Guid? nodeId = null;
				var pageId = new Guid("80b10445-c850-44cf-9c8c-57daca671dcf");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Name"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""name"": ""name"",
  ""mode"": ""3"",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 552a4fad-5236-4aad-b3fc-443a5f12e574 >>
			{
				var id = new Guid("552a4fad-5236-4aad-b3fc-443a5f12e574");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("80b10445-c850-44cf-9c8c-57daca671dcf");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""Entity.LabelPlural\"",\""default\"":\""\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""Record.label\"",\""default\"":\""\""}"",
  ""title"": ""Account Details"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""Entity.IconName\"",\""default\"":\""\""}"",
  ""return_url"": ""/crm/accounts/accounts/l/list""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: 81fda9cf-04d7-4f99-8448-34392e1c0640 >>
			{
				var id = new Guid("81fda9cf-04d7-4f99-8448-34392e1c0640");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcDrawer";
				var containerId = "";
				var options = @"{
  ""title"": ""Search Accounts"",
  ""width"": ""550px"",
  ""class"": """",
  ""body_class"": """",
  ""title_action_html"": ""<a href=\""javascript:void(0)\"" class=\""clear-filter-all\"">clear all</a>""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: 492d9088-16bc-40fd-963b-8a8c2acf0ffa >>
			{
				var id = new Guid("492d9088-16bc-40fd-963b-8a8c2acf0ffa");
				Guid? parentId = new Guid("81fda9cf-04d7-4f99-8448-34392e1c0640");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-492d9088-16bc-40fd-963b-8a8c2acf0ffa"",
  ""name"": ""form"",
  ""hook_key"": """",
  ""method"": ""get"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: 3845960e-4fc6-40f6-9ef6-36e7392f8ab0 >>
			{
				var id = new Guid("3845960e-4fc6-40f6-9ef6-36e7392f8ab0");
				Guid? parentId = new Guid("492d9088-16bc-40fd-963b-8a8c2acf0ffa");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcGridFilterField";
				var containerId = "body";
				var options = @"{
  ""label"": ""Name"",
  ""name"": ""name"",
  ""try_connect_to_entity"": ""true"",
  ""field_type"": ""18"",
  ""query_type"": ""2"",
  ""query_options"": [
    ""2""
  ],
  ""prefix"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: ec6f4bb5-aeeb-4706-a3dd-f3f208c63c6a >>
			{
				var id = new Guid("ec6f4bb5-aeeb-4706-a3dd-f3f208c63c6a");
				Guid? parentId = new Guid("492d9088-16bc-40fd-963b-8a8c2acf0ffa");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Search Accounts"",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 22af9111-4f15-48c1-a9fd-e5ab72074b3e >>
			{
				var id = new Guid("22af9111-4f15-48c1-a9fd-e5ab72074b3e");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 4,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""AllProjects\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""true"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No projects"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""true"",
  ""container1_label"": """",
  ""container1_width"": ""40px"",
  ""container1_name"": """",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_searchable"": ""false"",
  ""container2_label"": ""abbr"",
  ""container2_width"": ""60px"",
  ""container2_name"": ""abbr"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""true"",
  ""container2_searchable"": ""true"",
  ""container3_label"": ""name"",
  ""container3_width"": """",
  ""container3_name"": ""name"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_searchable"": ""false"",
  ""container4_label"": ""lead"",
  ""container4_width"": """",
  ""container4_name"": ""lead"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_searchable"": ""false"",
  ""container5_label"": ""column5"",
  ""container5_width"": """",
  ""container5_name"": ""column5"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_searchable"": ""false"",
  ""container6_label"": ""column6"",
  ""container6_width"": """",
  ""container6_name"": ""column6"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_searchable"": ""false"",
  ""container7_label"": ""column7"",
  ""container7_width"": """",
  ""container7_name"": ""column7"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_searchable"": ""false"",
  ""container8_label"": ""column8"",
  ""container8_width"": """",
  ""container8_name"": ""column8"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_searchable"": ""false"",
  ""container9_label"": ""column9"",
  ""container9_width"": """",
  ""container9_name"": ""column9"",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_searchable"": ""false"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_searchable"": ""false"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_searchable"": ""false"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_searchable"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 31a4f843-0ab5-4fd1-86ee-ad5f23f0d47a >>
			{
				var id = new Guid("31a4f843-0ab5-4fd1-86ee-ad5f23f0d47a");
				Guid? parentId = new Guid("22af9111-4f15-48c1-a9fd-e5ab72074b3e");
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column4";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_project_owner[0].username\"",\""default\"":\""Username\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: fcd1e0a0-bfc3-422f-b19d-2536dd919289 >>
			{
				var id = new Guid("fcd1e0a0-bfc3-422f-b19d-2536dd919289");
				Guid? parentId = new Guid("22af9111-4f15-48c1-a9fd-e5ab72074b3e");
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.abbr\"",\""default\"":\""abbr\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: ec508ea2-2332-40f0-838c-52d3ee250122 >>
			{
				var id = new Guid("ec508ea2-2332-40f0-838c-52d3ee250122");
				Guid? parentId = new Guid("22af9111-4f15-48c1-a9fd-e5ab72074b3e");
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column3";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.name\"",\""default\"":\""Project name\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 54d22f88-7a46-41e7-89b4-603dc14e7e73 >>
			{
				var id = new Guid("54d22f88-7a46-41e7-89b4-603dc14e7e73");
				Guid? parentId = new Guid("22af9111-4f15-48c1-a9fd-e5ab72074b3e");
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "column1";
				var options = @"{
  ""type"": ""2"",
  ""text"": """",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-eye"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//replace constants with your values\\n\\t\\tconst string DATASOURCE_NAME = \\\""RowRecord.id\\\"";\\n\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar dataSource = pageModel.TryGetDataSourceProperty<Guid>(DATASOURCE_NAME);\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (dataSource == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\treturn $\\\""/projects/projects/projects/r/{dataSource}/dashboard\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 39db266a-da49-4a6e-b74d-898c601ad78b >>
			{
				var id = new Guid("39db266a-da49-4a6e-b74d-898c601ad78b");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-plus"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: b5a15dac-a606-4c93-b258-f1a7ab799a05 >>
			{
				var id = new Guid("b5a15dac-a606-4c93-b258-f1a7ab799a05");
				Guid? parentId = new Guid("39db266a-da49-4a6e-b74d-898c601ad78b");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""Cancel"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""0\"",\""string\"":\""ReturnUrl\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 8e4e7f05-8942-4db1-8514-e460bde1e2b4 >>
			{
				var id = new Guid("8e4e7f05-8942-4db1-8514-e460bde1e2b4");
				Guid? parentId = new Guid("39db266a-da49-4a6e-b74d-898c601ad78b");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Create Project"",
  ""color"": ""1"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-save"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": ""CreateRecord""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: e6c5b22a-491a-4186-82d6-667253e2db0f >>
			{
				var id = new Guid("e6c5b22a-491a-4186-82d6-667253e2db0f");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "";
				var options = @"{
  ""id"": ""CreateRecord"",
  ""name"": ""CreateRecord"",
  ""hook_key"": """",
  ""method"": ""post"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 4dfaa373-e250-4a76-b5a5-98d596a52313 >>
			{
				var id = new Guid("4dfaa373-e250-4a76-b5a5-98d596a52313");
				Guid? parentId = new Guid("e6c5b22a-491a-4186-82d6-667253e2db0f");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 8,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: fc423988-297c-457d-a14b-9fe12557cc2e >>
			{
				var id = new Guid("fc423988-297c-457d-a14b-9fe12557cc2e");
				Guid? parentId = new Guid("4dfaa373-e250-4a76-b5a5-98d596a52313");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Name"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""name"": ""name"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""1"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 0f90af36-8f2d-4f26-8ba2-ea7e8accdc6d >>
			{
				var id = new Guid("0f90af36-8f2d-4f26-8ba2-ea7e8accdc6d");
				Guid? parentId = new Guid("4dfaa373-e250-4a76-b5a5-98d596a52313");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Abbreviation"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.abbr\"",\""default\"":\""\""}"",
  ""name"": ""abbr"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 7bbf3667-a26d-48d4-8eba-8ca5e03d14c3 >>
			{
				var id = new Guid("7bbf3667-a26d-48d4-8eba-8ca5e03d14c3");
				Guid? parentId = new Guid("e6c5b22a-491a-4186-82d6-667253e2db0f");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 1,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: cc487c98-c59f-4e8c-b147-36914bcf70fc >>
			{
				var id = new Guid("cc487c98-c59f-4e8c-b147-36914bcf70fc");
				Guid? parentId = new Guid("7bbf3667-a26d-48d4-8eba-8ca5e03d14c3");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Description"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.description\"",\""default\"":\""\""}"",
  ""name"": ""description"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 6529686c-c8b4-40f0-8242-e24153657be2 >>
			{
				var id = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? parentId = new Guid("7bbf3667-a26d-48d4-8eba-8ca5e03d14c3");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column1";
				var options = @"{
  ""visible_columns"": 3,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: ace9e1bf-47bf-495f-8e6b-7683d2a0fa78 >>
			{
				var id = new Guid("ace9e1bf-47bf-495f-8e6b-7683d2a0fa78");
				Guid? parentId = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Budget amount"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.budget_amount\"",\""default\"":\""\""}"",
  ""name"": ""budget_amount"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: aec39a46-526c-45f2-ad43-38618a366098 >>
			{
				var id = new Guid("aec39a46-526c-45f2-ad43-38618a366098");
				Guid? parentId = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Start date"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.start_date\"",\""default\"":\""\""}"",
  ""name"": ""start_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 8dc4fd15-a1eb-4b7e-a1a9-381ac8e7de9b >>
			{
				var id = new Guid("8dc4fd15-a1eb-4b7e-a1a9-381ac8e7de9b");
				Guid? parentId = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Budget type"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.budget_type\"",\""default\"":\""\""}"",
  ""name"": ""budget_type"",
  ""try_connect_to_entity"": ""true"",
  ""options"": """",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 0e6f5387-b9c4-4fdd-9349-73e8424c6788 >>
			{
				var id = new Guid("0e6f5387-b9c4-4fdd-9349-73e8424c6788");
				Guid? parentId = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Account"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.account_id\"",\""default\"":\""\""}"",
  ""name"": ""account_id"",
  ""try_connect_to_entity"": ""false"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllAccountsSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""0""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 5c8d449d-95b8-419b-9851-b9a227e7093b >>
			{
				var id = new Guid("5c8d449d-95b8-419b-9851-b9a227e7093b");
				Guid? parentId = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Project lead"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.owner_id\"",\""default\"":\""\""}"",
  ""name"": ""owner_id"",
  ""try_connect_to_entity"": ""false"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllUsersSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: f0de1f0c-b71d-4002-a547-4e6d08654ea8 >>
			{
				var id = new Guid("f0de1f0c-b71d-4002-a547-4e6d08654ea8");
				Guid? parentId = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""End date"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.end_date\"",\""default\"":\""\""}"",
  ""name"": ""end_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 7bc83302-6b26-46ef-a6a1-3e656527faef >>
			{
				var id = new Guid("7bc83302-6b26-46ef-a6a1-3e656527faef");
				Guid? parentId = new Guid("7bbf3667-a26d-48d4-8eba-8ca5e03d14c3");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column1";
				var options = @"{
  ""visible_columns"": 3,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: bf38fcf4-adeb-4388-ad4b-6aa4485f9258 >>
			{
				var id = new Guid("bf38fcf4-adeb-4388-ad4b-6aa4485f9258");
				Guid? parentId = new Guid("7bc83302-6b26-46ef-a6a1-3e656527faef");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldCheckbox";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Is Billable"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.is_billable\"",\""default\"":\""false\""}"",
  ""name"": ""is_billable"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""text_true"": """",
  ""text_false"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: c1e88619-37f4-4dd6-ae9b-f714191d02e3 >>
			{
				var id = new Guid("c1e88619-37f4-4dd6-ae9b-f714191d02e3");
				Guid? parentId = new Guid("7bc83302-6b26-46ef-a6a1-3e656527faef");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Billing method"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.billing_method\"",\""default\"":\""\""}"",
  ""name"": ""billing_method"",
  ""try_connect_to_entity"": ""true"",
  ""options"": """",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 50f07e9e-65a5-4feb-bf2a-4f12712305c2 >>
			{
				var id = new Guid("50f07e9e-65a5-4feb-bf2a-4f12712305c2");
				Guid? parentId = new Guid("7bc83302-6b26-46ef-a6a1-3e656527faef");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Hour rate"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.hour_rate\"",\""default\"":\""\""}"",
  ""name"": ""hour_rate"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: e15e2d00-e704-4212-a7d2-ee125dd687a6 >>
			{
				var id = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 8,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 4,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 754bf941-df31-4b13-ba32-eb3c7a8c8922 >>
			{
				var id = new Guid("754bf941-df31-4b13-ba32-eb3c7a8c8922");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.subject\"",\""default\"":\""\""}"",
  ""name"": ""subject"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 6e918333-a2fa-4cf7-9ca8-662e349625a7 >>
			{
				var id = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Budget"",
  ""title_tag"": ""h3"",
  ""is_card"": ""false"",
  ""class"": """",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""2"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: aa94aac4-5048-4d82-95b2-b38536028cbb >>
			{
				var id = new Guid("aa94aac4-5048-4d82-95b2-b38536028cbb");
				Guid? parentId = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.estimated_minutes\"",\""default\"":\""\""}"",
  ""name"": ""estimated_minutes"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 857698b9-f715-480a-bd74-29819a4dec2d >>
			{
				var id = new Guid("857698b9-f715-480a-bd74-29819a4dec2d");
				Guid? parentId = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.x_billable_minutes\"",\""default\"":\""\""}"",
  ""name"": ""x_billable_minutes"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""2"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: ddde395b-6cee-4907-a220-a8424e091b13 >>
			{
				var id = new Guid("ddde395b-6cee-4907-a220-a8424e091b13");
				Guid? parentId = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.x_nonbillable_minutes\"",\""default\"":\""\""}"",
  ""name"": ""x_nonbillable_minutes"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""2"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: d076f406-7ddd-4feb-b96a-137e10c2d14e >>
			{
				var id = new Guid("d076f406-7ddd-4feb-b96a-137e10c2d14e");
				Guid? parentId = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Project"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""TaskAuxData[0].$project_nn_task[0].name\"",\""default\"":\""Project name\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""2"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 452a6f4c-b415-409a-b9b6-a2918a137299 >>
			{
				var id = new Guid("452a6f4c-b415-409a-b9b6-a2918a137299");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""Activity"",
  ""title_tag"": ""h3"",
  ""is_card"": ""false"",
  ""class"": ""mt-5"",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 164261ae-2df4-409a-8fdd-adc85c86a6dc >>
			{
				var id = new Guid("164261ae-2df4-409a-8fdd-adc85c86a6dc");
				Guid? parentId = new Guid("452a6f4c-b415-409a-b9b6-a2918a137299");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcTabNav";
				var containerId = "body";
				var options = @"{
  ""visible_tabs"": 3,
  ""render_type"": ""1"",
  ""css_class"": ""mt-4"",
  ""body_css_class"": ""pt-4"",
  ""tab1_label"": ""Comments"",
  ""tab2_label"": ""Feed"",
  ""tab3_label"": ""Timelog"",
  ""tab4_label"": ""Tab 4"",
  ""tab5_label"": ""Tab 5"",
  ""tab6_label"": ""Tab 6"",
  ""tab7_label"": ""Tab 7""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 05459068-33a7-454e-a871-94f9ddc6e5d5 >>
			{
				var id = new Guid("05459068-33a7-454e-a871-94f9ddc6e5d5");
				Guid? parentId = new Guid("164261ae-2df4-409a-8fdd-adc85c86a6dc");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcFeedList";
				var containerId = "tab2";
				var options = @"{
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""FeedItemsForRecordId\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 8099e123-1218-4008-b8e6-8ff56678d64a >>
			{
				var id = new Guid("8099e123-1218-4008-b8e6-8ff56678d64a");
				Guid? parentId = new Guid("164261ae-2df4-409a-8fdd-adc85c86a6dc");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcTimelogList";
				var containerId = "tab3";
				var options = @"{
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""TimeLogsForRecordId\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 3e15a63d-8f5f-4357-a692-b5998c31d543 >>
			{
				var id = new Guid("3e15a63d-8f5f-4357-a692-b5998c31d543");
				Guid? parentId = new Guid("164261ae-2df4-409a-8fdd-adc85c86a6dc");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcPostList";
				var containerId = "tab1";
				var options = @"{
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""CommentsForRecordId\"",\""default\"":\""\""}"",
  ""mode"": ""comments""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: ecc262e9-fbad-4dd1-9c98-56ad047685fb >>
			{
				var id = new Guid("ecc262e9-fbad-4dd1-9c98-56ad047685fb");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""People"",
  ""title_tag"": ""h3"",
  ""is_card"": ""false"",
  ""class"": ""mb-4"",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""2"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: bbe36a16-9210-415b-95f3-912482d27fd2 >>
			{
				var id = new Guid("bbe36a16-9210-415b-95f3-912482d27fd2");
				Guid? parentId = new Guid("ecc262e9-fbad-4dd1-9c98-56ad047685fb");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.created_by\"",\""default\"":\""\""}"",
  ""name"": ""created_by"",
  ""try_connect_to_entity"": ""true"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllUsersSelectOption\"",\""default\"":\""\""}"",
  ""mode"": ""3""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 101245d5-1ff9-4eb3-ba28-0b29cb56a0ec >>
			{
				var id = new Guid("101245d5-1ff9-4eb3-ba28-0b29cb56a0ec");
				Guid? parentId = new Guid("ecc262e9-fbad-4dd1-9c98-56ad047685fb");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Owner"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.owner_id\"",\""default\"":\""\""}"",
  ""name"": ""owner_id"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllUsersSelectOption\"",\""default\"":\""\""}"",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: f2175b92-4941-4cbe-ba4b-305167b6738b >>
			{
				var id = new Guid("f2175b92-4941-4cbe-ba4b-305167b6738b");
				Guid? parentId = new Guid("ecc262e9-fbad-4dd1-9c98-56ad047685fb");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcHtmlBlock";
				var containerId = "body";
				var options = @"{
  ""html"": ""{\""type\"":\""2\"",\""string\"":\""<script src=\\\""/api/v3.0/p/project/files/javascript?file=task-details.js\\\"" type=\\\""text/javascript\\\""></script>\"",\""default\"":\""\""}""
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 9f15bb3a-b6bf-424c-9394-669cc2041215 >>
			{
				var id = new Guid("9f15bb3a-b6bf-424c-9394-669cc2041215");
				Guid? parentId = new Guid("ecc262e9-fbad-4dd1-9c98-56ad047685fb");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Watchers"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing System.Linq;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry{\\n\\t\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\t\\tif (pageModel == null)\\n\\t\\t\\t\\treturn null;\\n\\n\\t\\t\\tvar taskAuxData = pageModel.TryGetDataSourceProperty<EntityRecordList>(\\\""TaskAuxData\\\"");\\n\\t        var currentUser = pageModel.TryGetDataSourceProperty<ErpUser>(\\\""CurrentUser\\\"");\\n\\t        var recordId = pageModel.TryGetDataSourceProperty<Guid>(\\\""RecordId\\\"");\\n\\t\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\t\\tif (taskAuxData == null && !taskAuxData.Any())\\n\\t\\t\\t\\treturn \\\""\\\"";\\n\\t        var watcherIdList = new List<Guid>();\\n\\t        if(taskAuxData[0].Properties.ContainsKey(\\\""$user_nn_task_watchers\\\"") && taskAuxData[0][\\\""$user_nn_task_watchers\\\""] != null \\n\\t            && taskAuxData[0][\\\""$user_nn_task_watchers\\\""] is List<EntityRecord>){\\n\\t                watcherIdList = ((List<EntityRecord>)taskAuxData[0][\\\""$user_nn_task_watchers\\\""]).Select(x=> (Guid)x[\\\""id\\\""]).ToList();\\n\\t            }\\n\\t        var watcherCount = watcherIdList.Count;\\n\\t        var currentUserIsWatching = false;\\n\\t        if(currentUser != null && watcherIdList.Contains(currentUser.Id))\\n\\t            currentUserIsWatching = true;\\n\\t\\n\\t        var html = $\\\""<span class='badge go-bkg-blue-gray-light mr-3'>{watcherCount}</span>\\\"";\\n\\t        if(currentUserIsWatching)\\n\\t            html += \\\""<a href=\\\\\\\""#\\\\\\\"" onclick=\\\\\\\""StopTaskWatch('\\\"" + recordId + \\\""')\\\\\\\"">stop watching</a>\\\"";\\n\\t        else\\n\\t            html += \\\""<a href=\\\\\\\""#\\\\\\\"" onclick=\\\\\\\""StartTaskWatch('\\\"" + recordId + \\\""')\\\\\\\"">start watching</a>\\\"";\\n\\t\\n\\t\\t\\treturn html;\\n\\t\\t}\\n\\t\\tcatch(Exception ex){\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""field"",
  ""mode"": ""2"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1"",
  ""connected_entity_id"": """"
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 27843f6e-43ed-49e7-9cc5-ec35393e93f4 >>
			{
				var id = new Guid("27843f6e-43ed-49e7-9cc5-ec35393e93f4");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.body\"",\""default\"":\""\""}"",
  ""name"": ""body"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 651e5fb2-56df-4c46-86b3-19a641dc942d >>
			{
				var id = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Dates"",
  ""title_tag"": ""h3"",
  ""is_card"": ""false"",
  ""class"": ""mb-4"",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""2"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 798e39b3-7a36-406b-bed6-e77da68fc50f >>
			{
				var id = new Guid("798e39b3-7a36-406b-bed6-e77da68fc50f");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDateTime";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.start_time\"",\""default\"":\""\""}"",
  ""name"": ""start_time"",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 291477ec-dd9c-4fc3-97a0-d2fd62809b2f >>
			{
				var id = new Guid("291477ec-dd9c-4fc3-97a0-d2fd62809b2f");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDateTime";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.end_time\"",\""default\"":\""\""}"",
  ""name"": ""end_time"",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: b2935724-bfcc-4821-bdb2-81bc9b14f015 >>
			{
				var id = new Guid("b2935724-bfcc-4821-bdb2-81bc9b14f015");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDateTime";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Created on"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.created_on\"",\""default\"":\""\""}"",
  ""name"": ""created_on"",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 5;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: b105d13c-3710-4ace-b51f-b57323912524 >>
			{
				var id = new Guid("b105d13c-3710-4ace-b51f-b57323912524");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Details"",
  ""title_tag"": ""h3"",
  ""is_card"": ""false"",
  ""class"": ""mb-4"",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 70a864dc-8311-4dd3-bc13-1a3b87821e30 >>
			{
				var id = new Guid("70a864dc-8311-4dd3-bc13-1a3b87821e30");
				Guid? parentId = new Guid("b105d13c-3710-4ace-b51f-b57323912524");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Status"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.status_id\"",\""default\"":\""\""}"",
  ""name"": ""status_id"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""TaskStatusesSelectOption\"",\""default\"":\""\""}"",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 1fff4a92-d045-4019-b27c-bccb1fd1cb82 >>
			{
				var id = new Guid("1fff4a92-d045-4019-b27c-bccb1fd1cb82");
				Guid? parentId = new Guid("b105d13c-3710-4ace-b51f-b57323912524");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Type"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.type_id\"",\""default\"":\""\""}"",
  ""name"": ""type_id"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""TaskTypesSelectOption\"",\""default\"":\""\""}"",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: ee526509-7840-498a-9c1f-8a69d80c5f2e >>
			{
				var id = new Guid("ee526509-7840-498a-9c1f-8a69d80c5f2e");
				Guid? parentId = new Guid("b105d13c-3710-4ace-b51f-b57323912524");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Priority"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.priority\"",\""default\"":\""\""}"",
  ""name"": ""priority"",
  ""options"": """",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 6f8f9a9a-a464-4175-9178-246b792738a6 >>
			{
				var id = new Guid("6f8f9a9a-a464-4175-9178-246b792738a6");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""Record.abbr\"",\""default\"":\""\""}"",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-tachometer-alt"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 3ad8d6e5-eed7-44b7-95e1-12f22714037b >>
			{
				var id = new Guid("3ad8d6e5-eed7-44b7-95e1-12f22714037b");
				Guid? parentId = new Guid("6f8f9a9a-a464-4175-9178-246b792738a6");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "toolbar";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar projectId = pageModel.TryGetDataSourceProperty<Guid>(\\\""Record.id\\\"");\\n        var pageName = pageModel.TryGetDataSourceProperty<string>(\\\""Page.Name\\\"");\\n\\n\\t\\tif (projectId == null || pageName == null)\\n\\t\\t\\treturn null;\\n\\n        var result = $\\\""<a href='/projects/projects/projects/r/{projectId}/dashboard' class='btn btn-link btn-sm {(pageName == \\\""dashboard\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Dashboard</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/feed' class='btn btn-link btn-sm {(pageName == \\\""feed\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Feed</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/b1db4466-7423-44e9-b6b9-3063222c9e15/l/tasks' class='btn btn-link btn-sm {(pageName == \\\""tasks\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Tasks</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/55c8d6e2-f26d-4689-9d1b-a8c1b9de1672/l/milestones' class='btn btn-link btn-sm {(pageName == \\\""milestones\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Milestones</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/details' class='btn btn-link btn-sm {(pageName == \\\""details\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Details</a>\\\"";\\n\\t\\treturn result;\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 89cb4088-ea04-4ce2-8cbe-5367c5741ef3 >>
			{
				var id = new Guid("89cb4088-ea04-4ce2-8cbe-5367c5741ef3");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""Entity.IconName\"",\""default\"":\""\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 8cbdfd1a-5d0e-4961-8e79-74072c133202 >>
			{
				var id = new Guid("8cbdfd1a-5d0e-4961-8e79-74072c133202");
				Guid? parentId = new Guid("89cb4088-ea04-4ce2-8cbe-5367c5741ef3");
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""Create Project"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-plus go-green"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""/projects/projects/projects/c"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 2d3dddf7-cefb-4073-977f-4e1b6bf8935e >>
			{
				var id = new Guid("2d3dddf7-cefb-4073-977f-4e1b6bf8935e");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 3,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 5;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 747c108b-ed45-46f3-b06a-113e2490888d >>
			{
				var id = new Guid("747c108b-ed45-46f3-b06a-113e2490888d");
				Guid? parentId = new Guid("2d3dddf7-cefb-4073-977f-4e1b6bf8935e");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Hour rate"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.hour_rate\"",\""default\"":\""\""}"",
  ""name"": ""hour_rate"",
  ""mode"": ""3"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: df7c7cab-0e16-4e75-bb13-04666afeff81 >>
			{
				var id = new Guid("df7c7cab-0e16-4e75-bb13-04666afeff81");
				Guid? parentId = new Guid("2d3dddf7-cefb-4073-977f-4e1b6bf8935e");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Billing method"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.billing_method\"",\""default\"":\""\""}"",
  ""name"": ""billing_method"",
  ""options"": """",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 302de86f-7178-4e2b-9ac1-d447163a9558 >>
			{
				var id = new Guid("302de86f-7178-4e2b-9ac1-d447163a9558");
				Guid? parentId = new Guid("2d3dddf7-cefb-4073-977f-4e1b6bf8935e");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldCheckbox";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Is Billable"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.is_billable\"",\""default\"":\""\""}"",
  ""name"": ""is_billable"",
  ""mode"": ""3"",
  ""text_true"": """",
  ""text_false"": """",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 86506ad8-e1cb-4b46-84b9-881e0326ebaa >>
			{
				var id = new Guid("86506ad8-e1cb-4b46-84b9-881e0326ebaa");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""Projects\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""ParentRecord.abbr\"",\""default\"":\""Abbr\""}"",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""ParentRecord.name\"",\""default\"":\""Project name\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""ParentEntity.Color\"",\""default\"":\""#9c27b0\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""ParentEntity.IconName\"",\""default\"":\""fa fa-file\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 94bfe723-e5f6-478d-afb6-504edf2bdc2b >>
			{
				var id = new Guid("94bfe723-e5f6-478d-afb6-504edf2bdc2b");
				Guid? parentId = new Guid("86506ad8-e1cb-4b46-84b9-881e0326ebaa");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""Search"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-search"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer','open')"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 5891b0d8-6750-4502-bd8e-fe1380f08b0c >>
			{
				var id = new Guid("5891b0d8-6750-4502-bd8e-fe1380f08b0c");
				Guid? parentId = new Guid("86506ad8-e1cb-4b46-84b9-881e0326ebaa");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "toolbar";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar projectId = pageModel.TryGetDataSourceProperty<Guid>(\\\""ParentRecord.id\\\"");\\n        var pageName = pageModel.TryGetDataSourceProperty<string>(\\\""Page.Name\\\"");\\n\\n\\t\\tif (projectId == null || pageName == null)\\n\\t\\t\\treturn null;\\n\\n        var result = $\\\""<a href='/projects/projects/projects/r/{projectId}/dashboard' class='btn btn-link btn-sm {(pageName == \\\""dashboard\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Dashboard</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/feed' class='btn btn-link btn-sm {(pageName == \\\""feed\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Feed</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/b1db4466-7423-44e9-b6b9-3063222c9e15/l/tasks' class='btn btn-link btn-sm {(pageName == \\\""tasks\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Tasks</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/55c8d6e2-f26d-4689-9d1b-a8c1b9de1672/l/milestones' class='btn btn-link btn-sm {(pageName == \\\""milestones\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Milestones</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/details' class='btn btn-link btn-sm {(pageName == \\\""details\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Details</a>\\\"";\\n\\t\\treturn result;\\n\\t}\\n}\\n\"",\""default\"":\""Project sub navigation\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 94be4b02-07ea-4a54-a6fc-89316fa1e90a >>
			{
				var id = new Guid("94be4b02-07ea-4a54-a6fc-89316fa1e90a");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""Record.abbr\"",\""default\"":\""\""}"",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""ParentEntity.Color\"",\""default\"":\""#9c27b0\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-info"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 0c295451-1c38-4eb0-8000-feefe912a667 >>
			{
				var id = new Guid("0c295451-1c38-4eb0-8000-feefe912a667");
				Guid? parentId = new Guid("94be4b02-07ea-4a54-a6fc-89316fa1e90a");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "toolbar";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar projectId = pageModel.TryGetDataSourceProperty<Guid>(\\\""Record.id\\\"");\\n        var pageName = pageModel.TryGetDataSourceProperty<string>(\\\""Page.Name\\\"");\\n\\n\\t\\tif (projectId == null || pageName == null)\\n\\t\\t\\treturn null;\\n\\n        var result = $\\\""<a href='/projects/projects/projects/r/{projectId}/dashboard' class='btn btn-link btn-sm {(pageName == \\\""dashboard\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Dashboard</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/feed' class='btn btn-link btn-sm {(pageName == \\\""feed\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Feed</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/b1db4466-7423-44e9-b6b9-3063222c9e15/l/tasks' class='btn btn-link btn-sm {(pageName == \\\""tasks\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Tasks</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/55c8d6e2-f26d-4689-9d1b-a8c1b9de1672/l/milestones' class='btn btn-link btn-sm {(pageName == \\\""milestones\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Milestones</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/details' class='btn btn-link btn-sm {(pageName == \\\""details\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Details</a>\\\"";\\n\\t\\treturn result;\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: feed  id: 2684f725-38e2-4f8c-92ee-e3b1ccf04aff >>
			{
				var id = new Guid("2684f725-38e2-4f8c-92ee-e3b1ccf04aff");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("acb76466-32b8-428c-81cb-47b6013879e7");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcFeedList";
				var containerId = "";
				var options = @"{
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""FeedItemsForRecordId\"",\""default\"":\""\""}""
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 6029e40b-0835-460f-b782-1e4228ea4234 >>
			{
				var id = new Guid("6029e40b-0835-460f-b782-1e4228ea4234");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 8,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: be6aa619-e380-4bf9-b279-47dda4d5f4eb >>
			{
				var id = new Guid("be6aa619-e380-4bf9-b279-47dda4d5f4eb");
				Guid? parentId = new Guid("6029e40b-0835-460f-b782-1e4228ea4234");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Description"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.description\"",\""default\"":\""\""}"",
  ""name"": ""description"",
  ""mode"": ""3"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1"",
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 0dbdb202-7288-49e6-b922-f69e947590e5 >>
			{
				var id = new Guid("0dbdb202-7288-49e6-b922-f69e947590e5");
				Guid? parentId = new Guid("6029e40b-0835-460f-b782-1e4228ea4234");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Abbreviation"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.abbr\"",\""default\"":\""\""}"",
  ""name"": ""abbr"",
  ""mode"": ""2"",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 7f01d2c0-2542-4b88-b8f0-711947e4d0c6 >>
			{
				var id = new Guid("7f01d2c0-2542-4b88-b8f0-711947e4d0c6");
				Guid? parentId = new Guid("6029e40b-0835-460f-b782-1e4228ea4234");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Name"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""name"": ""name"",
  ""mode"": ""3"",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: milestones  id: fac5a2f6-b1b4-402a-bf0d-e0a3fb4dd36a >>
			{
				var id = new Guid("fac5a2f6-b1b4-402a-bf0d-e0a3fb4dd36a");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("d07cbf70-09c6-47ee-9a13-80568e43d331");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""Projects\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""ParentRecord.abbr\"",\""default\"":\""abbr\""}"",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""ParentRecord.name\"",\""default\"":\""Project name\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""ParentEntity.Color\"",\""default\"":\""#9c27b0\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""Entity.IconName\"",\""default\"":\""fa fa-file\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: milestones  id: 4a059596-3804-435e-b535-2da1f56abb29 >>
			{
				var id = new Guid("4a059596-3804-435e-b535-2da1f56abb29");
				Guid? parentId = new Guid("fac5a2f6-b1b4-402a-bf0d-e0a3fb4dd36a");
				Guid? nodeId = null;
				var pageId = new Guid("d07cbf70-09c6-47ee-9a13-80568e43d331");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "toolbar";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar projectId = pageModel.TryGetDataSourceProperty<Guid>(\\\""ParentRecord.id\\\"");\\n        var pageName = pageModel.TryGetDataSourceProperty<string>(\\\""Page.Name\\\"");\\n\\n\\t\\tif (projectId == null || pageName == null)\\n\\t\\t\\treturn null;\\n\\n        var result = $\\\""<a href='/projects/projects/projects/r/{projectId}/dashboard' class='btn btn-link btn-sm {(pageName == \\\""dashboard\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Dashboard</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/feed' class='btn btn-link btn-sm {(pageName == \\\""feed\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Feed</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/b1db4466-7423-44e9-b6b9-3063222c9e15/l/tasks' class='btn btn-link btn-sm {(pageName == \\\""tasks\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Tasks</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/55c8d6e2-f26d-4689-9d1b-a8c1b9de1672/l/milestones' class='btn btn-link btn-sm {(pageName == \\\""milestones\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Milestones</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/details' class='btn btn-link btn-sm {(pageName == \\\""details\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Details</a>\\\"";\\n\\t\\treturn result;\\n\\t}\\n}\\n\"",\""default\"":\""Project subnavigation\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: edc68b26-d508-4c2e-a431-5a6656957944 >>
			{
				var id = new Guid("edc68b26-d508-4c2e-a431-5a6656957944");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""Entity.IconName\"",\""default\"":\""\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: 719ca43f-bb66-4134-a2d2-ad2cc30ade6d >>
			{
				var id = new Guid("719ca43f-bb66-4134-a2d2-ad2cc30ade6d");
				Guid? parentId = new Guid("edc68b26-d508-4c2e-a431-5a6656957944");
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""Search"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-search"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer','open')"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: b6951134-f57f-4da2-8203-a8c36cc99fd7 >>
			{
				var id = new Guid("b6951134-f57f-4da2-8203-a8c36cc99fd7");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""Projects\""}"",
  ""area_sublabel"": """",
  ""title"": ""Create task"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""App.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-plus"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: bd3ed9ae-90aa-4373-9eb9-cc677353bc6d >>
			{
				var id = new Guid("bd3ed9ae-90aa-4373-9eb9-cc677353bc6d");
				Guid? parentId = new Guid("b6951134-f57f-4da2-8203-a8c36cc99fd7");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Create task"",
  ""color"": ""1"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-save"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": ""CreateRecord""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 48105732-6025-4614-9065-55647afa9b96 >>
			{
				var id = new Guid("48105732-6025-4614-9065-55647afa9b96");
				Guid? parentId = new Guid("b6951134-f57f-4da2-8203-a8c36cc99fd7");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""Cancel"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""0\"",\""string\"":\""ReturnUrl\"",\""default\"":\""/projects/dashboard/dashboard/a/dashboard\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 1af3c0cb-a58e-4d19-89a2-2ce4b8e60945 >>
			{
				var id = new Guid("1af3c0cb-a58e-4d19-89a2-2ce4b8e60945");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "";
				var options = @"{
  ""id"": ""CreateRecord"",
  ""name"": ""CreateRecord"",
  ""hook_key"": """",
  ""method"": ""post"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: a1110167-15bd-46b7-ae3c-cc8ba87be98f >>
			{
				var id = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? parentId = new Guid("1af3c0cb-a58e-4d19-89a2-2ce4b8e60945");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 8,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 884a8db1-aff0-4f86-ab7d-8fb17698fc33 >>
			{
				var id = new Guid("884a8db1-aff0-4f86-ab7d-8fb17698fc33");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDateTime";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.start_time\"",\""default\"":\""\""}"",
  ""name"": ""start_time"",
  ""mode"": ""0"",
  ""connected_entity_id"": """"
}";
				var weight = 8;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: ecef4b2c-6988-44c1-acea-0e28385ec528 >>
			{
				var id = new Guid("ecef4b2c-6988-44c1-acea-0e28385ec528");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDateTime";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.end_time\"",\""default\"":\""\""}"",
  ""name"": ""end_time"",
  ""mode"": ""0"",
  ""connected_entity_id"": """"
}";
				var weight = 9;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 4a588a7d-ea03-4be1-ab0d-3120d98c3548 >>
			{
				var id = new Guid("4a588a7d-ea03-4be1-ab0d-3120d98c3548");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "column2";
				var options = @"{
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing System.Globalization;\\n\\npublic class SampleCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\treturn DateTime.UtcNow.ToString(\\\""o\\\"", CultureInfo.InvariantCulture);\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""created_on"",
  ""try_connect_to_entity"": ""false""
}";
				var weight = 5;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: e03e40c2-dae2-4351-947c-02295a064328 >>
			{
				var id = new Guid("e03e40c2-dae2-4351-947c-02295a064328");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Type Id"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.type_id\"",\""default\"":\""a0465e9f-5d5f-433d-acf1-1da0eaec78b4\""}"",
  ""name"": ""type_id"",
  ""try_connect_to_entity"": ""true"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""TaskTypeSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""0""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 23200f02-439a-4719-ae0e-498c9dcde58c >>
			{
				var id = new Guid("23200f02-439a-4719-ae0e-498c9dcde58c");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Owner"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.owner_id\"",\""default\"":\""\""}"",
  ""name"": ""owner_id"",
  ""try_connect_to_entity"": ""false"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllUsersSelectOption\"",\""default\"":\""\""}"",
  ""mode"": ""0""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: fb3d0142-e080-43ef-ba31-a79d0221c0df >>
			{
				var id = new Guid("fb3d0142-e080-43ef-ba31-a79d0221c0df");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Subject"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.subject\"",\""default\"":\""\""}"",
  ""name"": ""subject"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: fe59f151-1e79-4df2-a034-9f45f1dcd691 >>
			{
				var id = new Guid("fe59f151-1e79-4df2-a034-9f45f1dcd691");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Description"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.body\"",\""default\"":\""\""}"",
  ""name"": ""body"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""3""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 30e99568-2727-4ce4-8da6-c97feaaf4432 >>
			{
				var id = new Guid("30e99568-2727-4ce4-8da6-c97feaaf4432");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "column2";
				var options = @"{
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""CurrentUser.Id\"",\""default\"":\""\""}"",
  ""name"": ""created_by"",
  ""try_connect_to_entity"": ""false""
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 1739a2f0-76ba-4343-a344-9b0564096d06 >>
			{
				var id = new Guid("1739a2f0-76ba-4343-a344-9b0564096d06");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Project"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry{\\n\\t\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\t\\tif (pageModel == null)\\n\\t\\t\\t\\treturn null;\\n\\t\\n\\t\\t\\t//try read data source by name and get result as specified type object\\n\\t\\t\\tvar record = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""Record\\\"");\\n\\n\\t\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\t\\tif (record == null)\\n\\t\\t\\t\\treturn null;\\n\\n            if(record.Properties.ContainsKey(\\\""$project_nn_task.id\\\"")){\\n                var relationObject = record[\\\""$project_nn_task.id\\\""];\\n                if(relationObject is List<Guid> && ((List<Guid>)relationObject).Count > 0){\\n                    return ((List<Guid>)relationObject)[0];\\n                }\\n            }\\n\\t\\t\\treturn record;\\n\\t\\t}\\n\\t\\tcatch(Exception ex){\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""$project_nn_task.id"",
  ""try_connect_to_entity"": ""false"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllProjectsSelectOption\"",\""default\"":\""\""}"",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 7612914f-21ea-4665-9b66-385cf1cafb41 >>
			{
				var id = new Guid("7612914f-21ea-4665-9b66-385cf1cafb41");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""Entity.IconName\"",\""default\"":\""\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: d7ef95ce-8508-4722-a5f0-3d114bda4585 >>
			{
				var id = new Guid("d7ef95ce-8508-4722-a5f0-3d114bda4585");
				Guid? parentId = new Guid("7612914f-21ea-4665-9b66-385cf1cafb41");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""Search"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-search"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer','open')"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 7e2d1d10-a9cc-4eae-b3d6-a30ab3647102 >>
			{
				var id = new Guid("7e2d1d10-a9cc-4eae-b3d6-a30ab3647102");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": ""mb-3"",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 66828292-07c7-4cc1-9060-a92798d6b95a >>
			{
				var id = new Guid("66828292-07c7-4cc1-9060-a92798d6b95a");
				Guid? parentId = new Guid("7e2d1d10-a9cc-4eae-b3d6-a30ab3647102");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""Timesheet"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": ""pt-3 pb-3"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 6bf0435c-f9c0-44b7-a801-00222cd7c0bb >>
			{
				var id = new Guid("6bf0435c-f9c0-44b7-a801-00222cd7c0bb");
				Guid? parentId = new Guid("66828292-07c7-4cc1-9060-a92798d6b95a");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetTimesheet";
				var containerId = "body";
				var options = @"{
  ""project_id"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 0c29732a-945e-4bbc-9486-b86efb2897b2 >>
			{
				var id = new Guid("0c29732a-945e-4bbc-9486-b86efb2897b2");
				Guid? parentId = new Guid("7e2d1d10-a9cc-4eae-b3d6-a30ab3647102");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column1";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": ""mb-3"",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: a94793d2-492a-4b7e-9fac-199f8bf46f46 >>
			{
				var id = new Guid("a94793d2-492a-4b7e-9fac-199f8bf46f46");
				Guid? parentId = new Guid("0c29732a-945e-4bbc-9486-b86efb2897b2");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""Tasks"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm h-100"",
  ""body_class"": ""p-3 align-center-col"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: bcf153c5-8c7c-4ac8-a5d9-04e288ff7ccf >>
			{
				var id = new Guid("bcf153c5-8c7c-4ac8-a5d9-04e288ff7ccf");
				Guid? parentId = new Guid("a94793d2-492a-4b7e-9fac-199f8bf46f46");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetTasksChart";
				var containerId = "body";
				var options = @"{
  ""project_id"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 5466f4d1-20a5-4808-8bb5-aefaac756347 >>
			{
				var id = new Guid("5466f4d1-20a5-4808-8bb5-aefaac756347");
				Guid? parentId = new Guid("0c29732a-945e-4bbc-9486-b86efb2897b2");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Budget"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm h-100"",
  ""body_class"": ""p-3 align-center-col "",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 4c58c1bb-321a-41c6-954f-4c6fafe6661c >>
			{
				var id = new Guid("4c58c1bb-321a-41c6-954f-4c6fafe6661c");
				Guid? parentId = new Guid("5466f4d1-20a5-4808-8bb5-aefaac756347");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetBudgetChart";
				var containerId = "body";
				var options = @"{
  ""project_id"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 734e7201-a15e-4ae5-8ea9-b683a94f80d0 >>
			{
				var id = new Guid("734e7201-a15e-4ae5-8ea9-b683a94f80d0");
				Guid? parentId = new Guid("7e2d1d10-a9cc-4eae-b3d6-a30ab3647102");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Tasks Due Today"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm"",
  ""body_class"": ""pb-3 pt-3"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 8fbba43e-cd4d-4b0d-9e3e-7d19a2cc8468 >>
			{
				var id = new Guid("8fbba43e-cd4d-4b0d-9e3e-7d19a2cc8468");
				Guid? parentId = new Guid("734e7201-a15e-4ae5-8ea9-b683a94f80d0");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetTasksQueue";
				var containerId = "body";
				var options = @"{
  ""project_id"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}"",
  ""user_id"": """",
  ""type"": ""2""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: f1c53374-0efb-4612-ab94-68e8e8242ddb >>
			{
				var id = new Guid("f1c53374-0efb-4612-ab94-68e8e8242ddb");
				Guid? parentId = new Guid("7e2d1d10-a9cc-4eae-b3d6-a30ab3647102");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""Task distribution"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": ""pt-3 pb-3"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: becc4486-be49-4fa6-9d3a-0d8e15606fcc >>
			{
				var id = new Guid("becc4486-be49-4fa6-9d3a-0d8e15606fcc");
				Guid? parentId = new Guid("f1c53374-0efb-4612-ab94-68e8e8242ddb");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetTaskDistribution";
				var containerId = "body";
				var options = @"{
  ""project_id"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 39fa86aa-3d7a-49af-bfa9-30f1c03671eb >>
			{
				var id = new Guid("39fa86aa-3d7a-49af-bfa9-30f1c03671eb");
				Guid? parentId = new Guid("7e2d1d10-a9cc-4eae-b3d6-a30ab3647102");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Overdue tasks"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": ""pb-3 pt-3"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 27a635a7-143c-4a28-bf08-601771a453c1 >>
			{
				var id = new Guid("27a635a7-143c-4a28-bf08-601771a453c1");
				Guid? parentId = new Guid("39fa86aa-3d7a-49af-bfa9-30f1c03671eb");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcProjectWidgetTasksQueue";
				var containerId = "body";
				var options = @"{
  ""project_id"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}"",
  ""user_id"": """",
  ""type"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: cb0e42ee-aa06-4a92-8bb0-940e7332411e >>
			{
				var id = new Guid("cb0e42ee-aa06-4a92-8bb0-940e7332411e");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcDrawer";
				var containerId = "";
				var options = @"{
  ""title"": ""Search Tasks"",
  ""width"": ""550px"",
  ""class"": """",
  ""body_class"": """",
  ""title_action_html"": """"
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 156877b1-d1ea-4fea-be4a-62a982bef3a7 >>
			{
				var id = new Guid("156877b1-d1ea-4fea-be4a-62a982bef3a7");
				Guid? parentId = new Guid("cb0e42ee-aa06-4a92-8bb0-940e7332411e");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-156877b1-d1ea-4fea-be4a-62a982bef3a7"",
  ""name"": ""form"",
  ""hook_key"": """",
  ""method"": ""get"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: bef8058a-2c62-47a8-abd3-813636ebd4a8 >>
			{
				var id = new Guid("bef8058a-2c62-47a8-abd3-813636ebd4a8");
				Guid? parentId = new Guid("156877b1-d1ea-4fea-be4a-62a982bef3a7");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Search Tasks"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 319c5697-21c9-4799-ae6f-343586f5d2cf >>
			{
				var id = new Guid("319c5697-21c9-4799-ae6f-343586f5d2cf");
				Guid? parentId = new Guid("156877b1-d1ea-4fea-be4a-62a982bef3a7");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcGridFilterField";
				var containerId = "body";
				var options = @"{
  ""label"": ""Task contents"",
  ""name"": ""x_search"",
  ""try_connect_to_entity"": ""true"",
  ""field_type"": ""18"",
  ""query_type"": ""2"",
  ""query_options"": [
    ""2""
  ],
  ""prefix"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 630bea4c-bccf-4587-83f7-6d0d2ed5bac0 >>
			{
				var id = new Guid("630bea4c-bccf-4587-83f7-6d0d2ed5bac0");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""Record.key\"",\""default\"":\""NXT-1\""}"",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Record.subject\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Linq;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar record = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""Record\\\"");\\n        var taskTypes = pageModel.TryGetDataSourceProperty<EntityRecordList>(\\\""TaskTypes\\\"");\\n        \\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (record == null || !record.Properties.ContainsKey(\\\""type_id\\\"") || taskTypes == null)\\n\\t\\t\\treturn null;\\n\\n        var taskType = taskTypes.FirstOrDefault(x=> (Guid)x[\\\""id\\\""] == (Guid)record[\\\""type_id\\\""]);\\n        if(taskType != null && taskType.Properties.ContainsKey(\\\""color\\\"") && !String.IsNullOrWhiteSpace((string)taskType[\\\""color\\\""])){\\n            return (string)taskType[\\\""color\\\""];\\n        }\\n\\n\\t\\treturn null;\\n\\t}\\n}\\n\"",\""default\"":\""#999\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Linq;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar record = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""Record\\\"");\\n        var taskTypes = pageModel.TryGetDataSourceProperty<EntityRecordList>(\\\""TaskTypes\\\"");\\n        \\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (record == null || !record.Properties.ContainsKey(\\\""type_id\\\"") || taskTypes == null)\\n\\t\\t\\treturn null;\\n\\n        var taskType = taskTypes.FirstOrDefault(x=> (Guid)x[\\\""id\\\""] == (Guid)record[\\\""type_id\\\""]);\\n        if(taskType != null && taskType.Properties.ContainsKey(\\\""icon_class\\\"") && !String.IsNullOrWhiteSpace((string)taskType[\\\""icon_class\\\""])){\\n            return (string)taskType[\\\""icon_class\\\""];\\n        }\\n\\n\\t\\treturn null;\\n\\t}\\n}\\n\"",\""default\"":\""fas fa-user-cog\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: b14bb20c-fab7-40a4-8feb-8a899b761dda >>
			{
				var id = new Guid("b14bb20c-fab7-40a4-8feb-8a899b761dda");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcHtmlBlock";
				var containerId = "";
				var options = @"{
  ""html"": ""{\""type\"":\""2\"",\""string\"":\""<script src=\\\""/api/v3.0/p/project/files/javascript?file=timetrack.js\\\"" type=\\\""text/javascript\\\""></script>\"",\""default\"":\""\""}""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: account-monthly-timelog  id: ca7a9302-afc3-4688-9748-676211bcddb3 >>
			{
				var id = new Guid("ca7a9302-afc3-4688-9748-676211bcddb3");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""#9C27B0"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-database"",
  ""return_url"": ""/projects/reports/list/a/list""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: a066475d-c2ff-4e59-9481-08cd637f71ca >>
			{
				var id = new Guid("a066475d-c2ff-4e59-9481-08cd637f71ca");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("84b892fc-6ca4-4c7e-8b7c-2f2f6954862f");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""#9C27B0"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-database"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4 >>
			{
				var id = new Guid("6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcModal";
				var containerId = "";
				var options = @"{
  ""title"": ""Create timelog"",
  ""backdrop"": ""true"",
  ""size"": ""2"",
  ""position"": ""0""
}";
				var weight = 5;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 25ac7fb9-2737-428d-9678-90222252c024 >>
			{
				var id = new Guid("25ac7fb9-2737-428d-9678-90222252c024");
				Guid? parentId = new Guid("6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "footer";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Create log"",
  ""color"": ""19"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-plus"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": ""wv-timetrack-log""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: d6b5ad6d-4455-4828-bc46-b072aa4919f5 >>
			{
				var id = new Guid("d6b5ad6d-4455-4828-bc46-b072aa4919f5");
				Guid? parentId = new Guid("6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-timetrack-log"",
  ""name"": ""TimeTrackCreateLog"",
  ""hook_key"": ""TimeTrackCreateLog"",
  ""method"": ""post"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 3658981b-cef7-4938-9c3a-a13cd5b760a0 >>
			{
				var id = new Guid("3658981b-cef7-4938-9c3a-a13cd5b760a0");
				Guid? parentId = new Guid("d6b5ad6d-4455-4828-bc46-b072aa4919f5");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 4,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: cd70f0e3-be35-4f94-8894-c8f26e021d88 >>
			{
				var id = new Guid("cd70f0e3-be35-4f94-8894-c8f26e021d88");
				Guid? parentId = new Guid("3658981b-cef7-4938-9c3a-a13cd5b760a0");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column4";
				var options = @"{
  ""label_text"": ""Log started on"",
  ""label_mode"": ""0"",
  ""value"": """",
  ""name"": ""timelog_started_on"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""1"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 6b3e9fec-7fc1-4455-8dcc-a3b67f4ca427 >>
			{
				var id = new Guid("6b3e9fec-7fc1-4455-8dcc-a3b67f4ca427");
				Guid? parentId = new Guid("3658981b-cef7-4938-9c3a-a13cd5b760a0");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Log Date"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""CurrentDate\"",\""default\"":\""\""}"",
  ""name"": ""logged_on"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 6b0ee717-f4af-47fb-b441-125a755af01b >>
			{
				var id = new Guid("6b0ee717-f4af-47fb-b441-125a755af01b");
				Guid? parentId = new Guid("3658981b-cef7-4938-9c3a-a13cd5b760a0");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldCheckbox";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Billable"",
  ""label_mode"": ""0"",
  ""value"": ""true"",
  ""name"": ""is_billable"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0"",
  ""text_true"": ""billable time"",
  ""text_false"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 9dcca796-cb6d-4c7f-bb63-761cff4c218a >>
			{
				var id = new Guid("9dcca796-cb6d-4c7f-bb63-761cff4c218a");
				Guid? parentId = new Guid("3658981b-cef7-4938-9c3a-a13cd5b760a0");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Logged Minutes"",
  ""label_mode"": ""0"",
  ""value"": """",
  ""name"": ""minutes"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: d11a258c-2ad3-4421-84db-990aa7683a2d >>
			{
				var id = new Guid("d11a258c-2ad3-4421-84db-990aa7683a2d");
				Guid? parentId = new Guid("d6b5ad6d-4455-4828-bc46-b072aa4919f5");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "body";
				var options = @"{
  ""value"": """",
  ""name"": ""task_id"",
  ""try_connect_to_entity"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 418b58a8-88d2-4dfc-b4a9-22617dab76c4 >>
			{
				var id = new Guid("418b58a8-88d2-4dfc-b4a9-22617dab76c4");
				Guid? parentId = new Guid("d6b5ad6d-4455-4828-bc46-b072aa4919f5");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldTextarea";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Description"",
  ""label_mode"": ""0"",
  ""value"": """",
  ""name"": ""body"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0"",
  ""height"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 9946104a-a6ec-4a0b-b996-7bc630c16287 >>
			{
				var id = new Guid("9946104a-a6ec-4a0b-b996-7bc630c16287");
				Guid? parentId = new Guid("6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "footer";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""Cancel"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:'wv-6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4',action:'close',payload:null})"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: account-monthly-timelog  id: ffac14be-00ee-4a72-a08e-f5b0956171c4 >>
			{
				var id = new Guid("ffac14be-00ee-4a72-a08e-f5b0956171c4");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "";
				var options = @"{
  ""id"": ""wv-ffac14be-00ee-4a72-a08e-f5b0956171c4"",
  ""name"": ""form"",
  ""hook_key"": """",
  ""method"": ""get"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: account-monthly-timelog  id: 7eff5a2c-5d5d-4989-a68f-a1362b0dad7c >>
			{
				var id = new Guid("7eff5a2c-5d5d-4989-a68f-a1362b0dad7c");
				Guid? parentId = new Guid("ffac14be-00ee-4a72-a08e-f5b0956171c4");
				Guid? nodeId = null;
				var pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 4,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 3,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_self_align"": ""1"",
  ""container1_flex_order"": 0,
  ""container2_span"": 3,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_self_align"": ""1"",
  ""container2_flex_order"": 0,
  ""container3_span"": 3,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_self_align"": ""1"",
  ""container3_flex_order"": 0,
  ""container4_span"": -1,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_self_align"": ""4"",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_self_align"": ""1"",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_self_align"": ""1"",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_self_align"": ""1"",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_self_align"": ""1"",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_self_align"": ""1"",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_self_align"": ""1"",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_self_align"": ""1"",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: account-monthly-timelog  id: 2e98b41a-f845-4ab1-aeaf-944ae963883b >>
			{
				var id = new Guid("2e98b41a-f845-4ab1-aeaf-944ae963883b");
				Guid? parentId = new Guid("7eff5a2c-5d5d-4989-a68f-a1362b0dad7c");
				Guid? nodeId = null;
				var pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "column4";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""generate"",
  ""color"": ""1"",
  ""size"": ""3"",
  ""class"": ""mb-3"",
  ""id"": """",
  ""icon_class"": ""fa fa-cog"",
  ""is_block"": ""true"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: account-monthly-timelog  id: 70424cd2-2b69-4c87-9977-cb60a72239fd >>
			{
				var id = new Guid("70424cd2-2b69-4c87-9977-cb60a72239fd");
				Guid? parentId = new Guid("7eff5a2c-5d5d-4989-a68f-a1362b0dad7c");
				Guid? nodeId = null;
				var pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Year"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SampleCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry\\n\\t\\t{\\n\\t\\t    var value = (string)pageModel.DataModel.GetProperty(\\\""RequestQuery.year\\\"");\\n\\t\\t    if(string.IsNullOrWhiteSpace(value))\\n\\t\\t\\t    return DateTime.Now.Year;\\n\\t\\t\\telse\\n\\t\\t\\t    return value;\\n\\t\\t}\\n\\t\\tcatch(PropertyDoesNotExistException ex)\\n\\t\\t{\\n\\t\\t  return DateTime.Now.Year;\\n\\t\\t}\\n\\t\\tcatch(Exception ex)\\n\\t\\t{\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\"",\""default\"":\""\""}"",
  ""name"": ""year"",
  ""mode"": ""0"",
  ""decimal_digits"": 0,
  ""min"": 2000,
  ""max"": 2100,
  ""step"": 1,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: account-monthly-timelog  id: 0c32036a-4432-4b17-beb7-198ba22ea134 >>
			{
				var id = new Guid("0c32036a-4432-4b17-beb7-198ba22ea134");
				Guid? parentId = new Guid("7eff5a2c-5d5d-4989-a68f-a1362b0dad7c");
				Guid? nodeId = null;
				var pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Month"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SampleCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry\\n\\t\\t{\\n\\t\\t    var value = (string)pageModel.DataModel.GetProperty(\\\""RequestQuery.month\\\"");\\n\\t\\t    if(string.IsNullOrWhiteSpace(value))\\n\\t\\t\\t    return DateTime.Now.Month;\\n\\t\\t\\telse\\n\\t\\t\\t    return value;\\n\\t\\t}\\n\\t\\tcatch(PropertyDoesNotExistException ex)\\n\\t\\t{\\n\\t\\t  return DateTime.Now.Month;\\n\\t\\t}\\n\\t\\tcatch(Exception ex)\\n\\t\\t{\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\"",\""default\"":\""\""}"",
  ""name"": ""month"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""MonthSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""0"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: account-monthly-timelog  id: bfd0dba8-dc50-4881-9815-7f5e56a6a2fb >>
			{
				var id = new Guid("bfd0dba8-dc50-4881-9815-7f5e56a6a2fb");
				Guid? parentId = new Guid("7eff5a2c-5d5d-4989-a68f-a1362b0dad7c");
				Guid? nodeId = null;
				var pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Account"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SampleCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry\\n\\t\\t{\\n\\t\\t    return pageModel.DataModel.GetProperty(\\\""RequestQuery.account\\\"");\\n\\t\\t}\\n\\t\\tcatch(Exception ex){\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\"",\""default\"":\""\""}"",
  ""name"": ""account"",
  ""try_connect_to_entity"": ""false"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AccountSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: d4c56ca4-52f8-47b8-8d62-e5a43930b377 >>
			{
				var id = new Guid("d4c56ca4-52f8-47b8-8d62-e5a43930b377");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("84b892fc-6ca4-4c7e-8b7c-2f2f6954862f");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 3,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: a7720205-8f62-4319-98c3-17c6e3a0462b >>
			{
				var id = new Guid("a7720205-8f62-4319-98c3-17c6e3a0462b");
				Guid? parentId = new Guid("d4c56ca4-52f8-47b8-8d62-e5a43930b377");
				Guid? nodeId = null;
				var pageId = new Guid("84b892fc-6ca4-4c7e-8b7c-2f2f6954862f");
				var componentName = "WebVella.Erp.Web.Components.PcHtmlBlock";
				var containerId = "column1";
				var options = @"{
  ""html"": ""{\""type\"":\""2\"",\""string\"":\""<div class=\\\""card app-card shadow-sm mb-3 shadow-hover\\\"">\\n\\t<div class=\\\""card-body p-0\\\"">\\n\\t    <div class=\\\""row no-gutters\\\"">\\n\\t\\t\\t<div class=\\\""col-lg-3\\\"">\\n\\t\\t\\t\\t<div class=\\\""app-image-wrapper pt-5 pb-5 pt-lg-0 pb-lg-0 go-bkg-blue-light\\\"">\\n\\t\\t\\t\\t\\t<span class=\\\""app-icon fa fa-database go-blue\\\""></span>\\n\\t\\t\\t\\t</div>\\n\\t\\t\\t</div>\\n\\t\\t\\t<div class=\\\""col-lg-9\\\"">\\n        \\t\\t<div class=\\\""app-meta p-3 p-lg-3\\\"">\\n        \\t\\t\\t<h3 class=\\\""label\\\"">Monthly timelog for an account</h3>\\n        \\t\\t\\t<div class=\\\""description mb-0\\\"">Lists all tasks that were worked on for the selected month and account, their billable and nonbillable total for the period</div>\\n        \\t\\t</div>\\n\\t\\t\\t</div>\\n\\t\\t</div>\\n\\n\\t</div>\\n    <a class=\\\""app-link\\\"" href=\\\""/projects/reports/list/a/account-monthly-timelog\\\""><em></em></a>\\n</div>\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: feed  id: 3f85bfe4-5040-42c6-a3fb-fefc9ab59b10 >>
			{
				var id = new Guid("3f85bfe4-5040-42c6-a3fb-fefc9ab59b10");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("dfe56667-174d-492d-8f84-b8ab8b70c63f");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcFeedList";
				var containerId = "";
				var options = @"{
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""FeedItemsForRecordId\"",\""default\"":\""\""}""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: 250115da-cea5-46f3-a77a-d2f7704c650d >>
			{
				var id = new Guid("250115da-cea5-46f3-a77a-d2f7704c650d");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""Entity.IconName\"",\""default\"":\""\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: e1b676c0-e128-46a2-b2cc-51a5b3ec2816 >>
			{
				var id = new Guid("e1b676c0-e128-46a2-b2cc-51a5b3ec2816");
				Guid? parentId = new Guid("250115da-cea5-46f3-a77a-d2f7704c650d");
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""Search"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-search"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer','open')"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc >>
			{
				var id = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 8,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""AllTasks\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""true"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No records"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""true"",
  ""container1_label"": """",
  ""container1_width"": ""40px"",
  ""container1_name"": """",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_class"": """",
  ""container1_vertical_align"": ""1"",
  ""container1_horizontal_align"": ""1"",
  ""container2_label"": ""type"",
  ""container2_width"": ""20px"",
  ""container2_name"": ""type"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_class"": """",
  ""container2_vertical_align"": ""1"",
  ""container2_horizontal_align"": ""1"",
  ""container3_label"": ""key"",
  ""container3_width"": ""120px"",
  ""container3_name"": ""key"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_class"": """",
  ""container3_vertical_align"": ""1"",
  ""container3_horizontal_align"": ""1"",
  ""container4_label"": ""task"",
  ""container4_width"": """",
  ""container4_name"": ""task"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_class"": """",
  ""container4_vertical_align"": ""1"",
  ""container4_horizontal_align"": ""1"",
  ""container5_label"": ""owner"",
  ""container5_width"": ""120px"",
  ""container5_name"": ""owner_id"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""true"",
  ""container5_class"": """",
  ""container5_vertical_align"": ""1"",
  ""container5_horizontal_align"": ""1"",
  ""container6_label"": ""created by"",
  ""container6_width"": ""120px"",
  ""container6_name"": ""created_by"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""true"",
  ""container6_class"": """",
  ""container6_vertical_align"": ""1"",
  ""container6_horizontal_align"": ""1"",
  ""container7_label"": ""target date"",
  ""container7_width"": ""120px"",
  ""container7_name"": ""target_date"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_class"": """",
  ""container7_vertical_align"": ""1"",
  ""container7_horizontal_align"": ""1"",
  ""container8_label"": ""status"",
  ""container8_width"": ""120px"",
  ""container8_name"": ""status"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_class"": """",
  ""container8_vertical_align"": ""1"",
  ""container8_horizontal_align"": ""1"",
  ""container9_label"": """",
  ""container9_width"": """",
  ""container9_name"": """",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_class"": """",
  ""container9_vertical_align"": ""1"",
  ""container9_horizontal_align"": ""1"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_class"": """",
  ""container10_vertical_align"": ""1"",
  ""container10_horizontal_align"": ""1"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_class"": """",
  ""container11_vertical_align"": ""1"",
  ""container11_horizontal_align"": ""1"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_class"": """",
  ""container12_vertical_align"": ""1"",
  ""container12_horizontal_align"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 15df96da-8d77-427f-a2a1-23017a6f8800 >>
			{
				var id = new Guid("15df96da-8d77-427f-a2a1-23017a6f8800");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column8";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$task_status_1n_task[0].label\"",\""default\"":\""n/a\""}"",
  ""name"": ""status_id"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 9bdd70b0-aa1d-4458-ad95-9fc455236350 >>
			{
				var id = new Guid("9bdd70b0-aa1d-4458-ad95-9fc455236350");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column3";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.key\"",\""default\"":\""key\""}"",
  ""name"": ""key"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 08d8dad6-594d-498f-aa0b-555d245ce9e2 >>
			{
				var id = new Guid("08d8dad6-594d-498f-aa0b-555d245ce9e2");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column5";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_task[0].username\"",\""default\"":\""n/a\""}"",
  ""name"": ""owner_id"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: e5756351-b9c2-4bd9-bcbc-be3cc9fb3751 >>
			{
				var id = new Guid("e5756351-b9c2-4bd9-bcbc-be3cc9fb3751");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar typeRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord.$task_type_1n_task[0]\\\"");\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (typeRecord == null)\\n\\t\\t\\treturn null;\\n\\n        var iconClass=\\\""fa fa-fw fa-file\\\"";\\n        var color=\\\""#999\\\"";\\n        if(typeRecord[\\\""icon_class\\\""] != null){\\n            iconClass = (string)typeRecord[\\\""icon_class\\\""];\\n        }\\n        if(typeRecord[\\\""color\\\""] != null){\\n            color = (string)typeRecord[\\\""color\\\""];\\n        }\\n\\t\\treturn $\\\""<i class=\\\\\\\""{iconClass}\\\\\\\"" style=\\\\\\\""color:{color};font-size:23px;\\\\\\\"" title=\\\\\\\""{typeRecord[\\\""label\\\""]}\\\\\\\""></i>\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""icon\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: ad9c357f-e620-4ed1-9593-d76c97019677 >>
			{
				var id = new Guid("ad9c357f-e620-4ed1-9593-d76c97019677");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column7";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.target_date\"",\""default\"":\""\""}"",
  ""name"": ""target_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 0660124a-cbf0-47ff-9757-3f072c39953a >>
			{
				var id = new Guid("0660124a-cbf0-47ff-9757-3f072c39953a");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column6";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_task_creator[0].username\"",\""default\"":\""n/a\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 5899b892-ee3d-4cb9-9811-a24ff8f1b791 >>
			{
				var id = new Guid("5899b892-ee3d-4cb9-9811-a24ff8f1b791");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column4";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.subject\"",\""default\"":\""Task subject\""}"",
  ""name"": ""subject"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: a918fec1-865b-4c54-8f93-685ffe85fb90 >>
			{
				var id = new Guid("a918fec1-865b-4c54-8f93-685ffe85fb90");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "column1";
				var options = @"{
  ""type"": ""2"",
  ""text"": """",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-eye"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//replace constants with your values\\n\\t\\tconst string DATASOURCE_NAME = \\\""RowRecord.id\\\"";\\n\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar dataSource = pageModel.TryGetDataSourceProperty<Guid>(DATASOURCE_NAME);\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (dataSource == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\treturn $\\\""/projects/tasks/tasks/r/{dataSource}/details?returnUrl=/projects/tasks/tasks/l/all\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: 73d24cb2-ae13-4ddd-9ea8-80d8ef6c2911 >>
			{
				var id = new Guid("73d24cb2-ae13-4ddd-9ea8-80d8ef6c2911");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcDrawer";
				var containerId = "";
				var options = @"{
  ""title"": ""Search Tasks"",
  ""width"": ""550px"",
  ""class"": """",
  ""body_class"": """",
  ""title_action_html"": """"
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: d1580be1-733d-477e-bd4a-65e325a8a263 >>
			{
				var id = new Guid("d1580be1-733d-477e-bd4a-65e325a8a263");
				Guid? parentId = new Guid("73d24cb2-ae13-4ddd-9ea8-80d8ef6c2911");
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-156877b1-d1ea-4fea-be4a-62a982bef3a7"",
  ""name"": ""form"",
  ""hook_key"": """",
  ""method"": ""get"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: 9888344d-c88f-4d1a-9984-7a718779e4cc >>
			{
				var id = new Guid("9888344d-c88f-4d1a-9984-7a718779e4cc");
				Guid? parentId = new Guid("d1580be1-733d-477e-bd4a-65e325a8a263");
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcGridFilterField";
				var containerId = "body";
				var options = @"{
  ""label"": ""Task contents"",
  ""name"": ""x_search"",
  ""try_connect_to_entity"": ""true"",
  ""field_type"": ""18"",
  ""query_type"": ""2"",
  ""query_options"": [
    ""2""
  ],
  ""prefix"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: c150a0fa-9c1a-4f05-a842-22d374e2c2e6 >>
			{
				var id = new Guid("c150a0fa-9c1a-4f05-a842-22d374e2c2e6");
				Guid? parentId = new Guid("d1580be1-733d-477e-bd4a-65e325a8a263");
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Search Tasks"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: feed  id: ff5b4808-9c2a-4d4f-8eaf-a4878594c55a >>
			{
				var id = new Guid("ff5b4808-9c2a-4d4f-8eaf-a4878594c55a");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("acb76466-32b8-428c-81cb-47b6013879e7");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n        try{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar queryRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RequestQuery\\\"");\\n        var type = \\\""\\\"";\\n        if(queryRecord.Properties.ContainsKey(\\\""type\\\""))\\n            type = (string)queryRecord[\\\""type\\\""];\\n\\n        var result = \\\""\\\"";\\n        result += $\\\""<ul class=\\\\\\\""nav nav-pills nav-sm mb-4\\\\\\\"">\\\"";\\n        result += $\\\""<li class=\\\\\\\""nav-item\\\\\\\"">\\\"";\\n        result += $\\\""<a class=\\\\\\\""nav-link {(type == \\\""\\\"" ? \\\""active\\\"" : \\\""\\\"")}\\\\\\\"" href=\\\\\\\""/projects/feed/feed/a/feed\\\\\\\"">All Feeds</a>\\\"";\\n\\t    result += $\\\""</li>\\\"";\\n\\t\\tresult += $\\\""<li class=\\\\\\\""nav-item\\\\\\\"">\\\"";\\n\\t\\tresult += $\\\""<a class=\\\\\\\""nav-link  {(type == \\\""task\\\"" ? \\\""active\\\"" : \\\""\\\"")}\\\\\\\"" href=\\\\\\\""/projects/feed/feed/a/feed?type=task\\\\\\\"">Task</a>\\\"";\\n\\t    result += $\\\""</li>\\\"";\\n\\t\\tresult += $\\\""<li class=\\\\\\\""nav-item\\\\\\\"">\\\"";\\n\\t\\tresult += $\\\""<a class=\\\\\\\""nav-link  {(type == \\\""comment\\\"" ? \\\""active\\\"" : \\\""\\\"")}\\\\\\\"" href=\\\\\\\""/projects/feed/feed/a/feed?type=comment\\\\\\\"">Comment</a>\\\"";\\n\\t    result += $\\\""</li>\\\"";\\n\\t\\tresult += $\\\""<li class=\\\\\\\""nav-item\\\\\\\"">\\\"";\\n\\t\\tresult += $\\\""<a class=\\\\\\\""nav-link  {(type == \\\""timelog\\\"" ? \\\""active\\\"" : \\\""\\\"")}\\\\\\\"" href=\\\\\\\""/projects/feed/feed/a/feed?type=timelog\\\\\\\"">Timelog</a>\\\"";\\n\\t    result += $\\\""</li>\\\"";\\t    \\n        result += $\\\""</ul>\\\"";\\t    \\n\\t\\treturn result;\\n        }\\n        catch(Exception ex){\\n            return ex.Message;\\n        }\\n\\t}\\n}\\n\"",\""default\"":\""Feed type Pill navigation\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 6de13934-ca81-4807-bb71-cadcdbb99ca7 >>
			{
				var id = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 3,
  ""class"": ""mt-4"",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_self_align"": ""1"",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_self_align"": ""1"",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_self_align"": ""1"",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_self_align"": ""1"",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_self_align"": ""1"",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_self_align"": ""1"",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_self_align"": ""1"",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_self_align"": ""1"",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_self_align"": ""1"",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_self_align"": ""1"",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_self_align"": ""1"",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 551483ab-262b-4541-b0dc-fadaa8de5284 >>
			{
				var id = new Guid("551483ab-262b-4541-b0dc-fadaa8de5284");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""End date"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.end_date\"",\""default\"":\""\""}"",
  ""name"": ""end_date"",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 7a7fbcd5-fb6f-40fd-a0cd-1a7c26e1c4ab >>
			{
				var id = new Guid("7a7fbcd5-fb6f-40fd-a0cd-1a7c26e1c4ab");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Project lead"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.owner_id\"",\""default\"":\""\""}"",
  ""name"": ""owner_id"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllUsersSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: b37c63a7-84ea-4673-9a81-ec4313c178b7 >>
			{
				var id = new Guid("b37c63a7-84ea-4673-9a81-ec4313c178b7");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Account"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.account_id\"",\""default\"":\""\""}"",
  ""name"": ""account_id"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllAccountsSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 30676929-f280-414d-8f4c-d41f851136ce >>
			{
				var id = new Guid("30676929-f280-414d-8f4c-d41f851136ce");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Budget type"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.budget_type\"",\""default\"":\""\""}"",
  ""name"": ""budget_type"",
  ""options"": """",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: b2caeb51-b6a5-4e15-a317-9825511792c6 >>
			{
				var id = new Guid("b2caeb51-b6a5-4e15-a317-9825511792c6");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Budget amount"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.budget_amount\"",\""default\"":\""\""}"",
  ""name"": ""budget_amount"",
  ""mode"": ""3"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 8a75b1d8-8184-40ed-a977-26616239fbb7 >>
			{
				var id = new Guid("8a75b1d8-8184-40ed-a977-26616239fbb7");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Start date"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.start_date\"",\""default\"":\""\""}"",
  ""name"": ""start_date"",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: 34916453-4d5a-40a7-b74c-3c4e8b5a8950 >>
			{
				var id = new Guid("34916453-4d5a-40a7-b74c-3c4e8b5a8950");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 8,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""NoOwnerTasks\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""true"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No records"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""true"",
  ""container1_label"": """",
  ""container1_width"": ""40px"",
  ""container1_name"": """",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_class"": """",
  ""container1_vertical_align"": ""1"",
  ""container1_horizontal_align"": ""1"",
  ""container2_label"": ""type"",
  ""container2_width"": ""20px"",
  ""container2_name"": ""type"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_class"": """",
  ""container2_vertical_align"": ""1"",
  ""container2_horizontal_align"": ""1"",
  ""container3_label"": ""key"",
  ""container3_width"": ""120px"",
  ""container3_name"": ""key"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_class"": """",
  ""container3_vertical_align"": ""1"",
  ""container3_horizontal_align"": ""1"",
  ""container4_label"": ""task"",
  ""container4_width"": """",
  ""container4_name"": ""task"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_class"": """",
  ""container4_vertical_align"": ""1"",
  ""container4_horizontal_align"": ""1"",
  ""container5_label"": ""owner"",
  ""container5_width"": ""120px"",
  ""container5_name"": ""owner_id"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_class"": """",
  ""container5_vertical_align"": ""1"",
  ""container5_horizontal_align"": ""1"",
  ""container6_label"": ""created by"",
  ""container6_width"": ""120px"",
  ""container6_name"": ""created_by"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_class"": """",
  ""container6_vertical_align"": ""1"",
  ""container6_horizontal_align"": ""1"",
  ""container7_label"": ""target date"",
  ""container7_width"": ""120px"",
  ""container7_name"": ""target_date"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_class"": """",
  ""container7_vertical_align"": ""1"",
  ""container7_horizontal_align"": ""1"",
  ""container8_label"": ""status"",
  ""container8_width"": ""80px"",
  ""container8_name"": ""status"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_class"": """",
  ""container8_vertical_align"": ""1"",
  ""container8_horizontal_align"": ""1"",
  ""container9_label"": """",
  ""container9_width"": """",
  ""container9_name"": """",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_class"": """",
  ""container9_vertical_align"": ""1"",
  ""container9_horizontal_align"": ""1"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_class"": """",
  ""container10_vertical_align"": ""1"",
  ""container10_horizontal_align"": ""1"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_class"": """",
  ""container11_vertical_align"": ""1"",
  ""container11_horizontal_align"": ""1"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_class"": """",
  ""container12_vertical_align"": ""1"",
  ""container12_horizontal_align"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: 91bbd374-13d5-4a86-8a07-84349ec57682 >>
			{
				var id = new Guid("91bbd374-13d5-4a86-8a07-84349ec57682");
				Guid? parentId = new Guid("34916453-4d5a-40a7-b74c-3c4e8b5a8950");
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column8";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$task_status_1n_task[0].label\"",\""default\"":\""n/a\""}"",
  ""name"": ""status_id"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: d0addd75-c216-4f25-9f61-44fb29f7f160 >>
			{
				var id = new Guid("d0addd75-c216-4f25-9f61-44fb29f7f160");
				Guid? parentId = new Guid("34916453-4d5a-40a7-b74c-3c4e8b5a8950");
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column6";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_task_creator[0].username\"",\""default\"":\""n/a\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: 001a3188-1d23-4f85-90f2-9053eac93bbc >>
			{
				var id = new Guid("001a3188-1d23-4f85-90f2-9053eac93bbc");
				Guid? parentId = new Guid("34916453-4d5a-40a7-b74c-3c4e8b5a8950");
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column4";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.subject\"",\""default\"":\""Task subject\""}"",
  ""name"": ""subject"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: fd73e317-ae0a-4c54-9bed-55f9c89965a9 >>
			{
				var id = new Guid("fd73e317-ae0a-4c54-9bed-55f9c89965a9");
				Guid? parentId = new Guid("34916453-4d5a-40a7-b74c-3c4e8b5a8950");
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "column1";
				var options = @"{
  ""type"": ""2"",
  ""text"": """",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-eye"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//replace constants with your values\\n\\t\\tconst string DATASOURCE_NAME = \\\""RowRecord.id\\\"";\\n\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar dataSource = pageModel.TryGetDataSourceProperty<Guid>(DATASOURCE_NAME);\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (dataSource == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\treturn $\\\""/projects/tasks/tasks/r/{dataSource}/details?returnUrl=/projects/tasks/tasks/l/all\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: 0ba5ed3f-625e-4df3-84ab-70f064b9905a >>
			{
				var id = new Guid("0ba5ed3f-625e-4df3-84ab-70f064b9905a");
				Guid? parentId = new Guid("34916453-4d5a-40a7-b74c-3c4e8b5a8950");
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column3";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.key\"",\""default\"":\""key\""}"",
  ""name"": ""key"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: 0867158f-f0c2-4284-838a-1c4ec3acb796 >>
			{
				var id = new Guid("0867158f-f0c2-4284-838a-1c4ec3acb796");
				Guid? parentId = new Guid("34916453-4d5a-40a7-b74c-3c4e8b5a8950");
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column5";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_task[0].username\"",\""default\"":\""n/a\""}"",
  ""name"": ""owner_id"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: b2b5e677-341a-43af-9baa-0aba98a7d8c3 >>
			{
				var id = new Guid("b2b5e677-341a-43af-9baa-0aba98a7d8c3");
				Guid? parentId = new Guid("34916453-4d5a-40a7-b74c-3c4e8b5a8950");
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar typeRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord.$task_type_1n_task[0]\\\"");\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (typeRecord == null)\\n\\t\\t\\treturn null;\\n\\n        var iconClass=\\\""fa fa-fw fa-file\\\"";\\n        var color=\\\""#999\\\"";\\n        if(typeRecord[\\\""icon_class\\\""] != null){\\n            iconClass = (string)typeRecord[\\\""icon_class\\\""];\\n        }\\n        if(typeRecord[\\\""color\\\""] != null){\\n            color = (string)typeRecord[\\\""color\\\""];\\n        }\\n\\t\\treturn $\\\""<i class=\\\\\\\""{iconClass}\\\\\\\"" style=\\\\\\\""color:{color};font-size:23px;\\\\\\\"" title=\\\\\\\""{typeRecord[\\\""label\\\""]}\\\\\\\""></i>\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""icon\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: no-owner  id: 8d244aa4-ad6b-464f-87e9-37a55fd18d19 >>
			{
				var id = new Guid("8d244aa4-ad6b-464f-87e9-37a55fd18d19");
				Guid? parentId = new Guid("34916453-4d5a-40a7-b74c-3c4e8b5a8950");
				Guid? nodeId = null;
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column7";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.target_date\"",\""default\"":\""\""}"",
  ""name"": ""target_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: 8012f7aa-e60b-4db9-a380-374c9238c12b >>
			{
				var id = new Guid("8012f7aa-e60b-4db9-a380-374c9238c12b");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcDrawer";
				var containerId = "";
				var options = @"{
  ""title"": ""Search Tasks"",
  ""width"": ""550px"",
  ""class"": """",
  ""body_class"": """",
  ""title_action_html"": """"
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: 9d6caedb-f43a-4ccb-a747-4c8917a6471e >>
			{
				var id = new Guid("9d6caedb-f43a-4ccb-a747-4c8917a6471e");
				Guid? parentId = new Guid("8012f7aa-e60b-4db9-a380-374c9238c12b");
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-156877b1-d1ea-4fea-be4a-62a982bef3a7"",
  ""name"": ""form"",
  ""hook_key"": """",
  ""method"": ""get"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: 9fa77e2f-c21d-48ef-82ea-825eaa697412 >>
			{
				var id = new Guid("9fa77e2f-c21d-48ef-82ea-825eaa697412");
				Guid? parentId = new Guid("9d6caedb-f43a-4ccb-a747-4c8917a6471e");
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcGridFilterField";
				var containerId = "body";
				var options = @"{
  ""label"": ""Task contents"",
  ""name"": ""x_search"",
  ""try_connect_to_entity"": ""true"",
  ""field_type"": ""18"",
  ""query_type"": ""2"",
  ""query_options"": [
    ""2""
  ],
  ""prefix"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: ecd7a737-fc1a-4766-9ea4-81ef60f099aa >>
			{
				var id = new Guid("ecd7a737-fc1a-4766-9ea4-81ef60f099aa");
				Guid? parentId = new Guid("9d6caedb-f43a-4ccb-a747-4c8917a6471e");
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Search Tasks"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: a4719fbd-b3d0-4f81-b302-96f5620e17cc >>
			{
				var id = new Guid("a4719fbd-b3d0-4f81-b302-96f5620e17cc");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 8,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""AllOpenTasks\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""true"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No records"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""true"",
  ""container1_label"": """",
  ""container1_width"": ""40px"",
  ""container1_name"": """",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_class"": """",
  ""container1_vertical_align"": ""1"",
  ""container1_horizontal_align"": ""1"",
  ""container2_label"": ""type"",
  ""container2_width"": ""20px"",
  ""container2_name"": ""type"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_class"": """",
  ""container2_vertical_align"": ""1"",
  ""container2_horizontal_align"": ""1"",
  ""container3_label"": ""key"",
  ""container3_width"": ""120px"",
  ""container3_name"": ""key"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_class"": """",
  ""container3_vertical_align"": ""1"",
  ""container3_horizontal_align"": ""1"",
  ""container4_label"": ""task"",
  ""container4_width"": """",
  ""container4_name"": ""task"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_class"": """",
  ""container4_vertical_align"": ""1"",
  ""container4_horizontal_align"": ""1"",
  ""container5_label"": ""owner"",
  ""container5_width"": ""120px"",
  ""container5_name"": ""owner_id"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_class"": """",
  ""container5_vertical_align"": ""1"",
  ""container5_horizontal_align"": ""1"",
  ""container6_label"": ""created by"",
  ""container6_width"": ""120px"",
  ""container6_name"": ""created_by"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_class"": """",
  ""container6_vertical_align"": ""1"",
  ""container6_horizontal_align"": ""1"",
  ""container7_label"": ""target date"",
  ""container7_width"": ""120px"",
  ""container7_name"": ""target_date"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_class"": """",
  ""container7_vertical_align"": ""1"",
  ""container7_horizontal_align"": ""1"",
  ""container8_label"": ""status"",
  ""container8_width"": ""80px"",
  ""container8_name"": ""status"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_class"": """",
  ""container8_vertical_align"": ""1"",
  ""container8_horizontal_align"": ""1"",
  ""container9_label"": """",
  ""container9_width"": """",
  ""container9_name"": """",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_class"": """",
  ""container9_vertical_align"": ""1"",
  ""container9_horizontal_align"": ""1"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_class"": """",
  ""container10_vertical_align"": ""1"",
  ""container10_horizontal_align"": ""1"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_class"": """",
  ""container11_vertical_align"": ""1"",
  ""container11_horizontal_align"": ""1"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_class"": """",
  ""container12_vertical_align"": ""1"",
  ""container12_horizontal_align"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: a38a8496-5f14-4c23-bd04-f036c4629824 >>
			{
				var id = new Guid("a38a8496-5f14-4c23-bd04-f036c4629824");
				Guid? parentId = new Guid("a4719fbd-b3d0-4f81-b302-96f5620e17cc");
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column6";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_task_creator[0].username\"",\""default\"":\""n/a\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: 2a4c131b-69b8-43bf-bf0f-7360cd953797 >>
			{
				var id = new Guid("2a4c131b-69b8-43bf-bf0f-7360cd953797");
				Guid? parentId = new Guid("a4719fbd-b3d0-4f81-b302-96f5620e17cc");
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column4";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.subject\"",\""default\"":\""Task subject\""}"",
  ""name"": ""subject"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: eb2e70db-8215-4097-a0f5-bc5154f21153 >>
			{
				var id = new Guid("eb2e70db-8215-4097-a0f5-bc5154f21153");
				Guid? parentId = new Guid("a4719fbd-b3d0-4f81-b302-96f5620e17cc");
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "column1";
				var options = @"{
  ""type"": ""2"",
  ""text"": """",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-eye"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//replace constants with your values\\n\\t\\tconst string DATASOURCE_NAME = \\\""RowRecord.id\\\"";\\n\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar dataSource = pageModel.TryGetDataSourceProperty<Guid>(DATASOURCE_NAME);\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (dataSource == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\treturn $\\\""/projects/tasks/tasks/r/{dataSource}/details?returnUrl=/projects/tasks/tasks/l/all\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: 35cb5466-3654-426d-97cd-caa83bb5ed3e >>
			{
				var id = new Guid("35cb5466-3654-426d-97cd-caa83bb5ed3e");
				Guid? parentId = new Guid("a4719fbd-b3d0-4f81-b302-96f5620e17cc");
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar typeRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord.$task_type_1n_task[0]\\\"");\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (typeRecord == null)\\n\\t\\t\\treturn null;\\n\\n        var iconClass=\\\""fa fa-fw fa-file\\\"";\\n        var color=\\\""#999\\\"";\\n        if(typeRecord[\\\""icon_class\\\""] != null){\\n            iconClass = (string)typeRecord[\\\""icon_class\\\""];\\n        }\\n        if(typeRecord[\\\""color\\\""] != null){\\n            color = (string)typeRecord[\\\""color\\\""];\\n        }\\n\\t\\treturn $\\\""<i class=\\\\\\\""{iconClass}\\\\\\\"" style=\\\\\\\""color:{color};font-size:23px;\\\\\\\"" title=\\\\\\\""{typeRecord[\\\""label\\\""]}\\\\\\\""></i>\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""icon\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: bd05d5ef-0ab4-48b0-a40e-5959875d071b >>
			{
				var id = new Guid("bd05d5ef-0ab4-48b0-a40e-5959875d071b");
				Guid? parentId = new Guid("a4719fbd-b3d0-4f81-b302-96f5620e17cc");
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column7";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.target_date\"",\""default\"":\""\""}"",
  ""name"": ""target_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: fdf94ef9-2130-4600-a3b1-53d1cb5489fc >>
			{
				var id = new Guid("fdf94ef9-2130-4600-a3b1-53d1cb5489fc");
				Guid? parentId = new Guid("a4719fbd-b3d0-4f81-b302-96f5620e17cc");
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column3";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.key\"",\""default\"":\""key\""}"",
  ""name"": ""key"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: 87e08d83-c8fb-4a41-a371-1316b6da0b17 >>
			{
				var id = new Guid("87e08d83-c8fb-4a41-a371-1316b6da0b17");
				Guid? parentId = new Guid("a4719fbd-b3d0-4f81-b302-96f5620e17cc");
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column5";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_task[0].username\"",\""default\"":\""n/a\""}"",
  ""name"": ""owner_id"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: open  id: d1b4df6b-5ce7-4831-8d59-efd2cfdd4d51 >>
			{
				var id = new Guid("d1b4df6b-5ce7-4831-8d59-efd2cfdd4d51");
				Guid? parentId = new Guid("a4719fbd-b3d0-4f81-b302-96f5620e17cc");
				Guid? nodeId = null;
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column8";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$task_status_1n_task[0].label\"",\""default\"":\""n/a\""}"",
  ""name"": ""status_id"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create data source*** Name: AllAccounts >>
			{
				var id = new Guid("61d21547-b353-48b8-8b75-b727680da79e");
				var name = @"AllAccounts";
				var description = @"Lists all accounts in the system";
				var eqlText = @"SELECT id,name 
FROM account
where name CONTAINS @name
ORDER BY @sortBy ASC
PAGE @page
PAGESIZE @pageSize";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_account.""id"" AS ""id"",
	 rec_account.""name"" AS ""name"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_account
WHERE  ( rec_account.""name""  ILIKE  @name ) 
ORDER BY rec_account.""name"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""name"",""type"":""text"",""value"":""null""},{""name"":""sortBy"",""type"":""text"",""value"":""name""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
				var weight = 10;
				var entityName = @"account";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: AllProjects >>
			{
				var id = new Guid("96218f33-42f1-4ff1-926c-b1765e1f8c6e");
				var name = @"AllProjects";
				var description = @"all project records";
				var eqlText = @"SELECT id,abbr,name,$user_1n_project_owner.username
FROM project
WHERE name CONTAINS @filterName
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_project.""id"" AS ""id"",
	 rec_project.""abbr"" AS ""abbr"",
	 rec_project.""name"" AS ""name"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_project_owner
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_project_owner.""id"" AS ""id"",
		 user_1n_project_owner.""username"" AS ""username"" 
	 FROM rec_user user_1n_project_owner
	 WHERE user_1n_project_owner.id = rec_project.owner_id ) d )::jsonb AS ""$user_1n_project_owner""	
	-------< $user_1n_project_owner
FROM rec_project
WHERE  ( rec_project.""name""  ILIKE  @filterName ) 
ORDER BY rec_project.""name"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""name""},{""name"":""sortOrder"",""type"":""text"",""value"":""asc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""filterName"",""type"":""text"",""value"":""null""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$user_1n_project_owner"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"project";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: AllUsers >>
			{
				var id = new Guid("f3e5ab66-9257-42f9-8bdf-f0233dd4aedd");
				var name = @"AllUsers";
				var description = @"All system users";
				var eqlText = @"SELECT *
FROM user
ORDER BY username asc
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_user.""id"" AS ""id"",
	 rec_user.""created_on"" AS ""created_on"",
	 rec_user.""first_name"" AS ""first_name"",
	 rec_user.""last_name"" AS ""last_name"",
	 rec_user.""username"" AS ""username"",
	 rec_user.""email"" AS ""email"",
	 rec_user.""password"" AS ""password"",
	 rec_user.""last_logged_in"" AS ""last_logged_in"",
	 rec_user.""enabled"" AS ""enabled"",
	 rec_user.""verified"" AS ""verified"",
	 rec_user.""preferences"" AS ""preferences"",
	 rec_user.""image"" AS ""image"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_user
ORDER BY rec_user.""username"" ASC
) X
";
				var parametersJson = @"[]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""first_name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""last_name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""email"",""type"":6,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""password"",""type"":13,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""last_logged_in"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""enabled"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""verified"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""preferences"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""image"",""type"":9,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
				var weight = 10;
				var entityName = @"user";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: ProjectOpenTasks >>
			{
				var id = new Guid("46aab266-e2a8-4b67-9155-39ec1cf3bccb");
				var name = @"ProjectOpenTasks";
				var description = @"All open tasks for a project";
				var eqlText = @"SELECT *,$milestone_nn_task.name,$task_status_1n_task.label,$task_type_1n_task.label
FROM task
WHERE $project_nn_task.id = @projectId
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""start_date"" AS ""start_date"",
	 rec_task.""target_date"" AS ""target_date"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""x_nonbillable_hours"" AS ""x_nonbillable_hours"",
	 rec_task.""x_billable_hours"" AS ""x_billable_hours"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""priority"" AS ""priority"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $milestone_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 milestone_nn_task.""id"" AS ""id"",
		 milestone_nn_task.""name"" AS ""name""
	 FROM rec_milestone milestone_nn_task
	 LEFT JOIN  rel_milestone_nn_task milestone_nn_task_target ON milestone_nn_task_target.target_id = rec_task.id
	 WHERE milestone_nn_task.id = milestone_nn_task_target.origin_id )d  )::jsonb AS ""$milestone_nn_task"",
	
	-------< $milestone_nn_task	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	
	-------< $task_status_1n_task	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task""	
	-------< $task_type_1n_task
FROM rec_task
LEFT OUTER JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
LEFT OUTER JOIN  rec_project project_nn_task_tar_org ON project_nn_task_target.origin_id = project_nn_task_tar_org.id
WHERE  ( project_nn_task_tar_org.""id"" = @projectId ) 
ORDER BY rec_task.""id"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""projectId"",""type"":""guid"",""value"":""00000000-0000-0000-0000-000000000000""},{""name"":""sortBy"",""type"":""text"",""value"":""id""},{""name"":""sortOrder"",""type"":""text"",""value"":""asc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_date"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""target_date"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_hours"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_hours"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$milestone_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: ProjectAuxData >>
			{
				var id = new Guid("3c5a9d64-47ea-466a-8b0e-49e61df58bd1");
				var name = @"ProjectAuxData";
				var description = @"getting related data for the current project";
				var eqlText = @"SELECT $user_1n_project_owner.id
FROM project
WHERE id = @recordId
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_project_owner
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_project_owner.""id"" AS ""id"" 
	 FROM rec_user user_1n_project_owner
	 WHERE user_1n_project_owner.id = rec_project.owner_id ) d )::jsonb AS ""$user_1n_project_owner""	
	-------< $user_1n_project_owner
FROM rec_project
WHERE  ( rec_project.""id"" = @recordId ) 
) X
";
				var parametersJson = @"[{""name"":""recordId"",""type"":""guid"",""value"":""00000000-0000-0000-0000-000000000000""}]";
				var fieldsJson = @"[{""name"":""$user_1n_project_owner"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"project";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: TaskStatuses >>
			{
				var id = new Guid("fad53f3d-4d3b-4c7b-8cd2-23e96a086ad8");
				var name = @"TaskStatuses";
				var description = @"All task statuses";
				var eqlText = @"SELECT *
FROM task_status
ORDER BY label asc";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task_status.""id"" AS ""id"",
	 rec_task_status.""is_closed"" AS ""is_closed"",
	 rec_task_status.""is_default"" AS ""is_default"",
	 rec_task_status.""l_scope"" AS ""l_scope"",
	 rec_task_status.""label"" AS ""label"",
	 rec_task_status.""sort_index"" AS ""sort_index"",
	 rec_task_status.""is_system"" AS ""is_system"",
	 rec_task_status.""is_enabled"" AS ""is_enabled"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_task_status
ORDER BY rec_task_status.""label"" ASC
) X
";
				var parametersJson = @"[]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_closed"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_default"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""sort_index"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_system"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_enabled"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
				var weight = 10;
				var entityName = @"task_status";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: ProjectWidgetMyTasksDueToday >>
			{
				var id = new Guid("eae07b63-9bf4-4e25-80af-df5228dedf35");
				var name = @"ProjectWidgetMyTasksDueToday";
				var description = @"My tasks due today";
				var eqlText = @"SELECT *,$project_nn_task.name
FROM task
WHERE owner_id = @userId AND end_time > @currentDateStart AND end_time < @currentDateEnd AND status_id <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf'
ORDER BY priority DESC
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""x_search"" AS ""x_search"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""start_time"" AS ""start_time"",
	 rec_task.""end_time"" AS ""end_time"",
	 rec_task.""recurrence_id"" AS ""recurrence_id"",
	 rec_task.""reserve_time"" AS ""reserve_time"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""name"" AS ""name""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task""	
	-------< $project_nn_task

FROM rec_task
WHERE  (  (  (  ( rec_task.""owner_id"" = @userId )  AND  ( rec_task.""end_time"" > @currentDateStart )  )  AND  ( rec_task.""end_time"" < @currentDateEnd )  )  AND  ( rec_task.""status_id"" <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf' )  ) 
ORDER BY rec_task.""priority"" DESC
) X
";
				var parametersJson = @"[{""name"":""userId"",""type"":""guid"",""value"":""guid.empty""},{""name"":""currentDateStart"",""type"":""date"",""value"":""now""},{""name"":""currentDateEnd"",""type"":""date"",""value"":""now""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""end_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reserve_time"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: TaskTypes >>
			{
				var id = new Guid("4857ace4-fcfc-4803-ad86-7c7afba91ce0");
				var name = @"TaskTypes";
				var description = @"All task types";
				var eqlText = @"SELECT *
FROM task_type
WHERE l_scope CONTAINS @scope
ORDER BY sort_index asc";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task_type.""id"" AS ""id"",
	 rec_task_type.""is_default"" AS ""is_default"",
	 rec_task_type.""l_scope"" AS ""l_scope"",
	 rec_task_type.""label"" AS ""label"",
	 rec_task_type.""sort_index"" AS ""sort_index"",
	 rec_task_type.""is_system"" AS ""is_system"",
	 rec_task_type.""is_enabled"" AS ""is_enabled"",
	 rec_task_type.""icon_class"" AS ""icon_class"",
	 rec_task_type.""color"" AS ""color"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_task_type
WHERE  ( rec_task_type.""l_scope""  ILIKE  @scope ) 
ORDER BY rec_task_type.""sort_index"" ASC
) X
";
				var parametersJson = @"[{""name"":""scope"",""type"":""text"",""value"":""projects""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_default"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""sort_index"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_system"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_enabled"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
				var weight = 10;
				var entityName = @"task_type";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: AllTasks >>
			{
				var id = new Guid("5a6e9d56-63bc-43b1-b95e-24838db9f435");
				var name = @"AllTasks";
				var description = @"All tasks selection";
				var eqlText = @"SELECT *,$project_nn_task.abbr,$user_1n_task.username,$task_status_1n_task.label,$task_type_1n_task.label,$task_type_1n_task.icon_class,$task_type_1n_task.color,$user_1n_task_creator.username
FROM task
WHERE x_search CONTAINS @searchQuery
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""x_search"" AS ""x_search"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""start_time"" AS ""start_time"",
	 rec_task.""end_time"" AS ""end_time"",
	 rec_task.""recurrence_id"" AS ""recurrence_id"",
	 rec_task.""reserve_time"" AS ""reserve_time"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""abbr"" AS ""abbr""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task"",
	-------< $project_nn_task
	------->: $user_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task.""id"" AS ""id"",
		 user_1n_task.""username"" AS ""username"" 
	 FROM rec_user user_1n_task
	 WHERE user_1n_task.id = rec_task.owner_id ) d )::jsonb AS ""$user_1n_task"",
	-------< $user_1n_task
	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	-------< $task_status_1n_task
	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"",
		 task_type_1n_task.""icon_class"" AS ""icon_class"",
		 task_type_1n_task.""color"" AS ""color"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task"",
	-------< $task_type_1n_task
	------->: $user_1n_task_creator
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task_creator.""id"" AS ""id"",
		 user_1n_task_creator.""username"" AS ""username"" 
	 FROM rec_user user_1n_task_creator
	 WHERE user_1n_task_creator.id = rec_task.created_by ) d )::jsonb AS ""$user_1n_task_creator""	
	-------< $user_1n_task_creator

FROM rec_task
WHERE  ( rec_task.""x_search""  ILIKE  @searchQuery ) 
ORDER BY rec_task.""end_time"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""end_time""},{""name"":""sortOrder"",""type"":""text"",""value"":""asc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""end_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reserve_time"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task_creator"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: TaskComments >>
			{
				var id = new Guid("f68fa8be-b957-4692-b459-4da62d23f472");
				var name = @"TaskComments";
				var description = @"All comments for a certain task";
				var eqlText = @"SELECT *,$task_nn_comment.id,$task_nn_comment.$project_nn_task.id,$case_nn_comment.id FROM comment
WHERE id = @commentId";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_comment.""id"" AS ""id"",
	 rec_comment.""body"" AS ""body"",
	 rec_comment.""created_by"" AS ""created_by"",
	 rec_comment.""created_on"" AS ""created_on"",
	 rec_comment.""l_scope"" AS ""l_scope"",
	 rec_comment.""parent_id"" AS ""parent_id"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $task_nn_comment
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 task_nn_comment.""id"" AS ""id"",
		------->: $project_nn_task
		(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
		 SELECT 
			 project_nn_task.""id"" AS ""id""
		 FROM rec_project project_nn_task
		 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = task_nn_comment.id
		 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task""		
		-------< $project_nn_task

	 FROM rec_task task_nn_comment
	 LEFT JOIN  rel_task_nn_comment task_nn_comment_target ON task_nn_comment_target.target_id = rec_comment.id
	 WHERE task_nn_comment.id = task_nn_comment_target.origin_id )d  )::jsonb AS ""$task_nn_comment"",
	-------< $task_nn_comment
	------->: $case_nn_comment
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 case_nn_comment.""id"" AS ""id""
	 FROM rec_case case_nn_comment
	 LEFT JOIN  rel_case_nn_comment case_nn_comment_target ON case_nn_comment_target.target_id = rec_comment.id
	 WHERE case_nn_comment.id = case_nn_comment_target.origin_id )d  )::jsonb AS ""$case_nn_comment""	
	-------< $case_nn_comment

FROM rec_comment
WHERE  ( rec_comment.""id"" = @commentId ) 
) X
";
				var parametersJson = @"[{""name"":""commentId"",""type"":""guid"",""value"":""d5e1d939-fa3e-4332-a521-4c4e0f051e8a""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$task_nn_comment"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]},{""name"":""$case_nn_comment"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"comment";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: CommentsForRecordId >>
			{
				var id = new Guid("a588e096-358d-4426-adf6-5db693f32322");
				var name = @"CommentsForRecordId";
				var description = @"Get all comments for a record";
				var eqlText = @"SELECT *,$user_1n_comment.image,$user_1n_comment.username
FROM comment
WHERE l_related_records CONTAINS @recordId 
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_comment.""id"" AS ""id"",
	 rec_comment.""body"" AS ""body"",
	 rec_comment.""created_by"" AS ""created_by"",
	 rec_comment.""created_on"" AS ""created_on"",
	 rec_comment.""l_scope"" AS ""l_scope"",
	 rec_comment.""parent_id"" AS ""parent_id"",
	 rec_comment.""l_related_records"" AS ""l_related_records"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_comment
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_comment.""id"" AS ""id"",
		 user_1n_comment.""image"" AS ""image"",
		 user_1n_comment.""username"" AS ""username"" 
	 FROM rec_user user_1n_comment
	 WHERE user_1n_comment.id = rec_comment.created_by ) d )::jsonb AS ""$user_1n_comment""	
	-------< $user_1n_comment

FROM rec_comment
WHERE  ( rec_comment.""l_related_records""  ILIKE  @recordId ) 
ORDER BY rec_comment.""created_on"" DESC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""created_on""},{""name"":""sortOrder"",""type"":""text"",""value"":""desc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""recordId"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_related_records"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$user_1n_comment"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""image"",""type"":9,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"comment";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: AllProjectTasks >>
			{
				var id = new Guid("c2284f3d-2ddc-4bad-9d1b-f6e44d502bdd");
				var name = @"AllProjectTasks";
				var description = @"All tasks in a project";
				var eqlText = @"SELECT *,$project_nn_task.abbr,$user_1n_task.username,$task_status_1n_task.label,$task_type_1n_task.label,$task_type_1n_task.icon_class,$task_type_1n_task.color,$user_1n_task_creator.username
FROM task
WHERE x_search CONTAINS @searchQuery AND $project_nn_task.id = @projectId
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""x_search"" AS ""x_search"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""start_time"" AS ""start_time"",
	 rec_task.""end_time"" AS ""end_time"",
	 rec_task.""recurrence_id"" AS ""recurrence_id"",
	 rec_task.""reserve_time"" AS ""reserve_time"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""abbr"" AS ""abbr""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task"",
	-------< $project_nn_task
	------->: $user_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task.""id"" AS ""id"",
		 user_1n_task.""username"" AS ""username"" 
	 FROM rec_user user_1n_task
	 WHERE user_1n_task.id = rec_task.owner_id ) d )::jsonb AS ""$user_1n_task"",
	-------< $user_1n_task
	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	-------< $task_status_1n_task
	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"",
		 task_type_1n_task.""icon_class"" AS ""icon_class"",
		 task_type_1n_task.""color"" AS ""color"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task"",
	-------< $task_type_1n_task
	------->: $user_1n_task_creator
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task_creator.""id"" AS ""id"",
		 user_1n_task_creator.""username"" AS ""username"" 
	 FROM rec_user user_1n_task_creator
	 WHERE user_1n_task_creator.id = rec_task.created_by ) d )::jsonb AS ""$user_1n_task_creator""	
	-------< $user_1n_task_creator

FROM rec_task
LEFT OUTER JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
LEFT OUTER JOIN  rec_project project_nn_task_tar_org ON project_nn_task_target.origin_id = project_nn_task_tar_org.id
WHERE  (  ( rec_task.""x_search""  ILIKE  @searchQuery )  AND  ( project_nn_task_tar_org.""id"" = @projectId )  ) 
ORDER BY rec_task.""end_time"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""end_time""},{""name"":""sortOrder"",""type"":""text"",""value"":""asc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty""},{""name"":""projectId"",""type"":""guid"",""value"":""guid.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""end_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reserve_time"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task_creator"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: TimeLogsForRecordId >>
			{
				var id = new Guid("e66b8374-82ea-4305-8456-085b3a1f1f2d");
				var name = @"TimeLogsForRecordId";
				var description = @"Get all time logs for a record";
				var eqlText = @"SELECT *,$user_1n_timelog.image,$user_1n_timelog.username
FROM timelog
WHERE l_related_records CONTAINS @recordId 
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_timelog.""id"" AS ""id"",
	 rec_timelog.""body"" AS ""body"",
	 rec_timelog.""created_by"" AS ""created_by"",
	 rec_timelog.""created_on"" AS ""created_on"",
	 rec_timelog.""is_billable"" AS ""is_billable"",
	 rec_timelog.""l_related_records"" AS ""l_related_records"",
	 rec_timelog.""l_scope"" AS ""l_scope"",
	 rec_timelog.""minutes"" AS ""minutes"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_timelog
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_timelog.""id"" AS ""id"",
		 user_1n_timelog.""image"" AS ""image"",
		 user_1n_timelog.""username"" AS ""username"" 
	 FROM rec_user user_1n_timelog
	 WHERE user_1n_timelog.id = rec_timelog.created_by ) d )::jsonb AS ""$user_1n_timelog""	
	-------< $user_1n_timelog

FROM rec_timelog
WHERE  ( rec_timelog.""l_related_records""  ILIKE  @recordId ) 
ORDER BY rec_timelog.""created_on"" DESC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""created_on""},{""name"":""sortOrder"",""type"":""text"",""value"":""desc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""recordId"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":10,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_billable"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_related_records"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$user_1n_timelog"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""image"",""type"":9,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"timelog";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: FeedItemsForRecordId >>
			{
				var id = new Guid("74e5a414-6deb-4af6-8e29-567f718ca430");
				var name = @"FeedItemsForRecordId";
				var description = @"Get all feed items for a record";
				var eqlText = @"SELECT *,$user_1n_feed_item.image,$user_1n_feed_item.username
FROM feed_item
WHERE l_related_records CONTAINS @recordId AND type CONTAINS @type
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_feed_item.""id"" AS ""id"",
	 rec_feed_item.""created_by"" AS ""created_by"",
	 rec_feed_item.""created_on"" AS ""created_on"",
	 rec_feed_item.""l_scope"" AS ""l_scope"",
	 rec_feed_item.""subject"" AS ""subject"",
	 rec_feed_item.""body"" AS ""body"",
	 rec_feed_item.""type"" AS ""type"",
	 rec_feed_item.""l_related_records"" AS ""l_related_records"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_feed_item
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_feed_item.""id"" AS ""id"",
		 user_1n_feed_item.""image"" AS ""image"",
		 user_1n_feed_item.""username"" AS ""username"" 
	 FROM rec_user user_1n_feed_item
	 WHERE user_1n_feed_item.id = rec_feed_item.created_by ) d )::jsonb AS ""$user_1n_feed_item""	
	-------< $user_1n_feed_item

FROM rec_feed_item
WHERE  (  ( rec_feed_item.""l_related_records""  ILIKE  @recordId )  AND  ( rec_feed_item.""type""  ILIKE  @type )  ) 
ORDER BY rec_feed_item.""created_on"" DESC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""created_on""},{""name"":""sortOrder"",""type"":""text"",""value"":""desc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""recordId"",""type"":""text"",""value"":""string.empty""},{""name"":""type"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_related_records"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$user_1n_feed_item"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""image"",""type"":9,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"feed_item";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: NoOwnerTasks >>
			{
				var id = new Guid("40c0bcc6-2e3e-4b68-ae6a-27f1f472f069");
				var name = @"NoOwnerTasks";
				var description = @"all tasks without an owner";
				var eqlText = @"SELECT *,$project_nn_task.abbr,$user_1n_task.username,$task_status_1n_task.label,$task_type_1n_task.label,$task_type_1n_task.icon_class,$task_type_1n_task.color,$user_1n_task_creator.username
FROM task
WHERE owner_id = NULL AND x_search CONTAINS @searchQuery AND status_id <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf'
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""x_search"" AS ""x_search"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""start_time"" AS ""start_time"",
	 rec_task.""end_time"" AS ""end_time"",
	 rec_task.""recurrence_id"" AS ""recurrence_id"",
	 rec_task.""reserve_time"" AS ""reserve_time"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""abbr"" AS ""abbr""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task"",
	-------< $project_nn_task
	------->: $user_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task.""id"" AS ""id"",
		 user_1n_task.""username"" AS ""username"" 
	 FROM rec_user user_1n_task
	 WHERE user_1n_task.id = rec_task.owner_id ) d )::jsonb AS ""$user_1n_task"",
	-------< $user_1n_task
	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	-------< $task_status_1n_task
	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"",
		 task_type_1n_task.""icon_class"" AS ""icon_class"",
		 task_type_1n_task.""color"" AS ""color"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task"",
	-------< $task_type_1n_task
	------->: $user_1n_task_creator
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task_creator.""id"" AS ""id"",
		 user_1n_task_creator.""username"" AS ""username"" 
	 FROM rec_user user_1n_task_creator
	 WHERE user_1n_task_creator.id = rec_task.created_by ) d )::jsonb AS ""$user_1n_task_creator""	
	-------< $user_1n_task_creator

FROM rec_task
WHERE  (  (  ( rec_task.""owner_id"" IS NULL )  AND  ( rec_task.""x_search""  ILIKE  @searchQuery )  )  AND  ( rec_task.""status_id"" <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf' )  ) 
ORDER BY rec_task.""end_time"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""end_time""},{""name"":""sortOrder"",""type"":""text"",""value"":""asc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""end_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reserve_time"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task_creator"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: TaskAuxData >>
			{
				var id = new Guid("587d963b-613f-4e77-a7d4-719f631ce6b2");
				var name = @"TaskAuxData";
				var description = @"getting related data for the current task";
				var eqlText = @"SELECT $project_nn_task.id,$project_nn_task.abbr,$project_nn_task.name,$user_nn_task_watchers.id
FROM task
WHERE id = @recordId";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""abbr"" AS ""abbr"",
		 project_nn_task.""name"" AS ""name""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task"",
	-------< $project_nn_task
	------->: $user_nn_task_watchers
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 user_nn_task_watchers.""id"" AS ""id""
	 FROM rec_user user_nn_task_watchers
	 LEFT JOIN  rel_user_nn_task_watchers user_nn_task_watchers_target ON user_nn_task_watchers_target.target_id = rec_task.id
	 WHERE user_nn_task_watchers.id = user_nn_task_watchers_target.origin_id )d  )::jsonb AS ""$user_nn_task_watchers""	
	-------< $user_nn_task_watchers

FROM rec_task
WHERE  ( rec_task.""id"" = @recordId ) 
) X
";
				var parametersJson = @"[{""name"":""recordId"",""type"":""guid"",""value"":""00000000-0000-0000-0000-000000000000""}]";
				var fieldsJson = @"[{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_nn_task_watchers"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: AllOpenTasks >>
			{
				var id = new Guid("9c2337ac-b505-4ce4-b1ff-ffde2e37b312");
				var name = @"AllOpenTasks";
				var description = @"All open tasks selection";
				var eqlText = @"SELECT *,$project_nn_task.abbr,$user_1n_task.username,$task_status_1n_task.label,$task_type_1n_task.label,$task_type_1n_task.icon_class,$task_type_1n_task.color,$user_1n_task_creator.username
FROM task
WHERE status_id <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf' AND x_search CONTAINS @searchQuery
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""x_search"" AS ""x_search"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""start_time"" AS ""start_time"",
	 rec_task.""end_time"" AS ""end_time"",
	 rec_task.""recurrence_id"" AS ""recurrence_id"",
	 rec_task.""reserve_time"" AS ""reserve_time"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""abbr"" AS ""abbr""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task"",
	-------< $project_nn_task
	------->: $user_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task.""id"" AS ""id"",
		 user_1n_task.""username"" AS ""username"" 
	 FROM rec_user user_1n_task
	 WHERE user_1n_task.id = rec_task.owner_id ) d )::jsonb AS ""$user_1n_task"",
	-------< $user_1n_task
	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	-------< $task_status_1n_task
	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"",
		 task_type_1n_task.""icon_class"" AS ""icon_class"",
		 task_type_1n_task.""color"" AS ""color"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task"",
	-------< $task_type_1n_task
	------->: $user_1n_task_creator
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task_creator.""id"" AS ""id"",
		 user_1n_task_creator.""username"" AS ""username"" 
	 FROM rec_user user_1n_task_creator
	 WHERE user_1n_task_creator.id = rec_task.created_by ) d )::jsonb AS ""$user_1n_task_creator""	
	-------< $user_1n_task_creator

FROM rec_task
WHERE  (  ( rec_task.""status_id"" <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf' )  AND  ( rec_task.""x_search""  ILIKE  @searchQuery )  ) 
ORDER BY rec_task.""end_time"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""end_time""},{""name"":""sortOrder"",""type"":""text"",""value"":""asc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""end_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reserve_time"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task_creator"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: ProjectWidgetMyTasks >>
			{
				var id = new Guid("c44eab77-c81e-4f55-95c8-4949b275fc99");
				var name = @"ProjectWidgetMyTasks";
				var description = @"top 5 upcoming tasks";
				var eqlText = @"SELECT *,$project_nn_task.name
FROM task
WHERE owner_id = @userId AND (end_time > @currentDate OR end_time = null) AND status_id <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf'
ORDER BY end_time ASC, priority DESC
PAGE 1
PAGESIZE 5
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""x_search"" AS ""x_search"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""start_time"" AS ""start_time"",
	 rec_task.""end_time"" AS ""end_time"",
	 rec_task.""recurrence_id"" AS ""recurrence_id"",
	 rec_task.""reserve_time"" AS ""reserve_time"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""name"" AS ""name""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task""	
	-------< $project_nn_task

FROM rec_task
WHERE  (  (  ( rec_task.""owner_id"" = @userId )  AND  (  ( rec_task.""end_time"" > @currentDate )  OR  ( rec_task.""end_time"" IS NULL )  )  )  AND  ( rec_task.""status_id"" <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf' )  ) 
ORDER BY rec_task.""end_time"" ASC , rec_task.""priority"" DESC
LIMIT 5
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""userId"",""type"":""guid"",""value"":""guid.empty""},{""name"":""currentDate"",""type"":""date"",""value"":""now""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""end_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reserve_time"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: ProjectWidgetMyOverdueTasks >>
			{
				var id = new Guid("946919a6-e4cd-41a2-97dc-1069d73adcd1");
				var name = @"ProjectWidgetMyOverdueTasks";
				var description = @"all my overdue tasks";
				var eqlText = @"SELECT *,$project_nn_task.name
FROM task
WHERE owner_id = @userId AND end_time < @currentDate AND status_id <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf'
ORDER BY end_time ASC, priority DESC ";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""x_search"" AS ""x_search"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""start_time"" AS ""start_time"",
	 rec_task.""end_time"" AS ""end_time"",
	 rec_task.""recurrence_id"" AS ""recurrence_id"",
	 rec_task.""reserve_time"" AS ""reserve_time"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""name"" AS ""name""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task""	
	-------< $project_nn_task

FROM rec_task
WHERE  (  (  ( rec_task.""owner_id"" = @userId )  AND  ( rec_task.""end_time"" < @currentDate )  )  AND  ( rec_task.""status_id"" <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf' )  ) 
ORDER BY rec_task.""end_time"" ASC , rec_task.""priority"" DESC
) X
";
				var parametersJson = @"[{""name"":""userId"",""type"":""guid"",""value"":""guid.empty""},{""name"":""currentDate"",""type"":""date"",""value"":""now""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""end_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reserve_time"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create page data source*** Name: Accounts >>
			{
				var id = new Guid("a2db7724-f05b-4820-9269-64792398c309");
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var dataSourceId = new Guid("61d21547-b353-48b8-8b75-b727680da79e");
				var name = @"Accounts";
				var parameters = @"[{""name"":""name"",""type"":""text"",""value"":""{{RequestQuery.q_name_v}}""},{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy ?? name}}""},{""name"":""page"",""type"":""int"",""value"":""{{RequestQuery.page ?? 1 }}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskTypes >>
			{
				var id = new Guid("d13ee96e-64e6-4174-b16d-c1c5a7bcb9f9");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("4857ace4-fcfc-4803-ad86-7c7afba91ce0");
				var name = @"TaskTypes";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllProjects >>
			{
				var id = new Guid("993d643a-1c10-4475-8b1f-3e5ac5f2e036");
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var dataSourceId = new Guid("96218f33-42f1-4ff1-926c-b1765e1f8c6e");
				var name = @"AllProjects";
				var parameters = @"[{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy ?? name}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsers >>
			{
				var id = new Guid("a94b7669-edd2-484e-88fb-d480f79b4ec6");
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var dataSourceId = new Guid("f3e5ab66-9257-42f9-8bdf-f0233dd4aedd");
				var name = @"AllUsers";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllAccounts >>
			{
				var id = new Guid("6e38b5c3-43ba-4d5e-8454-11e7f6eef235");
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var dataSourceId = new Guid("61d21547-b353-48b8-8b75-b727680da79e");
				var name = @"AllAccounts";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsersSelectOptions >>
			{
				var id = new Guid("1487e7c6-60b2-4c2c-9ebe-0648435d2330");
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllUsersSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllUsers""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""username""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllAccountsSelectOptions >>
			{
				var id = new Guid("7d05f40e-71ae-49de-9dd0-2231b1c9265a");
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllAccountsSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllAccounts""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""name""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllAccountsSelectOptions >>
			{
				var id = new Guid("8b29596b-3310-46e0-838b-682e243f4611");
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllAccountsSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllAccounts""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""name""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsers >>
			{
				var id = new Guid("fefbdab5-57ee-4343-9355-199c154bde3d");
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var dataSourceId = new Guid("f3e5ab66-9257-42f9-8bdf-f0233dd4aedd");
				var name = @"AllUsers";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsersSelectOptions >>
			{
				var id = new Guid("f92520fe-8ea9-4284-a991-bb74810660e5");
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllUsersSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllUsers""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""username""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsers >>
			{
				var id = new Guid("defaf774-60d6-4c15-9683-da15ca53730c");
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var dataSourceId = new Guid("f3e5ab66-9257-42f9-8bdf-f0233dd4aedd");
				var name = @"AllUsers";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsersSelectOption >>
			{
				var id = new Guid("ebf5c697-3a01-4759-b9c6-ec7f3414bb54");
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllUsersSelectOption";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllUsers""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""username""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllProjects >>
			{
				var id = new Guid("c4bb6351-2fa9-4953-852f-62eb782e839c");
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var dataSourceId = new Guid("96218f33-42f1-4ff1-926c-b1765e1f8c6e");
				var name = @"AllProjects";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllProjectsSelectOption >>
			{
				var id = new Guid("561c85b5-b016-4420-8770-9752ff5347b9");
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllProjectsSelectOption";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllProjects""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""name""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: MonthSelectOptions >>
			{
				var id = new Guid("e58cc762-4e3f-4b6f-9968-f3b6ed907a86");
				var pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var dataSourceId = new Guid("bd83b38b-0211-4aab-9049-97e9e2847c57");
				var name = @"MonthSelectOptions";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllProjectTasks >>
			{
				var id = new Guid("6c41cbd7-d99f-4019-84f0-24361bfd7a0a");
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var dataSourceId = new Guid("c2284f3d-2ddc-4bad-9d1b-f6e44d502bdd");
				var name = @"AllProjectTasks";
				var parameters = @"[{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy}}""},{""name"":""sortOrder"",""type"":""text"",""value"":""{{RequestQuery.sortOrder}}""},{""name"":""page"",""type"":""int"",""value"":""{{RequestQuery.page}}""},{""name"":""searchQuery"",""type"":""text"",""value"":""{{RequestQuery.q_x_search_v}}""},{""name"":""projectId"",""type"":""guid"",""value"":""{{ParentRecord.id}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskAuxData >>
			{
				var id = new Guid("f8c429ee-c6fe-457d-9339-44e626a6dd27");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("587d963b-613f-4e77-a7d4-719f631ce6b2");
				var name = @"TaskAuxData";
				var parameters = @"[{""name"":""recordId"",""type"":""guid"",""value"":""{{Record.id}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsers >>
			{
				var id = new Guid("5ff5cc0c-c06e-4b58-8a31-4714914778aa");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("f3e5ab66-9257-42f9-8bdf-f0233dd4aedd");
				var name = @"AllUsers";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsersSelectOption >>
			{
				var id = new Guid("43691d9f-65ef-433c-934b-ccf6eaafdd3f");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllUsersSelectOption";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllUsers""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""username""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskTypesSelectOption >>
			{
				var id = new Guid("750213cb-8c69-4749-b10f-211b53369958");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"TaskTypesSelectOption";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""TaskTypes""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""label""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskStatuses >>
			{
				var id = new Guid("f09fe186-8617-4f94-a67b-3a69172b1257");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("fad53f3d-4d3b-4c7b-8cd2-23e96a086ad8");
				var name = @"TaskStatuses";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllTasks >>
			{
				var id = new Guid("9c8ec6cc-b389-4baa-b4ce-770edf2520dd");
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var dataSourceId = new Guid("5a6e9d56-63bc-43b1-b95e-24838db9f435");
				var name = @"AllTasks";
				var parameters = @"[{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy}}""},{""name"":""sortOrder"",""type"":""text"",""value"":""{{RequestQuery.sortOrder}}""},{""name"":""page"",""type"":""int"",""value"":""{{RequestQuery.page}}""},{""name"":""searchQuery"",""type"":""text"",""value"":""{{RequestQuery.q_x_search_v}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskTypes >>
			{
				var id = new Guid("9e50f76d-f56c-4204-9d8b-4db8860371a5");
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var dataSourceId = new Guid("4857ace4-fcfc-4803-ad86-7c7afba91ce0");
				var name = @"TaskTypes";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskTypeSelectOptions >>
			{
				var id = new Guid("120c783a-f04c-4be9-a9ef-f991aae3d648");
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"TaskTypeSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""TaskTypes""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllAccounts >>
			{
				var id = new Guid("cf3b936e-ec45-4937-a157-a008ef97d594");
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var dataSourceId = new Guid("61d21547-b353-48b8-8b75-b727680da79e");
				var name = @"AllAccounts";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: FeedItemsForRecordId >>
			{
				var id = new Guid("ee65976e-d5d0-4dd4-ac6a-2047e8817add");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("74e5a414-6deb-4af6-8e29-567f718ca430");
				var name = @"FeedItemsForRecordId";
				var parameters = @"[{""name"":""recordId"",""type"":""text"",""value"":""{{Record.id}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: CommentsForRecordId >>
			{
				var id = new Guid("2f523831-0437-4250-a6b5-8eeb3da9d04c");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("a588e096-358d-4426-adf6-5db693f32322");
				var name = @"CommentsForRecordId";
				var parameters = @"[{""name"":""recordId"",""type"":""text"",""value"":""{{Record.id}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TimeLogsForRecordId >>
			{
				var id = new Guid("24e093ae-ab0f-4c52-86b2-9e1fe2ed2a0a");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("e66b8374-82ea-4305-8456-085b3a1f1f2d");
				var name = @"TimeLogsForRecordId";
				var parameters = @"[{""name"":""recordId"",""type"":""text"",""value"":""{{Record.id}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: CurrentDate >>
			{
				var id = new Guid("361dc0a8-68b8-45ec-8002-11779a304899");
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var dataSourceId = new Guid("64207638-d75e-4a25-9965-6e35b0aa835a");
				var name = @"CurrentDate";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: ProjectWidgetMyTasks >>
			{
				var id = new Guid("e688cbdd-0fa9-43b4-aed1-3d667fdecf87");
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var dataSourceId = new Guid("c44eab77-c81e-4f55-95c8-4949b275fc99");
				var name = @"ProjectWidgetMyTasks";
				var parameters = @"[{""name"":""userId"",""type"":""guid"",""value"":""{{CurrentUser.Id}}""},{""name"":""currentDate"",""type"":""date"",""value"":""{{CurrentDate}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskStatusesSelectOption >>
			{
				var id = new Guid("f5a2f77f-6d79-4180-b73f-7deb21895f4e");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"TaskStatusesSelectOption";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""TaskStatuses""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""label""},{""name"":""SortOrderPropName"",""type"":""text"",""value"":""sort_index""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: FeedItemsForRecordId >>
			{
				var id = new Guid("7717b418-7eed-472a-b4cd-6ada2e85d6df");
				var pageId = new Guid("dfe56667-174d-492d-8f84-b8ab8b70c63f");
				var dataSourceId = new Guid("74e5a414-6deb-4af6-8e29-567f718ca430");
				var name = @"FeedItemsForRecordId";
				var parameters = @"[{""name"":""recordId"",""type"":""text"",""value"":""{{Record.id}}""},{""name"":""type"",""type"":""text"",""value"":""{{RequestQuery.type}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: ProjectWidgetMyTasksDueToday >>
			{
				var id = new Guid("f1da592e-d696-426a-a60c-ef262d101a56");
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var dataSourceId = new Guid("eae07b63-9bf4-4e25-80af-df5228dedf35");
				var name = @"ProjectWidgetMyTasksDueToday";
				var parameters = @"[{""name"":""userId"",""type"":""guid"",""value"":""{{CurrentUser.Id}}""},{""name"":""currentDate"",""type"":""date"",""value"":""{{CurrentDate}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllAccounts >>
			{
				var id = new Guid("26443ab3-5dd7-42ef-85c6-8f6a1c271957");
				var pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var dataSourceId = new Guid("61d21547-b353-48b8-8b75-b727680da79e");
				var name = @"AllAccounts";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: CurrentDate >>
			{
				var id = new Guid("cb29b5cf-18b4-404c-bd8e-511766624ad7");
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var dataSourceId = new Guid("64207638-d75e-4a25-9965-6e35b0aa835a");
				var name = @"CurrentDate";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TrackTimeTasks >>
			{
				var id = new Guid("9ba5e65b-b10c-4217-8aa8-e2d3db5f22f8");
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var dataSourceId = new Guid("473ee9b6-2131-4164-b5fe-d9b3073e9178");
				var name = @"TrackTimeTasks";
				var parameters = @"[{""name"":""search_query"",""type"":""text"",""value"":""{{RequestQuery.q_x_fts_v}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AccountSelectOptions >>
			{
				var id = new Guid("40b84bc0-00bc-422c-a292-e1f805d3ad93");
				var pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AccountSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllAccounts""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""name""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: NoOwnerTasks >>
			{
				var id = new Guid("0335866b-023d-4922-af27-a27960d72177");
				var pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
				var dataSourceId = new Guid("40c0bcc6-2e3e-4b68-ae6a-27f1f472f069");
				var name = @"NoOwnerTasks";
				var parameters = @"[{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy}}""},{""name"":""sortOrder"",""type"":""text"",""value"":""{{RequestQuery.sortOrder}}""},{""name"":""page"",""type"":""int"",""value"":""{{RequestQuery.page}}""},{""name"":""searchQuery"",""type"":""text"",""value"":""{{RequestQuery.q_x_search_v}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: FeedItemsForRecordId >>
			{
				var id = new Guid("0b3fefbc-0c11-4d22-8343-8d638165a026");
				var pageId = new Guid("acb76466-32b8-428c-81cb-47b6013879e7");
				var dataSourceId = new Guid("74e5a414-6deb-4af6-8e29-567f718ca430");
				var name = @"FeedItemsForRecordId";
				var parameters = @"[{""name"":""recordId"",""type"":""text"",""value"":""{{CurrentUser.Id}}""},{""name"":""type"",""type"":""text"",""value"":""{{RequestQuery.type}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllOpenTasks >>
			{
				var id = new Guid("952120f1-d736-400c-817a-1f43ac455bc3");
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var dataSourceId = new Guid("9c2337ac-b505-4ce4-b1ff-ffde2e37b312");
				var name = @"AllOpenTasks";
				var parameters = @"[{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy}}""},{""name"":""sortOrder"",""type"":""text"",""value"":""{{RequestQuery.sortOrder}}""},{""name"":""page"",""type"":""int"",""value"":""{{RequestQuery.page}}""},{""name"":""searchQuery"",""type"":""text"",""value"":""{{RequestQuery.q_x_search_v}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskStatuses >>
			{
				var id = new Guid("39358f5c-122d-40a8-8501-7e944f72ec7d");
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var dataSourceId = new Guid("fad53f3d-4d3b-4c7b-8cd2-23e96a086ad8");
				var name = @"TaskStatuses";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskStatusSelectOptions >>
			{
				var id = new Guid("676f2a8a-cbeb-40e7-b9fb-66cfd3cb9a1b");
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"TaskStatusSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""TaskStatuses""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion



		}
	}
}
