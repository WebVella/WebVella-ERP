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
		private static void Patch20190207(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{

			#region << ***Delete page body node*** Page name: create ID: ecef4b2c-6988-44c1-acea-0e28385ec528 >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("ecef4b2c-6988-44c1-acea-0e28385ec528"), WebVella.Erp.Database.DbContext.Current.Transaction, cascade: false);
			}
			#endregion

			#region << ***Delete page body node*** Page name: create ID: 884a8db1-aff0-4f86-ab7d-8fb17698fc33 >>
			{

				new WebVella.Erp.Web.Services.PageService().DeletePageBodyNode(new Guid("884a8db1-aff0-4f86-ab7d-8fb17698fc33"), WebVella.Erp.Database.DbContext.Current.Transaction, cascade: false);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: d07d36ac-2536-4cf8-9cfc-b07eaa7a1320 >>
			{
				var id = new Guid("d07d36ac-2536-4cf8-9cfc-b07eaa7a1320");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.start_time\"",\""default\"":\""\""}"",
  ""name"": ""start_time"",
  ""mode"": ""0"",
  ""connected_entity_id"": """"
}";
				var weight = 10;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: db2d036e-df04-4514-9533-2ac31ade4602 >>
			{
				var id = new Guid("db2d036e-df04-4514-9533-2ac31ade4602");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.end_time\"",\""default\"":\""\""}"",
  ""name"": ""end_time"",
  ""mode"": ""0"",
  ""connected_entity_id"": """"
}";
				var weight = 11;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion



		}
	}
}
