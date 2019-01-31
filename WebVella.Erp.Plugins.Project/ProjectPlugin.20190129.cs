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
		private static void Patch20190129(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
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
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""name"": ""name"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""maxlength"": 0
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

			#region << ***Create page data source*** Name: Accounts >>
			{
				var id = new Guid("a2db7724-f05b-4820-9269-64792398c309");
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var dataSourceId = new Guid("61d21547-b353-48b8-8b75-b727680da79e");
				var name = @"Accounts";
				var parameters = @"[{""name"":""name"",""type"":""text"",""value"":""{{RequestQuery.q_name_v}}""},{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy ?? name}}""},{""name"":""page"",""type"":""int"",""value"":""{{RequestQuery.page ?? 1 }}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion



		}
	}
}
