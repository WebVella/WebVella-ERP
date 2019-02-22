using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;

namespace WebVella.Erp.Plugins.Next
{
	public partial class NextPlugin : ErpPlugin
	{
		private static void Patch20190222(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Update record*** Id: a0465e9f-5d5f-433d-acf1-1da0eaec78b4 (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a0465e9f-5d5f-433d-acf1-1da0eaec78b4"",
  ""is_default"": true,
  ""label"": ""New Feature"",
  ""sort_index"": 6.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": ""fas fa-fw fa-plus-square"",
  ""color"": ""#4CAF50"",
  ""l_scope"": ""[\""projects\""]""
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.UpdateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Update record*** Id: 6105dcf4-4115-435f-94bb-0190d45d1b87 (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6105dcf4-4115-435f-94bb-0190d45d1b87"",
  ""is_default"": false,
  ""label"": ""Improvement"",
  ""sort_index"": 7.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": ""far fa-fw fa-caret-square-up"",
  ""color"": ""#9C27B0"",
  ""l_scope"": ""[\""projects\""]""
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.UpdateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Update record*** Id: c0a2554c-f59a-434e-be00-217a416f8efd (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c0a2554c-f59a-434e-be00-217a416f8efd"",
  ""is_default"": false,
  ""label"": ""Bug"",
  ""sort_index"": 8.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": ""fas fa-fw fa-bug"",
  ""color"": ""#F44336"",
  ""l_scope"": ""[\""projects\""]""
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.UpdateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Update record*** Id: da9bf72d-3655-4c51-9f99-047ef9297bf2 (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""da9bf72d-3655-4c51-9f99-047ef9297bf2"",
  ""is_default"": true,
  ""label"": ""General"",
  ""sort_index"": 1.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": ""fa fa-cog"",
  ""color"": ""#2196F3"",
  ""l_scope"": ""[\""projects\""]""
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.UpdateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Update record*** Id: 7b191135-5fbb-4db9-bf24-1a5fc72d8cd5 (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7b191135-5fbb-4db9-bf24-1a5fc72d8cd5"",
  ""is_default"": false,
  ""label"": ""Call"",
  ""sort_index"": 2.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": ""fas fa-phone"",
  ""color"": ""#2196F3"",
  ""l_scope"": ""[\""projects\""]""
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.UpdateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Update record*** Id: 894ba1ef-1b31-440c-9b33-f301d047d8fb (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""894ba1ef-1b31-440c-9b33-f301d047d8fb"",
  ""is_default"": false,
  ""label"": ""Meeting"",
  ""sort_index"": 4.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": ""fas fa-users"",
  ""color"": ""#2196F3"",
  ""l_scope"": ""[\""projects\""]""
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.UpdateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Update record*** Id: 489b16e1-91b1-4a05-b247-50ed74f7aaaf (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""489b16e1-91b1-4a05-b247-50ed74f7aaaf"",
  ""is_default"": false,
  ""label"": ""Email"",
  ""sort_index"": 3.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": ""fa fa-envelope"",
  ""color"": ""#2196F3"",
  ""l_scope"": ""[\""projects\""]""
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.UpdateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion



		}
	}
}
