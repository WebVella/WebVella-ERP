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
		private static void Patch20190203(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
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

			#region << ***Create field***  Entity: task Field Name: start_time >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("6f4a77ba-1ac2-4d9a-934e-ed5f9026102a");
				datetimeField.Name = "start_time";
				datetimeField.Label = "Start time";
				datetimeField.PlaceholderText = null;
				datetimeField.Description = null;
				datetimeField.HelpText = null;
				datetimeField.Required = false;
				datetimeField.Unique = false;
				datetimeField.Searchable = true;
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
						throw new Exception("System error 10060. Entity: task Field: start_time Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: end_time >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("452dd069-6353-48af-ba37-6fa7672c59a4");
				datetimeField.Name = "end_time";
				datetimeField.Label = "End time";
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
						throw new Exception("System error 10060. Entity: task Field: end_time Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: recurrence_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("36a9a76e-9be7-4796-b5b9-e8485fe61c4a");
				guidField.Name = "recurrence_id";
				guidField.Label = "Recurrence";
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
						throw new Exception("System error 10060. Entity: task Field: recurrence_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: task Field Name: reserve_time >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("e1ba8fa3-1aba-4563-8baa-0a2fe409178d");
				checkboxField.Name = "reserve_time";
				checkboxField.Label = "Reserve time";
				checkboxField.PlaceholderText = null;
				checkboxField.Description = null;
				checkboxField.HelpText = "Whether other tasks can be performed in the same time period";
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
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: reserve_time Message:" + response.Message);
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

			#region << ***Create relation*** Relation name: user_nn_task_watchers >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("9386226e-381e-4522-b27b-fb5514d77902")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
				relation.Id = new Guid("879b49cc-6af6-4b34-a554-761ec992534d");
				relation.Name = "user_nn_task_watchers";
				relation.Label = "user_nn_task_watchers";
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
						throw new Exception("System error 10060. Relation: user_nn_task_watchers Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create record*** Id: 4f17785b-c430-4fea-9fa9-8cfef931c60e (case_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""4f17785b-c430-4fea-9fa9-8cfef931c60e"",
  ""is_default"": true,
  ""label"": ""Open"",
  ""sort_index"": 1.0,
  ""is_closed"": false,
  ""is_system"": true,
  ""is_enabled"": true,
  ""l_scope"": """",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c04d2a73-9fd3-4d00-b32e-9887e517f3bf (case_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c04d2a73-9fd3-4d00-b32e-9887e517f3bf"",
  ""is_default"": false,
  ""label"": ""Closed - Duplicate"",
  ""sort_index"": 103.0,
  ""is_closed"": true,
  ""is_system"": true,
  ""is_enabled"": true,
  ""l_scope"": """",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b7368bd9-ea1c-4091-8c57-26e5c8360c29 (case_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b7368bd9-ea1c-4091-8c57-26e5c8360c29"",
  ""is_default"": false,
  ""label"": ""Closed - No Response"",
  ""sort_index"": 102.0,
  ""is_closed"": true,
  ""is_system"": true,
  ""is_enabled"": true,
  ""l_scope"": """",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2aac0c08-5e84-477d-add0-5bc60057eba4 (case_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2aac0c08-5e84-477d-add0-5bc60057eba4"",
  ""is_default"": false,
  ""label"": ""Closed - Resolved"",
  ""sort_index"": 100.0,
  ""is_closed"": true,
  ""is_system"": true,
  ""is_enabled"": true,
  ""l_scope"": """",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 61cba6d4-b175-4a89-94b6-6b700ce9adb9 (case_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""61cba6d4-b175-4a89-94b6-6b700ce9adb9"",
  ""is_default"": false,
  ""label"": ""Closed - Rejected"",
  ""sort_index"": 101.0,
  ""is_closed"": true,
  ""is_system"": true,
  ""is_enabled"": true,
  ""l_scope"": """",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: fe9d8d44-996a-4e8a-8448-3d7731d4f278 (case_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""fe9d8d44-996a-4e8a-8448-3d7731d4f278"",
  ""is_default"": false,
  ""label"": ""Re-Open"",
  ""sort_index"": 10.0,
  ""is_closed"": false,
  ""is_system"": true,
  ""is_enabled"": true,
  ""l_scope"": """",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 508d9e1b-8896-46ed-a6fd-734197bdb1c8 (case_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""508d9e1b-8896-46ed-a6fd-734197bdb1c8"",
  ""is_default"": false,
  ""label"": ""Wait for Customer"",
  ""sort_index"": 50.0,
  ""is_closed"": false,
  ""is_system"": true,
  ""is_enabled"": true,
  ""l_scope"": """",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 95170be2-dcd9-4399-9ac4-7ecefb67ad2d (case_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""95170be2-dcd9-4399-9ac4-7ecefb67ad2d"",
  ""is_default"": false,
  ""label"": ""Escalated"",
  ""sort_index"": 52.0,
  ""is_closed"": false,
  ""is_system"": true,
  ""is_enabled"": true,
  ""l_scope"": """",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ef18bf1e-314e-472f-887b-e348daef9676 (case_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ef18bf1e-314e-472f-887b-e348daef9676"",
  ""is_default"": false,
  ""label"": ""On Hold"",
  ""sort_index"": 40.0,
  ""is_closed"": false,
  ""is_system"": true,
  ""is_enabled"": true,
  ""l_scope"": """",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 3298c9b3-560b-48b2-b148-997f9cbb3bec (case_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""3298c9b3-560b-48b2-b148-997f9cbb3bec"",
  ""is_default"": true,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""sort_index"": 1.0,
  ""label"": ""General"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: f228d073-bd09-48ed-85c7-54c6231c9182 (case_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""f228d073-bd09-48ed-85c7-54c6231c9182"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""sort_index"": 2.0,
  ""label"": ""Problem"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 92b35547-f91b-492d-9c83-c29c3a4d132d (case_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""92b35547-f91b-492d-9c83-c29c3a4d132d"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""sort_index"": 3.0,
  ""label"": ""Question"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 15e7adc5-a3e7-47c5-ae54-252cffe82923 (case_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""15e7adc5-a3e7-47c5-ae54-252cffe82923"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""sort_index"": 4.0,
  ""label"": ""Feature Request"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: dc4b7e9f-0790-47b5-a89c-268740aded38 (case_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""dc4b7e9f-0790-47b5-a89c-268740aded38"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""sort_index"": 5.0,
  ""label"": ""Duplicate"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("case_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 00a97e45-d521-4b86-9964-f154509eb614 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""00a97e45-d521-4b86-9964-f154509eb614"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""sort_index"": 41.0,
  ""label"": ""Central African Republic"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 8fcb5a15-edf1-4dfd-8a5a-e84687503279 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""8fcb5a15-edf1-4dfd-8a5a-e84687503279"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 1.0,
  ""label"": ""Albania"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7552e3fd-9ccd-4515-84e9-3b1fe9794e00 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7552e3fd-9ccd-4515-84e9-3b1fe9794e00"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 2.0,
  ""label"": ""Algeria"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 443df119-21b0-4e47-abe5-ef223e5b0258 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""443df119-21b0-4e47-abe5-ef223e5b0258"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 3.0,
  ""label"": ""American Samoa"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 1b01fa05-0166-46b7-933a-d11e6deeb4e5 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""1b01fa05-0166-46b7-933a-d11e6deeb4e5"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 4.0,
  ""label"": ""Andorra"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 5a26477c-b265-48cf-bcb5-ca933ef5f1a5 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""5a26477c-b265-48cf-bcb5-ca933ef5f1a5"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 5.0,
  ""label"": ""Angola"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 700af9ba-173a-43c8-aa90-c1f7f01264c8 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""700af9ba-173a-43c8-aa90-c1f7f01264c8"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 6.0,
  ""label"": ""Anguilla"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2ae5dce0-5523-46cd-87fc-4495703b3e56 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2ae5dce0-5523-46cd-87fc-4495703b3e56"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 7.0,
  ""label"": ""Antigua and Barbuda"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 458a5e2a-2cef-4713-a74b-0c1de9942b4a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""458a5e2a-2cef-4713-a74b-0c1de9942b4a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 8.0,
  ""label"": ""Argentina"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 99ca3348-9692-4938-aec1-d378b4dfcf1e (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""99ca3348-9692-4938-aec1-d378b4dfcf1e"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 9.0,
  ""label"": ""Armenia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7a4e79dd-3ef7-4176-a1e7-553af43f2c94 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7a4e79dd-3ef7-4176-a1e7-553af43f2c94"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 10.0,
  ""label"": ""Aruba"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 180f24e5-a587-4255-90a0-b5ef2974fc9a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""180f24e5-a587-4255-90a0-b5ef2974fc9a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 12.0,
  ""label"": ""Austria"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a66ad5ec-54d2-45a1-b305-378b6692aadd (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a66ad5ec-54d2-45a1-b305-378b6692aadd"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 13.0,
  ""label"": ""Azerbaijan"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: aad0c9d9-a8f3-475f-b6fd-94b32ce358a3 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""aad0c9d9-a8f3-475f-b6fd-94b32ce358a3"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 15.0,
  ""label"": ""Bahamas"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 98060283-843a-46b0-b9e3-c77a48b9366a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""98060283-843a-46b0-b9e3-c77a48b9366a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 16.0,
  ""label"": ""Bahrain"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7b5a30da-b737-4f48-a612-1e2d6b2a8c4b (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7b5a30da-b737-4f48-a612-1e2d6b2a8c4b"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 18.0,
  ""label"": ""Barbados"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2b63c3b9-e752-4583-bdaf-ff9c2188e3f5 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2b63c3b9-e752-4583-bdaf-ff9c2188e3f5"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 19.0,
  ""label"": ""Belarus"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 9d31d56b-e17e-4f41-b102-e12a79204180 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""9d31d56b-e17e-4f41-b102-e12a79204180"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 20.0,
  ""label"": ""Belgium"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 35ebbc8c-657f-4fb7-b03e-e287318e0434 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""35ebbc8c-657f-4fb7-b03e-e287318e0434"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 22.0,
  ""label"": ""Benin"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 313c938d-ce07-487f-96cb-4ae515e5c2d1 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""313c938d-ce07-487f-96cb-4ae515e5c2d1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 23.0,
  ""label"": ""Bermuda"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 9fce8b44-ab1f-42e4-98f0-e94ab78d1470 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""9fce8b44-ab1f-42e4-98f0-e94ab78d1470"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 24.0,
  ""label"": ""Bhutan"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 63e56df7-dd17-4dc7-9331-7de47841afa8 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""63e56df7-dd17-4dc7-9331-7de47841afa8"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 25.0,
  ""label"": ""Bolivia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 96c15c5d-f43e-4e32-b448-0ad55312ee5a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""96c15c5d-f43e-4e32-b448-0ad55312ee5a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 26.0,
  ""label"": ""Bonaire"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a0ae3808-db80-484f-a991-8af98ea598eb (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a0ae3808-db80-484f-a991-8af98ea598eb"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 27.0,
  ""label"": ""Bosnia and Herzegovina"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 36c6c20c-a8c0-4817-9219-0351f71f0a5f (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""36c6c20c-a8c0-4817-9219-0351f71f0a5f"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 29.0,
  ""label"": ""Brazil"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 08dbf43a-13ce-4959-9324-f9e594afab56 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""08dbf43a-13ce-4959-9324-f9e594afab56"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 30.0,
  ""label"": ""British Virgin Islands"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 72991df0-462d-4792-ae9b-ab8e2cf7c46a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""72991df0-462d-4792-ae9b-ab8e2cf7c46a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 31.0,
  ""label"": ""Brunei"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 5649873f-6a5f-400f-9c52-23ca3abf898c (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""5649873f-6a5f-400f-9c52-23ca3abf898c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 33.0,
  ""label"": ""Burkina Faso"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 97c3bc3b-1a8a-44b2-be35-7d116843798a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""97c3bc3b-1a8a-44b2-be35-7d116843798a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 34.0,
  ""label"": ""Burundi"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: afa9ee14-aa5b-4182-9d88-ff27e77305df (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""afa9ee14-aa5b-4182-9d88-ff27e77305df"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 35.0,
  ""label"": ""Cabo Verde"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 51695904-cd3f-48a6-95cc-e60b712ea890 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""51695904-cd3f-48a6-95cc-e60b712ea890"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 36.0,
  ""label"": ""Cambodia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 59fc7bd3-42ce-4a49-ba57-1198b478fd41 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""59fc7bd3-42ce-4a49-ba57-1198b478fd41"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 37.0,
  ""label"": ""Cameroon"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2d19e676-2abd-4b2a-8225-ad95693e1ac1 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2d19e676-2abd-4b2a-8225-ad95693e1ac1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 38.0,
  ""label"": ""Canada"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 456c51ff-6056-47ce-b4fd-7a3de1b8fa4f (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""456c51ff-6056-47ce-b4fd-7a3de1b8fa4f"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 39.0,
  ""label"": ""Canary Islands"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2d5e8aca-205f-4139-8337-433b0375807d (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2d5e8aca-205f-4139-8337-433b0375807d"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 44.0,
  ""label"": ""China"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 770cde3a-4983-4443-a38d-a9a652831f1c (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""770cde3a-4983-4443-a38d-a9a652831f1c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 46.0,
  ""label"": ""Colombia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 0a84a4c3-48a6-48b3-920f-f5e528356ab7 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""0a84a4c3-48a6-48b3-920f-f5e528356ab7"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 47.0,
  ""label"": ""Congo"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 6a8a6d3e-f244-43b2-af8c-8cbaf607fc42 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6a8a6d3e-f244-43b2-af8c-8cbaf607fc42"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 48.0,
  ""label"": ""Congo, Democratic Republic of the"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 8a5a35a7-d736-445c-9602-7b83126b31d8 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""8a5a35a7-d736-445c-9602-7b83126b31d8"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 49.0,
  ""label"": ""Congo, Republic of the"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2319e1fb-06fc-43b2-90f7-7a39307a3d6f (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2319e1fb-06fc-43b2-90f7-7a39307a3d6f"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 51.0,
  ""label"": ""Costa Rica"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 6e141c27-56cf-4c90-828c-35859f13e699 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6e141c27-56cf-4c90-828c-35859f13e699"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 58.0,
  ""label"": ""Djibouti"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 4b80a7e8-fa18-49ab-baa5-79c59de03171 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""4b80a7e8-fa18-49ab-baa5-79c59de03171"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 60.0,
  ""label"": ""Dominican Republic"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7333bbe1-3a60-4825-8f69-e493ea8b7460 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7333bbe1-3a60-4825-8f69-e493ea8b7460"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 61.0,
  ""label"": ""East Timor"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 82889b2a-6856-4623-b9e0-70d5437d4bf5 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""82889b2a-6856-4623-b9e0-70d5437d4bf5"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 65.0,
  ""label"": ""England"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 9a2229b3-1558-4977-b380-4a0b7cb3e767 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""9a2229b3-1558-4977-b380-4a0b7cb3e767"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 67.0,
  ""label"": ""Eritrea"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 6daad92e-8478-4b94-b872-88b665a9d175 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6daad92e-8478-4b94-b872-88b665a9d175"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 68.0,
  ""label"": ""Estonia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 75fdf457-d7d5-4167-b895-8a92e85008a7 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""75fdf457-d7d5-4167-b895-8a92e85008a7"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 69.0,
  ""label"": ""Ethiopia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 851b0969-ae6f-4f1d-aaf2-86d44332a01e (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""851b0969-ae6f-4f1d-aaf2-86d44332a01e"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 70.0,
  ""label"": ""Faroe Islands"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 5c33f6b0-4fb2-4d9f-9fbe-4dc06de5e25a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""5c33f6b0-4fb2-4d9f-9fbe-4dc06de5e25a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 71.0,
  ""label"": ""Fiji"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 90190569-8f85-49a1-9c4b-3b83a6269308 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""90190569-8f85-49a1-9c4b-3b83a6269308"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 72.0,
  ""label"": ""Finland"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: aaa88c2a-5a6d-43a5-ae87-04b504b39baf (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""aaa88c2a-5a6d-43a5-ae87-04b504b39baf"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 73.0,
  ""label"": ""France"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 91ea802f-920d-486f-9adf-074533857dea (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""91ea802f-920d-486f-9adf-074533857dea"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 76.0,
  ""label"": ""Gabon"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 6bcec4ed-4b0b-43f3-9672-30761b433dce (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6bcec4ed-4b0b-43f3-9672-30761b433dce"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 77.0,
  ""label"": ""Gambia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a1ce84a2-aa75-420c-85fc-f77b88f0cba1 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a1ce84a2-aa75-420c-85fc-f77b88f0cba1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 78.0,
  ""label"": ""Georgia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 8f7f96d8-4ef2-4a40-aba9-3fd8e1e9eaaf (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""8f7f96d8-4ef2-4a40-aba9-3fd8e1e9eaaf"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 79.0,
  ""label"": ""Germany"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 248c7a5c-3e2e-43fa-8cd2-4d67606699a2 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""248c7a5c-3e2e-43fa-8cd2-4d67606699a2"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 81.0,
  ""label"": ""Gibraltar"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 9a974050-0431-4a13-be61-b4db5daac50c (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""9a974050-0431-4a13-be61-b4db5daac50c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 82.0,
  ""label"": ""Greece"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 829c7e8b-706e-4f0c-962b-5faaed73c5ec (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""829c7e8b-706e-4f0c-962b-5faaed73c5ec"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 83.0,
  ""label"": ""Greenland"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 6f5787c6-33db-4688-83b1-c8ba345217e4 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6f5787c6-33db-4688-83b1-c8ba345217e4"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 87.0,
  ""label"": ""Guatemala"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a2a3c0a8-29a4-4aee-aa14-7ccaca935447 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a2a3c0a8-29a4-4aee-aa14-7ccaca935447"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 88.0,
  ""label"": ""Guernsey"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 4028bdb9-404e-4ff3-8f89-b06c1eed3813 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""4028bdb9-404e-4ff3-8f89-b06c1eed3813"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 89.0,
  ""label"": ""Guinea"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 0317bdf7-afae-415a-bc05-1d5d3bee3ee8 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""0317bdf7-afae-415a-bc05-1d5d3bee3ee8"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 90.0,
  ""label"": ""Guinea-Bissau"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 5e8ca369-26f1-4078-a3f3-4d974387e76c (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""5e8ca369-26f1-4078-a3f3-4d974387e76c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 91.0,
  ""label"": ""Guyana"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 4211a4bd-1727-429e-aaae-60864eda86c7 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""4211a4bd-1727-429e-aaae-60864eda86c7"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 92.0,
  ""label"": ""Haiti"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 3008271e-6ad1-42db-8c0b-3e5ac3eaefb6 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""3008271e-6ad1-42db-8c0b-3e5ac3eaefb6"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 93.0,
  ""label"": ""Honduras"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 6801a090-6718-461e-b00d-bb4018e2fbe2 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6801a090-6718-461e-b00d-bb4018e2fbe2"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 95.0,
  ""label"": ""Hungary"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2bc72bd5-21e6-43ea-adda-06fb801ea0f1 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2bc72bd5-21e6-43ea-adda-06fb801ea0f1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 96.0,
  ""label"": ""Iceland"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 9b868f72-d81d-4f98-ba2b-ad0e044f0fa3 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""9b868f72-d81d-4f98-ba2b-ad0e044f0fa3"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 97.0,
  ""label"": ""India"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 12b77bd5-cebe-4614-8a96-fb8632bdd75a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""12b77bd5-cebe-4614-8a96-fb8632bdd75a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 98.0,
  ""label"": ""Indonesia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a159a30b-6484-42e5-a72d-e0c9e7e70fe7 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a159a30b-6484-42e5-a72d-e0c9e7e70fe7"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 99.0,
  ""label"": ""Iraq"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 52ba1946-c3e9-45fb-a8e4-4acab80d0bb9 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""52ba1946-c3e9-45fb-a8e4-4acab80d0bb9"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 100.0,
  ""label"": ""Ireland"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 35f72bb7-cfa9-4748-bf4f-762955293c0d (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""35f72bb7-cfa9-4748-bf4f-762955293c0d"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 101.0,
  ""label"": ""Israel"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7012c266-6ac7-445b-a30d-1ebdd04bc065 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7012c266-6ac7-445b-a30d-1ebdd04bc065"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 104.0,
  ""label"": ""Japan"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 410f24a7-a8a3-4e55-89dd-d0a2501faf5a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""410f24a7-a8a3-4e55-89dd-d0a2501faf5a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 106.0,
  ""label"": ""Jordan"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 804e269e-6052-4632-8cc3-a47ec8409e13 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""804e269e-6052-4632-8cc3-a47ec8409e13"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 107.0,
  ""label"": ""Kazakhstan"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 4b07c136-9b81-41df-8a49-f4165e1aea71 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""4b07c136-9b81-41df-8a49-f4165e1aea71"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 110.0,
  ""label"": ""Kosrae"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 3f00cd68-3888-4681-a2be-8dffe3fac464 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""3f00cd68-3888-4681-a2be-8dffe3fac464"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 111.0,
  ""label"": ""Kuwait"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 69228099-e931-497a-b3db-d0ab1036b729 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""69228099-e931-497a-b3db-d0ab1036b729"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 114.0,
  ""label"": ""Latvia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 189b5f11-ddea-413e-9c90-6ed50974ab42 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""189b5f11-ddea-413e-9c90-6ed50974ab42"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 115.0,
  ""label"": ""Lebanon"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 870a34c2-4af0-4c38-bae7-45b9a947d1bc (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""870a34c2-4af0-4c38-bae7-45b9a947d1bc"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 118.0,
  ""label"": ""Liechtenstein"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 69e439aa-5ce0-4020-8c1b-47360da8b00b (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""69e439aa-5ce0-4020-8c1b-47360da8b00b"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 119.0,
  ""label"": ""Lithuania"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 5b1e74c7-c6ef-4075-bf77-9c8c3b6f9a1b (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""5b1e74c7-c6ef-4075-bf77-9c8c3b6f9a1b"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 120.0,
  ""label"": ""Luxembourg"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 54c24327-9936-46f5-b8c0-c2d3d5710b08 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""54c24327-9936-46f5-b8c0-c2d3d5710b08"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 121.0,
  ""label"": ""Macau"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 67f4027f-88a6-4932-bfbf-604ab888fd14 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""67f4027f-88a6-4932-bfbf-604ab888fd14"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 124.0,
  ""label"": ""Madeira"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 6d03ade1-2639-417c-b3fd-53cec6ef1bf7 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6d03ade1-2639-417c-b3fd-53cec6ef1bf7"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 126.0,
  ""label"": ""Malaysia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 5d77fb04-d7ce-4f1c-8475-6969d59ce556 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""5d77fb04-d7ce-4f1c-8475-6969d59ce556"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 127.0,
  ""label"": ""Maldives"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 607cbc5e-b66c-40fe-b008-bf973080d966 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""607cbc5e-b66c-40fe-b008-bf973080d966"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 130.0,
  ""label"": ""Marshall Islands"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a506a791-d102-4eb8-bda5-a78457f0331d (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a506a791-d102-4eb8-bda5-a78457f0331d"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 131.0,
  ""label"": ""Martinique"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 3fad6fc4-9399-4209-a13e-8fea70b72309 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""3fad6fc4-9399-4209-a13e-8fea70b72309"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 132.0,
  ""label"": ""Mauritania"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 15d42391-1892-4c05-b9f7-a73c4cc723ca (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""15d42391-1892-4c05-b9f7-a73c4cc723ca"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 133.0,
  ""label"": ""Mauritius"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 0b62cb59-6cb7-40d9-8506-9101991ea733 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""0b62cb59-6cb7-40d9-8506-9101991ea733"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 134.0,
  ""label"": ""Mexico"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 6ce0293c-17ad-426d-abe8-a79abb684173 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6ce0293c-17ad-426d-abe8-a79abb684173"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 136.0,
  ""label"": ""Moldova"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 49b23737-1fdc-4ed8-9868-4bf6bbd84a6e (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""49b23737-1fdc-4ed8-9868-4bf6bbd84a6e"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 139.0,
  ""label"": ""Montenegro"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 23d87f89-8bee-44dd-bb61-a1403d1bd856 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""23d87f89-8bee-44dd-bb61-a1403d1bd856"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 140.0,
  ""label"": ""Montserrat"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 035725de-5ad8-40cf-8398-0ca35b33c056 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""035725de-5ad8-40cf-8398-0ca35b33c056"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 141.0,
  ""label"": ""Morocco"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: aea77264-ab46-441e-b09a-71e8d9046692 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""aea77264-ab46-441e-b09a-71e8d9046692"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 145.0,
  ""label"": ""Netherlands"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ada831b9-4508-414c-a71d-32c3d6a5694a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ada831b9-4508-414c-a71d-32c3d6a5694a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 146.0,
  ""label"": ""Netherlands Antilles"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 31591ebf-9ab5-4c15-a77d-b01e41d98fa0 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""31591ebf-9ab5-4c15-a77d-b01e41d98fa0"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 147.0,
  ""label"": ""New Caledonia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 1acc9930-c156-4aeb-b74d-509f71d4c16b (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""1acc9930-c156-4aeb-b74d-509f71d4c16b"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 148.0,
  ""label"": ""New Zealand"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c5af8be7-5886-4bf9-abb5-2387e01c0ae4 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c5af8be7-5886-4bf9-abb5-2387e01c0ae4"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 149.0,
  ""label"": ""Nicaragua"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 36371162-e81b-4843-9598-021b1c001f0e (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""36371162-e81b-4843-9598-021b1c001f0e"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 150.0,
  ""label"": ""Niger"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 72810f32-d1e9-4486-a78a-e0cb13554168 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""72810f32-d1e9-4486-a78a-e0cb13554168"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 151.0,
  ""label"": ""Nigeria"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7ecf10a3-b685-4554-a30e-529b638ea0a5 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7ecf10a3-b685-4554-a30e-529b638ea0a5"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 152.0,
  ""label"": ""Norfolk"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: eac88b44-158a-479e-939e-d39dad07ca7b (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""eac88b44-158a-479e-939e-d39dad07ca7b"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 153.0,
  ""label"": ""Northern Ireland"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: d4ff935a-3906-4079-bd1e-a5974e2060b7 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""d4ff935a-3906-4079-bd1e-a5974e2060b7"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 154.0,
  ""label"": ""Northern Mariana Islands"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 04a3ae51-a265-40a7-8547-14795c875b68 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""04a3ae51-a265-40a7-8547-14795c875b68"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 155.0,
  ""label"": ""Norway"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 444dbb6f-42d9-4df1-aef4-6ba9e1827bf2 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""444dbb6f-42d9-4df1-aef4-6ba9e1827bf2"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 156.0,
  ""label"": ""Oman"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: fdaf223e-051b-4d3a-bc55-270c3e42b8f4 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""fdaf223e-051b-4d3a-bc55-270c3e42b8f4"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 157.0,
  ""label"": ""Pakistan"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 8890b5dd-d60c-4682-b79d-fff539a4000f (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""8890b5dd-d60c-4682-b79d-fff539a4000f"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 158.0,
  ""label"": ""Palau"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7bcc9884-0d52-4d5c-ae6b-b79219f65673 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7bcc9884-0d52-4d5c-ae6b-b79219f65673"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 159.0,
  ""label"": ""Panama"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 4b539516-8695-4c1f-9905-91402f4419eb (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""4b539516-8695-4c1f-9905-91402f4419eb"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 160.0,
  ""label"": ""Papua New Guinea"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: e0baecff-afbf-48df-aa65-38507216b2bf (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""e0baecff-afbf-48df-aa65-38507216b2bf"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 161.0,
  ""label"": ""Paraguay"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: e3e8c386-ba59-443d-886b-458009e75abe (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""e3e8c386-ba59-443d-886b-458009e75abe"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 162.0,
  ""label"": ""Peru"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ebff2d0e-8293-4094-ab6b-d0c0a6f14f04 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ebff2d0e-8293-4094-ab6b-d0c0a6f14f04"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 163.0,
  ""label"": ""Philippines"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 97f1aa67-0b70-4f55-931f-abb59d612beb (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""97f1aa67-0b70-4f55-931f-abb59d612beb"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 164.0,
  ""label"": ""Pohnpei"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a243eb96-6678-4ccf-88fd-f23567261114 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a243eb96-6678-4ccf-88fd-f23567261114"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 165.0,
  ""label"": ""Poland"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 3212a114-0264-4b26-b9f5-19fa4e1681b3 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""3212a114-0264-4b26-b9f5-19fa4e1681b3"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 166.0,
  ""label"": ""Portugal"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ed65bedf-4dd1-4c71-9c00-dd37302a75d6 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ed65bedf-4dd1-4c71-9c00-dd37302a75d6"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 167.0,
  ""label"": ""Puerto Rico"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 707a74ea-16c9-47f5-807f-19a833f5ddf3 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""707a74ea-16c9-47f5-807f-19a833f5ddf3"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 168.0,
  ""label"": ""Qatar"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 58360fa1-1b8e-4570-85af-e8e965fc74de (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""58360fa1-1b8e-4570-85af-e8e965fc74de"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 169.0,
  ""label"": ""Reunion"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 3d2269d1-342a-4586-ad21-f0c53a83478c (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""3d2269d1-342a-4586-ad21-f0c53a83478c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 170.0,
  ""label"": ""Romania"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7af589cc-495e-4744-a2f2-e70662aec5d1 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7af589cc-495e-4744-a2f2-e70662aec5d1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 171.0,
  ""label"": ""Rota"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: f81e060b-150a-4f8e-b4e8-e53f1579d3f9 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""f81e060b-150a-4f8e-b4e8-e53f1579d3f9"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 172.0,
  ""label"": ""Russia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 6c19f7df-0c40-47c1-bcb8-1bc82d9ebca5 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6c19f7df-0c40-47c1-bcb8-1bc82d9ebca5"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 173.0,
  ""label"": ""Rwanda"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: cc790c14-ce65-478b-9407-852e3cc37cdc (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""cc790c14-ce65-478b-9407-852e3cc37cdc"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 174.0,
  ""label"": ""Saba"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 293bf16a-792e-4d21-a5f2-cbc621547df4 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""293bf16a-792e-4d21-a5f2-cbc621547df4"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 175.0,
  ""label"": ""Saint Barthelemy"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 23599d38-7003-4a9c-9a35-a1523b8e794f (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""23599d38-7003-4a9c-9a35-a1523b8e794f"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 176.0,
  ""label"": ""Saint Christopher"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 8381ab15-2178-46e2-8ca1-6e4b687e23ea (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""8381ab15-2178-46e2-8ca1-6e4b687e23ea"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 177.0,
  ""label"": ""Saint John"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 8c47b95d-5982-459b-a38a-97ae24e59457 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""8c47b95d-5982-459b-a38a-97ae24e59457"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 178.0,
  ""label"": ""Saint Kitts and Nevis"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 4010d7e9-1cc7-454a-bdc0-bac967a6ec6a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""4010d7e9-1cc7-454a-bdc0-bac967a6ec6a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 179.0,
  ""label"": ""Saint Lucia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 0a12f116-71fb-4623-a732-7292120848d3 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""0a12f116-71fb-4623-a732-7292120848d3"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 180.0,
  ""label"": ""Saint Martin"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 79175111-4650-4c9e-8656-6caafcfb0b96 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""79175111-4650-4c9e-8656-6caafcfb0b96"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 181.0,
  ""label"": ""Saint Thomas"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 0b68ba8a-4048-47a1-a50c-e78740abdae3 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""0b68ba8a-4048-47a1-a50c-e78740abdae3"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 182.0,
  ""label"": ""Saint Vincent and the Grenadines"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2b57742b-8d76-4395-887f-b187491be6b1 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2b57742b-8d76-4395-887f-b187491be6b1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 183.0,
  ""label"": ""Saipan"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 6708ab17-62de-4c88-9b64-86f7d8bea0be (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6708ab17-62de-4c88-9b64-86f7d8bea0be"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 184.0,
  ""label"": ""Samoa"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a4321928-40fe-40a4-a89c-25afe7e38482 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a4321928-40fe-40a4-a89c-25afe7e38482"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 185.0,
  ""label"": ""San Marino"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 77d2ade7-5ad6-4c58-b9f9-399756d0897e (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""77d2ade7-5ad6-4c58-b9f9-399756d0897e"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 186.0,
  ""label"": ""Santa Cruz"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ced8ffdb-e052-4c14-bc15-a7a8f734e0ba (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ced8ffdb-e052-4c14-bc15-a7a8f734e0ba"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 187.0,
  ""label"": ""Saudi Arabia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 43311d42-4ea5-4495-b4ef-cd19338d8437 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""43311d42-4ea5-4495-b4ef-cd19338d8437"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 188.0,
  ""label"": ""Scotland"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: d6501b98-2428-4ef4-9a37-4421987bb622 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""d6501b98-2428-4ef4-9a37-4421987bb622"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 189.0,
  ""label"": ""Senegal"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 90e716a2-4624-4d3f-a3f0-e5199f19c5b3 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""90e716a2-4624-4d3f-a3f0-e5199f19c5b3"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 190.0,
  ""label"": ""Serbia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 070c72c1-f4c7-4e9c-b11d-5ff813d458e6 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""070c72c1-f4c7-4e9c-b11d-5ff813d458e6"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 191.0,
  ""label"": ""Seychelles"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 0b680a36-f887-491d-a541-90b07cda3802 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""0b680a36-f887-491d-a541-90b07cda3802"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 192.0,
  ""label"": ""Sierra Leone"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: bd2f8d60-b47d-4339-a06e-93c42e2b1255 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""bd2f8d60-b47d-4339-a06e-93c42e2b1255"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 193.0,
  ""label"": ""Singapore"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: f22662d9-d99d-432d-b1be-b798a2f410e4 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""f22662d9-d99d-432d-b1be-b798a2f410e4"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 194.0,
  ""label"": ""Sint Eustatius"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 0c8f6b1d-5074-4396-ac98-abbb7e76edfe (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""0c8f6b1d-5074-4396-ac98-abbb7e76edfe"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 195.0,
  ""label"": ""Sint Maarten"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c69d56b6-e02d-4d8c-8016-1fe8a53e6449 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c69d56b6-e02d-4d8c-8016-1fe8a53e6449"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 196.0,
  ""label"": ""Slovakia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7a9baa4e-bd43-44c6-bf74-93f511578e89 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7a9baa4e-bd43-44c6-bf74-93f511578e89"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 197.0,
  ""label"": ""Slovenia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: e7f19e13-1638-4a62-8954-07f5a1b0b8c2 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""e7f19e13-1638-4a62-8954-07f5a1b0b8c2"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 198.0,
  ""label"": ""Solomon Islands"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 6c99f5a0-f678-47ab-bf5e-205ac1ca78b8 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""6c99f5a0-f678-47ab-bf5e-205ac1ca78b8"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 199.0,
  ""label"": ""South Africa"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b26180fb-ecef-4974-a3be-5c89e6566750 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b26180fb-ecef-4974-a3be-5c89e6566750"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 200.0,
  ""label"": ""South Korea"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: dfa79ffb-6890-451a-a95e-39487a545487 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""dfa79ffb-6890-451a-a95e-39487a545487"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 201.0,
  ""label"": ""Spain"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 9be69bf5-85bd-4558-8c3f-bcfce32e7608 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""9be69bf5-85bd-4558-8c3f-bcfce32e7608"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 202.0,
  ""label"": ""Sri Lanka"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: bbbaa690-7d07-4c66-8bca-1ceacdb4d521 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""bbbaa690-7d07-4c66-8bca-1ceacdb4d521"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 203.0,
  ""label"": ""Suriname"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 5fd93690-4149-4d4b-ad93-bf9459be5d69 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""5fd93690-4149-4d4b-ad93-bf9459be5d69"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 204.0,
  ""label"": ""Swaziland"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 216747a4-7187-4e9b-9e20-7486ceba08fc (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""216747a4-7187-4e9b-9e20-7486ceba08fc"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 205.0,
  ""label"": ""Sweden"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 94c16262-4fa4-4d72-997b-68938ebd001d (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""94c16262-4fa4-4d72-997b-68938ebd001d"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 206.0,
  ""label"": ""Switzerland"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a842b53f-bbfd-45a6-9ffb-1c5846038bd4 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a842b53f-bbfd-45a6-9ffb-1c5846038bd4"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 207.0,
  ""label"": ""Tahiti"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 8d32c793-b1f5-4e97-b774-a1d82b1486b2 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""8d32c793-b1f5-4e97-b774-a1d82b1486b2"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 208.0,
  ""label"": ""Taiwan"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 0c9048c0-d4c2-45f1-beca-6be23b52baf4 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""0c9048c0-d4c2-45f1-beca-6be23b52baf4"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 209.0,
  ""label"": ""Tajikistan"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 54d9d320-a4ea-482a-8e43-2688eca58379 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""54d9d320-a4ea-482a-8e43-2688eca58379"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 210.0,
  ""label"": ""Tanzania"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: bf1d8ab1-692d-433f-8fbf-bb711afe5713 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""bf1d8ab1-692d-433f-8fbf-bb711afe5713"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 211.0,
  ""label"": ""Thailand"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 239907ff-4b2e-4002-8b92-91a6f1d2f091 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""239907ff-4b2e-4002-8b92-91a6f1d2f091"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 212.0,
  ""label"": ""Tinian"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 05e52310-bdf4-4c25-b9fc-8702e3e9eb9c (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""05e52310-bdf4-4c25-b9fc-8702e3e9eb9c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 213.0,
  ""label"": ""Togo"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 97f22041-b11b-4ac6-ba28-8299bf3bf770 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""97f22041-b11b-4ac6-ba28-8299bf3bf770"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 214.0,
  ""label"": ""Tonga"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 080f010a-89f5-4177-8402-748bf44b3368 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""080f010a-89f5-4177-8402-748bf44b3368"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 215.0,
  ""label"": ""Tortola"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ff4fd84f-9e06-44f4-b055-2a72395e994e (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ff4fd84f-9e06-44f4-b055-2a72395e994e"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 216.0,
  ""label"": ""Trinidad and Tobago"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c3b07a4b-5a29-403e-84ea-46bbab34ba6f (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c3b07a4b-5a29-403e-84ea-46bbab34ba6f"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 217.0,
  ""label"": ""Tunisia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2562b235-ca58-4e71-8057-85bd4682f38c (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2562b235-ca58-4e71-8057-85bd4682f38c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 218.0,
  ""label"": ""Turkey"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 1921e1c3-c848-4d15-b61b-413a2ddd58f1 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""1921e1c3-c848-4d15-b61b-413a2ddd58f1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 219.0,
  ""label"": ""Turkmenistan"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 961ca601-43e4-4213-9fe3-08f019b843af (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""961ca601-43e4-4213-9fe3-08f019b843af"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 220.0,
  ""label"": ""Turks and Caicos Islands"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2560b9e9-3e16-495c-92c4-bce079397899 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2560b9e9-3e16-495c-92c4-bce079397899"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 221.0,
  ""label"": ""Tuvalu"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b4d711cc-133b-4416-b437-75bc56edb893 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b4d711cc-133b-4416-b437-75bc56edb893"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 222.0,
  ""label"": ""Uganda"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a6cc78fc-bb5c-4b69-af69-aacd3b23fa40 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a6cc78fc-bb5c-4b69-af69-aacd3b23fa40"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 223.0,
  ""label"": ""Ukraine"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 0c189a67-1952-46d7-8f1a-63e16559c095 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""0c189a67-1952-46d7-8f1a-63e16559c095"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 224.0,
  ""label"": ""Union Island"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2229431a-9dbd-481e-b818-7c1dc39e5c5b (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2229431a-9dbd-481e-b818-7c1dc39e5c5b"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 225.0,
  ""label"": ""United Arab Emirates"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 1e7af9ef-aa19-46ee-b03a-a6667afb1548 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""1e7af9ef-aa19-46ee-b03a-a6667afb1548"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 226.0,
  ""label"": ""United Kingdom"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 8b79fbee-30a4-498e-b4bd-9f97372d984e (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""8b79fbee-30a4-498e-b4bd-9f97372d984e"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 227.0,
  ""label"": ""United States"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ae88e961-bbe3-4c51-9d7f-536943023fdb (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ae88e961-bbe3-4c51-9d7f-536943023fdb"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 228.0,
  ""label"": ""United States Virgin Islands"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7daaa8ae-1dc2-45d3-bdb4-375d3bd05993 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7daaa8ae-1dc2-45d3-bdb4-375d3bd05993"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 229.0,
  ""label"": ""Uruguay"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 697008fb-1ac8-4e35-b285-8ec6f215b83c (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""697008fb-1ac8-4e35-b285-8ec6f215b83c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 230.0,
  ""label"": ""Uzbekistan"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 93d8d077-aa70-4a27-9572-ab229efb4368 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""93d8d077-aa70-4a27-9572-ab229efb4368"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 231.0,
  ""label"": ""Vanuatu"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: d502fe49-c9a7-4f64-80b2-4b08b2c42928 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""d502fe49-c9a7-4f64-80b2-4b08b2c42928"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 232.0,
  ""label"": ""Vatican"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7a69e222-b8ba-448e-9dfc-711d62bba861 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7a69e222-b8ba-448e-9dfc-711d62bba861"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 233.0,
  ""label"": ""Venezuela"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a7ee86a2-bf1f-4619-8694-26e4408c0db6 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a7ee86a2-bf1f-4619-8694-26e4408c0db6"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 234.0,
  ""label"": ""Vietnam"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a1192bb2-1063-40a0-bad0-676d72ec4f20 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a1192bb2-1063-40a0-bad0-676d72ec4f20"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 235.0,
  ""label"": ""Virgin Gorda"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 467b2049-2db4-4ed3-bc30-da5c4815d0d1 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""467b2049-2db4-4ed3-bc30-da5c4815d0d1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 236.0,
  ""label"": ""Wales"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ec2e72b3-964c-41ec-90b5-6454a23e018e (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ec2e72b3-964c-41ec-90b5-6454a23e018e"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 237.0,
  ""label"": ""Wallis and Futuna"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 79afe36e-2271-49ea-a48e-f7f83ab9f300 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""79afe36e-2271-49ea-a48e-f7f83ab9f300"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 238.0,
  ""label"": ""Yap"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 3726c375-fd31-4183-9b49-0b2555fa8c9e (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""3726c375-fd31-4183-9b49-0b2555fa8c9e"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 239.0,
  ""label"": ""Yemen"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ea156f50-2329-4896-8857-7e10726edecc (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ea156f50-2329-4896-8857-7e10726edecc"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 240.0,
  ""label"": ""Zambia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 155fd4c8-f8f4-4cfa-8e49-636b355cdd2f (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""155fd4c8-f8f4-4cfa-8e49-636b355cdd2f"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 241.0,
  ""label"": ""Zimbabwe"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: e02c45fd-ed89-48ad-992e-5629b27b471c (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""e02c45fd-ed89-48ad-992e-5629b27b471c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 11.0,
  ""label"": ""Australia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: d0e295a0-ac6f-41ea-83a8-4844334cb22b (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""d0e295a0-ac6f-41ea-83a8-4844334cb22b"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 14.0,
  ""label"": ""Azores"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: de2cead2-c917-47b4-88f6-49b4abadbac3 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""de2cead2-c917-47b4-88f6-49b4abadbac3"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 17.0,
  ""label"": ""Bangladesh"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: fd6c108c-cdb4-4e43-a623-6a646f6db60e (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""fd6c108c-cdb4-4e43-a623-6a646f6db60e"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 21.0,
  ""label"": ""Belize"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b3f56d82-dd6c-45e4-9b05-5baf2b3b69f0 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b3f56d82-dd6c-45e4-9b05-5baf2b3b69f0"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 28.0,
  ""label"": ""Botswana"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c1bbaefd-3817-417b-91a9-cb91b4bdaa57 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c1bbaefd-3817-417b-91a9-cb91b4bdaa57"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 32.0,
  ""label"": ""Bulgaria"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: e52c5d59-782c-4b4a-864b-77b551b6f834 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""e52c5d59-782c-4b4a-864b-77b551b6f834"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 40.0,
  ""label"": ""Cayman Islands"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c3de7db2-0e31-4c44-9eec-1c95122f99c6 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c3de7db2-0e31-4c44-9eec-1c95122f99c6"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 42.0,
  ""label"": ""Chad"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b561b5c5-a9f7-4aa7-81c6-a6395dfb7a00 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b561b5c5-a9f7-4aa7-81c6-a6395dfb7a00"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 43.0,
  ""label"": ""Chile"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: cd671675-150b-4b76-bb1e-0250afd18a11 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""cd671675-150b-4b76-bb1e-0250afd18a11"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 45.0,
  ""label"": ""Chuuk"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: d2642961-62be-442b-a7f6-ceb4cc3b8e4a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""d2642961-62be-442b-a7f6-ceb4cc3b8e4a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 50.0,
  ""label"": ""Cook Islands"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c3b6e74d-d8d8-48df-80ea-77409c373d14 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c3b6e74d-d8d8-48df-80ea-77409c373d14"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 52.0,
  ""label"": ""Cote d'Ivoire"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ed692228-0c32-4de9-8405-8bf72a7ea31a (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ed692228-0c32-4de9-8405-8bf72a7ea31a"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 53.0,
  ""label"": ""Croatia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: e70b20b1-d9d3-451f-a89f-38f30f2ab277 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""e70b20b1-d9d3-451f-a89f-38f30f2ab277"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 54.0,
  ""label"": ""Curacao"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: bc9ff27d-071c-4fa3-8bec-3439d5c5293d (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""bc9ff27d-071c-4fa3-8bec-3439d5c5293d"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 55.0,
  ""label"": ""Cyprus"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: efef0aec-48a7-46ba-8ae0-9cd898ded2f5 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""efef0aec-48a7-46ba-8ae0-9cd898ded2f5"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 56.0,
  ""label"": ""Czech Republic"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: e8b59913-2abb-4585-a65d-8e27e04c9d1c (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""e8b59913-2abb-4585-a65d-8e27e04c9d1c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 57.0,
  ""label"": ""Denmark"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: eaa6a054-71c7-4604-a889-707f066b93ca (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""eaa6a054-71c7-4604-a889-707f066b93ca"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 59.0,
  ""label"": ""Dominica"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: f7b9b3e4-c9bb-48a8-9c41-43d72e5df03d (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""f7b9b3e4-c9bb-48a8-9c41-43d72e5df03d"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 62.0,
  ""label"": ""Ecuador"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: cd71c428-84c9-4c29-954d-1cbbf270425f (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""cd71c428-84c9-4c29-954d-1cbbf270425f"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 63.0,
  ""label"": ""Egypt"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: e0f15e6c-ce5b-4833-8609-7b85e88ef4fd (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""e0f15e6c-ce5b-4833-8609-7b85e88ef4fd"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 64.0,
  ""label"": ""El Salvador"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: cc54a3a1-9bd0-4088-b99c-002be029a412 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""cc54a3a1-9bd0-4088-b99c-002be029a412"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 66.0,
  ""label"": ""Equatorial Guinea"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: d2c5be1c-be3f-4b9d-9f9e-3c18a37c6f54 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""d2c5be1c-be3f-4b9d-9f9e-3c18a37c6f54"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 74.0,
  ""label"": ""French Guiana"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c758672a-e2a1-41c0-acc2-c40d696c3259 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c758672a-e2a1-41c0-acc2-c40d696c3259"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 75.0,
  ""label"": ""French Polynesia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: f8689427-eb35-4184-a701-b42053188919 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""f8689427-eb35-4184-a701-b42053188919"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 80.0,
  ""label"": ""Ghana"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: f2fb06a8-f319-42fd-b8cb-3115cfbc23cf (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""f2fb06a8-f319-42fd-b8cb-3115cfbc23cf"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 84.0,
  ""label"": ""Grenada"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c6e21a6d-afcf-4257-afee-038ce1f3a4bf (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c6e21a6d-afcf-4257-afee-038ce1f3a4bf"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 85.0,
  ""label"": ""Guadeloupe"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b07e7e65-58c9-44e4-ac22-d87e9debc0ae (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b07e7e65-58c9-44e4-ac22-d87e9debc0ae"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 86.0,
  ""label"": ""Guam"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: f3273a3f-0ef6-48a1-90b6-785cb14ac4cf (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""f3273a3f-0ef6-48a1-90b6-785cb14ac4cf"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 94.0,
  ""label"": ""Hong Kong"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c53ee60f-4686-4f03-8100-cd082e6bbbac (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c53ee60f-4686-4f03-8100-cd082e6bbbac"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 102.0,
  ""label"": ""Italy"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: bc43e849-e2b2-4c3b-8bbb-4a261c381552 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""bc43e849-e2b2-4c3b-8bbb-4a261c381552"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 103.0,
  ""label"": ""Jamaica"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: d69e127d-cf29-43da-8178-616dfd8d4777 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""d69e127d-cf29-43da-8178-616dfd8d4777"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 105.0,
  ""label"": ""Jersey"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: bb106c93-3e1d-44f2-8900-2c06190b86d8 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""bb106c93-3e1d-44f2-8900-2c06190b86d8"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 108.0,
  ""label"": ""Kenya"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: efff78aa-dbd3-407c-9e28-64c6d75b403e (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""efff78aa-dbd3-407c-9e28-64c6d75b403e"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 109.0,
  ""label"": ""Kiribati"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: e0e2a617-f9dc-4eeb-9e17-f6ebea52207b (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""e0e2a617-f9dc-4eeb-9e17-f6ebea52207b"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 112.0,
  ""label"": ""Kyrgyzstan"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: d7b03028-d3c1-42c9-b0c6-85c8ed865215 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""d7b03028-d3c1-42c9-b0c6-85c8ed865215"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 113.0,
  ""label"": ""Laos"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b0753abe-0176-4f5d-9c60-a308b0722822 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b0753abe-0176-4f5d-9c60-a308b0722822"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 116.0,
  ""label"": ""Lesotho"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: e84b63f4-783b-46e3-b8da-179e1ea0d360 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""e84b63f4-783b-46e3-b8da-179e1ea0d360"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 117.0,
  ""label"": ""Liberia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: feb76ff7-755e-4925-84b4-008fb92262b6 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""feb76ff7-755e-4925-84b4-008fb92262b6"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 122.0,
  ""label"": ""Macedonia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: da3b80b9-de3e-4323-95ba-2b1e7bd886fc (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""da3b80b9-de3e-4323-95ba-2b1e7bd886fc"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 123.0,
  ""label"": ""Madagascar"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b52c4958-6b8f-4d41-8474-1c061b7de80f (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b52c4958-6b8f-4d41-8474-1c061b7de80f"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 125.0,
  ""label"": ""Malawi"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b1380313-3531-4e80-a19d-3ae0ef0e2ba0 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b1380313-3531-4e80-a19d-3ae0ef0e2ba0"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 128.0,
  ""label"": ""Mali"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: efdc8eab-d628-43fa-b8fd-364ec19d0db6 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""efdc8eab-d628-43fa-b8fd-364ec19d0db6"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 129.0,
  ""label"": ""Malta"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: d20d1956-53cd-47e4-a4be-1af5ef0435c1 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""d20d1956-53cd-47e4-a4be-1af5ef0435c1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 135.0,
  ""label"": ""Micronesia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b582b6d3-c9c6-4d5b-85df-b0cd2ad1cde5 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b582b6d3-c9c6-4d5b-85df-b0cd2ad1cde5"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 137.0,
  ""label"": ""Monaco"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: d92382ef-3b8d-46ba-abad-fd64c6e9d6ec (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""d92382ef-3b8d-46ba-abad-fd64c6e9d6ec"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 138.0,
  ""label"": ""Mongolia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: bac1d6a7-d012-42aa-b77f-cc449de7a708 (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""bac1d6a7-d012-42aa-b77f-cc449de7a708"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 142.0,
  ""label"": ""Mozambique"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c6bbd80d-cc07-423d-bb73-c093a1c063bc (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c6bbd80d-cc07-423d-bb73-c093a1c063bc"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 143.0,
  ""label"": ""Namibia"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: fa670dcf-4f1d-4444-b94f-e14bf91ca22c (country) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""fa670dcf-4f1d-4444-b94f-e14bf91ca22c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": null,
  ""sort_index"": 144.0,
  ""label"": ""Nepal"",
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("country", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 991ac1a3-1488-4721-ba1d-e31602d2259c (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""991ac1a3-1488-4721-ba1d-e31602d2259c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Agriculture"",
  ""sort_index"": 1.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2dedd5cf-f7ba-4c60-a8a0-24b877254f6d (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2dedd5cf-f7ba-4c60-a8a0-24b877254f6d"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Apparel"",
  ""sort_index"": 2.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 57387434-69f1-4412-81d5-cfc78accb136 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""57387434-69f1-4412-81d5-cfc78accb136"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Banking"",
  ""sort_index"": 3.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b3f98678-054a-42c3-8417-461a36432cbb (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b3f98678-054a-42c3-8417-461a36432cbb"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Biotechnology"",
  ""sort_index"": 4.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 5fdf025f-3f0f-422b-8c01-0e836a244cb1 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""5fdf025f-3f0f-422b-8c01-0e836a244cb1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Chemicals"",
  ""sort_index"": 5.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ef119a92-0aee-455c-aca0-6dd511f94311 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ef119a92-0aee-455c-aca0-6dd511f94311"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Communications"",
  ""sort_index"": 6.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7651b55b-acd6-48c1-8cb4-a23f1abf5aca (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7651b55b-acd6-48c1-8cb4-a23f1abf5aca"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Construction"",
  ""sort_index"": 7.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 86db7d49-31e7-4a25-a1c5-f738c02c603b (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""86db7d49-31e7-4a25-a1c5-f738c02c603b"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Consulting"",
  ""sort_index"": 8.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ea52bba9-8215-4103-a7b0-a6eb0c5e99ff (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ea52bba9-8215-4103-a7b0-a6eb0c5e99ff"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Education"",
  ""sort_index"": 9.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 86b7d188-9595-4e38-bf00-c2c6754657f6 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""86b7d188-9595-4e38-bf00-c2c6754657f6"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Electronics"",
  ""sort_index"": 10.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: cf54ccbc-1334-49d4-a51d-159b33dbc6b4 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""cf54ccbc-1334-49d4-a51d-159b33dbc6b4"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Energy"",
  ""sort_index"": 11.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 532204c0-ed8b-44b2-80fa-c582d38e3218 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""532204c0-ed8b-44b2-80fa-c582d38e3218"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Engineering"",
  ""sort_index"": 12.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 37714bd5-2f00-4211-a13d-bb78f5d71263 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""37714bd5-2f00-4211-a13d-bb78f5d71263"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Entertainment"",
  ""sort_index"": 13.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: c0f0ae79-5ec2-436f-ab80-07986ca7a7e0 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""c0f0ae79-5ec2-436f-ab80-07986ca7a7e0"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Environmental"",
  ""sort_index"": 14.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 30cd82e0-7392-45ba-8cf5-7346eb7af733 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""30cd82e0-7392-45ba-8cf5-7346eb7af733"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Finance"",
  ""sort_index"": 15.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 068b7b08-de54-4628-bc54-b2fe614a42ba (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""068b7b08-de54-4628-bc54-b2fe614a42ba"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Food & Beverage"",
  ""sort_index"": 16.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: e91a880a-ee18-4a3d-b23c-8ea29a02b3f7 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""e91a880a-ee18-4a3d-b23c-8ea29a02b3f7"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Government"",
  ""sort_index"": 17.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: d4373f58-0427-4d6d-90dc-ceb62a11fef8 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""d4373f58-0427-4d6d-90dc-ceb62a11fef8"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Healthcare"",
  ""sort_index"": 18.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 9c3e18f9-e95e-4af5-b1db-60a610b3c64e (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""9c3e18f9-e95e-4af5-b1db-60a610b3c64e"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Hospitality"",
  ""sort_index"": 19.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b1756fdc-055e-4df3-909a-594c586495c5 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b1756fdc-055e-4df3-909a-594c586495c5"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Insurance"",
  ""sort_index"": 20.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 2890c7f0-b213-41f1-9bf4-a3d93df9d727 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""2890c7f0-b213-41f1-9bf4-a3d93df9d727"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Machinery"",
  ""sort_index"": 21.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a557bb08-e5f6-46aa-a848-b5faa6d3e644 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a557bb08-e5f6-46aa-a848-b5faa6d3e644"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Manufacturing"",
  ""sort_index"": 22.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b22f8247-15e7-4e4b-bbd2-8c2c62dfee09 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b22f8247-15e7-4e4b-bbd2-8c2c62dfee09"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Media"",
  ""sort_index"": 23.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 904b50d2-ef93-442e-a5e8-92a690d0b8bd (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""904b50d2-ef93-442e-a5e8-92a690d0b8bd"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Not for Profit"",
  ""sort_index"": 24.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ad4991cd-a3a1-4e4e-9046-71700e2a5bfb (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ad4991cd-a3a1-4e4e-9046-71700e2a5bfb"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Recreation"",
  ""sort_index"": 25.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: cc7549f8-a583-4da9-875b-c81617ea6c41 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""cc7549f8-a583-4da9-875b-c81617ea6c41"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Retail"",
  ""sort_index"": 26.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ef55d4fe-0979-49be-be8e-27c57c9cde31 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ef55d4fe-0979-49be-be8e-27c57c9cde31"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Shipping"",
  ""sort_index"": 27.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b5b8af14-c500-40d9-9bb8-76a03e34425c (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b5b8af14-c500-40d9-9bb8-76a03e34425c"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Technology"",
  ""sort_index"": 28.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 23488f45-0108-445d-ad4b-91d2cd516298 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""23488f45-0108-445d-ad4b-91d2cd516298"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Telecommunications"",
  ""sort_index"": 29.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 9caa3931-75e0-43d8-b98e-674e11afae21 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""9caa3931-75e0-43d8-b98e-674e11afae21"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Transportation"",
  ""sort_index"": 30.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 12686d8f-0a19-4721-a7f2-0ab946afc746 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""12686d8f-0a19-4721-a7f2-0ab946afc746"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Utilities"",
  ""sort_index"": 31.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 667251fa-9bcf-4d3f-b538-6b6b3926ca53 (industry) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""667251fa-9bcf-4d3f-b538-6b6b3926ca53"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Other"",
  ""sort_index"": 32.0,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("industry", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 87c08ee1-8d4d-4c89-9b37-4e3cc3f98698 (solutation) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""87c08ee1-8d4d-4c89-9b37-4e3cc3f98698"",
  ""is_default"": true,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Mr."",
  ""sort_index"": 1.0
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("solutation", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 0ede7d96-2d85-45fa-818b-01327d4c47a9 (solutation) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""0ede7d96-2d85-45fa-818b-01327d4c47a9"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Ms."",
  ""sort_index"": 2.0
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("solutation", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ab073457-ddc8-4d36-84a5-38619528b578 (solutation) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ab073457-ddc8-4d36-84a5-38619528b578"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Mrs."",
  ""sort_index"": 3.0
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("solutation", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 5b8d0137-9ec5-4b1c-a9b0-e982ef8698c1 (solutation) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""5b8d0137-9ec5-4b1c-a9b0-e982ef8698c1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Dr."",
  ""sort_index"": 4.0
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("solutation", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a74cd934-b425-4061-8f4e-a6d6b9d7adb1 (solutation) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a74cd934-b425-4061-8f4e-a6d6b9d7adb1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Prof."",
  ""sort_index"": 5.0
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("solutation", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: f3fdd750-0c16-4215-93b3-5373bd528d1f (task_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""f3fdd750-0c16-4215-93b3-5373bd528d1f"",
  ""is_closed"": false,
  ""is_default"": true,
  ""l_scope"": """",
  ""label"": ""Not Started"",
  ""sort_index"": 1.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("task_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 20d73f63-3501-4565-a55e-2d291549a9bd (task_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""20d73f63-3501-4565-a55e-2d291549a9bd"",
  ""is_closed"": false,
  ""is_default"": false,
  ""l_scope"": """",
  ""label"": ""In Progress"",
  ""sort_index"": 2.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("task_status", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: b1cc69e5-ce09-40e0-8785-b6452b257bdf (task_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""b1cc69e5-ce09-40e0-8785-b6452b257bdf"",
  ""is_closed"": true,
  ""is_default"": false,
  ""l_scope"": """",
  ""label"": ""Completed"",
  ""sort_index"": 4.0,
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

			#region << ***Create record*** Id: 8b2aa2af-17dd-400a-a221-78ee744c4866 (task_status) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""8b2aa2af-17dd-400a-a221-78ee744c4866"",
  ""is_closed"": false,
  ""is_default"": false,
  ""l_scope"": """",
  ""label"": ""Blocked"",
  ""sort_index"": 3.0,
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

			#region << ***Create record*** Id: da9bf72d-3655-4c51-9f99-047ef9297bf2 (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""da9bf72d-3655-4c51-9f99-047ef9297bf2"",
  ""is_default"": true,
  ""l_scope"": """",
  ""label"": ""General"",
  ""sort_index"": 1.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 7b191135-5fbb-4db9-bf24-1a5fc72d8cd5 (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""7b191135-5fbb-4db9-bf24-1a5fc72d8cd5"",
  ""is_default"": false,
  ""l_scope"": """",
  ""label"": ""Call"",
  ""sort_index"": 2.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 489b16e1-91b1-4a05-b247-50ed74f7aaaf (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""489b16e1-91b1-4a05-b247-50ed74f7aaaf"",
  ""is_default"": false,
  ""l_scope"": """",
  ""label"": ""Email"",
  ""sort_index"": 3.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 894ba1ef-1b31-440c-9b33-f301d047d8fb (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""894ba1ef-1b31-440c-9b33-f301d047d8fb"",
  ""is_default"": false,
  ""l_scope"": """",
  ""label"": ""Meeting"",
  ""sort_index"": 4.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ddb9c170-706d-4b17-a8ee-78ed3a544fa3 (task_type) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ddb9c170-706d-4b17-a8ee-78ed3a544fa3"",
  ""is_default"": false,
  ""l_scope"": """",
  ""label"": ""Send Quote"",
  ""sort_index"": 5.0,
  ""is_system"": true,
  ""is_enabled"": true,
  ""icon_class"": null,
  ""color"": null
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("task_type", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

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
