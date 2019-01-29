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
		private static void Patch20190128(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{

			#region << ***Delete page body node*** Page name: comment-test ID: ba6e3266-baed-4aba-9eb8-b964f2ed06ad >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("ba6e3266-baed-4aba-9eb8-b964f2ed06ad"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete page body node*** Page name: comment-test ID: f22e377d-2725-4052-b5be-b4c9597a6ce9 >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("f22e377d-2725-4052-b5be-b4c9597a6ce9"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete page body node*** Page name: comment-test ID: 3463f4aa-ee46-472d-bd95-a6c9a3c09ee7 >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("3463f4aa-ee46-472d-bd95-a6c9a3c09ee7"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete page body node*** Page name: comment-test ID: 7f36d6d5-02bf-4c41-9c22-94a9f1d19500 >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("7f36d6d5-02bf-4c41-9c22-94a9f1d19500"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete page body node*** Page name: comment-test ID: 092f0714-92d7-47a3-bc7c-a46b966393c8 >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("092f0714-92d7-47a3-bc7c-a46b966393c8"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete page body node*** Page name: comment-test ID: a6d7045d-0368-4103-adab-e6aeecc7c40b >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("a6d7045d-0368-4103-adab-e6aeecc7c40b"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete page body node*** Page name: comment-test ID: 28dd5bc8-0672-4217-aba1-b50d6740247d >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("28dd5bc8-0672-4217-aba1-b50d6740247d"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete page body node*** Page name: track-time ID: 663aa356-14d2-4e22-8dc8-a12b9fc971a1 >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("663aa356-14d2-4e22-8dc8-a12b9fc971a1"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete page body node*** Page name: comment-test ID: eb825f3d-8273-4b1b-8df6-b1d286b3644f >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("eb825f3d-8273-4b1b-8df6-b1d286b3644f"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete page*** Page name: comment-test >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePage(new Guid("9401d420-808d-475f-a38a-bd34d3763ce9"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete sitemap node*** Node name: accounts >>
			{

				new WebVella.Erp.Web.Services.AppService().DeleteAreaNode(new Guid("c087059c-7ce2-4c8e-be28-265f05ac3d0f"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete sitemap node*** Node name: comments >>
			{

				new WebVella.Erp.Web.Services.AppService().DeleteAreaNode(new Guid("fa94b569-67f2-4805-97c3-517ab52fb4d9"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete sitemap area*** Area name: accounts >>
			{

				new WebVella.Erp.Web.Services.AppService().DeleteArea(new Guid("a0ef4c2d-b837-4428-b726-0de89ea19867"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete app*** App name: crm >>
			{

				new WebVella.Erp.Web.Services.AppService().DeleteApplication(new Guid("45291b1a-a590-4633-b0bb-11e97c179e91"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap area*** Sitemap area name: tasks >>
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

				new WebVella.Erp.Web.Services.AppService().UpdateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap area*** Sitemap area name: projects >>
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

				new WebVella.Erp.Web.Services.AppService().UpdateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
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

			#region << ***Update sitemap area*** Sitemap area name: reports >>
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

				new WebVella.Erp.Web.Services.AppService().UpdateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
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

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page*** Page name: list >>
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
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var entityId = new Guid("2e22b50f-e444-4b62-a171-076e51246939");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().UpdatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page*** Page name: details >>
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
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var entityId = new Guid("2e22b50f-e444-4b62-a171-076e51246939");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().UpdatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page*** Page name: create >>
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
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var entityId = new Guid("2e22b50f-e444-4b62-a171-076e51246939");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().UpdatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: track-time ID: 6bb17b95-258a-4572-99f3-898d1895cfba >>
			{
				var id = new Guid("6bb17b95-258a-4572-99f3-898d1895cfba");
				Guid? parentId = null;
				Guid? nodeId = null;
				Guid pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcValidation";
				var containerId = "";
				var options = @"{
  ""validation"": ""{\""type\"":\""0\"",\""string\"":\""Validation\"",\""default\"":\""\""}""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: list ID: c0689f85-235d-484e-bea3-e534e6e10094 >>
			{
				var id = new Guid("c0689f85-235d-484e-bea3-e534e6e10094");
				Guid? parentId = new Guid("dedd97f6-1b09-4942-aae1-684cdc49a3eb");
				Guid? nodeId = null;
				Guid pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: list ID: 77abedcf-4bea-46f3-b50c-340a7aa237d6 >>
			{
				var id = new Guid("77abedcf-4bea-46f3-b50c-340a7aa237d6");
				Guid? parentId = new Guid("13a1d868-93ee-41d1-bb94-231d99899f74");
				Guid? nodeId = null;
				Guid pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
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

			#region << ***Update page body node*** Page: create ID: d32f39bb-8ad4-438d-a8d1-7abca6f5e6b4 >>
			{
				var id = new Guid("d32f39bb-8ad4-438d-a8d1-7abca6f5e6b4");
				Guid? parentId = null;
				Guid? nodeId = null;
				Guid pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: list ID: 3845960e-4fc6-40f6-9ef6-36e7392f8ab0 >>
			{
				var id = new Guid("3845960e-4fc6-40f6-9ef6-36e7392f8ab0");
				Guid? parentId = new Guid("492d9088-16bc-40fd-963b-8a8c2acf0ffa");
				Guid? nodeId = null;
				Guid pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
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
  ""html"": ""{\""type\"":\""2\"",\""string\"":\""<script src=\\\""/api/v3.0/p/project/files/timetrack.js\\\"" type=\\\""text/javascript\\\""></script>\"",\""default\"":\""\""}""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion



		}
	}
}
