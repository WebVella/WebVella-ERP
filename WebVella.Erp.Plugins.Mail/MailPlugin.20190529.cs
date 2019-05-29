using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Plugins.Mail
{
	public partial class MailPlugin : ErpPlugin
	{
		private static void Patch20190529(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Create field***  Entity: email Field Name: attachments >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("3e24f113-0236-4474-b6ed-adf0f29c052f");
				textboxField.Name = "attachments";
				textboxField.Label = "Attachments";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
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
						throw new Exception("System error 10060. Entity: email Field: attachments Message:" + response.Message);
				}
			}
			#endregion
		}
	}
}
