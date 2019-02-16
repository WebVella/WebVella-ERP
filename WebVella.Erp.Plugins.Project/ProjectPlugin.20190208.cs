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
		private static void Patch20190208(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{

			#region << ***Update page body node*** Page: track-time ID: c57d94a6-9c90-4071-b54b-2c05b79aa522 >>
			{
				var id = new Guid("c57d94a6-9c90-4071-b54b-2c05b79aa522");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				Guid pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column2";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry{\\n\\t\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\t\\tif (pageModel == null)\\n\\t\\t\\t\\treturn null;\\n\\t\\n\\t\\t\\t//try read data source by name and get result as specified type object\\n\\t\\t\\tvar dataSource = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord\\\"");\\n\\t\\n\\t\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\t\\tif (dataSource == null)\\n\\t\\t\\t\\treturn null;\\n\\t        var loggedSeconds = ((int)dataSource[\\\""logged_minutes\\\""])*60;\\n\\t        var logStartedOn = (DateTime?)dataSource[\\\""timelog_started_on\\\""];\\n\\t        var logStartString = \\\""\\\"";\\n\\t        if(logStartedOn != null){\\n\\t            //loggedSeconds = loggedSeconds + (int)((DateTime.UtcNow - logStartedOn.Value).TotalSeconds);\\n\\t            logStartString = logStartedOn.Value.ToString(\\\""o\\\"");\\n\\t        }\\n\\n\\t        var hours = (int)(loggedSeconds/3600);\\n\\t        var loggedSecondsLeft = loggedSeconds - hours*3600;\\n\\t        var hoursString = \\\""00\\\"";\\n\\t        if(hours < 10)\\n\\t            hoursString = \\\""0\\\"" + hours;\\n            else\\n                hoursString = hours.ToString();\\n\\t            \\n\\t        var minutes = (int)(loggedSecondsLeft/60);\\n\\t        var minutesString = \\\""00\\\"";\\n\\t        if(minutes < 10)\\n\\t            minutesString = \\\""0\\\"" + minutes;\\n            else\\n                minutesString = minutes.ToString();\\t        \\n                \\n            var seconds =  loggedSecondsLeft -  minutes*60;\\n\\t        var secondsString = \\\""00\\\"";\\n\\t        if(seconds < 10)\\n\\t            secondsString = \\\""0\\\"" + seconds;\\n            else\\n                secondsString = seconds.ToString();\\t                    \\n            \\n            var result = $\\\""<span class='go-gray wv-timer' style='font-size:16px;'>{hoursString + \\\"" : \\\"" + minutesString + \\\"" : \\\"" + secondsString}</span>\\\\n\\\"";\\n            result += $\\\""<input type='hidden' name='timelog_total_seconds' value='{loggedSeconds}'/>\\\\n\\\"";\\n            result += $\\\""<input type='hidden' name='timelog_started_on' value='{logStartString}'/>\\\"";\\n            return result;\\n\\t\\t}\\n\\t\\tcatch(Exception ex){\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\\n\"",\""default\"":\""<span class=\\\""go-gray\\\"" style='font-size:16px;'>00 : 00 : 00</span>\""}"",
  ""name"": ""field"",
  ""class"": """",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion




		}
	}
}
