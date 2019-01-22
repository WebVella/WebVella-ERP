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

			#region << ***Create field***  Entity: task Field Name: owner_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("aa486ab3-5510-4373-90b9-5285a6c6468f");
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
					var response = entMan.CreateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: owner_id Message:" + response.Message);
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

			#region << ***Create field***  Entity: task Field Name: type_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("955ed90c-c158-4423-a766-33646ce1d7e7");
				guidField.Name = "type_id";
				guidField.Label = "Type Id";
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
		new SelectOption() { Label = "project_hour_cost", Value = "project_hour_cost", IconClass = "", Color = ""},
		new SelectOption() { Label = "user_hour_cost", Value = "user_hour_cost", IconClass = "", Color = ""}
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

			#region << ***Create app*** App name: crm >>
			{
				var id = new Guid("45291b1a-a590-4633-b0bb-11e97c179e91");
				var name = "crm";
				var label = "Customer Relationship Management";
				var description = "Accounts";
				var iconClass = "fas fa-user-tie";
				var author = "WebVella";
				var color = "#2196F3";
				var weight = 20;
				var access = new List<Guid>();

				new WebVella.Erp.Web.Services.AppService().CreateApplication(id, name, label, description, iconClass, author, color, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create app*** App name: projects >>
			{
				var id = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "projects";
				var label = "Projects";
				var description = "Project management, task and time accounting";
				var iconClass = "fa fa-code";
				var author = "WebVella";
				var color = "#9c27b0";
				var weight = 1;
				var access = new List<Guid>();
				access.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));

				new WebVella.Erp.Web.Services.AppService().CreateApplication(id, name, label, description, iconClass, author, color, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Update sitemap area*** Sitemap area name: objects >>
			{
				var id = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
				var appId = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
				var name = "objects";
				var label = "Objects";
				var description = @"Schema and Layout management";
				var iconClass = "ti-ruler-pencil";
				var color = "#2196F3";
				var weight = 1;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().UpdateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: accounts >>
			{
				var id = new Guid("a0ef4c2d-b837-4428-b726-0de89ea19867");
				var appId = new Guid("45291b1a-a590-4633-b0bb-11e97c179e91");
				var name = "accounts";
				var label = "Accounts";
				var description = @"";
				var iconClass = "fas fa-user-tie";
				var color = "#2196F3";
				var weight = 1;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: dashboard >>
			{
				var id = new Guid("d99e07df-b5f3-4a01-8506-b607c3389308");
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "dashboard";
				var label = "Dashboard";
				var description = @"";
				var iconClass = "fas fa-tachometer-alt";
				var color = "#9C27B0";
				var weight = 1;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: track-time >>
			{
				var id = new Guid("fe9ac91f-a52f-4127-a74b-c4b335930c1d");
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "track-time";
				var label = "Track Time";
				var description = @"User time track";
				var iconClass = "fas fa-clock";
				var color = "#9C27B0";
				var weight = 2;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: feed >>
			{
				var id = new Guid("24028a64-748b-43a2-98ae-47514da142fe");
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "feed";
				var label = "Feed";
				var description = @"";
				var iconClass = "fas fa-rss-square";
				var color = "#9C27B0";
				var weight = 3;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: projects >>
			{
				var id = new Guid("dadd2bb1-459b-48da-a798-f2eea579c4e5");
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "projects";
				var label = "Projects";
				var description = @"";
				var iconClass = "fa fa-cogs";
				var color = "#9C27B0";
				var weight = 4;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: tasks >>
			{
				var id = new Guid("9aacb1b4-c03d-44bb-8d79-554971f4a25c");
				var appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				var name = "tasks";
				var label = "Tasks";
				var description = @"";
				var iconClass = "fas fa-check-double";
				var color = "#9C27B0";
				var weight = 5;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: accounts >>
			{
				var id = new Guid("c087059c-7ce2-4c8e-be28-265f05ac3d0f");
				var areaId = new Guid("a0ef4c2d-b837-4428-b726-0de89ea19867");
				Guid? entityId = new Guid("2e22b50f-e444-4b62-a171-076e51246939");
				var name = "accounts";
				var label = "Accounts";
				var url = "";
				var iconClass = "fas fa-user-tie";
				var weight = 1;
				var type = ((int)1);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: comments >>
			{
				var id = new Guid("fa94b569-67f2-4805-97c3-517ab52fb4d9");
				var areaId = new Guid("a0ef4c2d-b837-4428-b726-0de89ea19867");
				Guid? entityId = new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d");
				var name = "comments";
				var label = "Comments";
				var url = "";
				var iconClass = "";
				var weight = 1;
				var type = ((int)1);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: dashboard >>
			{
				var id = new Guid("3edb7097-a998-4e2e-9ba0-716f0767ce35");
				var areaId = new Guid("d99e07df-b5f3-4a01-8506-b607c3389308");
				Guid? entityId = null;
				var name = "dashboard";
				var label = "Dashboard";
				var url = "/projects/dashboard/dashboard/a";
				var iconClass = "fas fa-tachometer-alt";
				var weight = 1;
				var type = ((int)2);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: track-time >>
			{
				var id = new Guid("8c27983c-d215-48ad-9e73-49fd4e8acdb8");
				var areaId = new Guid("fe9ac91f-a52f-4127-a74b-c4b335930c1d");
				Guid? entityId = null;
				var name = "track-time";
				var label = "Track Time";
				var url = "";
				var iconClass = "fas fa-clock";
				var weight = 1;
				var type = ((int)2);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: feed >>
			{
				var id = new Guid("8950c6c6-7848-4a0b-b260-e8dbedf7486c");
				var areaId = new Guid("24028a64-748b-43a2-98ae-47514da142fe");
				Guid? entityId = null;
				var name = "feed";
				var label = "Feed";
				var url = "/projects/feed/feed/a";
				var iconClass = "fas fa-rss-square";
				var weight = 1;
				var type = ((int)2);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: projects >>
			{
				var id = new Guid("48200d8b-6b7d-47b5-931c-17033ad8a679");
				var areaId = new Guid("dadd2bb1-459b-48da-a798-f2eea579c4e5");
				Guid? entityId = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				var name = "projects";
				var label = "All projects";
				var url = "";
				var iconClass = "fas fa-list-ul";
				var weight = 2;
				var type = ((int)1);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: tasks >>
			{
				var id = new Guid("dda5c020-c2bd-4f1f-9d8d-447659decc15");
				var areaId = new Guid("9aacb1b4-c03d-44bb-8d79-554971f4a25c");
				Guid? entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				var name = "tasks";
				var label = "All tasks";
				var url = "";
				var iconClass = "fas fa-list-ul";
				var weight = 2;
				var type = ((int)1);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: list >>
			{
				var id = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var name = @"list";
				var label = "Accounts List";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("45291b1a-a590-4633-b0bb-11e97c179e91");
				Guid? entityId = new Guid("2e22b50f-e444-4b62-a171-076e51246939");
				Guid? nodeId = new Guid("c087059c-7ce2-4c8e-be28-265f05ac3d0f");
				Guid? areaId = new Guid("a0ef4c2d-b837-4428-b726-0de89ea19867");
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: all >>
			{
				var id = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var name = @"all";
				var label = "All Projects";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: feed >>
			{
				var id = new Guid("acb76466-32b8-428c-81cb-47b6013879e7");
				var name = @"feed";
				var label = "Feed";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)2);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = null;
				Guid? nodeId = new Guid("8950c6c6-7848-4a0b-b260-e8dbedf7486c");
				Guid? areaId = new Guid("24028a64-748b-43a2-98ae-47514da142fe");
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: create >>
			{
				var id = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var name = @"create";
				var label = "Create Account";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)4);
				var isRazorBody = false;
				Guid? appId = new Guid("45291b1a-a590-4633-b0bb-11e97c179e91");
				Guid? entityId = new Guid("2e22b50f-e444-4b62-a171-076e51246939");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: details >>
			{
				var id = new Guid("80b10445-c850-44cf-9c8c-57daca671dcf");
				var name = @"details";
				var label = "Account details";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("45291b1a-a590-4633-b0bb-11e97c179e91");
				Guid? entityId = new Guid("2e22b50f-e444-4b62-a171-076e51246939");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: site-page >>
			{
				var id = new Guid("f3d2caa9-cdfb-4aad-915f-6b48c181938c");
				var name = @"site-page";
				var label = "General Site Page";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)1);
				var isRazorBody = false;
				Guid? appId = null;
				Guid? entityId = null;
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: create >>
			{
				var id = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var name = @"create";
				var label = "Create Project";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)4);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: dashboard >>
			{
				var id = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var name = @"dashboard";
				var label = "My Dashboard";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)2);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = null;
				Guid? nodeId = new Guid("3edb7097-a998-4e2e-9ba0-716f0767ce35");
				Guid? areaId = new Guid("d99e07df-b5f3-4a01-8506-b607c3389308");
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: dashboard >>
			{
				var id = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var name = @"dashboard";
				var label = "Project dashboard";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: details >>
			{
				var id = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var name = @"details";
				var label = "Project details";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: milestones >>
			{
				var id = new Guid("d07cbf70-09c6-47ee-9a13-80568e43d331");
				var name = @"milestones";
				var label = "Project milestones";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: all >>
			{
				var id = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var name = @"all";
				var label = "All tasks";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: tasks >>
			{
				var id = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var name = @"tasks";
				var label = "Project tasks";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 1000;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: create >>
			{
				var id = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var name = @"create";
				var label = "Create task";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)4);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: details >>
			{
				var id = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var name = @"details";
				var label = "Task details";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: comment-test >>
			{
				var id = new Guid("9401d420-808d-475f-a38a-bd34d3763ce9");
				var name = @"comment-test";
				var label = "Comment create";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)4);
				var isRazorBody = false;
				Guid? appId = new Guid("45291b1a-a590-4633-b0bb-11e97c179e91");
				Guid? entityId = new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: feed >>
			{
				var id = new Guid("dfe56667-174d-492d-8f84-b8ab8b70c63f");
				var name = @"feed";
				var label = "Project feed";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 100;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: track-time >>
			{
				var id = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var name = @"track-time";
				var label = "Track Time";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)2);
				var isRazorBody = false;
				Guid? appId = new Guid("652ccabf-d5ad-46d8-aa67-25842537ed4c");
				Guid? entityId = null;
				Guid? nodeId = new Guid("8c27983c-d215-48ad-9e73-49fd4e8acdb8");
				Guid? areaId = new Guid("fe9ac91f-a52f-4127-a74b-c4b335930c1d");
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: f4f2b086-1181-4db5-b78f-51d1b41e1611 >>
			{
				var id = new Guid("f4f2b086-1181-4db5-b78f-51d1b41e1611");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcDrawer";
				var containerId = "";
				var options = @"{
  ""title"": ""Search Tasks"",
  ""width"": ""550px"",
  ""class"": """",
  ""body_class"": """",
  ""title_action_html"": """"
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 7590ab09-b749-4051-935a-b51d16d7b76a >>
			{
				var id = new Guid("7590ab09-b749-4051-935a-b51d16d7b76a");
				Guid? parentId = new Guid("f4f2b086-1181-4db5-b78f-51d1b41e1611");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-7590ab09-b749-4051-935a-b51d16d7b76a"",
  ""name"": ""form"",
  ""hook_key"": """",
  ""method"": ""get"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: df667b11-30ac-4b6b-a12d-41e5aaf6cae5 >>
			{
				var id = new Guid("df667b11-30ac-4b6b-a12d-41e5aaf6cae5");
				Guid? parentId = new Guid("7590ab09-b749-4051-935a-b51d16d7b76a");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Search Tasks"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 57789d88-e897-4b7b-9999-239821db4274 >>
			{
				var id = new Guid("57789d88-e897-4b7b-9999-239821db4274");
				Guid? parentId = new Guid("7590ab09-b749-4051-935a-b51d16d7b76a");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcGridFilterField";
				var containerId = "body";
				var options = @"{
  ""label"": ""Task contents"",
  ""name"": ""x_search"",
  ""try_connect_to_entity"": ""true"",
  ""field_type"": ""18"",
  ""query_type"": ""2"",
  ""query_options"": [
    ""2""
  ],
  ""prefix"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: dedd97f6-1b09-4942-aae1-684cdc49a3eb >>
			{
				var id = new Guid("dedd97f6-1b09-4942-aae1-684cdc49a3eb");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 2,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""Accounts\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""true"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No accounts matching your query"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""true"",
  ""container1_label"": """",
  ""container1_width"": ""20px"",
  ""container1_name"": ""action"",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_searchable"": ""false"",
  ""container2_label"": ""name"",
  ""container2_width"": """",
  ""container2_name"": ""name"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_searchable"": ""false"",
  ""container3_label"": ""column3"",
  ""container3_width"": """",
  ""container3_name"": ""column3"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_searchable"": ""false"",
  ""container4_label"": ""column4"",
  ""container4_width"": """",
  ""container4_name"": ""column4"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_searchable"": ""false"",
  ""container5_label"": ""column5"",
  ""container5_width"": """",
  ""container5_name"": ""column5"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_searchable"": ""false"",
  ""container6_label"": ""column6"",
  ""container6_width"": """",
  ""container6_name"": ""column6"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_searchable"": ""false"",
  ""container7_label"": ""column7"",
  ""container7_width"": """",
  ""container7_name"": ""column7"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_searchable"": ""false"",
  ""container8_label"": ""column8"",
  ""container8_width"": """",
  ""container8_name"": ""column8"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_searchable"": ""false"",
  ""container9_label"": ""column9"",
  ""container9_width"": """",
  ""container9_name"": ""column9"",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_searchable"": ""false"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_searchable"": ""false"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_searchable"": ""false"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_searchable"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: 8f61ba2d-9c8a-434d-9f78-d12926cd80ef >>
			{
				var id = new Guid("8f61ba2d-9c8a-434d-9f78-d12926cd80ef");
				Guid? parentId = new Guid("dedd97f6-1b09-4942-aae1-684cdc49a3eb");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.name\"",\""default\"":\""Account Name\""}"",
  ""name"": ""name"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: c0689f85-235d-484e-bea3-e534e6e10094 >>
			{
				var id = new Guid("c0689f85-235d-484e-bea3-e534e6e10094");
				Guid? parentId = new Guid("dedd97f6-1b09-4942-aae1-684cdc49a3eb");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "column1";
				var options = @"{
  ""type"": ""2"",
  ""text"": """",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-eye"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//replace constants with your values\\n\\t\\tconst string DATASOURCE_NAME = \\\""RowRecord.id\\\"";\\n\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar dataSource = pageModel.TryGetDataSourceProperty<Guid>(DATASOURCE_NAME);\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (dataSource == null)\\n\\t\\t\\treturn null;\\n\\n        \\n\\t\\treturn $\\\""/crm/accounts/accounts/r/{dataSource}\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: 13a1d868-93ee-41d1-bb94-231d99899f74 >>
			{
				var id = new Guid("13a1d868-93ee-41d1-bb94-231d99899f74");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": """",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""Entity.IconName\"",\""default\"":\""\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: 77abedcf-4bea-46f3-b50c-340a7aa237d6 >>
			{
				var id = new Guid("77abedcf-4bea-46f3-b50c-340a7aa237d6");
				Guid? parentId = new Guid("13a1d868-93ee-41d1-bb94-231d99899f74");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""New Customer"",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-plus go-green"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""/crm/accounts/accounts/c"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: b9258d04-360b-426f-b542-ec458f946edf >>
			{
				var id = new Guid("b9258d04-360b-426f-b542-ec458f946edf");
				Guid? parentId = new Guid("13a1d868-93ee-41d1-bb94-231d99899f74");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""Search"",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-search"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH(\""WebVella.Erp.Web.Components.PcDrawer\"",\""open\"")"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: feed  id: e1e493ac-6b74-490f-a0e3-ffd2f2f71f1b >>
			{
				var id = new Guid("e1e493ac-6b74-490f-a0e3-ffd2f2f71f1b");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("acb76466-32b8-428c-81cb-47b6013879e7");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""App.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fas fa-rss-square"",
  ""return_url"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: a584a5ed-96a2-4a28-95e8-23266bc36926 >>
			{
				var id = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 12,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 6,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 12,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 6,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 63daa5c0-ed7f-432e-bfbb-746b94207146 >>
			{
				var id = new Guid("63daa5c0-ed7f-432e-bfbb-746b94207146");
				Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""My Overdue Tasks"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3 "",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: c6bf09c3-fc5e-4e2f-8df8-b96f733b964b >>
			{
				var id = new Guid("c6bf09c3-fc5e-4e2f-8df8-b96f733b964b");
				Guid? parentId = new Guid("63daa5c0-ed7f-432e-bfbb-746b94207146");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 2,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""ProjectWidgetMyOverdueTasks\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""false"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No tasks available"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""false"",
  ""container1_label"": ""name"",
  ""container1_width"": """",
  ""container1_name"": ""name"",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_class"": """",
  ""container2_label"": ""Target Date"",
  ""container2_width"": ""120px"",
  ""container2_name"": ""target_date"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_class"": ""go-red"",
  ""container3_label"": """",
  ""container3_width"": """",
  ""container3_name"": """",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_class"": """",
  ""container4_label"": ""column4"",
  ""container4_width"": """",
  ""container4_name"": ""column4"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_class"": """",
  ""container5_label"": ""column5"",
  ""container5_width"": """",
  ""container5_name"": ""column5"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_class"": """",
  ""container6_label"": ""column6"",
  ""container6_width"": """",
  ""container6_name"": ""column6"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_class"": """",
  ""container7_label"": ""column7"",
  ""container7_width"": """",
  ""container7_name"": ""column7"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_class"": """",
  ""container8_label"": ""column8"",
  ""container8_width"": """",
  ""container8_name"": ""column8"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_class"": """",
  ""container9_label"": ""column9"",
  ""container9_width"": """",
  ""container9_name"": ""column9"",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_class"": """",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_class"": """",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_class"": """",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_class"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: ee57a3d3-4319-439f-bd89-69d4bfdd616f >>
			{
				var id = new Guid("ee57a3d3-4319-439f-bd89-69d4bfdd616f");
				Guid? parentId = new Guid("c6bf09c3-fc5e-4e2f-8df8-b96f733b964b");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.target_date\"",\""default\"":\""date\""}"",
  ""name"": ""target_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 2c8ab3cb-4cee-4507-839c-137699f13e8b >>
			{
				var id = new Guid("2c8ab3cb-4cee-4507-839c-137699f13e8b");
				Guid? parentId = new Guid("c6bf09c3-fc5e-4e2f-8df8-b96f733b964b");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing System.Diagnostics;\\nusing WebVella.Erp.Plugins.Next.Services;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar taskRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord\\\"");\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (taskRecord == null)\\n\\t\\t\\treturn null;\\n\\t\\t\\t\\n        var iconClass = \\\""\\\"";\\n        var color = \\\""\\\"";\\n        new TaskService().GetTaskIconAndColor((string)taskRecord[\\\""priority\\\""], out iconClass, out color);\\n\\n\\t\\treturn $\\\""<i class='{iconClass}' style='color:{color}'></i> <a href='/projects/tasks/tasks/r/{(Guid)taskRecord[\\\""id\\\""]}/details'>[{(string)taskRecord[\\\""key\\\""]}] {taskRecord[\\\""subject\\\""]}</a>\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""Task name\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: ae930e6f-38b5-4c48-a17f-63b0bdf7dab6 >>
			{
				var id = new Guid("ae930e6f-38b5-4c48-a17f-63b0bdf7dab6");
				Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""All Users' Timesheet"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": ""pt-3 pb-3"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 483e09f0-98c4-4e70-ad9a-3a92abebaf74 >>
			{
				var id = new Guid("483e09f0-98c4-4e70-ad9a-3a92abebaf74");
				Guid? parentId = new Guid("ae930e6f-38b5-4c48-a17f-63b0bdf7dab6");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcProjectWidgetTimesheet";
				var containerId = "body";
				var options = @"""{}""";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 151e265c-d3d3-4340-92fc-0cace2ca45f9 >>
			{
				var id = new Guid("151e265c-d3d3-4340-92fc-0cace2ca45f9");
				Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column1";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": ""mb-3"",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 47303562-04a3-4935-b228-aaa61527f963 >>
			{
				var id = new Guid("47303562-04a3-4935-b228-aaa61527f963");
				Guid? parentId = new Guid("151e265c-d3d3-4340-92fc-0cace2ca45f9");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""Tasks"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm h-100"",
  ""body_class"": ""p-3 align-center-col"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 5b95ff72-dfc0-4a99-ad3a-6c6107f7bd4c >>
			{
				var id = new Guid("5b95ff72-dfc0-4a99-ad3a-6c6107f7bd4c");
				Guid? parentId = new Guid("47303562-04a3-4935-b228-aaa61527f963");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcProjectWidgetTasks";
				var containerId = "body";
				var options = @"{
  ""project_id"": """",
  ""user_id"": ""{\""type\"":\""0\"",\""string\"":\""CurrentUser.Id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: be907fa3-0971-45b5-9dcf-fabbb277fe54 >>
			{
				var id = new Guid("be907fa3-0971-45b5-9dcf-fabbb277fe54");
				Guid? parentId = new Guid("151e265c-d3d3-4340-92fc-0cace2ca45f9");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Priority"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm h-100"",
  ""body_class"": ""p-3 align-center-col"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 209d32c9-6c2f-4f45-859a-3ae2718ebf88 >>
			{
				var id = new Guid("209d32c9-6c2f-4f45-859a-3ae2718ebf88");
				Guid? parentId = new Guid("be907fa3-0971-45b5-9dcf-fabbb277fe54");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcProjectWidgetTasksPriority";
				var containerId = "body";
				var options = @"{
  ""project_id"": """",
  ""user_id"": ""{\""type\"":\""0\"",\""string\"":\""CurrentUser.Id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 8e533c53-0bf5-4082-ae06-f47f1bd9b3b5 >>
			{
				var id = new Guid("8e533c53-0bf5-4082-ae06-f47f1bd9b3b5");
				Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""My 5 Upcoming Tasks "",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: b0fe4c98-2766-4b00-90f0-c583f8001ad0 >>
			{
				var id = new Guid("b0fe4c98-2766-4b00-90f0-c583f8001ad0");
				Guid? parentId = new Guid("8e533c53-0bf5-4082-ae06-f47f1bd9b3b5");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 2,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""ProjectWidgetMyTasks\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": ""mb-2 mt-2"",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""false"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No tasks available"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""false"",
  ""container1_label"": ""name"",
  ""container1_width"": """",
  ""container1_name"": ""name"",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_searchable"": ""false"",
  ""container2_label"": ""target date"",
  ""container2_width"": ""120px"",
  ""container2_name"": ""target_date"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_searchable"": ""false"",
  ""container3_label"": """",
  ""container3_width"": """",
  ""container3_name"": """",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_searchable"": ""false"",
  ""container4_label"": ""column4"",
  ""container4_width"": """",
  ""container4_name"": ""column4"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_searchable"": ""false"",
  ""container5_label"": ""column5"",
  ""container5_width"": """",
  ""container5_name"": ""column5"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_searchable"": ""false"",
  ""container6_label"": ""column6"",
  ""container6_width"": """",
  ""container6_name"": ""column6"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_searchable"": ""false"",
  ""container7_label"": ""column7"",
  ""container7_width"": """",
  ""container7_name"": ""column7"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_searchable"": ""false"",
  ""container8_label"": ""column8"",
  ""container8_width"": """",
  ""container8_name"": ""column8"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_searchable"": ""false"",
  ""container9_label"": ""column9"",
  ""container9_width"": """",
  ""container9_name"": ""column9"",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_searchable"": ""false"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_searchable"": ""false"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_searchable"": ""false"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_searchable"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 8e5c35f3-d43a-4422-a06e-466343900a8b >>
			{
				var id = new Guid("8e5c35f3-d43a-4422-a06e-466343900a8b");
				Guid? parentId = new Guid("b0fe4c98-2766-4b00-90f0-c583f8001ad0");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.target_date\"",\""default\"":\""\""}"",
  ""name"": ""target_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 5572d01d-d455-4b4a-a4ad-eca0be212870 >>
			{
				var id = new Guid("5572d01d-d455-4b4a-a4ad-eca0be212870");
				Guid? parentId = new Guid("b0fe4c98-2766-4b00-90f0-c583f8001ad0");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column3";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""25 June 2018"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: d2ff8513-7320-409f-8900-2f3620adefa5 >>
			{
				var id = new Guid("d2ff8513-7320-409f-8900-2f3620adefa5");
				Guid? parentId = new Guid("b0fe4c98-2766-4b00-90f0-c583f8001ad0");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing System.Diagnostics;\\nusing WebVella.Erp.Plugins.Next.Services;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar taskRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord\\\"");\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (taskRecord == null)\\n\\t\\t\\treturn null;\\n\\t\\t\\t\\n        var iconClass = \\\""\\\"";\\n        var color = \\\""\\\"";\\n        new TaskService().GetTaskIconAndColor((string)taskRecord[\\\""priority\\\""], out iconClass, out color);\\n\\n\\t\\treturn $\\\""<i class='{iconClass}' style='color:{color}'></i> <a href='/projects/tasks/tasks/r/{(Guid)taskRecord[\\\""id\\\""]}/details'>[{(string)taskRecord[\\\""key\\\""]}] {taskRecord[\\\""subject\\\""]}</a>\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""Task name\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: e49cf2f9-82b0-4988-aa29-427e8d9501d9 >>
			{
				var id = new Guid("e49cf2f9-82b0-4988-aa29-427e8d9501d9");
				Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""My Timesheet"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": ""pt-3 pb-3"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: c510d93d-e3d5-40d2-9655-73a3d2f63020 >>
			{
				var id = new Guid("c510d93d-e3d5-40d2-9655-73a3d2f63020");
				Guid? parentId = new Guid("e49cf2f9-82b0-4988-aa29-427e8d9501d9");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcProjectWidgetTimesheet";
				var containerId = "body";
				var options = @"{
  ""project_id"": """",
  ""user_id"": ""{\""type\"":\""0\"",\""string\"":\""CurrentUser.Id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 6ef7bbd7-b96c-45d4-97e1-b8e43f489ed5 >>
			{
				var id = new Guid("6ef7bbd7-b96c-45d4-97e1-b8e43f489ed5");
				Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""My Tasks Due Today"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 0d3f70c2-0fc9-426e-8174-954a4dd08638 >>
			{
				var id = new Guid("0d3f70c2-0fc9-426e-8174-954a4dd08638");
				Guid? parentId = new Guid("6ef7bbd7-b96c-45d4-97e1-b8e43f489ed5");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 2,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""ProjectWidgetMyTasksDueToday\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": ""mb-2 mt-2"",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""false"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No tasks available"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""false"",
  ""container1_label"": ""name"",
  ""container1_width"": """",
  ""container1_name"": ""name"",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_class"": """",
  ""container2_label"": ""target date"",
  ""container2_width"": ""120px"",
  ""container2_name"": ""target_date"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_class"": ""go-orange"",
  ""container3_label"": ""column3"",
  ""container3_width"": """",
  ""container3_name"": ""column3"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_class"": """",
  ""container4_label"": ""column4"",
  ""container4_width"": """",
  ""container4_name"": ""column4"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_class"": """",
  ""container5_label"": ""column5"",
  ""container5_width"": """",
  ""container5_name"": ""column5"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_class"": """",
  ""container6_label"": ""column6"",
  ""container6_width"": """",
  ""container6_name"": ""column6"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_class"": """",
  ""container7_label"": ""column7"",
  ""container7_width"": """",
  ""container7_name"": ""column7"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_class"": """",
  ""container8_label"": ""column8"",
  ""container8_width"": """",
  ""container8_name"": ""column8"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_class"": """",
  ""container9_label"": ""column9"",
  ""container9_width"": """",
  ""container9_name"": ""column9"",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_class"": """",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_class"": """",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_class"": """",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_class"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 8b436d58-7953-4c7c-9e18-e7243b363eb4 >>
			{
				var id = new Guid("8b436d58-7953-4c7c-9e18-e7243b363eb4");
				Guid? parentId = new Guid("0d3f70c2-0fc9-426e-8174-954a4dd08638");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.target_date\"",\""default\"":\""\""}"",
  ""name"": ""target_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: f1650fc8-0810-4dba-bdff-f7b87d0a6e6a >>
			{
				var id = new Guid("f1650fc8-0810-4dba-bdff-f7b87d0a6e6a");
				Guid? parentId = new Guid("0d3f70c2-0fc9-426e-8174-954a4dd08638");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing WebVella.Erp.Api;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\nusing WebVella.Erp.Plugins.Next.Services;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar taskRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord\\\"");\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (taskRecord == null)\\n\\t\\t\\treturn null;\\n\\t\\t\\t\\n        var iconClass = \\\""\\\"";\\n        var color = \\\""\\\"";\\n        new TaskService().GetTaskIconAndColor((string)taskRecord[\\\""priority\\\""], out iconClass, out color);\\n\\n\\t\\treturn $\\\""<i class='{iconClass}' style='color:{color}'></i> <a href='/projects/tasks/tasks/r/{(Guid)taskRecord[\\\""id\\\""]}/details'>[{(string)taskRecord[\\\""key\\\""]}] {taskRecord[\\\""subject\\\""]}</a>\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""Task Name\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: f68e4fb5-64d1-48ff-8846-e0ec36aa7e69 >>
			{
				var id = new Guid("f68e4fb5-64d1-48ff-8846-e0ec36aa7e69");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""App.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fas fa-tachometer-alt"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 6b53e36e-9003-4325-8899-7fbf9da32209 >>
			{
				var id = new Guid("6b53e36e-9003-4325-8899-7fbf9da32209");
				Guid? parentId = new Guid("f68e4fb5-64d1-48ff-8846-e0ec36aa7e69");
				Guid? nodeId = null;
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""New Task"",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-fw fa-plus go-green"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""/projects/tasks/tasks/c/create"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 6bb17b95-258a-4572-99f3-898d1895cfba >>
			{
				var id = new Guid("6bb17b95-258a-4572-99f3-898d1895cfba");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcValidation";
				var containerId = "";
				var options = @"{
  ""validation"": ""{\""type\"":\""0\"",\""string\"":\""Validation\"",\""default\"":\""\""}""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc >>
			{
				var id = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 8,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""AllTasks\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""true"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No records"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""true"",
  ""container1_label"": """",
  ""container1_width"": ""40px"",
  ""container1_name"": """",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_searchable"": ""false"",
  ""container2_label"": ""type"",
  ""container2_width"": ""20px"",
  ""container2_name"": ""type"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_searchable"": ""false"",
  ""container3_label"": ""key"",
  ""container3_width"": ""80px"",
  ""container3_name"": ""key"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_searchable"": ""false"",
  ""container4_label"": ""task"",
  ""container4_width"": """",
  ""container4_name"": ""task"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_searchable"": ""false"",
  ""container5_label"": ""owner"",
  ""container5_width"": ""120px"",
  ""container5_name"": ""owner_id"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_searchable"": ""false"",
  ""container6_label"": ""created by"",
  ""container6_width"": ""120px"",
  ""container6_name"": ""created_by"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_searchable"": ""false"",
  ""container7_label"": ""target date"",
  ""container7_width"": ""120px"",
  ""container7_name"": ""target_date"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_searchable"": ""false"",
  ""container8_label"": ""status"",
  ""container8_width"": ""80px"",
  ""container8_name"": ""status"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_searchable"": ""false"",
  ""container9_label"": """",
  ""container9_width"": """",
  ""container9_name"": """",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_searchable"": ""false"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_searchable"": ""false"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_searchable"": ""false"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_searchable"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 15df96da-8d77-427f-a2a1-23017a6f8800 >>
			{
				var id = new Guid("15df96da-8d77-427f-a2a1-23017a6f8800");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column8";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$task_status_1n_task[0].label\"",\""default\"":\""n/a\""}"",
  ""name"": ""status_id"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 9bdd70b0-aa1d-4458-ad95-9fc455236350 >>
			{
				var id = new Guid("9bdd70b0-aa1d-4458-ad95-9fc455236350");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column3";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.key\"",\""default\"":\""key\""}"",
  ""name"": ""key"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 08d8dad6-594d-498f-aa0b-555d245ce9e2 >>
			{
				var id = new Guid("08d8dad6-594d-498f-aa0b-555d245ce9e2");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column5";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_task[0].username\"",\""default\"":\""n/a\""}"",
  ""name"": ""owner_id"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: e5756351-b9c2-4bd9-bcbc-be3cc9fb3751 >>
			{
				var id = new Guid("e5756351-b9c2-4bd9-bcbc-be3cc9fb3751");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar typeRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord.$task_type_1n_task[0]\\\"");\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (typeRecord == null)\\n\\t\\t\\treturn null;\\n\\n        var iconClass=\\\""fa fa-fw fa-file\\\"";\\n        var color=\\\""#999\\\"";\\n        if(typeRecord[\\\""icon_class\\\""] != null){\\n            iconClass = (string)typeRecord[\\\""icon_class\\\""];\\n        }\\n        if(typeRecord[\\\""color\\\""] != null){\\n            color = (string)typeRecord[\\\""color\\\""];\\n        }\\n\\t\\treturn $\\\""<i class=\\\\\\\""{iconClass}\\\\\\\"" style=\\\\\\\""color:{color};font-size:23px;\\\\\\\"" title=\\\\\\\""{typeRecord[\\\""label\\\""]}\\\\\\\""></i>\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""icon\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: ad9c357f-e620-4ed1-9593-d76c97019677 >>
			{
				var id = new Guid("ad9c357f-e620-4ed1-9593-d76c97019677");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column7";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.target_date\"",\""default\"":\""\""}"",
  ""name"": ""target_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 0660124a-cbf0-47ff-9757-3f072c39953a >>
			{
				var id = new Guid("0660124a-cbf0-47ff-9757-3f072c39953a");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column6";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_task_creator[0].username\"",\""default\"":\""n/a\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 5899b892-ee3d-4cb9-9811-a24ff8f1b791 >>
			{
				var id = new Guid("5899b892-ee3d-4cb9-9811-a24ff8f1b791");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column4";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.subject\"",\""default\"":\""Task subject\""}"",
  ""name"": ""subject"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: a918fec1-865b-4c54-8f93-685ffe85fb90 >>
			{
				var id = new Guid("a918fec1-865b-4c54-8f93-685ffe85fb90");
				Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
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
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//replace constants with your values\\n\\t\\tconst string DATASOURCE_NAME = \\\""RowRecord.id\\\"";\\n\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar dataSource = pageModel.TryGetDataSourceProperty<Guid>(DATASOURCE_NAME);\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (dataSource == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\treturn $\\\""/projects/tasks/tasks/r/{dataSource}/details?returnUrl=/projects/tasks/tasks/l/all\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: c984f52a-5121-471d-ae66-e8a64de68c3d >>
			{
				var id = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 8,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""AllProjectTasks\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""true"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No records"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""true"",
  ""container1_label"": """",
  ""container1_width"": ""40px"",
  ""container1_name"": """",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_searchable"": ""false"",
  ""container2_label"": ""type"",
  ""container2_width"": ""20px"",
  ""container2_name"": ""type"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_searchable"": ""false"",
  ""container3_label"": ""key"",
  ""container3_width"": ""80px"",
  ""container3_name"": ""key"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_searchable"": ""false"",
  ""container4_label"": ""task"",
  ""container4_width"": """",
  ""container4_name"": ""task"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_searchable"": ""false"",
  ""container5_label"": ""owner"",
  ""container5_width"": ""120px"",
  ""container5_name"": ""owner_id"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_searchable"": ""false"",
  ""container6_label"": ""created by"",
  ""container6_width"": ""120px"",
  ""container6_name"": ""created_by"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_searchable"": ""false"",
  ""container7_label"": ""target date"",
  ""container7_width"": ""120px"",
  ""container7_name"": ""target_date"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_searchable"": ""false"",
  ""container8_label"": ""status"",
  ""container8_width"": ""80px"",
  ""container8_name"": ""status"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_searchable"": ""false"",
  ""container9_label"": ""column9"",
  ""container9_width"": """",
  ""container9_name"": ""column9"",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_searchable"": ""false"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_searchable"": ""false"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_searchable"": ""false"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_searchable"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: d088ba1c-15b8-48b9-8673-a871338cbdea >>
			{
				var id = new Guid("d088ba1c-15b8-48b9-8673-a871338cbdea");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column4";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.subject\"",\""default\"":\""Task subject\""}"",
  ""name"": ""subject"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 064ea82a-c5c2-40dd-96e4-7859aa879b14 >>
			{
				var id = new Guid("064ea82a-c5c2-40dd-96e4-7859aa879b14");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column5";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_task[0].username\"",\""default\"":\""n/a\""}"",
  ""name"": ""owner_id"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: e83f6542-f9f8-4fec-aeb8-48731951f182 >>
			{
				var id = new Guid("e83f6542-f9f8-4fec-aeb8-48731951f182");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column7";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.target_date\"",\""default\"":\""n/a\""}"",
  ""name"": ""target_date"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: cfa8a277-5447-45f2-ad06-26818381b54a >>
			{
				var id = new Guid("cfa8a277-5447-45f2-ad06-26818381b54a");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column3";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.key\"",\""default\"":\""key\""}"",
  ""name"": ""key"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 9cd708aa-60c5-4dfa-b95c-73e5508aec64 >>
			{
				var id = new Guid("9cd708aa-60c5-4dfa-b95c-73e5508aec64");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column8";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$task_status_1n_task[0].label\"",\""default\"":\""n/a\""}"",
  ""name"": ""status_id"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: ad1e60ec-813c-4b1d-aa33-ad76d705d5d9 >>
			{
				var id = new Guid("ad1e60ec-813c-4b1d-aa33-ad76d705d5d9");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column6";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_task_creator[0].username\"",\""default\"":\""n/a\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: a20664ce-a3fe-436a-84f0-42f4e14564c1 >>
			{
				var id = new Guid("a20664ce-a3fe-436a-84f0-42f4e14564c1");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar typeRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord.$task_type_1n_task[0]\\\"");\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (typeRecord == null)\\n\\t\\t\\treturn null;\\n\\n        var iconClass=\\\""fa fa-fw fa-file\\\"";\\n        var color=\\\""#999\\\"";\\n        if(typeRecord[\\\""icon_class\\\""] != null){\\n            iconClass = (string)typeRecord[\\\""icon_class\\\""];\\n        }\\n        if(typeRecord[\\\""color\\\""] != null){\\n            color = (string)typeRecord[\\\""color\\\""];\\n        }\\n\\t\\treturn $\\\""<i class=\\\\\\\""{iconClass}\\\\\\\"" style=\\\\\\\""color:{color};font-size:23px;\\\\\\\"" title=\\\\\\\""{typeRecord[\\\""label\\\""]}\\\\\\\""></i>\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""icon\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 4f298a92-4592-4714-948e-eaebb3962785 >>
			{
				var id = new Guid("4f298a92-4592-4714-948e-eaebb3962785");
				Guid? parentId = new Guid("c984f52a-5121-471d-ae66-e8a64de68c3d");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
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
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar taskId = pageModel.TryGetDataSourceProperty<Guid>(\\\""RowRecord.id\\\"");\\n        var projectId = pageModel.TryGetDataSourceProperty<Guid>(\\\""ParentRecord.id\\\"");\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (taskId == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\treturn $\\\""/projects/tasks/tasks/r/{taskId}/details?returnUrl=/projects/projects/projects/r/{projectId}/rl/b1db4466-7423-44e9-b6b9-3063222c9e15/l/tasks\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: d65f22f5-6644-4ca9-81ce-c3ce5898f8b5 >>
			{
				var id = new Guid("d65f22f5-6644-4ca9-81ce-c3ce5898f8b5");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""App.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""far fa-clock"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 2dd6b580-b5f3-4fed-82c9-3da50f086139 >>
			{
				var id = new Guid("2dd6b580-b5f3-4fed-82c9-3da50f086139");
				Guid? parentId = new Guid("d65f22f5-6644-4ca9-81ce-c3ce5898f8b5");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""search"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-search"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer','open')"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: comment-test  id: eb825f3d-8273-4b1b-8df6-b1d286b3644f >>
			{
				var id = new Guid("eb825f3d-8273-4b1b-8df6-b1d286b3644f");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("9401d420-808d-475f-a38a-bd34d3763ce9");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "";
				var options = @"{
  ""id"": ""wv-eb825f3d-8273-4b1b-8df6-b1d286b3644f"",
  ""name"": ""CreateRecord"",
  ""hook_key"": """",
  ""method"": ""post"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: comment-test  id: 28dd5bc8-0672-4217-aba1-b50d6740247d >>
			{
				var id = new Guid("28dd5bc8-0672-4217-aba1-b50d6740247d");
				Guid? parentId = new Guid("eb825f3d-8273-4b1b-8df6-b1d286b3644f");
				Guid? nodeId = null;
				var pageId = new Guid("9401d420-808d-475f-a38a-bd34d3763ce9");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "body";
				var options = @"{
  ""value"": ""[\""38472d89-7cb1-4972-8f32-f3f2aabbdd81\"",\""1fc66615-27d2-4366-81ef-ce4684c13498\""]"",
  ""name"": ""l_scope"",
  ""try_connect_to_entity"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: comment-test  id: a6d7045d-0368-4103-adab-e6aeecc7c40b >>
			{
				var id = new Guid("a6d7045d-0368-4103-adab-e6aeecc7c40b");
				Guid? parentId = new Guid("eb825f3d-8273-4b1b-8df6-b1d286b3644f");
				Guid? nodeId = null;
				var pageId = new Guid("9401d420-808d-475f-a38a-bd34d3763ce9");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "body";
				var options = @"{
  ""value"": ""eabd66fd-8de1-4d79-9674-447ee89921c2"",
  ""name"": ""created_by"",
  ""try_connect_to_entity"": ""true""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: comment-test  id: 092f0714-92d7-47a3-bc7c-a46b966393c8 >>
			{
				var id = new Guid("092f0714-92d7-47a3-bc7c-a46b966393c8");
				Guid? parentId = new Guid("eb825f3d-8273-4b1b-8df6-b1d286b3644f");
				Guid? nodeId = null;
				var pageId = new Guid("9401d420-808d-475f-a38a-bd34d3763ce9");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Create Comment"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 7;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: comment-test  id: 7f36d6d5-02bf-4c41-9c22-94a9f1d19500 >>
			{
				var id = new Guid("7f36d6d5-02bf-4c41-9c22-94a9f1d19500");
				Guid? parentId = new Guid("eb825f3d-8273-4b1b-8df6-b1d286b3644f");
				Guid? nodeId = null;
				var pageId = new Guid("9401d420-808d-475f-a38a-bd34d3763ce9");
				var componentName = "WebVella.Erp.Web.Components.PcFieldTextarea";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.body\"",\""default\"":\""\""}"",
  ""name"": ""body"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""height"": """"
}";
				var weight = 6;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: comment-test  id: 3463f4aa-ee46-472d-bd95-a6c9a3c09ee7 >>
			{
				var id = new Guid("3463f4aa-ee46-472d-bd95-a6c9a3c09ee7");
				Guid? parentId = new Guid("eb825f3d-8273-4b1b-8df6-b1d286b3644f");
				Guid? nodeId = null;
				var pageId = new Guid("9401d420-808d-475f-a38a-bd34d3763ce9");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "body";
				var options = @"{
  ""value"": ""38472d89-7cb1-4972-8f32-f3f2aabbdd81"",
  ""name"": ""$task_nn_comment.id"",
  ""try_connect_to_entity"": ""true""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: comment-test  id: f22e377d-2725-4052-b5be-b4c9597a6ce9 >>
			{
				var id = new Guid("f22e377d-2725-4052-b5be-b4c9597a6ce9");
				Guid? parentId = new Guid("eb825f3d-8273-4b1b-8df6-b1d286b3644f");
				Guid? nodeId = null;
				var pageId = new Guid("9401d420-808d-475f-a38a-bd34d3763ce9");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "body";
				var options = @"{
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SampleCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\treturn Guid.NewGuid();\\n\\t}\\n}\"",\""default\"":\""\""}"",
  ""name"": ""id"",
  ""try_connect_to_entity"": ""false""
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: comment-test  id: ba6e3266-baed-4aba-9eb8-b964f2ed06ad >>
			{
				var id = new Guid("ba6e3266-baed-4aba-9eb8-b964f2ed06ad");
				Guid? parentId = new Guid("eb825f3d-8273-4b1b-8df6-b1d286b3644f");
				Guid? nodeId = null;
				var pageId = new Guid("9401d420-808d-475f-a38a-bd34d3763ce9");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "body";
				var options = @"{
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SampleCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\treturn DateTime.UtcNow;\\n\\t}\\n}\"",\""default\"":\""\""}"",
  ""name"": ""created_on"",
  ""try_connect_to_entity"": ""true""
}";
				var weight = 5;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: feed  id: 33cba2bb-6070-4b00-ba92-64064077a49b >>
			{
				var id = new Guid("33cba2bb-6070-4b00-ba92-64064077a49b");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("dfe56667-174d-492d-8f84-b8ab8b70c63f");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""Record.abbr\"",\""default\"":\""\""}"",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-rss"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: feed  id: c50ad432-98f2-4140-a40c-3157fc52f93c >>
			{
				var id = new Guid("c50ad432-98f2-4140-a40c-3157fc52f93c");
				Guid? parentId = new Guid("33cba2bb-6070-4b00-ba92-64064077a49b");
				Guid? nodeId = null;
				var pageId = new Guid("dfe56667-174d-492d-8f84-b8ab8b70c63f");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "toolbar";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar projectId = pageModel.TryGetDataSourceProperty<Guid>(\\\""Record.id\\\"");\\n        var pageName = pageModel.TryGetDataSourceProperty<string>(\\\""Page.Name\\\"");\\n\\n\\t\\tif (projectId == null || pageName == null)\\n\\t\\t\\treturn null;\\n\\n        var result = $\\\""<a href='/projects/projects/projects/r/{projectId}/dashboard' class='btn btn-link btn-sm {(pageName == \\\""dashboard\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Dashboard</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/feed' class='btn btn-link btn-sm {(pageName == \\\""feed\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Feed</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/b1db4466-7423-44e9-b6b9-3063222c9e15/l/tasks' class='btn btn-link btn-sm {(pageName == \\\""tasks\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Tasks</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/55c8d6e2-f26d-4689-9d1b-a8c1b9de1672/l/milestones' class='btn btn-link btn-sm {(pageName == \\\""milestones\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Milestones</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/details' class='btn btn-link btn-sm {(pageName == \\\""details\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Details</a>\\\"";\\n\\t\\treturn result;\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: e84c527a-4feb-4d60-ab91-4b1ecd89b39c >>
			{
				var id = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 3,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""TrackTimeTasks\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""false"",
  ""bordered"": ""true"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No records"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""false"",
  ""container1_label"": ""my tasks "",
  ""container1_width"": """",
  ""container1_name"": ""task"",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_class"": """",
  ""container1_vertical_align"": ""3"",
  ""container1_horizontal_align"": ""1"",
  ""container2_label"": ""logged"",
  ""container2_width"": ""120px"",
  ""container2_name"": ""logged"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_class"": ""timer-td"",
  ""container2_vertical_align"": ""3"",
  ""container2_horizontal_align"": ""1"",
  ""container3_label"": ""action"",
  ""container3_width"": ""130px"",
  ""container3_name"": """",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_class"": """",
  ""container3_vertical_align"": ""3"",
  ""container3_horizontal_align"": ""1"",
  ""container4_label"": ""column4"",
  ""container4_width"": """",
  ""container4_name"": ""column4"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_class"": """",
  ""container4_vertical_align"": ""1"",
  ""container4_horizontal_align"": ""1"",
  ""container5_label"": ""column5"",
  ""container5_width"": """",
  ""container5_name"": ""column5"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_class"": """",
  ""container5_vertical_align"": ""1"",
  ""container5_horizontal_align"": ""1"",
  ""container6_label"": ""column6"",
  ""container6_width"": """",
  ""container6_name"": ""column6"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_class"": """",
  ""container6_vertical_align"": ""1"",
  ""container6_horizontal_align"": ""1"",
  ""container7_label"": ""column7"",
  ""container7_width"": """",
  ""container7_name"": ""column7"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_class"": """",
  ""container7_vertical_align"": ""1"",
  ""container7_horizontal_align"": ""1"",
  ""container8_label"": ""column8"",
  ""container8_width"": """",
  ""container8_name"": ""column8"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_class"": """",
  ""container8_vertical_align"": ""1"",
  ""container8_horizontal_align"": ""1"",
  ""container9_label"": ""column9"",
  ""container9_width"": """",
  ""container9_name"": ""column9"",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_class"": """",
  ""container9_vertical_align"": ""1"",
  ""container9_horizontal_align"": ""1"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_class"": """",
  ""container10_vertical_align"": ""1"",
  ""container10_horizontal_align"": ""1"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_class"": """",
  ""container11_vertical_align"": ""1"",
  ""container11_horizontal_align"": ""1"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_class"": """",
  ""container12_vertical_align"": ""1"",
  ""container12_horizontal_align"": ""1""
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 3d0eb8a7-1182-4974-a039-433954aa8d7c >>
			{
				var id = new Guid("3d0eb8a7-1182-4974-a039-433954aa8d7c");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "column3";
				var options = @"{
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.id\"",\""default\"":\""\""}"",
  ""name"": ""task_id"",
  ""try_connect_to_entity"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 46657d5a-0102-43b7-9ca3-9259953d37b6 >>
			{
				var id = new Guid("46657d5a-0102-43b7-9ca3-9259953d37b6");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcBtnGroup";
				var containerId = "column3";
				var options = @"{
  ""size"": ""3"",
  ""is_vertical"": ""false"",
  ""class"": ""d-none stop-log-group w-100"",
  ""id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 700954cc-7407-4b20-81de-a882380e5d4d >>
			{
				var id = new Guid("700954cc-7407-4b20-81de-a882380e5d4d");
				Guid? parentId = new Guid("46657d5a-0102-43b7-9ca3-9259953d37b6");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""stop logging"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": ""btn-block stop-log"",
  ""id"": """",
  ""icon_class"": ""fas fa-fw fa-square go-red"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: c57d94a6-9c90-4071-b54b-2c05b79aa522 >>
			{
				var id = new Guid("c57d94a6-9c90-4071-b54b-2c05b79aa522");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\ttry{\\n\\t\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\t\\tif (pageModel == null)\\n\\t\\t\\t\\treturn null;\\n\\t\\n\\t\\t\\t//try read data source by name and get result as specified type object\\n\\t\\t\\tvar dataSource = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord\\\"");\\n\\t\\n\\t\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\t\\tif (dataSource == null)\\n\\t\\t\\t\\treturn null;\\n\\t        var loggedSeconds = ((int)dataSource[\\\""logged_minutes\\\""])*60;\\n\\t        var logStartedOn = (DateTime?)dataSource[\\\""timelog_started_on\\\""];\\n\\t        var logStartString = \\\""\\\"";\\n\\t        if(logStartedOn != null){\\n\\t            loggedSeconds = loggedSeconds + (int)((DateTime.UtcNow - logStartedOn.Value).TotalSeconds);\\n\\t            logStartString = logStartedOn.Value.ToString(\\\""o\\\"");\\n\\t        }\\n\\n\\t        var hours = (int)(loggedSeconds/3600);\\n\\t        var loggedSecondsLeft = loggedSeconds - hours*3600;\\n\\t        var hoursString = \\\""00\\\"";\\n\\t        if(hours < 10)\\n\\t            hoursString = \\\""0\\\"" + hours;\\n            else\\n                hoursString = hours.ToString();\\n\\t            \\n\\t        var minutes = (int)(loggedSecondsLeft/60);\\n\\t        var minutesString = \\\""00\\\"";\\n\\t        if(minutes < 10)\\n\\t            minutesString = \\\""0\\\"" + minutes;\\n            else\\n                minutesString = minutes.ToString();\\t        \\n                \\n            var seconds =  loggedSecondsLeft -  minutes*60;\\n\\t        var secondsString = \\\""00\\\"";\\n\\t        if(seconds < 10)\\n\\t            secondsString = \\\""0\\\"" + seconds;\\n            else\\n                secondsString = seconds.ToString();\\t                    \\n            \\n            var result = $\\\""<span class='go-gray wv-timer' style='font-size:16px;'>{hoursString + \\\"" : \\\"" + minutesString + \\\"" : \\\"" + secondsString}</span>\\\\n\\\"";\\n            result += $\\\""<input type='hidden' name='timelog_total_seconds' value='{loggedSeconds}'/>\\\\n\\\"";\\n            result += $\\\""<input type='hidden' name='timelog_started_on' value='{logStartString}'/>\\\"";\\n            return result;\\n\\t\\t}\\n\\t\\tcatch(Exception ex){\\n\\t\\t\\treturn \\\""Error: \\\"" + ex.Message;\\n\\t\\t}\\n\\t}\\n}\\n\"",\""default\"":\""<span class=\\\""go-gray\\\"" style='font-size:16px;'>00 : 00 : 00</span>\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: b2baa937-e32a-4a06-8b9b-404f89e539c0 >>
			{
				var id = new Guid("b2baa937-e32a-4a06-8b9b-404f89e539c0");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing System.Diagnostics;\\nusing WebVella.Erp.Plugins.Next.Services;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar taskRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RowRecord\\\"");\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (taskRecord == null)\\n\\t\\t\\treturn null;\\n\\t\\t\\t\\n        var iconClass = \\\""\\\"";\\n        var color = \\\""\\\"";\\n        new TaskService().GetTaskIconAndColor((string)taskRecord[\\\""priority\\\""], out iconClass, out color);\\n\\n\\t\\treturn $\\\""<i class='{iconClass}' style='color:{color}'></i> <a href='/projects/tasks/tasks/r/{(Guid)taskRecord[\\\""id\\\""]}/details'>[{(string)taskRecord[\\\""key\\\""]}] {taskRecord[\\\""subject\\\""]}</a>\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""Task name\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 278b4db1-b310-416a-9f32-66ecd3475ba8 >>
			{
				var id = new Guid("278b4db1-b310-416a-9f32-66ecd3475ba8");
				Guid? parentId = new Guid("e84c527a-4feb-4d60-ab91-4b1ecd89b39c");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcBtnGroup";
				var containerId = "column3";
				var options = @"{
  ""size"": ""1"",
  ""is_vertical"": ""false"",
  ""class"": ""start-log-group w-100 d-none"",
  ""id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 55ff1b2f-43d4-4bde-818c-fd139d799261 >>
			{
				var id = new Guid("55ff1b2f-43d4-4bde-818c-fd139d799261");
				Guid? parentId = new Guid("278b4db1-b310-416a-9f32-66ecd3475ba8");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""0"",
  ""text"": """",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": ""manual-log"",
  ""id"": """",
  ""icon_class"": ""fa fa-plus"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 4603466b-422c-4666-9f05-aae386569590 >>
			{
				var id = new Guid("4603466b-422c-4666-9f05-aae386569590");
				Guid? parentId = new Guid("278b4db1-b310-416a-9f32-66ecd3475ba8");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""start log"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": ""start-log"",
  ""id"": """",
  ""icon_class"": ""fa fa-fw fa-play"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: d32f39bb-8ad4-438d-a8d1-7abca6f5e6b4 >>
			{
				var id = new Guid("d32f39bb-8ad4-438d-a8d1-7abca6f5e6b4");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"""{}""";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: aeacecd8-8b3e-4cdb-84f6-114a2fb3c06d >>
			{
				var id = new Guid("aeacecd8-8b3e-4cdb-84f6-114a2fb3c06d");
				Guid? parentId = new Guid("d32f39bb-8ad4-438d-a8d1-7abca6f5e6b4");
				Guid? nodeId = null;
				var pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Create Account"",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-fw fa-plus go-green"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": ""CreateRecord""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: b7b8ed33-910f-4d28-bbe8-48c0799b00b5 >>
			{
				var id = new Guid("b7b8ed33-910f-4d28-bbe8-48c0799b00b5");
				Guid? parentId = new Guid("d32f39bb-8ad4-438d-a8d1-7abca6f5e6b4");
				Guid? nodeId = null;
				var pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""Cancel"",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""0\"",\""string\"":\""ReturnUrl\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 037ee1a4-e26c-4cd1-91ca-0e626c2995ed >>
			{
				var id = new Guid("037ee1a4-e26c-4cd1-91ca-0e626c2995ed");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "";
				var options = @"{
  ""id"": ""CreateRecord"",
  ""name"": ""CreateAccount"",
  ""hook_key"": """",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 0fb05f08-6066-4de8-8452-c8b3c7306ff9 >>
			{
				var id = new Guid("0fb05f08-6066-4de8-8452-c8b3c7306ff9");
				Guid? parentId = new Guid("037ee1a4-e26c-4cd1-91ca-0e626c2995ed");
				Guid? nodeId = null;
				var pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"""{}""";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 5ecb652c-e474-4700-bc32-5173d2fdad00 >>
			{
				var id = new Guid("5ecb652c-e474-4700-bc32-5173d2fdad00");
				Guid? parentId = new Guid("0fb05f08-6066-4de8-8452-c8b3c7306ff9");
				Guid? nodeId = null;
				var pageId = new Guid("d4b31a98-b1ed-44b5-aa69-32a6fc87205e");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Name"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""name"": ""name"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 03d2ed0f-33ed-4b7d-84fb-102f4b7452a8 >>
			{
				var id = new Guid("03d2ed0f-33ed-4b7d-84fb-102f4b7452a8");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("80b10445-c850-44cf-9c8c-57daca671dcf");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"""{}""";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 7eb7af4f-bdd3-410a-b3c4-71e620b627c5 >>
			{
				var id = new Guid("7eb7af4f-bdd3-410a-b3c4-71e620b627c5");
				Guid? parentId = new Guid("03d2ed0f-33ed-4b7d-84fb-102f4b7452a8");
				Guid? nodeId = null;
				var pageId = new Guid("80b10445-c850-44cf-9c8c-57daca671dcf");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Name"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""name"": ""name"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 552a4fad-5236-4aad-b3fc-443a5f12e574 >>
			{
				var id = new Guid("552a4fad-5236-4aad-b3fc-443a5f12e574");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("80b10445-c850-44cf-9c8c-57daca671dcf");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""Entity.LabelPlural\"",\""default\"":\""\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""Record.label\"",\""default\"":\""\""}"",
  ""title"": ""Account Details"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""Entity.IconName\"",\""default\"":\""\""}"",
  ""return_url"": ""/crm/accounts/accounts/l/list""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: 81fda9cf-04d7-4f99-8448-34392e1c0640 >>
			{
				var id = new Guid("81fda9cf-04d7-4f99-8448-34392e1c0640");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcDrawer";
				var containerId = "";
				var options = @"{
  ""title"": ""Search Accounts"",
  ""width"": ""550px"",
  ""class"": """",
  ""body_class"": """",
  ""title_action_html"": ""<a href=\""javascript:void(0)\"" class=\""clear-filter-all\"">clear all</a>""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: 492d9088-16bc-40fd-963b-8a8c2acf0ffa >>
			{
				var id = new Guid("492d9088-16bc-40fd-963b-8a8c2acf0ffa");
				Guid? parentId = new Guid("81fda9cf-04d7-4f99-8448-34392e1c0640");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-492d9088-16bc-40fd-963b-8a8c2acf0ffa"",
  ""name"": ""form"",
  ""hook_key"": """",
  ""method"": ""get"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: 3845960e-4fc6-40f6-9ef6-36e7392f8ab0 >>
			{
				var id = new Guid("3845960e-4fc6-40f6-9ef6-36e7392f8ab0");
				Guid? parentId = new Guid("492d9088-16bc-40fd-963b-8a8c2acf0ffa");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcGridFilterField";
				var containerId = "body";
				var options = @"{
  ""label"": ""Name"",
  ""name"": ""name"",
  ""try_connect_to_entity"": ""true"",
  ""field_type"": ""18"",
  ""query_type"": ""2"",
  ""query_options"": """",
  ""prefix"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: list  id: ec6f4bb5-aeeb-4706-a3dd-f3f208c63c6a >>
			{
				var id = new Guid("ec6f4bb5-aeeb-4706-a3dd-f3f208c63c6a");
				Guid? parentId = new Guid("492d9088-16bc-40fd-963b-8a8c2acf0ffa");
				Guid? nodeId = null;
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Search Accounts"",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 22af9111-4f15-48c1-a9fd-e5ab72074b3e >>
			{
				var id = new Guid("22af9111-4f15-48c1-a9fd-e5ab72074b3e");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 4,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""AllProjects\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""true"",
  ""bordered"": ""true"",
  ""borderless"": ""false"",
  ""hover"": ""true"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No projects"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""true"",
  ""container1_label"": """",
  ""container1_width"": ""40px"",
  ""container1_name"": """",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_searchable"": ""false"",
  ""container2_label"": ""abbr"",
  ""container2_width"": ""60px"",
  ""container2_name"": ""abbr"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""true"",
  ""container2_searchable"": ""true"",
  ""container3_label"": ""name"",
  ""container3_width"": """",
  ""container3_name"": ""name"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_searchable"": ""false"",
  ""container4_label"": ""lead"",
  ""container4_width"": """",
  ""container4_name"": ""lead"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_searchable"": ""false"",
  ""container5_label"": ""column5"",
  ""container5_width"": """",
  ""container5_name"": ""column5"",
  ""container5_nowrap"": ""false"",
  ""container5_sortable"": ""false"",
  ""container5_searchable"": ""false"",
  ""container6_label"": ""column6"",
  ""container6_width"": """",
  ""container6_name"": ""column6"",
  ""container6_nowrap"": ""false"",
  ""container6_sortable"": ""false"",
  ""container6_searchable"": ""false"",
  ""container7_label"": ""column7"",
  ""container7_width"": """",
  ""container7_name"": ""column7"",
  ""container7_nowrap"": ""false"",
  ""container7_sortable"": ""false"",
  ""container7_searchable"": ""false"",
  ""container8_label"": ""column8"",
  ""container8_width"": """",
  ""container8_name"": ""column8"",
  ""container8_nowrap"": ""false"",
  ""container8_sortable"": ""false"",
  ""container8_searchable"": ""false"",
  ""container9_label"": ""column9"",
  ""container9_width"": """",
  ""container9_name"": ""column9"",
  ""container9_nowrap"": ""false"",
  ""container9_sortable"": ""false"",
  ""container9_searchable"": ""false"",
  ""container10_label"": ""column10"",
  ""container10_width"": """",
  ""container10_name"": ""column10"",
  ""container10_nowrap"": ""false"",
  ""container10_sortable"": ""false"",
  ""container10_searchable"": ""false"",
  ""container11_label"": ""column11"",
  ""container11_width"": """",
  ""container11_name"": ""column11"",
  ""container11_nowrap"": ""false"",
  ""container11_sortable"": ""false"",
  ""container11_searchable"": ""false"",
  ""container12_label"": ""column12"",
  ""container12_width"": """",
  ""container12_name"": ""column12"",
  ""container12_nowrap"": ""false"",
  ""container12_sortable"": ""false"",
  ""container12_searchable"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 31a4f843-0ab5-4fd1-86ee-ad5f23f0d47a >>
			{
				var id = new Guid("31a4f843-0ab5-4fd1-86ee-ad5f23f0d47a");
				Guid? parentId = new Guid("22af9111-4f15-48c1-a9fd-e5ab72074b3e");
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column4";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.$user_1n_project_owner[0].username\"",\""default\"":\""Username\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: fcd1e0a0-bfc3-422f-b19d-2536dd919289 >>
			{
				var id = new Guid("fcd1e0a0-bfc3-422f-b19d-2536dd919289");
				Guid? parentId = new Guid("22af9111-4f15-48c1-a9fd-e5ab72074b3e");
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.abbr\"",\""default\"":\""abbr\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: ec508ea2-2332-40f0-838c-52d3ee250122 >>
			{
				var id = new Guid("ec508ea2-2332-40f0-838c-52d3ee250122");
				Guid? parentId = new Guid("22af9111-4f15-48c1-a9fd-e5ab72074b3e");
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column3";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.name\"",\""default\"":\""Project name\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 54d22f88-7a46-41e7-89b4-603dc14e7e73 >>
			{
				var id = new Guid("54d22f88-7a46-41e7-89b4-603dc14e7e73");
				Guid? parentId = new Guid("22af9111-4f15-48c1-a9fd-e5ab72074b3e");
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "column1";
				var options = @"{
  ""type"": ""2"",
  ""text"": """",
  ""color"": ""0"",
  ""size"": ""1"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-eye"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//replace constants with your values\\n\\t\\tconst string DATASOURCE_NAME = \\\""RowRecord.id\\\"";\\n\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar dataSource = pageModel.TryGetDataSourceProperty<Guid>(DATASOURCE_NAME);\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (dataSource == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\treturn $\\\""/projects/projects/projects/r/{dataSource}/dashboard\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 39db266a-da49-4a6e-b74d-898c601ad78b >>
			{
				var id = new Guid("39db266a-da49-4a6e-b74d-898c601ad78b");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-plus"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: b5a15dac-a606-4c93-b258-f1a7ab799a05 >>
			{
				var id = new Guid("b5a15dac-a606-4c93-b258-f1a7ab799a05");
				Guid? parentId = new Guid("39db266a-da49-4a6e-b74d-898c601ad78b");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""Cancel"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""0\"",\""string\"":\""ReturnUrl\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 8e4e7f05-8942-4db1-8514-e460bde1e2b4 >>
			{
				var id = new Guid("8e4e7f05-8942-4db1-8514-e460bde1e2b4");
				Guid? parentId = new Guid("39db266a-da49-4a6e-b74d-898c601ad78b");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Create Project"",
  ""color"": ""1"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-save"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": ""CreateRecord""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: e6c5b22a-491a-4186-82d6-667253e2db0f >>
			{
				var id = new Guid("e6c5b22a-491a-4186-82d6-667253e2db0f");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "";
				var options = @"{
  ""id"": ""CreateRecord"",
  ""name"": ""CreateRecord"",
  ""hook_key"": """",
  ""method"": ""post"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 4dfaa373-e250-4a76-b5a5-98d596a52313 >>
			{
				var id = new Guid("4dfaa373-e250-4a76-b5a5-98d596a52313");
				Guid? parentId = new Guid("e6c5b22a-491a-4186-82d6-667253e2db0f");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 8,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: fc423988-297c-457d-a14b-9fe12557cc2e >>
			{
				var id = new Guid("fc423988-297c-457d-a14b-9fe12557cc2e");
				Guid? parentId = new Guid("4dfaa373-e250-4a76-b5a5-98d596a52313");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Name"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""name"": ""name"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""1"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 0f90af36-8f2d-4f26-8ba2-ea7e8accdc6d >>
			{
				var id = new Guid("0f90af36-8f2d-4f26-8ba2-ea7e8accdc6d");
				Guid? parentId = new Guid("4dfaa373-e250-4a76-b5a5-98d596a52313");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Abbreviation"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.abbr\"",\""default\"":\""\""}"",
  ""name"": ""abbr"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 7bbf3667-a26d-48d4-8eba-8ca5e03d14c3 >>
			{
				var id = new Guid("7bbf3667-a26d-48d4-8eba-8ca5e03d14c3");
				Guid? parentId = new Guid("e6c5b22a-491a-4186-82d6-667253e2db0f");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 1,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: cc487c98-c59f-4e8c-b147-36914bcf70fc >>
			{
				var id = new Guid("cc487c98-c59f-4e8c-b147-36914bcf70fc");
				Guid? parentId = new Guid("7bbf3667-a26d-48d4-8eba-8ca5e03d14c3");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Description"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.description\"",\""default\"":\""\""}"",
  ""name"": ""description"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 6529686c-c8b4-40f0-8242-e24153657be2 >>
			{
				var id = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? parentId = new Guid("7bbf3667-a26d-48d4-8eba-8ca5e03d14c3");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column1";
				var options = @"{
  ""visible_columns"": 3,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: ace9e1bf-47bf-495f-8e6b-7683d2a0fa78 >>
			{
				var id = new Guid("ace9e1bf-47bf-495f-8e6b-7683d2a0fa78");
				Guid? parentId = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Budget amount"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.budget_amount\"",\""default\"":\""\""}"",
  ""name"": ""budget_amount"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: aec39a46-526c-45f2-ad43-38618a366098 >>
			{
				var id = new Guid("aec39a46-526c-45f2-ad43-38618a366098");
				Guid? parentId = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Start date"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.start_date\"",\""default\"":\""\""}"",
  ""name"": ""start_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 8dc4fd15-a1eb-4b7e-a1a9-381ac8e7de9b >>
			{
				var id = new Guid("8dc4fd15-a1eb-4b7e-a1a9-381ac8e7de9b");
				Guid? parentId = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Budget type"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.budget_type\"",\""default\"":\""\""}"",
  ""name"": ""budget_type"",
  ""try_connect_to_entity"": ""true"",
  ""options"": """",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 0e6f5387-b9c4-4fdd-9349-73e8424c6788 >>
			{
				var id = new Guid("0e6f5387-b9c4-4fdd-9349-73e8424c6788");
				Guid? parentId = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Account"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.account_id\"",\""default\"":\""\""}"",
  ""name"": ""account_id"",
  ""try_connect_to_entity"": ""false"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllAccountsSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""0""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 5c8d449d-95b8-419b-9851-b9a227e7093b >>
			{
				var id = new Guid("5c8d449d-95b8-419b-9851-b9a227e7093b");
				Guid? parentId = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Project lead"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.owner_id\"",\""default\"":\""\""}"",
  ""name"": ""owner_id"",
  ""try_connect_to_entity"": ""false"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllUsersSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: f0de1f0c-b71d-4002-a547-4e6d08654ea8 >>
			{
				var id = new Guid("f0de1f0c-b71d-4002-a547-4e6d08654ea8");
				Guid? parentId = new Guid("6529686c-c8b4-40f0-8242-e24153657be2");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""End date"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.end_date\"",\""default\"":\""\""}"",
  ""name"": ""end_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 7bc83302-6b26-46ef-a6a1-3e656527faef >>
			{
				var id = new Guid("7bc83302-6b26-46ef-a6a1-3e656527faef");
				Guid? parentId = new Guid("7bbf3667-a26d-48d4-8eba-8ca5e03d14c3");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column1";
				var options = @"{
  ""visible_columns"": 3,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: bf38fcf4-adeb-4388-ad4b-6aa4485f9258 >>
			{
				var id = new Guid("bf38fcf4-adeb-4388-ad4b-6aa4485f9258");
				Guid? parentId = new Guid("7bc83302-6b26-46ef-a6a1-3e656527faef");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldCheckbox";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Is Billable"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.is_billable\"",\""default\"":\""false\""}"",
  ""name"": ""is_billable"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""text_true"": """",
  ""text_false"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: c1e88619-37f4-4dd6-ae9b-f714191d02e3 >>
			{
				var id = new Guid("c1e88619-37f4-4dd6-ae9b-f714191d02e3");
				Guid? parentId = new Guid("7bc83302-6b26-46ef-a6a1-3e656527faef");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Billing method"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.billing_method\"",\""default\"":\""\""}"",
  ""name"": ""billing_method"",
  ""try_connect_to_entity"": ""true"",
  ""options"": """",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 50f07e9e-65a5-4feb-bf2a-4f12712305c2 >>
			{
				var id = new Guid("50f07e9e-65a5-4feb-bf2a-4f12712305c2");
				Guid? parentId = new Guid("7bc83302-6b26-46ef-a6a1-3e656527faef");
				Guid? nodeId = null;
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Hour rate"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.hour_rate\"",\""default\"":\""\""}"",
  ""name"": ""hour_rate"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""0"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: e15e2d00-e704-4212-a7d2-ee125dd687a6 >>
			{
				var id = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 8,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 4,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 6e918333-a2fa-4cf7-9ca8-662e349625a7 >>
			{
				var id = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Budget"",
  ""title_tag"": ""h3"",
  ""is_card"": ""false"",
  ""class"": """",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: aa94aac4-5048-4d82-95b2-b38536028cbb >>
			{
				var id = new Guid("aa94aac4-5048-4d82-95b2-b38536028cbb");
				Guid? parentId = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.estimated_minutes\"",\""default\"":\""\""}"",
  ""name"": ""estimated_minutes"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: ddde395b-6cee-4907-a220-a8424e091b13 >>
			{
				var id = new Guid("ddde395b-6cee-4907-a220-a8424e091b13");
				Guid? parentId = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.x_nonbillable_minutes\"",\""default\"":\""\""}"",
  ""name"": ""x_nonbillable_minutes"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""2"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 857698b9-f715-480a-bd74-29819a4dec2d >>
			{
				var id = new Guid("857698b9-f715-480a-bd74-29819a4dec2d");
				Guid? parentId = new Guid("6e918333-a2fa-4cf7-9ca8-662e349625a7");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.x_billable_minutes\"",\""default\"":\""\""}"",
  ""name"": ""x_billable_minutes"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""2"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 452a6f4c-b415-409a-b9b6-a2918a137299 >>
			{
				var id = new Guid("452a6f4c-b415-409a-b9b6-a2918a137299");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""Activity"",
  ""title_tag"": ""h3"",
  ""is_card"": ""false"",
  ""class"": """",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 164261ae-2df4-409a-8fdd-adc85c86a6dc >>
			{
				var id = new Guid("164261ae-2df4-409a-8fdd-adc85c86a6dc");
				Guid? parentId = new Guid("452a6f4c-b415-409a-b9b6-a2918a137299");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcTabNav";
				var containerId = "body";
				var options = @"{
  ""visible_tabs"": 3,
  ""render_type"": ""1"",
  ""css_class"": ""mt-4"",
  ""body_css_class"": ""pt-4"",
  ""tab1_label"": ""Comments"",
  ""tab2_label"": ""Feed"",
  ""tab3_label"": ""Timelog"",
  ""tab4_label"": ""Tab 4"",
  ""tab5_label"": ""Tab 5"",
  ""tab6_label"": ""Tab 6"",
  ""tab7_label"": ""Tab 7""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 05459068-33a7-454e-a871-94f9ddc6e5d5 >>
			{
				var id = new Guid("05459068-33a7-454e-a871-94f9ddc6e5d5");
				Guid? parentId = new Guid("164261ae-2df4-409a-8fdd-adc85c86a6dc");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcFeedList";
				var containerId = "tab2";
				var options = @"{
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""FeedItemsForRecordId\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 8099e123-1218-4008-b8e6-8ff56678d64a >>
			{
				var id = new Guid("8099e123-1218-4008-b8e6-8ff56678d64a");
				Guid? parentId = new Guid("164261ae-2df4-409a-8fdd-adc85c86a6dc");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcTimelogList";
				var containerId = "tab3";
				var options = @"{
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""TimeLogsForRecordId\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 3e15a63d-8f5f-4357-a692-b5998c31d543 >>
			{
				var id = new Guid("3e15a63d-8f5f-4357-a692-b5998c31d543");
				Guid? parentId = new Guid("164261ae-2df4-409a-8fdd-adc85c86a6dc");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcPostList";
				var containerId = "tab1";
				var options = @"{
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""CommentsForRecordId\"",\""default\"":\""\""}"",
  ""mode"": ""comments""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: b105d13c-3710-4ace-b51f-b57323912524 >>
			{
				var id = new Guid("b105d13c-3710-4ace-b51f-b57323912524");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""Details"",
  ""title_tag"": ""h3"",
  ""is_card"": ""false"",
  ""class"": ""mb-4"",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 63bada37-e9c5-497f-8d7d-4ef34fdf3cd4 >>
			{
				var id = new Guid("63bada37-e9c5-497f-8d7d-4ef34fdf3cd4");
				Guid? parentId = new Guid("b105d13c-3710-4ace-b51f-b57323912524");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 1,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 754bf941-df31-4b13-ba32-eb3c7a8c8922 >>
			{
				var id = new Guid("754bf941-df31-4b13-ba32-eb3c7a8c8922");
				Guid? parentId = new Guid("63bada37-e9c5-497f-8d7d-4ef34fdf3cd4");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.subject\"",\""default\"":\""\""}"",
  ""name"": ""subject"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 27843f6e-43ed-49e7-9cc5-ec35393e93f4 >>
			{
				var id = new Guid("27843f6e-43ed-49e7-9cc5-ec35393e93f4");
				Guid? parentId = new Guid("63bada37-e9c5-497f-8d7d-4ef34fdf3cd4");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.body\"",\""default\"":\""\""}"",
  ""name"": ""body"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 8eca6986-c648-4815-b4a6-af2580c53ce2 >>
			{
				var id = new Guid("8eca6986-c648-4815-b4a6-af2580c53ce2");
				Guid? parentId = new Guid("b105d13c-3710-4ace-b51f-b57323912524");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 3,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 70a864dc-8311-4dd3-bc13-1a3b87821e30 >>
			{
				var id = new Guid("70a864dc-8311-4dd3-bc13-1a3b87821e30");
				Guid? parentId = new Guid("8eca6986-c648-4815-b4a6-af2580c53ce2");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Status"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.status_id\"",\""default\"":\""\""}"",
  ""name"": ""status_id"",
  ""try_connect_to_entity"": ""true"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""TaskStatusesSelectOption\"",\""default\"":\""\""}"",
  ""mode"": ""3""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 1fff4a92-d045-4019-b27c-bccb1fd1cb82 >>
			{
				var id = new Guid("1fff4a92-d045-4019-b27c-bccb1fd1cb82");
				Guid? parentId = new Guid("8eca6986-c648-4815-b4a6-af2580c53ce2");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Type"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.type_id\"",\""default\"":\""\""}"",
  ""name"": ""type_id"",
  ""try_connect_to_entity"": ""true"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""TaskTypesSelectOption\"",\""default\"":\""\""}"",
  ""mode"": ""3""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: ee526509-7840-498a-9c1f-8a69d80c5f2e >>
			{
				var id = new Guid("ee526509-7840-498a-9c1f-8a69d80c5f2e");
				Guid? parentId = new Guid("8eca6986-c648-4815-b4a6-af2580c53ce2");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.priority\"",\""default\"":\""\""}"",
  ""name"": ""priority"",
  ""try_connect_to_entity"": ""true"",
  ""options"": """",
  ""mode"": ""3""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: ecc262e9-fbad-4dd1-9c98-56ad047685fb >>
			{
				var id = new Guid("ecc262e9-fbad-4dd1-9c98-56ad047685fb");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""People"",
  ""title_tag"": ""h3"",
  ""is_card"": ""false"",
  ""class"": ""mb-4"",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 101245d5-1ff9-4eb3-ba28-0b29cb56a0ec >>
			{
				var id = new Guid("101245d5-1ff9-4eb3-ba28-0b29cb56a0ec");
				Guid? parentId = new Guid("ecc262e9-fbad-4dd1-9c98-56ad047685fb");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Owner"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.owner_id\"",\""default\"":\""\""}"",
  ""name"": ""owner_id"",
  ""try_connect_to_entity"": ""true"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllUsersSelectOption\"",\""default\"":\""\""}"",
  ""mode"": ""3""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: bbe36a16-9210-415b-95f3-912482d27fd2 >>
			{
				var id = new Guid("bbe36a16-9210-415b-95f3-912482d27fd2");
				Guid? parentId = new Guid("ecc262e9-fbad-4dd1-9c98-56ad047685fb");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.created_by\"",\""default\"":\""\""}"",
  ""name"": ""created_by"",
  ""try_connect_to_entity"": ""true"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllUsersSelectOption\"",\""default\"":\""\""}"",
  ""mode"": ""3""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 651e5fb2-56df-4c46-86b3-19a641dc942d >>
			{
				var id = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Dates"",
  ""title_tag"": ""h3"",
  ""is_card"": ""false"",
  ""class"": ""mb-4"",
  ""body_class"": """",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: b9d27f33-0a0b-40e0-888b-be49c6c9f1bb >>
			{
				var id = new Guid("b9d27f33-0a0b-40e0-888b-be49c6c9f1bb");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.target_date\"",\""default\"":\""\""}"",
  ""name"": ""target_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 67be245e-07be-4b05-aaf2-b769537878f9 >>
			{
				var id = new Guid("67be245e-07be-4b05-aaf2-b769537878f9");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "body";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.start_date\"",\""default\"":\""\""}"",
  ""name"": ""start_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: b2935724-bfcc-4821-bdb2-81bc9b14f015 >>
			{
				var id = new Guid("b2935724-bfcc-4821-bdb2-81bc9b14f015");
				Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDateTime";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Created on"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.created_on\"",\""default\"":\""\""}"",
  ""name"": ""created_on"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""2""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 6f8f9a9a-a464-4175-9178-246b792738a6 >>
			{
				var id = new Guid("6f8f9a9a-a464-4175-9178-246b792738a6");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""Record.abbr\"",\""default\"":\""\""}"",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-tachometer-alt"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 3ad8d6e5-eed7-44b7-95e1-12f22714037b >>
			{
				var id = new Guid("3ad8d6e5-eed7-44b7-95e1-12f22714037b");
				Guid? parentId = new Guid("6f8f9a9a-a464-4175-9178-246b792738a6");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "toolbar";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar projectId = pageModel.TryGetDataSourceProperty<Guid>(\\\""Record.id\\\"");\\n        var pageName = pageModel.TryGetDataSourceProperty<string>(\\\""Page.Name\\\"");\\n\\n\\t\\tif (projectId == null || pageName == null)\\n\\t\\t\\treturn null;\\n\\n        var result = $\\\""<a href='/projects/projects/projects/r/{projectId}/dashboard' class='btn btn-link btn-sm {(pageName == \\\""dashboard\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Dashboard</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/feed' class='btn btn-link btn-sm {(pageName == \\\""feed\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Feed</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/b1db4466-7423-44e9-b6b9-3063222c9e15/l/tasks' class='btn btn-link btn-sm {(pageName == \\\""tasks\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Tasks</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/55c8d6e2-f26d-4689-9d1b-a8c1b9de1672/l/milestones' class='btn btn-link btn-sm {(pageName == \\\""milestones\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Milestones</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/details' class='btn btn-link btn-sm {(pageName == \\\""details\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Details</a>\\\"";\\n\\t\\treturn result;\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 6de13934-ca81-4807-bb71-cadcdbb99ca7 >>
			{
				var id = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 3,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: b37c63a7-84ea-4673-9a81-ec4313c178b7 >>
			{
				var id = new Guid("b37c63a7-84ea-4673-9a81-ec4313c178b7");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Account"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.account_id\"",\""default\"":\""\""}"",
  ""name"": ""account_id"",
  ""try_connect_to_entity"": ""true"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllAccountsSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""3""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 551483ab-262b-4541-b0dc-fadaa8de5284 >>
			{
				var id = new Guid("551483ab-262b-4541-b0dc-fadaa8de5284");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.end_date\"",\""default\"":\""\""}"",
  ""name"": ""end_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 30676929-f280-414d-8f4c-d41f851136ce >>
			{
				var id = new Guid("30676929-f280-414d-8f4c-d41f851136ce");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column3";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.budget_type\"",\""default\"":\""\""}"",
  ""name"": ""budget_type"",
  ""try_connect_to_entity"": ""true"",
  ""options"": """",
  ""mode"": ""3""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 7a7fbcd5-fb6f-40fd-a0cd-1a7c26e1c4ab >>
			{
				var id = new Guid("7a7fbcd5-fb6f-40fd-a0cd-1a7c26e1c4ab");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Project lead"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.owner_id\"",\""default\"":\""\""}"",
  ""name"": ""owner_id"",
  ""try_connect_to_entity"": ""true"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllUsersSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""3""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 8a75b1d8-8184-40ed-a977-26616239fbb7 >>
			{
				var id = new Guid("8a75b1d8-8184-40ed-a977-26616239fbb7");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.start_date\"",\""default\"":\""\""}"",
  ""name"": ""start_date"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: b2caeb51-b6a5-4e15-a317-9825511792c6 >>
			{
				var id = new Guid("b2caeb51-b6a5-4e15-a317-9825511792c6");
				Guid? parentId = new Guid("6de13934-ca81-4807-bb71-cadcdbb99ca7");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column3";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.budget_amount\"",\""default\"":\""\""}"",
  ""name"": ""budget_amount"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 89cb4088-ea04-4ce2-8cbe-5367c5741ef3 >>
			{
				var id = new Guid("89cb4088-ea04-4ce2-8cbe-5367c5741ef3");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""Entity.IconName\"",\""default\"":\""\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 8cbdfd1a-5d0e-4961-8e79-74072c133202 >>
			{
				var id = new Guid("8cbdfd1a-5d0e-4961-8e79-74072c133202");
				Guid? parentId = new Guid("89cb4088-ea04-4ce2-8cbe-5367c5741ef3");
				Guid? nodeId = null;
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""Create Project"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-plus go-green"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""/projects/projects/projects/c"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 2d3dddf7-cefb-4073-977f-4e1b6bf8935e >>
			{
				var id = new Guid("2d3dddf7-cefb-4073-977f-4e1b6bf8935e");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 3,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 5;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: df7c7cab-0e16-4e75-bb13-04666afeff81 >>
			{
				var id = new Guid("df7c7cab-0e16-4e75-bb13-04666afeff81");
				Guid? parentId = new Guid("2d3dddf7-cefb-4073-977f-4e1b6bf8935e");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.billing_method\"",\""default\"":\""\""}"",
  ""name"": ""billing_method"",
  ""try_connect_to_entity"": ""true"",
  ""options"": """",
  ""mode"": ""3""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 747c108b-ed45-46f3-b06a-113e2490888d >>
			{
				var id = new Guid("747c108b-ed45-46f3-b06a-113e2490888d");
				Guid? parentId = new Guid("2d3dddf7-cefb-4073-977f-4e1b6bf8935e");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Hour rate"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.hour_rate\"",\""default\"":\""\""}"",
  ""name"": ""hour_rate"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 302de86f-7178-4e2b-9ac1-d447163a9558 >>
			{
				var id = new Guid("302de86f-7178-4e2b-9ac1-d447163a9558");
				Guid? parentId = new Guid("2d3dddf7-cefb-4073-977f-4e1b6bf8935e");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldCheckbox";
				var containerId = "column1";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.is_billable\"",\""default\"":\""\""}"",
  ""name"": ""is_billable"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""text_true"": """",
  ""text_false"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 86506ad8-e1cb-4b46-84b9-881e0326ebaa >>
			{
				var id = new Guid("86506ad8-e1cb-4b46-84b9-881e0326ebaa");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""Projects\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""ParentRecord.abbr\"",\""default\"":\""Abbr\""}"",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""ParentRecord.name\"",\""default\"":\""Project name\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""ParentEntity.Color\"",\""default\"":\""#9c27b0\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""ParentEntity.IconName\"",\""default\"":\""fa fa-file\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 94bfe723-e5f6-478d-afb6-504edf2bdc2b >>
			{
				var id = new Guid("94bfe723-e5f6-478d-afb6-504edf2bdc2b");
				Guid? parentId = new Guid("86506ad8-e1cb-4b46-84b9-881e0326ebaa");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""Search"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-search"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer','open')"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: tasks  id: 5891b0d8-6750-4502-bd8e-fe1380f08b0c >>
			{
				var id = new Guid("5891b0d8-6750-4502-bd8e-fe1380f08b0c");
				Guid? parentId = new Guid("86506ad8-e1cb-4b46-84b9-881e0326ebaa");
				Guid? nodeId = null;
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "toolbar";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar projectId = pageModel.TryGetDataSourceProperty<Guid>(\\\""ParentRecord.id\\\"");\\n        var pageName = pageModel.TryGetDataSourceProperty<string>(\\\""Page.Name\\\"");\\n\\n\\t\\tif (projectId == null || pageName == null)\\n\\t\\t\\treturn null;\\n\\n        var result = $\\\""<a href='/projects/projects/projects/r/{projectId}/dashboard' class='btn btn-link btn-sm {(pageName == \\\""dashboard\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Dashboard</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/feed' class='btn btn-link btn-sm {(pageName == \\\""feed\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Feed</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/b1db4466-7423-44e9-b6b9-3063222c9e15/l/tasks' class='btn btn-link btn-sm {(pageName == \\\""tasks\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Tasks</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/55c8d6e2-f26d-4689-9d1b-a8c1b9de1672/l/milestones' class='btn btn-link btn-sm {(pageName == \\\""milestones\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Milestones</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/details' class='btn btn-link btn-sm {(pageName == \\\""details\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Details</a>\\\"";\\n\\t\\treturn result;\\n\\t}\\n}\\n\"",\""default\"":\""Project sub navigation\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 94be4b02-07ea-4a54-a6fc-89316fa1e90a >>
			{
				var id = new Guid("94be4b02-07ea-4a54-a6fc-89316fa1e90a");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""Record.abbr\"",\""default\"":\""\""}"",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""ParentEntity.Color\"",\""default\"":\""#9c27b0\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-info"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 0c295451-1c38-4eb0-8000-feefe912a667 >>
			{
				var id = new Guid("0c295451-1c38-4eb0-8000-feefe912a667");
				Guid? parentId = new Guid("94be4b02-07ea-4a54-a6fc-89316fa1e90a");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "toolbar";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar projectId = pageModel.TryGetDataSourceProperty<Guid>(\\\""Record.id\\\"");\\n        var pageName = pageModel.TryGetDataSourceProperty<string>(\\\""Page.Name\\\"");\\n\\n\\t\\tif (projectId == null || pageName == null)\\n\\t\\t\\treturn null;\\n\\n        var result = $\\\""<a href='/projects/projects/projects/r/{projectId}/dashboard' class='btn btn-link btn-sm {(pageName == \\\""dashboard\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Dashboard</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/feed' class='btn btn-link btn-sm {(pageName == \\\""feed\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Feed</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/b1db4466-7423-44e9-b6b9-3063222c9e15/l/tasks' class='btn btn-link btn-sm {(pageName == \\\""tasks\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Tasks</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/55c8d6e2-f26d-4689-9d1b-a8c1b9de1672/l/milestones' class='btn btn-link btn-sm {(pageName == \\\""milestones\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Milestones</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/details' class='btn btn-link btn-sm {(pageName == \\\""details\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Details</a>\\\"";\\n\\t\\treturn result;\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: feed  id: 2684f725-38e2-4f8c-92ee-e3b1ccf04aff >>
			{
				var id = new Guid("2684f725-38e2-4f8c-92ee-e3b1ccf04aff");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("acb76466-32b8-428c-81cb-47b6013879e7");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcFeedList";
				var containerId = "";
				var options = @"{
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""FeedItemsForRecordId\"",\""default\"":\""\""}""
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 6029e40b-0835-460f-b782-1e4228ea4234 >>
			{
				var id = new Guid("6029e40b-0835-460f-b782-1e4228ea4234");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 8,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 0dbdb202-7288-49e6-b922-f69e947590e5 >>
			{
				var id = new Guid("0dbdb202-7288-49e6-b922-f69e947590e5");
				Guid? parentId = new Guid("6029e40b-0835-460f-b782-1e4228ea4234");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Abbreviation"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.abbr\"",\""default\"":\""\""}"",
  ""name"": ""abbr"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""2"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 7f01d2c0-2542-4b88-b8f0-711947e4d0c6 >>
			{
				var id = new Guid("7f01d2c0-2542-4b88-b8f0-711947e4d0c6");
				Guid? parentId = new Guid("6029e40b-0835-460f-b782-1e4228ea4234");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""name"": ""name"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: dac09646-e097-4b8c-9854-cc6fb2362af5 >>
			{
				var id = new Guid("dac09646-e097-4b8c-9854-cc6fb2362af5");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 1,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: be6aa619-e380-4bf9-b279-47dda4d5f4eb >>
			{
				var id = new Guid("be6aa619-e380-4bf9-b279-47dda4d5f4eb");
				Guid? parentId = new Guid("dac09646-e097-4b8c-9854-cc6fb2362af5");
				Guid? nodeId = null;
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.description\"",\""default\"":\""\""}"",
  ""name"": ""description"",
  ""try_connect_to_entity"": ""true"",
  ""mode"": ""3"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: milestones  id: fac5a2f6-b1b4-402a-bf0d-e0a3fb4dd36a >>
			{
				var id = new Guid("fac5a2f6-b1b4-402a-bf0d-e0a3fb4dd36a");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("d07cbf70-09c6-47ee-9a13-80568e43d331");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""Projects\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""ParentRecord.abbr\"",\""default\"":\""abbr\""}"",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""ParentRecord.name\"",\""default\"":\""Project name\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""ParentEntity.Color\"",\""default\"":\""#9c27b0\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""Entity.IconName\"",\""default\"":\""fa fa-file\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: milestones  id: 4a059596-3804-435e-b535-2da1f56abb29 >>
			{
				var id = new Guid("4a059596-3804-435e-b535-2da1f56abb29");
				Guid? parentId = new Guid("fac5a2f6-b1b4-402a-bf0d-e0a3fb4dd36a");
				Guid? nodeId = null;
				var pageId = new Guid("d07cbf70-09c6-47ee-9a13-80568e43d331");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "toolbar";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar projectId = pageModel.TryGetDataSourceProperty<Guid>(\\\""ParentRecord.id\\\"");\\n        var pageName = pageModel.TryGetDataSourceProperty<string>(\\\""Page.Name\\\"");\\n\\n\\t\\tif (projectId == null || pageName == null)\\n\\t\\t\\treturn null;\\n\\n        var result = $\\\""<a href='/projects/projects/projects/r/{projectId}/dashboard' class='btn btn-link btn-sm {(pageName == \\\""dashboard\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Dashboard</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/feed' class='btn btn-link btn-sm {(pageName == \\\""feed\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Feed</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/b1db4466-7423-44e9-b6b9-3063222c9e15/l/tasks' class='btn btn-link btn-sm {(pageName == \\\""tasks\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Tasks</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/rl/55c8d6e2-f26d-4689-9d1b-a8c1b9de1672/l/milestones' class='btn btn-link btn-sm {(pageName == \\\""milestones\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Milestones</a>\\\"";\\n        result += $\\\""<a href='/projects/projects/projects/r/{projectId}/details' class='btn btn-link btn-sm {(pageName == \\\""details\\\"" ? \\\""active\\\"" : \\\""\\\"")}'>Details</a>\\\"";\\n\\t\\treturn result;\\n\\t}\\n}\\n\"",\""default\"":\""Project subnavigation\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: b6951134-f57f-4da2-8203-a8c36cc99fd7 >>
			{
				var id = new Guid("b6951134-f57f-4da2-8203-a8c36cc99fd7");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""Projects\""}"",
  ""area_sublabel"": """",
  ""title"": ""Create task"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""true"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""App.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""fa fa-plus"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: bd3ed9ae-90aa-4373-9eb9-cc677353bc6d >>
			{
				var id = new Guid("bd3ed9ae-90aa-4373-9eb9-cc677353bc6d");
				Guid? parentId = new Guid("b6951134-f57f-4da2-8203-a8c36cc99fd7");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Create task"",
  ""color"": ""1"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-save"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": ""CreateRecord""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 48105732-6025-4614-9065-55647afa9b96 >>
			{
				var id = new Guid("48105732-6025-4614-9065-55647afa9b96");
				Guid? parentId = new Guid("b6951134-f57f-4da2-8203-a8c36cc99fd7");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""Cancel"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""0\"",\""string\"":\""ReturnUrl\"",\""default\"":\""/projects/dashboard/dashboard/a/dashboard\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 1af3c0cb-a58e-4d19-89a2-2ce4b8e60945 >>
			{
				var id = new Guid("1af3c0cb-a58e-4d19-89a2-2ce4b8e60945");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "";
				var options = @"{
  ""id"": ""CreateRecord"",
  ""name"": ""CreateRecord"",
  ""hook_key"": """",
  ""method"": ""post"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: a1110167-15bd-46b7-ae3c-cc8ba87be98f >>
			{
				var id = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? parentId = new Guid("1af3c0cb-a58e-4d19-89a2-2ce4b8e60945");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 8,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 4a588a7d-ea03-4be1-ab0d-3120d98c3548 >>
			{
				var id = new Guid("4a588a7d-ea03-4be1-ab0d-3120d98c3548");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "column2";
				var options = @"{
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing System.Globalization;\\n\\npublic class SampleCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\treturn DateTime.UtcNow.ToString(\\\""o\\\"", CultureInfo.InvariantCulture);\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""created_on"",
  ""try_connect_to_entity"": ""false""
}";
				var weight = 5;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: e03e40c2-dae2-4351-947c-02295a064328 >>
			{
				var id = new Guid("e03e40c2-dae2-4351-947c-02295a064328");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Type Id"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.task_id\"",\""default\"":\""a0465e9f-5d5f-433d-acf1-1da0eaec78b4\""}"",
  ""name"": ""type_id"",
  ""try_connect_to_entity"": ""true"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""TaskTypeSelectOptions\"",\""default\"":\""\""}"",
  ""mode"": ""0""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 1739a2f0-76ba-4343-a344-9b0564096d06 >>
			{
				var id = new Guid("1739a2f0-76ba-4343-a344-9b0564096d06");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Project"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.$project_nn_task[0].id\"",\""default\"":\""\""}"",
  ""name"": ""$project_nn_task.id"",
  ""try_connect_to_entity"": ""false"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllProjectsSelectOption\"",\""default\"":\""\""}"",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 23200f02-439a-4719-ae0e-498c9dcde58c >>
			{
				var id = new Guid("23200f02-439a-4719-ae0e-498c9dcde58c");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Owner"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.owner_id\"",\""default\"":\""\""}"",
  ""name"": ""owner_id"",
  ""try_connect_to_entity"": ""false"",
  ""options"": ""{\""type\"":\""0\"",\""string\"":\""AllUsersSelectOption\"",\""default\"":\""\""}"",
  ""mode"": ""0""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: fb3d0142-e080-43ef-ba31-a79d0221c0df >>
			{
				var id = new Guid("fb3d0142-e080-43ef-ba31-a79d0221c0df");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Subject"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.subject\"",\""default\"":\""\""}"",
  ""name"": ""subject"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0"",
  ""maxlength"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: fe59f151-1e79-4df2-a034-9f45f1dcd691 >>
			{
				var id = new Guid("fe59f151-1e79-4df2-a034-9f45f1dcd691");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Description"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.body\"",\""default\"":\""\""}"",
  ""name"": ""body"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""3""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 30e99568-2727-4ce4-8da6-c97feaaf4432 >>
			{
				var id = new Guid("30e99568-2727-4ce4-8da6-c97feaaf4432");
				Guid? parentId = new Guid("a1110167-15bd-46b7-ae3c-cc8ba87be98f");
				Guid? nodeId = null;
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "column2";
				var options = @"{
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""CurrentUser.Id\"",\""default\"":\""\""}"",
  ""name"": ""created_by"",
  ""try_connect_to_entity"": ""false""
}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 7612914f-21ea-4665-9b66-385cf1cafb41 >>
			{
				var id = new Guid("7612914f-21ea-4665-9b66-385cf1cafb41");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Page.Label\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""0\"",\""string\"":\""Entity.Color\"",\""default\"":\""\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""0\"",\""string\"":\""Entity.IconName\"",\""default\"":\""\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 7b98ed4c-9240-46a2-bdd9-5904eeb3c7a3 >>
			{
				var id = new Guid("7b98ed4c-9240-46a2-bdd9-5904eeb3c7a3");
				Guid? parentId = new Guid("7612914f-21ea-4665-9b66-385cf1cafb41");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""New Task"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-fw fa-plus go-green"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""/projects/tasks/tasks/c/create?returnUrl=/projects/tasks/tasks/l/all"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: d7ef95ce-8508-4722-a5f0-3d114bda4585 >>
			{
				var id = new Guid("d7ef95ce-8508-4722-a5f0-3d114bda4585");
				Guid? parentId = new Guid("7612914f-21ea-4665-9b66-385cf1cafb41");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""Search"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-search"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer','open')"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 7e2d1d10-a9cc-4eae-b3d6-a30ab3647102 >>
			{
				var id = new Guid("7e2d1d10-a9cc-4eae-b3d6-a30ab3647102");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": ""mb-3"",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 66828292-07c7-4cc1-9060-a92798d6b95a >>
			{
				var id = new Guid("66828292-07c7-4cc1-9060-a92798d6b95a");
				Guid? parentId = new Guid("7e2d1d10-a9cc-4eae-b3d6-a30ab3647102");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""Timesheet"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": ""pt-3 pb-3"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 6bf0435c-f9c0-44b7-a801-00222cd7c0bb >>
			{
				var id = new Guid("6bf0435c-f9c0-44b7-a801-00222cd7c0bb");
				Guid? parentId = new Guid("66828292-07c7-4cc1-9060-a92798d6b95a");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcProjectWidgetTimesheet";
				var containerId = "body";
				var options = @"{
  ""project_id"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 0c29732a-945e-4bbc-9486-b86efb2897b2 >>
			{
				var id = new Guid("0c29732a-945e-4bbc-9486-b86efb2897b2");
				Guid? parentId = new Guid("7e2d1d10-a9cc-4eae-b3d6-a30ab3647102");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column1";
				var options = @"{
  ""visible_columns"": 2,
  ""class"": ""mb-3"",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: a94793d2-492a-4b7e-9fac-199f8bf46f46 >>
			{
				var id = new Guid("a94793d2-492a-4b7e-9fac-199f8bf46f46");
				Guid? parentId = new Guid("0c29732a-945e-4bbc-9486-b86efb2897b2");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""Tasks"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm h-100"",
  ""body_class"": ""p-3 align-center-col"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: bcf153c5-8c7c-4ac8-a5d9-04e288ff7ccf >>
			{
				var id = new Guid("bcf153c5-8c7c-4ac8-a5d9-04e288ff7ccf");
				Guid? parentId = new Guid("a94793d2-492a-4b7e-9fac-199f8bf46f46");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcProjectWidgetTasks";
				var containerId = "body";
				var options = @"{
  ""project_id"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 5466f4d1-20a5-4808-8bb5-aefaac756347 >>
			{
				var id = new Guid("5466f4d1-20a5-4808-8bb5-aefaac756347");
				Guid? parentId = new Guid("0c29732a-945e-4bbc-9486-b86efb2897b2");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Budget"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm h-100"",
  ""body_class"": ""p-3 align-center-col "",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 4c58c1bb-321a-41c6-954f-4c6fafe6661c >>
			{
				var id = new Guid("4c58c1bb-321a-41c6-954f-4c6fafe6661c");
				Guid? parentId = new Guid("5466f4d1-20a5-4808-8bb5-aefaac756347");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcProjectWidgetBudget";
				var containerId = "body";
				var options = @"{
  ""project_id"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 734e7201-a15e-4ae5-8ea9-b683a94f80d0 >>
			{
				var id = new Guid("734e7201-a15e-4ae5-8ea9-b683a94f80d0");
				Guid? parentId = new Guid("7e2d1d10-a9cc-4eae-b3d6-a30ab3647102");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Tasks Due Today"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm"",
  ""body_class"": ""pb-3 pt-3"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 46041332-edc7-417d-9f1a-40e4f7985869 >>
			{
				var id = new Guid("46041332-edc7-417d-9f1a-40e4f7985869");
				Guid? parentId = new Guid("734e7201-a15e-4ae5-8ea9-b683a94f80d0");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcProjectWidgetTasksDueToday";
				var containerId = "body";
				var options = @"{
  ""project_id"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: f1c53374-0efb-4612-ab94-68e8e8242ddb >>
			{
				var id = new Guid("f1c53374-0efb-4612-ab94-68e8e8242ddb");
				Guid? parentId = new Guid("7e2d1d10-a9cc-4eae-b3d6-a30ab3647102");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column1";
				var options = @"{
  ""title"": ""Task distribution"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": ""pt-3 pb-3"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: becc4486-be49-4fa6-9d3a-0d8e15606fcc >>
			{
				var id = new Guid("becc4486-be49-4fa6-9d3a-0d8e15606fcc");
				Guid? parentId = new Guid("f1c53374-0efb-4612-ab94-68e8e8242ddb");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcProjectWidgetTaskDistribution";
				var containerId = "body";
				var options = @"{
  ""project_id"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 39fa86aa-3d7a-49af-bfa9-30f1c03671eb >>
			{
				var id = new Guid("39fa86aa-3d7a-49af-bfa9-30f1c03671eb");
				Guid? parentId = new Guid("7e2d1d10-a9cc-4eae-b3d6-a30ab3647102");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Web.Components.PcSection";
				var containerId = "column2";
				var options = @"{
  ""title"": ""Overdue tasks"",
  ""title_tag"": ""strong"",
  ""is_card"": ""true"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": ""pb-3 pt-3"",
  ""is_collapsable"": ""false"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1"",
  ""is_collapsed"": ""false""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: dashboard  id: 1bb9ca30-d167-40e9-bd93-e76b3e6f4db5 >>
			{
				var id = new Guid("1bb9ca30-d167-40e9-bd93-e76b3e6f4db5");
				Guid? parentId = new Guid("39fa86aa-3d7a-49af-bfa9-30f1c03671eb");
				Guid? nodeId = null;
				var pageId = new Guid("50e4e84d-4148-4635-8372-4f2262747668");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcProjectWidgetOverdueTasks";
				var containerId = "body";
				var options = @"{
  ""project_id"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: cb0e42ee-aa06-4a92-8bb0-940e7332411e >>
			{
				var id = new Guid("cb0e42ee-aa06-4a92-8bb0-940e7332411e");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcDrawer";
				var containerId = "";
				var options = @"{
  ""title"": ""Search Tasks"",
  ""width"": ""550px"",
  ""class"": """",
  ""body_class"": """",
  ""title_action_html"": """"
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 156877b1-d1ea-4fea-be4a-62a982bef3a7 >>
			{
				var id = new Guid("156877b1-d1ea-4fea-be4a-62a982bef3a7");
				Guid? parentId = new Guid("cb0e42ee-aa06-4a92-8bb0-940e7332411e");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-156877b1-d1ea-4fea-be4a-62a982bef3a7"",
  ""name"": ""form"",
  ""hook_key"": """",
  ""method"": ""get"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: bef8058a-2c62-47a8-abd3-813636ebd4a8 >>
			{
				var id = new Guid("bef8058a-2c62-47a8-abd3-813636ebd4a8");
				Guid? parentId = new Guid("156877b1-d1ea-4fea-be4a-62a982bef3a7");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Search Tasks"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 319c5697-21c9-4799-ae6f-343586f5d2cf >>
			{
				var id = new Guid("319c5697-21c9-4799-ae6f-343586f5d2cf");
				Guid? parentId = new Guid("156877b1-d1ea-4fea-be4a-62a982bef3a7");
				Guid? nodeId = null;
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var componentName = "WebVella.Erp.Web.Components.PcGridFilterField";
				var containerId = "body";
				var options = @"{
  ""label"": ""Task contents"",
  ""name"": ""x_search"",
  ""try_connect_to_entity"": ""true"",
  ""field_type"": ""18"",
  ""query_type"": ""2"",
  ""query_options"": [
    ""2""
  ],
  ""prefix"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 630bea4c-bccf-4587-83f7-6d0d2ed5bac0 >>
			{
				var id = new Guid("630bea4c-bccf-4587-83f7-6d0d2ed5bac0");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": ""{\""type\"":\""0\"",\""string\"":\""Record.key\"",\""default\"":\""NXT-1\""}"",
  ""title"": ""{\""type\"":\""0\"",\""string\"":\""Record.subject\"",\""default\"":\""\""}"",
  ""subtitle"": """",
  ""description"": """",
  ""show_page_switch"": ""false"",
  ""color"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Linq;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar record = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""Record\\\"");\\n        var taskTypes = pageModel.TryGetDataSourceProperty<EntityRecordList>(\\\""TaskTypes\\\"");\\n        \\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (record == null || !record.Properties.ContainsKey(\\\""type_id\\\"") || taskTypes == null)\\n\\t\\t\\treturn null;\\n\\n        var taskType = taskTypes.FirstOrDefault(x=> (Guid)x[\\\""id\\\""] == (Guid)record[\\\""type_id\\\""]);\\n        if(taskType != null && taskType.Properties.ContainsKey(\\\""color\\\"") && !String.IsNullOrWhiteSpace((string)taskType[\\\""color\\\""])){\\n            return (string)taskType[\\\""color\\\""];\\n        }\\n\\n\\t\\treturn null;\\n\\t}\\n}\\n\"",\""default\"":\""#999\""}"",
  ""icon_color"": ""#fff"",
  ""icon_class"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Linq;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar record = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""Record\\\"");\\n        var taskTypes = pageModel.TryGetDataSourceProperty<EntityRecordList>(\\\""TaskTypes\\\"");\\n        \\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (record == null || !record.Properties.ContainsKey(\\\""type_id\\\"") || taskTypes == null)\\n\\t\\t\\treturn null;\\n\\n        var taskType = taskTypes.FirstOrDefault(x=> (Guid)x[\\\""id\\\""] == (Guid)record[\\\""type_id\\\""]);\\n        if(taskType != null && taskType.Properties.ContainsKey(\\\""icon_class\\\"") && !String.IsNullOrWhiteSpace((string)taskType[\\\""icon_class\\\""])){\\n            return (string)taskType[\\\""icon_class\\\""];\\n        }\\n\\n\\t\\treturn null;\\n\\t}\\n}\\n\"",\""default\"":\""fas fa-user-cog\""}"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: feed  id: 3f85bfe4-5040-42c6-a3fb-fefc9ab59b10 >>
			{
				var id = new Guid("3f85bfe4-5040-42c6-a3fb-fefc9ab59b10");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("dfe56667-174d-492d-8f84-b8ab8b70c63f");
				var componentName = "WebVella.Erp.Plugins.Next.Components.PcFeedList";
				var containerId = "";
				var options = @"{
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""FeedItemsForRecordId\"",\""default\"":\""\""}""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4 >>
			{
				var id = new Guid("6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcModal";
				var containerId = "";
				var options = @"{
  ""title"": ""Create timelog"",
  ""backdrop"": ""true"",
  ""size"": ""2"",
  ""position"": ""0""
}";
				var weight = 5;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 25ac7fb9-2737-428d-9678-90222252c024 >>
			{
				var id = new Guid("25ac7fb9-2737-428d-9678-90222252c024");
				Guid? parentId = new Guid("6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "footer";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Create log"",
  ""color"": ""19"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-plus"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": ""wv-timetrack-log""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: d6b5ad6d-4455-4828-bc46-b072aa4919f5 >>
			{
				var id = new Guid("d6b5ad6d-4455-4828-bc46-b072aa4919f5");
				Guid? parentId = new Guid("6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-timetrack-log"",
  ""name"": ""TimeTrackCreateLog"",
  ""hook_key"": ""TimeTrackCreateLog"",
  ""method"": ""post"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 3658981b-cef7-4938-9c3a-a13cd5b760a0 >>
			{
				var id = new Guid("3658981b-cef7-4938-9c3a-a13cd5b760a0");
				Guid? parentId = new Guid("d6b5ad6d-4455-4828-bc46-b072aa4919f5");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 3,
  ""class"": """",
  ""no_gutters"": ""false"",
  ""flex_vertical_alignment"": ""1"",
  ""flex_horizontal_alignment"": ""1"",
  ""container1_span"": 0,
  ""container1_span_sm"": 0,
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_selft_align"": """",
  ""container1_flex_order"": 0,
  ""container2_span"": 0,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_selft_align"": """",
  ""container2_flex_order"": 0,
  ""container3_span"": 0,
  ""container3_span_sm"": 0,
  ""container3_span_md"": 0,
  ""container3_span_lg"": 0,
  ""container3_span_xl"": 0,
  ""container3_offset"": 0,
  ""container3_offset_sm"": 0,
  ""container3_offset_md"": 0,
  ""container3_offset_lg"": 0,
  ""container3_offset_xl"": 0,
  ""container3_flex_selft_align"": """",
  ""container3_flex_order"": 0,
  ""container4_span"": 0,
  ""container4_span_sm"": 0,
  ""container4_span_md"": 0,
  ""container4_span_lg"": 0,
  ""container4_span_xl"": 0,
  ""container4_offset"": 0,
  ""container4_offset_sm"": 0,
  ""container4_offset_md"": 0,
  ""container4_offset_lg"": 0,
  ""container4_offset_xl"": 0,
  ""container4_flex_selft_align"": """",
  ""container4_flex_order"": 0,
  ""container5_span"": 0,
  ""container5_span_sm"": 0,
  ""container5_span_md"": 0,
  ""container5_span_lg"": 0,
  ""container5_span_xl"": 0,
  ""container5_offset"": 0,
  ""container5_offset_sm"": 0,
  ""container5_offset_md"": 0,
  ""container5_offset_lg"": 0,
  ""container5_offset_xl"": 0,
  ""container5_flex_selft_align"": """",
  ""container5_flex_order"": 0,
  ""container6_span"": 0,
  ""container6_span_sm"": 0,
  ""container6_span_md"": 0,
  ""container6_span_lg"": 0,
  ""container6_span_xl"": 0,
  ""container6_offset"": 0,
  ""container6_offset_sm"": 0,
  ""container6_offset_md"": 0,
  ""container6_offset_lg"": 0,
  ""container6_offset_xl"": 0,
  ""container6_flex_selft_align"": """",
  ""container6_flex_order"": 0,
  ""container7_span"": 0,
  ""container7_span_sm"": 0,
  ""container7_span_md"": 0,
  ""container7_span_lg"": 0,
  ""container7_span_xl"": 0,
  ""container7_offset"": 0,
  ""container7_offset_sm"": 0,
  ""container7_offset_md"": 0,
  ""container7_offset_lg"": 0,
  ""container7_offset_xl"": 0,
  ""container7_flex_selft_align"": """",
  ""container7_flex_order"": 0,
  ""container8_span"": 0,
  ""container8_span_sm"": 0,
  ""container8_span_md"": 0,
  ""container8_span_lg"": 0,
  ""container8_span_xl"": 0,
  ""container8_offset"": 0,
  ""container8_offset_sm"": 0,
  ""container8_offset_md"": 0,
  ""container8_offset_lg"": 0,
  ""container8_offset_xl"": 0,
  ""container8_flex_selft_align"": """",
  ""container8_flex_order"": 0,
  ""container9_span"": 0,
  ""container9_span_sm"": 0,
  ""container9_span_md"": 0,
  ""container9_span_lg"": 0,
  ""container9_span_xl"": 0,
  ""container9_offset"": 0,
  ""container9_offset_sm"": 0,
  ""container9_offset_md"": 0,
  ""container9_offset_lg"": 0,
  ""container9_offset_xl"": 0,
  ""container9_flex_selft_align"": """",
  ""container9_flex_order"": 0,
  ""container10_span"": 0,
  ""container10_span_sm"": 0,
  ""container10_span_md"": 0,
  ""container10_span_lg"": 0,
  ""container10_span_xl"": 0,
  ""container10_offset"": 0,
  ""container10_offset_sm"": 0,
  ""container10_offset_md"": 0,
  ""container10_offset_lg"": 0,
  ""container10_offset_xl"": 0,
  ""container10_flex_selft_align"": """",
  ""container10_flex_order"": 0,
  ""container11_span"": 0,
  ""container11_span_sm"": 0,
  ""container11_span_md"": 0,
  ""container11_span_lg"": 0,
  ""container11_span_xl"": 0,
  ""container11_offset"": 0,
  ""container11_offset_sm"": 0,
  ""container11_offset_md"": 0,
  ""container11_offset_lg"": 0,
  ""container11_offset_xl"": 0,
  ""container11_flex_selft_align"": """",
  ""container11_flex_order"": 0,
  ""container12_span"": 0,
  ""container12_span_sm"": 0,
  ""container12_span_md"": 0,
  ""container12_span_lg"": 0,
  ""container12_span_xl"": 0,
  ""container12_offset"": 0,
  ""container12_offset_sm"": 0,
  ""container12_offset_md"": 0,
  ""container12_offset_lg"": 0,
  ""container12_offset_xl"": 0,
  ""container12_flex_selft_align"": """",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 6b3e9fec-7fc1-4455-8dcc-a3b67f4ca427 >>
			{
				var id = new Guid("6b3e9fec-7fc1-4455-8dcc-a3b67f4ca427");
				Guid? parentId = new Guid("3658981b-cef7-4938-9c3a-a13cd5b760a0");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Log Date"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""CurrentDate\"",\""default\"":\""\""}"",
  ""name"": ""logged_on"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 6b0ee717-f4af-47fb-b441-125a755af01b >>
			{
				var id = new Guid("6b0ee717-f4af-47fb-b441-125a755af01b");
				Guid? parentId = new Guid("3658981b-cef7-4938-9c3a-a13cd5b760a0");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldCheckbox";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Billable"",
  ""label_mode"": ""0"",
  ""value"": ""true"",
  ""name"": ""is_billable"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0"",
  ""text_true"": ""billable time"",
  ""text_false"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 9dcca796-cb6d-4c7f-bb63-761cff4c218a >>
			{
				var id = new Guid("9dcca796-cb6d-4c7f-bb63-761cff4c218a");
				Guid? parentId = new Guid("3658981b-cef7-4938-9c3a-a13cd5b760a0");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Logged Minutes"",
  ""label_mode"": ""0"",
  ""value"": """",
  ""name"": ""minutes"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0"",
  ""decimal_digits"": 2,
  ""min"": 0,
  ""max"": 0,
  ""step"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: d11a258c-2ad3-4421-84db-990aa7683a2d >>
			{
				var id = new Guid("d11a258c-2ad3-4421-84db-990aa7683a2d");
				Guid? parentId = new Guid("d6b5ad6d-4455-4828-bc46-b072aa4919f5");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "body";
				var options = @"{
  ""value"": """",
  ""name"": ""task_id"",
  ""try_connect_to_entity"": ""false""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 418b58a8-88d2-4dfc-b4a9-22617dab76c4 >>
			{
				var id = new Guid("418b58a8-88d2-4dfc-b4a9-22617dab76c4");
				Guid? parentId = new Guid("d6b5ad6d-4455-4828-bc46-b072aa4919f5");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcFieldTextarea";
				var containerId = "body";
				var options = @"{
  ""label_text"": ""Description"",
  ""label_mode"": ""0"",
  ""value"": """",
  ""name"": ""body"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""0"",
  ""height"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 9946104a-a6ec-4a0b-b996-7bc630c16287 >>
			{
				var id = new Guid("9946104a-a6ec-4a0b-b996-7bc630c16287");
				Guid? parentId = new Guid("6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "footer";
				var options = @"{
  ""type"": ""0"",
  ""text"": ""Cancel"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:'wv-6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4',action:'close',payload:null})"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 402274a5-b89d-4829-b8a4-e975c8552dd7 >>
			{
				var id = new Guid("402274a5-b89d-4829-b8a4-e975c8552dd7");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcDrawer";
				var containerId = "";
				var options = @"{
  ""title"": ""Search tasks"",
  ""width"": ""550px"",
  ""class"": """",
  ""body_class"": """",
  ""title_action_html"": """"
}";
				var weight = 6;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: e6f79322-616c-42db-b603-a0cfb3125f9d >>
			{
				var id = new Guid("e6f79322-616c-42db-b603-a0cfb3125f9d");
				Guid? parentId = new Guid("402274a5-b89d-4829-b8a4-e975c8552dd7");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-e6f79322-616c-42db-b603-a0cfb3125f9d"",
  ""name"": ""form"",
  ""hook_key"": """",
  ""method"": ""get"",
  ""label_mode"": ""1"",
  ""mode"": ""1""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: b58e6f1f-e44d-4470-b3c6-5a46e05b3667 >>
			{
				var id = new Guid("b58e6f1f-e44d-4470-b3c6-5a46e05b3667");
				Guid? parentId = new Guid("e6f79322-616c-42db-b603-a0cfb3125f9d");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcGridFilterField";
				var containerId = "body";
				var options = @"{
  ""label"": ""Task content"",
  ""name"": ""x_fts"",
  ""try_connect_to_entity"": ""false"",
  ""field_type"": ""18"",
  ""query_type"": ""2"",
  ""query_options"": [
    ""2""
  ],
  ""prefix"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: aac4d9a2-b3f7-4c3a-8f2a-b5242a83da21 >>
			{
				var id = new Guid("aac4d9a2-b3f7-4c3a-8f2a-b5242a83da21");
				Guid? parentId = new Guid("e6f79322-616c-42db-b603-a0cfb3125f9d");
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Search tasks"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: track-time  id: 663aa356-14d2-4e22-8dc8-a12b9fc971a1 >>
			{
				var id = new Guid("663aa356-14d2-4e22-8dc8-a12b9fc971a1");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var componentName = "WebVella.Erp.Web.Components.PcJavaScriptBlock";
				var containerId = "";
				var options = @"{
  ""script"": ""$(function(){\r\n\r\n\tfunction RunTimer(wvTimerEl) {\r\n\t\tvar recordRow = $(wvTimerEl).closest(\""tr\"");\r\n\t\trecordRow.addClass(\""go-bkg-orange-light\"");\r\n\t\tvar inputTotalEl = recordRow.find(\""input[name='timelog_total_seconds']\"");\r\n\t\tvar totalLoggedSeconds = $(inputTotalEl).val();\r\n\t\tvar totalLoggedSecondsDec = new Decimal(totalLoggedSeconds);\t\t\r\n\t\tvar loggedHours = totalLoggedSecondsDec.div(3600).toDecimalPlaces(0,Decimal.ROUND_DOWN);\r\n\t\tvar totalLeft = totalLoggedSecondsDec.minus(loggedHours.times(3600));\r\n\t\tvar loggedMinutes = totalLeft.div(60).toDecimalPlaces(0,Decimal.ROUND_DOWN);\r\n\t\ttotalLeft = totalLeft.minus(loggedMinutes.times(60));\r\n\t\tvar loggedSeconds = totalLeft;\r\n\t\tvar loggedHoursString = loggedHours.toString();\r\n\t\tif (loggedHours.lessThan(10)) {\r\n\t\t\tloggedHoursString = \""0\""+loggedHoursString;\r\n\t\t}\r\n\t\tvar loggedMinutesString = loggedMinutes.toString();\r\n\t\tif (loggedMinutes.lessThan(10)) {\r\n\t\t\tloggedMinutesString = \""0\""+loggedMinutesString;\r\n\t\t}\r\n\t\tvar loggedSecondsString = loggedSeconds.toString();\r\n\t\tif (loggedSeconds.lessThan(10)) {\r\n\t\t\tloggedSecondsString = \""0\""+loggedSecondsString;\r\n\t\t}\r\n\t\trecordRow.find(\"".timer-td span\"").html(loggedHoursString + ' : ' + loggedMinutesString + ' : ' + loggedSecondsString);\r\n\t\trecordRow.find(\"".timer-td span\"").addClass(\""go-orange\"").removeClass(\""go-gray\"");\r\n\t\ttotalLoggedSecondsDec = totalLoggedSecondsDec.plus(1);\r\n\t\t$(inputTotalEl).val(totalLoggedSecondsDec);\r\n\t}\r\n\r\n\tfunction EvaluateTimer(wvTimerEl) {\r\n\t\tvar recordRow = $(wvTimerEl).closest(\""tr\"");\r\n\t\tvar inputLogStartedOn = recordRow.find(\""input[name='timelog_started_on']\"");\r\n\t\tif (inputLogStartedOn.val()) {\r\n\t\t\trecordRow.find(\"".start-log-group\"").addClass(\""d-none\"");\r\n\t\t\trecordRow.find(\"".stop-log-group\"").removeClass(\""d-none\"");\r\n\t\t\tRunTimer(wvTimerEl);\r\n\t\t\tsetInterval(function () {\r\n\t\t\t\tRunTimer(wvTimerEl);\r\n\t\t\t}, 1000);\r\n\t\t}\r\n\t\telse {\r\n\t\t\trecordRow.find(\"".start-log-group\"").removeClass(\""d-none\"");\r\n\t\t\trecordRow.find(\"".stop-log-group\"").addClass(\""d-none\"");\r\n\t\t}\r\n\r\n\t}\r\n\r\n\r\n    $(\"".wv-timer\"").each(function(){\r\n\t\tEvaluateTimer(this);\r\n    });\r\n    \r\n    $(\"".start-log\"").click(function(){\r\n        var clickedBtn = $(this);\r\n\t\tvar recordRow = clickedBtn.closest(\""tr\"");\r\n\t\tvar recordTimer = recordRow.find(\"".wv-timer\"");\r\n\r\n        var clickedBtnIcon = clickedBtn.find(\"".fa\"");\r\n        var clickedBtnTd = clickedBtn.closest(\""td\"");\r\n        var hiddenTaskInput = clickedBtnTd.find(\""input[name='task_id']\"");\r\n        var startLogGroup = clickedBtnTd.find(\"".start-log-group\"");\r\n        var stopLogGroup = clickedBtnTd.find(\"".stop-log-group\"");\r\n        var taskId = hiddenTaskInput.val();\r\n        \r\n        clickedBtn.prop('disabled', true);\r\n        clickedBtnIcon.removeClass(\""fa-play\"").addClass(\""fa-spin fa-spinner\"");\r\n        \r\n\t\t$.ajax({\r\n\t\ttype: \""POST\"",\r\n\t\turl: \""/api/v3.0/p/next/timelog/start?taskId=\""+taskId,\r\n\t\tdataType:\""json\"",\r\n\t\tsuccess: function(response){\r\n\t\t\tif(!response.success){\r\n\t\t\t\ttoastr.error(response.message, 'Error!', { closeButton: false, tapToDismiss: true });\r\n\t\t\t\tclickedBtn.prop('disabled', false);\r\n\t\t\t\tclickedBtnIcon.addClass(\""fa-play\"").removeClass(\""fa-spin fa-spinner\"");\r\n\t\t\t}\r\n\t\t\telse{\r\n\t\t\t\tstartLogGroup.addClass(\""d-none\"");\r\n\t\t\t\tstopLogGroup.removeClass(\""d-none\"");\r\n\t\t\t\tclickedBtn.prop('disabled', false);\r\n\t\t\t\tclickedBtnIcon.addClass(\""fa-play\"").removeClass(\""fa-spin fa-spinner\"");\r\n\t\t\t\trecordRow.find(\""input[name='timelog_started_on']\"").val(moment().toISOString());\r\n\t\t\t\tEvaluateTimer(recordTimer);\r\n\t\t\t}\r\n        \r\n\t\t},\r\n\t\terror:function(XMLHttpRequest, textStatus, errorThrown) {\r\n\t\t\ttoastr.error(textStatus, 'Error!', { closeButton: false, tapToDismiss: true });\r\n\t\t}\r\n\t\t});        \r\n\r\n    });\r\n\r\n\t$(\"".stop-log\"").click(function(){\r\n        var clickedBtn = $(this);\r\n\t\tvar recordRow = clickedBtn.closest(\""tr\"");\t\t\r\n\t\tvar inputTimelogStartEl = recordRow.find(\""input[name='timelog_started_on']\"");\r\n\t\tvar inputTaskId =  recordRow.find(\""input[name='task_id']\"");\r\n\t\tvar formId = \""wv-6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4\"";\r\n\t\tvar formEl = $(\""#\""+formId);\r\n\t\tvar minutesFormInputEl = formEl.find(\""input[name='minutes']\"");\r\n\t\tvar taskIdFormInputEl = formEl.find(\""input[name='task_id']\"");\r\n\t\t//set minutes\r\n\t\tvar logstartDate = $(inputTimelogStartEl).val();\r\n\t\tvar totalLoggedSeconds = moment().utc().diff(logstartDate, 'seconds');\r\n\t\tvar totalLoggedSecondsDec = new Decimal(totalLoggedSeconds);\t\r\n\t\tvar totalMinutes = totalLoggedSecondsDec.div(60).toDecimalPlaces(0,Decimal.ROUND_DOWN);\r\n\t\tminutesFormInputEl.val(totalMinutes.toNumber());\r\n\t\t//set taskId\r\n\t\ttaskIdFormInputEl.val(inputTaskId.val());\r\n\t\tErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:formId,action:'open',payload:null});\r\n\t});\r\n\t$(\"".manual-log\"").click(function(){\r\n        var clickedBtn = $(this);\r\n\t\tvar recordRow = clickedBtn.closest(\""tr\"");\t\t\r\n\t\tvar inputTotalEl = recordRow.find(\""input[name='timelog_total_seconds']\"");\r\n\t\tvar inputTaskId =  recordRow.find(\""input[name='task_id']\"");\r\n\t\tvar formId = \""wv-6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4\"";\r\n\t\tvar formEl = $(\""#\""+formId);\r\n\t\tvar taskIdFormInputEl = formEl.find(\""input[name='task_id']\"");\r\n\t\t//set taskId\r\n\t\ttaskIdFormInputEl.val(inputTaskId.val());\r\n\t\tErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcModal',{htmlId:formId,action:'open',payload:null});\t\t\r\n\t});\r\n});""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: feed  id: ff5b4808-9c2a-4d4f-8eaf-a4878594c55a >>
			{
				var id = new Guid("ff5b4808-9c2a-4d4f-8eaf-a4878594c55a");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("acb76466-32b8-428c-81cb-47b6013879e7");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "";
				var options = @"{
  ""label_text"": """",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n        try{\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar queryRecord = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RequestQuery\\\"");\\n        var type = \\\""\\\"";\\n        if(queryRecord.Properties.ContainsKey(\\\""type\\\""))\\n            type = (string)queryRecord[\\\""type\\\""];\\n\\n        var result = \\\""\\\"";\\n        result += $\\\""<ul class=\\\\\\\""nav nav-pills nav-sm mb-4\\\\\\\"">\\\"";\\n        result += $\\\""<li class=\\\\\\\""nav-item\\\\\\\"">\\\"";\\n        result += $\\\""<a class=\\\\\\\""nav-link {(type == \\\""\\\"" ? \\\""active\\\"" : \\\""\\\"")}\\\\\\\"" href=\\\\\\\""/projects/feed/feed/a/feed\\\\\\\"">All Feeds</a>\\\"";\\n\\t    result += $\\\""</li>\\\"";\\n\\t\\tresult += $\\\""<li class=\\\\\\\""nav-item\\\\\\\"">\\\"";\\n\\t\\tresult += $\\\""<a class=\\\\\\\""nav-link  {(type == \\\""task\\\"" ? \\\""active\\\"" : \\\""\\\"")}\\\\\\\"" href=\\\\\\\""/projects/feed/feed/a/feed?type=task\\\\\\\"">Task</a>\\\"";\\n\\t    result += $\\\""</li>\\\"";\\n\\t\\tresult += $\\\""<li class=\\\\\\\""nav-item\\\\\\\"">\\\"";\\n\\t\\tresult += $\\\""<a class=\\\\\\\""nav-link  {(type == \\\""comment\\\"" ? \\\""active\\\"" : \\\""\\\"")}\\\\\\\"" href=\\\\\\\""/projects/feed/feed/a/feed?type=comment\\\\\\\"">Comment</a>\\\"";\\n\\t    result += $\\\""</li>\\\"";\\n\\t\\tresult += $\\\""<li class=\\\\\\\""nav-item\\\\\\\"">\\\"";\\n\\t\\tresult += $\\\""<a class=\\\\\\\""nav-link  {(type == \\\""timelog\\\"" ? \\\""active\\\"" : \\\""\\\"")}\\\\\\\"" href=\\\\\\\""/projects/feed/feed/a/feed?type=timelog\\\\\\\"">Timelog</a>\\\"";\\n\\t    result += $\\\""</li>\\\"";\\t    \\n        result += $\\\""</ul>\\\"";\\t    \\n\\t\\treturn result;\\n        }\\n        catch(Exception ex){\\n            return ex.Message;\\n        }\\n\\t}\\n}\\n\"",\""default\"":\""Feed type Pill navigation\""}"",
  ""name"": ""field"",
  ""try_connect_to_entity"": ""false"",
  ""mode"": ""4"",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1""
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create data source*** Name: AllAccounts >>
			{
				var id = new Guid("61d21547-b353-48b8-8b75-b727680da79e");
				var name = @"AllAccounts";
				var description = @"Lists all accounts in the system";
				var eqlText = @"SELECT id,name 
FROM account
where name CONTAINS @name
ORDER BY @sortBy ASC
PAGE @page
PAGESIZE @pageSize";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_account.""id"" AS ""id"",
	 rec_account.""name"" AS ""name"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_account
WHERE  ( rec_account.""name""  ILIKE  @name ) 
ORDER BY rec_account.""name"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""name"",""type"":""text"",""value"":""null""},{""name"":""sortBy"",""type"":""text"",""value"":""name""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
				var weight = 10;
				var entityName = @"account";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: AllProjects >>
			{
				var id = new Guid("96218f33-42f1-4ff1-926c-b1765e1f8c6e");
				var name = @"AllProjects";
				var description = @"all project records";
				var eqlText = @"SELECT id,abbr,name,$user_1n_project_owner.username
FROM project
WHERE name CONTAINS @filterName
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_project.""id"" AS ""id"",
	 rec_project.""abbr"" AS ""abbr"",
	 rec_project.""name"" AS ""name"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_project_owner
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_project_owner.""id"" AS ""id"",
		 user_1n_project_owner.""username"" AS ""username"" 
	 FROM rec_user user_1n_project_owner
	 WHERE user_1n_project_owner.id = rec_project.owner_id ) d )::jsonb AS ""$user_1n_project_owner""	
	-------< $user_1n_project_owner
FROM rec_project
WHERE  ( rec_project.""name""  ILIKE  @filterName ) 
ORDER BY rec_project.""name"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""name""},{""name"":""sortOrder"",""type"":""text"",""value"":""asc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""filterName"",""type"":""text"",""value"":""null""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$user_1n_project_owner"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"project";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: AllUsers >>
			{
				var id = new Guid("f3e5ab66-9257-42f9-8bdf-f0233dd4aedd");
				var name = @"AllUsers";
				var description = @"All system users";
				var eqlText = @"SELECT *
FROM user
ORDER BY username asc
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_user.""id"" AS ""id"",
	 rec_user.""created_on"" AS ""created_on"",
	 rec_user.""first_name"" AS ""first_name"",
	 rec_user.""last_name"" AS ""last_name"",
	 rec_user.""username"" AS ""username"",
	 rec_user.""email"" AS ""email"",
	 rec_user.""password"" AS ""password"",
	 rec_user.""last_logged_in"" AS ""last_logged_in"",
	 rec_user.""enabled"" AS ""enabled"",
	 rec_user.""verified"" AS ""verified"",
	 rec_user.""preferences"" AS ""preferences"",
	 rec_user.""image"" AS ""image"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_user
ORDER BY rec_user.""username"" ASC
) X
";
				var parametersJson = @"[]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""first_name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""last_name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""email"",""type"":6,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""password"",""type"":13,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""last_logged_in"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""enabled"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""verified"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""preferences"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""image"",""type"":9,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
				var weight = 10;
				var entityName = @"user";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: ProjectOpenTasks >>
			{
				var id = new Guid("46aab266-e2a8-4b67-9155-39ec1cf3bccb");
				var name = @"ProjectOpenTasks";
				var description = @"All open tasks for a project";
				var eqlText = @"SELECT *,$milestone_nn_task.name,$task_status_1n_task.label,$task_type_1n_task.label
FROM task
WHERE $project_nn_task.id = @projectId
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""start_date"" AS ""start_date"",
	 rec_task.""target_date"" AS ""target_date"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""x_nonbillable_hours"" AS ""x_nonbillable_hours"",
	 rec_task.""x_billable_hours"" AS ""x_billable_hours"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""priority"" AS ""priority"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $milestone_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 milestone_nn_task.""id"" AS ""id"",
		 milestone_nn_task.""name"" AS ""name""
	 FROM rec_milestone milestone_nn_task
	 LEFT JOIN  rel_milestone_nn_task milestone_nn_task_target ON milestone_nn_task_target.target_id = rec_task.id
	 WHERE milestone_nn_task.id = milestone_nn_task_target.origin_id )d  )::jsonb AS ""$milestone_nn_task"",
	
	-------< $milestone_nn_task	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	
	-------< $task_status_1n_task	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task""	
	-------< $task_type_1n_task
FROM rec_task
LEFT OUTER JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
LEFT OUTER JOIN  rec_project project_nn_task_tar_org ON project_nn_task_target.origin_id = project_nn_task_tar_org.id
WHERE  ( project_nn_task_tar_org.""id"" = @projectId ) 
ORDER BY rec_task.""id"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""projectId"",""type"":""guid"",""value"":""00000000-0000-0000-0000-000000000000""},{""name"":""sortBy"",""type"":""text"",""value"":""id""},{""name"":""sortOrder"",""type"":""text"",""value"":""asc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_date"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""target_date"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_hours"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_hours"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$milestone_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: ProjectAuxData >>
			{
				var id = new Guid("3c5a9d64-47ea-466a-8b0e-49e61df58bd1");
				var name = @"ProjectAuxData";
				var description = @"getting related data for the current project";
				var eqlText = @"SELECT $user_1n_project_owner.id
FROM project
WHERE id = @recordId
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_project_owner
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_project_owner.""id"" AS ""id"" 
	 FROM rec_user user_1n_project_owner
	 WHERE user_1n_project_owner.id = rec_project.owner_id ) d )::jsonb AS ""$user_1n_project_owner""	
	-------< $user_1n_project_owner
FROM rec_project
WHERE  ( rec_project.""id"" = @recordId ) 
) X
";
				var parametersJson = @"[{""name"":""recordId"",""type"":""guid"",""value"":""00000000-0000-0000-0000-000000000000""}]";
				var fieldsJson = @"[{""name"":""$user_1n_project_owner"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"project";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: TaskStatuses >>
			{
				var id = new Guid("fad53f3d-4d3b-4c7b-8cd2-23e96a086ad8");
				var name = @"TaskStatuses";
				var description = @"All task statuses";
				var eqlText = @"SELECT *
FROM task_status
ORDER BY label asc";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task_status.""id"" AS ""id"",
	 rec_task_status.""is_closed"" AS ""is_closed"",
	 rec_task_status.""is_default"" AS ""is_default"",
	 rec_task_status.""l_scope"" AS ""l_scope"",
	 rec_task_status.""label"" AS ""label"",
	 rec_task_status.""sort_index"" AS ""sort_index"",
	 rec_task_status.""is_system"" AS ""is_system"",
	 rec_task_status.""is_enabled"" AS ""is_enabled"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_task_status
ORDER BY rec_task_status.""label"" ASC
) X
";
				var parametersJson = @"[]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_closed"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_default"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""sort_index"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_system"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_enabled"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
				var weight = 10;
				var entityName = @"task_status";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: ProjectWidgetMyOverdueTasks >>
			{
				var id = new Guid("946919a6-e4cd-41a2-97dc-1069d73adcd1");
				var name = @"ProjectWidgetMyOverdueTasks";
				var description = @"all my overdue tasks";
				var eqlText = @"SELECT *,$project_nn_task.name
FROM task
WHERE owner_id = @userId AND target_date < @currentDate
ORDER BY target_date ASC, priority DESC ";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""x_search"" AS ""x_search"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""start_date"" AS ""start_date"",
	 rec_task.""target_date"" AS ""target_date"",
	 rec_task.""priority"" AS ""priority"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""name"" AS ""name""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task""	
	-------< $project_nn_task

FROM rec_task
WHERE  (  ( rec_task.""owner_id"" = @userId )  AND  ( rec_task.""target_date"" < @currentDate )  ) 
ORDER BY rec_task.""target_date"" ASC , rec_task.""priority"" DESC
) X
";
				var parametersJson = @"[{""name"":""userId"",""type"":""guid"",""value"":""guid.empty""},{""name"":""currentDate"",""type"":""date"",""value"":""now""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""target_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: ProjectWidgetMyTasksDueToday >>
			{
				var id = new Guid("eae07b63-9bf4-4e25-80af-df5228dedf35");
				var name = @"ProjectWidgetMyTasksDueToday";
				var description = @"My tasks due today";
				var eqlText = @"SELECT *,$project_nn_task.name
FROM task
WHERE owner_id = @userId AND target_date = @currentDate
ORDER BY priority DESC
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""x_search"" AS ""x_search"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""start_date"" AS ""start_date"",
	 rec_task.""target_date"" AS ""target_date"",
	 rec_task.""priority"" AS ""priority"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""name"" AS ""name""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task""	
	-------< $project_nn_task

FROM rec_task
WHERE  (  ( rec_task.""owner_id"" = @userId )  AND  ( rec_task.""target_date"" = @currentDate )  ) 
ORDER BY rec_task.""priority"" DESC
) X
";
				var parametersJson = @"[{""name"":""userId"",""type"":""guid"",""value"":""guid.empty""},{""name"":""currentDate"",""type"":""date"",""value"":""now""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""target_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: TaskAuxData >>
			{
				var id = new Guid("587d963b-613f-4e77-a7d4-719f631ce6b2");
				var name = @"TaskAuxData";
				var description = @"getting related data for the current task";
				var eqlText = @"SELECT $project_nn_task.id,$project_nn_task.abbr
FROM task
WHERE id = @recordId";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""abbr"" AS ""abbr""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task""	
	-------< $project_nn_task
FROM rec_task
WHERE  ( rec_task.""id"" = @recordId ) 
) X
";
				var parametersJson = @"[{""name"":""recordId"",""type"":""guid"",""value"":""00000000-0000-0000-0000-000000000000""}]";
				var fieldsJson = @"[{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: TaskTypes >>
			{
				var id = new Guid("4857ace4-fcfc-4803-ad86-7c7afba91ce0");
				var name = @"TaskTypes";
				var description = @"All task types";
				var eqlText = @"SELECT *
FROM task_type
WHERE l_scope CONTAINS @scope
ORDER BY sort_index asc";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task_type.""id"" AS ""id"",
	 rec_task_type.""is_default"" AS ""is_default"",
	 rec_task_type.""l_scope"" AS ""l_scope"",
	 rec_task_type.""label"" AS ""label"",
	 rec_task_type.""sort_index"" AS ""sort_index"",
	 rec_task_type.""is_system"" AS ""is_system"",
	 rec_task_type.""is_enabled"" AS ""is_enabled"",
	 rec_task_type.""icon_class"" AS ""icon_class"",
	 rec_task_type.""color"" AS ""color"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_task_type
WHERE  ( rec_task_type.""l_scope""  ILIKE  @scope ) 
ORDER BY rec_task_type.""sort_index"" ASC
) X
";
				var parametersJson = @"[{""name"":""scope"",""type"":""text"",""value"":""projects""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_default"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""sort_index"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_system"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_enabled"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
				var weight = 10;
				var entityName = @"task_type";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: AllTasks >>
			{
				var id = new Guid("5a6e9d56-63bc-43b1-b95e-24838db9f435");
				var name = @"AllTasks";
				var description = @"All tasks selection";
				var eqlText = @"SELECT *,$project_nn_task.abbr,$user_1n_task.username,$task_status_1n_task.label,$task_type_1n_task.label,$task_type_1n_task.icon_class,$task_type_1n_task.color,$user_1n_task_creator.username
FROM task
WHERE x_search CONTAINS @searchQuery
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""start_date"" AS ""start_date"",
	 rec_task.""target_date"" AS ""target_date"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""x_nonbillable_hours"" AS ""x_nonbillable_hours"",
	 rec_task.""x_billable_hours"" AS ""x_billable_hours"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""estimation"" AS ""estimation"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""x_search"" AS ""x_search"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""abbr"" AS ""abbr""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task"",
	-------< $project_nn_task
	------->: $user_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task.""id"" AS ""id"",
		 user_1n_task.""username"" AS ""username"" 
	 FROM rec_user user_1n_task
	 WHERE user_1n_task.id = rec_task.owner_id ) d )::jsonb AS ""$user_1n_task"",
	-------< $user_1n_task
	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	-------< $task_status_1n_task
	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"",
		 task_type_1n_task.""icon_class"" AS ""icon_class"",
		 task_type_1n_task.""color"" AS ""color"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task"",
	-------< $task_type_1n_task
	------->: $user_1n_task_creator
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task_creator.""id"" AS ""id"",
		 user_1n_task_creator.""username"" AS ""username"" 
	 FROM rec_user user_1n_task_creator
	 WHERE user_1n_task_creator.id = rec_task.created_by ) d )::jsonb AS ""$user_1n_task_creator""	
	-------< $user_1n_task_creator

FROM rec_task
WHERE  ( rec_task.""x_search""  ILIKE  @searchQuery ) 
ORDER BY rec_task.""target_date"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""target_date""},{""name"":""sortOrder"",""type"":""text"",""value"":""asc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_date"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""target_date"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_hours"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_hours"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimation"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task_creator"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: ProjectWidgetMyTasks >>
			{
				var id = new Guid("c44eab77-c81e-4f55-95c8-4949b275fc99");
				var name = @"ProjectWidgetMyTasks";
				var description = @"top 5 upcoming tasks";
				var eqlText = @"SELECT *,$project_nn_task.name
FROM task
WHERE owner_id = @userId AND target_date > @currentDate
ORDER BY target_date ASC, priority DESC
PAGE 1
PAGESIZE 5
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""x_search"" AS ""x_search"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""start_date"" AS ""start_date"",
	 rec_task.""target_date"" AS ""target_date"",
	 rec_task.""priority"" AS ""priority"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""name"" AS ""name""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task""	
	-------< $project_nn_task

FROM rec_task
WHERE  (  ( rec_task.""owner_id"" = @userId )  AND  ( rec_task.""target_date"" > @currentDate )  ) 
ORDER BY rec_task.""target_date"" ASCrec_task.""priority"" DESC
LIMIT 5
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""userId"",""type"":""guid"",""value"":""guid.empty""},{""name"":""currentDate"",""type"":""date"",""value"":""now""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""target_date"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: TaskComments >>
			{
				var id = new Guid("f68fa8be-b957-4692-b459-4da62d23f472");
				var name = @"TaskComments";
				var description = @"All comments for a certain task";
				var eqlText = @"SELECT *,$task_nn_comment.id,$task_nn_comment.$project_nn_task.id,$case_nn_comment.id FROM comment
WHERE id = @commentId";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_comment.""id"" AS ""id"",
	 rec_comment.""body"" AS ""body"",
	 rec_comment.""created_by"" AS ""created_by"",
	 rec_comment.""created_on"" AS ""created_on"",
	 rec_comment.""l_scope"" AS ""l_scope"",
	 rec_comment.""parent_id"" AS ""parent_id"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $task_nn_comment
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 task_nn_comment.""id"" AS ""id"",
		------->: $project_nn_task
		(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
		 SELECT 
			 project_nn_task.""id"" AS ""id""
		 FROM rec_project project_nn_task
		 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = task_nn_comment.id
		 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task""		
		-------< $project_nn_task

	 FROM rec_task task_nn_comment
	 LEFT JOIN  rel_task_nn_comment task_nn_comment_target ON task_nn_comment_target.target_id = rec_comment.id
	 WHERE task_nn_comment.id = task_nn_comment_target.origin_id )d  )::jsonb AS ""$task_nn_comment"",
	-------< $task_nn_comment
	------->: $case_nn_comment
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 case_nn_comment.""id"" AS ""id""
	 FROM rec_case case_nn_comment
	 LEFT JOIN  rel_case_nn_comment case_nn_comment_target ON case_nn_comment_target.target_id = rec_comment.id
	 WHERE case_nn_comment.id = case_nn_comment_target.origin_id )d  )::jsonb AS ""$case_nn_comment""	
	-------< $case_nn_comment

FROM rec_comment
WHERE  ( rec_comment.""id"" = @commentId ) 
) X
";
				var parametersJson = @"[{""name"":""commentId"",""type"":""guid"",""value"":""d5e1d939-fa3e-4332-a521-4c4e0f051e8a""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$task_nn_comment"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]},{""name"":""$case_nn_comment"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"comment";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: AllProjectTasks >>
			{
				var id = new Guid("c2284f3d-2ddc-4bad-9d1b-f6e44d502bdd");
				var name = @"AllProjectTasks";
				var description = @"All tasks in a project";
				var eqlText = @"SELECT *,$project_nn_task.abbr,$user_1n_task.username,$task_status_1n_task.label,$task_type_1n_task.label,$task_type_1n_task.icon_class,$task_type_1n_task.color,$user_1n_task_creator.username
FROM task
WHERE x_search CONTAINS @searchQuery AND $project_nn_task.id = @projectId
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""start_date"" AS ""start_date"",
	 rec_task.""target_date"" AS ""target_date"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""x_nonbillable_hours"" AS ""x_nonbillable_hours"",
	 rec_task.""x_billable_hours"" AS ""x_billable_hours"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""estimation"" AS ""estimation"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""x_search"" AS ""x_search"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""abbr"" AS ""abbr""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task"",
	-------< $project_nn_task
	------->: $user_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task.""id"" AS ""id"",
		 user_1n_task.""username"" AS ""username"" 
	 FROM rec_user user_1n_task
	 WHERE user_1n_task.id = rec_task.owner_id ) d )::jsonb AS ""$user_1n_task"",
	-------< $user_1n_task
	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	-------< $task_status_1n_task
	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"",
		 task_type_1n_task.""icon_class"" AS ""icon_class"",
		 task_type_1n_task.""color"" AS ""color"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task"",
	-------< $task_type_1n_task
	------->: $user_1n_task_creator
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task_creator.""id"" AS ""id"",
		 user_1n_task_creator.""username"" AS ""username"" 
	 FROM rec_user user_1n_task_creator
	 WHERE user_1n_task_creator.id = rec_task.created_by ) d )::jsonb AS ""$user_1n_task_creator""	
	-------< $user_1n_task_creator

FROM rec_task
LEFT OUTER JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
LEFT OUTER JOIN  rec_project project_nn_task_tar_org ON project_nn_task_target.origin_id = project_nn_task_tar_org.id
WHERE  (  ( rec_task.""x_search""  ILIKE  @searchQuery )  AND  ( project_nn_task_tar_org.""id"" = @projectId )  ) 
ORDER BY rec_task.""target_date"" ASC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""target_date""},{""name"":""sortOrder"",""type"":""text"",""value"":""asc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty""},{""name"":""projectId"",""type"":""guid"",""value"":""guid.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_date"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""target_date"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_hours"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_hours"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimation"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task_creator"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"task";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: CommentsForRecordId >>
			{
				var id = new Guid("a588e096-358d-4426-adf6-5db693f32322");
				var name = @"CommentsForRecordId";
				var description = @"Get all comments for a record";
				var eqlText = @"SELECT *,$user_1n_comment.image,$user_1n_comment.username
FROM comment
WHERE l_related_records CONTAINS @recordId 
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_comment.""id"" AS ""id"",
	 rec_comment.""body"" AS ""body"",
	 rec_comment.""created_by"" AS ""created_by"",
	 rec_comment.""created_on"" AS ""created_on"",
	 rec_comment.""l_scope"" AS ""l_scope"",
	 rec_comment.""parent_id"" AS ""parent_id"",
	 rec_comment.""l_related_records"" AS ""l_related_records"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_comment
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_comment.""id"" AS ""id"",
		 user_1n_comment.""image"" AS ""image"",
		 user_1n_comment.""username"" AS ""username"" 
	 FROM rec_user user_1n_comment
	 WHERE user_1n_comment.id = rec_comment.created_by ) d )::jsonb AS ""$user_1n_comment""	
	-------< $user_1n_comment

FROM rec_comment
WHERE  ( rec_comment.""l_related_records""  ILIKE  @recordId ) 
ORDER BY rec_comment.""created_on"" DESC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""created_on""},{""name"":""sortOrder"",""type"":""text"",""value"":""desc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""recordId"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_related_records"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$user_1n_comment"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""image"",""type"":9,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"comment";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: TimeLogsForRecordId >>
			{
				var id = new Guid("e66b8374-82ea-4305-8456-085b3a1f1f2d");
				var name = @"TimeLogsForRecordId";
				var description = @"Get all time logs for a record";
				var eqlText = @"SELECT *,$user_1n_timelog.image,$user_1n_timelog.username
FROM timelog
WHERE l_related_records CONTAINS @recordId 
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_timelog.""id"" AS ""id"",
	 rec_timelog.""body"" AS ""body"",
	 rec_timelog.""created_by"" AS ""created_by"",
	 rec_timelog.""created_on"" AS ""created_on"",
	 rec_timelog.""is_billable"" AS ""is_billable"",
	 rec_timelog.""l_related_records"" AS ""l_related_records"",
	 rec_timelog.""l_scope"" AS ""l_scope"",
	 rec_timelog.""minutes"" AS ""minutes"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_timelog
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_timelog.""id"" AS ""id"",
		 user_1n_timelog.""image"" AS ""image"",
		 user_1n_timelog.""username"" AS ""username"" 
	 FROM rec_user user_1n_timelog
	 WHERE user_1n_timelog.id = rec_timelog.created_by ) d )::jsonb AS ""$user_1n_timelog""	
	-------< $user_1n_timelog

FROM rec_timelog
WHERE  ( rec_timelog.""l_related_records""  ILIKE  @recordId ) 
ORDER BY rec_timelog.""created_on"" DESC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""created_on""},{""name"":""sortOrder"",""type"":""text"",""value"":""desc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""recordId"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":10,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_billable"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_related_records"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$user_1n_timelog"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""image"",""type"":9,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"timelog";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: FeedItemsForRecordId >>
			{
				var id = new Guid("74e5a414-6deb-4af6-8e29-567f718ca430");
				var name = @"FeedItemsForRecordId";
				var description = @"Get all feed items for a record";
				var eqlText = @"SELECT *,$user_1n_feed_item.image,$user_1n_feed_item.username
FROM feed_item
WHERE l_related_records CONTAINS @recordId AND type CONTAINS @type
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_feed_item.""id"" AS ""id"",
	 rec_feed_item.""created_by"" AS ""created_by"",
	 rec_feed_item.""created_on"" AS ""created_on"",
	 rec_feed_item.""l_scope"" AS ""l_scope"",
	 rec_feed_item.""subject"" AS ""subject"",
	 rec_feed_item.""body"" AS ""body"",
	 rec_feed_item.""type"" AS ""type"",
	 rec_feed_item.""l_related_records"" AS ""l_related_records"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_feed_item
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_feed_item.""id"" AS ""id"",
		 user_1n_feed_item.""image"" AS ""image"",
		 user_1n_feed_item.""username"" AS ""username"" 
	 FROM rec_user user_1n_feed_item
	 WHERE user_1n_feed_item.id = rec_feed_item.created_by ) d )::jsonb AS ""$user_1n_feed_item""	
	-------< $user_1n_feed_item

FROM rec_feed_item
WHERE  (  ( rec_feed_item.""l_related_records""  ILIKE  @recordId )  AND  ( rec_feed_item.""type""  ILIKE  @type )  ) 
ORDER BY rec_feed_item.""created_on"" DESC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""created_on""},{""name"":""sortOrder"",""type"":""text"",""value"":""desc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""recordId"",""type"":""text"",""value"":""string.empty""},{""name"":""type"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_related_records"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$user_1n_feed_item"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""image"",""type"":9,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
				var weight = 10;
				var entityName = @"feed_item";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create page data source*** Name: Accounts >>
			{
				var id = new Guid("a2db7724-f05b-4820-9269-64792398c309");
				var pageId = new Guid("2f11031a-41da-4dfc-8e40-ddc6dca71e2c");
				var dataSourceId = new Guid("61d21547-b353-48b8-8b75-b727680da79e");
				var name = @"Accounts";
				var parameters = @"[{""name"":""name"",""type"":""text"",""value"":""{{RequestQuery.q_name_v}}""},{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy ?? name}}""},{""name"":""page"",""type"":""int"",""value"":""{{RequestQuery.page ?? 1 }}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskTypes >>
			{
				var id = new Guid("d13ee96e-64e6-4174-b16d-c1c5a7bcb9f9");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("4857ace4-fcfc-4803-ad86-7c7afba91ce0");
				var name = @"TaskTypes";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllProjects >>
			{
				var id = new Guid("993d643a-1c10-4475-8b1f-3e5ac5f2e036");
				var pageId = new Guid("57db749f-e69e-4d88-b9d1-66203da05da1");
				var dataSourceId = new Guid("96218f33-42f1-4ff1-926c-b1765e1f8c6e");
				var name = @"AllProjects";
				var parameters = @"[{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy ?? name}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsers >>
			{
				var id = new Guid("a94b7669-edd2-484e-88fb-d480f79b4ec6");
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var dataSourceId = new Guid("f3e5ab66-9257-42f9-8bdf-f0233dd4aedd");
				var name = @"AllUsers";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllAccounts >>
			{
				var id = new Guid("6e38b5c3-43ba-4d5e-8454-11e7f6eef235");
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var dataSourceId = new Guid("61d21547-b353-48b8-8b75-b727680da79e");
				var name = @"AllAccounts";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsersSelectOptions >>
			{
				var id = new Guid("1487e7c6-60b2-4c2c-9ebe-0648435d2330");
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllUsersSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllUsers""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""username""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllAccountsSelectOptions >>
			{
				var id = new Guid("7d05f40e-71ae-49de-9dd0-2231b1c9265a");
				var pageId = new Guid("c2e38698-24cd-4209-b560-02c225f3ff4a");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllAccountsSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllAccounts""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""name""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllAccountsSelectOptions >>
			{
				var id = new Guid("8b29596b-3310-46e0-838b-682e243f4611");
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllAccountsSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllAccounts""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""name""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsers >>
			{
				var id = new Guid("fefbdab5-57ee-4343-9355-199c154bde3d");
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var dataSourceId = new Guid("f3e5ab66-9257-42f9-8bdf-f0233dd4aedd");
				var name = @"AllUsers";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsersSelectOptions >>
			{
				var id = new Guid("f92520fe-8ea9-4284-a991-bb74810660e5");
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllUsersSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllUsers""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""username""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsers >>
			{
				var id = new Guid("defaf774-60d6-4c15-9683-da15ca53730c");
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var dataSourceId = new Guid("f3e5ab66-9257-42f9-8bdf-f0233dd4aedd");
				var name = @"AllUsers";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsersSelectOption >>
			{
				var id = new Guid("ebf5c697-3a01-4759-b9c6-ec7f3414bb54");
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllUsersSelectOption";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllUsers""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""username""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllProjects >>
			{
				var id = new Guid("c4bb6351-2fa9-4953-852f-62eb782e839c");
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var dataSourceId = new Guid("96218f33-42f1-4ff1-926c-b1765e1f8c6e");
				var name = @"AllProjects";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllProjectsSelectOption >>
			{
				var id = new Guid("561c85b5-b016-4420-8770-9752ff5347b9");
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllProjectsSelectOption";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllProjects""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""name""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllProjectTasks >>
			{
				var id = new Guid("6c41cbd7-d99f-4019-84f0-24361bfd7a0a");
				var pageId = new Guid("6f673561-fad7-4844-8262-589834f1b2ce");
				var dataSourceId = new Guid("c2284f3d-2ddc-4bad-9d1b-f6e44d502bdd");
				var name = @"AllProjectTasks";
				var parameters = @"[{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy}}""},{""name"":""sortOrder"",""type"":""text"",""value"":""{{RequestQuery.sortOrder}}""},{""name"":""page"",""type"":""int"",""value"":""{{RequestQuery.page}}""},{""name"":""searchQuery"",""type"":""text"",""value"":""{{RequestQuery.q_x_search_v}}""},{""name"":""projectId"",""type"":""guid"",""value"":""{{ParentRecord.id}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskAuxData >>
			{
				var id = new Guid("f8c429ee-c6fe-457d-9339-44e626a6dd27");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("587d963b-613f-4e77-a7d4-719f631ce6b2");
				var name = @"TaskAuxData";
				var parameters = @"[{""name"":""recordId"",""type"":""guid"",""value"":""{{Record.id}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsers >>
			{
				var id = new Guid("5ff5cc0c-c06e-4b58-8a31-4714914778aa");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("f3e5ab66-9257-42f9-8bdf-f0233dd4aedd");
				var name = @"AllUsers";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllUsersSelectOption >>
			{
				var id = new Guid("43691d9f-65ef-433c-934b-ccf6eaafdd3f");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"AllUsersSelectOption";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""AllUsers""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""username""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskTypesSelectOption >>
			{
				var id = new Guid("750213cb-8c69-4749-b10f-211b53369958");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"TaskTypesSelectOption";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""TaskTypes""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""label""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskStatusesSelectOption >>
			{
				var id = new Guid("f5a2f77f-6d79-4180-b73f-7deb21895f4e");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"TaskStatusesSelectOption";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""TaskStatuses""},{""name"":""KeyPropName"",""type"":""text"",""value"":""id""},{""name"":""ValuePropName"",""type"":""text"",""value"":""label""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskStatuses >>
			{
				var id = new Guid("f09fe186-8617-4f94-a67b-3a69172b1257");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("fad53f3d-4d3b-4c7b-8cd2-23e96a086ad8");
				var name = @"TaskStatuses";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllTasks >>
			{
				var id = new Guid("9c8ec6cc-b389-4baa-b4ce-770edf2520dd");
				var pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
				var dataSourceId = new Guid("5a6e9d56-63bc-43b1-b95e-24838db9f435");
				var name = @"AllTasks";
				var parameters = @"[{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy}}""},{""name"":""sortOrder"",""type"":""text"",""value"":""{{RequestQuery.sortOrder}}""},{""name"":""page"",""type"":""int"",""value"":""{{RequestQuery.page}}""},{""name"":""searchQuery"",""type"":""text"",""value"":""{{RequestQuery.q_x_search_v}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskTypes >>
			{
				var id = new Guid("9e50f76d-f56c-4204-9d8b-4db8860371a5");
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var dataSourceId = new Guid("4857ace4-fcfc-4803-ad86-7c7afba91ce0");
				var name = @"TaskTypes";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TaskTypeSelectOptions >>
			{
				var id = new Guid("120c783a-f04c-4be9-a9ef-f991aae3d648");
				var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
				var dataSourceId = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
				var name = @"TaskTypeSelectOptions";
				var parameters = @"[{""name"":""DataSourceName"",""type"":""text"",""value"":""TaskTypes""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllAccounts >>
			{
				var id = new Guid("cf3b936e-ec45-4937-a157-a008ef97d594");
				var pageId = new Guid("7a0aad34-0f2f-4c40-a77f-cee92c9550a3");
				var dataSourceId = new Guid("61d21547-b353-48b8-8b75-b727680da79e");
				var name = @"AllAccounts";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: FeedItemsForRecordId >>
			{
				var id = new Guid("ee65976e-d5d0-4dd4-ac6a-2047e8817add");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("74e5a414-6deb-4af6-8e29-567f718ca430");
				var name = @"FeedItemsForRecordId";
				var parameters = @"[{""name"":""recordId"",""type"":""text"",""value"":""{{Record.id}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: CommentsForRecordId >>
			{
				var id = new Guid("2f523831-0437-4250-a6b5-8eeb3da9d04c");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("a588e096-358d-4426-adf6-5db693f32322");
				var name = @"CommentsForRecordId";
				var parameters = @"[{""name"":""recordId"",""type"":""text"",""value"":""{{Record.id}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TimeLogsForRecordId >>
			{
				var id = new Guid("24e093ae-ab0f-4c52-86b2-9e1fe2ed2a0a");
				var pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
				var dataSourceId = new Guid("e66b8374-82ea-4305-8456-085b3a1f1f2d");
				var name = @"TimeLogsForRecordId";
				var parameters = @"[{""name"":""recordId"",""type"":""text"",""value"":""{{Record.id}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: CurrentDate >>
			{
				var id = new Guid("361dc0a8-68b8-45ec-8002-11779a304899");
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var dataSourceId = new Guid("64207638-d75e-4a25-9965-6e35b0aa835a");
				var name = @"CurrentDate";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: ProjectWidgetMyTasks >>
			{
				var id = new Guid("e688cbdd-0fa9-43b4-aed1-3d667fdecf87");
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var dataSourceId = new Guid("c44eab77-c81e-4f55-95c8-4949b275fc99");
				var name = @"ProjectWidgetMyTasks";
				var parameters = @"[{""name"":""userId"",""type"":""guid"",""value"":""{{CurrentUser.Id}}""},{""name"":""currentDate"",""type"":""date"",""value"":""{{CurrentDate}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: ProjectWidgetMyOverdueTasks >>
			{
				var id = new Guid("5af27b39-f700-400a-bbb7-69ddb50e39cd");
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var dataSourceId = new Guid("946919a6-e4cd-41a2-97dc-1069d73adcd1");
				var name = @"ProjectWidgetMyOverdueTasks";
				var parameters = @"[{""name"":""userId"",""type"":""guid"",""value"":""{{CurrentUser.Id}}""},{""name"":""currentDate"",""type"":""date"",""value"":""{{CurrentDate}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: FeedItemsForRecordId >>
			{
				var id = new Guid("7717b418-7eed-472a-b4cd-6ada2e85d6df");
				var pageId = new Guid("dfe56667-174d-492d-8f84-b8ab8b70c63f");
				var dataSourceId = new Guid("74e5a414-6deb-4af6-8e29-567f718ca430");
				var name = @"FeedItemsForRecordId";
				var parameters = @"[{""name"":""recordId"",""type"":""text"",""value"":""{{Record.id}}""},{""name"":""type"",""type"":""text"",""value"":""{{RequestQuery.type}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: FeedItemsForRecordId >>
			{
				var id = new Guid("0b3fefbc-0c11-4d22-8343-8d638165a026");
				var pageId = new Guid("acb76466-32b8-428c-81cb-47b6013879e7");
				var dataSourceId = new Guid("74e5a414-6deb-4af6-8e29-567f718ca430");
				var name = @"FeedItemsForRecordId";
				var parameters = @"[{""name"":""type"",""type"":""text"",""value"":""{{RequestQuery.type}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: ProjectWidgetMyTasksDueToday >>
			{
				var id = new Guid("f1da592e-d696-426a-a60c-ef262d101a56");
				var pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
				var dataSourceId = new Guid("eae07b63-9bf4-4e25-80af-df5228dedf35");
				var name = @"ProjectWidgetMyTasksDueToday";
				var parameters = @"[{""name"":""userId"",""type"":""guid"",""value"":""{{CurrentUser.Id}}""},{""name"":""currentDate"",""type"":""date"",""value"":""{{CurrentDate}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: CurrentDate >>
			{
				var id = new Guid("cb29b5cf-18b4-404c-bd8e-511766624ad7");
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var dataSourceId = new Guid("64207638-d75e-4a25-9965-6e35b0aa835a");
				var name = @"CurrentDate";
				var parameters = @"[]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: TrackTimeTasks >>
			{
				var id = new Guid("9ba5e65b-b10c-4217-8aa8-e2d3db5f22f8");
				var pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
				var dataSourceId = new Guid("473ee9b6-2131-4164-b5fe-d9b3073e9178");
				var name = @"TrackTimeTasks";
				var parameters = @"[{""name"":""search_query"",""type"":""text"",""value"":""{{RequestQuery.q_x_fts_v}}""}]";

				new WebVella.Erp.Web.Repositories.PageDataSourceRepository(ErpSettings.ConnectionString).Insert(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion


		}
	}
}
