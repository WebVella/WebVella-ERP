using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Plugins.Mail.Api;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.Mail
{
	public partial class MailPlugin : ErpPlugin
	{
		private static void Patch20190420(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Update page body node*** Page: all_emails ID: 60a62365-f290-421e-af82-30bbd6474ea9 >>
			{
				var id = new Guid("60a62365-f290-421e-af82-30bbd6474ea9");
				Guid? parentId = new Guid("5dfef806-4448-4bce-8a5d-91e8587cbe33");
				Guid? nodeId = null;
				Guid pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column3";
				var options = @"{
  ""is_visible"": """",
  ""label_mode"": ""3"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing Newtonsoft.Json;\\nusing WebVella.Erp.Plugins.Mail.Api;\\n\\npublic class GetDatasourceValueCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry{\\n\\t\\t\\tconst string DATASOURCE_NAME = \\\""RowRecord\\\"";\\n\\t\\t\\tif (pageModel == null)\\n\\t\\t\\t\\treturn null;\\n\\t\\n\\t\\t\\tvar rowRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(DATASOURCE_NAME);\\n\\t\\t\\tif ( rowRecord  == null)\\n\\t\\t\\t\\treturn null;\\n\\t\\n\\t\\t\\tvar mailAddress = JsonConvert.DeserializeObject<EmailAddress>( (string) rowRecord[\\\""sender\\\""] );\\n\\t\\t\\treturn mailAddress.Address;\\n\\t\\t}\\n\\t\\tcatch(Exception ex)\\n\\t\\t{\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""sender_email"",
  ""class"": """",
  ""maxlength"": 0,
  ""placeholder"": """",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: all_emails ID: 7be29cb8-8449-451a-b6de-3bba406f1b48 >>
			{
				var id = new Guid("7be29cb8-8449-451a-b6de-3bba406f1b48");
				Guid? parentId = new Guid("5dfef806-4448-4bce-8a5d-91e8587cbe33");
				Guid? nodeId = null;
				Guid pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""is_visible"": """",
  ""label_mode"": ""3"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Linq;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing Newtonsoft.Json;\\nusing WebVella.Erp.Plugins.Mail.Api;\\n\\npublic class GetDatasourceValueCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry{\\n\\t\\t\\tconst string DATASOURCE_NAME = \\\""RowRecord\\\"";\\n\\t\\t\\tif (pageModel == null)\\n\\t\\t\\t\\treturn null;\\n\\t\\n\\t\\t\\tvar rowRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(DATASOURCE_NAME);\\n\\t\\t\\tif ( rowRecord  == null)\\n\\t\\t\\t\\treturn null;\\n\\t\\n\\t\\t\\tvar recipients = JsonConvert.DeserializeObject<List<EmailAddress>>( (string) rowRecord[\\\""recipients\\\""] );\\n\\t\\t\\treturn string.Join( \\\"";\\\"", recipients.Select( x=> x.Address ).ToList() );\\n\\t\\t}\\n\\t\\tcatch(Exception ex)\\n\\t\\t{\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""recipient_email"",
  ""class"": """",
  ""maxlength"": 0,
  ""placeholder"": """",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

		}
	}
}
