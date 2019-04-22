using System;
using WebVella.Erp.Api;

namespace WebVella.Erp.Plugins.Mail
{
	public partial class MailPlugin : ErpPlugin
	{
		private static void Patch20190422(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Update page body node*** Page: details ID: 314c56d3-4e85-4e35-8af7-5f909750139e >>
			{
				var id = new Guid("314c56d3-4e85-4e35-8af7-5f909750139e");
				Guid? parentId = new Guid("fb0885f1-9377-437b-8851-3c97c25a9b5b");
				Guid? nodeId = null;
				Guid pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""is_visible"": """",
  ""label_mode"": ""0"",
  ""label_text"": ""Service"",
  ""mode"": ""2"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing WebVella.Erp.Plugins.Mail.Api;\\n\\npublic class ViewEmailUrlCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar serviceId = pageModel.TryGetDataSourceProperty<Guid>(\\\""Record.service_id\\\"");\\n\\n\\t\\tif (serviceId == null)\\n\\t\\t\\treturn \\\""SMTP service not found\\\"";\\n\\t\\t\\t\\n\\t\\tEmailServiceManager serviceManager = new EmailServiceManager();\\n\\t\\tvar smtpService = serviceManager.GetSmtpService(serviceId);\\n\\t\\tif( smtpService == null )\\n\\t\\t    return \\\""SMTP service not found\\\"";\\n\\t\\t    \\n\\t\\treturn smtpService.Name;\\n\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""service_id"",
  ""class"": """",
  ""maxlength"": 0,
  ""placeholder"": """",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: details ID: 4deda1c4-e4bd-474b-b9cd-71c7d27f5891 >>
			{
				var id = new Guid("4deda1c4-e4bd-474b-b9cd-71c7d27f5891");
				Guid? parentId = new Guid("e04237dc-3454-4309-b183-09e83f9bd37c");
				Guid? nodeId = null;
				Guid pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""is_visible"": """",
  ""label_mode"": ""0"",
  ""label_text"": ""Recipient"",
  ""mode"": ""2"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Linq;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing Newtonsoft.Json;\\nusing WebVella.Erp.Plugins.Mail.Api;\\n\\npublic class CompositeSenderEmailVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar record = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""Record\\\"");\\n\\t\\tvar recipients = JsonConvert.DeserializeObject<List<EmailAddress>>( (string) record[\\\""recipients\\\""] );\\n\\t\\treturn string.Join( \\\"";\\\"", recipients.Select( x=> x.Address ).ToList() );\\n\\t}\\n}\"",\""default\"":\""\""}"",
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

			#region << ***Update page body node*** Page: details ID: 3b5fcbef-27e3-44c3-ad38-1202cccab408 >>
			{
				var id = new Guid("3b5fcbef-27e3-44c3-ad38-1202cccab408");
				Guid? parentId = new Guid("e04237dc-3454-4309-b183-09e83f9bd37c");
				Guid? nodeId = null;
				Guid pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""is_visible"": """",
  ""label_mode"": ""0"",
  ""label_text"": ""Sender"",
  ""mode"": ""2"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Linq;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing Newtonsoft.Json;\\nusing WebVella.Erp.Plugins.Mail.Api;\\n\\npublic class CompositeSenderEmailVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar record = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""Record\\\"");\\n\\t\\tvar sender = JsonConvert.DeserializeObject<EmailAddress>( (string) record[\\\""sender\\\""] );\\n\\t\\treturn sender.Address;\\n\\t}\\n}\"",\""default\"":\""\""}"",
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

		}
	}
}
