using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.Next
{
	public partial class NextPlugin : ErpPlugin
	{
		private static void Patch20190117(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Create entity*** Entity name: timelog >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("829b8572-0084-40e2-b589-c4e8dc7cbbd7");
					entity.Id = new Guid("750153c5-1df9-408f-b856-727078a525bc");
					entity.Name = "timelog";
					entity.Label = "Timelog";
					entity.LabelPlural = "Timelogs";
					entity.System = true;
					entity.IconName = "far fa-clock";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = null;
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					entity.RecordPermissions.CanDelete.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: timelog creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: timelog Field Name: body >>
			{
				InputMultiLineTextField textareaField = new InputMultiLineTextField();
				textareaField.Id = new Guid("07fdec6b-5442-47c8-aa66-76e506932af1");
				textareaField.Name = "body";
				textareaField.Label = "Body";
				textareaField.PlaceholderText = null;
				textareaField.Description = null;
				textareaField.HelpText = null;
				textareaField.Required = false;
				textareaField.Unique = false;
				textareaField.Searchable = false;
				textareaField.Auditable = false;
				textareaField.System = true;
				textareaField.DefaultValue = null;
				textareaField.MaxLength = null;
				textareaField.VisibleLineNumber = null;
				textareaField.EnableSecurity = false;
				textareaField.Permissions = new FieldPermissions();
				textareaField.Permissions.CanRead = new List<Guid>();
				textareaField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("750153c5-1df9-408f-b856-727078a525bc"), textareaField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: timelog Field: body Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: timelog Field Name: created_by >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("94f7e796-f78b-4991-b50f-0fa17ad4b58a");
				guidField.Name = "created_by";
				guidField.Label = "Created by";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("750153c5-1df9-408f-b856-727078a525bc"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: timelog Field: created_by Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: timelog Field Name: created_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("a9c86907-a2d5-4bcb-92dd-e5de409e3754");
				datetimeField.Name = "created_on";
				datetimeField.Label = "Created on";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = true;
				datetimeField.Unique = false;
				datetimeField.Searchable = true;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = true;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("750153c5-1df9-408f-b856-727078a525bc"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: timelog Field: created_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: timelog Field Name: is_billable >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("d62fcab9-24c8-473b-894f-a3b73e7f60ac");
				checkboxField.Name = "is_billable";
				checkboxField.Label = "Is Billable";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = true;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("750153c5-1df9-408f-b856-727078a525bc"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: timelog Field: is_billable Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: timelog Field Name: l_related_records >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("9a1b7025-2037-472e-983f-a57444dc44da");
				textboxField.Name = "l_related_records";
				textboxField.Label = "Related records";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = false;
				textboxField.Unique = false;
				textboxField.Searchable = true;
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
					var response = entMan.CreateField(new Guid("750153c5-1df9-408f-b856-727078a525bc"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: timelog Field: l_related_records Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: timelog Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("8cc443ae-d45c-4186-a7f7-fc20ce5fc31c");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = false;
				textboxField.Unique = false;
				textboxField.Searchable = true;
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
					var response = entMan.CreateField(new Guid("750153c5-1df9-408f-b856-727078a525bc"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: timelog Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: timelog Field Name: minutes >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("879b14b7-bd32-4f6c-b3a0-9b2e3a1cdc3a");
				numberField.Name = "minutes";
				numberField.Label = "Minutes";
				numberField.PlaceholderText = null;
				numberField.Description = null;
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
					var response = entMan.CreateField(new Guid("750153c5-1df9-408f-b856-727078a525bc"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: timelog Field: minutes Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: timelog Field Name: logged_on >>
			{
				InputDateField dateField = new InputDateField();
				dateField.Id = new Guid("480363dd-e296-4572-8be5-618c32388ba3");
				dateField.Name = "logged_on";
				dateField.Label = "Logged on";
				dateField.PlaceholderText = null;
				dateField.Description = null;
				dateField.HelpText = null;
				dateField.Required = true;
				dateField.Unique = false;
				dateField.Searchable = true;
				dateField.Auditable = false;
				dateField.System = true;
				dateField.DefaultValue = null;
				dateField.Format = "yyyy-MMM-dd";
				dateField.UseCurrentTimeAsDefaultValue = true;
				dateField.EnableSecurity = false;
				dateField.Permissions = new FieldPermissions();
				dateField.Permissions.CanRead = new List<Guid>();
				dateField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("750153c5-1df9-408f-b856-727078a525bc"), dateField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: timelog Field: logged_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: user_file Field Name: type >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("5c666c54-9e76-4327-ac7a-55851037810c")).Object;
				InputSelectField dropdownField = new InputSelectField();
				dropdownField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "type").Id;
				dropdownField.Name = "type";
				dropdownField.Label = "Type";
				dropdownField.PlaceholderText = null;
				dropdownField.Description = null;
				dropdownField.HelpText = null;
				dropdownField.Required = true;
				dropdownField.Unique = false;
				dropdownField.Searchable = true;
				dropdownField.Auditable = false;
				dropdownField.System = true;
				dropdownField.DefaultValue = "image";
				dropdownField.Options = new List<SelectOption>
	{
		new SelectOption() { Label = "image", Value = "image" , IconClass = "", Color = ""},
		new SelectOption() { Label = "document", Value = "document" , IconClass = "", Color = ""},
		new SelectOption() { Label = "audio", Value = "audio" , IconClass = "", Color = ""},
		new SelectOption() { Label = "video", Value = "video" , IconClass = "", Color = ""},
		new SelectOption() { Label = "other", Value = "other" , IconClass = "", Color = ""}
	};
				dropdownField.EnableSecurity = false;
				dropdownField.Permissions = new FieldPermissions();
				dropdownField.Permissions.CanRead = new List<Guid>();
				dropdownField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.UpdateField(new Guid("5c666c54-9e76-4327-ac7a-55851037810c"), dropdownField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: user_file Field: type Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: comment >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("7ffcc5b7-e347-4923-af5f-e368549d7f16");
					entity.Id = new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d");
					entity.Name = "comment";
					entity.Label = "Comment";
					entity.LabelPlural = "Comments";
					entity.System = true;
					entity.IconName = "far fa-comment";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = null;
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					entity.RecordPermissions.CanDelete.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: comment creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: comment Field Name: body >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("0a4195d1-aa37-4aea-9c56-52e8d22d6f13");
				textboxField.Name = "body";
				textboxField.Label = "Body";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "body";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: comment Field: body Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: comment Field Name: created_by >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("8b2d1f1c-bcdd-4c1d-94df-884205c2bf9c");
				guidField.Name = "created_by";
				guidField.Label = "Created by";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: comment Field: created_by Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: comment Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("28ea1822-6030-4b63-9532-d7f846105a11");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = false;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = false;
				textboxField.DefaultValue = null;
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: comment Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: comment Field Name: parent_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("4629bdb5-9a79-4c87-b764-74491b5b2cfa");
				guidField.Name = "parent_id";
				guidField.Label = "Parent id";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = false;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = null;
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: comment Field: parent_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: comment Field Name: l_related_records >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("364b886e-850a-438d-8e8f-4a6719272bfc");
				textboxField.Name = "l_related_records";
				textboxField.Label = "Related Record lookup";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = false;
				textboxField.Unique = false;
				textboxField.Searchable = true;
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
					var response = entMan.CreateField(new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: comment Field: l_related_records Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: comment Field Name: created_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("4c61dbe0-04a6-4bca-b4cd-40689bd232f1");
				datetimeField.Name = "created_on";
				datetimeField.Label = "Created On";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = true;
				datetimeField.Unique = false;
				datetimeField.Searchable = true;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = true;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: comment Field: created_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: attachment >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("d7f3e637-b514-4090-b605-7884d05bcc2d");
					entity.Id = new Guid("4b56686e-971e-4b8e-8356-642a8f341bff");
					entity.Name = "attachment";
					entity.Label = "Attachment";
					entity.LabelPlural = "Attachments";
					entity.System = true;
					entity.IconName = "fas fa-paperclip";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = new Guid("d7f3e637-b514-4090-b605-7884d05bcc2d");
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: attachment creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: attachment Field Name: created_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("489b7326-4da8-4c46-90c8-dc882a626ae0");
				datetimeField.Name = "created_on";
				datetimeField.Label = "Created on";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = true;
				datetimeField.Unique = false;
				datetimeField.Searchable = true;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = true;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("4b56686e-971e-4b8e-8356-642a8f341bff"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: attachment Field: created_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: attachment Field Name: created_by >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("3b152606-bf9e-49ac-b9da-67e35a9b42be");
				guidField.Name = "created_by";
				guidField.Label = "Created by";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("4b56686e-971e-4b8e-8356-642a8f341bff"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: attachment Field: created_by Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: attachment Field Name: file >>
			{
				InputFileField fileField = new InputFileField();
				fileField.Id = new Guid("258c87ab-28ef-41c7-8a3f-fd132a4868d7");
				fileField.Name = "file";
				fileField.Label = "File";
				fileField.PlaceholderText = null;
				fileField.Description = null;
				fileField.HelpText = null;
				fileField.Required = true;
				fileField.Unique = false;
				fileField.Searchable = false;
				fileField.Auditable = false;
				fileField.System = true;
				fileField.DefaultValue = "/not-found.txt";
				fileField.EnableSecurity = false;
				fileField.Permissions = new FieldPermissions();
				fileField.Permissions.CanRead = new List<Guid>();
				fileField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("4b56686e-971e-4b8e-8356-642a8f341bff"), fileField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: attachment Field: file Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: attachment Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("e00c4e7b-4d36-4b75-bc74-42bac73f2c35");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("4b56686e-971e-4b8e-8356-642a8f341bff"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: attachment Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: feed_item >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("d50269c9-d1ab-4800-bdcc-5b25a2d15847");
					entity.Id = new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a");
					entity.Name = "feed_item";
					entity.Label = "Feed item";
					entity.LabelPlural = "Feed items";
					entity.System = true;
					entity.IconName = "fa fa-file";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = null;
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					//DELETE
					entity.RecordPermissions.CanDelete.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: feed_item creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: feed_item Field Name: created_by >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("00b257db-7a60-4ebe-b316-4f4937426cf1");
				guidField.Name = "created_by";
				guidField.Label = "Created by";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: feed_item Field: created_by Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: feed_item Field Name: created_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("5d3b46e0-d884-4025-adc0-7ae39085d36a");
				datetimeField.Name = "created_on";
				datetimeField.Label = "Created on";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = true;
				datetimeField.Unique = false;
				datetimeField.Searchable = true;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = true;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: feed_item Field: created_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: feed_item Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("a38bc9f4-9073-47e6-9dc0-c8d193545825");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: feed_item Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: feed_item Field Name: subject >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("feb5c8ac-45dc-4b07-9cd9-38e18eb9bf31");
				textboxField.Name = "subject";
				textboxField.Label = "Subject";
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
					var response = entMan.CreateField(new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: feed_item Field: subject Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: feed_item Field Name: body >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("370f3f96-b008-449d-8a8e-ff29519bd295");
				textboxField.Name = "body";
				textboxField.Label = "Body";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = "text,html or json of the feed item content";
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
					var response = entMan.CreateField(new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: feed_item Field: body Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: feed_item Field Name: type >>
			{
				InputSelectField dropdownField = new InputSelectField();
				dropdownField.Id = new Guid("ecc28658-571b-467d-9d85-51972de8b94d");
				dropdownField.Name = "type";
				dropdownField.Label = "Type";
				dropdownField.PlaceholderText = null;
				dropdownField.Description = null;
				dropdownField.HelpText = null;
				dropdownField.Required = true;
				dropdownField.Unique = false;
				dropdownField.Searchable = false;
				dropdownField.Auditable = false;
				dropdownField.System = true;
				dropdownField.DefaultValue = "system";
				dropdownField.Options = new List<SelectOption>
	{
		new SelectOption() { Label = "system", Value = "system", IconClass = "", Color = ""},
		new SelectOption() { Label = "task", Value = "task", IconClass = "", Color = ""},
		new SelectOption() { Label = "case", Value = "case", IconClass = "", Color = ""},
		new SelectOption() { Label = "timelog", Value = "timelog", IconClass = "", Color = ""},
		new SelectOption() { Label = "comment", Value = "comment", IconClass = "", Color = ""}
	};
				dropdownField.EnableSecurity = false;
				dropdownField.Permissions = new FieldPermissions();
				dropdownField.Permissions.CanRead = new List<Guid>();
				dropdownField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a"), dropdownField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: feed_item Field: type Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: feed_item Field Name: l_related_records >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("7411b977-ff65-493d-b00d-d09cb82e409e");
				textboxField.Name = "l_related_records";
				textboxField.Label = "Related Record lookup";
				textboxField.PlaceholderText = null;
				textboxField.Description = "csv list of related parent primary key";
				textboxField.HelpText = null;
				textboxField.Required = false;
				textboxField.Unique = false;
				textboxField.Searchable = true;
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
					var response = entMan.CreateField(new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: feed_item Field: l_related_records Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: case_status >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("0e54a6c3-aa35-4048-bf8f-7d05afcc5eb3");
					entity.Id = new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27");
					entity.Name = "case_status";
					entity.Label = "Case status";
					entity.LabelPlural = "Case statuses";
					entity.System = true;
					entity.IconName = "far fa-dot-circle";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = new Guid("f9082286-ff37-402b-b860-284d86dff1b6");
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: case_status creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: case_status Field Name: is_default >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("1ac9f589-f785-4046-ab73-13678afa007c");
				checkboxField.Name = "is_default";
				checkboxField.Label = "Is Default";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_status Field: is_default Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_status Field Name: label >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("f9082286-ff37-402b-b860-284d86dff1b6");
				textboxField.Name = "label";
				textboxField.Label = "Label";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = true;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "label";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_status Field: label Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_status Field Name: sort_index >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("db39f9f2-f2e2-4dfb-80bd-75691983e5ce");
				numberField.Name = "sort_index";
				numberField.Label = "Sort index";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("1.0");
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
					var response = entMan.CreateField(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_status Field: sort_index Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_status Field Name: is_closed >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("1060aeda-8374-4d7e-a746-72fd082b120c");
				checkboxField.Name = "is_closed";
				checkboxField.Label = "Is Closed";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_status Field: is_closed Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_status Field Name: is_system >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("4356d21a-5c7b-4716-af4c-571045e582f6");
				checkboxField.Name = "is_system";
				checkboxField.Label = "System";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_status Field: is_system Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_status Field Name: is_enabled >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("ce3d169c-4eb2-488b-9b52-d15a800b4588");
				checkboxField.Name = "is_enabled";
				checkboxField.Label = "Enabled";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = true;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = true;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_status Field: is_enabled Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_status Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("21fa7a14-d953-47b0-a842-7516851197b9");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_status Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_status Field Name: icon_class >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("be916956-db6d-4f5b-9cf1-b892e3dafcca");
				textboxField.Name = "icon_class";
				textboxField.Label = "Icon class";
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
					var response = entMan.CreateField(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_status Field: icon_class Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_status Field Name: color >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("3c822afe-764e-4144-9da2-06f2801883f7");
				textboxField.Name = "color";
				textboxField.Label = "Color";
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
					var response = entMan.CreateField(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_status Field: color Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: case >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("5f50a281-8106-4b21-bb14-78ba7cf8ba37");
					entity.Id = new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c");
					entity.Name = "case";
					entity.Label = "Case";
					entity.LabelPlural = "Cases";
					entity.System = true;
					entity.IconName = "fa fa-file";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = null;
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: case creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: case Field Name: account_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("829fefbc-3578-4311-881c-33597d236830");
				guidField.Name = "account_id";
				guidField.Label = "Account";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: account_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case Field Name: created_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("104ef526-773d-464a-98cd-774d184cc7de");
				datetimeField.Name = "created_on";
				datetimeField.Label = "Created on";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = true;
				datetimeField.Unique = false;
				datetimeField.Searchable = false;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = true;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: created_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case Field Name: created_by >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("c3d1aeb5-0d96-4be0-aa9e-d7732ca68709");
				guidField.Name = "created_by";
				guidField.Label = "Created by";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: created_by Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case Field Name: owner_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("3c25fb36-8d33-4a90-bd60-7a9bf401b547");
				guidField.Name = "owner_id";
				guidField.Label = "Owner";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = true;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: owner_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case Field Name: description >>
			{
				InputHtmlField htmlField = new InputHtmlField();
				htmlField.Id = new Guid("b8ac2f8c-1f24-4452-ad47-e7f3cf254ff4");
				htmlField.Name = "description";
				htmlField.Label = "Description";
				htmlField.PlaceholderText = null;
				htmlField.Description = null;
				htmlField.HelpText = null;
				htmlField.Required = true;
				htmlField.Unique = false;
				htmlField.Searchable = false;
				htmlField.Auditable = false;
				htmlField.System = true;
				htmlField.DefaultValue = "description";
				htmlField.EnableSecurity = false;
				htmlField.Permissions = new FieldPermissions();
				htmlField.Permissions.CanRead = new List<Guid>();
				htmlField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), htmlField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: description Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case Field Name: subject >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("8f5477aa-0fc6-4c97-9192-b9dadadaf497");
				textboxField.Name = "subject";
				textboxField.Label = "Subject";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "subject";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: subject Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field*** Entity: case Field Name: number >>
			{
				InputAutoNumberField autonumberField = new InputAutoNumberField();
				autonumberField.Id = new Guid("19648468-893b-49f9-b8bd-b84add0c50f5");
				autonumberField.Name = "number";
				autonumberField.Label = "Number";
				autonumberField.PlaceholderText = null;
				autonumberField.Description = null;
				autonumberField.HelpText = null;
				autonumberField.Required = true;
				autonumberField.Unique = true;
				autonumberField.Searchable = false;
				autonumberField.Auditable = false;
				autonumberField.System = true;
				autonumberField.DefaultValue = Decimal.Parse("1.0");
				autonumberField.DisplayFormat = "{0}";
				autonumberField.StartingNumber = Decimal.Parse("1.0");
				autonumberField.EnableSecurity = false;
				autonumberField.Permissions = new FieldPermissions();
				autonumberField.Permissions.CanRead = new List<Guid>();
				autonumberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), autonumberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: number Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case Field Name: closed_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("ac852183-e438-4c84-aaa3-dc12a0f2ad8e");
				datetimeField.Name = "closed_on";
				datetimeField.Label = "Closed on";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = false;
				datetimeField.Unique = false;
				datetimeField.Searchable = false;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = false;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: closed_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("b8af3f7a-78a4-445c-ad28-b7eea1d9eff5");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = false;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = false;
				textboxField.DefaultValue = null;
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case Field Name: priority >>
			{
				InputSelectField dropdownField = new InputSelectField();
				dropdownField.Id = new Guid("1dbe204d-3771-4f56-a2f5-bff0cf1831b4");
				dropdownField.Name = "priority";
				dropdownField.Label = "Priority";
				dropdownField.PlaceholderText = null;
				dropdownField.Description = null;
				dropdownField.HelpText = null;
				dropdownField.Required = true;
				dropdownField.Unique = false;
				dropdownField.Searchable = true;
				dropdownField.Auditable = false;
				dropdownField.System = true;
				dropdownField.DefaultValue = "low";
				dropdownField.Options = new List<SelectOption>
	{
		new SelectOption() { Label = "high", Value = "high", IconClass = "", Color = ""},
		new SelectOption() { Label = "medium", Value = "medium", IconClass = "", Color = ""},
		new SelectOption() { Label = "low", Value = "low", IconClass = "", Color = ""}
	};
				dropdownField.EnableSecurity = false;
				dropdownField.Permissions = new FieldPermissions();
				dropdownField.Permissions.CanRead = new List<Guid>();
				dropdownField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), dropdownField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: priority Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case Field Name: status_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("05b97041-7a65-4d27-8c06-fc154d2fcbf5");
				guidField.Name = "status_id";
				guidField.Label = "Status";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("4f17785b-c430-4fea-9fa9-8cfef931c60e");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: status_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case Field Name: type_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("0b1f1244-6090-41e7-9684-53d2968bb33a");
				guidField.Name = "type_id";
				guidField.Label = "Type";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("3298c9b3-560b-48b2-b148-997f9cbb3bec");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: type_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: milestone >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("2a719820-91cd-4c88-9424-eead58a7ed34");
					entity.Id = new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e");
					entity.Name = "milestone";
					entity.Label = "Milestone";
					entity.LabelPlural = "Milestones";
					entity.System = true;
					entity.IconName = "fa fa-file";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = null;
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: milestone creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: milestone Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("7e45be43-3c6b-4213-832c-81f6ddeaaa8d");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: milestone Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: milestone Field Name: name >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("d7ef0086-0198-4322-8bee-07ff0f8f8ce6");
				textboxField.Name = "name";
				textboxField.Label = "Name";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "name";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: milestone Field: name Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: milestone Field Name: description >>
			{
				InputMultiLineTextField textareaField = new InputMultiLineTextField();
				textareaField.Id = new Guid("80e565bf-045b-4e66-84a3-1e73f516d68e");
				textareaField.Name = "description";
				textareaField.Label = "Description";
				textareaField.PlaceholderText = null;
				textareaField.Description = null;
				textareaField.HelpText = null;
				textareaField.Required = false;
				textareaField.Unique = false;
				textareaField.Searchable = false;
				textareaField.Auditable = false;
				textareaField.System = true;
				textareaField.DefaultValue = null;
				textareaField.MaxLength = null;
				textareaField.VisibleLineNumber = null;
				textareaField.EnableSecurity = false;
				textareaField.Permissions = new FieldPermissions();
				textareaField.Permissions.CanRead = new List<Guid>();
				textareaField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e"), textareaField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: milestone Field: description Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: milestone Field Name: completed_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("89e72f14-976a-4858-b328-acc932b79331");
				datetimeField.Name = "completed_on";
				datetimeField.Label = "Completed on";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = false;
				datetimeField.Unique = false;
				datetimeField.Searchable = false;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = false;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: milestone Field: completed_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: milestone Field Name: start_date >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("5721d151-a436-4f2b-b8e2-a9fa39f57da6");
				datetimeField.Name = "start_date";
				datetimeField.Label = "Start date";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = true;
				datetimeField.Unique = false;
				datetimeField.Searchable = false;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = true;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: milestone Field: start_date Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: milestone Field Name: target_date >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("386bd473-0b74-437a-9c12-5f7d51f3da78");
				datetimeField.Name = "target_date";
				datetimeField.Label = "Target date";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = true;
				datetimeField.Unique = false;
				datetimeField.Searchable = false;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = true;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: milestone Field: target_date Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: milestone Field Name: is_completed >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("47fffd98-09f4-47a8-b0da-2f727412ffb5");
				checkboxField.Name = "is_completed";
				checkboxField.Label = "Is Completed";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: milestone Field: is_completed Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: milestone Field Name: x_tasks_assigned >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("f1935d22-4b36-4e00-bb4b-9e164569df6c");
				numberField.Name = "x_tasks_assigned";
				numberField.Label = "Tasks assigned calculation";
				numberField.PlaceholderText = null;
				numberField.Description = null;
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
				numberField.EnableSecurity = true;
				numberField.Permissions = new FieldPermissions();
				numberField.Permissions.CanRead = new List<Guid>();
				numberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				numberField.Permissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
				numberField.Permissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: milestone Field: x_tasks_assigned Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: milestone Field Name: x_tasks_completed >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("3ac8fb5c-325c-4678-941f-d0df421b014c");
				numberField.Name = "x_tasks_completed";
				numberField.Label = "Tasks completed";
				numberField.PlaceholderText = null;
				numberField.Description = null;
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
				numberField.EnableSecurity = true;
				numberField.Permissions = new FieldPermissions();
				numberField.Permissions.CanRead = new List<Guid>();
				numberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				numberField.Permissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
				numberField.Permissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: milestone Field: x_tasks_completed Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: task_status >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("f4f9b011-b4d5-4651-8dc9-c608a0d216da");
					entity.Id = new Guid("9221f095-f749-4b88-94e5-9fa485527ef7");
					entity.Name = "task_status";
					entity.Label = "Task status";
					entity.LabelPlural = "Task statuses";
					entity.System = true;
					entity.IconName = "far fa-dot-circle";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = new Guid("6c242d1c-420e-4649-8a73-b891d5b508e0");
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: task_status creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: task_status Field Name: is_closed >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("7500864e-4106-4b36-ba9a-93f70c386c88");
				checkboxField.Name = "is_closed";
				checkboxField.Label = "Is closed";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9221f095-f749-4b88-94e5-9fa485527ef7"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_status Field: is_closed Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_status Field Name: is_default >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("55bed50d-c263-4ebe-9ea7-2f79afb0d39b");
				checkboxField.Name = "is_default";
				checkboxField.Label = "Is Default";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9221f095-f749-4b88-94e5-9fa485527ef7"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_status Field: is_default Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_status Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("4ccd4df3-860f-44d8-b486-5f47cb798451");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("9221f095-f749-4b88-94e5-9fa485527ef7"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_status Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_status Field Name: label >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("6c242d1c-420e-4649-8a73-b891d5b508e0");
				textboxField.Name = "label";
				textboxField.Label = "Label";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "label";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9221f095-f749-4b88-94e5-9fa485527ef7"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_status Field: label Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_status Field Name: sort_index >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("b16db646-3c63-4fb8-acac-499d1ddda5f9");
				numberField.Name = "sort_index";
				numberField.Label = "Sort index";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("1.0");
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
					var response = entMan.CreateField(new Guid("9221f095-f749-4b88-94e5-9fa485527ef7"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_status Field: sort_index Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_status Field Name: is_system >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("cd1217f7-0342-47e0-9017-7374a5419091");
				checkboxField.Name = "is_system";
				checkboxField.Label = "System";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9221f095-f749-4b88-94e5-9fa485527ef7"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_status Field: is_system Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_status Field Name: is_enabled >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("8ff595bc-edb9-4a07-8dca-9fd9a380ab35");
				checkboxField.Name = "is_enabled";
				checkboxField.Label = "Enabled";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = true;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9221f095-f749-4b88-94e5-9fa485527ef7"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_status Field: is_enabled Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_status Field Name: icon_class >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("2cbbd720-9bdc-48c4-a405-c2c2bcf81d35");
				textboxField.Name = "icon_class";
				textboxField.Label = "Icon class";
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
					var response = entMan.CreateField(new Guid("9221f095-f749-4b88-94e5-9fa485527ef7"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_status Field: icon_class Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_status Field Name: color >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("0a3b6de4-1678-47a5-ab0e-512c9abd2f1b");
				textboxField.Name = "color";
				textboxField.Label = "Color";
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
					var response = entMan.CreateField(new Guid("9221f095-f749-4b88-94e5-9fa485527ef7"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_status Field: color Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: task_type >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("6ad6d228-1714-4c02-a8a5-b22ddaa6a97f");
					entity.Id = new Guid("35999e55-821c-4798-8e8f-29d8c672c9b9");
					entity.Name = "task_type";
					entity.Label = "Task type";
					entity.LabelPlural = "Task types";
					entity.System = true;
					entity.IconName = "far fa-dot-circle";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = new Guid("ddbf3b6f-8f09-4e37-95e2-e71de7ca5d3c");
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: task_type creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: task_type Field Name: is_default >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("ab08ae57-de06-4788-999d-32d42cd4b75e");
				checkboxField.Name = "is_default";
				checkboxField.Label = "Is Default";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("35999e55-821c-4798-8e8f-29d8c672c9b9"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_type Field: is_default Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_type Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("133f2fc0-b3ca-44f0-8624-724493ec4de5");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("35999e55-821c-4798-8e8f-29d8c672c9b9"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_type Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_type Field Name: label >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("ddbf3b6f-8f09-4e37-95e2-e71de7ca5d3c");
				textboxField.Name = "label";
				textboxField.Label = "Label";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "label";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("35999e55-821c-4798-8e8f-29d8c672c9b9"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_type Field: label Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_type Field Name: sort_index >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("ef3aa457-ef03-4942-81d5-57753e1fc226");
				numberField.Name = "sort_index";
				numberField.Label = "Sort index";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("1.0");
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
					var response = entMan.CreateField(new Guid("35999e55-821c-4798-8e8f-29d8c672c9b9"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_type Field: sort_index Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_type Field Name: is_system >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("6fa4d8a2-60ad-4882-9f06-e44dfc83266e");
				checkboxField.Name = "is_system";
				checkboxField.Label = "System";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("35999e55-821c-4798-8e8f-29d8c672c9b9"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_type Field: is_system Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_type Field Name: is_enabled >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("22d47a37-7c03-4d6a-bb8a-0e76fd5a3371");
				checkboxField.Name = "is_enabled";
				checkboxField.Label = "Enabled";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = true;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("35999e55-821c-4798-8e8f-29d8c672c9b9"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_type Field: is_enabled Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_type Field Name: icon_class >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("525efe80-eb47-42e7-8f34-910ab13afa29");
				textboxField.Name = "icon_class";
				textboxField.Label = "Icon class";
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
					var response = entMan.CreateField(new Guid("35999e55-821c-4798-8e8f-29d8c672c9b9"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_type Field: icon_class Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task_type Field Name: color >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("8026ba91-abb2-4bdf-aa5f-134285b4a959");
				textboxField.Name = "color";
				textboxField.Label = "Color";
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
					var response = entMan.CreateField(new Guid("35999e55-821c-4798-8e8f-29d8c672c9b9"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_type Field: color Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: task >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("0e540cf8-2de6-419c-8ed1-42b8c637d191");
					entity.Id = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
					entity.Name = "task";
					entity.Label = "Task";
					entity.LabelPlural = "Tasks";
					entity.System = true;
					entity.IconName = "fas fa-user-cog";
					entity.Color = "#009688";
					entity.RecordScreenIdField = null;
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: task creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("94d069fd-04c1-4a3e-a735-867225df364d");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
						throw new Exception("System error 10060. Entity: task Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: subject >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("0b7a6ede-3439-4826-9438-05da4a428f98");
				textboxField.Name = "subject";
				textboxField.Label = "Subject";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "subject";
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
						throw new Exception("System error 10060. Entity: task Field: subject Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: body >>
			{
				InputHtmlField htmlField = new InputHtmlField();
				htmlField.Id = new Guid("e88fcb5d-f581-49d9-81e7-6840200ed3c1");
				htmlField.Name = "body";
				htmlField.Label = "Body";
				htmlField.PlaceholderText = null;
				htmlField.Description = null;
				htmlField.HelpText = null;
				htmlField.Required = false;
				htmlField.Unique = false;
				htmlField.Searchable = false;
				htmlField.Auditable = false;
				htmlField.System = true;
				htmlField.DefaultValue = null;
				htmlField.EnableSecurity = false;
				htmlField.Permissions = new FieldPermissions();
				htmlField.Permissions.CanRead = new List<Guid>();
				htmlField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), htmlField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: body Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: created_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("0047b19a-d691-468f-9125-e7d29d3edcd1");
				datetimeField.Name = "created_on";
				datetimeField.Label = "Created on";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = true;
				datetimeField.Unique = false;
				datetimeField.Searchable = false;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = true;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: created_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: created_by >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("8f864e2e-8b03-4b82-b80b-b7f7537005cf");
				guidField.Name = "created_by";
				guidField.Label = "Created by";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: created_by Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: completed_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("ad426e80-56f9-45b5-bb48-5ced8b39918b");
				datetimeField.Name = "completed_on";
				datetimeField.Label = "Completed on";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = false;
				datetimeField.Unique = false;
				datetimeField.Searchable = false;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = false;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: completed_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field*** Entity: task Field Name: number >>
			{
				InputAutoNumberField autonumberField = new InputAutoNumberField();
				autonumberField.Id = new Guid("b27da2eb-d872-44c8-ba54-58d2e05c298f");
				autonumberField.Name = "number";
				autonumberField.Label = "Number";
				autonumberField.PlaceholderText = null;
				autonumberField.Description = null;
				autonumberField.HelpText = null;
				autonumberField.Required = true;
				autonumberField.Unique = false;
				autonumberField.Searchable = false;
				autonumberField.Auditable = false;
				autonumberField.System = true;
				autonumberField.DefaultValue = Decimal.Parse("1.0");
				autonumberField.DisplayFormat = "{0}";
				autonumberField.StartingNumber = Decimal.Parse("1.0");
				autonumberField.EnableSecurity = false;
				autonumberField.Permissions = new FieldPermissions();
				autonumberField.Permissions.CanRead = new List<Guid>();
				autonumberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), autonumberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: number Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: parent_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("95a24e9c-4505-4b41-8a6d-55d13896504e");
				guidField.Name = "parent_id";
				guidField.Label = "Parent task Id";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = false;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = null;
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: parent_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: status_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("686dd7fd-6280-4dd3-bb20-1126179f261e");
				guidField.Name = "status_id";
				guidField.Label = "Status Id";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("f3fdd750-0c16-4215-93b3-5373bd528d1f");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: status_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: key >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("35de5afe-39d8-4c0c-9b11-4151d3fddb13");
				textboxField.Name = "key";
				textboxField.Label = "Key";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = true;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "Key";
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
						throw new Exception("System error 10060. Entity: task Field: key Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: x_search >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("4141d7d2-a35c-418a-a13e-426fc6cc072f");
				textboxField.Name = "x_search";
				textboxField.Label = "Search index";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = false;
				textboxField.Unique = false;
				textboxField.Searchable = true;
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
						throw new Exception("System error 10060. Entity: task Field: x_search Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: estimated_minutes >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("873ef699-8921-434d-97ff-97c42d011b18");
				numberField.Name = "estimated_minutes";
				numberField.Label = "Estimated minutes";
				numberField.PlaceholderText = null;
				numberField.Description = null;
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
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: estimated_minutes Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: x_billable_minutes >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("3398512d-1f0e-48a9-b5e8-ee36920c70ba");
				numberField.Name = "x_billable_minutes";
				numberField.Label = "Logged Billable minutes";
				numberField.PlaceholderText = null;
				numberField.Description = null;
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
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: x_billable_minutes Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: x_nonbillable_minutes >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("9ba52854-fe79-483e-8142-4aa2ae85008b");
				numberField.Name = "x_nonbillable_minutes";
				numberField.Label = "Logged Nonbillable minutes";
				numberField.PlaceholderText = null;
				numberField.Description = null;
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
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: x_nonbillable_minutes Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: start_date >>
			{
				InputDateField dateField = new InputDateField();
				dateField.Id = new Guid("9eda2377-04fa-4c6d-8bd5-2dea6dcf12d7");
				dateField.Name = "start_date";
				dateField.Label = "Start date";
				dateField.PlaceholderText = null;
				dateField.Description = null;
				dateField.HelpText = null;
				dateField.Required = false;
				dateField.Unique = false;
				dateField.Searchable = false;
				dateField.Auditable = false;
				dateField.System = true;
				dateField.DefaultValue = null;
				dateField.Format = "yyyy-MMM-dd";
				dateField.UseCurrentTimeAsDefaultValue = false;
				dateField.EnableSecurity = false;
				dateField.Permissions = new FieldPermissions();
				dateField.Permissions.CanRead = new List<Guid>();
				dateField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), dateField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: start_date Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: target_date >>
			{
				InputDateField dateField = new InputDateField();
				dateField.Id = new Guid("1ded55ce-8daf-4444-abbe-3c3be064554e");
				dateField.Name = "target_date";
				dateField.Label = "Target day";
				dateField.PlaceholderText = null;
				dateField.Description = null;
				dateField.HelpText = null;
				dateField.Required = false;
				dateField.Unique = false;
				dateField.Searchable = false;
				dateField.Auditable = false;
				dateField.System = true;
				dateField.DefaultValue = null;
				dateField.Format = "yyyy-MMM-dd";
				dateField.UseCurrentTimeAsDefaultValue = false;
				dateField.EnableSecurity = false;
				dateField.Permissions = new FieldPermissions();
				dateField.Permissions.CanRead = new List<Guid>();
				dateField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), dateField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: target_date Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: priority >>
			{
				InputSelectField dropdownField = new InputSelectField();
				dropdownField.Id = new Guid("ce81430c-bf33-4d8d-bfcd-5c4be13700d3");
				dropdownField.Name = "priority";
				dropdownField.Label = "Priority";
				dropdownField.PlaceholderText = null;
				dropdownField.Description = null;
				dropdownField.HelpText = null;
				dropdownField.Required = true;
				dropdownField.Unique = false;
				dropdownField.Searchable = true;
				dropdownField.Auditable = false;
				dropdownField.System = true;
				dropdownField.DefaultValue = "1";
				dropdownField.Options = new List<SelectOption>
	{
		new SelectOption() { Label = "low", Value = "1", IconClass = "fas fa-fw fa-arrow-circle-down", Color = "#4CAF50"},
		new SelectOption() { Label = "medium", Value = "2", IconClass = "fa fa-fw fa-minus-circle", Color = "#2196F3"},
		new SelectOption() { Label = "high", Value = "3", IconClass = "fas fa-fw fa-arrow-circle-up", Color = "#F44336"}
	};
				dropdownField.EnableSecurity = false;
				dropdownField.Permissions = new FieldPermissions();
				dropdownField.Permissions.CanRead = new List<Guid>();
				dropdownField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), dropdownField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: priority Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: timelog_started_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("41266a06-98a0-4e48-9d47-a7fe20bc3c3f");
				datetimeField.Name = "timelog_started_on";
				datetimeField.Label = "Timelog started on";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = false;
				datetimeField.Unique = false;
				datetimeField.Searchable = false;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = false;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: timelog_started_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: owner_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("aa486ab3-5510-4373-90b9-5285a6c6468f");
				guidField.Name = "owner_id";
				guidField.Label = "Owner";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = false;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: owner_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: type_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("955ed90c-c158-4423-a766-33646ce1d7e7");
				guidField.Name = "type_id";
				guidField.Label = "Type";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("a0465e9f-5d5f-433d-acf1-1da0eaec78b4");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: type_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: account >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("4c0c80d0-8b01-445f-9913-0be18d9086d1");
					entity.Id = new Guid("2e22b50f-e444-4b62-a171-076e51246939");
					entity.Name = "account";
					entity.Label = "Account";
					entity.LabelPlural = "Accounts";
					entity.System = true;
					entity.IconName = "fas fa-user-tie";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = new Guid("b8be9afb-687c-411a-a274-ebe5d36a8100");
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					entity.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: account creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: name >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("b8be9afb-687c-411a-a274-ebe5d36a8100");
				textboxField.Name = "name";
				textboxField.Label = "Name";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "name";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: name Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("fda3238e-52b5-48b7-82ad-558573c6e25c");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: case_type >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("d46667d4-8dc1-4834-884d-3578b717a5f1");
					entity.Id = new Guid("0dfeba58-40bb-4205-a539-c16d5c0885ad");
					entity.Name = "case_type";
					entity.Label = "Case type";
					entity.LabelPlural = "Case types";
					entity.System = true;
					entity.IconName = "far fa-dot-circle";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = new Guid("db0edb8f-a5f6-4baa-91f5-929fc732cc95");
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: case_type creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: case_type Field Name: is_default >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("bcd41123-7264-4fde-bb2b-460a78d823d5");
				checkboxField.Name = "is_default";
				checkboxField.Label = "Is Default";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0dfeba58-40bb-4205-a539-c16d5c0885ad"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_type Field: is_default Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_type Field Name: is_enabled >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("892063aa-5007-4d0f-a584-65da00704bed");
				checkboxField.Name = "is_enabled";
				checkboxField.Label = "Is Enabled";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = true;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = true;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0dfeba58-40bb-4205-a539-c16d5c0885ad"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_type Field: is_enabled Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_type Field Name: is_system >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("d7e49c0e-f715-4789-90bf-d33557f549c1");
				checkboxField.Name = "is_system";
				checkboxField.Label = "Is System";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0dfeba58-40bb-4205-a539-c16d5c0885ad"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_type Field: is_system Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_type Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("44b50db8-da41-45db-918f-d25599ab4673");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("0dfeba58-40bb-4205-a539-c16d5c0885ad"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_type Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_type Field Name: sort_index >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("ac65a1e4-af93-4b96-9c49-902b0b1f7524");
				numberField.Name = "sort_index";
				numberField.Label = "Sort Index";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("1.0");
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
					var response = entMan.CreateField(new Guid("0dfeba58-40bb-4205-a539-c16d5c0885ad"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_type Field: sort_index Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_type Field Name: label >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("db0edb8f-a5f6-4baa-91f5-929fc732cc95");
				textboxField.Name = "label";
				textboxField.Label = "Label";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = true;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "label";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("0dfeba58-40bb-4205-a539-c16d5c0885ad"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_type Field: label Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_type Field Name: icon_class >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("991233b8-ff9c-4f32-9b9c-502425b41486");
				textboxField.Name = "icon_class";
				textboxField.Label = "Icon class";
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
					var response = entMan.CreateField(new Guid("0dfeba58-40bb-4205-a539-c16d5c0885ad"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_type Field: icon_class Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case_type Field Name: color >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("0bde42d0-adb0-4c70-894a-394d480685d7");
				textboxField.Name = "color";
				textboxField.Label = "Color";
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
					var response = entMan.CreateField(new Guid("0dfeba58-40bb-4205-a539-c16d5c0885ad"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_type Field: color Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: industry >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("c1dd4f43-ec95-4f08-b9b7-26d46d6f5305");
					entity.Id = new Guid("2c60e662-367e-475d-9fcb-3ead55178a56");
					entity.Name = "industry";
					entity.Label = "Industry";
					entity.LabelPlural = "Industries";
					entity.System = true;
					entity.IconName = "far fa-dot-circle";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = new Guid("cdc0ddda-d38c-46fa-901c-71409c685dd1");
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: industry creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: industry Field Name: is_default >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("24a63589-ecc8-4f33-84bf-7d2259357d7e");
				checkboxField.Name = "is_default";
				checkboxField.Label = "Is Default";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2c60e662-367e-475d-9fcb-3ead55178a56"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: industry Field: is_default Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: industry Field Name: is_enabled >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("2b679734-662d-4a3e-9823-1924d52de2d9");
				checkboxField.Name = "is_enabled";
				checkboxField.Label = "Is Enabled";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = true;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2c60e662-367e-475d-9fcb-3ead55178a56"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: industry Field: is_enabled Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: industry Field Name: is_system >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("6924ac15-fbf6-4385-8ef4-7ecf2ee9391a");
				checkboxField.Name = "is_system";
				checkboxField.Label = "Is System";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2c60e662-367e-475d-9fcb-3ead55178a56"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: industry Field: is_system Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: industry Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("99691c52-8bf5-4ccf-9efa-23906a5d6811");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("2c60e662-367e-475d-9fcb-3ead55178a56"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: industry Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: industry Field Name: label >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("cdc0ddda-d38c-46fa-901c-71409c685dd1");
				textboxField.Name = "label";
				textboxField.Label = "Label";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = true;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "label";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2c60e662-367e-475d-9fcb-3ead55178a56"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: industry Field: label Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: industry Field Name: sort_index >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("e3e4c409-cc40-4885-b208-df4af05ddfa6");
				numberField.Name = "sort_index";
				numberField.Label = "Sort Index";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("1.0");
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
					var response = entMan.CreateField(new Guid("2c60e662-367e-475d-9fcb-3ead55178a56"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: industry Field: sort_index Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: industry Field Name: icon_class >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("12054115-1434-444c-a669-97d8eee32910");
				textboxField.Name = "icon_class";
				textboxField.Label = "Icon class";
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
					var response = entMan.CreateField(new Guid("2c60e662-367e-475d-9fcb-3ead55178a56"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: industry Field: icon_class Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: industry Field Name: color >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("69e2e024-e4e1-492a-9a39-c7ae24f74bd6");
				textboxField.Name = "color";
				textboxField.Label = "Color";
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
					var response = entMan.CreateField(new Guid("2c60e662-367e-475d-9fcb-3ead55178a56"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: industry Field: color Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: project >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("51990f1b-fbe9-4700-9b11-0822b149edd1");
					entity.Id = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
					entity.Name = "project";
					entity.Label = "Project";
					entity.LabelPlural = "Projects";
					entity.System = true;
					entity.IconName = "fas fa-cogs";
					entity.Color = "#9c27b0";
					entity.RecordScreenIdField = null;
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: project creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("d19e0db3-6a35-4cc6-bb84-a213ecc2b3a5");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: account_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("b30b4f7a-d5b0-423c-b341-76d2bcfff290");
				guidField.Name = "account_id";
				guidField.Label = "Account";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = false;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = null;
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: account_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: description >>
			{
				InputHtmlField htmlField = new InputHtmlField();
				htmlField.Id = new Guid("a84c650d-67db-40f1-8cdd-14f735de6124");
				htmlField.Name = "description";
				htmlField.Label = "Description";
				htmlField.PlaceholderText = null;
				htmlField.Description = null;
				htmlField.HelpText = null;
				htmlField.Required = false;
				htmlField.Unique = false;
				htmlField.Searchable = false;
				htmlField.Auditable = false;
				htmlField.System = true;
				htmlField.DefaultValue = null;
				htmlField.EnableSecurity = false;
				htmlField.Permissions = new FieldPermissions();
				htmlField.Permissions.CanRead = new List<Guid>();
				htmlField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), htmlField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: description Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: name >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("2d8e82c2-38c6-4aa9-94c8-119bcda02db4");
				textboxField.Name = "name";
				textboxField.Label = "Name";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "name";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: name Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: owner_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("44a0a125-fab5-4be0-825b-24187946be21");
				guidField.Name = "owner_id";
				guidField.Label = "Owner";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
				guidField.GenerateNewId = false;
				guidField.EnableSecurity = false;
				guidField.Permissions = new FieldPermissions();
				guidField.Permissions.CanRead = new List<Guid>();
				guidField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: owner_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: hour_rate >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("34a76b35-227d-404a-ba82-4575ca6679bc");
				numberField.Name = "hour_rate";
				numberField.Label = "Hour rate";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = false;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = null;
				numberField.MinValue = null;
				numberField.MaxValue = null;
				numberField.DecimalPlaces = byte.Parse("2");
				numberField.EnableSecurity = false;
				numberField.Permissions = new FieldPermissions();
				numberField.Permissions.CanRead = new List<Guid>();
				numberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: hour_rate Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: start_date >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("90bdb090-763b-4fb3-a79b-0652dfe170a1");
				datetimeField.Name = "start_date";
				datetimeField.Label = "Start date";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = false;
				datetimeField.Unique = false;
				datetimeField.Searchable = false;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = false;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: start_date Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: end_date >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("b26ada8b-3ac4-4992-9ec6-f0764b9bdb68");
				datetimeField.Name = "end_date";
				datetimeField.Label = "End date";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = false;
				datetimeField.Unique = false;
				datetimeField.Searchable = false;
				datetimeField.Auditable = false;
				datetimeField.System = true;
				datetimeField.DefaultValue = null;
				datetimeField.Format = "yyyy-MMM-dd HH:mm";
				datetimeField.UseCurrentTimeAsDefaultValue = false;
				datetimeField.EnableSecurity = false;
				datetimeField.Permissions = new FieldPermissions();
				datetimeField.Permissions.CanRead = new List<Guid>();
				datetimeField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: end_date Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: budget_type >>
			{
				InputSelectField dropdownField = new InputSelectField();
				dropdownField.Id = new Guid("d558a173-0bfe-4be2-86a3-2b22e60cd9c4");
				dropdownField.Name = "budget_type";
				dropdownField.Label = "Budget type";
				dropdownField.PlaceholderText = null;
				dropdownField.Description = null;
				dropdownField.HelpText = null;
				dropdownField.Required = true;
				dropdownField.Unique = false;
				dropdownField.Searchable = false;
				dropdownField.Auditable = false;
				dropdownField.System = true;
				dropdownField.DefaultValue = "none";
				dropdownField.Options = new List<SelectOption>
	{
		new SelectOption() { Label = "none", Value = "none", IconClass = "", Color = ""},
		new SelectOption() { Label = "on amount", Value = "on amount", IconClass = "", Color = ""},
		new SelectOption() { Label = "on duration", Value = "on duration", IconClass = "", Color = ""}
	};
				dropdownField.EnableSecurity = false;
				dropdownField.Permissions = new FieldPermissions();
				dropdownField.Permissions.CanRead = new List<Guid>();
				dropdownField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), dropdownField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: budget_type Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: status >>
			{
				InputSelectField dropdownField = new InputSelectField();
				dropdownField.Id = new Guid("683f6289-b642-4d8c-bf97-fd950d1bbd35");
				dropdownField.Name = "status";
				dropdownField.Label = "Status";
				dropdownField.PlaceholderText = null;
				dropdownField.Description = null;
				dropdownField.HelpText = null;
				dropdownField.Required = true;
				dropdownField.Unique = false;
				dropdownField.Searchable = false;
				dropdownField.Auditable = false;
				dropdownField.System = true;
				dropdownField.DefaultValue = "draft";
				dropdownField.Options = new List<SelectOption>
	{
		new SelectOption() { Label = "draft", Value = "draft", IconClass = "", Color = ""},
		new SelectOption() { Label = "published", Value = "published", IconClass = "", Color = ""},
		new SelectOption() { Label = "archived", Value = "archived", IconClass = "", Color = ""}
	};
				dropdownField.EnableSecurity = false;
				dropdownField.Permissions = new FieldPermissions();
				dropdownField.Permissions.CanRead = new List<Guid>();
				dropdownField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), dropdownField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: status Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: abbr >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("041ef7cc-7a70-4d90-bd78-63d513acd879");
				textboxField.Name = "abbr";
				textboxField.Label = "Abbreviation";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = "used to better identify the tasks";
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "NXT";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: abbr Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: budget_amount >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("4e654797-5661-4003-a64f-f9abb0e8d95a");
				numberField.Name = "budget_amount";
				numberField.Label = "Budget amount";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = "money or hours";
				numberField.Required = false;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = null;
				numberField.MinValue = null;
				numberField.MaxValue = null;
				numberField.DecimalPlaces = byte.Parse("2");
				numberField.EnableSecurity = false;
				numberField.Permissions = new FieldPermissions();
				numberField.Permissions.CanRead = new List<Guid>();
				numberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: budget_amount Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: is_billable >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("93032d98-309d-4564-9244-5354ff284381");
				checkboxField.Name = "is_billable";
				checkboxField.Label = "Is Billable";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = "default task billable status";
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = true;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: is_billable Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: project Field Name: billing_method >>
			{
				InputSelectField dropdownField = new InputSelectField();
				dropdownField.Id = new Guid("f64e585c-dab4-4d51-9e73-5a769389aaa2");
				dropdownField.Name = "billing_method";
				dropdownField.Label = "Billing method";
				dropdownField.PlaceholderText = null;
				dropdownField.Description = null;
				dropdownField.HelpText = null;
				dropdownField.Required = true;
				dropdownField.Unique = false;
				dropdownField.Searchable = false;
				dropdownField.Auditable = false;
				dropdownField.System = true;
				dropdownField.DefaultValue = "project_hour_cost";
				dropdownField.Options = new List<SelectOption>
	{
		new SelectOption() { Label = "Project hour cost", Value = "project_hour_cost", IconClass = "", Color = ""},
		new SelectOption() { Label = "User hour cost", Value = "user_hour_cost", IconClass = "", Color = ""}
	};
				dropdownField.EnableSecurity = false;
				dropdownField.Permissions = new FieldPermissions();
				dropdownField.Permissions.CanRead = new List<Guid>();
				dropdownField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), dropdownField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: billing_method Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: solutation >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("9ecf246f-6c4c-40dd-88cc-4e99a285e5bd");
					entity.Id = new Guid("f0b64034-e0f6-452e-b82b-88186af6df88");
					entity.Name = "solutation";
					entity.Label = "Salutation";
					entity.LabelPlural = "Salutations";
					entity.System = true;
					entity.IconName = "far fa-dot-circle";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = new Guid("ba844d45-7d08-458f-9f07-1d70ff5f7706");
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: solutation creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: solutation Field Name: is_default >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("276dcad3-0fa5-4864-9160-17f16f64f269");
				checkboxField.Name = "is_default";
				checkboxField.Label = "Is Default";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("f0b64034-e0f6-452e-b82b-88186af6df88"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: solutation Field: is_default Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: solutation Field Name: is_enabled >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("85a54d91-bc6c-42c2-90d8-d365b5735b84");
				checkboxField.Name = "is_enabled";
				checkboxField.Label = "Is Enabled";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = true;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("f0b64034-e0f6-452e-b82b-88186af6df88"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: solutation Field: is_enabled Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: solutation Field Name: is_system >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("173d6a40-b0ee-489a-94b3-615e3d57f24a");
				checkboxField.Name = "is_system";
				checkboxField.Label = "Is System";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("f0b64034-e0f6-452e-b82b-88186af6df88"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: solutation Field: is_system Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: solutation Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("c15c4fa2-5988-46a5-87e8-342007aa66b3");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("f0b64034-e0f6-452e-b82b-88186af6df88"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: solutation Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: solutation Field Name: label >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("ba844d45-7d08-458f-9f07-1d70ff5f7706");
				textboxField.Name = "label";
				textboxField.Label = "Label";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = true;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "label";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("f0b64034-e0f6-452e-b82b-88186af6df88"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: solutation Field: label Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: solutation Field Name: sort_index >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("e7e65a42-72e1-493f-bd94-5fe79ec61bea");
				numberField.Name = "sort_index";
				numberField.Label = "Sort Index";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("1.0");
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
					var response = entMan.CreateField(new Guid("f0b64034-e0f6-452e-b82b-88186af6df88"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: solutation Field: sort_index Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: country >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("504fd581-4995-4508-89c9-4efd73e86f26");
					entity.Id = new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8");
					entity.Name = "country";
					entity.Label = "Country";
					entity.LabelPlural = "Countries";
					entity.System = true;
					entity.IconName = "far fa-dot-circle";
					entity.Color = "#f44336";
					entity.RecordScreenIdField = new Guid("a530c698-7bbd-43cb-bc34-d7aab34cfe73");
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//READ
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: country creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: country Field Name: is_default >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("e42a54bf-32bf-477e-8728-43574bcc8419");
				checkboxField.Name = "is_default";
				checkboxField.Label = "Is default";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: country Field: is_default Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: country Field Name: is_enabled >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("fe0e63fc-ae1c-403f-baa3-099c5d7f7029");
				checkboxField.Name = "is_enabled";
				checkboxField.Label = "Is Enabled";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = true;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: country Field: is_enabled Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: country Field Name: is_system >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("866dcf10-1c58-4edd-9074-f07328c2346a");
				checkboxField.Name = "is_system";
				checkboxField.Label = "Is System";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = null;
				checkboxField.Required = true;
				checkboxField.Unique = false;
				checkboxField.Searchable = false;
				checkboxField.Auditable = false;
				checkboxField.System = true;
				checkboxField.DefaultValue = false;
				checkboxField.EnableSecurity = false;
				checkboxField.Permissions = new FieldPermissions();
				checkboxField.Permissions.CanRead = new List<Guid>();
				checkboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: country Field: is_system Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: country Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("75a9ffef-b200-4933-9eea-76f3dba79170");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: country Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: country Field Name: sort_index >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("8db4f17f-db29-45ea-95c1-b98b1e32af24");
				numberField.Name = "sort_index";
				numberField.Label = "Sort Index";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = true;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("1.0");
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
					var response = entMan.CreateField(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: country Field: sort_index Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: country Field Name: label >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("a530c698-7bbd-43cb-bc34-d7aab34cfe73");
				textboxField.Name = "label";
				textboxField.Label = "Label";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = true;
				textboxField.Searchable = true;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "label";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: country Field: label Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: country Field Name: icon_class >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("07890d89-c039-426c-acac-d4f10a3d9ac7");
				textboxField.Name = "icon_class";
				textboxField.Label = "Icon class";
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
					var response = entMan.CreateField(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: country Field: icon_class Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: country Field Name: color >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("8c86ebcf-926f-42a7-9eec-58c1caa43024");
				textboxField.Name = "color";
				textboxField.Label = "Color";
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
					var response = entMan.CreateField(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: country Field: color Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: task_type_1n_task >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("35999e55-821c-4798-8e8f-29d8c672c9b9")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("9386226e-381e-4522-b27b-fb5514d77902")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "type_id");
				relation.Id = new Guid("2925c7ea-72fe-4c12-a1f6-9baa9281141e");
				relation.Name = "task_type_1n_task";
				relation.Label = "task_type_1n_task";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.OneToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: task_type_1n_task Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: task_status_1n_task >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("9221f095-f749-4b88-94e5-9fa485527ef7")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("9386226e-381e-4522-b27b-fb5514d77902")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "status_id");
				relation.Id = new Guid("dcc6eb09-627b-4525-839f-d26dd57a0608");
				relation.Name = "task_status_1n_task";
				relation.Label = "task_status_1n_task";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.OneToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: task_status_1n_task Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: milestone_nn_task >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("9386226e-381e-4522-b27b-fb5514d77902")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
				relation.Id = new Guid("b070a627-01ce-4534-ab45-5c6f1a3867a4");
				relation.Name = "milestone_nn_task";
				relation.Label = "milestone_nn_task";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.ManyToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: milestone_nn_task Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: project_nn_task >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("9386226e-381e-4522-b27b-fb5514d77902")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
				relation.Id = new Guid("b1db4466-7423-44e9-b6b9-3063222c9e15");
				relation.Name = "project_nn_task";
				relation.Label = "project_nn_task";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.ManyToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: project_nn_task Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: project_nn_milestone >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
				relation.Id = new Guid("55c8d6e2-f26d-4689-9d1b-a8c1b9de1672");
				relation.Name = "project_nn_milestone";
				relation.Label = "project_nn_milestone";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.ManyToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: project_nn_milestone Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: case_status_1n_case >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
				relation.Id = new Guid("c523c594-1f84-495e-84f3-a569cb384586");
				relation.Name = "case_status_1n_case";
				relation.Label = "case_status_1n_case";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.OneToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: case_status_1n_case Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: account_1n_case >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("2e22b50f-e444-4b62-a171-076e51246939")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
				relation.Id = new Guid("06d07760-41ba-408c-af61-a1fdc8493de3");
				relation.Name = "account_1n_case";
				relation.Label = "account_1n_case";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.OneToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: account_1n_case Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: comment_nn_attachment >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("4b56686e-971e-4b8e-8356-642a8f341bff")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
				relation.Id = new Guid("4b80a487-83ed-42e6-9be7-0ddf91afee15");
				relation.Name = "comment_nn_attachment";
				relation.Label = "comment_nn_attachment";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.ManyToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: comment_nn_attachment Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: case_type_1n_case >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("0dfeba58-40bb-4205-a539-c16d5c0885ad")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "type_id");
				relation.Id = new Guid("c4a6918b-7918-4806-83cb-fd3d87fe5a10");
				relation.Name = "case_type_1n_case";
				relation.Label = "case_type_1n_case";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.OneToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: case_type_1n_case Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: user_1n_project_owner >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "owner_id");
				relation.Id = new Guid("2f0ff495-54a0-4343-a4e5-67f5ca552519");
				relation.Name = "user_1n_project_owner";
				relation.Label = "user_1n_project_owner";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.OneToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: user_1n_project_owner Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: user_1n_task >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("9386226e-381e-4522-b27b-fb5514d77902")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "owner_id");
				relation.Id = new Guid("a28ceeb8-10a8-4652-bf44-8dc2ad4350b7");
				relation.Name = "user_1n_task";
				relation.Label = "user_1n_task";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.OneToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: user_1n_task Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: user_1n_task_creator >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("9386226e-381e-4522-b27b-fb5514d77902")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "created_by");
				relation.Id = new Guid("871bd069-8351-4e14-a064-96081ea3d811");
				relation.Name = "user_1n_task_creator";
				relation.Label = "user_1n_task_creator";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.OneToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: user_1n_task_creator Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: user_1n_comment >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "created_by");
				relation.Id = new Guid("6cd28549-5e99-4e05-9d12-c7b2ee104d02");
				relation.Name = "user_1n_comment";
				relation.Label = "user_1n_comment";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.OneToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: user_1n_comment Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: user_1n_feed_item >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "created_by");
				relation.Id = new Guid("df720595-dd35-484b-8b64-d21c2b57a687");
				relation.Name = "user_1n_feed_item";
				relation.Label = "user_1n_feed_item";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.OneToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: user_1n_feed_item Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: user_1n_timelog >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("750153c5-1df9-408f-b856-727078a525bc")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "created_by");
				relation.Id = new Guid("c4cb9a51-483b-4798-93b0-d73568c1bfc7");
				relation.Name = "user_1n_timelog";
				relation.Label = "user_1n_timelog";
				relation.Description = "";
				relation.System = true;
				relation.RelationType = EntityRelationType.OneToMany;
				relation.OriginEntityId = originEntity.Id;
				relation.OriginEntityName = originEntity.Name;
				relation.OriginFieldId = originField.Id;
				relation.OriginFieldName = originField.Name;
				relation.TargetEntityId = targetEntity.Id;
				relation.TargetEntityName = targetEntity.Name;
				relation.TargetFieldId = targetField.Id;
				relation.TargetFieldName = targetField.Name;
				{
					var response = relMan.Create(relation);
					if (!response.Success)
						throw new Exception("System error 10060. Relation: user_1n_timelog Create. Message:" + response.Message);
				}
			}
			#endregion




		}
	}
}
