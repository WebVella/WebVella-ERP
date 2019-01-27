using Newtonsoft.Json;
using System;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Plugins.Next
{
	public partial class NextPlugin : ErpPlugin
	{
		private static void Patch20190127(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Update record*** Id: b1cc69e5-ce09-40e0-8785-b6452b257bdf (task_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b1cc69e5-ce09-40e0-8785-b6452b257bdf"",
  ""is_closed"": true,
  ""is_default"": false,
  ""l_scope"": """",
  ""label"": ""Completed"",
  ""sort_index"": 4.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": """",
  ""color"": """"
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.UpdateRecord("task_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Update record*** Id: 8b2aa2af-17dd-400a-a221-78ee744c4866 (task_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""8b2aa2af-17dd-400a-a221-78ee744c4866"",
  ""is_closed"": false,
  ""is_default"": false,
  ""l_scope"": """",
  ""label"": ""Blocked"",
  ""sort_index"": 3.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": """",
  ""color"": """"
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.UpdateRecord("task_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion


		}
	}
}
