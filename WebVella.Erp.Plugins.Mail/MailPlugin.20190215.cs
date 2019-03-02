using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.Mail
{
	public partial class MailPlugin : ErpPlugin
	{
		private static void Patch20190215(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Create entity*** Entity name: email >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("9e8b7cd7-f340-411d-933a-62be8cb591e4");
					entity.Id = new Guid("085e2442-820a-4df7-ab92-516ce23197c4");
					entity.Name = "email";
					entity.Label = "Email";
					entity.LabelPlural = "Emails";
					entity.System = true;
					entity.IconName = "far fa-envelope";
					entity.Color = "#8bc34a";
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
							throw new Exception("System error 10050. Entity: email creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: subject >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("15af2fd2-a1cb-424d-b777-41139c65dbcc");
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
				textboxField.MaxLength = Int32.Parse("1000");
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: subject Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: content_text >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("eb3a49f7-8216-4300-847f-15daca6cd087");
				textboxField.Name = "content_text";
				textboxField.Label = "Content text";
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
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: content_text Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: content_html >>
			{
				InputHtmlField htmlField = new InputHtmlField();
				htmlField.Id = new Guid("e1fd62b4-5630-4974-8ddf-0324f3d965e9");
				htmlField.Name = "content_html";
				htmlField.Label = "Content Html";
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
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), htmlField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: content_html Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: sent_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("2adbf0ae-1701-4a07-8e2f-82be6740bb7a");
				datetimeField.Name = "sent_on";
				datetimeField.Label = "Sent On";
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
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: sent_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: created_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("cf69678d-6447-4e2f-9e83-ccc9b5fa610f");
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
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: created_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: server_error >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("fee93ffb-7991-4f3c-9f78-1b18b9589422");
				textboxField.Name = "server_error";
				textboxField.Label = "Server error";
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
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: server_error Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: retries_count >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("9192c99e-ec5c-40d3-a591-fd46a92d15fa");
				numberField.Name = "retries_count";
				numberField.Label = "Retries count";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("0.0");
				numberField.MinValue = Decimal.Parse("0.0");
				numberField.MaxValue = null;
				numberField.DecimalPlaces = byte.Parse("0");
				numberField.EnableSecurity = false;
				numberField.Permissions = new FieldPermissions();
				numberField.Permissions.CanRead = new List<Guid>();
				numberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: retries_count Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: service_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("81119e86-bd2d-456b-8215-daafcac2870c");
				guidField.Name = "service_id";
				guidField.Label = "Service Identifier";
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
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: service_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: priority >>
			{
				InputSelectField dropdownField = new InputSelectField();
				dropdownField.Id = new Guid("ffa6a87b-a638-4acd-8306-79ef2bf091c4");
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
		new SelectOption() { Label = "low", Value = "0", IconClass = "", Color = ""},
		new SelectOption() { Label = "normal", Value = "1", IconClass = "", Color = ""},
		new SelectOption() { Label = "high", Value = "2", IconClass = "", Color = ""}
	};
				dropdownField.EnableSecurity = false;
				dropdownField.Permissions = new FieldPermissions();
				dropdownField.Permissions.CanRead = new List<Guid>();
				dropdownField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), dropdownField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: priority Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: reply_to_email >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("eb9cf95b-0876-4bb2-bfa8-c3970551e582");
				textboxField.Name = "reply_to_email";
				textboxField.Label = "ReplyTo Email";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = false;
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
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: reply_to_email Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: scheduled_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("5c08f305-8209-4c96-a7ac-03043095cc73");
				datetimeField.Name = "scheduled_on";
				datetimeField.Label = "ScheduledOn";
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
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: scheduled_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: status >>
			{
				InputSelectField dropdownField = new InputSelectField();
				dropdownField.Id = new Guid("b1cd96e9-c786-4261-ab2b-1a51cab243e0");
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
				dropdownField.DefaultValue = "0";
				dropdownField.Options = new List<SelectOption>
	{
		new SelectOption() { Label = "pending", Value = "0", IconClass = "", Color = ""},
		new SelectOption() { Label = "sent", Value = "1", IconClass = "", Color = ""},
		new SelectOption() { Label = "aborted", Value = "2", IconClass = "", Color = ""}
	};
				dropdownField.EnableSecurity = false;
				dropdownField.Permissions = new FieldPermissions();
				dropdownField.Permissions.CanRead = new List<Guid>();
				dropdownField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), dropdownField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: status Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: x_search >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("9ab2ab99-7293-4772-8874-d7ca7383b317");
				textboxField.Name = "x_search";
				textboxField.Label = "x_search";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = true;
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
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: x_search Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: recipient_name >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("a3015639-7fd9-4231-89e3-76a7a133dd6d");
				textboxField.Name = "recipient_name";
				textboxField.Label = "Recipient Name";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = false;
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
						throw new Exception("System error 10060. Entity: email Field: recipient_name Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: sender_name >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("4d9e646c-0105-4370-ad21-d6547a7cabb1");
				textboxField.Name = "sender_name";
				textboxField.Label = "Sender Name";
				textboxField.PlaceholderText = null;
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
						throw new Exception("System error 10060. Entity: email Field: sender_name Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: recipient_email >>
			{
				InputEmailField emailField = new InputEmailField();
				emailField.Id = new Guid("cae76d3b-bf91-47bc-aec0-d7ac26eced7b");
				emailField.Name = "recipient_email";
				emailField.Label = "Recipient Email";
				emailField.PlaceholderText = null;
				emailField.Description = null;
				emailField.HelpText = null;
				emailField.Required = true;
				emailField.Unique = false;
				emailField.Searchable = true;
				emailField.Auditable = false;
				emailField.System = true;
				emailField.DefaultValue = "";
				emailField.MaxLength = null;
				emailField.EnableSecurity = false;
				emailField.Permissions = new FieldPermissions();
				emailField.Permissions.CanRead = new List<Guid>();
				emailField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), emailField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: recipient_email Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: email Field Name: sender_email >>
			{
				InputEmailField emailField = new InputEmailField();
				emailField.Id = new Guid("94845377-b845-49fe-b693-789f1ed5740e");
				emailField.Name = "sender_email";
				emailField.Label = "Sender Email";
				emailField.PlaceholderText = null;
				emailField.Description = null;
				emailField.HelpText = null;
				emailField.Required = true;
				emailField.Unique = false;
				emailField.Searchable = true;
				emailField.Auditable = false;
				emailField.System = true;
				emailField.DefaultValue = "";
				emailField.MaxLength = null;
				emailField.EnableSecurity = false;
				emailField.Permissions = new FieldPermissions();
				emailField.Permissions.CanRead = new List<Guid>();
				emailField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("085e2442-820a-4df7-ab92-516ce23197c4"), emailField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: email Field: sender_email Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: smtp_service >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("f2f2e3ec-c7d5-4169-b175-b741c24b66b4");
					entity.Id = new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e");
					entity.Name = "smtp_service";
					entity.Label = "Smtp Service";
					entity.LabelPlural = "Smtp Services";
					entity.System = true;
					entity.IconName = "fas fa-cogs";
					entity.Color = "#8bc34a";
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
							throw new Exception("System error 10050. Entity: smtp_service creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: default_reply_to_email >>
			{
				InputEmailField emailField = new InputEmailField();
				emailField.Id = new Guid("5da218c9-cfb3-41ce-9fee-350adc0b2d7d");
				emailField.Name = "default_reply_to_email";
				emailField.Label = "Default ReplyTo Email";
				emailField.PlaceholderText = null;
				emailField.Description = null;
				emailField.HelpText = null;
				emailField.Required = false;
				emailField.Unique = false;
				emailField.Searchable = false;
				emailField.Auditable = false;
				emailField.System = true;
				emailField.DefaultValue = null;
				emailField.MaxLength = null;
				emailField.EnableSecurity = false;
				emailField.Permissions = new FieldPermissions();
				emailField.Permissions.CanRead = new List<Guid>();
				emailField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), emailField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: default_reply_to_email Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: max_retries_count >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("5d4a40fd-d4c5-4d20-b45c-0622f8acbe93");
				numberField.Name = "max_retries_count";
				numberField.Label = "Max retries count";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("3.0");
				numberField.MinValue = Decimal.Parse("0.0");
				numberField.MaxValue = Decimal.Parse("10.0");
				numberField.DecimalPlaces = byte.Parse("0");
				numberField.EnableSecurity = false;
				numberField.Permissions = new FieldPermissions();
				numberField.Permissions.CanRead = new List<Guid>();
				numberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: max_retries_count Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: retry_wait_minutes >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("6f6e2836-955c-49a2-8880-7960c1c8206d");
				numberField.Name = "retry_wait_minutes";
				numberField.Label = "Wait minutes to retry";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("60.0");
				numberField.MinValue = Decimal.Parse("0.0");
				numberField.MaxValue = Decimal.Parse("1440.0");
				numberField.DecimalPlaces = byte.Parse("0");
				numberField.EnableSecurity = false;
				numberField.Permissions = new FieldPermissions();
				numberField.Permissions.CanRead = new List<Guid>();
				numberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: retry_wait_minutes Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: is_default >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("27a24518-c8ea-4ad6-93ce-2baed017d782");
				checkboxField.Name = "is_default";
				checkboxField.Label = "Is default Smtp service";
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
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: is_default Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: name >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("d3406be9-6a81-46a3-a0be-f39d7bd55392");
				textboxField.Name = "name";
				textboxField.Label = "Name";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = true;
				textboxField.Searchable = true;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "smtp service";
				textboxField.MaxLength = Int32.Parse("100");
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: name Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: server >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("b827c863-1d75-4019-a3b5-3b0628b91cb2");
				textboxField.Name = "server";
				textboxField.Label = "Server";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = "domain name or ip address of smtp server";
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "smtp.domain.com";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: server Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: username >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("e921a804-71ac-4af3-81a9-903b505dc53d");
				textboxField.Name = "username";
				textboxField.Label = "Username";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = "only if smtp server requires authentication";
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
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: username Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: password >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("420b9f71-ed26-4fd1-9c25-933d39b7d610");
				textboxField.Name = "password";
				textboxField.Label = "Password";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = "only if smtp server requires authentication";
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
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: password Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: port >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("8d52e394-8e1c-4e97-b192-06e238d6c550");
				numberField.Name = "port";
				numberField.Label = "Port";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("25.0");
				numberField.MinValue = Decimal.Parse("1.0");
				numberField.MaxValue = Decimal.Parse("65535.0");
				numberField.DecimalPlaces = byte.Parse("0");
				numberField.EnableSecurity = false;
				numberField.Permissions = new FieldPermissions();
				numberField.Permissions.CanRead = new List<Guid>();
				numberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: port Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: connection_security >>
			{
				InputSelectField dropdownField = new InputSelectField();
				dropdownField.Id = new Guid("fdd4123b-7578-4a57-9a57-ef51034fd145");
				dropdownField.Name = "connection_security";
				dropdownField.Label = "Connection security";
				dropdownField.PlaceholderText = null;
				dropdownField.Description = null;
				dropdownField.HelpText = null;
				dropdownField.Required = true;
				dropdownField.Unique = false;
				dropdownField.Searchable = false;
				dropdownField.Auditable = false;
				dropdownField.System = true;
				dropdownField.DefaultValue = "1";
				dropdownField.Options = new List<SelectOption>
	{
		new SelectOption() { Label = "None", Value = "0", IconClass = "", Color = ""},
		new SelectOption() { Label = "Auto", Value = "1", IconClass = "", Color = ""},
		new SelectOption() { Label = "SslOnConnect", Value = "2", IconClass = "", Color = ""},
		new SelectOption() { Label = "StartTls", Value = "3", IconClass = "", Color = ""},
		new SelectOption() { Label = "StartTlsWhenAvailable", Value = "4", IconClass = "", Color = ""}
	};
				dropdownField.EnableSecurity = false;
				dropdownField.Permissions = new FieldPermissions();
				dropdownField.Permissions.CanRead = new List<Guid>();
				dropdownField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), dropdownField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: connection_security Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: default_sender_name >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("cd7c8228-e40b-4b1b-86d0-4764663f03ec");
				textboxField.Name = "default_sender_name";
				textboxField.Label = "Default sender name";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = false;
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
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: default_sender_name Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: default_sender_email >>
			{
				InputEmailField emailField = new InputEmailField();
				emailField.Id = new Guid("362cd0c1-ea7c-4aee-8909-df3e91ec15cb");
				emailField.Name = "default_sender_email";
				emailField.Label = "Default sender email";
				emailField.PlaceholderText = null;
				emailField.Description = null;
				emailField.HelpText = null;
				emailField.Required = true;
				emailField.Unique = false;
				emailField.Searchable = true;
				emailField.Auditable = false;
				emailField.System = true;
				emailField.DefaultValue = "";
				emailField.MaxLength = null;
				emailField.EnableSecurity = false;
				emailField.Permissions = new FieldPermissions();
				emailField.Permissions.CanRead = new List<Guid>();
				emailField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), emailField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: default_sender_email Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: smtp_service Field Name: is_enabled >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("6c3ba722-e78e-4365-8376-86584025c065");
				checkboxField.Name = "is_enabled";
				checkboxField.Label = "Is enabled";
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
					var response = entMan.CreateField(new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: smtp_service Field: is_enabled Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create app*** App name: mail >>
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

				new WebVella.Erp.Web.Services.AppService().CreateApplication(id, name, label, description, iconClass, author, color, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: emails >>
			{
				var id = new Guid("c5835090-9089-496d-ac0f-6f67bb593384");
				var appId = new Guid("9d3b5497-e136-43b7-ad87-857e615a54c9");
				var name = "emails";
				var label = "Emails";
				var description = @"";
				var iconClass = "";
				var color = "";
				var weight = 1;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap area*** Sitemap area name: services >>
			{
				var id = new Guid("fe4a1467-099e-4a3e-a9ed-f38a0d81a2f3");
				var appId = new Guid("9d3b5497-e136-43b7-ad87-857e615a54c9");
				var name = "services";
				var label = "Services";
				var description = @"";
				var iconClass = "";
				var color = "";
				var weight = 2;
				var showGroupNames = false;
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
				var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateArea(id, appId, name, label, labelTranslations, description, descriptionTranslations, iconClass, color, weight, showGroupNames, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: all >>
			{
				var id = new Guid("91696f9d-b439-4823-95b7-afc05debf5e6");
				var areaId = new Guid("c5835090-9089-496d-ac0f-6f67bb593384");
				Guid? entityId = new Guid("085e2442-820a-4df7-ab92-516ce23197c4");
				var name = "all";
				var label = "All email list";
				var url = "";
				var iconClass = "";
				var weight = 1;
				var type = ((int)1);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create sitemap node*** Sitemap node name: smtp >>
			{
				var id = new Guid("fbf00f09-67f1-4d36-97d0-6af509c360aa");
				var areaId = new Guid("fe4a1467-099e-4a3e-a9ed-f38a0d81a2f3");
				Guid? entityId = new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e");
				var name = "smtp";
				var label = "Smtp";
				var url = "http://google.com";
				var iconClass = "";
				var weight = 1;
				var type = ((int)1);
				var access = new List<Guid>();
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.AppService().CreateAreaNode(id, areaId, name, label, labelTranslations, iconClass, url, type, entityId, weight, access, new List<Guid>(), new List<Guid>(), new List<Guid>(), new List<Guid>(), WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: create >>
			{
				var id = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var name = @"create";
				var label = "Create Smtp Service";
				var iconClass = "";
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)4);
				var isRazorBody = false;
				Guid? appId = new Guid("9d3b5497-e136-43b7-ad87-857e615a54c9");
				Guid? entityId = new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: details >>
			{
				var id = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var name = @"details";
				var label = "Smtp Service Details";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("9d3b5497-e136-43b7-ad87-857e615a54c9");
				Guid? entityId = new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: all >>
			{
				var id = new Guid("31c40750-99c7-4402-9e9b-8157e9459df7");
				var name = @"all";
				var label = "Smtp Services ";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("9d3b5497-e136-43b7-ad87-857e615a54c9");
				Guid? entityId = new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: all_emails >>
			{
				var id = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var name = @"all_emails";
				var label = "All emails list";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)3);
				var isRazorBody = false;
				Guid? appId = new Guid("9d3b5497-e136-43b7-ad87-857e615a54c9");
				Guid? entityId = new Guid("085e2442-820a-4df7-ab92-516ce23197c4");
				Guid? nodeId = new Guid("91696f9d-b439-4823-95b7-afc05debf5e6");
				Guid? areaId = new Guid("c5835090-9089-496d-ac0f-6f67bb593384");
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: details >>
			{
				var id = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var name = @"details";
				var label = "Email Details";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("9d3b5497-e136-43b7-ad87-857e615a54c9");
				Guid? entityId = new Guid("085e2442-820a-4df7-ab92-516ce23197c4");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page*** Page name: service_test >>
			{
				var id = new Guid("27bfa096-8cac-4eea-b4c4-7f50e875e797");
				var name = @"service_test";
				var label = "Smtp Service Test";
				string iconClass = null;
				var system = false;
				var layout = @"";
				var weight = 10;
				var type = (PageType)((int)5);
				var isRazorBody = false;
				Guid? appId = new Guid("9d3b5497-e136-43b7-ad87-857e615a54c9");
				Guid? entityId = new Guid("17698b9f-e533-4f8d-a651-a00f7de2989e");
				Guid? nodeId = null;
				Guid? areaId = null;
				string razorBody = null;
				var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

				new WebVella.Erp.Web.Services.PageService().CreatePage(id, name, label, labelTranslations, iconClass, system, weight, type, appId, entityId, nodeId, areaId, isRazorBody, razorBody, layout, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 9ed30b88-f9e8-4c31-8157-60b891e305a5 >>
			{
				var id = new Guid("9ed30b88-f9e8-4c31-8157-60b891e305a5");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Error"",
  ""mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.server_error\"",\""default\"":\""\""}"",
  ""name"": ""server_error"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 6;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: d5e674b6-f7f5-42de-80e5-b492bea54432 >>
			{
				var id = new Guid("d5e674b6-f7f5-42de-80e5-b492bea54432");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("31c40750-99c7-4402-9e9b-8157e9459df7");
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
  ""icon_class"": ""fas fa-tachometer-alt"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: f42d0a52-bb4b-4b2f-b634-fe6639d5860d >>
			{
				var id = new Guid("f42d0a52-bb4b-4b2f-b634-fe6639d5860d");
				Guid? parentId = new Guid("d5e674b6-f7f5-42de-80e5-b492bea54432");
				Guid? nodeId = null;
				var pageId = new Guid("31c40750-99c7-4402-9e9b-8157e9459df7");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""create new"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-plus"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""/mail/services/smtp/c/create"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: cbd7c6ff-6f54-4757-8184-e3e6eb16351c >>
			{
				var id = new Guid("cbd7c6ff-6f54-4757-8184-e3e6eb16351c");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 27f793be-96d7-46fc-ae67-f454254a17d9 >>
			{
				var id = new Guid("27f793be-96d7-46fc-ae67-f454254a17d9");
				Guid? parentId = new Guid("cbd7c6ff-6f54-4757-8184-e3e6eb16351c");
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDateTime";
				var containerId = "column1";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Created On"",
  ""mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.created_on\"",\""default\"":\""\""}"",
  ""name"": ""created_on"",
  ""class"": """",
  ""show_icon"": ""false"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 6aff7a05-0f41-44c0-a89b-1158c4fdd36e >>
			{
				var id = new Guid("6aff7a05-0f41-44c0-a89b-1158c4fdd36e");
				Guid? parentId = new Guid("cbd7c6ff-6f54-4757-8184-e3e6eb16351c");
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDateTime";
				var containerId = "column2";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Sent On"",
  ""mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.sent_on\"",\""default\"":\""\""}"",
  ""name"": ""sent_on"",
  ""class"": """",
  ""show_icon"": ""false"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: c714545c-7eb4-4d7f-9da9-afd8d17c5724 >>
			{
				var id = new Guid("c714545c-7eb4-4d7f-9da9-afd8d17c5724");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcPageHeader";
				var containerId = "";
				var options = @"{
  ""area_label"": ""{\""type\"":\""0\"",\""string\"":\""App.Label\"",\""default\"":\""\""}"",
  ""area_sublabel"": """",
  ""title"": ""Create Smtp Service"",
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

			#region << ***Create page body node*** Page name: create  id: c3c926d3-26e0-41ea-9cd4-525f2053a33e >>
			{
				var id = new Guid("c3c926d3-26e0-41ea-9cd4-525f2053a33e");
				Guid? parentId = new Guid("c714545c-7eb4-4d7f-9da9-afd8d17c5724");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
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
  ""href"": ""{\""type\"":\""0\"",\""string\"":\""ReturnUrl\"",\""default\"":\""/mail/services/smtp/l/all\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 1b1e0031-d5e3-4c2b-b9a4-daaebf34986c >>
			{
				var id = new Guid("1b1e0031-d5e3-4c2b-b9a4-daaebf34986c");
				Guid? parentId = new Guid("c714545c-7eb4-4d7f-9da9-afd8d17c5724");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Create service"",
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

			#region << ***Create page body node*** Page name: create  id: 2db9c1a4-84c3-4c5a-a7f8-525540bbc36f >>
			{
				var id = new Guid("2db9c1a4-84c3-4c5a-a7f8-525540bbc36f");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
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

			#region << ***Create page body node*** Page name: create  id: 88b44b85-3d13-4fc9-8cce-fc676ea20c22 >>
			{
				var id = new Guid("88b44b85-3d13-4fc9-8cce-fc676ea20c22");
				Guid? parentId = new Guid("2db9c1a4-84c3-4c5a-a7f8-525540bbc36f");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 2,
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
  ""container1_flex_self_align"": ""1"",
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
  ""container2_flex_self_align"": ""1"",
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
  ""container3_flex_self_align"": ""1"",
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
  ""container4_flex_self_align"": ""1"",
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
  ""container5_flex_self_align"": ""1"",
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
  ""container6_flex_self_align"": ""1"",
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
  ""container7_flex_self_align"": ""1"",
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
  ""container8_flex_self_align"": ""1"",
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
  ""container9_flex_self_align"": ""1"",
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
  ""container10_flex_self_align"": ""1"",
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
  ""container11_flex_self_align"": ""1"",
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
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 67a80898-b484-4b67-8c4b-05160a667766 >>
			{
				var id = new Guid("67a80898-b484-4b67-8c4b-05160a667766");
				Guid? parentId = new Guid("88b44b85-3d13-4fc9-8cce-fc676ea20c22");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column1";
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
  ""container1_flex_self_align"": ""1"",
  ""container1_flex_order"": 0,
  ""container2_span"": 3,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_self_align"": ""1"",
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
  ""container3_flex_self_align"": ""1"",
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
  ""container4_flex_self_align"": ""1"",
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
  ""container5_flex_self_align"": ""1"",
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
  ""container6_flex_self_align"": ""1"",
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
  ""container7_flex_self_align"": ""1"",
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
  ""container8_flex_self_align"": ""1"",
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
  ""container9_flex_self_align"": ""1"",
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
  ""container10_flex_self_align"": ""1"",
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
  ""container11_flex_self_align"": ""1"",
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
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: e6ca9f58-cf01-4aba-bd67-95509850264e >>
			{
				var id = new Guid("e6ca9f58-cf01-4aba-bd67-95509850264e");
				Guid? parentId = new Guid("67a80898-b484-4b67-8c4b-05160a667766");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Server"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.server\"",\""default\"":\""mail.domain.com\""}"",
  ""name"": ""server"",
  ""mode"": ""0"",
  ""maxlength"": 0,
  ""connected_entity_id"": ""17698b9f-e533-4f8d-a651-a00f7de2989e""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: d22f219e-34d8-4ca3-8224-0c9b7409d5b5 >>
			{
				var id = new Guid("d22f219e-34d8-4ca3-8224-0c9b7409d5b5");
				Guid? parentId = new Guid("88b44b85-3d13-4fc9-8cce-fc676ea20c22");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column2";
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
  ""container1_flex_self_align"": ""1"",
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
  ""container2_flex_self_align"": ""1"",
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
  ""container3_flex_self_align"": ""1"",
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
  ""container4_flex_self_align"": ""1"",
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
  ""container5_flex_self_align"": ""1"",
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
  ""container6_flex_self_align"": ""1"",
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
  ""container7_flex_self_align"": ""1"",
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
  ""container8_flex_self_align"": ""1"",
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
  ""container9_flex_self_align"": ""1"",
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
  ""container10_flex_self_align"": ""1"",
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
  ""container11_flex_self_align"": ""1"",
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
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: d02af112-5583-4269-b891-fed8cd3a1c9e >>
			{
				var id = new Guid("d02af112-5583-4269-b891-fed8cd3a1c9e");
				Guid? parentId = new Guid("d22f219e-34d8-4ca3-8224-0c9b7409d5b5");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldCheckbox";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Is default service"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.is_default\"",\""default\"":\""\""}"",
  ""name"": ""is_default"",
  ""mode"": ""0"",
  ""text_true"": """",
  ""text_false"": """",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 4a542ee8-a4ea-4e74-a9ae-9085538cc84f >>
			{
				var id = new Guid("4a542ee8-a4ea-4e74-a9ae-9085538cc84f");
				Guid? parentId = new Guid("d22f219e-34d8-4ca3-8224-0c9b7409d5b5");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Port"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.port\"",\""default\"":\""25\""}"",
  ""name"": ""port"",
  ""mode"": ""0"",
  ""decimal_digits"": 0,
  ""min"": 1,
  ""max"": 65535,
  ""step"": 1,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 1c26bae7-e491-4a3b-9dad-24729f0047da >>
			{
				var id = new Guid("1c26bae7-e491-4a3b-9dad-24729f0047da");
				Guid? parentId = new Guid("d22f219e-34d8-4ca3-8224-0c9b7409d5b5");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldCheckbox";
				var containerId = "column3";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Is enabled"",
  ""mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.is_enabled\"",\""default\"":\""\""}"",
  ""name"": ""is_enabled"",
  ""class"": """",
  ""text_true"": """",
  ""text_false"": """",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 5e5ea999-7008-4236-aa2f-cc3b0dfe174c >>
			{
				var id = new Guid("5e5ea999-7008-4236-aa2f-cc3b0dfe174c");
				Guid? parentId = new Guid("2db9c1a4-84c3-4c5a-a7f8-525540bbc36f");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 17138e02-8856-4c4f-8b9c-fff9a862276a >>
			{
				var id = new Guid("17138e02-8856-4c4f-8b9c-fff9a862276a");
				Guid? parentId = new Guid("5e5ea999-7008-4236-aa2f-cc3b0dfe174c");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column2";
				var options = @"{}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 99b51c49-1dc5-476a-99c0-1a33b2d31571 >>
			{
				var id = new Guid("99b51c49-1dc5-476a-99c0-1a33b2d31571");
				Guid? parentId = new Guid("17138e02-8856-4c4f-8b9c-fff9a862276a");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Wait period between retries (minutes)"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.retry_wait_minutes\"",\""default\"":\""60\""}"",
  ""name"": ""retry_wait_minutes"",
  ""mode"": ""0"",
  ""decimal_digits"": 0,
  ""min"": 1,
  ""max"": 1440,
  ""step"": 1,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 596b55f4-fc45-4a22-a82a-1f8b4cf136ff >>
			{
				var id = new Guid("596b55f4-fc45-4a22-a82a-1f8b4cf136ff");
				Guid? parentId = new Guid("17138e02-8856-4c4f-8b9c-fff9a862276a");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Number of retries on error"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.max_retries_count\"",\""default\"":\""3\""}"",
  ""name"": ""max_retries_count"",
  ""mode"": ""0"",
  ""decimal_digits"": 0,
  ""min"": 0,
  ""max"": 10,
  ""step"": 1,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: f7bf787c-c137-41d4-b354-024228bea833 >>
			{
				var id = new Guid("f7bf787c-c137-41d4-b354-024228bea833");
				Guid? parentId = new Guid("5e5ea999-7008-4236-aa2f-cc3b0dfe174c");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Default sender email address"",
  ""mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.default_sender_email\"",\""default\"":\""\""}"",
  ""name"": ""default_sender_email"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 5e34424b-315c-4dfa-87f0-2d852eb456bd >>
			{
				var id = new Guid("5e34424b-315c-4dfa-87f0-2d852eb456bd");
				Guid? parentId = new Guid("5e5ea999-7008-4236-aa2f-cc3b0dfe174c");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Default RetryTo email address"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.default_reply_to_email\"",\""default\"":\""\""}"",
  ""name"": ""default_reply_to_email"",
  ""mode"": ""0"",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 7320e565-42f9-412a-a131-2825bfd3ed99 >>
			{
				var id = new Guid("7320e565-42f9-412a-a131-2825bfd3ed99");
				Guid? parentId = new Guid("5e5ea999-7008-4236-aa2f-cc3b0dfe174c");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Default sender name"",
  ""mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.default_sender_name\"",\""default\"":\""\""}"",
  ""name"": ""default_sender_name"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 066f4520-532d-4ac6-bd5f-da6cfdcac488 >>
			{
				var id = new Guid("066f4520-532d-4ac6-bd5f-da6cfdcac488");
				Guid? parentId = new Guid("2db9c1a4-84c3-4c5a-a7f8-525540bbc36f");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{
  ""visible_columns"": 2,
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
  ""container1_flex_self_align"": ""1"",
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
  ""container2_flex_self_align"": ""1"",
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
  ""container3_flex_self_align"": ""1"",
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
  ""container4_flex_self_align"": ""1"",
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
  ""container5_flex_self_align"": ""1"",
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
  ""container6_flex_self_align"": ""1"",
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
  ""container7_flex_self_align"": ""1"",
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
  ""container8_flex_self_align"": ""1"",
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
  ""container9_flex_self_align"": ""1"",
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
  ""container10_flex_self_align"": ""1"",
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
  ""container11_flex_self_align"": ""1"",
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
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: ca4b97d9-598e-4935-93a0-534bbc6cce6a >>
			{
				var id = new Guid("ca4b97d9-598e-4935-93a0-534bbc6cce6a");
				Guid? parentId = new Guid("066f4520-532d-4ac6-bd5f-da6cfdcac488");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Name"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""name"": ""name"",
  ""mode"": ""0"",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 444644ae-aa91-4b97-a683-9b8dfb99efca >>
			{
				var id = new Guid("444644ae-aa91-4b97-a683-9b8dfb99efca");
				Guid? parentId = new Guid("066f4520-532d-4ac6-bd5f-da6cfdcac488");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column2";
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
  ""container1_flex_self_align"": ""1"",
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
  ""container2_flex_self_align"": ""1"",
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
  ""container3_flex_self_align"": ""1"",
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
  ""container4_flex_self_align"": ""1"",
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
  ""container5_flex_self_align"": ""1"",
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
  ""container6_flex_self_align"": ""1"",
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
  ""container7_flex_self_align"": ""1"",
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
  ""container8_flex_self_align"": ""1"",
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
  ""container9_flex_self_align"": ""1"",
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
  ""container10_flex_self_align"": ""1"",
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
  ""container11_flex_self_align"": ""1"",
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
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 75102c57-af60-48f5-9fbc-aa3508eb44d7 >>
			{
				var id = new Guid("75102c57-af60-48f5-9fbc-aa3508eb44d7");
				Guid? parentId = new Guid("444644ae-aa91-4b97-a683-9b8dfb99efca");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldPassword";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Password"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.password\"",\""default\"":\""\""}"",
  ""name"": ""password"",
  ""mode"": ""0"",
  ""min"": 0,
  ""max"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: 96d7a5e3-5bf1-4844-8e3e-46ae85b5fe79 >>
			{
				var id = new Guid("96d7a5e3-5bf1-4844-8e3e-46ae85b5fe79");
				Guid? parentId = new Guid("444644ae-aa91-4b97-a683-9b8dfb99efca");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Username"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.username\"",\""default\"":\""\""}"",
  ""name"": ""username"",
  ""mode"": ""0"",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: create  id: f8e25983-1f77-4b98-b9da-399bbca4e57c >>
			{
				var id = new Guid("f8e25983-1f77-4b98-b9da-399bbca4e57c");
				Guid? parentId = new Guid("444644ae-aa91-4b97-a683-9b8dfb99efca");
				Guid? nodeId = null;
				var pageId = new Guid("b5002548-daf7-456f-aa2c-43c205050195");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Connection security"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.connection_security\"",\""default\"":\""\""}"",
  ""name"": ""connection_security"",
  ""options"": """",
  ""mode"": ""0"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: f4f1a2d9-8c0b-4e6f-8495-593fc53ca0d8 >>
			{
				var id = new Guid("f4f1a2d9-8c0b-4e6f-8495-593fc53ca0d8");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
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
  ""icon_class"": ""fa fa-plus"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 77e49a48-840b-43a1-afb8-cebd2b874ff2 >>
			{
				var id = new Guid("77e49a48-840b-43a1-afb8-cebd2b874ff2");
				Guid? parentId = new Guid("f4f1a2d9-8c0b-4e6f-8495-593fc53ca0d8");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Delete service"",
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
  ""form"": ""deleteServiceForm""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 784494c6-d145-4062-a7c1-efdb1422a57e >>
			{
				var id = new Guid("784494c6-d145-4062-a7c1-efdb1422a57e");
				Guid? parentId = new Guid("f4f1a2d9-8c0b-4e6f-8495-593fc53ca0d8");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""2"",
  ""text"": ""Test service"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_visible"": """",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class TestServiceUrlCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t    if (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar queryRec = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RequestQuery\\\"");\\n\\t\\tvar recordId = pageModel.TryGetDataSourceProperty<Guid>(\\\""Record.id\\\"");\\n\\n\\t\\tif (recordId == null)\\n\\t\\t\\treturn null;\\n\\n        string queryString = string.Empty;\\n        if(queryRec != null)\\n        {\\n            foreach(var key in queryRec.Properties.Keys )\\n            {\\n                queryString +=  $\\\""{key}={queryRec[key]}&\\\"";\\n            }\\n        }\\n        \\n        var returnUrl = System.Net.WebUtility.UrlEncode( $\\\""/mail/services/smtp/r/{recordId}/details?{queryString}\\\"" );\\n\\t\\treturn $\\\""/mail/services/smtp/r/{recordId}/service_test?returnUrl={returnUrl}\\\"";\\n\\t}\\n}\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: fb0885f1-9377-437b-8851-3c97c25a9b5b >>
			{
				var id = new Guid("fb0885f1-9377-437b-8851-3c97c25a9b5b");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{}";
				var weight = 5;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 314c56d3-4e85-4e35-8af7-5f909750139e >>
			{
				var id = new Guid("314c56d3-4e85-4e35-8af7-5f909750139e");
				Guid? parentId = new Guid("fb0885f1-9377-437b-8851-3c97c25a9b5b");
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Service"",
  ""mode"": ""2"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\nusing WebVella.Erp.Plugins.Mail.Api;\\n\\npublic class ViewEmailUrlCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar serviceId = pageModel.TryGetDataSourceProperty<Guid>(\\\""Record.service_id\\\"");\\n\\n\\t\\tif (serviceId == null)\\n\\t\\t\\treturn \\\""SMTP service not found\\\"";\\n\\t\\t\\t\\n\\t\\tServiceManager serviceManager = new ServiceManager();\\n\\t\\tvar smtpService = serviceManager.GetSmtpService(serviceId);\\n\\t\\tif( smtpService == null )\\n\\t\\t    return \\\""SMTP service not found\\\"";\\n\\t\\t    \\n\\t\\treturn smtpService.Name;\\n\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""name"": ""service_id"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: be2bc9e8-83d6-4e0d-b6ea-223daa630f76 >>
			{
				var id = new Guid("be2bc9e8-83d6-4e0d-b6ea-223daa630f76");
				Guid? parentId = new Guid("fb0885f1-9377-437b-8851-3c97c25a9b5b");
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column2";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Status"",
  ""mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.status\"",\""default\"":\""\""}"",
  ""name"": ""status"",
  ""class"": """",
  ""options"": """",
  ""show_icon"": ""false"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 5cbdcf2f-e178-41e2-aec8-f629cd549914 >>
			{
				var id = new Guid("5cbdcf2f-e178-41e2-aec8-f629cd549914");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("31c40750-99c7-4402-9e9b-8157e9459df7");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 4,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""AllSmtpSevices\"",\""default\"":\""\""}"",
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
  ""has_tfoot"": ""false"",
  ""container1_label"": """",
  ""container1_width"": ""40px"",
  ""container1_name"": """",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_class"": """",
  ""container1_vertical_align"": ""1"",
  ""container1_horizontal_align"": ""3"",
  ""container2_label"": ""default"",
  ""container2_width"": ""30px"",
  ""container2_name"": ""is_default"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""false"",
  ""container2_class"": """",
  ""container2_vertical_align"": ""3"",
  ""container2_horizontal_align"": ""1"",
  ""container3_label"": ""enabled"",
  ""container3_width"": ""30px"",
  ""container3_name"": ""is_enabled"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""false"",
  ""container3_class"": """",
  ""container3_vertical_align"": ""3"",
  ""container3_horizontal_align"": ""1"",
  ""container4_label"": ""name"",
  ""container4_width"": """",
  ""container4_name"": ""name"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""false"",
  ""container4_class"": """",
  ""container4_vertical_align"": ""3"",
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
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 2ccb07b7-420e-4b80-b015-60ad6f117a24 >>
			{
				var id = new Guid("2ccb07b7-420e-4b80-b015-60ad6f117a24");
				Guid? parentId = new Guid("5cbdcf2f-e178-41e2-aec8-f629cd549914");
				Guid? nodeId = null;
				var pageId = new Guid("31c40750-99c7-4402-9e9b-8157e9459df7");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column4";
				var options = @"{
  ""label_text"": ""name"",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.name\"",\""default\"":\""Smtp service name\""}"",
  ""name"": ""name"",
  ""mode"": ""4"",
  ""maxlength"": 100,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 508e3821-b4cb-4eb6-89dd-5ea1f55c0809 >>
			{
				var id = new Guid("508e3821-b4cb-4eb6-89dd-5ea1f55c0809");
				Guid? parentId = new Guid("5cbdcf2f-e178-41e2-aec8-f629cd549914");
				Guid? nodeId = null;
				var pageId = new Guid("31c40750-99c7-4402-9e9b-8157e9459df7");
				var componentName = "WebVella.Erp.Web.Components.PcFieldCheckbox";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Is default Smtp service"",
  ""label_mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.is_default\"",\""default\"":\""\""}"",
  ""name"": ""is_default"",
  ""mode"": ""4"",
  ""text_true"": ""yes"",
  ""text_false"": """",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 5e357a9f-3052-4bd0-be09-1b4ba2d06ce7 >>
			{
				var id = new Guid("5e357a9f-3052-4bd0-be09-1b4ba2d06ce7");
				Guid? parentId = new Guid("5cbdcf2f-e178-41e2-aec8-f629cd549914");
				Guid? nodeId = null;
				var pageId = new Guid("31c40750-99c7-4402-9e9b-8157e9459df7");
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
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class SelectOptionsConvertCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\t//replace constants with your values\\n\\t\\tconst string DATASOURCE_NAME = \\\""RowRecord.id\\\"";\\n\\n\\t\\t//if pageModel is not provided, returns empty List<SelectOption>()\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\t//try read data source by name and get result as specified type object\\n\\t\\tvar dataSourceValue = pageModel.TryGetDataSourceProperty<Guid>(DATASOURCE_NAME);\\n\\n\\t\\t//if data source not found or different type, return empty List<SelectOption>()\\n\\t\\tif (dataSourceValue == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\treturn $\\\""/mail/services/smtp/r/{dataSourceValue}/details?returnUrl=/mail/services/smtp/l/\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all  id: 1a6c8d01-27cd-4204-9aaa-5b23f0139e54 >>
			{
				var id = new Guid("1a6c8d01-27cd-4204-9aaa-5b23f0139e54");
				Guid? parentId = new Guid("5cbdcf2f-e178-41e2-aec8-f629cd549914");
				Guid? nodeId = null;
				var pageId = new Guid("31c40750-99c7-4402-9e9b-8157e9459df7");
				var componentName = "WebVella.Erp.Web.Components.PcFieldCheckbox";
				var containerId = "column3";
				var options = @"{
  ""label_mode"": ""3"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.is_enabled\"",\""default\"":\""\""}"",
  ""name"": ""is_enabled"",
  ""class"": """",
  ""text_true"": """",
  ""text_false"": """",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 161da05f-db82-4afc-8990-b68c4eb5d036 >>
			{
				var id = new Guid("161da05f-db82-4afc-8990-b68c4eb5d036");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{}";
				var weight = 4;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 5a39ccba-1478-477e-a7a9-243601e995ab >>
			{
				var id = new Guid("5a39ccba-1478-477e-a7a9-243601e995ab");
				Guid? parentId = new Guid("161da05f-db82-4afc-8990-b68c4eb5d036");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column2";
				var options = @"{}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 64914a17-9d25-4e9a-bc63-28b5dd59f505 >>
			{
				var id = new Guid("64914a17-9d25-4e9a-bc63-28b5dd59f505");
				Guid? parentId = new Guid("5a39ccba-1478-477e-a7a9-243601e995ab");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Number of retries on error"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.max_retries_count\"",\""default\"":\""3\""}"",
  ""name"": ""max_retries_count"",
  ""mode"": ""3"",
  ""decimal_digits"": 0,
  ""min"": 0,
  ""max"": 10,
  ""step"": 1,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: affb5873-a71e-460a-b080-21b244e7eff5 >>
			{
				var id = new Guid("affb5873-a71e-460a-b080-21b244e7eff5");
				Guid? parentId = new Guid("5a39ccba-1478-477e-a7a9-243601e995ab");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Wait period between retries (minutes)"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.retry_wait_minutes\"",\""default\"":\""60\""}"",
  ""name"": ""retry_wait_minutes"",
  ""mode"": ""3"",
  ""decimal_digits"": 0,
  ""min"": 0,
  ""max"": 1440,
  ""step"": 1,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: a2a8358a-b666-4012-b9b4-3dfe9b251df7 >>
			{
				var id = new Guid("a2a8358a-b666-4012-b9b4-3dfe9b251df7");
				Guid? parentId = new Guid("161da05f-db82-4afc-8990-b68c4eb5d036");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Default RetryTo email address"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.default_reply_to_email\"",\""default\"":\""\""}"",
  ""name"": ""default_reply_to_email"",
  ""mode"": ""3"",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 18bbbe7a-7a36-458d-a95c-a1ff0ff501a4 >>
			{
				var id = new Guid("18bbbe7a-7a36-458d-a95c-a1ff0ff501a4");
				Guid? parentId = new Guid("161da05f-db82-4afc-8990-b68c4eb5d036");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Default sender email address"",
  ""mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.default_sender_email\"",\""default\"":\""\""}"",
  ""name"": ""default_sender_email"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: dce9e4f0-3e7b-471a-ada0-9a39babec0e8 >>
			{
				var id = new Guid("dce9e4f0-3e7b-471a-ada0-9a39babec0e8");
				Guid? parentId = new Guid("161da05f-db82-4afc-8990-b68c4eb5d036");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Default sender name"",
  ""mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.default_sender_name\"",\""default\"":\""\""}"",
  ""name"": ""default_sender_name"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 3682d81a-68d1-413d-898f-e4d7c77e0380 >>
			{
				var id = new Guid("3682d81a-68d1-413d-898f-e4d7c77e0380");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
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
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_self_align"": ""1"",
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
  ""container2_flex_self_align"": ""1"",
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
  ""container3_flex_self_align"": ""1"",
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
  ""container4_flex_self_align"": ""1"",
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
  ""container5_flex_self_align"": ""1"",
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
  ""container6_flex_self_align"": ""1"",
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
  ""container7_flex_self_align"": ""1"",
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
  ""container8_flex_self_align"": ""1"",
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
  ""container9_flex_self_align"": ""1"",
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
  ""container10_flex_self_align"": ""1"",
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
  ""container11_flex_self_align"": ""1"",
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
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: ac9c7a5c-2c75-4dbe-b29a-40d28da5ec2f >>
			{
				var id = new Guid("ac9c7a5c-2c75-4dbe-b29a-40d28da5ec2f");
				Guid? parentId = new Guid("3682d81a-68d1-413d-898f-e4d7c77e0380");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column2";
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
  ""container1_flex_self_align"": ""1"",
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
  ""container2_flex_self_align"": ""1"",
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
  ""container3_flex_self_align"": ""1"",
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
  ""container4_flex_self_align"": ""1"",
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
  ""container5_flex_self_align"": ""1"",
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
  ""container6_flex_self_align"": ""1"",
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
  ""container7_flex_self_align"": ""1"",
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
  ""container8_flex_self_align"": ""1"",
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
  ""container9_flex_self_align"": ""1"",
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
  ""container10_flex_self_align"": ""1"",
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
  ""container11_flex_self_align"": ""1"",
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
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 182d70f7-d496-4ac7-8603-80d7fbb8c4e6 >>
			{
				var id = new Guid("182d70f7-d496-4ac7-8603-80d7fbb8c4e6");
				Guid? parentId = new Guid("ac9c7a5c-2c75-4dbe-b29a-40d28da5ec2f");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldCheckbox";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Is default service"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.is_default\"",\""default\"":\""\""}"",
  ""name"": ""is_default"",
  ""mode"": ""3"",
  ""text_true"": """",
  ""text_false"": """",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: aff2f960-6a64-4344-8105-f2aa3c2b52a1 >>
			{
				var id = new Guid("aff2f960-6a64-4344-8105-f2aa3c2b52a1");
				Guid? parentId = new Guid("ac9c7a5c-2c75-4dbe-b29a-40d28da5ec2f");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldNumber";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Port"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.port\"",\""default\"":\""25\""}"",
  ""name"": ""port"",
  ""mode"": ""3"",
  ""decimal_digits"": 0,
  ""min"": 1,
  ""max"": 65535,
  ""step"": 1,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: f919c315-814d-47c5-b7b9-4d62e5f1e896 >>
			{
				var id = new Guid("f919c315-814d-47c5-b7b9-4d62e5f1e896");
				Guid? parentId = new Guid("ac9c7a5c-2c75-4dbe-b29a-40d28da5ec2f");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldCheckbox";
				var containerId = "column3";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Is enabled"",
  ""mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.is_enabled\"",\""default\"":\""true\""}"",
  ""name"": ""is_enabled"",
  ""class"": """",
  ""text_true"": """",
  ""text_false"": """",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 2107a419-fa2f-4ecf-a17a-12a0b4551d67 >>
			{
				var id = new Guid("2107a419-fa2f-4ecf-a17a-12a0b4551d67");
				Guid? parentId = new Guid("3682d81a-68d1-413d-898f-e4d7c77e0380");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column1";
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
  ""container1_flex_self_align"": ""1"",
  ""container1_flex_order"": 0,
  ""container2_span"": 3,
  ""container2_span_sm"": 0,
  ""container2_span_md"": 0,
  ""container2_span_lg"": 0,
  ""container2_span_xl"": 0,
  ""container2_offset"": 0,
  ""container2_offset_sm"": 0,
  ""container2_offset_md"": 0,
  ""container2_offset_lg"": 0,
  ""container2_offset_xl"": 0,
  ""container2_flex_self_align"": ""1"",
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
  ""container3_flex_self_align"": ""1"",
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
  ""container4_flex_self_align"": ""1"",
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
  ""container5_flex_self_align"": ""1"",
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
  ""container6_flex_self_align"": ""1"",
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
  ""container7_flex_self_align"": ""1"",
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
  ""container8_flex_self_align"": ""1"",
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
  ""container9_flex_self_align"": ""1"",
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
  ""container10_flex_self_align"": ""1"",
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
  ""container11_flex_self_align"": ""1"",
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
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: ea760805-23c6-4542-8066-7ad085f09e81 >>
			{
				var id = new Guid("ea760805-23c6-4542-8066-7ad085f09e81");
				Guid? parentId = new Guid("2107a419-fa2f-4ecf-a17a-12a0b4551d67");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Server"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.server\"",\""default\"":\""mail.domain.com\""}"",
  ""name"": ""server"",
  ""mode"": ""3"",
  ""maxlength"": 0,
  ""connected_entity_id"": ""17698b9f-e533-4f8d-a651-a00f7de2989e""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 66e3d827-9d68-4dcf-813d-e093506e09ca >>
			{
				var id = new Guid("66e3d827-9d68-4dcf-813d-e093506e09ca");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
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
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_self_align"": ""1"",
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
  ""container2_flex_self_align"": ""1"",
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
  ""container3_flex_self_align"": ""1"",
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
  ""container4_flex_self_align"": ""1"",
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
  ""container5_flex_self_align"": ""1"",
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
  ""container6_flex_self_align"": ""1"",
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
  ""container7_flex_self_align"": ""1"",
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
  ""container8_flex_self_align"": ""1"",
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
  ""container9_flex_self_align"": ""1"",
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
  ""container10_flex_self_align"": ""1"",
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
  ""container11_flex_self_align"": ""1"",
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
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 1d46346b-ad9b-4b57-85f5-382207096486 >>
			{
				var id = new Guid("1d46346b-ad9b-4b57-85f5-382207096486");
				Guid? parentId = new Guid("66e3d827-9d68-4dcf-813d-e093506e09ca");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Name"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.name\"",\""default\"":\""\""}"",
  ""name"": ""name"",
  ""mode"": ""3"",
  ""maxlength"": 100,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 89d53ce4-5376-4405-a7fb-df7a727e6039 >>
			{
				var id = new Guid("89d53ce4-5376-4405-a7fb-df7a727e6039");
				Guid? parentId = new Guid("66e3d827-9d68-4dcf-813d-e093506e09ca");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "column2";
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
  ""container1_flex_self_align"": ""1"",
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
  ""container2_flex_self_align"": ""1"",
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
  ""container3_flex_self_align"": ""1"",
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
  ""container4_flex_self_align"": ""1"",
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
  ""container5_flex_self_align"": ""1"",
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
  ""container6_flex_self_align"": ""1"",
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
  ""container7_flex_self_align"": ""1"",
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
  ""container8_flex_self_align"": ""1"",
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
  ""container9_flex_self_align"": ""1"",
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
  ""container10_flex_self_align"": ""1"",
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
  ""container11_flex_self_align"": ""1"",
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
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: f1d71a11-7062-4c33-8dab-13336ea8a315 >>
			{
				var id = new Guid("f1d71a11-7062-4c33-8dab-13336ea8a315");
				Guid? parentId = new Guid("89d53ce4-5376-4405-a7fb-df7a727e6039");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_text"": ""Username"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.username\"",\""default\"":\""\""}"",
  ""name"": ""username"",
  ""mode"": ""3"",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: fc2ffe53-1602-403b-9167-5a49fcaf4e24 >>
			{
				var id = new Guid("fc2ffe53-1602-403b-9167-5a49fcaf4e24");
				Guid? parentId = new Guid("89d53ce4-5376-4405-a7fb-df7a727e6039");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldPassword";
				var containerId = "column3";
				var options = @"{
  ""label_text"": ""Password"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.password\"",\""default\"":\""\""}"",
  ""name"": ""password"",
  ""mode"": ""3"",
  ""min"": 0,
  ""max"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 2a82c7d6-ffa5-419f-8585-2d75eb0ef2af >>
			{
				var id = new Guid("2a82c7d6-ffa5-419f-8585-2d75eb0ef2af");
				Guid? parentId = new Guid("89d53ce4-5376-4405-a7fb-df7a727e6039");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column1";
				var options = @"{
  ""label_text"": ""Connection security"",
  ""label_mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.connection_security\"",\""default\"":\""\""}"",
  ""name"": ""connection_security"",
  ""options"": """",
  ""mode"": ""3"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: e0033958-7520-4609-8730-4ecc681cc520 >>
			{
				var id = new Guid("e0033958-7520-4609-8730-4ecc681cc520");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
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
  ""icon_class"": ""fa fa-plus"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: c8453cf4-e2c5-4ee8-bac3-68b50b2ca179 >>
			{
				var id = new Guid("c8453cf4-e2c5-4ee8-bac3-68b50b2ca179");
				Guid? parentId = new Guid("e0033958-7520-4609-8730-4ecc681cc520");
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Send Now"",
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
  ""form"": ""formSendNow""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: 211c5b44-92b9-4cbc-9092-d6a45479e313 >>
			{
				var id = new Guid("211c5b44-92b9-4cbc-9092-d6a45479e313");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcDrawer";
				var containerId = "";
				var options = @"{
  ""title"": ""Search emails"",
  ""width"": ""550px"",
  ""class"": """",
  ""body_class"": """",
  ""title_action_html"": """"
}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: dcf1c3ce-9e1c-48f4-8054-f3426e7f3782 >>
			{
				var id = new Guid("dcf1c3ce-9e1c-48f4-8054-f3426e7f3782");
				Guid? parentId = new Guid("211c5b44-92b9-4cbc-9092-d6a45479e313");
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "body";
				var options = @"{
  ""id"": ""wv-dcf1c3ce-9e1c-48f4-8054-f3426e7f3782"",
  ""name"": ""form"",
  ""hook_key"": """",
  ""method"": ""get"",
  ""label_mode"": ""1"",
  ""mode"": ""1"",
  ""class"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: 47375368-90a1-4da7-a3a2-fda488fa93bc >>
			{
				var id = new Guid("47375368-90a1-4da7-a3a2-fda488fa93bc");
				Guid? parentId = new Guid("dcf1c3ce-9e1c-48f4-8054-f3426e7f3782");
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcGridFilterField";
				var containerId = "body";
				var options = @"{
  ""label"": ""Emails, subject and content"",
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

			#region << ***Create page body node*** Page name: all_emails  id: c903a30f-c75f-414f-81d7-282ce0f53091 >>
			{
				var id = new Guid("c903a30f-c75f-414f-81d7-282ce0f53091");
				Guid? parentId = new Guid("dcf1c3ce-9e1c-48f4-8054-f3426e7f3782");
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "body";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Search"",
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

			#region << ***Create page body node*** Page name: details  id: d1f634e6-e0f0-4164-a473-dd924b90cdac >>
			{
				var id = new Guid("d1f634e6-e0f0-4164-a473-dd924b90cdac");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "";
				var options = @"{
  ""id"": ""deleteServiceForm"",
  ""name"": ""form"",
  ""hook_key"": ""delete"",
  ""method"": ""post"",
  ""label_mode"": ""1"",
  ""mode"": ""1"",
  ""class"": """"
}";
				var weight = 5;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 02fc1355-6324-48b5-880c-891e56e7b931 >>
			{
				var id = new Guid("02fc1355-6324-48b5-880c-891e56e7b931");
				Guid? parentId = new Guid("d1f634e6-e0f0-4164-a473-dd924b90cdac");
				Guid? nodeId = null;
				var pageId = new Guid("6ee77414-ca24-4664-b73a-7c3cdd9c6bbb");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "body";
				var options = @"{
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}"",
  ""name"": ""id""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: 61cb13ad-4ac6-4fe3-a15d-615a8e225270 >>
			{
				var id = new Guid("61cb13ad-4ac6-4fe3-a15d-615a8e225270");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
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
  ""icon_class"": ""fa fa-file"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: 8d4c670e-dedb-471d-b9b2-d6f91a1c52ef >>
			{
				var id = new Guid("8d4c670e-dedb-471d-b9b2-d6f91a1c52ef");
				Guid? parentId = new Guid("61cb13ad-4ac6-4fe3-a15d-615a8e225270");
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
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

			#region << ***Create page body node*** Page name: details  id: 8b0f6ff1-7eaa-41b1-9879-4908554c2305 >>
			{
				var id = new Guid("8b0f6ff1-7eaa-41b1-9879-4908554c2305");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "";
				var options = @"{
  ""id"": ""formSendNow"",
  ""name"": ""formSendNow"",
  ""hook_key"": ""email_send_now"",
  ""method"": ""post"",
  ""label_mode"": ""1"",
  ""mode"": ""1"",
  ""class"": """"
}";
				var weight = 7;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 4900ebdf-cdea-4caa-92a1-bbfb0492a2d3 >>
			{
				var id = new Guid("4900ebdf-cdea-4caa-92a1-bbfb0492a2d3");
				Guid? parentId = new Guid("8b0f6ff1-7eaa-41b1-9879-4908554c2305");
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHidden";
				var containerId = "body";
				var options = @"{
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.id\"",\""default\"":\""\""}"",
  ""name"": ""id""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: 5dfef806-4448-4bce-8a5d-91e8587cbe33 >>
			{
				var id = new Guid("5dfef806-4448-4bce-8a5d-91e8587cbe33");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcGrid";
				var containerId = "";
				var options = @"{
  ""visible_columns"": 7,
  ""records"": ""{\""type\"":\""0\"",\""string\"":\""AllEmails\"",\""default\"":\""\""}"",
  ""id"": """",
  ""name"": """",
  ""prefix"": """",
  ""class"": """",
  ""striped"": ""false"",
  ""small"": ""false"",
  ""bordered"": ""false"",
  ""borderless"": ""false"",
  ""hover"": ""false"",
  ""responsive_breakpoint"": ""0"",
  ""empty_text"": ""No records"",
  ""has_thead"": ""true"",
  ""has_tfoot"": ""true"",
  ""container1_label"": """",
  ""container1_width"": ""10px"",
  ""container1_name"": """",
  ""container1_nowrap"": ""false"",
  ""container1_sortable"": ""false"",
  ""container1_class"": """",
  ""container1_vertical_align"": ""3"",
  ""container1_horizontal_align"": ""3"",
  ""container2_label"": ""Recipient"",
  ""container2_width"": ""20%"",
  ""container2_name"": ""recipient_email"",
  ""container2_nowrap"": ""false"",
  ""container2_sortable"": ""true"",
  ""container2_class"": """",
  ""container2_vertical_align"": ""1"",
  ""container2_horizontal_align"": ""1"",
  ""container3_label"": ""Sender"",
  ""container3_width"": ""20%"",
  ""container3_name"": ""sender_email"",
  ""container3_nowrap"": ""false"",
  ""container3_sortable"": ""true"",
  ""container3_class"": """",
  ""container3_vertical_align"": ""1"",
  ""container3_horizontal_align"": ""1"",
  ""container4_label"": ""Subject"",
  ""container4_width"": ""40%"",
  ""container4_name"": ""subject"",
  ""container4_nowrap"": ""false"",
  ""container4_sortable"": ""true"",
  ""container4_class"": """",
  ""container4_vertical_align"": ""1"",
  ""container4_horizontal_align"": ""1"",
  ""container5_label"": ""Created On"",
  ""container5_width"": ""80px"",
  ""container5_name"": ""created_on"",
  ""container5_nowrap"": ""true"",
  ""container5_sortable"": ""true"",
  ""container5_class"": """",
  ""container5_vertical_align"": ""1"",
  ""container5_horizontal_align"": ""1"",
  ""container6_label"": ""Sent On"",
  ""container6_width"": ""80px"",
  ""container6_name"": ""sent_on"",
  ""container6_nowrap"": ""true"",
  ""container6_sortable"": ""true"",
  ""container6_class"": """",
  ""container6_vertical_align"": ""1"",
  ""container6_horizontal_align"": ""1"",
  ""container7_label"": ""Status"",
  ""container7_width"": ""10px"",
  ""container7_name"": ""status"",
  ""container7_nowrap"": ""true"",
  ""container7_sortable"": ""true"",
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
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: 3bfea9b2-82eb-4cff-9472-ad4314fe22ba >>
			{
				var id = new Guid("3bfea9b2-82eb-4cff-9472-ad4314fe22ba");
				Guid? parentId = new Guid("5dfef806-4448-4bce-8a5d-91e8587cbe33");
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDateTime";
				var containerId = "column5";
				var options = @"{
  ""label_mode"": ""3"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.created_on\"",\""default\"":\""\""}"",
  ""name"": ""created_on"",
  ""class"": """",
  ""show_icon"": ""false"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: 811bafab-6be0-42d1-9c92-70cd1aee1e0e >>
			{
				var id = new Guid("811bafab-6be0-42d1-9c92-70cd1aee1e0e");
				Guid? parentId = new Guid("5dfef806-4448-4bce-8a5d-91e8587cbe33");
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldSelect";
				var containerId = "column7";
				var options = @"{
  ""label_mode"": ""3"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.status\"",\""default\"":\""\""}"",
  ""name"": ""status"",
  ""class"": """",
  ""options"": """",
  ""show_icon"": ""false"",
  ""connected_entity_id"": ""085e2442-820a-4df7-ab92-516ce23197c4""
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: 60a62365-f290-421e-af82-30bbd6474ea9 >>
			{
				var id = new Guid("60a62365-f290-421e-af82-30bbd6474ea9");
				Guid? parentId = new Guid("5dfef806-4448-4bce-8a5d-91e8587cbe33");
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column3";
				var options = @"{
  ""label_mode"": ""3"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.sender_email\"",\""default\"":\""sender@domain.com\""}"",
  ""name"": ""sender_email"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: 7be29cb8-8449-451a-b6de-3bba406f1b48 >>
			{
				var id = new Guid("7be29cb8-8449-451a-b6de-3bba406f1b48");
				Guid? parentId = new Guid("5dfef806-4448-4bce-8a5d-91e8587cbe33");
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_mode"": ""3"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.recipient_email\"",\""default\"":\""recipient@domain.com\""}"",
  ""name"": ""recipient_email"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: 5c3e5439-2dfa-4813-abaf-5c6180db9bfd >>
			{
				var id = new Guid("5c3e5439-2dfa-4813-abaf-5c6180db9bfd");
				Guid? parentId = new Guid("5dfef806-4448-4bce-8a5d-91e8587cbe33");
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldDateTime";
				var containerId = "column6";
				var options = @"{
  ""label_mode"": ""3"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.sent_on\"",\""default\"":\""---\""}"",
  ""name"": ""field"",
  ""class"": """",
  ""show_icon"": ""false"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: 71c52f52-db94-41dd-a6b8-2091fbb671d5 >>
			{
				var id = new Guid("71c52f52-db94-41dd-a6b8-2091fbb671d5");
				Guid? parentId = new Guid("5dfef806-4448-4bce-8a5d-91e8587cbe33");
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column4";
				var options = @"{
  ""label_mode"": ""3"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.subject\"",\""default\"":\""subject\""}"",
  ""name"": ""subject"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: 555c9704-efe8-4e15-832f-9f49ef553e16 >>
			{
				var id = new Guid("555c9704-efe8-4e15-832f-9f49ef553e16");
				Guid? parentId = new Guid("5dfef806-4448-4bce-8a5d-91e8587cbe33");
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column7";
				var options = @"{
  ""label_mode"": ""3"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class ErrorCodeHtmlVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n        var recordId  = pageModel.TryGetDataSourceProperty<Guid>(\\\""RowRecord.id\\\"");\\n\\t\\tvar serverError = pageModel.TryGetDataSourceProperty<string>(\\\""RowRecord.server_error\\\"");\\n\\t\\tvar retriesCount = pageModel.TryGetDataSourceProperty<decimal>(\\\""RowRecord.retries_count\\\"");\\n\\t\\t\\n\\t\\tif( string.IsNullOrWhiteSpace(serverError))\\n\\t\\t    return \\\""\\\"";\\n\\t\\t    \\n\\t\\t serverError = $\\\""Atempts to send: {retriesCount}&#xA;Error: \\\"" + serverError;\\n\\t\\t return $\\\""<i class='fas fa-exclamation-triangle' style='color:#CC0000' title='{serverError}'></i> &nbsp;\\\"";\\n\\t}\\n}\"",\""default\"":\""\""}"",
  ""name"": ""field"",
  ""class"": """",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: all_emails  id: d46a084d-4390-4134-80f4-c2fa4ceba811 >>
			{
				var id = new Guid("d46a084d-4390-4134-80f4-c2fa4ceba811");
				Guid? parentId = new Guid("5dfef806-4448-4bce-8a5d-91e8587cbe33");
				Guid? nodeId = null;
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
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
  ""href"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class ViewEmailUrlCodeVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar queryRec = pageModel.TryGetDataSourceProperty<EntityRecord>(\\\""RequestQuery\\\"");\\n\\t\\tvar recordId = pageModel.TryGetDataSourceProperty<Guid>(\\\""RowRecord.id\\\"");\\n\\n\\t\\tif (recordId == null)\\n\\t\\t\\treturn null;\\n\\n        string queryString = string.Empty;\\n        if(queryRec != null)\\n        {\\n            foreach(var key in queryRec.Properties.Keys )\\n            {\\n                queryString +=  $\\\""{key}={queryRec[key]}&\\\"";\\n            }\\n        }\\n        \\n        var returnUrl = System.Net.WebUtility.UrlEncode( $\\\""/mail/emails/all/l?{queryString}\\\"" );\\n\\t\\treturn $\\\""/mail/emails/all/r/{recordId}/details?returnUrl={returnUrl}\\\"";\\n\\t}\\n}\\n\"",\""default\"":\""\""}"",
  ""new_tab"": ""false"",
  ""form"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: e04237dc-3454-4309-b183-09e83f9bd37c >>
			{
				var id = new Guid("e04237dc-3454-4309-b183-09e83f9bd37c");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
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
  ""container1_span_md"": 0,
  ""container1_span_lg"": 0,
  ""container1_span_xl"": 0,
  ""container1_offset"": 0,
  ""container1_offset_sm"": 0,
  ""container1_offset_md"": 0,
  ""container1_offset_lg"": 0,
  ""container1_offset_xl"": 0,
  ""container1_flex_self_align"": ""1"",
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
  ""container2_flex_self_align"": ""1"",
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
  ""container3_flex_self_align"": ""1"",
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
  ""container4_flex_self_align"": ""1"",
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
  ""container5_flex_self_align"": ""1"",
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
  ""container6_flex_self_align"": ""1"",
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
  ""container7_flex_self_align"": ""1"",
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
  ""container8_flex_self_align"": ""1"",
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
  ""container9_flex_self_align"": ""1"",
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
  ""container10_flex_self_align"": ""1"",
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
  ""container11_flex_self_align"": ""1"",
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
  ""container12_flex_self_align"": ""1"",
  ""container12_flex_order"": 0
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 4deda1c4-e4bd-474b-b9cd-71c7d27f5891 >>
			{
				var id = new Guid("4deda1c4-e4bd-474b-b9cd-71c7d27f5891");
				Guid? parentId = new Guid("e04237dc-3454-4309-b183-09e83f9bd37c");
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Recipient"",
  ""mode"": ""2"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class CompositeRecipientEmailVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar recipientName = pageModel.TryGetDataSourceProperty<string>(\\\""Record.recipient_name\\\"");\\n\\t\\tvar recipientEmail = pageModel.TryGetDataSourceProperty<string>(\\\""Record.recipient_email\\\"");\\n\\t\\t\\n\\t\\tif( !string.IsNullOrWhiteSpace(recipientName))\\n\\t\\t    return $\\\""{recipientName} <{recipientEmail}>\\\"";\\n\\t\\telse\\n\\t\\t    return recipientEmail;\\n\\t}\\n}\"",\""default\"":\""\""}"",
  ""name"": ""recipient_email"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 4a08e1b3-eb65-4202-928e-32de6881ea4a >>
			{
				var id = new Guid("4a08e1b3-eb65-4202-928e-32de6881ea4a");
				Guid? parentId = new Guid("e04237dc-3454-4309-b183-09e83f9bd37c");
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Reply to email address"",
  ""mode"": ""2"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.reply_to_email\"",\""default\"":\""\""}"",
  ""name"": ""reply_to_email"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: 3b5fcbef-27e3-44c3-ad38-1202cccab408 >>
			{
				var id = new Guid("3b5fcbef-27e3-44c3-ad38-1202cccab408");
				Guid? parentId = new Guid("e04237dc-3454-4309-b183-09e83f9bd37c");
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Sender"",
  ""mode"": ""2"",
  ""value"": ""{\""type\"":\""1\"",\""string\"":\""using System;\\nusing System.Collections.Generic;\\nusing WebVella.Erp.Web.Models;\\nusing WebVella.Erp.Api.Models;\\n\\npublic class CompositeSenderEmailVariable : ICodeVariable\\n{\\n\\tpublic object Evaluate(BaseErpPageModel pageModel)\\n\\t{\\n\\t\\tif (pageModel == null)\\n\\t\\t\\treturn null;\\n\\n\\t\\tvar senderName = pageModel.TryGetDataSourceProperty<string>(\\\""Record.sender_name\\\"");\\n\\t\\tvar senderEmail = pageModel.TryGetDataSourceProperty<string>(\\\""Record.sender_email\\\"");\\n\\t\\t\\n\\t\\tif( !string.IsNullOrWhiteSpace(senderName))\\n\\t\\t    return $\\\""{senderName} <{senderEmail}>\\\"";\\n\\t\\telse\\n\\t\\t    return senderEmail;\\n\\t}\\n}\"",\""default\"":\""\""}"",
  ""name"": ""sender_email"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: c1eb1a0c-c6b6-401e-9df0-0306708c417d >>
			{
				var id = new Guid("c1eb1a0c-c6b6-401e-9df0-0306708c417d");
				Guid? parentId = new Guid("e04237dc-3454-4309-b183-09e83f9bd37c");
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column1";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Subject"",
  ""mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.subject\"",\""default\"":\""\""}"",
  ""name"": ""subject"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: ff41ddf3-d5f8-4ee8-817a-e383f3d73b44 >>
			{
				var id = new Guid("ff41ddf3-d5f8-4ee8-817a-e383f3d73b44");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "";
				var options = @"{}";
				var weight = 3;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: f6139bf2-a36c-4f18-ad9f-d4eeddb98d10 >>
			{
				var id = new Guid("f6139bf2-a36c-4f18-ad9f-d4eeddb98d10");
				Guid? parentId = new Guid("ff41ddf3-d5f8-4ee8-817a-e383f3d73b44");
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "column2";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Html body:"",
  ""mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.content_html\"",\""default\"":\""\""}"",
  ""name"": ""content_html"",
  ""class"": """",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: details  id: a8a899fb-0859-4467-a648-21eaf9661cf9 >>
			{
				var id = new Guid("a8a899fb-0859-4467-a648-21eaf9661cf9");
				Guid? parentId = new Guid("ff41ddf3-d5f8-4ee8-817a-e383f3d73b44");
				Guid? nodeId = null;
				var pageId = new Guid("24d7c716-fa27-4ccd-99d1-c7a8813a13f2");
				var componentName = "WebVella.Erp.Web.Components.PcFieldTextarea";
				var containerId = "column1";
				var options = @"{
  ""label_mode"": ""0"",
  ""label_text"": ""Text body:"",
  ""mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.content_text\"",\""default\"":\""\""}"",
  ""name"": ""content_text"",
  ""class"": """",
  ""height"": """",
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: service_test  id: 549b1fe3-75cd-4b50-a6e8-62f2d9bd3132 >>
			{
				var id = new Guid("549b1fe3-75cd-4b50-a6e8-62f2d9bd3132");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("27bfa096-8cac-4eea-b4c4-7f50e875e797");
				var componentName = "WebVella.Erp.Web.Components.PcForm";
				var containerId = "";
				var options = @"{
  ""is_visible"": """",
  ""id"": ""sendTestEmailForm"",
  ""name"": ""sendTestEmailForm"",
  ""hook_key"": ""test_smtp_service"",
  ""method"": ""post"",
  ""label_mode"": ""1"",
  ""mode"": ""1"",
  ""class"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: service_test  id: 74f538cf-7ff7-49e3-86b1-617c86aea580 >>
			{
				var id = new Guid("74f538cf-7ff7-49e3-86b1-617c86aea580");
				Guid? parentId = new Guid("549b1fe3-75cd-4b50-a6e8-62f2d9bd3132");
				Guid? nodeId = null;
				var pageId = new Guid("27bfa096-8cac-4eea-b4c4-7f50e875e797");
				var componentName = "WebVella.Erp.Web.Components.PcRow";
				var containerId = "body";
				var options = @"{}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: service_test  id: 097816d0-67e5-427a-8360-a7e815abe57a >>
			{
				var id = new Guid("097816d0-67e5-427a-8360-a7e815abe57a");
				Guid? parentId = new Guid("74f538cf-7ff7-49e3-86b1-617c86aea580");
				Guid? nodeId = null;
				var pageId = new Guid("27bfa096-8cac-4eea-b4c4-7f50e875e797");
				var componentName = "WebVella.Erp.Web.Components.PcFieldText";
				var containerId = "column2";
				var options = @"{
  ""is_visible"": """",
  ""label_mode"": ""0"",
  ""label_text"": ""Subject"",
  ""mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.subject\"",\""default\"":\""This is a test email from WebVella ERP Mail \""}"",
  ""name"": ""subject"",
  ""class"": """",
  ""maxlength"": 0,
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: service_test  id: ea0dab12-e429-46eb-86ea-808698018daf >>
			{
				var id = new Guid("ea0dab12-e429-46eb-86ea-808698018daf");
				Guid? parentId = new Guid("74f538cf-7ff7-49e3-86b1-617c86aea580");
				Guid? nodeId = null;
				var pageId = new Guid("27bfa096-8cac-4eea-b4c4-7f50e875e797");
				var componentName = "WebVella.Erp.Web.Components.PcFieldEmail";
				var containerId = "column1";
				var options = @"{
  ""is_visible"": """",
  ""label_mode"": ""0"",
  ""label_text"": ""Recipient email address"",
  ""mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.recipient_email\"",\""default\"":\""\""}"",
  ""name"": ""recipient_email"",
  ""class"": """",
  ""maxlength"": 0,
  ""show_icon"": ""false"",
  ""connected_entity_id"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: service_test  id: 6f908a06-2daf-4bb0-90f1-593d660cf521 >>
			{
				var id = new Guid("6f908a06-2daf-4bb0-90f1-593d660cf521");
				Guid? parentId = new Guid("549b1fe3-75cd-4b50-a6e8-62f2d9bd3132");
				Guid? nodeId = null;
				var pageId = new Guid("27bfa096-8cac-4eea-b4c4-7f50e875e797");
				var componentName = "WebVella.Erp.Web.Components.PcFieldHtml";
				var containerId = "body";
				var options = @"{
  ""is_visible"": """",
  ""label_mode"": ""0"",
  ""label_text"": ""Email content"",
  ""mode"": ""0"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.content\"",\""default\"":\""Hello,  <br/><br/> this is a test email from WebVella ERP Mail.  <br /><br />Regards, <br />WebVella Team \""}"",
  ""name"": ""content"",
  ""class"": """",
  ""upload_mode"": ""1"",
  ""toolbar_mode"": ""1"",
  ""connected_entity_id"": """"
}";
				var weight = 2;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: service_test  id: 71380054-b83f-4829-a778-0cc237fc2cf2 >>
			{
				var id = new Guid("71380054-b83f-4829-a778-0cc237fc2cf2");
				Guid? parentId = null;
				Guid? nodeId = null;
				var pageId = new Guid("27bfa096-8cac-4eea-b4c4-7f50e875e797");
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
  ""icon_class"": ""fa fa-plus"",
  ""return_url"": """"
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page body node*** Page name: service_test  id: 146f9c91-272b-41a5-b528-1c335a136245 >>
			{
				var id = new Guid("146f9c91-272b-41a5-b528-1c335a136245");
				Guid? parentId = new Guid("71380054-b83f-4829-a778-0cc237fc2cf2");
				Guid? nodeId = null;
				var pageId = new Guid("27bfa096-8cac-4eea-b4c4-7f50e875e797");
				var componentName = "WebVella.Erp.Web.Components.PcButton";
				var containerId = "actions";
				var options = @"{
  ""type"": ""1"",
  ""text"": ""Send test email"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_visible"": """",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": """",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": ""sendTestEmailForm""
}";
				var weight = 1;

				new WebVella.Erp.Web.Services.PageService().CreatePageBodyNode(id, parentId, pageId, nodeId, weight, componentName, containerId, options, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create data source*** Name: AllSmtpSevices >>
			{
				var id = new Guid("e37f0e27-1506-44aa-9196-18a9f43e425c");
				var name = @"AllSmtpSevices";
				var description = @"List of all smtp services";
				var eqlText = @"select * 
from smtp_service";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_smtp_service.""id"" AS ""id"",
	 rec_smtp_service.""name"" AS ""name"",
	 rec_smtp_service.""server"" AS ""server"",
	 rec_smtp_service.""port"" AS ""port"",
	 rec_smtp_service.""username"" AS ""username"",
	 rec_smtp_service.""password"" AS ""password"",
	 rec_smtp_service.""connection_secutity"" AS ""connection_secutity"",
	 rec_smtp_service.""default_from_name"" AS ""default_from_name"",
	 rec_smtp_service.""default_from_email"" AS ""default_from_email"",
	 rec_smtp_service.""default_reply_to_email"" AS ""default_reply_to_email"",
	 rec_smtp_service.""max_retries_count"" AS ""max_retries_count"",
	 rec_smtp_service.""retry_wait_minutes"" AS ""retry_wait_minutes"",
	 rec_smtp_service.""is_default"" AS ""is_default"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_smtp_service
) X
";
				var parametersJson = @"[]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""server"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""port"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""password"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""connection_secutity"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""default_from_name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""default_from_email"",""type"":6,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""default_reply_to_email"",""type"":6,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""max_retries_count"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""retry_wait_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_default"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
				var weight = 10;
				var entityName = @"smtp_service";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: AllServiceEmaills >>
			{
				var id = new Guid("f33f961c-8b72-4767-a77b-df8900041754");
				var name = @"AllServiceEmaills";
				var description = @"records of all emails for specified service";
				var eqlText = @"SELECT * FROM email
WHERE service_id = @service_id AND x_search CONTAINS @searchQuery
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_email.""id"" AS ""id"",
	 rec_email.""from_name"" AS ""from_name"",
	 rec_email.""to_name"" AS ""to_name"",
	 rec_email.""subject"" AS ""subject"",
	 rec_email.""content_text"" AS ""content_text"",
	 rec_email.""content_html"" AS ""content_html"",
	 rec_email.""from_email"" AS ""from_email"",
	 rec_email.""to_email"" AS ""to_email"",
	 rec_email.""sent_on"" AS ""sent_on"",
	 rec_email.""created_on"" AS ""created_on"",
	 rec_email.""server_error"" AS ""server_error"",
	 rec_email.""retries_count"" AS ""retries_count"",
	 rec_email.""service_id"" AS ""service_id"",
	 rec_email.""priority"" AS ""priority"",
	 rec_email.""reply_to_email"" AS ""reply_to_email"",
	 rec_email.""scheduled_on"" AS ""scheduled_on"",
	 rec_email.""status"" AS ""status"",
	 rec_email.""x_search"" AS ""x_search"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_email
WHERE  (  ( rec_email.""service_id"" = @service_id )  AND  ( rec_email.""x_search""  ILIKE  @searchQuery )  ) 
ORDER BY rec_email.""created_on"" DESC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""service_id"",""type"":""guid"",""value"":""guid.empty""},{""name"":""sortBy"",""type"":""text"",""value"":""created_on""},{""name"":""sortOrder"",""type"":""text"",""value"":""desc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""from_name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""to_name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""content_text"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""content_html"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""from_email"",""type"":6,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""to_email"",""type"":6,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""sent_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""server_error"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""retries_count"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""service_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reply_to_email"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""scheduled_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
				var weight = 10;
				var entityName = @"email";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create data source*** Name: AllEmails >>
			{
				var id = new Guid("82f0b63e-3647-4106-839c-4d5adca4f3b1");
				var name = @"AllEmails";
				var description = @"records of all emails";
				var eqlText = @"SELECT * FROM email
WHERE x_search CONTAINS @searchQuery
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
				var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_email.""id"" AS ""id"",
	 rec_email.""from_name"" AS ""from_name"",
	 rec_email.""to_name"" AS ""to_name"",
	 rec_email.""subject"" AS ""subject"",
	 rec_email.""content_text"" AS ""content_text"",
	 rec_email.""content_html"" AS ""content_html"",
	 rec_email.""from_email"" AS ""from_email"",
	 rec_email.""to_email"" AS ""to_email"",
	 rec_email.""sent_on"" AS ""sent_on"",
	 rec_email.""created_on"" AS ""created_on"",
	 rec_email.""server_error"" AS ""server_error"",
	 rec_email.""retries_count"" AS ""retries_count"",
	 rec_email.""service_id"" AS ""service_id"",
	 rec_email.""priority"" AS ""priority"",
	 rec_email.""reply_to_email"" AS ""reply_to_email"",
	 rec_email.""scheduled_on"" AS ""scheduled_on"",
	 rec_email.""status"" AS ""status"",
	 rec_email.""x_search"" AS ""x_search"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_email
WHERE  ( rec_email.""x_search""  ILIKE  @searchQuery ) 
ORDER BY rec_email.""created_on"" DESC
LIMIT 15
OFFSET 0
) X
";
				var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""created_on""},{""name"":""sortOrder"",""type"":""text"",""value"":""desc""},{""name"":""page"",""type"":""int"",""value"":""1""},{""name"":""pageSize"",""type"":""int"",""value"":""15""},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty""}]";
				var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""from_name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""to_name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""content_text"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""content_html"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""from_email"",""type"":6,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""to_email"",""type"":6,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""sent_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""server_error"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""retries_count"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""service_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reply_to_email"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""scheduled_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
				var weight = 10;
				var entityName = @"email";

				new WebVella.Erp.Database.DbDataSourceRepository().Create(id, name, description, weight, eqlText, sqlText, parametersJson, fieldsJson, entityName);
			}
			#endregion

			#region << ***Create page data source*** Name: AllSmtpSevices >>
			{
				var id = new Guid("6694ed3d-7a6a-45ef-a592-968032087245");
				var pageId = new Guid("31c40750-99c7-4402-9e9b-8157e9459df7");
				var dataSourceId = new Guid("e37f0e27-1506-44aa-9196-18a9f43e425c");
				var name = @"AllSmtpSevices";
				var parameters = @"[]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

			#region << ***Create page data source*** Name: AllEmails >>
			{
				var id = new Guid("ba833220-5327-4f2d-9354-5a04b706c35e");
				var pageId = new Guid("3374a8ee-653b-43f6-a4e8-c6db9a4f76d2");
				var dataSourceId = new Guid("82f0b63e-3647-4106-839c-4d5adca4f3b1");
				var name = @"AllEmails";
				var parameters = @"[{""name"":""sortBy"",""type"":""text"",""value"":""{{RequestQuery.sortBy}}""},{""name"":""sortOrder"",""type"":""text"",""value"":""{{RequestQuery.sortOrder}}""},{""name"":""page"",""type"":""int"",""value"":""{{RequestQuery.page}}""},{""name"":""searchQuery"",""type"":""text"",""value"":""{{RequestQuery.q_x_search_v}}""}]";

				new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).CreatePageDataSource(id, pageId, dataSourceId, name, parameters, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion


		}
	}
}
