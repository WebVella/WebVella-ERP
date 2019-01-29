using Newtonsoft.Json;
using System;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Plugins.Next
{
	public partial class NextPlugin : ErpPlugin
	{
		private static void Patch20190128(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Create record*** Id: a1e527fd-4472-4b39-a1d4-af4905d2310c (task_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a1e527fd-4472-4b39-a1d4-af4905d2310c"",
  ""is_closed"": true,
  ""is_default"": false,
  ""l_scope"": """",
  ""label"": ""Rejected"",
  ""sort_index"": 5.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": """",
  ""color"": """"
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("task_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion



		}
	}
}
