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
		private static void Patch20190126(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{

			#region << ***Delete page body node*** Page name: no-owner ID: 4ac33cdb-3058-4a85-833c-9d8473d48dfe >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("4ac33cdb-3058-4a85-833c-9d8473d48dfe"), WebVella.Erp.Database.DbContext.Current.Transaction);
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

			#region << ***Update page*** Page name: no-owner >>
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
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().UpdatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page*** Page name: all >>
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
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().UpdatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: track-time ID: e84c527a-4feb-4d60-ab91-4b1ecd89b39c >>
			{
				var id = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? parentId = null;
				Guid? nodeId = null;
				Guid pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
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
  ""container3_label"": ""action"",
  ""container3_width"": ""130px"",
  ""container3_name"": """",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_class"": """",
  ""container3_vertical_align"": ""3"",
  ""container3_horizontal_align"": ""1"",
  ""container4_label"": ""status"",
  ""container4_width"": ""200px"",
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: ea44e884-d6da-4666-953e-d05ac3f24b57 >>
			{
				var id = new Guid("ea44e884-d6da-4666-953e-d05ac3f24b57");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column4";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.status_id\"",\""default\"":\""\""}"",
  ""name"": ""status_id"",
  ""try_connect_to_entity"": ""true"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""TaskStatusSelectOptions\"",\""default\"":\""\""}"",
  ""connected_entity_id"": ""9386226e-381e-4522-b27b-fb5514d77902"",
  ""mode"": ""0""
}";
				var weight = 1;

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

			#region << ***Update data source*** Name: ProjectWidgetMyTasksDueToday >>
			{
				var id = new Guid("eae07b63-9bf4-4e25-80af-df5228dedf35");
				var name = @"ProjectWidgetMyTasksDueToday";
				var description = @"My tasks due today";
				var eqlText = @"SELECT *,$project_nn_task.name
FROM task
WHERE owner_id = @userId AND target_date = @currentDate AND status_id <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf'
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
	 rec_task.""start_date"" AS ""start_date"",
	 rec_task.""target_date"" AS ""target_date"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
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
WHERE  (  (  ( rec_task.""owner_id"" = @userId )  AND  ( rec_task.""target_date"" = @currentDate )  )  AND  ( rec_task.""status_id"" <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf' )  ) 
ORDER BY rec_task.""priority"" DESC
) X
";
				var parametersJson = @"[{""name"":""userId"",""type"":""guid"",""value"":""guid.empty""},{""name"":""currentDate"",""type"":""date"",""value"":""now""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""target_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Update(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Update data source*** Name: ProjectWidgetMyTasks >>
			{
				var id = new Guid("c44eab77-c81e-4f55-95c8-4949b275fc99");
				var name = @"ProjectWidgetMyTasks";
				var description = @"top 5 upcoming tasks";
				var eqlText = @"SELECT *,$project_nn_task.name
FROM task
WHERE owner_id = @userId AND (target_date > @currentDate OR target_date = null) AND status_id <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf'
ORDER BY target_date ASC, priority DESC
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
	 rec_task.""start_date"" AS ""start_date"",
	 rec_task.""target_date"" AS ""target_date"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
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
WHERE  (  (  ( rec_task.""owner_id"" = @userId )  AND  (  ( rec_task.""target_date"" > @currentDate )  OR  ( rec_task.""target_date"" IS NULL )  )  )  AND  ( rec_task.""status_id"" <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf' )  ) 
ORDER BY rec_task.""target_date"" ASC , rec_task.""priority"" DESC
LIMIT 5
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""userId"",""type"":""guid"",""value"":""guid.empty""},{""name"":""currentDate"",""type"":""date"",""value"":""now""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""target_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Update(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Update data source*** Name: ProjectWidgetMyOverdueTasks >>
			{
				var id = new Guid("946919a6-e4cd-41a2-97dc-1069d73adcd1");
				var name = @"ProjectWidgetMyOverdueTasks";
				var description = @"all my overdue tasks";
				var eqlText = @"SELECT *,$project_nn_task.name
FROM task
WHERE owner_id = @userId AND target_date < @currentDate AND status_id <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf'
ORDER BY target_date ASC, priority DESC ";
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
	 rec_task.""start_date"" AS ""start_date"",
	 rec_task.""target_date"" AS ""target_date"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
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
WHERE  (  (  ( rec_task.""owner_id"" = @userId )  AND  ( rec_task.""target_date"" < @currentDate )  )  AND  ( rec_task.""status_id"" <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf' )  ) 
ORDER BY rec_task.""target_date"" ASC , rec_task.""priority"" DESC
) X
";
				var parametersJson = @"[{""name"":""userId"",""type"":""guid"",""value"":""guid.empty""},{""name"":""currentDate"",""type"":""date"",""value"":""now""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""target_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Update(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Update data source*** Name: NoOwnerTasks >>
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
	 rec_task.""start_date"" AS ""start_date"",
	 rec_task.""target_date"" AS ""target_date"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
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
ORDER BY rec_task.""target_date"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""target_date""},{""name"":""sortOrder"",""type"":""text"",""value"":""asc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""target_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task_creator"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Update(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
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
	 rec_task.""start_date"" AS ""start_date"",
	 rec_task.""target_date"" AS ""target_date"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
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
ORDER BY rec_task.""target_date"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""target_date""},{""name"":""sortOrder"",""type"":""text"",""value"":""asc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""target_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task_creator"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create page data source*** Name: AllOpenTasks >>
			{
				var id = new Guid("952120f1-d736-400c-817a-1f43ac455bc3");
				var pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
				var dataSourceId = new Guid("9c2337ac-b505-4ce4-b1ff-ffde2e37b312");
				var name = @"AllOpenTasks";
				var parameters = @"[{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy}}""},{""name"":""sortOrder"",""type"":""text"",""value"":""{{RequestQuery.sortOrder}}""},{""name"":""page"",""type"":""int"",""value"":""{{RequestQuery.page}}""},{""name"":""searchQuery"",""type"":""text"",""value"":""{{RequestQuery.q_x_search_v}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskStatuses >>
			{
				var id = new Guid("39358f5c-122d-40a8-8501-7e944f72ec7d");
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var dataSourceId = new Guid("fad53f3d-4d3b-4c7b-8cd2-23e96a086ad8");
				var name = @"TaskStatuses";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskStatusSelectOptions >>
			{
				var id = new Guid("676f2a8a-cbeb-40e7-b9fb-66cfd3cb9a1b");
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"TaskStatusSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""TaskStatuses""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion



		}
	}
}
