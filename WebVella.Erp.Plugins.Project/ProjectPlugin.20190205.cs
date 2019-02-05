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
		private static void Patch20190205(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{

			#region << ***Update page body node*** Page: details ID: 552a4fad-5236-4aad-b3fc-443a5f12e574 >>
			{
				var id = new Guid("552a4fad-5236-4aad-b3fc-443a5f12e574");
				Guid? parentId = null;
				Guid? nodeId = null;
				Guid pageId = new Guid("80b10445-c850-44cf-9c8c-57daca671dcf");
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
  ""return_url"": ""/projects/accounts/accounts/l/list""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: aa94aac4-5048-4d82-95b2-b38536028cbb >>
			{
				var id = new Guid("aa94aac4-5048-4d82-95b2-b38536028cbb");
				Guid? parentId = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Estimated (min)"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.estimated_minutes\"",\""default\"":\""\""}"",
  ""name"": ""estimated_minutes"",
  ""mode"": ""3"",
  ""decimal_digits"": 0,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 857698b9-f715-480a-bd74-29819a4dec2d >>
			{
				var id = new Guid("857698b9-f715-480a-bd74-29819a4dec2d");
				Guid? parentId = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Billable (min)"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.x_billable_minutes\"",\""default\"":\""\""}"",
  ""name"": ""x_billable_minutes"",
  ""mode"": ""2"",
  ""decimal_digits"": 0,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: ddde395b-6cee-4907-a220-a8424e091b13 >>
			{
				var id = new Guid("ddde395b-6cee-4907-a220-a8424e091b13");
				Guid? parentId = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Nonbillable (min)"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.x_nonbillable_minutes\"",\""default\"":\""\""}"",
  ""name"": ""x_nonbillable_minutes"",
  ""mode"": ""2"",
  ""decimal_digits"": 0,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 9f15bb3a-b6bf-424c-9394-669cc2041215 >>
			{
				var id = new Guid("9f15bb3a-b6bf-424c-9394-669cc2041215");
				Guid? parentId = new Guid("ecc262e9-fbad-4dd1-9c98-56ad047685fb");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Watchers"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing System.Linq;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry{\\n\\t\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\t\\tif (pageModel == null)\\n\\t\\t\\t\\treturn null;\\n\\n\\t\\t\\tvar taskAuxData = pageModel.TryGetDataSourceProperty<EntityRecordList>(\\\""TaskAuxData\\\"");\\n\\t        var currentUser = pageModel.TryGetDataSourceProperty<ErpUser>(\\\""CurrentUser\\\"");\\n\\t        var recordId = pageModel.TryGetDataSourceProperty<Guid>(\\\""RecordId\\\"");\\n\\t\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\t\\tif (taskAuxData == null && !taskAuxData.Any())\\n\\t\\t\\t\\treturn \\\""\\\"";\\n\\t        var watcherIdList = new List<Guid>();\\n\\t        if(taskAuxData[0].Properties.ContainsKey(\\\""$user_nn_task_watchers\\\"") && taskAuxData[0][\\\""$user_nn_task_watchers\\\""] != null \\n\\t            && taskAuxData[0][\\\""$user_nn_task_watchers\\\""] is List<EntityRecord>){\\n\\t                watcherIdList = ((List<EntityRecord>)taskAuxData[0][\\\""$user_nn_task_watchers\\\""]).Select(x=> (Guid)x[\\\""id\\\""]).ToList();\\n\\t            }\\n\\t        var watcherCount = watcherIdList.Count;\\n\\t        var currentUserIsWatching = false;\\n\\t        if(currentUser != null && watcherIdList.Contains(currentUser.Id))\\n\\t            currentUserIsWatching = true;\\n\\t\\n\\t        var html = $\\\""<span class='badge go-bkg-blue-gray-light mr-2'>{watcherCount}</span>\\\"";\\n\\t        if(currentUserIsWatching)\\n\\t            html += \\\""<a href=\\\\\\\""#\\\\\\\"" onclick=\\\\\\\""StopTaskWatch('\\\"" + recordId + \\\""')\\\\\\\"">stop watching</a>\\\"";\\n\\t        else\\n\\t            html += \\\""<a href=\\\\\\\""#\\\\\\\"" onclick=\\\\\\\""StartTaskWatch('\\\"" + recordId + \\\""')\\\\\\\"">start watching</a>\\\"";\\n\\t\\n\\t\\t\\treturn html;\\n\\t\\t}\\n\\t\\tcatch(Exception ex){\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""field"",
  ""mode"": ""2"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1"",
  ""connected_entity_id"": """"
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: b2935724-bfcc-4821-bdb2-81bc9b14f015 >>
			{
				var id = new Guid("b2935724-bfcc-4821-bdb2-81bc9b14f015");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 526c7435-9ace-4032-b754-5d2e9c817436 >>
			{
				var id = new Guid("526c7435-9ace-4032-b754-5d2e9c817436");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Recurrence"",
  ""label_mode"": ""2"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry{\\n\\t\\t\\tif (pageModel == null)\\n\\t\\t\\t\\treturn null;\\n\\t\\n\\t\\t\\t//try read data source by name and get result as specified type object\\n\\t\\t\\tvar dataSource = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""Record\\\"");\\n\\t\\n\\t\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\t\\tif (dataSource == null)\\n\\t\\t\\t\\treturn null;\\n\\t\\n\\t\\t\\treturn \\\""<a href='#' onclick=\\\\\\\""ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:'wv-97402edb-3a5a-4cc3-bc40-4d4d012619e2',action:'open',payload:null})\\\\\\\"">Does not repeat</a>\\\"";\\n\\t\\t}\\n\\t\\tcatch(Exception ex){\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""field"",
  ""mode"": ""2"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1"",
  ""connected_entity_id"": """"
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 97402edb-3a5a-4cc3-bc40-4d4d012619e2 >>
			{
				var id = new Guid("97402edb-3a5a-4cc3-bc40-4d4d012619e2");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcModal";
				var containerId = "body";
				var options = @"{
  ""title"": ""Task recurrence setting"",
  ""backdrop"": ""true"",
  ""size"": ""2"",
  ""position"": ""0""
}";
				var weight = 5;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 0abd8d18-1e8f-418c-a18b-8c337e2ad43e >>
			{
				var id = new Guid("0abd8d18-1e8f-418c-a18b-8c337e2ad43e");
				Guid? parentId = new Guid("97402edb-3a5a-4cc3-bc40-4d4d012619e2");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "footer";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""Close"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:'wv-97402edb-3a5a-4cc3-bc40-4d4d012619e2',action:'close',payload:null})"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 394d04b3-7b5b-4cdb-b74e-e6f1c4fda8c3 >>
			{
				var id = new Guid("394d04b3-7b5b-4cdb-b74e-e6f1c4fda8c3");
				Guid? parentId = new Guid("97402edb-3a5a-4cc3-bc40-4d4d012619e2");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "footer";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Save"",
  ""color"": ""1"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fas fa-save"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": ""wv-f3661768-ad30-4949-8a87-499ca0ab5491""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: f3661768-ad30-4949-8a87-499ca0ab5491 >>
			{
				var id = new Guid("f3661768-ad30-4949-8a87-499ca0ab5491");
				Guid? parentId = new Guid("97402edb-3a5a-4cc3-bc40-4d4d012619e2");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-f3661768-ad30-4949-8a87-499ca0ab5491"",
  ""name"": ""form"",
  ""hook_key"": ""SetTaskRecurrence"",
  ""method"": ""post"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: c2081b0a-c230-4f5a-959e-75c78c70132f >>
			{
				var id = new Guid("c2081b0a-c230-4f5a-959e-75c78c70132f");
				Guid? parentId = new Guid("f3661768-ad30-4949-8a87-499ca0ab5491");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Plugins.Project.Components.PcTaskRepeatRecurrenceSet";
				var containerId = "body";
				var options = @"{}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 1428fd69-6431-4a51-8051-5d24692a0730 >>
			{
				var id = new Guid("1428fd69-6431-4a51-8051-5d24692a0730");
				Guid? parentId = new Guid("f3661768-ad30-4949-8a87-499ca0ab5491");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "body";
				var options = @"{
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}"",
  ""name"": ""id""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: track-time ID: 9dcca796-cb6d-4c7f-bb63-761cff4c218a >>
			{
				var id = new Guid("9dcca796-cb6d-4c7f-bb63-761cff4c218a");
				Guid? parentId = new Guid("3658981b-cef7-4938-9c3a-a13cd5b760a0");
				Guid? nodeId = null;
				Guid pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Logged Minutes"",
  ""label_mode"": ""0"",
  ""value"": """",
  ""name"": ""minutes"",
  ""mode"": ""0"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0,
  ""connected_entity_id"": ""750153c5-1df9-408f-b856-727078a525bc""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion


		}
	}
}
