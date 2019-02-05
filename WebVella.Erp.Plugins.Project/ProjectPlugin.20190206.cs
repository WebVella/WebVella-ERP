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
		private static void Patch20190206(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Delete page body node*** Page name: details ID: 291477ec-dd9c-4fc3-97a0-d2fd62809b2f >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("291477ec-dd9c-4fc3-97a0-d2fd62809b2f"), WebVella.Erp.Database.DbContext.Current.Transaction, cascade: false);
			}
			#endregion

			#region << ***Delete page body node*** Page name: details ID: 798e39b3-7a36-406b-bed6-e77da68fc50f >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("798e39b3-7a36-406b-bed6-e77da68fc50f"), WebVella.Erp.Database.DbContext.Current.Transaction, cascade: false);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 151d5da3-161a-44c0-97fa-84c76c9d3b60 >>
			{
				var id = new Guid("151d5da3-161a-44c0-97fa-84c76c9d3b60");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
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

			#region << ***Create page body node*** Page name: details  id: caa34ee6-0be6-48eb-b6bd-8b9f1ef83009 >>
			{
				var id = new Guid("caa34ee6-0be6-48eb-b6bd-8b9f1ef83009");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.end_time\"",\""default\"":\""\""}"",
  ""name"": ""end_time"",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
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
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 526c7435-9ace-4032-b754-5d2e9c817436 >>
			{
				var id = new Guid("526c7435-9ace-4032-b754-5d2e9c817436");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
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
				var weight = 5;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 97402edb-3a5a-4cc3-bc40-4d4d012619e2 >>
			{
				var id = new Guid("97402edb-3a5a-4cc3-bc40-4d4d012619e2");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcModal";
				var containerId = "body";
				var options = @"{
  ""title"": ""Task recurrence setting"",
  ""backdrop"": ""true"",
  ""size"": ""2"",
  ""position"": ""0""
}";
				var weight = 6;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion



		}
	}
}
