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
		private static void Patch20190130(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Delete page body node*** Page name: details ID: 8eca6986-c648-4815-b4a6-af2580c53ce2 >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("8eca6986-c648-4815-b4a6-af2580c53ce2"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete page body node*** Page name: details ID: 63bada37-e9c5-497f-8d7d-4ef34fdf3cd4 >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("63bada37-e9c5-497f-8d7d-4ef34fdf3cd4"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete page body node*** Page name: details ID: dac09646-e097-4b8c-9854-cc6fb2362af5 >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("dac09646-e097-4b8c-9854-cc6fb2362af5"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page*** Page name: feed >>
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
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = null;
				var nodeId = new Guid("8950c6c6-7848-4a0b-b260-e8dbedf7486c");
				var areaId = new Guid("24028a64-748b-43a2-98ae-47514da142fe");
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().UpdatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 7eb7af4f-bdd3-410a-b3c4-71e620b627c5 >>
			{
				var id = new Guid("7eb7af4f-bdd3-410a-b3c4-71e620b627c5");
				Guid? parentId = new Guid("03d2ed0f-33ed-4b7d-84fb-102f4b7452a8");
				Guid? nodeId = null;
				Guid pageId = new Guid("80b10445-c850-44cf-9c8c-57daca671dcf");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 754bf941-df31-4b13-ba32-eb3c7a8c8922 >>
			{
				var id = new Guid("754bf941-df31-4b13-ba32-eb3c7a8c8922");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 6e918333-a2fa-4cf7-9ca8-662e349625a7 >>
			{
				var id = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 452a6f4c-b415-409a-b9b6-a2918a137299 >>
			{
				var id = new Guid("452a6f4c-b415-409a-b9b6-a2918a137299");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: ecc262e9-fbad-4dd1-9c98-56ad047685fb >>
			{
				var id = new Guid("ecc262e9-fbad-4dd1-9c98-56ad047685fb");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 101245d5-1ff9-4eb3-ba28-0b29cb56a0ec >>
			{
				var id = new Guid("101245d5-1ff9-4eb3-ba28-0b29cb56a0ec");
				Guid? parentId = new Guid("ecc262e9-fbad-4dd1-9c98-56ad047685fb");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
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

			#region << ***Update page body node*** Page: details ID: 27843f6e-43ed-49e7-9cc5-ec35393e93f4 >>
			{
				var id = new Guid("27843f6e-43ed-49e7-9cc5-ec35393e93f4");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 651e5fb2-56df-4c46-86b3-19a641dc942d >>
			{
				var id = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: b105d13c-3710-4ace-b51f-b57323912524 >>
			{
				var id = new Guid("b105d13c-3710-4ace-b51f-b57323912524");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 70a864dc-8311-4dd3-bc13-1a3b87821e30 >>
			{
				var id = new Guid("70a864dc-8311-4dd3-bc13-1a3b87821e30");
				Guid? parentId = new Guid("b105d13c-3710-4ace-b51f-b57323912524");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 1fff4a92-d045-4019-b27c-bccb1fd1cb82 >>
			{
				var id = new Guid("1fff4a92-d045-4019-b27c-bccb1fd1cb82");
				Guid? parentId = new Guid("b105d13c-3710-4ace-b51f-b57323912524");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: ee526509-7840-498a-9c1f-8a69d80c5f2e >>
			{
				var id = new Guid("ee526509-7840-498a-9c1f-8a69d80c5f2e");
				Guid? parentId = new Guid("b105d13c-3710-4ace-b51f-b57323912524");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 747c108b-ed45-46f3-b06a-113e2490888d >>
			{
				var id = new Guid("747c108b-ed45-46f3-b06a-113e2490888d");
				Guid? parentId = new Guid("2d3dddf7-cefb-4073-977f-4e1b6bf8935e");
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: df7c7cab-0e16-4e75-bb13-04666afeff81 >>
			{
				var id = new Guid("df7c7cab-0e16-4e75-bb13-04666afeff81");
				Guid? parentId = new Guid("2d3dddf7-cefb-4073-977f-4e1b6bf8935e");
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 302de86f-7178-4e2b-9ac1-d447163a9558 >>
			{
				var id = new Guid("302de86f-7178-4e2b-9ac1-d447163a9558");
				Guid? parentId = new Guid("2d3dddf7-cefb-4073-977f-4e1b6bf8935e");
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: be6aa619-e380-4bf9-b279-47dda4d5f4eb >>
			{
				var id = new Guid("be6aa619-e380-4bf9-b279-47dda4d5f4eb");
				Guid? parentId = new Guid("6029e40b-0835-460f-b782-1e4228ea4234");
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 0dbdb202-7288-49e6-b922-f69e947590e5 >>
			{
				var id = new Guid("0dbdb202-7288-49e6-b922-f69e947590e5");
				Guid? parentId = new Guid("6029e40b-0835-460f-b782-1e4228ea4234");
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 7f01d2c0-2542-4b88-b8f0-711947e4d0c6 >>
			{
				var id = new Guid("7f01d2c0-2542-4b88-b8f0-711947e4d0c6");
				Guid? parentId = new Guid("6029e40b-0835-460f-b782-1e4228ea4234");
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: track-time ID: b14bb20c-fab7-40a4-8feb-8a899b761dda >>
			{
				var id = new Guid("b14bb20c-fab7-40a4-8feb-8a899b761dda");
				Guid? parentId = null;
				Guid? nodeId = null;
				Guid pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcHtmlBlock";
				var containerId = "";
				var options = @"{
  ""html"": ""{\""type\"":\""2\"",\""string\"":\""<script src=\\\""/api/v3.0/p/project/files/javascript?file=timetrack.js\\\"" type=\\\""text/javascript\\\""></script>\"",\""default\"":\""\""}""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 6de13934-ca81-4807-bb71-cadcdbb99ca7 >>
			{
				var id = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? parentId = null;
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 551483ab-262b-4541-b0dc-fadaa8de5284 >>
			{
				var id = new Guid("551483ab-262b-4541-b0dc-fadaa8de5284");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 7a7fbcd5-fb6f-40fd-a0cd-1a7c26e1c4ab >>
			{
				var id = new Guid("7a7fbcd5-fb6f-40fd-a0cd-1a7c26e1c4ab");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: b37c63a7-84ea-4673-9a81-ec4313c178b7 >>
			{
				var id = new Guid("b37c63a7-84ea-4673-9a81-ec4313c178b7");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 30676929-f280-414d-8f4c-d41f851136ce >>
			{
				var id = new Guid("30676929-f280-414d-8f4c-d41f851136ce");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: b2caeb51-b6a5-4e15-a317-9825511792c6 >>
			{
				var id = new Guid("b2caeb51-b6a5-4e15-a317-9825511792c6");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 8a75b1d8-8184-40ed-a977-26616239fbb7 >>
			{
				var id = new Guid("8a75b1d8-8184-40ed-a977-26616239fbb7");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				Guid pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update data source*** Name: TaskAuxData >>
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

				new WebVella.Erp.Database.DbDataSourceRepository().Update(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Update page data source*** Name: FeedItemsForRecordId >>
			{
				var id = new Guid("0b3fefbc-0c11-4d22-8343-8d638165a026");
				var pageId = new Guid("acb76466-32b8-428c-81cb-47b6013879e7");
				var dataSourceId = new Guid("74e5a414-6deb-4af6-8e29-567f718ca430");
				var name = @"FeedItemsForRecordId";
				var parameters = @"[{""name"":""recordId"",""type"":""text"",""value"":""{{CurrentUser.Id}}""},{""name"":""type"",""type"":""text"",""value"":""{{RequestQuery.type}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Update(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion



		}
	}
}
