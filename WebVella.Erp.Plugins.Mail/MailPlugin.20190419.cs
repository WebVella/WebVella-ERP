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
		private static void Patch20190419(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Create field***  Entity: email Field Name: sender >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("8f59eaa9-873e-4461-83fb-34ecbbc88e7c");
				textboxField.Name = "sender";
				textboxField.Label = "Sender";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = true;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "[]";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: sender Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: recipients >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("ab748700-d13b-4df4-917e-093d74879a8e");
				textboxField.Name = "recipients";
				textboxField.Label = "Recipients";
				textboxField.PlaceholderText = "[]";
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = true;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: recipients Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: email Field Name: x_search >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("085e2442-820a-4df7-ab92-516ce23197c4")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "x_search").Id;
				textboxField.Name = "x_search";
				textboxField.Label = "x_search";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = false;
				textboxField.DefaultValue = "";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.UpdateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: x_search Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update app*** App name: mail >>
			{
				var id = new Guid("9d3b5497-e136-43b7-ad87-857e615a54c9");
				var name = "mail";
				var label = "Mail";
				var description = "Provides services for sending emails.";
				var iconClass = "far fa-envelope";
				var author = "WebVella";
				var color = "#8bc34a";
				var weight = 10;
				var access = new List<Guid>();
				access.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));

				new WebVella.Erp.Web.Services.AppService().UpdateApplication(id, name, label, description, iconClass, author, color, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap area*** Sitemap area name: objects >>
			{
				var id = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
				var appId = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
				var name = "objects";
				var label = "Objects";
				var description = @"Schema and Layout management";
				var iconClass = "fa fa-pencil-ruler";
				var color = "#2196F3";
				var weight = 1;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap area*** Sitemap area name: access >>
			{
				var id = new Guid("c5c4cefc-1402-4a8b-9867-7f2a059b745d");
				var appId = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
				var name = "access";
				var label = "Access";
				var description = @"Manage users and roles";
				var iconClass = "fa fa-key";
				var color = "#673AB7";
				var weight = 2;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap area*** Sitemap area name: server >>
			{
				var id = new Guid("fee72214-f1c4-4ed5-8bda-35698dc11528");
				var appId = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
				var name = "server";
				var label = "Server";
				var description = @"Background jobs and maintenance";
				var iconClass = "fa fa-database";
				var color = "#F44336";
				var weight = 3;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: page >>
			{
				var id = new Guid("5b132ac0-703e-4342-a13d-c7ff93d07a4f");
				var areaId = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
				Guid? entityId = null;
				var name = "page";
				var label = "Pages";
				var url = "/sdk/objects/page/l";
				var iconClass = "fa fa-file";
				var weight = 1;
				var type = ((int)3);
				var access = new List<Guid>();
				var entityListPages = new List<Guid>();
				var entityCreatePages = new List<Guid>();
				var entityDetailsPages = new List<Guid>();
				var entityManagePages = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, entityListPages, entityCreatePages, entityDetailsPages, entityManagePages, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: data_source >>
			{
				var id = new Guid("9b30bf96-67d9-4d20-bf07-e6ef1c44d553");
				var areaId = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
				Guid? entityId = null;
				var name = "data_source";
				var label = "Data sources";
				var url = "/sdk/objects/data_source/l/list";
				var iconClass = "fa fa-cloud-download-alt";
				var weight = 2;
				var type = ((int)3);
				var access = new List<Guid>();
				var entityListPages = new List<Guid>();
				var entityCreatePages = new List<Guid>();
				var entityDetailsPages = new List<Guid>();
				var entityManagePages = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, entityListPages, entityCreatePages, entityDetailsPages, entityManagePages, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: application >>
			{
				var id = new Guid("02d75ea5-8fc6-4f95-9933-0eed6b36ca49");
				var areaId = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
				Guid? entityId = null;
				var name = "application";
				var label = "Applications";
				var url = "/sdk/objects/application/l/list";
				var iconClass = "fa fa-th";
				var weight = 3;
				var type = ((int)3);
				var access = new List<Guid>();
				var entityListPages = new List<Guid>();
				var entityCreatePages = new List<Guid>();
				var entityDetailsPages = new List<Guid>();
				var entityManagePages = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, entityListPages, entityCreatePages, entityDetailsPages, entityManagePages, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: entity >>
			{
				var id = new Guid("dfa7ec55-b55b-404f-b251-889f1d81df29");
				var areaId = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
				Guid? entityId = null;
				var name = "entity";
				var label = "Entities";
				var url = "/sdk/objects/entity/l";
				var iconClass = "fa fa-database";
				var weight = 4;
				var type = ((int)3);
				var access = new List<Guid>();
				var entityListPages = new List<Guid>();
				var entityCreatePages = new List<Guid>();
				var entityDetailsPages = new List<Guid>();
				var entityManagePages = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, entityListPages, entityCreatePages, entityDetailsPages, entityManagePages, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: user >>
			{
				var id = new Guid("ff578868-817e-433d-988f-bb8d4e9baa0d");
				var areaId = new Guid("c5c4cefc-1402-4a8b-9867-7f2a059b745d");
				Guid? entityId = null;
				var name = "user";
				var label = "Users";
				var url = "/sdk/access/user/l/list";
				var iconClass = "fa fa-user";
				var weight = 1;
				var type = ((int)3);
				var access = new List<Guid>();
				var entityListPages = new List<Guid>();
				var entityCreatePages = new List<Guid>();
				var entityDetailsPages = new List<Guid>();
				var entityManagePages = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, entityListPages, entityCreatePages, entityDetailsPages, entityManagePages, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: job >>
			{
				var id = new Guid("396ec481-3b2e-461c-b514-743fb3252003");
				var areaId = new Guid("fee72214-f1c4-4ed5-8bda-35698dc11528");
				Guid? entityId = null;
				var name = "job";
				var label = "Background jobs";
				var url = "/sdk/server/job/l/plan";
				var iconClass = "fa fa-cogs";
				var weight = 1;
				var type = ((int)3);
				var access = new List<Guid>();
				var entityListPages = new List<Guid>();
				var entityCreatePages = new List<Guid>();
				var entityDetailsPages = new List<Guid>();
				var entityManagePages = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, entityListPages, entityCreatePages, entityDetailsPages, entityManagePages, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap node*** Sitemap node name: log >>
			{
				var id = new Guid("78a29ac8-d2aa-4379-b990-08f7f164a895");
				var areaId = new Guid("fee72214-f1c4-4ed5-8bda-35698dc11528");
				Guid? entityId = null;
				var name = "log";
				var label = "Logs";
				var url = "/sdk/server/log/l/list";
				var iconClass = "fas fa-sticky-note";
				var weight = 2;
				var type = ((int)3);
				var access = new List<Guid>();
				var entityListPages = new List<Guid>();
				var entityCreatePages = new List<Guid>();
				var entityDetailsPages = new List<Guid>();
				var entityManagePages = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, entityListPages, entityCreatePages, entityDetailsPages, entityManagePages, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update page body node*** Page: all_emails ID: d46a084d-4390-4134-80f4-c2fa4ceba811 >>
			{
				var id = new Guid("d46a084d-4390-4134-80f4-c2fa4ceba811");
				Guid? parentId = new Guid("5dfef806-4448-4bce-8a5d-91e8587cbe33");
				Guid? nodeId = null;
				Guid pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "column1";
				var options = @"{
  ""type"": ""2"",
  ""text"": """",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-eye"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_visible"": """",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class ViewEmailUrlCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar queryRec = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RequestQuery\\\"");\\n\\t\\tvar recordId = pageModel.TryGetDataSourceProperty<Guid>(\\\""RowRecord.id\\\"");\\n\\n\\t\\tif (recordId == null)\\n\\t\\t\\treturn null;\\n\\n        string queryString = string.Empty;\\n        if(queryRec != null)\\n        {\\n            foreach(var key in queryRec.Properties.Keys )\\n            {\\n                queryString +=  $\\\""{key}={queryRec[key]}&\\\"";\\n            }\\n        }\\n        \\n        var returnUrl = System.Net.WebUtility.UrlEncode( $\\\""/mail/emails/all/l?{queryString}\\\"" );\\n\\t\\treturn $\\\""/mail/emails/all/r/{recordId}/details?returnUrl={returnUrl}\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			FixSenderAndRecipients(recMan);

			#region << ***Delete field*** Entity: email Field Name: recipient_name >>
			{
				{
					var response = entMan.DeleteField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), new Guid("a3015639-7fd9-4231-89e3-76a7a133dd6d"));
					if (!response.Success)
						throw new Exception("System error 10060. Delete field failed for Field: recipient_name Entity: email. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Delete field*** Entity: email Field Name: sender_name >>
			{
				{
					var response = entMan.DeleteField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), new Guid("4d9e646c-0105-4370-ad21-d6547a7cabb1"));
					if (!response.Success)
						throw new Exception("System error 10060. Delete field failed for Field: sender_name Entity: email. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Delete field*** Entity: email Field Name: recipient_email >>
			{
				{
					var response = entMan.DeleteField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), new Guid("cae76d3b-bf91-47bc-aec0-d7ac26eced7b"));
					if (!response.Success)
						throw new Exception("System error 10060. Delete field failed for Field: recipient_email Entity: email. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Delete field*** Entity: email Field Name: sender_email >>
			{
				{
					var response = entMan.DeleteField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), new Guid("94845377-b845-49fe-b693-789f1ed5740e"));
					if (!response.Success)
						throw new Exception("System error 10060. Delete field failed for Field: sender_email Entity: email. Message:" + response.Message);
				}
			}
			#endregion
		}

		private static void FixSenderAndRecipients(RecordManager recMan)
		{
			var emails = new EqlCommand("SELECT * FROM email").Execute();
			foreach (var email in emails)
			{
				email["sender"] = JsonConvert.SerializeObject(new EmailAddress { Name = (string)email["sender_name"], Address = (string)email["sender_email"] });
				email["recipients"] = JsonConvert.SerializeObject(new List<EmailAddress> { new EmailAddress { Name = (string)email["recipient_name"], Address = (string)email["recipient_email"] } });
				var response = recMan.UpdateRecord("email", email);
				if (!response.Success)
					throw new Exception(response.Message);
			}

		}
	}
}
