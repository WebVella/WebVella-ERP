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
		private static void Patch20190125(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{

			#region << ***Delete page body node*** Page name: all ID: 7b98ed4c-9240-46a2-bdd9-5904eeb3c7a3 >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("7b98ed4c-9240-46a2-bdd9-5904eeb3c7a3"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Delete page body node*** Page name: dashboard ID: 6b53e36e-9003-4325-8899-7fbf9da32209 >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("6b53e36e-9003-4325-8899-7fbf9da32209"), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page*** Page name: tasks >>
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
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().UpdatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: track-time ID: b2baa937-e32a-4a06-8b9b-404f89e539c0 >>
			{
				var id = new Guid("b2baa937-e32a-4a06-8b9b-404f89e539c0");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				Guid pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
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

			#region << ***Update page body node*** Page: details ID: 67be245e-07be-4b05-aaf2-b769537878f9 >>
			{
				var id = new Guid("67be245e-07be-4b05-aaf2-b769537878f9");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.start_date\"",\""default\"":\""\""}"",
  ""name"": ""start_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3""
}";
				var weight = 1;

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
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""2""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 18934356-8467-41a4-b57f-f8528f151437 >>
			{
				var id = new Guid("18934356-8467-41a4-b57f-f8528f151437");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Start Date"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.start_date\"",\""default\"":\""\""}"",
  ""name"": ""start_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""1""
}";
				var weight = 6;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: e8b748e0-2e0e-42f1-93ee-6854fb01531a >>
			{
				var id = new Guid("e8b748e0-2e0e-42f1-93ee-6854fb01531a");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.target_date\"",\""default\"":\""\""}"",
  ""name"": ""target_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0""
}";
				var weight = 7;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: create ID: e03e40c2-dae2-4351-947c-02295a064328 >>
			{
				var id = new Guid("e03e40c2-dae2-4351-947c-02295a064328");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				Guid pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: create ID: 1739a2f0-76ba-4343-a344-9b0564096d06 >>
			{
				var id = new Guid("1739a2f0-76ba-4343-a344-9b0564096d06");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				Guid pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: account-monthly-timelog ID: 0c32036a-4432-4b17-beb7-198ba22ea134 >>
			{
				var id = new Guid("0c32036a-4432-4b17-beb7-198ba22ea134");
				Guid? parentId = new Guid("7eff5a2c-5d5d-4989-a68f-a1362b0dad7c");
				Guid? nodeId = null;
				Guid pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Month"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SampleCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry\\n\\t\\t{\\n\\t\\t    var value = (string)pageModel.DataModel.GetProperty(\\\""RequestQuery.month\\\"");\\n\\t\\t    if(string.IsNullOrWhiteSpace(value))\\n\\t\\t\\t    return DateTime.Now.Month;\\n\\t\\t\\telse\\n\\t\\t\\t    return value;\\n\\t\\t}\\n\\t\\tcatch(Exception ex){\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\"",\""default\"":\""\""}"",
  ""name"": ""month"",
  ""try_connect_to_entity"": ""false"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""MonthSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: account-monthly-timelog ID: 70424cd2-2b69-4c87-9977-cb60a72239fd >>
			{
				var id = new Guid("70424cd2-2b69-4c87-9977-cb60a72239fd");
				Guid? parentId = new Guid("7eff5a2c-5d5d-4989-a68f-a1362b0dad7c");
				Guid? nodeId = null;
				Guid pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Year"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SampleCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry\\n\\t\\t{\\n\\t\\t    var value = (string)pageModel.DataModel.GetProperty(\\\""RequestQuery.year\\\"");\\n\\t\\t    if(string.IsNullOrWhiteSpace(value))\\n\\t\\t\\t    return DateTime.Now.Year;\\n\\t\\t\\telse\\n\\t\\t\\t    return value;\\n\\t\\t}\\n\\t\\tcatch(Exception ex){\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\"",\""default\"":\""\""}"",
  ""name"": ""year"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0"",
  ""decimal_digits"": 0,
  ""min"": 2000,
  ""max"": 2100,
  ""step"": 1
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: account-monthly-timelog ID: bfd0dba8-dc50-4881-9815-7f5e56a6a2fb >>
			{
				var id = new Guid("bfd0dba8-dc50-4881-9815-7f5e56a6a2fb");
				Guid? parentId = new Guid("7eff5a2c-5d5d-4989-a68f-a1362b0dad7c");
				Guid? nodeId = null;
				Guid pageId = new Guid("d23be591-dbb5-4795-86e4-8adbd9aff08b");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: all ID: 8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc >>
			{
				var id = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? parentId = null;
				Guid? nodeId = null;
				Guid pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: no-owner ID: 34916453-4d5a-40a7-b74c-3c4e8b5a8950 >>
			{
				var id = new Guid("34916453-4d5a-40a7-b74c-3c4e8b5a8950");
				Guid? parentId = null;
				Guid? nodeId = null;
				Guid pageId = new Guid("db1cfef5-50a9-42ba-8f5e-34f80e6aad3c");
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

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: track-time ID: 663aa356-14d2-4e22-8dc8-a12b9fc971a1 >>
			{
				var id = new Guid("663aa356-14d2-4e22-8dc8-a12b9fc971a1");
				Guid? parentId = null;
				Guid? nodeId = null;
				Guid pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcJavaScriptBlock";
				var containerId = "";
				var options = @"{
  ""script"": ""$(function(){\n\n\tfunction RunTimer(wvTimerEl) {\n\t\tvar recordRow = $(wvTimerEl).closest(\""tr\"");\n\t\trecordRow.addClass(\""go-bkg-orange-light\"");\n\t\tvar inputTotalEl = recordRow.find(\""input[name='timelog_total_seconds']\"");\n\t\tvar totalLoggedSeconds = $(inputTotalEl).val();\n\t\tvar totalLoggedSecondsDec = new Decimal(totalLoggedSeconds);\t\t\n\t\tvar loggedHours = totalLoggedSecondsDec.div(3600).toDecimalPlaces(0,Decimal.ROUND_DOWN);\n\t\tvar totalLeft = totalLoggedSecondsDec.minus(loggedHours.times(3600));\n\t\tvar loggedMinutes = totalLeft.div(60).toDecimalPlaces(0,Decimal.ROUND_DOWN);\n\t\ttotalLeft = totalLeft.minus(loggedMinutes.times(60));\n\t\tvar loggedSeconds = totalLeft;\n\t\tvar loggedHoursString = loggedHours.toString();\n\t\tif (loggedHours.lessThan(10)) {\n\t\t\tloggedHoursString = \""0\""+loggedHoursString;\n\t\t}\n\t\tvar loggedMinutesString = loggedMinutes.toString();\n\t\tif (loggedMinutes.lessThan(10)) {\n\t\t\tloggedMinutesString = \""0\""+loggedMinutesString;\n\t\t}\n\t\tvar loggedSecondsString = loggedSeconds.toString();\n\t\tif (loggedSeconds.lessThan(10)) {\n\t\t\tloggedSecondsString = \""0\""+loggedSecondsString;\n\t\t}\n\t\trecordRow.find(\"".timer-td span\"").html(loggedHoursString + ' : ' + loggedMinutesString + ' : ' + loggedSecondsString);\n\t\trecordRow.find(\"".timer-td span\"").addClass(\""go-orange\"").removeClass(\""go-gray\"");\n\t\ttotalLoggedSecondsDec = totalLoggedSecondsDec.plus(1);\n\t\t$(inputTotalEl).val(totalLoggedSecondsDec);\n\t}\n\n\tfunction EvaluateTimer(wvTimerEl) {\n\t\tvar recordRow = $(wvTimerEl).closest(\""tr\"");\n\t\tvar inputLogStartedOn = recordRow.find(\""input[name='timelog_started_on']\"");\n\t\tif (inputLogStartedOn.val()) {\n\t\t\trecordRow.find(\"".start-log-group\"").addClass(\""d-none\"");\n\t\t\trecordRow.find(\"".stop-log-group\"").removeClass(\""d-none\"");\n\t\t\tRunTimer(wvTimerEl);\n\t\t\tsetInterval(function () {\n\t\t\t\tRunTimer(wvTimerEl);\n\t\t\t}, 1000);\n\t\t}\n\t\telse {\n\t\t\trecordRow.find(\"".start-log-group\"").removeClass(\""d-none\"");\n\t\t\trecordRow.find(\"".stop-log-group\"").addClass(\""d-none\"");\n\t\t}\n\n\t}\n\n\n    $(\"".wv-timer\"").each(function(){\n\t\tEvaluateTimer(this);\n    });\n    \n    $(\"".start-log\"").click(function(){\n        var clickedBtn = $(this);\n\t\tvar recordRow = clickedBtn.closest(\""tr\"");\n\t\tvar recordTimer = recordRow.find(\"".wv-timer\"");\n\n        var clickedBtnIcon = clickedBtn.find(\"".fa\"");\n        var clickedBtnTd = clickedBtn.closest(\""td\"");\n        var hiddenTaskInput = clickedBtnTd.find(\""input[name='task_id']\"");\n        var startLogGroup = clickedBtnTd.find(\"".start-log-group\"");\n        var stopLogGroup = clickedBtnTd.find(\"".stop-log-group\"");\n        var taskId = hiddenTaskInput.val();\n        \n        clickedBtn.prop('disabled', true);\n        clickedBtnIcon.removeClass(\""fa-play\"").addClass(\""fa-spin fa-spinner\"");\n        \n\t\t$.ajax({\n\t\ttype: \""POST\"",\n\t\turl: \""/api/v3.0/p/project/timelog/start?taskId=\""+taskId,\n\t\tdataType:\""json\"",\n\t\tsuccess: function(response){\n\t\t\tif(!response.success){\n\t\t\t\ttoastr.error(response.message, 'Error!', { closeButton: false, tapToDismiss: true });\n\t\t\t\tclickedBtn.prop('disabled', false);\n\t\t\t\tclickedBtnIcon.addClass(\""fa-play\"").removeClass(\""fa-spin fa-spinner\"");\n\t\t\t}\n\t\t\telse{\n\t\t\t\tstartLogGroup.addClass(\""d-none\"");\n\t\t\t\tstopLogGroup.removeClass(\""d-none\"");\n\t\t\t\tclickedBtn.prop('disabled', false);\n\t\t\t\tclickedBtnIcon.addClass(\""fa-play\"").removeClass(\""fa-spin fa-spinner\"");\n\t\t\t\trecordRow.find(\""input[name='timelog_started_on']\"").val(moment().toISOString());\n\t\t\t\tEvaluateTimer(recordTimer);\n\t\t\t}\n        \n\t\t},\n\t\terror:function(XMLHttpRequest, textStatus, errorThrown) {\n\t\t\ttoastr.error(textStatus, 'Error!', { closeButton: false, tapToDismiss: true });\n\t\t}\n\t\t});        \n\n    });\n\n\t$(\"".stop-log\"").click(function(){\n        var clickedBtn = $(this);\n\t\tvar recordRow = clickedBtn.closest(\""tr\"");\t\t\n\t\tvar inputTimelogStartEl = recordRow.find(\""input[name='timelog_started_on']\"");\n\t\tvar inputTaskId =  recordRow.find(\""input[name='task_id']\"");\n\t\tvar modalId = \""wv-6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4\"";\n\t\tvar formEl = $(\""#\""+modalId);\n\t\tvar minutesFormInputEl = formEl.find(\""input[name='minutes']\"");\n\t\tvar taskIdFormInputEl = formEl.find(\""input[name='task_id']\"");\n\t\tvar logStartFormInputEl = formEl.find(\""input[name='timelog_started_on']\"");\n\t\t//set minutes\n\t\tvar logstartDate = $(inputTimelogStartEl).val();\n\t\tvar totalLoggedSeconds = moment().utc().diff(logstartDate, 'seconds');\n\t\tvar totalLoggedSecondsDec = new Decimal(totalLoggedSeconds);\t\n\t\tvar totalMinutes = totalLoggedSecondsDec.div(60).toDecimalPlaces(0,Decimal.ROUND_DOWN);\n\t\tminutesFormInputEl.val(totalMinutes.toNumber());\n\t\t//set taskId\n\t\ttaskIdFormInputEl.val(inputTaskId.val());\n\t\tlogStartFormInputEl.val(moment(logstartDate).format(\""DD MMM YYYY HH:mm\""));\n\t\tlogStartFormInputEl.prop('disabled', true);\n\t\t//set logstart date\n\t\tErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:modalId,action:'open',payload:null});\n\t});\n\t$(\"".manual-log\"").click(function(){\n        var clickedBtn = $(this);\n\t\tvar recordRow = clickedBtn.closest(\""tr\"");\t\t\n\t\tvar inputTotalEl = recordRow.find(\""input[name='timelog_total_seconds']\"");\n\t\tvar inputTaskId =  recordRow.find(\""input[name='task_id']\"");\n\t\tvar formId = \""wv-6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4\"";\n\t\tvar formEl = $(\""#\""+formId);\n\t\tvar taskIdFormInputEl = formEl.find(\""input[name='task_id']\"");\n\t\t//set taskId\n\t\ttaskIdFormInputEl.val(inputTaskId.val());\n\t\tErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:formId,action:'open',payload:null});\t\t\n\t});\n});""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update data source*** Name: TaskAuxData >>
			{
				var id = new Guid("587d963b-613f-4e77-a7d4-719f631ce6b2");
				var name = @"TaskAuxData";
				var description = @"getting related data for the current task";
				var eqlText = @"SELECT $project_nn_task.id,$project_nn_task.abbr,$project_nn_task.name
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
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task""	
	-------< $project_nn_task

FROM rec_task
WHERE  ( rec_task.""id"" = @recordId ) 
) X
";
				var parametersJson = @"[{""name"":""recordId"",""type"":""guid"",""value"":""00000000-0000-0000-0000-000000000000""}]";
				var fieldsJson = @"[{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Update(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion


		}
	}
}
