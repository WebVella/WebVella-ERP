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
		private static void Patch20190204(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{

			#region << ***Create field***  Entity: account Field Name: type >>
			{
				InputSelectField dropdownField = new InputSelectField();
				dropdownField.Id = new Guid("7cab7793-1ae4-4c05-9191-4035a0d54bd1");
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
				dropdownField.DefaultValue = "1";
				dropdownField.Options = new List<SelectOption>
	{
		new SelectOption() { Label = "Company", Value = "1", IconClass = "", Color = ""},
		new SelectOption() { Label = "Person", Value = "2", IconClass = "", Color = ""}
	};
				dropdownField.EnableSecurity = false;
				dropdownField.Permissions = new FieldPermissions();
				dropdownField.Permissions.CanRead = new List<Guid>();
				dropdownField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), dropdownField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: type Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: website >>
			{
				InputUrlField urlField = new InputUrlField();
				urlField.Id = new Guid("df7114b5-49ad-400b-ae16-a6ed1daa8a0c");
				urlField.Name = "website";
				urlField.Label = "Website";
				urlField.PlaceholderText = null;
				urlField.Description = null;
				urlField.HelpText = null;
				urlField.Required = false;
				urlField.Unique = false;
				urlField.Searchable = false;
				urlField.Auditable = false;
				urlField.System = true;
				urlField.DefaultValue = null;
				urlField.MaxLength = null;
				urlField.OpenTargetInNewWindow = false;
				urlField.EnableSecurity = false;
				urlField.Permissions = new FieldPermissions();
				urlField.Permissions.CanRead = new List<Guid>();
				urlField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), urlField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: website Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: street >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("1bc1ead8-2673-4cdd-b0f3-b99d4cf4fadc");
				textboxField.Name = "street";
				textboxField.Label = "Street";
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
						throw new Exception("System error 10060. Entity: account Field: street Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: region >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("9c29b56d-2db2-47c6-bcf6-96cbe7187119");
				textboxField.Name = "region";
				textboxField.Label = "Region";
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
						throw new Exception("System error 10060. Entity: account Field: region Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: post_code >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("caaaf464-67b7-47b2-afec-beec03d90e4f");
				textboxField.Name = "post_code";
				textboxField.Label = "Post code";
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
						throw new Exception("System error 10060. Entity: account Field: post_code Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: fixed_phone >>
			{
				InputPhoneField phoneField = new InputPhoneField();
				phoneField.Id = new Guid("f51f7451-b9f1-4a5a-a282-3d83525a9094");
				phoneField.Name = "fixed_phone";
				phoneField.Label = "Fixed Phone";
				phoneField.PlaceholderText = null;
				phoneField.Description = null;
				phoneField.HelpText = null;
				phoneField.Required = false;
				phoneField.Unique = false;
				phoneField.Searchable = false;
				phoneField.Auditable = false;
				phoneField.System = true;
				phoneField.DefaultValue = null;
				phoneField.MaxLength = null;
				phoneField.Format = null;
				phoneField.EnableSecurity = false;
				phoneField.Permissions = new FieldPermissions();
				phoneField.Permissions.CanRead = new List<Guid>();
				phoneField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), phoneField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: fixed_phone Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: mobile_phone >>
			{
				InputPhoneField phoneField = new InputPhoneField();
				phoneField.Id = new Guid("01e8d8e6-457b-49c8-9194-81f06bd9f8ed");
				phoneField.Name = "mobile_phone";
				phoneField.Label = "Mobile phone";
				phoneField.PlaceholderText = null;
				phoneField.Description = null;
				phoneField.HelpText = null;
				phoneField.Required = false;
				phoneField.Unique = false;
				phoneField.Searchable = false;
				phoneField.Auditable = false;
				phoneField.System = true;
				phoneField.DefaultValue = null;
				phoneField.MaxLength = null;
				phoneField.Format = null;
				phoneField.EnableSecurity = false;
				phoneField.Permissions = new FieldPermissions();
				phoneField.Permissions.CanRead = new List<Guid>();
				phoneField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), phoneField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: mobile_phone Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: fax_phone >>
			{
				InputPhoneField phoneField = new InputPhoneField();
				phoneField.Id = new Guid("8f6bbfac-8f10-4023-b2b0-af03d22b9cef");
				phoneField.Name = "fax_phone";
				phoneField.Label = "Fax";
				phoneField.PlaceholderText = null;
				phoneField.Description = null;
				phoneField.HelpText = null;
				phoneField.Required = false;
				phoneField.Unique = false;
				phoneField.Searchable = false;
				phoneField.Auditable = false;
				phoneField.System = true;
				phoneField.DefaultValue = null;
				phoneField.MaxLength = null;
				phoneField.Format = null;
				phoneField.EnableSecurity = false;
				phoneField.Permissions = new FieldPermissions();
				phoneField.Permissions.CanRead = new List<Guid>();
				phoneField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), phoneField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: fax_phone Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: notes >>
			{
				InputMultiLineTextField textareaField = new InputMultiLineTextField();
				textareaField.Id = new Guid("d2c7a984-c173-434f-a711-1f1efa07f0c1");
				textareaField.Name = "notes";
				textareaField.Label = "Notes";
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
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), textareaField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: notes Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: last_name >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("c9da8e17-9511-4f2c-8576-8756f34a17b9");
				textboxField.Name = "last_name";
				textboxField.Label = "Last name";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "last name";
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
						throw new Exception("System error 10060. Entity: account Field: last_name Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: first_name >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("66de2df4-f42a-4bc9-817d-8960578a8302");
				textboxField.Name = "first_name";
				textboxField.Label = "First name";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "first name";
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
						throw new Exception("System error 10060. Entity: account Field: first_name Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: x_search >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("d8ce135d-f6c4-45b7-a543-c58e154c06df");
				textboxField.Name = "x_search";
				textboxField.Label = "Search";
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
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: x_search Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: email >>
			{
				InputEmailField emailField = new InputEmailField();
				emailField.Id = new Guid("25dcf767-2e12-4413-b096-60d37700194f");
				emailField.Name = "email";
				emailField.Label = "Email";
				emailField.PlaceholderText = null;
				emailField.Description = null;
				emailField.HelpText = null;
				emailField.Required = false;
				emailField.Unique = false;
				emailField.Searchable = true;
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
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), emailField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: email Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: city >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("4e18d041-0daf-4db4-9bd9-6d5b631af0bd");
				textboxField.Name = "city";
				textboxField.Label = "City";
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
						throw new Exception("System error 10060. Entity: account Field: city Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: country_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("76c1d754-8bf5-4a78-a2d7-bf771e1b032b");
				guidField.Name = "country_id";
				guidField.Label = "Country";
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
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: country_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: tax_id >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("c4bbc47c-2dc0-4c24-9159-1b5a6bfa8ed3");
				textboxField.Name = "tax_id";
				textboxField.Label = "Tax ID";
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
						throw new Exception("System error 10060. Entity: account Field: tax_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: solutation_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("4ace48d2-ece0-43a5-a04f-5a8e080c7428");
				guidField.Name = "solutation_id";
				guidField.Label = "Solutation";
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
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: solutation_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: street_2 >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("8829ff72-2910-40a8-834d-5f05c51c8d2f");
				textboxField.Name = "street_2";
				textboxField.Label = "Street 2";
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
						throw new Exception("System error 10060. Entity: account Field: street_2 Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: language_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("02b796b4-2b7a-4662-8a16-01dbffdd1ba1");
				guidField.Name = "language_id";
				guidField.Label = "Language";
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
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: language_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: currency_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("c2a2a490-951d-4395-b359-0dc88ad56c11");
				guidField.Name = "currency_id";
				guidField.Label = "Currency";
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
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: currency_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: language >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("a9d26ddc-48d1-4403-922a-4459ced5d45d");
					entity.Id = new Guid("f22c806a-6495-4f12-be79-ce2105466baf");
					entity.Name = "language";
					entity.Label = "Language";
					entity.LabelPlural = "Languages";
					entity.System = true;
					entity.IconName = "far fa-dot-circle";
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
					entity.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: language creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: language Field Name: color >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("74d575c0-e431-4e0f-8a1b-462419c3ea90");
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
					var response = entMan.CreateField(new Guid("f22c806a-6495-4f12-be79-ce2105466baf"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: language Field: color Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: language Field Name: icon_class >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("84717a0e-b418-492c-a7ea-8ec6cf3c90fe");
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
					var response = entMan.CreateField(new Guid("f22c806a-6495-4f12-be79-ce2105466baf"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: language Field: icon_class Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: language Field Name: is_default >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("bae39ef3-38fe-4f06-a2e9-d7a78501a417");
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
					var response = entMan.CreateField(new Guid("f22c806a-6495-4f12-be79-ce2105466baf"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: language Field: is_default Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: language Field Name: is_enabled >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("38285d7a-b78a-42ad-80fd-868663eb4d67");
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
					var response = entMan.CreateField(new Guid("f22c806a-6495-4f12-be79-ce2105466baf"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: language Field: is_enabled Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: language Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("1a36c109-e856-4f03-b9c3-d853d9f74ad2");
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
					var response = entMan.CreateField(new Guid("f22c806a-6495-4f12-be79-ce2105466baf"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: language Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: language Field Name: label >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("344b56b3-676b-4f65-a25f-88b50b484fa7");
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
					var response = entMan.CreateField(new Guid("f22c806a-6495-4f12-be79-ce2105466baf"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: language Field: label Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: language Field Name: sort_index >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("751f56e3-6efa-49ec-8e19-8873cf83cc74");
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
					var response = entMan.CreateField(new Guid("f22c806a-6495-4f12-be79-ce2105466baf"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: language Field: sort_index Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: currency >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("8d3b3b25-76dd-458f-98cc-0fb5d4e8c13f");
					entity.Id = new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721");
					entity.Name = "currency";
					entity.Label = "Currency";
					entity.LabelPlural = "Currencies";
					entity.System = true;
					entity.IconName = "far fa-dot-circle";
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
					entity.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: currency creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: color >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("1db90eea-aba8-426e-b53f-81097f45ab53");
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
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: color Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: icon_class >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("26b4075b-4295-45be-be42-de9670e52a95");
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
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: icon_class Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: is_default >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("a9305867-2129-445f-ba9a-1988fd55031b");
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
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: is_default Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: is_enabled >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("6b6d7f4e-8f69-497b-8182-1ac701828007");
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
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: is_enabled Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: is_system >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("37b86cbe-74eb-4d40-811e-5947951eb6da");
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
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: is_system Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("8f664f83-d6f9-4e19-8399-0298267176cd");
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
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: label >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("d758cd23-4fa6-434e-8269-13ca2f0ab018");
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
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: label Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: sort_index >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("855b3003-bca9-41f7-b646-51f221fc02d6");
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
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: sort_index Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: code >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("50f2fbbc-fbe8-4337-ad0f-2f897e93b317");
				textboxField.Name = "code";
				textboxField.Label = "Code";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "USD";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: code Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: decimal_digits >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("2c28fd9d-6e1b-4985-b741-698a5e2776f6");
				numberField.Name = "decimal_digits";
				numberField.Label = "Decimal digits";
				numberField.PlaceholderText = null;
				numberField.Description = null;
				numberField.HelpText = null;
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = true;
				numberField.DefaultValue = Decimal.Parse("2.0");
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
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: decimal_digits Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: exchange_rate >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("3d468cb1-19c3-4335-9ba6-cc0d20861141");
				numberField.Name = "exchange_rate";
				numberField.Label = "Exchange rate";
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
				numberField.DecimalPlaces = byte.Parse("2");
				numberField.EnableSecurity = false;
				numberField.Permissions = new FieldPermissions();
				numberField.Permissions.CanRead = new List<Guid>();
				numberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: exchange_rate Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: exchange_rate_date >>
			{
				InputDateField dateField = new InputDateField();
				dateField.Id = new Guid("22d48e15-dfe2-4135-ba38-b92238e6a228");
				dateField.Name = "exchange_rate_date";
				dateField.Label = "Exchange rate date";
				dateField.PlaceholderText = null;
				dateField.Description = null;
				dateField.HelpText = null;
				dateField.Required = true;
				dateField.Unique = false;
				dateField.Searchable = false;
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
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), dateField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: exchange_rate_date Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: label_plural >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("b7026734-8dc1-43d7-8f27-a385500094b3");
				textboxField.Name = "label_plural";
				textboxField.Label = "Label Plural";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "labels";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: label_plural Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: symbol >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("e69ea134-5b76-42b4-824d-d11dfe33e1da");
				textboxField.Name = "symbol";
				textboxField.Label = "Symbol";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "$";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: symbol Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: symbol_native >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("9c2a22a9-142b-43fc-b00c-83635c5976a9");
				textboxField.Name = "symbol_native";
				textboxField.Label = "Symbol native";
				textboxField.PlaceholderText = null;
				textboxField.Description = null;
				textboxField.HelpText = null;
				textboxField.Required = true;
				textboxField.Unique = false;
				textboxField.Searchable = false;
				textboxField.Auditable = false;
				textboxField.System = true;
				textboxField.DefaultValue = "$";
				textboxField.MaxLength = null;
				textboxField.EnableSecurity = false;
				textboxField.Permissions = new FieldPermissions();
				textboxField.Permissions.CanRead = new List<Guid>();
				textboxField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: symbol_native Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: currency Field Name: symbol_placement >>
			{
				InputSelectField dropdownField = new InputSelectField();
				dropdownField.Id = new Guid("484bdd0d-dcba-4dc9-991e-79b3c0d62711");
				dropdownField.Name = "symbol_placement";
				dropdownField.Label = "Symbol placement";
				dropdownField.PlaceholderText = null;
				dropdownField.Description = null;
				dropdownField.HelpText = null;
				dropdownField.Required = true;
				dropdownField.Unique = false;
				dropdownField.Searchable = false;
				dropdownField.Auditable = false;
				dropdownField.System = true;
				dropdownField.DefaultValue = "2";
				dropdownField.Options = new List<SelectOption>
	{
		new SelectOption() { Label = "before", Value = "1", IconClass = "", Color = ""},
		new SelectOption() { Label = "after", Value = "2", IconClass = "", Color = ""}
	};
				dropdownField.EnableSecurity = false;
				dropdownField.Permissions = new FieldPermissions();
				dropdownField.Permissions.CanRead = new List<Guid>();
				dropdownField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), dropdownField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: symbol_placement Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: contact >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("859f24ec-4d3e-4597-9972-1d5a9cba918b");
					entity.Id = new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0");
					entity.Name = "contact";
					entity.Label = "Contact";
					entity.LabelPlural = "Contacts";
					entity.System = true;
					entity.IconName = "fa fa-user";
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
							throw new Exception("System error 10050. Entity: contact creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: email >>
			{
				InputEmailField emailField = new InputEmailField();
				emailField.Id = new Guid("ca400904-1334-48fe-884c-223df1d08545");
				emailField.Name = "email";
				emailField.Label = "Email";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), emailField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: email Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: job_title >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("ddcc1807-6651-411d-9eed-668ee34d0c1b");
				textboxField.Name = "job_title";
				textboxField.Label = "Job title";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: job_title Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: first_name >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("6670c70c-c46e-4912-a70f-b1ad20816415");
				textboxField.Name = "first_name";
				textboxField.Label = "First name";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: first_name Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: last_name >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("4f711d55-11a7-464a-a4c3-3b3047c6c014");
				textboxField.Name = "last_name";
				textboxField.Label = "Last name";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: last_name Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: notes >>
			{
				InputMultiLineTextField textareaField = new InputMultiLineTextField();
				textareaField.Id = new Guid("9912ff90-bc26-4879-9615-c5963a42fe22");
				textareaField.Name = "notes";
				textareaField.Label = "Notes";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), textareaField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: notes Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: fixed_phone >>
			{
				InputPhoneField phoneField = new InputPhoneField();
				phoneField.Id = new Guid("0f947ba0-ccac-40c4-9d31-5e5f5be953ce");
				phoneField.Name = "fixed_phone";
				phoneField.Label = "Fixed phone";
				phoneField.PlaceholderText = null;
				phoneField.Description = null;
				phoneField.HelpText = null;
				phoneField.Required = false;
				phoneField.Unique = false;
				phoneField.Searchable = false;
				phoneField.Auditable = false;
				phoneField.System = true;
				phoneField.DefaultValue = null;
				phoneField.MaxLength = null;
				phoneField.Format = null;
				phoneField.EnableSecurity = false;
				phoneField.Permissions = new FieldPermissions();
				phoneField.Permissions.CanRead = new List<Guid>();
				phoneField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), phoneField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: fixed_phone Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: mobile_phone >>
			{
				InputPhoneField phoneField = new InputPhoneField();
				phoneField.Id = new Guid("519bd797-1dc7-4aef-b1ed-f27442f855ef");
				phoneField.Name = "mobile_phone";
				phoneField.Label = "Mobile phone";
				phoneField.PlaceholderText = null;
				phoneField.Description = null;
				phoneField.HelpText = null;
				phoneField.Required = false;
				phoneField.Unique = false;
				phoneField.Searchable = false;
				phoneField.Auditable = false;
				phoneField.System = true;
				phoneField.DefaultValue = null;
				phoneField.MaxLength = null;
				phoneField.Format = null;
				phoneField.EnableSecurity = false;
				phoneField.Permissions = new FieldPermissions();
				phoneField.Permissions.CanRead = new List<Guid>();
				phoneField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), phoneField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: mobile_phone Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: fax_phone >>
			{
				InputPhoneField phoneField = new InputPhoneField();
				phoneField.Id = new Guid("0475b344-8f8e-464c-a182-9c2beae105f3");
				phoneField.Name = "fax_phone";
				phoneField.Label = "Fax phone";
				phoneField.PlaceholderText = null;
				phoneField.Description = null;
				phoneField.HelpText = null;
				phoneField.Required = false;
				phoneField.Unique = false;
				phoneField.Searchable = false;
				phoneField.Auditable = false;
				phoneField.System = true;
				phoneField.DefaultValue = null;
				phoneField.MaxLength = null;
				phoneField.Format = null;
				phoneField.EnableSecurity = false;
				phoneField.Permissions = new FieldPermissions();
				phoneField.Permissions.CanRead = new List<Guid>();
				phoneField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), phoneField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: fax_phone Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: solutation_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("66b49907-2c0f-4914-a71c-1a9ccba1c704");
				guidField.Name = "solutation_id";
				guidField.Label = "Solutation";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: solutation_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: city >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("acc25b72-6e17-437f-bfaf-f514b0a7406f");
				textboxField.Name = "city";
				textboxField.Label = "City";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: city Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: country_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("08a67742-21ef-4ecb-8872-54ac18b50bdc");
				guidField.Name = "country_id";
				guidField.Label = "Country";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: country_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: region >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("f5cab626-c215-4922-be4f-8931d0cf0b66");
				textboxField.Name = "region";
				textboxField.Label = "Region";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: region Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: street >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("1147a14a-d9ae-4c88-8441-80f668676b1c");
				textboxField.Name = "street";
				textboxField.Label = "Street";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: street Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: street_2 >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("2b1532c0-528c-4dfb-b40a-3d75ef1491fc");
				textboxField.Name = "street_2";
				textboxField.Label = "Street 2";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: street_2 Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: post_code >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("c3433c76-dee9-4dce-94a0-ea5f03527ee6");
				textboxField.Name = "post_code";
				textboxField.Label = "Post code";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: post_code Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: address >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("158c33cc-f7b2-4b0a-aeb6-ce5e908f6c5d");
					entity.Id = new Guid("34a126ba-1dee-4099-a1c1-a24e70eb10f0");
					entity.Name = "address";
					entity.Label = "Address";
					entity.LabelPlural = "Addresses";
					entity.System = true;
					entity.IconName = "fas fa-building";
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
							throw new Exception("System error 10050. Entity: address creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: address Field Name: street >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("79e7a689-6407-4a03-8580-5bdb20e2337d");
				textboxField.Name = "street";
				textboxField.Label = "Street";
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
					var response = entMan.CreateField(new Guid("34a126ba-1dee-4099-a1c1-a24e70eb10f0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: address Field: street Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: address Field Name: street_2 >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("3aeb73d9-8879-4f25-93e9-0b22944a5bba");
				textboxField.Name = "street_2";
				textboxField.Label = "Street 2";
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
					var response = entMan.CreateField(new Guid("34a126ba-1dee-4099-a1c1-a24e70eb10f0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: address Field: street_2 Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: address Field Name: city >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("6b8150d5-ea81-4a74-b35a-b6c888665fe5");
				textboxField.Name = "city";
				textboxField.Label = "City";
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
					var response = entMan.CreateField(new Guid("34a126ba-1dee-4099-a1c1-a24e70eb10f0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: address Field: city Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: address Field Name: region >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("6225169e-fcde-4c66-9066-d08bbe9a7b1b");
				textboxField.Name = "region";
				textboxField.Label = "Region";
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
					var response = entMan.CreateField(new Guid("34a126ba-1dee-4099-a1c1-a24e70eb10f0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: address Field: region Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: address Field Name: country_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("c40192ea-c81c-4140-9c7b-6134184f942c");
				guidField.Name = "country_id";
				guidField.Label = "Country";
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
					var response = entMan.CreateField(new Guid("34a126ba-1dee-4099-a1c1-a24e70eb10f0"), guidField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: address Field: country_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: address Field Name: notes >>
			{
				InputMultiLineTextField textareaField = new InputMultiLineTextField();
				textareaField.Id = new Guid("a977b2af-78ea-4df0-97dc-652d82cee2df");
				textareaField.Name = "notes";
				textareaField.Label = "Notes";
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
					var response = entMan.CreateField(new Guid("34a126ba-1dee-4099-a1c1-a24e70eb10f0"), textareaField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: address Field: notes Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: address Field Name: name >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("487d6795-6cec-4598-bbeb-094bcbeadcf6");
				textboxField.Name = "name";
				textboxField.Label = "Name";
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
					var response = entMan.CreateField(new Guid("34a126ba-1dee-4099-a1c1-a24e70eb10f0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: address Field: name Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update entity*** Entity name: task >>
			{
				var updateObject = new InputEntity();
				updateObject.Id = new Guid("9386226e-381e-4522-b27b-fb5514d77902");
				updateObject.Name = "task";
				updateObject.Label = "Task";
				updateObject.LabelPlural = "Tasks";
				updateObject.System = true;
				updateObject.IconName = "fas fa-user-cog";
				updateObject.Color = "#f44336";
				updateObject.RecordScreenIdField = null;
				updateObject.RecordPermissions = new RecordPermissions();
				updateObject.RecordPermissions.CanRead = new List<Guid>();
				updateObject.RecordPermissions.CanCreate = new List<Guid>();
				updateObject.RecordPermissions.CanUpdate = new List<Guid>();
				updateObject.RecordPermissions.CanDelete = new List<Guid>();
				updateObject.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
				updateObject.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				updateObject.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				updateObject.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				var updateEntityResult = entMan.UpdateEntity(updateObject);
				if (!updateEntityResult.Success)
				{
					throw new Exception("System error 10060. Entity update with name : task. Message:" + updateEntityResult.Message);
				}
			}
			#endregion

			#region << ***Update entity*** Entity name: project >>
			{
				var updateObject = new InputEntity();
				updateObject.Id = new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792");
				updateObject.Name = "project";
				updateObject.Label = "Project";
				updateObject.LabelPlural = "Projects";
				updateObject.System = true;
				updateObject.IconName = "fas fa-cogs";
				updateObject.Color = "#f44336";
				updateObject.RecordScreenIdField = null;
				updateObject.RecordPermissions = new RecordPermissions();
				updateObject.RecordPermissions.CanRead = new List<Guid>();
				updateObject.RecordPermissions.CanCreate = new List<Guid>();
				updateObject.RecordPermissions.CanUpdate = new List<Guid>();
				updateObject.RecordPermissions.CanDelete = new List<Guid>();
				updateObject.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
				updateObject.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				updateObject.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				updateObject.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				var updateEntityResult = entMan.UpdateEntity(updateObject);
				if (!updateEntityResult.Success)
				{
					throw new Exception("System error 10060. Entity update with name : project. Message:" + updateEntityResult.Message);
				}
			}
			#endregion

			#region << ***Delete relation*** Relation name: case_status_1n_case >>
			{
				{
					var response = relMan.Delete(new Guid("c523c594-1f84-495e-84f3-a569cb384586"));
					if (!response.Success)
						throw new Exception("System error 10060. Relation: case_status_1n_case Delete. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Delete relation*** Relation name: account_1n_case >>
			{
				{
					var response = relMan.Delete(new Guid("06d07760-41ba-408c-af61-a1fdc8493de3"));
					if (!response.Success)
						throw new Exception("System error 10060. Relation: account_1n_case Delete. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Delete relation*** Relation name: case_type_1n_case >>
			{
				{
					var response = relMan.Delete(new Guid("c4a6918b-7918-4806-83cb-fd3d87fe5a10"));
					if (!response.Success)
						throw new Exception("System error 10060. Relation: case_type_1n_case Delete. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: account_nn_contact >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("2e22b50f-e444-4b62-a171-076e51246939")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
				relation.Id = new Guid("dd211c99-5415-4195-923a-cb5a56e5d544");
				relation.Name = "account_nn_contact";
				relation.Label = "account_nn_contact";
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
						throw new Exception("System error 10060. Relation: account_nn_contact Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: case_status_1n_case >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "status_id");
				relation.Id = new Guid("4713f0fc-0154-4ce0-b804-0e92ed50bdec");
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

			#region << ***Create relation*** Relation name: currency_1n_account >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("2e22b50f-e444-4b62-a171-076e51246939")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "currency_id");
				relation.Id = new Guid("5e5c17df-2f50-4f88-82f1-d76cb7cd6156");
				relation.Name = "currency_1n_account";
				relation.Label = "currency_1n_account";
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
						throw new Exception("System error 10060. Relation: currency_1n_account Create. Message:" + response.Message);
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
				relation.Id = new Guid("fbd6df76-345e-427a-a4af-fae553bc02c0");
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

			#region << ***Create relation*** Relation name: account_nn_case >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("2e22b50f-e444-4b62-a171-076e51246939")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
				relation.Id = new Guid("3690c12e-40e1-4e8f-a0a8-27221c686b43");
				relation.Name = "account_nn_case";
				relation.Label = "account_nn_case";
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
						throw new Exception("System error 10060. Relation: account_nn_case Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: solutation_1n_contact >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("f0b64034-e0f6-452e-b82b-88186af6df88")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
				relation.Id = new Guid("54a6e20a-9e94-45fb-b77c-e2bb35cb20fc");
				relation.Name = "solutation_1n_contact";
				relation.Label = "solutation_1n_contact";
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
						throw new Exception("System error 10060. Relation: solutation_1n_contact Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: address_nn_account >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("34a126ba-1dee-4099-a1c1-a24e70eb10f0")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("2e22b50f-e444-4b62-a171-076e51246939")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
				relation.Id = new Guid("dcf76eb5-16cf-466d-b760-c0d8ae57da94");
				relation.Name = "address_nn_account";
				relation.Label = "address_nn_account";
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
						throw new Exception("System error 10060. Relation: address_nn_account Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: country_1n_address >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("34a126ba-1dee-4099-a1c1-a24e70eb10f0")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "country_id");
				relation.Id = new Guid("f04f74bc-5525-4685-a72a-5b49e4b0f273");
				relation.Name = "country_1n_address";
				relation.Label = "country_1n_address";
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
						throw new Exception("System error 10060. Relation: country_1n_address Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: country_1n_account >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("2e22b50f-e444-4b62-a171-076e51246939")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "country_id");
				relation.Id = new Guid("66661380-49f8-4a50-b0d9-4d2a8d2f0990");
				relation.Name = "country_1n_account";
				relation.Label = "country_1n_account";
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
						throw new Exception("System error 10060. Relation: country_1n_account Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: language_1n_account >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("f22c806a-6495-4f12-be79-ce2105466baf")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("2e22b50f-e444-4b62-a171-076e51246939")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "language_id");
				relation.Id = new Guid("6e7f99d8-712c-451d-80fc-3a5fba4580f4");
				relation.Name = "language_1n_account";
				relation.Label = "language_1n_account";
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
						throw new Exception("System error 10060. Relation: language_1n_account Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: solutation_1n_account >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("f0b64034-e0f6-452e-b82b-88186af6df88")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("2e22b50f-e444-4b62-a171-076e51246939")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "solutation_id");
				relation.Id = new Guid("66f62cd6-174c-4a0b-b56f-6356b24bd73d");
				relation.Name = "solutation_1n_account";
				relation.Label = "solutation_1n_account";
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
						throw new Exception("System error 10060. Relation: solutation_1n_account Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: country_1n_contact >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "country_id");
				relation.Id = new Guid("dc4ece26-fff7-440a-9e19-76189507b5b9");
				relation.Name = "country_1n_contact";
				relation.Label = "country_1n_contact";
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
						throw new Exception("System error 10060. Relation: country_1n_contact Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create record*** Id: f2d52490-9ddf-496c-a923-d6e374fc6eab (currency) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""f2d52490-9ddf-496c-a923-d6e374fc6eab"",
  ""color"": """",
  ""icon_class"": """",
  ""is_default"": true,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""American dollar"",
  ""sort_index"": 1.0,
  ""code"": ""USD"",
  ""decimal_digits"": 2.0,
  ""exchange_rate"": 1.0,
  ""exchange_rate_date"": ""2019-02-04T00:00:00.000"",
  ""label_plural"": ""American dollars"",
  ""symbol"": ""$"",
  ""symbol_native"": ""$"",
  ""symbol_placement"": ""1""
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("currency", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 09262a1f-a3b1-4f44-84aa-f1ade53fb14c (currency) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""09262a1f-a3b1-4f44-84aa-f1ade53fb14c"",
  ""color"": """",
  ""icon_class"": """",
  ""is_default"": true,
  ""is_enabled"": true,
  ""is_system"": true,
  ""l_scope"": """",
  ""label"": ""Euro"",
  ""sort_index"": 2.0,
  ""code"": ""EUR"",
  ""decimal_digits"": 2.0,
  ""exchange_rate"": 0.87,
  ""exchange_rate_date"": ""2019-02-04T00:00:00.000"",
  ""label_plural"": ""Euros"",
  ""symbol"": ""€"",
  ""symbol_native"": ""€"",
  ""symbol_placement"": ""1""
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("currency", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 1773d68f-76f4-41b8-89d0-c33c2c187ddd (language) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""1773d68f-76f4-41b8-89d0-c33c2c187ddd"",
  ""color"": """",
  ""icon_class"": """",
  ""is_default"": true,
  ""is_enabled"": true,
  ""l_scope"": """",
  ""label"": ""English"",
  ""sort_index"": 1.0
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("language", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion



		}
	}
}
