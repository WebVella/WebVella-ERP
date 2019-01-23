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
		private static void Patch20190124(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{

			#region << ***Create record*** Id: 6105dcf4-4115-435f-94bb-0190d45d1b87 (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6105dcf4-4115-435f-94bb-0190d45d1b87"",
  ""is_default"": false,
  ""l_scope"": ""[\""projects\""]"",
  ""label"": ""Improvement"",
  ""sort_index"": 2.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": ""far fa-fw fa-caret-square-up"",
  ""color"": ""#9C27B0""
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a0465e9f-5d5f-433d-acf1-1da0eaec78b4 (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a0465e9f-5d5f-433d-acf1-1da0eaec78b4"",
  ""is_default"": true,
  ""l_scope"": ""[\""projects\""]"",
  ""label"": ""New Feature"",
  ""sort_index"": 1.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": ""fas fa-fw fa-plus-square"",
  ""color"": ""#4CAF50""
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c0a2554c-f59a-434e-be00-217a416f8efd (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c0a2554c-f59a-434e-be00-217a416f8efd"",
  ""is_default"": false,
  ""l_scope"": ""[\""projects\""]"",
  ""label"": ""Bug"",
  ""sort_index"": 3.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": ""fas fa-fw fa-bug"",
  ""color"": ""#F44336""
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion


		}
	}
}
