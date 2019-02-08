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
		private static void Patch20190206(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Delete relation*** Relation name: solutation_1n_contact >>
			{
				{
					var response = relMan.Delete(new Guid("54a6e20a-9e94-45fb-b77c-e2bb35cb20fc"));
					if (!response.Success)
						throw new Exception("System error 10060. Relation: solutation_1n_contact Delete. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Delete relation*** Relation name: solutation_1n_account >>
			{
				{
					var response = relMan.Delete(new Guid("66f62cd6-174c-4a0b-b56f-6356b24bd73d"));
					if (!response.Success)
						throw new Exception("System error 10060. Relation: solutation_1n_account Delete. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Delete field*** Entity: account Field Name: solutation_id >>
			{
				{
					var response = entMan.DeleteField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), new Guid("4ace48d2-ece0-43a5-a04f-5a8e080c7428"));
					if (!response.Success)
						throw new Exception("System error 10060. Delete field failed for Field: solutation_id Entity: account. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Delete field*** Entity: contact Field Name: solutation_id >>
			{
				{
					var response = entMan.DeleteField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), new Guid("66b49907-2c0f-4914-a71c-1a9ccba1c704"));
					if (!response.Success)
						throw new Exception("System error 10060. Delete field failed for Field: solutation_id Entity: contact. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: language Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("f22c806a-6495-4f12-be79-ce2105466baf")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("f22c806a-6495-4f12-be79-ce2105466baf"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: language Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: created_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("48a33ffe-d5e4-4fa1-b74c-272733201652");
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
					var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: created_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: account Field Name: salutation_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("dce30f5b-7c87-450e-a60a-757f758d9f62");
				guidField.Name = "salutation_id";
				guidField.Label = "Salutation";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("87c08ee1-8d4d-4c89-9b37-4e3cc3f98698");
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
						throw new Exception("System error 10060. Entity: account Field: salutation_id Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: account Field Name: x_search >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("2e22b50f-e444-4b62-a171-076e51246939")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "x_search").Id;
				textboxField.Name = "x_search";
				textboxField.Label = "Search Index";
				textboxField.PlaceholderText = "search accounts";
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
					var response = entMan.UpdateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: x_search Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: account Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("2e22b50f-e444-4b62-a171-076e51246939")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: account Field: l_scope Message:" + response.Message);
				}
			}
			#endregion



			#region << ***Update field***  Entity: attachment Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("4b56686e-971e-4b8e-8356-642a8f341bff")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("4b56686e-971e-4b8e-8356-642a8f341bff"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: attachment Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: currency Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("4d049df9-10eb-48a3-91b8-ee4106df9721"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: currency Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: timelog Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("750153c5-1df9-408f-b856-727078a525bc")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("750153c5-1df9-408f-b856-727078a525bc"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: timelog Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: timelog Field Name: l_related_records >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("750153c5-1df9-408f-b856-727078a525bc")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_related_records").Id;
				textboxField.Name = "l_related_records";
				textboxField.Label = "Related records";
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
					var response = entMan.UpdateField(new Guid("750153c5-1df9-408f-b856-727078a525bc"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: timelog Field: l_related_records Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: comment Field Name: l_related_records >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_related_records").Id;
				textboxField.Name = "l_related_records";
				textboxField.Label = "Related Record lookup";
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
					var response = entMan.UpdateField(new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: comment Field: l_related_records Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: comment Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("b1d218d5-68c2-41a5-bea5-1b4a78cbf91d"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: comment Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update entity*** Entity name: contact >>
			{
				var updateObject = new InputEntity();
				updateObject.Id = new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0");
				updateObject.Name = "contact";
				updateObject.Label = "Contact";
				updateObject.LabelPlural = "Contacts";
				updateObject.System = true;
				updateObject.IconName = "far fa-address-card";
				updateObject.Color = "#f44336";
				updateObject.RecordScreenIdField = null;
				updateObject.RecordPermissions = new RecordPermissions();
				updateObject.RecordPermissions.CanRead = new List<Guid>();
				updateObject.RecordPermissions.CanCreate = new List<Guid>();
				updateObject.RecordPermissions.CanUpdate = new List<Guid>();
				updateObject.RecordPermissions.CanDelete = new List<Guid>();
				updateObject.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
				updateObject.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				updateObject.RecordPermissions.CanCreate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
				updateObject.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				updateObject.RecordPermissions.CanUpdate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
				updateObject.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				updateObject.RecordPermissions.CanDelete.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
				updateObject.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				var updateEntityResult = entMan.UpdateEntity(updateObject);
				if (!updateEntityResult.Success)
				{
					throw new Exception("System error 10060. Entity update with name : contact. Message:" + updateEntityResult.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: created_on >>
			{
				InputDateTimeField datetimeField = new InputDateTimeField();
				datetimeField.Id = new Guid("52f89031-2d6d-47af-ba28-40da08b040ae");
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), datetimeField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: created_on Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: photo >>
			{
				InputImageField imageField = new InputImageField();
				imageField.Id = new Guid("63e82ecb-ff4e-4fd0-91be-6278875ea39c");
				imageField.Name = "photo";
				imageField.Label = "Photo";
				imageField.PlaceholderText = null;
				imageField.Description = null;
				imageField.HelpText = null;
				imageField.Required = false;
				imageField.Unique = false;
				imageField.Searchable = false;
				imageField.Auditable = false;
				imageField.System = true;
				imageField.DefaultValue = null;
				imageField.EnableSecurity = false;
				imageField.Permissions = new FieldPermissions();
				imageField.Permissions.CanRead = new List<Guid>();
				imageField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), imageField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: photo Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: x_search >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("6d33f297-1cd4-4b75-a0cf-1887b7a3ced8");
				textboxField.Name = "x_search";
				textboxField.Label = "Search Index";
				textboxField.PlaceholderText = "Search contacts";
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
					var response = entMan.CreateField(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: contact Field: x_search Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: contact Field Name: salutation_id >>
			{
				InputGuidField guidField = new InputGuidField();
				guidField.Id = new Guid("afd8d03c-8bd8-44f8-8c46-b13e57cffa30");
				guidField.Name = "salutation_id";
				guidField.Label = "Salutation";
				guidField.PlaceholderText = null;
				guidField.Description = null;
				guidField.HelpText = null;
				guidField.Required = true;
				guidField.Unique = false;
				guidField.Searchable = false;
				guidField.Auditable = false;
				guidField.System = true;
				guidField.DefaultValue = Guid.Parse("87c08ee1-8d4d-4c89-9b37-4e3cc3f98698");
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
						throw new Exception("System error 10060. Entity: contact Field: salutation_id Message:" + response.Message);
				}
			}
			#endregion



			#region << ***Update field***  Entity: feed_item Field Name: l_related_records >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_related_records").Id;
				textboxField.Name = "l_related_records";
				textboxField.Label = "Related Record lookup";
				textboxField.PlaceholderText = null;
				textboxField.Description = "csv list of related parent primary key";
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
					var response = entMan.UpdateField(new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: feed_item Field: l_related_records Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: feed_item Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("db83b9b0-448c-4675-be71-640aca2e2a3a"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: feed_item Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create entity*** Entity name: salutation >>
			{
				#region << entity >>
				{
					var entity = new InputEntity();
					var systemFieldIdDictionary = new Dictionary<string, Guid>();
					systemFieldIdDictionary["id"] = new Guid("8721d461-ded9-46e7-8b1e-b7d0703a8d21");
					entity.Id = new Guid("690dc799-e732-4d17-80d8-0f761bc33def");
					entity.Name = "salutation";
					entity.Label = "Salutation";
					entity.LabelPlural = "Salutations";
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
					entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
					//DELETE
					{
						var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: salutation creation Message: " + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << ***Create field***  Entity: salutation Field Name: is_default >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("17f9eb90-f712-472a-9b33-a5cdcfd15c68");
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
					var response = entMan.CreateField(new Guid("690dc799-e732-4d17-80d8-0f761bc33def"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: salutation Field: is_default Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: salutation Field Name: is_enabled >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("77ac9673-86df-43e9-bb17-b648f1fe5eb4");
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
					var response = entMan.CreateField(new Guid("690dc799-e732-4d17-80d8-0f761bc33def"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: salutation Field: is_enabled Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: salutation Field Name: is_system >>
			{
				InputCheckboxField checkboxField = new InputCheckboxField();
				checkboxField.Id = new Guid("059917a0-4fdd-4154-9500-ebe8a0124ee2");
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
					var response = entMan.CreateField(new Guid("690dc799-e732-4d17-80d8-0f761bc33def"), checkboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: salutation Field: is_system Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: salutation Field Name: label >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("8318dfb5-c656-459b-adc8-83f4f0ee65a0");
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
					var response = entMan.CreateField(new Guid("690dc799-e732-4d17-80d8-0f761bc33def"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: salutation Field: label Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: salutation Field Name: sort_index >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("e2a82937-7982-4fc2-84ca-b734efabb6b8");
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
					var response = entMan.CreateField(new Guid("690dc799-e732-4d17-80d8-0f761bc33def"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: salutation Field: sort_index Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: salutation Field Name: l_scope >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("a2de0020-63c6-4fb9-a35c-f3b63cc3455e");
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.CreateField(new Guid("690dc799-e732-4d17-80d8-0f761bc33def"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: salutation Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: case Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create field***  Entity: case Field Name: x_search >>
			{
				InputTextField textboxField = new InputTextField();
				textboxField.Id = new Guid("d74a9521-c81c-4784-9aac-6339025ce51a");
				textboxField.Name = "x_search";
				textboxField.Label = "Search Index";
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
					var response = entMan.CreateField(new Guid("0ebb3981-7443-45c8-ab38-db0709daf58c"), textboxField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case Field: x_search Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: case_status Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("960afdc1-cd78-41ab-8135-816f7f7b8a27"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_status Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: milestone Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("c15f030a-9d94-4767-89aa-c55a09f8b83e"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: milestone Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: task_status Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("9221f095-f749-4b88-94e5-9fa485527ef7")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("9221f095-f749-4b88-94e5-9fa485527ef7"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_status Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: task_type Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("35999e55-821c-4798-8e8f-29d8c672c9b9")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("35999e55-821c-4798-8e8f-29d8c672c9b9"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task_type Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: task Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("9386226e-381e-4522-b27b-fb5514d77902")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: task Field Name: x_search >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("9386226e-381e-4522-b27b-fb5514d77902")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "x_search").Id;
				textboxField.Name = "x_search";
				textboxField.Label = "Search index";
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
					var response = entMan.UpdateField(new Guid("9386226e-381e-4522-b27b-fb5514d77902"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: task Field: x_search Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: case_type Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("0dfeba58-40bb-4205-a539-c16d5c0885ad")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("0dfeba58-40bb-4205-a539-c16d5c0885ad"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: case_type Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: industry Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("2c60e662-367e-475d-9fcb-3ead55178a56")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("2c60e662-367e-475d-9fcb-3ead55178a56"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: industry Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: project Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("2d9b2d1d-e32b-45e1-a013-91d92a9ce792"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: project Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Update field***  Entity: country Field Name: l_scope >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8")).Object;
				InputTextField textboxField = new InputTextField();
				textboxField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "l_scope").Id;
				textboxField.Name = "l_scope";
				textboxField.Label = "Scope";
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
					var response = entMan.UpdateField(new Guid("54cfe9e9-5e0e-44d2-a1f9-5c3bbb9822c8"), textboxField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: country Field: l_scope Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Delete entity*** Entity Name: solutation >>
			{
				{
					var response = entMan.DeleteEntity(new Guid("f0b64034-e0f6-452e-b82b-88186af6df88"));
					if (!response.Success)
						throw new Exception("System error 10060. Entity: solutation Delete. Message:" + response.Message);
				}
			}
			#endregion


			#region << ***Create record*** Id: 87c08ee1-8d4d-4c89-9b37-4e3cc3f98698 (salutation) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""87c08ee1-8d4d-4c89-9b37-4e3cc3f98698"",
  ""is_default"": true,
  ""is_enabled"": true,
  ""is_system"": true,
  ""label"": ""Mr."",
  ""sort_index"": 1.0,
  ""l_scope"": """"
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("salutation", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 0ede7d96-2d85-45fa-818b-01327d4c47a9 (salutation) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""0ede7d96-2d85-45fa-818b-01327d4c47a9"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""label"": ""Ms."",
  ""sort_index"": 2.0,
  ""l_scope"": """"
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("salutation", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: ab073457-ddc8-4d36-84a5-38619528b578 (salutation) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""ab073457-ddc8-4d36-84a5-38619528b578"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""label"": ""Mrs."",
  ""sort_index"": 3.0,
  ""l_scope"": """"
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("salutation", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: 5b8d0137-9ec5-4b1c-a9b0-e982ef8698c1 (salutation) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""5b8d0137-9ec5-4b1c-a9b0-e982ef8698c1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""label"": ""Dr."",
  ""sort_index"": 4.0,
  ""l_scope"": """"
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("salutation", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion

			#region << ***Create record*** Id: a74cd934-b425-4061-8f4e-a6d6b9d7adb1 (salutation) >>
			{
				var json = @"{
  ""$type"": ""WebVella.Erp.Api.Models.EntityRecord, WebVella.Erp"",
  ""id"": ""a74cd934-b425-4061-8f4e-a6d6b9d7adb1"",
  ""is_default"": false,
  ""is_enabled"": true,
  ""is_system"": true,
  ""label"": ""Prof."",
  ""sort_index"": 5.0,
  ""l_scope"": """"
}";
				EntityRecord rec = JsonConvert.DeserializeObject<EntityRecord>(json);
				var result = recMan.CreateRecord("salutation", rec);
				if (!result.Success) throw new Exception(result.Message);
			}
			#endregion



			//If existing accounts change to new default
			{
				var eqlCommand = "SELECT * FROM account WHERE salutation_id = @salutationNull";
				var eqlResult = new EqlCommand(eqlCommand, new EqlParameter("salutationNull", null)).Execute();
				foreach (var record in eqlResult)
				{
					var patchRecord = new EntityRecord();
					patchRecord["id"] = (Guid)record["id"];
					patchRecord["salutation_id"] = new Guid("87c08ee1-8d4d-4c89-9b37-4e3cc3f98698");
					var patchResult = recMan.UpdateRecord("account", patchRecord);
					if (!patchResult.Success)
						throw new Exception(patchResult.Message);
				}
			}


			{
				var eqlCommand = "SELECT * FROM contact WHERE salutation_id = @salutationNull";
				var eqlResult = new EqlCommand(eqlCommand, new EqlParameter("salutationNull", null)).Execute();
				foreach (var record in eqlResult)
				{
					var patchRecord = new EntityRecord();
					patchRecord["id"] = (Guid)record["id"];
					patchRecord["salutation_id"] = new Guid("87c08ee1-8d4d-4c89-9b37-4e3cc3f98698");
					var patchResult = recMan.UpdateRecord("contact", patchRecord);
					if (!patchResult.Success)
						throw new Exception(patchResult.Message);
				}
			}

			#region << ***Create relation*** Relation name: salutation_1n_account >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("690dc799-e732-4d17-80d8-0f761bc33def")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("2e22b50f-e444-4b62-a171-076e51246939")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "salutation_id");
				relation.Id = new Guid("99e1a18b-05c2-4fca-986e-37ecebd62168");
				relation.Name = "salutation_1n_account";
				relation.Label = "salutation_1n_account";
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
						throw new Exception("System error 10060. Relation: salutation_1n_account Create. Message:" + response.Message);
				}
			}
			#endregion

			#region << ***Create relation*** Relation name: salutation_1n_contact >>
			{
				var relation = new EntityRelation();
				var originEntity = entMan.ReadEntity(new Guid("690dc799-e732-4d17-80d8-0f761bc33def")).Object;
				var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
				var targetEntity = entMan.ReadEntity(new Guid("39e1dd9b-827f-464d-95ea-507ade81cbd0")).Object;
				var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "salutation_id");
				relation.Id = new Guid("77ca10ff-18c9-44d6-a7ae-ddb0baa6a3a9");
				relation.Name = "salutation_1n_contact";
				relation.Label = "salutation_1n_contact";
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
						throw new Exception("System error 10060. Relation: salutation_1n_contact Create. Message:" + response.Message);
				}
			}
			#endregion



		}
	}
}
