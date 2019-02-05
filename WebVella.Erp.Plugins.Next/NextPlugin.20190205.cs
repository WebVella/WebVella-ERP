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
		private static void Patch20190205(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Update field***  Entity: timelog Field Name: minutes >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("750153c5-1df9-408f-b856-727078a525bc")).Object;
				InputNumberField numberField = new InputNumberField();
				numberField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "minutes").Id;
				numberField.Name = "minutes";
				numberField.Label = "Minutes";
				numberField.PlaceholderText = null;
				numberField.Description = "0 will not create timelog";
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("0.0");
				numberField.MinValue = null;
				numberField.MaxValue = null;
				numberField.DecimalPlaces = byte.Parse("0");
				numberField.EnableSecurity = false;
				numberField.Permissions = new FieldPermissions();
				numberField.Permissions.CanRead = new List<Guid>();
				numberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.UpdateField(new Guid("750153c5-1df9-408f-b856-727078a525bc"), numberField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: timelog Field: minutes Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: recurrence_template >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("9973acd9-86eb-41de-8c93-295b17876adb");
				textboxField.Name = "recurrence_template";
				textboxField.Label = "Recurrence Template";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = false;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = null;
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: recurrence_template Message:" + response.Message);
				}
			}
			#endregion


		}
	}
}
