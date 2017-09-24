using System;
using System.Diagnostics;
using System.Collections.Generic;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Api;
using System.Linq;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Database;
using WebVella.ERP.Jobs;
using System.Dynamic;

namespace WebVella.ERP
{
	public class ErpService : IErpService
	{
		public ErpService()
		{
		}

		public void InitializeSystemEntities()
		{
			FieldResponse fieldResponse = null;
			EntityManager entMan = new EntityManager();
			EntityRelationManager rm = new EntityRelationManager();
			RecordManager recMan = new RecordManager(true);

			using (var connection = DbContext.Current.CreateConnection())
			{
				//setup necessary extensions
				DbRepository.CreatePostgresqlExtensions();

				try
				{
					connection.BeginTransaction();

					CheckCreateSystemTables();

					DbSystemSettings storeSystemSettings = DbContext.Current.SettingsRepository.Read();

					Guid systemSettingsId = new Guid("F3223177-B2FF-43F5-9A4B-FF16FC67D186");
					SystemSettings systemSettings = new SystemSettings();
					systemSettings.Id = systemSettingsId;

					int currentVersion = 0;
					if (storeSystemSettings != null)
					{
						systemSettings = new SystemSettings(storeSystemSettings);
						currentVersion = systemSettings.Version;
					}

					//tmp code - during debug only
					//entityManager.DeleteEntity(SystemIds.UserEntityId);
					//entityManager.DeleteEntity(SystemIds.RoleEntityId);
					//rm.Delete(SystemIds.UserRoleRelationId);
					//currentVersion = 0; 

					if (currentVersion < 150508)
					{
						systemSettings.Version = 150508;

						List<Guid> allowedRoles = new List<Guid>();
						allowedRoles.Add(SystemIds.AdministratorRoleId);

						#region << create user entity >>
						{

							InputEntity userEntity = new InputEntity();
							userEntity.Id = SystemIds.UserEntityId;
							userEntity.Name = "user";
							userEntity.Label = "User";
							userEntity.LabelPlural = "Users";
							userEntity.System = true;
							userEntity.RecordPermissions = new RecordPermissions();
							userEntity.RecordPermissions.CanCreate = new List<Guid>();
							userEntity.RecordPermissions.CanRead = new List<Guid>();
							userEntity.RecordPermissions.CanUpdate = new List<Guid>();
							userEntity.RecordPermissions.CanDelete = new List<Guid>();
							userEntity.RecordPermissions.CanCreate.Add(SystemIds.GuestRoleId);
							userEntity.RecordPermissions.CanCreate.Add(SystemIds.AdministratorRoleId);
							userEntity.RecordPermissions.CanRead.Add(SystemIds.GuestRoleId);
							userEntity.RecordPermissions.CanRead.Add(SystemIds.RegularRoleId);
							userEntity.RecordPermissions.CanRead.Add(SystemIds.AdministratorRoleId);
							userEntity.RecordPermissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
							userEntity.RecordPermissions.CanDelete.Add(SystemIds.AdministratorRoleId);
							var systemItemIdDictionary = new Dictionary<string, Guid>();
							systemItemIdDictionary["id"] = new Guid("8e438549-fd30-4766-95a9-061008cee48e");
							systemItemIdDictionary["created_on"] = new Guid("6fda5e6b-80e6-4d8a-9e2a-d983c3694e96");
							systemItemIdDictionary["created_by"] = new Guid("825e8367-3be1-4022-ba66-6494859d70d9");
							systemItemIdDictionary["last_modified_on"] = new Guid("5a975d33-47c6-4ba6-83c8-c24034206879");
							systemItemIdDictionary["last_modified_by"] = new Guid("cafc8cda-1a1d-43e4-9406-6acf8ba8fa8d");
							var response = entMan.CreateEntity(userEntity, false, false, systemItemIdDictionary);

							InputTextField firstName = new InputTextField();

							firstName.Id = new Guid("DF211549-41CC-4D11-BB43-DACA4C164411");
							firstName.Name = "first_name";
							firstName.Label = "First Name";
							firstName.PlaceholderText = "";
							firstName.Description = "First name of the user";
							firstName.HelpText = "";
							firstName.Required = false;
							firstName.Unique = false;
							firstName.Searchable = false;
							firstName.Auditable = false;
							firstName.System = true;
							firstName.DefaultValue = "";

							firstName.MaxLength = 200;

							fieldResponse = entMan.CreateField(userEntity.Id.Value, firstName, false);

							InputTextField lastName = new InputTextField();

							lastName.Id = new Guid("63E685B1-B2C6-4961-B393-2B6723EBD1BF");
							lastName.Name = "last_name";
							lastName.Label = "Last Name";
							lastName.PlaceholderText = "";
							lastName.Description = "Last name of the user";
							lastName.HelpText = "";
							lastName.Required = false;
							lastName.Unique = false;
							lastName.Searchable = false;
							lastName.Auditable = false;
							lastName.System = true;
							lastName.DefaultValue = "";

							lastName.MaxLength = 200;

							fieldResponse = entMan.CreateField(userEntity.Id.Value, lastName, false);

							InputTextField userName = new InputTextField();
							userName.Id = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
							userName.Name = "username";
							userName.Label = "User Name";
							userName.PlaceholderText = "";
							userName.Description = "screen name for the user";
							userName.HelpText = "";
							userName.Required = true;
							userName.Unique = true;
							userName.Searchable = true;
							userName.Auditable = false;
							userName.System = true;
							userName.DefaultValue = string.Empty;
							userName.MaxLength = 200;
							fieldResponse = entMan.CreateField(userEntity.Id.Value, userName, false);


							InputEmailField email = new InputEmailField();

							email.Id = new Guid("9FC75C8F-CE80-4A64-81D7-E2BEFA5E4815");
							email.Name = "email";
							email.Label = "Email";
							email.PlaceholderText = "";
							email.Description = "Email address of the user";
							email.HelpText = "";
							email.Required = true;
							email.Unique = true;
							email.Searchable = true;
							email.Auditable = false;
							email.System = true;
							email.DefaultValue = string.Empty;

							email.MaxLength = 255;

							fieldResponse = entMan.CreateField(userEntity.Id.Value, email, false);

							InputPasswordField password = new InputPasswordField();

							password.Id = new Guid("4EDE88D9-217A-4462-9300-EA0D6AFCDCEA");
							password.Name = "password";
							password.Label = "Password";
							password.PlaceholderText = "";
							password.Description = "Password for the user account";
							password.HelpText = "";
							password.Required = true;
							password.Unique = false;
							password.Searchable = false;
							password.Auditable = false;
							password.System = true;
							password.MinLength = 6;
							password.MaxLength = 24;
							password.Encrypted = true;

							fieldResponse = entMan.CreateField(userEntity.Id.Value, password, false);

							InputDateTimeField lastLoggedIn = new InputDateTimeField();

							lastLoggedIn.Id = new Guid("3C85CCEC-D526-4E47-887F-EE169D1F508D");
							lastLoggedIn.Name = "last_logged_in";
							lastLoggedIn.Label = "Last Logged In";
							lastLoggedIn.PlaceholderText = "";
							lastLoggedIn.Description = "";
							lastLoggedIn.HelpText = "";
							lastLoggedIn.Required = false;
							lastLoggedIn.Unique = false;
							lastLoggedIn.Searchable = false;
							lastLoggedIn.Auditable = true;
							lastLoggedIn.System = true;
							lastLoggedIn.DefaultValue = null;

							lastLoggedIn.Format = "dd MMM yyyy HH:mm:ss";
							lastLoggedIn.UseCurrentTimeAsDefaultValue = true;

							fieldResponse = entMan.CreateField(userEntity.Id.Value, lastLoggedIn, false);

							InputCheckboxField enabledField = new InputCheckboxField();

							enabledField.Id = new Guid("C0C63650-7572-4252-8E4B-4E25C94897A6");
							enabledField.Name = "enabled";
							enabledField.Label = "Enabled";
							enabledField.PlaceholderText = "";
							enabledField.Description = "Shows if the user account is enabled";
							enabledField.HelpText = "";
							enabledField.Required = true;
							enabledField.Unique = false;
							enabledField.Searchable = false;
							enabledField.Auditable = false;
							enabledField.System = true;
							enabledField.DefaultValue = false;

							fieldResponse = entMan.CreateField(userEntity.Id.Value, enabledField, false);

							InputCheckboxField verifiedUserField = new InputCheckboxField();

							verifiedUserField.Id = new Guid("F1BA5069-8CC9-4E66-BCC3-60E33C79C265");
							verifiedUserField.Name = "verified";
							verifiedUserField.Label = "Verified";
							verifiedUserField.PlaceholderText = "";
							verifiedUserField.Description = "Shows if the user email is verified";
							verifiedUserField.HelpText = "";
							verifiedUserField.Required = true;
							verifiedUserField.Unique = false;
							verifiedUserField.Searchable = false;
							verifiedUserField.Auditable = false;
							verifiedUserField.System = true;
							verifiedUserField.DefaultValue = false;

							fieldResponse = entMan.CreateField(userEntity.Id.Value, verifiedUserField, false);

							#region << image >>
							{
								InputImageField imageField = new InputImageField();
								imageField.Id = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
								imageField.Name = "image";
								imageField.Label = "Image";
								imageField.PlaceholderText = "";
								imageField.Description = "";
								imageField.HelpText = "";
								imageField.Required = false;
								imageField.Unique = false;
								imageField.Searchable = false;
								imageField.Auditable = false;
								imageField.System = true;
								imageField.DefaultValue = string.Empty;
								imageField.EnableSecurity = false;
								{
									var createResponse = entMan.CreateField(SystemIds.UserEntityId, imageField, false);
									if (!createResponse.Success)
										throw new Exception("System error 10060. Entity: user. Field: image" + " Message:" + createResponse.Message);
								}
							}
							#endregion
						}

						#endregion

						#region << create role entity >>

						{
							InputEntity roleEntity = new InputEntity();
							roleEntity.Id = SystemIds.RoleEntityId;
							roleEntity.Name = "role";
							roleEntity.Label = "Role";
							roleEntity.LabelPlural = "Roles";
							roleEntity.System = true;
							roleEntity.RecordPermissions = new RecordPermissions();
							roleEntity.RecordPermissions.CanCreate = new List<Guid>();
							roleEntity.RecordPermissions.CanRead = new List<Guid>();
							roleEntity.RecordPermissions.CanUpdate = new List<Guid>();
							roleEntity.RecordPermissions.CanDelete = new List<Guid>();
							roleEntity.RecordPermissions.CanCreate.Add(SystemIds.GuestRoleId);
							roleEntity.RecordPermissions.CanCreate.Add(SystemIds.AdministratorRoleId);
							roleEntity.RecordPermissions.CanRead.Add(SystemIds.RegularRoleId);
							roleEntity.RecordPermissions.CanRead.Add(SystemIds.GuestRoleId);
							roleEntity.RecordPermissions.CanRead.Add(SystemIds.AdministratorRoleId);
							roleEntity.RecordPermissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
							roleEntity.RecordPermissions.CanDelete.Add(SystemIds.AdministratorRoleId);
							var systemItemIdDictionary = new Dictionary<string, Guid>();
							systemItemIdDictionary["id"] = new Guid("37fd9c4f-5413-4f3a-aa2f-d831cc106d03");
							systemItemIdDictionary["created_on"] = new Guid("64047bab-dc73-4175-a744-e5d565e8adbb");
							systemItemIdDictionary["created_by"] = new Guid("0ccd806b-715c-42d4-a580-f3f11f55d937");
							systemItemIdDictionary["last_modified_on"] = new Guid("c4522433-1c67-44f9-b461-e85d4d363b70");
							systemItemIdDictionary["last_modified_by"] = new Guid("a4489db4-9d76-4d5a-8940-6ef2da562c25");
							systemItemIdDictionary["user_role_created_by"] = new Guid("c6151e80-9dce-4c0b-ae5f-4798e14cff4c");
							systemItemIdDictionary["user_role_modified_by"] = new Guid("f3efaefe-32d2-4840-ac06-bc5723e323d0");
							var response = entMan.CreateEntity(roleEntity, false, false, systemItemIdDictionary);

							InputTextField nameRoleField = new InputTextField();

							nameRoleField.Id = new Guid("36F91EBD-5A02-4032-8498-B7F716F6A349");
							nameRoleField.Name = "name";
							nameRoleField.Label = "Name";
							nameRoleField.PlaceholderText = "";
							nameRoleField.Description = "The name of the role";
							nameRoleField.HelpText = "";
							nameRoleField.Required = true;
							nameRoleField.Unique = false;
							nameRoleField.Searchable = false;
							nameRoleField.Auditable = false;
							nameRoleField.System = true;
							nameRoleField.DefaultValue = "";
							nameRoleField.MaxLength = 200;
							nameRoleField.EnableSecurity = true;
							nameRoleField.Permissions = new FieldPermissions();
							nameRoleField.Permissions.CanRead = new List<Guid>();
							nameRoleField.Permissions.CanUpdate = new List<Guid>();
							//READ
							nameRoleField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
							nameRoleField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
							//UPDATE
							nameRoleField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);

							fieldResponse = entMan.CreateField(roleEntity.Id.Value, nameRoleField, false);

							InputTextField descriptionRoleField = new InputTextField();

							descriptionRoleField.Id = new Guid("4A8B9E0A-1C36-40C6-972B-B19E2B5D265B");
							descriptionRoleField.Name = "description";
							descriptionRoleField.Label = "Description";
							descriptionRoleField.PlaceholderText = "";
							descriptionRoleField.Description = "";
							descriptionRoleField.HelpText = "";
							descriptionRoleField.Required = true;
							descriptionRoleField.Unique = false;
							descriptionRoleField.Searchable = false;
							descriptionRoleField.Auditable = false;
							descriptionRoleField.System = true;
							descriptionRoleField.DefaultValue = "";

							descriptionRoleField.MaxLength = 200;

							fieldResponse = entMan.CreateField(roleEntity.Id.Value, descriptionRoleField, false);
						}

						#endregion


						#region << create user - role relation >>
						{
							var userEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
							var roleEntity = entMan.ReadEntity(SystemIds.RoleEntityId).Object;

							EntityRelation userRoleRelation = new EntityRelation();
							userRoleRelation.Id = SystemIds.UserRoleRelationId;
							userRoleRelation.Name = "user_role";
							userRoleRelation.Label = "User-Role";
							userRoleRelation.System = true;
							userRoleRelation.RelationType = EntityRelationType.ManyToMany;
							userRoleRelation.TargetEntityId = userEntity.Id;
							userRoleRelation.TargetFieldId = userEntity.Fields.Single(x => x.Name == "id").Id;
							userRoleRelation.OriginEntityId = roleEntity.Id;
							userRoleRelation.OriginFieldId = roleEntity.Fields.Single(x => x.Name == "id").Id;
							{
								var result = rm.Create(userRoleRelation);
								if (!result.Success)
									throw new Exception("CREATE USER-ROLE RELATION:" + result.Message);
							}
						}
						#endregion

						#region << create system records >>

						{
							EntityRecord user = new EntityRecord();
							user["id"] = SystemIds.SystemUserId;
							user["first_name"] = "Local";
							user["last_name"] = "System";
							user["password"] = Guid.NewGuid().ToString();
							user["email"] = "system@webvella.com";
							user["username"] = "system";
							user["created_by"] = SystemIds.SystemUserId;
							user["last_modified_by"] = SystemIds.SystemUserId;
							user["created_on"] = DateTime.UtcNow;
							user["enabled"] = true;

							QueryResponse result = recMan.CreateRecord("user", user);
							if (!result.Success)
								throw new Exception("CREATE SYSTEM USER RECORD:" + result.Message);
						}

						{
							EntityRecord user = new EntityRecord();
							user["id"] = SystemIds.FirstUserId;
							user["first_name"] = "WebVella";
							user["last_name"] = "Erp";
							user["password"] = "erp";
							user["email"] = "erp@webvella.com";
							user["username"] = "administrator";
							user["created_by"] = SystemIds.SystemUserId;
							user["last_modified_by"] = SystemIds.SystemUserId;
							user["created_on"] = DateTime.UtcNow;
							user["enabled"] = true;

							QueryResponse result = recMan.CreateRecord("user", user);
							if (!result.Success)
								throw new Exception("CREATE FIRST USER RECORD:" + result.Message);
						}

						{
							EntityRecord adminRole = new EntityRecord();
							adminRole["id"] = SystemIds.AdministratorRoleId;
							adminRole["name"] = "administrator";
							adminRole["description"] = "";
							adminRole["created_by"] = SystemIds.SystemUserId;
							adminRole["last_modified_by"] = SystemIds.SystemUserId;
							adminRole["created_on"] = DateTime.UtcNow;

							QueryResponse result = recMan.CreateRecord("role", adminRole);
							if (!result.Success)
								throw new Exception("CREATE ADMINITRATOR ROLE RECORD:" + result.Message);
						}

						{
							EntityRecord regularRole = new EntityRecord();
							regularRole["id"] = SystemIds.RegularRoleId;
							regularRole["name"] = "regular";
							regularRole["description"] = "";
							regularRole["created_by"] = SystemIds.SystemUserId;
							regularRole["last_modified_by"] = SystemIds.SystemUserId;
							regularRole["created_on"] = DateTime.UtcNow;

							QueryResponse result = recMan.CreateRecord("role", regularRole);
							if (!result.Success)
								throw new Exception("CREATE REGULAR ROLE RECORD:" + result.Message);
						}

						{
							EntityRecord guestRole = new EntityRecord();
							guestRole["id"] = SystemIds.GuestRoleId;
							guestRole["name"] = "guest";
							guestRole["description"] = "";
							guestRole["created_by"] = SystemIds.SystemUserId;
							guestRole["last_modified_by"] = SystemIds.SystemUserId;
							guestRole["created_on"] = DateTime.UtcNow;

							QueryResponse result = recMan.CreateRecord("role", guestRole);
							if (!result.Success)
								throw new Exception("CREATE GUEST ROLE RECORD:" + result.Message);
						}

						{
							QueryResponse result = recMan.CreateRelationManyToManyRecord(SystemIds.UserRoleRelationId, SystemIds.AdministratorRoleId, SystemIds.SystemUserId);
							if (!result.Success)
								throw new Exception("CREATE SYSTEM-USER <-> ADMINISTRATOR ROLE RELATION RECORD:" + result.Message);

						}

						{
							QueryResponse result = recMan.CreateRelationManyToManyRecord(SystemIds.UserRoleRelationId, SystemIds.AdministratorRoleId, SystemIds.FirstUserId);
							if (!result.Success)
								throw new Exception("CREATE FIRST-USER <-> ADMINISTRATOR ROLE RELATION RECORD:" + result.Message);


							result = recMan.CreateRelationManyToManyRecord(SystemIds.UserRoleRelationId, SystemIds.RegularRoleId, SystemIds.FirstUserId);
							if (!result.Success)
								throw new Exception("CREATE FIRST-USER <-> REGULAR ROLE RELATION RECORD:" + result.Message);

						}

						#endregion

						#region << create Area entity >>
						{
							InputEntity areaEntity = new InputEntity();
							areaEntity.Id = SystemIds.AreaEntityId;
							areaEntity.Name = "area";
							areaEntity.Label = "Area";
							areaEntity.LabelPlural = "areas";
							areaEntity.System = true;
							areaEntity.IconName = "folder";
							areaEntity.Weight = 10;
							areaEntity.RecordPermissions = new RecordPermissions();
							areaEntity.RecordPermissions.CanCreate = new List<Guid>();
							areaEntity.RecordPermissions.CanRead = new List<Guid>();
							areaEntity.RecordPermissions.CanUpdate = new List<Guid>();
							areaEntity.RecordPermissions.CanDelete = new List<Guid>();
							areaEntity.RecordPermissions.CanCreate.Add(SystemIds.AdministratorRoleId);
							areaEntity.RecordPermissions.CanRead.Add(SystemIds.RegularRoleId);
							areaEntity.RecordPermissions.CanRead.Add(SystemIds.AdministratorRoleId);
							areaEntity.RecordPermissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
							areaEntity.RecordPermissions.CanDelete.Add(SystemIds.AdministratorRoleId);

							var systemItemIdDictionary = new Dictionary<string, Guid>();
							systemItemIdDictionary["id"] = new Guid("19f16bdb-56e6-46bf-8310-2b42fd78be2a");
							systemItemIdDictionary["created_on"] = new Guid("3e6be69e-8f25-40e4-9f21-86b0d1404230");
							systemItemIdDictionary["created_by"] = new Guid("16fbba6c-6282-4828-9873-86b8fef745d4");
							systemItemIdDictionary["last_modified_on"] = new Guid("5f076d8b-e587-4201-9481-67e19789ff6c");
							systemItemIdDictionary["last_modified_by"] = new Guid("721b27b3-741d-4414-8783-a0245a4eec58");
							systemItemIdDictionary["user_area_created_by"] = new Guid("5fe5fdc4-ee10-4661-93e7-25ea2a61e710");
							systemItemIdDictionary["user_area_modified_by"] = new Guid("bb52122c-a354-4668-9423-71dfdc3d9f36");
							{
								var createResponse = entMan.CreateEntity(areaEntity, false, false, systemItemIdDictionary);
								if (!createResponse.Success)
									throw new Exception("System error 10330. Message:" + createResponse.Message);
							}

							InputTextField color = new InputTextField();
							color.Id = new Guid("2B4AACD9-3C34-4C44-B3A3-8AFF1520CFF6");
							color.Name = "color";
							color.Label = "Color";
							color.PlaceholderText = "";
							color.Description = "";
							color.HelpText = "";
							color.Required = true;
							color.Unique = false;
							color.Searchable = false;
							color.Auditable = false;
							color.System = true;
							color.DefaultValue = "teal";
							color.MaxLength = null;
							{
								var createResponse = entMan.CreateField(SystemIds.AreaEntityId, color, false);
								if (!createResponse.Success)
									throw new Exception("System error 10340. Message:" + createResponse.Message);
							}


							InputTextField label = new InputTextField();
							label.Id = new Guid("F050E7A1-AFB7-4346-B57B-1F12B2BD5AE5");
							label.Name = "label";
							label.Label = "Label";
							label.PlaceholderText = "";
							label.Description = "";
							label.HelpText = "";
							label.Required = true;
							label.Unique = false;
							label.Searchable = false;
							label.Auditable = false;
							label.System = true;
							label.DefaultValue = "Default";
							label.MaxLength = null;
							{
								var createResponse = entMan.CreateField(SystemIds.AreaEntityId, label, false);
								if (!createResponse.Success)
									throw new Exception("System error 10340. Message:" + createResponse.Message);
							}

							InputTextField iconName = new InputTextField();
							iconName.Id = new Guid("5EA0C872-D219-4D94-9EFA-C5DA978D316B");
							iconName.Name = "icon_name";
							iconName.Label = "Icon Name";
							iconName.PlaceholderText = "";
							iconName.Description = "";
							iconName.HelpText = "";
							iconName.Required = true;
							iconName.Unique = false;
							iconName.Searchable = false;
							iconName.Auditable = false;
							iconName.System = true;
							iconName.DefaultValue = "database";
							iconName.MaxLength = null;
							{
								var createResponse = entMan.CreateField(SystemIds.AreaEntityId, iconName, false);
								if (!createResponse.Success)
									throw new Exception("System error 10340. Message:" + createResponse.Message);
							}

							InputNumberField weight = new InputNumberField();
							weight.Id = new Guid("9B169431-6C31-4141-80EB-5844B8333E63");
							weight.Name = "weight";
							weight.Label = "Weight";
							weight.PlaceholderText = "";
							weight.Description = "";
							weight.HelpText = "";
							weight.Required = true;
							weight.Unique = false;
							weight.Searchable = false;
							weight.Auditable = false;
							weight.System = true;
							weight.DefaultValue = 10;
							weight.MinValue = 0;
							weight.DecimalPlaces = 2;
							{
								var createResponse = entMan.CreateField(SystemIds.AreaEntityId, weight, false);
								if (!createResponse.Success)
									throw new Exception("System error 10340. Message:" + createResponse.Message);
							}

							InputTextField attachments = new InputTextField();
							attachments.Id = new Guid("288EA657-C12C-4AC1-B701-81D6F9F39363");
							attachments.Name = "attachments";
							attachments.Label = "Attachments JSON String";
							attachments.PlaceholderText = "";
							attachments.Description = "Stringified Array of attached objects";
							attachments.HelpText = "";
							attachments.Required = false;
							attachments.Unique = false;
							attachments.Searchable = false;
							attachments.Auditable = false;
							attachments.System = true;
							attachments.DefaultValue = null;
							attachments.MaxLength = null;
							{
								var createResponse = entMan.CreateField(SystemIds.AreaEntityId, attachments, false);
								if (!createResponse.Success)
									throw new Exception("System error 10340. Message:" + createResponse.Message);
							}

							InputTextField name = new InputTextField();
							name.Id = new Guid("F297577B-073E-4D18-81F3-675C1AFB466D");
							name.Name = "name";
							name.Label = "Name";
							name.PlaceholderText = "";
							name.Description = "";
							name.HelpText = "";
							name.Required = true;
							name.Unique = false;
							name.Searchable = false;
							name.Auditable = false;
							name.System = true;
							name.DefaultValue = "default";
							name.MaxLength = null;
							{
								var createResponse = entMan.CreateField(SystemIds.AreaEntityId, name, false);
								if (!createResponse.Success)
									throw new Exception("System error 10340. Message:" + createResponse.Message);
							}

							InputTextField roles = new InputTextField();
							roles.Id = new Guid("8E486F76-D0C1-4D0E-8617-9EF868BF1C55");
							roles.Name = "roles";
							roles.Label = "Roles JSON String";
							roles.PlaceholderText = "";
							roles.Description = "Stringified Array of roles that have access to this area";
							roles.HelpText = "";
							roles.Required = false;
							roles.Unique = false;
							roles.Searchable = false;
							roles.Auditable = false;
							roles.System = true;
							roles.DefaultValue = null;
							roles.MaxLength = null;
							{
								var createResponse = entMan.CreateField(SystemIds.AreaEntityId, roles, false);
								if (!createResponse.Success)
									throw new Exception("System error 10340. Message:" + createResponse.Message);
							}

							#region << folder >>
							{
								InputTextField textboxField = new InputTextField();
								textboxField.Id = new Guid("6a0fdf14-2d2b-4c6c-b2f1-4d846c7d5ab8");
								textboxField.Name = "folder";
								textboxField.Label = "folder";
								textboxField.PlaceholderText = "";
								textboxField.Description = "";
								textboxField.HelpText = "";
								textboxField.Required = false;
								textboxField.Unique = false;
								textboxField.Searchable = false;
								textboxField.Auditable = false;
								textboxField.System = true;
								textboxField.DefaultValue = string.Empty;
								textboxField.MaxLength = null;
								textboxField.EnableSecurity = true;
								textboxField.Permissions = new FieldPermissions();
								textboxField.Permissions.CanRead = new List<Guid>();
								textboxField.Permissions.CanUpdate = new List<Guid>();
								//READ
								textboxField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
								//UPDATE
								textboxField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
								{
									var createResponse = entMan.CreateField(SystemIds.AreaEntityId, textboxField, false);
									if (!createResponse.Success)
										throw new Exception("System error 10060. Entity: area Field: folder" + " Message:" + createResponse.Message);
								}
							}
							#endregion

						}
						#endregion
					}


					if (currentVersion < 20160430)
					{
						systemSettings.Version = 20160430;

						#region << plugin_data >>
						var PLUGIN_DATA_ID = new Guid("d928d031-c8b1-4359-be3e-39bceb58f268");
						var PLUGIN_DATA_NAME = "plugin_data";
						{
							#region << entity >>
							{
								InputEntity entity = new InputEntity();
								entity.Id = PLUGIN_DATA_ID;
								entity.Name = PLUGIN_DATA_NAME;
								entity.Label = "Plugin Data";
								entity.LabelPlural = "Plugin Data";
								entity.System = true;
								entity.IconName = "database";
								entity.Weight = 99;
								entity.RecordPermissions = new RecordPermissions();
								entity.RecordPermissions.CanCreate = new List<Guid>();
								entity.RecordPermissions.CanRead = new List<Guid>();
								entity.RecordPermissions.CanUpdate = new List<Guid>();
								entity.RecordPermissions.CanDelete = new List<Guid>();
								//Create
								entity.RecordPermissions.CanCreate.Add(SystemIds.AdministratorRoleId);
								//READ
								entity.RecordPermissions.CanRead.Add(SystemIds.AdministratorRoleId);
								//UPDATE
								entity.RecordPermissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
								//DELETE
								entity.RecordPermissions.CanDelete.Add(SystemIds.AdministratorRoleId);

								var systemItemIdDictionary = new Dictionary<string, Guid>();
								systemItemIdDictionary["id"] = new Guid("bdb47d11-b8ee-42e9-8cd1-56e43246656b");
								systemItemIdDictionary["created_on"] = new Guid("00f172b1-393b-4674-b6cd-32669dfb0924");
								systemItemIdDictionary["created_by"] = new Guid("89379dbe-98ea-40b0-a794-a7cbf36201af");
								systemItemIdDictionary["last_modified_on"] = new Guid("aaee0db8-d131-4273-b06a-a788757e24c3");
								systemItemIdDictionary["last_modified_by"] = new Guid("eb0d2a71-4172-4293-87d7-d238a2153abf");
								systemItemIdDictionary["user_plugin_data_created_by"] = new Guid("00e3f673-9dbc-4b57-b6d8-38d75e7d165a");
								systemItemIdDictionary["user_plugin_data_modified_by"] = new Guid("c228125d-066c-415b-8c2a-a43ba2774411");
								{
									var createResponse = entMan.CreateEntity(entity, false, false, systemItemIdDictionary);
									if (!createResponse.Success)
										throw new Exception("System error 10050. Entity: " + PLUGIN_DATA_NAME + " Field: entity creation" + " Message:" + createResponse.Message);
								}
							}
							#endregion

							#region << name >>
							{
								InputTextField textboxField = new InputTextField();
								textboxField.Id = new Guid("ab81aec7-da90-4ba8-8ac7-378faa01763f");
								textboxField.Name = "name";
								textboxField.Label = "Name";
								textboxField.PlaceholderText = "";
								textboxField.Description = "";
								textboxField.HelpText = "";
								textboxField.Required = true;
								textboxField.Unique = true;
								textboxField.Searchable = false;
								textboxField.Auditable = false;
								textboxField.System = true;
								textboxField.DefaultValue = string.Empty;
								textboxField.MaxLength = null;
								textboxField.EnableSecurity = true;
								textboxField.Permissions = new FieldPermissions();
								textboxField.Permissions.CanRead = new List<Guid>();
								textboxField.Permissions.CanUpdate = new List<Guid>();
								//READ
								textboxField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
								//UPDATE
								textboxField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
								{
									var createResponse = entMan.CreateField(PLUGIN_DATA_ID, textboxField, false);
									if (!createResponse.Success)
										throw new Exception("System error 10060. Entity: " + PLUGIN_DATA_NAME + " Field: field_name" + " Message:" + createResponse.Message);
								}
							}
							#endregion

							#region << data >>
							{
								InputTextField textboxField = new InputTextField();
								textboxField.Id = new Guid("52a799ad-80a3-404b-99b5-1f58ce437982");
								textboxField.Name = "data";
								textboxField.Label = "Data";
								textboxField.PlaceholderText = "";
								textboxField.Description = "";
								textboxField.HelpText = "";
								textboxField.Required = false;
								textboxField.Unique = false;
								textboxField.Searchable = false;
								textboxField.Auditable = false;
								textboxField.System = true;
								textboxField.DefaultValue = string.Empty;
								textboxField.MaxLength = null;
								textboxField.EnableSecurity = true;
								textboxField.Permissions = new FieldPermissions();
								textboxField.Permissions.CanRead = new List<Guid>();
								textboxField.Permissions.CanUpdate = new List<Guid>();
								//READ
								textboxField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
								//UPDATE
								textboxField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
								{
									var createResponse = entMan.CreateField(PLUGIN_DATA_ID, textboxField, false);
									if (!createResponse.Success)
										throw new Exception("System error 10060. Entity: " + PLUGIN_DATA_NAME + " Field: field_name" + " Message:" + createResponse.Message);
								}
							}
							#endregion

						}
						#endregion

					}

					if (currentVersion < 20170125)
					{
						systemSettings.Version = 20170125;

						#region << ***Create entity*** Entity name: system_log >>
						{
							#region << entity >>
							{
								var entity = new InputEntity();
								var systemFieldIdDictionary = new Dictionary<string, Guid>();
								systemFieldIdDictionary["id"] = new Guid("fef43796-fb04-4e62-964d-c9d9a3941dfb");
								systemFieldIdDictionary["created_on"] = new Guid("33127843-8a6f-4d89-b3ef-1f9652ecf3a3");
								systemFieldIdDictionary["created_by"] = new Guid("43361778-2197-4f41-a7c7-e4b74b241a1f");
								systemFieldIdDictionary["last_modified_on"] = new Guid("9a0fe7df-1e22-4074-9536-a77fad885105");
								systemFieldIdDictionary["last_modified_by"] = new Guid("73dc9f66-dd35-4b89-80ae-7710167e31c7");
								systemFieldIdDictionary["user_system_log_created_by"] = new Guid("fd947482-89d9-4908-bdc2-7bf619204a87");
								systemFieldIdDictionary["user_system_log_modified_by"] = new Guid("9fb4fabb-8eca-4c80-b4dd-bcbccd94d356");
								entity.Id = new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933");
								entity.Name = "system_log";
								entity.Label = "Log";
								entity.LabelPlural = "Logs";
								entity.System = true;
								entity.IconName = "database";
								entity.Weight = (decimal)10.0;
								entity.RecordPermissions = new RecordPermissions();
								entity.RecordPermissions.CanCreate = new List<Guid>();
								entity.RecordPermissions.CanRead = new List<Guid>();
								entity.RecordPermissions.CanUpdate = new List<Guid>();
								entity.RecordPermissions.CanDelete = new List<Guid>();
								//Create
								entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
								//READ
								entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
								//UPDATE
								entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
								//DELETE
								entity.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
								{
									var response = entMan.CreateEntity(entity, false, false, systemFieldIdDictionary);
									if (!response.Success)
										throw new Exception("System error 10050. Entity: log creation Message: " + response.Message);
								}
							}
							#endregion
						}
						#endregion

						#region << ***Create field***  Entity: system_log Field Name: type >>
						{
							InputSelectField dropdownField = new InputSelectField();
							dropdownField.Id = new Guid("4dd8a502-e6bd-48a0-bf27-69aa47f2d73d");
							dropdownField.Name = "type";
							dropdownField.Label = "Type";
							dropdownField.PlaceholderText = "";
							dropdownField.Description = "";
							dropdownField.HelpText = "";
							dropdownField.Required = true;
							dropdownField.Unique = false;
							dropdownField.Searchable = false;
							dropdownField.Auditable = false;
							dropdownField.System = true;
							dropdownField.DefaultValue = "1";
							dropdownField.Options = new List<SelectFieldOption>
	{
		new SelectFieldOption() { Key = "1", Value = "error"},
		new SelectFieldOption() { Key = "2", Value = "info"}
	};
							dropdownField.EnableSecurity = false;
							dropdownField.Permissions = new FieldPermissions();
							dropdownField.Permissions.CanRead = new List<Guid>();
							dropdownField.Permissions.CanUpdate = new List<Guid>();
							//READ
							//UPDATE
							{
								var response = entMan.CreateField(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), dropdownField, false);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: log Field: type Message:" + response.Message);
							}
						}
						#endregion

						#region << ***Create field***  Entity: system_log Field Name: message >>
						{
							InputTextField textboxField = new InputTextField();
							textboxField.Id = new Guid("4d2bbc62-af48-44df-a397-f8fd86eef086");
							textboxField.Name = "message";
							textboxField.Label = "Message";
							textboxField.PlaceholderText = "";
							textboxField.Description = "";
							textboxField.HelpText = "";
							textboxField.Required = true;
							textboxField.Unique = false;
							textboxField.Searchable = true;
							textboxField.Auditable = false;
							textboxField.System = true;
							textboxField.DefaultValue = "message";
							textboxField.MaxLength = null;
							textboxField.EnableSecurity = false;
							textboxField.Permissions = new FieldPermissions();
							textboxField.Permissions.CanRead = new List<Guid>();
							textboxField.Permissions.CanUpdate = new List<Guid>();
							//READ
							//UPDATE
							{
								var response = entMan.CreateField(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), textboxField, false);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: log Field: message Message:" + response.Message);
							}
						}
						#endregion

						#region << ***Create field***  Entity: system_log Field Name: source >>
						{
							InputTextField textboxField = new InputTextField();
							textboxField.Id = new Guid("d96024cb-adc0-4f0b-a708-cd7e72cb913c");
							textboxField.Name = "source";
							textboxField.Label = "Source";
							textboxField.PlaceholderText = "";
							textboxField.Description = "";
							textboxField.HelpText = "";
							textboxField.Required = true;
							textboxField.Unique = false;
							textboxField.Searchable = true;
							textboxField.Auditable = false;
							textboxField.System = true;
							textboxField.DefaultValue = "source";
							textboxField.MaxLength = null;
							textboxField.EnableSecurity = false;
							textboxField.Permissions = new FieldPermissions();
							textboxField.Permissions.CanRead = new List<Guid>();
							textboxField.Permissions.CanUpdate = new List<Guid>();
							//READ
							//UPDATE
							{
								var response = entMan.CreateField(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), textboxField, false);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: log Field: source Message:" + response.Message);
							}
						}
						#endregion

						#region << ***Create field***  Entity: system_log Field Name: details >>
						{
							InputTextField textboxField = new InputTextField();
							textboxField.Id = new Guid("fec5493a-bdc9-4814-afcf-990c97e5abf1");
							textboxField.Name = "details";
							textboxField.Label = "Details";
							textboxField.PlaceholderText = "";
							textboxField.Description = "";
							textboxField.HelpText = "";
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
								var response = entMan.CreateField(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), textboxField, false);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: log Field: details Message:" + response.Message);
							}
						}
						#endregion

						#region << ***Create field***  Entity: system_log Field Name: notification_status >>
						{
							InputSelectField dropdownField = new InputSelectField();
							dropdownField.Id = new Guid("986e96b9-6ab0-4ed5-92db-91f1faff1faf");
							dropdownField.Name = "notification_status";
							dropdownField.Label = "Notification Status";
							dropdownField.PlaceholderText = "";
							dropdownField.Description = "";
							dropdownField.HelpText = "";
							dropdownField.Required = true;
							dropdownField.Unique = false;
							dropdownField.Searchable = false;
							dropdownField.Auditable = false;
							dropdownField.System = true;
							dropdownField.DefaultValue = "1";
							dropdownField.Options = new List<SelectFieldOption>
	{
		new SelectFieldOption() { Key = "1", Value = "Do not notify"},
		new SelectFieldOption() { Key = "2", Value = "Not notified"},
		new SelectFieldOption() { Key = "3", Value = "Notified"},
		new SelectFieldOption() { Key = "4", Value = "Notification failed"}
	};
							dropdownField.EnableSecurity = false;
							dropdownField.Permissions = new FieldPermissions();
							dropdownField.Permissions.CanRead = new List<Guid>();
							dropdownField.Permissions.CanUpdate = new List<Guid>();
							//READ
							//UPDATE
							{
								var response = entMan.CreateField(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), dropdownField, false);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: log Field: notification_status Message:" + response.Message);
							}
						}
						#endregion

						#region << View  Entity: system_log name: create >>
						{
							var createViewEntity = entMan.ReadEntity(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933")).Object;
							var createViewInput = new InputRecordView();

							#region << details >>
							createViewInput.Id = new Guid("20848d58-5c94-41b2-a118-ee047720cd30");
							createViewInput.Type = "Create";
							createViewInput.Name = "create";
							createViewInput.Label = "Create";
							createViewInput.Title = "Create";
							createViewInput.Default = true;
							createViewInput.System = false;
							createViewInput.Weight = Decimal.Parse("10.0");
							createViewInput.CssClass = "";
							createViewInput.IconName = "file-text-o";
							createViewInput.DynamicHtmlTemplate = null;
							createViewInput.DataSourceUrl = null;
							createViewInput.ServiceCode = null;
							#endregion

							#region << regions >>
							createViewInput.Regions = new List<InputRecordViewRegion>();

							#region << Region: header >>
							{
								var viewRegion = new InputRecordViewRegion();
								viewRegion.Name = "header";
								viewRegion.Label = "Header";
								viewRegion.Render = true;
								viewRegion.Weight = Decimal.Parse("10.0");
								viewRegion.CssClass = "";
								viewRegion.Sections = new List<InputRecordViewSection>();

								//Save region
								createViewInput.Regions.Add(viewRegion);
							}
							#endregion

							#endregion

							#region << Relation options >>
							{
								createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
							}
							#endregion

							#region << Action items >>
							{
								createViewInput.ActionItems = new List<ActionItem>();

								#region << action item: wv_back_button >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_back_button";
									actionItem.Menu = "sidebar-top";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_create_and_list >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_create_and_list";
									actionItem.Menu = "create-bottom";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a class=""btn btn-primary"" ng-click='ngCtrl.create(""default"")' ng-if=""::ngCtrl.createViewRegion != null"">Create</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_create_and_details >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_create_and_details";
									actionItem.Menu = "create-bottom";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-click='ngCtrl.create(""details"")' ng-if=""::ngCtrl.createViewRegion != null"">Create & Details</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_create_cancel >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_create_cancel";
									actionItem.Menu = "create-bottom";
									actionItem.Weight = Decimal.Parse("3.0");
									actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-click=""ngCtrl.cancel()"">Cancel</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_add_existing >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_add_existing";
									actionItem.Menu = "recursive-view-title";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-sm btn-outline"" ng-if=""::canAddExisting"" ng-click=""addExistingItem()""><i class=""fa fa-download""></i> Add existing</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_add_new >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_add_new";
									actionItem.Menu = "recursive-view-title";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-sm btn-outline"" ng-if=""::canCreate"" ng-click=""manageRelatedRecordItem(null)""><i class=""fa fa-plus""></i> Create & Add</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_edit >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_edit";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""quick edit"" class=""btn btn-sm btn-outline"" ng-click=""manageRelatedRecordItem(recordData)"" ng-if=""::canUpdate""><i class=""fa fa-pencil""></i></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_unrelate >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_unrelate";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""remove relation"" class=""btn btn-sm btn-outline"" confirmed-click=""instantDetachRecord(recordData)"" ng-confirm-click=""Are you sure that you need this relation removed?"" ng-if=""::canRemove""><i class=""fa fa-times go-red""></i></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

							}
							#endregion

							#region << Sidebar >>
							createViewInput.Sidebar = new InputRecordViewSidebar();
							createViewInput.Sidebar.CssClass = "";
							createViewInput.Sidebar.Render = false;
							createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

							#endregion
							{
								var response = entMan.CreateRecordView(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), createViewInput);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: log Updated view: create Message:" + response.Message);
							}
						}
						#endregion

						#region << View  Entity: system_log name: quick_create >>
						{
							var createViewEntity = entMan.ReadEntity(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933")).Object;
							var createViewInput = new InputRecordView();

							#region << details >>
							createViewInput.Id = new Guid("fbc7f61c-4b74-4925-a2c1-ed17f62ccbc3");
							createViewInput.Type = "Quick_Create";
							createViewInput.Name = "quick_create";
							createViewInput.Label = "Quick create";
							createViewInput.Title = "Quick create";
							createViewInput.Default = true;
							createViewInput.System = false;
							createViewInput.Weight = Decimal.Parse("10.0");
							createViewInput.CssClass = "";
							createViewInput.IconName = "file-text-o";
							createViewInput.DynamicHtmlTemplate = null;
							createViewInput.DataSourceUrl = null;
							createViewInput.ServiceCode = null;
							#endregion

							#region << regions >>
							createViewInput.Regions = new List<InputRecordViewRegion>();

							#region << Region: header >>
							{
								var viewRegion = new InputRecordViewRegion();
								viewRegion.Name = "header";
								viewRegion.Label = "Header";
								viewRegion.Render = true;
								viewRegion.Weight = Decimal.Parse("10.0");
								viewRegion.CssClass = "";
								viewRegion.Sections = new List<InputRecordViewSection>();

								//Save region
								createViewInput.Regions.Add(viewRegion);
							}
							#endregion

							#endregion

							#region << Relation options >>
							{
								createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
							}
							#endregion

							#region << Action items >>
							{
								createViewInput.ActionItems = new List<ActionItem>();

								#region << action item: wv_back_button >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_back_button";
									actionItem.Menu = "sidebar-top";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_create_and_list >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_create_and_list";
									actionItem.Menu = "create-bottom";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a class=""btn btn-primary"" ng-click='ngCtrl.create(""default"")' ng-if=""::ngCtrl.createViewRegion != null"">Create</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_create_and_details >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_create_and_details";
									actionItem.Menu = "create-bottom";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-click='ngCtrl.create(""details"")' ng-if=""::ngCtrl.createViewRegion != null"">Create & Details</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_create_cancel >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_create_cancel";
									actionItem.Menu = "create-bottom";
									actionItem.Weight = Decimal.Parse("3.0");
									actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-click=""ngCtrl.cancel()"">Cancel</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_add_existing >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_add_existing";
									actionItem.Menu = "recursive-view-title";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-sm btn-outline"" ng-if=""::canAddExisting"" ng-click=""addExistingItem()""><i class=""fa fa-download""></i> Add existing</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_add_new >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_add_new";
									actionItem.Menu = "recursive-view-title";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-sm btn-outline"" ng-if=""::canCreate"" ng-click=""manageRelatedRecordItem(null)""><i class=""fa fa-plus""></i> Create & Add</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_edit >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_edit";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""quick edit"" class=""btn btn-sm btn-outline"" ng-click=""manageRelatedRecordItem(recordData)"" ng-if=""::canUpdate""><i class=""fa fa-pencil""></i></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_unrelate >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_unrelate";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""remove relation"" class=""btn btn-sm btn-outline"" confirmed-click=""instantDetachRecord(recordData)"" ng-confirm-click=""Are you sure that you need this relation removed?"" ng-if=""::canRemove""><i class=""fa fa-times go-red""></i></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

							}
							#endregion

							#region << Sidebar >>
							createViewInput.Sidebar = new InputRecordViewSidebar();
							createViewInput.Sidebar.CssClass = "";
							createViewInput.Sidebar.Render = false;
							createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

							#endregion
							{
								var response = entMan.CreateRecordView(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), createViewInput);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: log Updated view: quick_create Message:" + response.Message);
							}
						}
						#endregion

						#region << View  Entity: system_log name: quick_view >>
						{
							var createViewEntity = entMan.ReadEntity(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933")).Object;
							var createViewInput = new InputRecordView();

							#region << details >>
							createViewInput.Id = new Guid("13de1db9-6283-41f3-90a9-6787eeec7ab2");
							createViewInput.Type = "Quick_View";
							createViewInput.Name = "quick_view";
							createViewInput.Label = "Quick view";
							createViewInput.Title = "Quick view";
							createViewInput.Default = true;
							createViewInput.System = false;
							createViewInput.Weight = Decimal.Parse("10.0");
							createViewInput.CssClass = "";
							createViewInput.IconName = "file-text-o";
							createViewInput.DynamicHtmlTemplate = null;
							createViewInput.DataSourceUrl = null;
							createViewInput.ServiceCode = null;
							#endregion

							#region << regions >>
							createViewInput.Regions = new List<InputRecordViewRegion>();

							#region << Region: header >>
							{
								var viewRegion = new InputRecordViewRegion();
								viewRegion.Name = "header";
								viewRegion.Label = "Header";
								viewRegion.Render = true;
								viewRegion.Weight = Decimal.Parse("10.0");
								viewRegion.CssClass = "";
								viewRegion.Sections = new List<InputRecordViewSection>();

								//Save region
								createViewInput.Regions.Add(viewRegion);
							}
							#endregion

							#endregion

							#region << Relation options >>
							{
								createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
							}
							#endregion

							#region << Action items >>
							{
								createViewInput.ActionItems = new List<ActionItem>();

								#region << action item: wv_record_delete >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_record_delete";
									actionItem.Menu = "page-title-dropdown";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?"" ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')""><i class=""fa fa-trash go-red""></i> Delete Record</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_manage_view >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_manage_view";
									actionItem.Menu = "page-title-dropdown";
									actionItem.Weight = Decimal.Parse("101.0");
									actionItem.Template = @"<a target=""_blank"" ng-href=""{{ngCtrl.getViewManageUrl()}}"" ng-if=""::ngCtrl.userIsAdmin()"">
	<i class=""fa fa-fw fa-cog""></i> Manage View
</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_back_button >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_back_button";
									actionItem.Menu = "sidebar-top";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_add_existing >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_add_existing";
									actionItem.Menu = "recursive-view-title";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-sm btn-outline"" ng-if=""::canAddExisting"" ng-click=""addExistingItem()""><i class=""fa fa-download""></i> Add existing</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_add_new >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_add_new";
									actionItem.Menu = "recursive-view-title";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-sm btn-outline"" ng-if=""::canCreate"" ng-click=""manageRelatedRecordItem(null)""><i class=""fa fa-plus""></i> Create & Add</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_edit >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_edit";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""quick edit"" class=""btn btn-sm btn-outline"" ng-click=""manageRelatedRecordItem(recordData)"" ng-if=""::canUpdate""><i class=""fa fa-pencil""></i></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_unrelate >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_unrelate";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""remove relation"" class=""btn btn-sm btn-outline"" confirmed-click=""instantDetachRecord(recordData)"" ng-confirm-click=""Are you sure that you need this relation removed?"" ng-if=""::canRemove""><i class=""fa fa-times go-red""></i></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

							}
							#endregion

							#region << Sidebar >>
							createViewInput.Sidebar = new InputRecordViewSidebar();
							createViewInput.Sidebar.CssClass = "";
							createViewInput.Sidebar.Render = false;
							createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

							#endregion
							{
								var response = entMan.CreateRecordView(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), createViewInput);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: log Updated view: quick_view Message:" + response.Message);
							}
						}
						#endregion

						#region << View  Entity: system_log name: general >>
						{
							var createViewEntity = entMan.ReadEntity(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933")).Object;
							var createViewInput = new InputRecordView();

							#region << details >>
							createViewInput.Id = new Guid("a5df793f-9fb0-4deb-a0a2-afaaf8f2a3ac");
							createViewInput.Type = "General";
							createViewInput.Name = "general";
							createViewInput.Label = "General";
							createViewInput.Title = "General";
							createViewInput.Default = true;
							createViewInput.System = false;
							createViewInput.Weight = Decimal.Parse("10.0");
							createViewInput.CssClass = "";
							createViewInput.IconName = "file-text-o";
							createViewInput.DynamicHtmlTemplate = null;
							createViewInput.DataSourceUrl = null;
							createViewInput.ServiceCode = null;
							#endregion

							#region << regions >>
							createViewInput.Regions = new List<InputRecordViewRegion>();

							#region << Region: header >>
							{
								var viewRegion = new InputRecordViewRegion();
								viewRegion.Name = "header";
								viewRegion.Label = "Header";
								viewRegion.Render = true;
								viewRegion.Weight = Decimal.Parse("10.0");
								viewRegion.CssClass = "";
								viewRegion.Sections = new List<InputRecordViewSection>();

								//Save region
								createViewInput.Regions.Add(viewRegion);
							}
							#endregion

							#endregion

							#region << Relation options >>
							{
								createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
							}
							#endregion

							#region << Action items >>
							{
								createViewInput.ActionItems = new List<ActionItem>();

								#region << action item: wv_record_delete >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_record_delete";
									actionItem.Menu = "page-title-dropdown";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?"" ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')""><i class=""fa fa-trash go-red""></i> Delete Record</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_manage_view >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_manage_view";
									actionItem.Menu = "page-title-dropdown";
									actionItem.Weight = Decimal.Parse("101.0");
									actionItem.Template = @"<a target=""_blank"" ng-href=""{{ngCtrl.getViewManageUrl()}}"" ng-if=""::ngCtrl.userIsAdmin()"">
	<i class=""fa fa-fw fa-cog""></i> Manage View
</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_back_button >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_back_button";
									actionItem.Menu = "sidebar-top";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_add_existing >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_add_existing";
									actionItem.Menu = "recursive-view-title";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-sm btn-outline"" ng-if=""::canAddExisting"" ng-click=""addExistingItem()""><i class=""fa fa-download""></i> Add existing</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_add_new >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_add_new";
									actionItem.Menu = "recursive-view-title";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-sm btn-outline"" ng-if=""::canCreate"" ng-click=""manageRelatedRecordItem(null)""><i class=""fa fa-plus""></i> Create & Add</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_edit >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_edit";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""quick edit"" class=""btn btn-sm btn-outline"" ng-click=""manageRelatedRecordItem(recordData)"" ng-if=""::canUpdate""><i class=""fa fa-pencil""></i></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_view_unrelate >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_view_unrelate";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""remove relation"" class=""btn btn-sm btn-outline"" confirmed-click=""instantDetachRecord(recordData)"" ng-confirm-click=""Are you sure that you need this relation removed?"" ng-if=""::canRemove""><i class=""fa fa-times go-red""></i></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

							}
							#endregion

							#region << Sidebar >>
							createViewInput.Sidebar = new InputRecordViewSidebar();
							createViewInput.Sidebar.CssClass = "";
							createViewInput.Sidebar.Render = false;
							createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

							#endregion
							{
								var response = entMan.CreateRecordView(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), createViewInput);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: log Updated view: general Message:" + response.Message);
							}
						}
						#endregion

						#region << List  Entity: system_log name: general >>
						{
							var createListEntity = entMan.ReadEntity(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933")).Object;
							var createListInput = new InputRecordList();

							#region << details >>
							createListInput.Id = new Guid("089cbb50-13a5-4a9c-a080-7aa52f59b093");
							createListInput.Type = "General";
							createListInput.Name = "general";
							createListInput.Label = "General";
							createListInput.Title = "General";
							createListInput.Weight = Decimal.Parse("10.0");
							createListInput.Default = true;
							createListInput.System = false;
							createListInput.CssClass = null;
							createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
							createListInput.VisibleColumnsCount = Int32.Parse("5");
							createListInput.ColumnWidthsCSV = null;
							createListInput.PageSize = Int32.Parse("10");
							createListInput.DynamicHtmlTemplate = null;
							createListInput.DataSourceUrl = null;
							createListInput.ServiceCode = null;
							#endregion

							#region << Relation options >>
							{
								createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
							}
							#endregion

							#region << Action items >>
							{
								createListInput.ActionItems = new List<ActionItem>();

								#region << action item: wv_create_record >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_create_record";
									actionItem.Menu = "page-title";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_import_records >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_import_records";
									actionItem.Menu = "page-title-dropdown";
									actionItem.Weight = Decimal.Parse("10.0");
									actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')""><i class=""fa fa-fw fa-upload""></i> Import CSV</a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_export_records >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_export_records";
									actionItem.Menu = "page-title-dropdown";
									actionItem.Weight = Decimal.Parse("11.0");
									actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')""><i class=""fa fa-fw fa-download""></i> Export CSV</a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_record_details >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_record_details";
									actionItem.Menu = "record-row";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}""><i class=""fa fa-fw fa-eye""></i></a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_list_add_existing >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_list_add_existing";
									actionItem.Menu = "recursive-list-title";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-outline btn-sm"" ng-if=""::canAddExisting"" ng-click=""addExistingItem()""><i class=""fa fa-download""></i> Add existing</a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_list_add_new >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_list_add_new";
									actionItem.Menu = "recursive-list-title";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-outline btn-sm"" ng-if=""::canCreate"" ng-click=""manageRelatedRecordItem(null)""><i class=""fa fa-plus""></i> Create & Add</a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_list_view >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_list_view";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""quick view this record"" class=""btn btn-sm btn-outline"" ng-click=""viewRelatedRecordItem(record)""><i class=""fa fa-eye""></i></a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_list_edit >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_list_edit";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""quick edit this record"" class=""btn btn-sm btn-outline"" ng-click=""manageRelatedRecordItem(record)"" ng-if=""::canUpdate""><i class=""fa fa-pencil""></i></a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_list_unrelate >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_list_unrelate";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("3.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""Detach records relation"" class=""btn btn-sm btn-outline"" confirmed-click=""instantDetachRecord(record)"" ng-confirm-click=""Are you sure that you need this relation removed?"" ng-if=""::canRemove""><i class=""fa fa-times go-red""></i></a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

							}
							#endregion

							#region << Columns >>
							{
								createListInput.Columns = new List<InputRecordListItemBase>();

							}
							#endregion

							#region << Query >>
							{
								createListInput.Query = null;
							}
							#endregion

							#region << Sorts >>
							{
								createListInput.Sorts = new List<InputRecordListSort>();

							}
							#endregion

							{
								var response = entMan.CreateRecordList(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), createListInput);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: log Created list: general Message:" + response.Message);
							}
						}
						#endregion

						#region << List  Entity: system_log name: lookup >>
						{
							var createListEntity = entMan.ReadEntity(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933")).Object;
							var createListInput = new InputRecordList();

							#region << details >>
							createListInput.Id = new Guid("afe4ff81-654e-4a66-8972-31aff8b556d3");
							createListInput.Type = "Lookup";
							createListInput.Name = "lookup";
							createListInput.Label = "Lookup";
							createListInput.Title = "Lookup";
							createListInput.Weight = Decimal.Parse("10.0");
							createListInput.Default = true;
							createListInput.System = false;
							createListInput.CssClass = null;
							createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
							createListInput.VisibleColumnsCount = Int32.Parse("5");
							createListInput.ColumnWidthsCSV = null;
							createListInput.PageSize = Int32.Parse("10");
							createListInput.DynamicHtmlTemplate = null;
							createListInput.DataSourceUrl = null;
							createListInput.ServiceCode = null;
							#endregion

							#region << Relation options >>
							{
								createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
							}
							#endregion

							#region << Action items >>
							{
								createListInput.ActionItems = new List<ActionItem>();

								#region << action item: wv_create_record >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_create_record";
									actionItem.Menu = "page-title";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_import_records >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_import_records";
									actionItem.Menu = "page-title-dropdown";
									actionItem.Weight = Decimal.Parse("10.0");
									actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')""><i class=""fa fa-fw fa-upload""></i> Import CSV</a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_export_records >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_export_records";
									actionItem.Menu = "page-title-dropdown";
									actionItem.Weight = Decimal.Parse("11.0");
									actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')""><i class=""fa fa-fw fa-download""></i> Export CSV</a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_record_details >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_record_details";
									actionItem.Menu = "record-row";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}""><i class=""fa fa-fw fa-eye""></i></a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_list_add_existing >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_list_add_existing";
									actionItem.Menu = "recursive-list-title";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-outline btn-sm"" ng-if=""::canAddExisting"" ng-click=""addExistingItem()""><i class=""fa fa-download""></i> Add existing</a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_list_add_new >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_list_add_new";
									actionItem.Menu = "recursive-list-title";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-outline btn-sm"" ng-if=""::canCreate"" ng-click=""manageRelatedRecordItem(null)""><i class=""fa fa-plus""></i> Create & Add</a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_list_view >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_list_view";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("1.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""quick view this record"" class=""btn btn-sm btn-outline"" ng-click=""viewRelatedRecordItem(record)""><i class=""fa fa-eye""></i></a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_list_edit >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_list_edit";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("2.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""quick edit this record"" class=""btn btn-sm btn-outline"" ng-click=""manageRelatedRecordItem(record)"" ng-if=""::canUpdate""><i class=""fa fa-pencil""></i></a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << action item: wv_recursive_list_unrelate >>
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_recursive_list_unrelate";
									actionItem.Menu = "recursive-list-record-row";
									actionItem.Weight = Decimal.Parse("3.0");
									actionItem.Template = @"<a href=""javascript:void(0)"" title=""Detach records relation"" class=""btn btn-sm btn-outline"" confirmed-click=""instantDetachRecord(record)"" ng-confirm-click=""Are you sure that you need this relation removed?"" ng-if=""::canRemove""><i class=""fa fa-times go-red""></i></a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

							}
							#endregion

							#region << Columns >>
							{
								createListInput.Columns = new List<InputRecordListItemBase>();

							}
							#endregion

							#region << Query >>
							{
								createListInput.Query = null;
							}
							#endregion

							#region << Sorts >>
							{
								createListInput.Sorts = new List<InputRecordListSort>();

							}
							#endregion

							{
								var response = entMan.CreateRecordList(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), createListInput);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: log Created list: lookup Message:" + response.Message);
							}
						}
						#endregion
					}

					if (currentVersion < 20170425)
					{
						systemSettings.Version = 20170425;

						#region << ***Update field***  Entity: system_log Field Name: notification_status >>
						{
							var currentEntity = entMan.ReadEntity(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933")).Object;
							InputSelectField dropdownField = new InputSelectField();
							dropdownField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "notification_status").Id;
							dropdownField.Name = "notification_status";
							dropdownField.Label = "Notification Status";
							dropdownField.PlaceholderText = "";
							dropdownField.Description = "";
							dropdownField.HelpText = "";
							dropdownField.Required = true;
							dropdownField.Unique = false;
							dropdownField.Searchable = true;
							dropdownField.Auditable = false;
							dropdownField.System = true;
							dropdownField.DefaultValue = "1";
							dropdownField.Options = new List<SelectFieldOption>
							{
								new SelectFieldOption() { Key = "1", Value = "Do not notify"},
								new SelectFieldOption() { Key = "2", Value = "Not notified"},
								new SelectFieldOption() { Key = "3", Value = "Notified"},
								new SelectFieldOption() { Key = "4", Value = "Notification failed"}
							};
							dropdownField.EnableSecurity = false;
							dropdownField.Permissions = new FieldPermissions();
							dropdownField.Permissions.CanRead = new List<Guid>();
							dropdownField.Permissions.CanUpdate = new List<Guid>();
							//READ
							//UPDATE
							{
								var response = entMan.UpdateField(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), dropdownField);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: system_log Field: notification_status Message:" + response.Message);
							}
						}
						#endregion

						#region << ***Update field***  Entity: system_log Field Name: type >>
						{
							var currentEntity = entMan.ReadEntity(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933")).Object;
							InputSelectField dropdownField = new InputSelectField();
							dropdownField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "type").Id;
							dropdownField.Name = "type";
							dropdownField.Label = "Type";
							dropdownField.PlaceholderText = "";
							dropdownField.Description = "";
							dropdownField.HelpText = "";
							dropdownField.Required = true;
							dropdownField.Unique = false;
							dropdownField.Searchable = true;
							dropdownField.Auditable = false;
							dropdownField.System = true;
							dropdownField.DefaultValue = "1";
							dropdownField.Options = new List<SelectFieldOption>
							{
								new SelectFieldOption() { Key = "1", Value = "error"},
								new SelectFieldOption() { Key = "2", Value = "info"}
							};
							dropdownField.EnableSecurity = false;
							dropdownField.Permissions = new FieldPermissions();
							dropdownField.Permissions.CanRead = new List<Guid>();
							dropdownField.Permissions.CanUpdate = new List<Guid>();
							//READ
							//UPDATE
							{
								var response = entMan.UpdateField(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), dropdownField);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: system_log Field: type Message:" + response.Message);
							}
						}
						#endregion

						#region << ***Update field***  Entity: system_log Field Name: created_on >>
						{
							var currentEntity = entMan.ReadEntity(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933")).Object;
							InputDateTimeField datetimeField = new InputDateTimeField();
							datetimeField.Id =  currentEntity.Fields.SingleOrDefault(x => x.Name == "created_on").Id;
							datetimeField.Name = "created_on";
							datetimeField.Label = "Created On";
							datetimeField.PlaceholderText = "";
							datetimeField.Description = "";
							datetimeField.HelpText = "";
							datetimeField.Required = false;
							datetimeField.Unique = false;
							datetimeField.Searchable = true;
							datetimeField.Auditable = false;
							datetimeField.System = true;
							datetimeField.DefaultValue = null;
							datetimeField.Format = "dd MMM yyyy HH:mm";
							datetimeField.UseCurrentTimeAsDefaultValue = true;
							datetimeField.EnableSecurity = false;
							datetimeField.Permissions = new FieldPermissions();
							datetimeField.Permissions.CanRead = new List<Guid>();
							datetimeField.Permissions.CanUpdate = new List<Guid>();
							//READ
							//UPDATE
							{
								var response = entMan.UpdateField(new Guid("8ded30ad-32d5-4d7f-937f-9ed380a30933"), datetimeField);
								if (!response.Success)
									throw new Exception("System error 10060. Entity: system_log Field: created_on Message:" + response.Message);
							}
						}
						#endregion

					}

					if (currentVersion < 20170924)
					{
						systemSettings.Version = 20170924;

							#region << ***Create entity*** Entity name: user_file >>
							{
								#region << entity >>
								{
									var entity = new InputEntity();
									var systemFieldIdDictionary = new Dictionary<string,Guid>();
									systemFieldIdDictionary["id"] = new Guid("3df98e7c-af26-47a4-8f6c-cdbbf752d147");
									systemFieldIdDictionary["created_on"] = new Guid("7bc7c1a2-93aa-40bc-8374-fd350cdc7fac");
									systemFieldIdDictionary["created_by"] = new Guid("5637340f-0c1b-445f-9890-1715a02d8fea");
									systemFieldIdDictionary["last_modified_on"] = new Guid("e456bbf8-e609-4405-b420-75424ad6112b");
									systemFieldIdDictionary["last_modified_by"] = new Guid("6fefe57b-a162-4ab6-a9ce-905a4f990b29");
									systemFieldIdDictionary["user_user_file_created_by"] = new Guid("909aa55e-300b-40f0-8afd-f01c4d223ea5");
									systemFieldIdDictionary["user_user_file_modified_by"] = new Guid("19d03f1c-52cf-41c8-9d29-1ec8061f7db9");
									entity.Id = new Guid("5c666c54-9e76-4327-ac7a-55851037810c");
									entity.Name = "user_file";
									entity.Label = "User File";
									entity.LabelPlural = "User Files";
									entity.System = true;
									entity.IconName = "folder";
									entity.Weight = (decimal)100.0;
									entity.RecordPermissions = new RecordPermissions();
									entity.RecordPermissions.CanCreate = new List<Guid>();
									entity.RecordPermissions.CanRead = new List<Guid>();
									entity.RecordPermissions.CanUpdate = new List<Guid>();
									entity.RecordPermissions.CanDelete = new List<Guid>();
									//Create
									entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
									entity.RecordPermissions.CanCreate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
									//READ
									entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
									entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
									//UPDATE
									entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
									entity.RecordPermissions.CanUpdate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
									//DELETE
									entity.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
									entity.RecordPermissions.CanDelete.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
									{
										var response = entMan.CreateEntity(entity, false, false,systemFieldIdDictionary);
										if (!response.Success)
											throw new Exception("System error 10050. Entity: user_file creation Message: " + response.Message);
									}
								}
								#endregion
							}
							#endregion

							#region << ***Create field***  Entity: user_file Field Name: alt >>
							{
								InputTextField textboxField = new InputTextField();
								textboxField.Id = new Guid("168a9777-a156-4b0b-9b18-909fec043ce5");
								textboxField.Name = "alt";
								textboxField.Label = "Alt";
								textboxField.PlaceholderText = "";
								textboxField.Description = "";
								textboxField.HelpText = "";
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
									var response = entMan.CreateField(new Guid("5c666c54-9e76-4327-ac7a-55851037810c"), textboxField, false);
									if (!response.Success)
										throw new Exception("System error 10060. Entity: user_file Field: alt Message:" + response.Message);
								}
							}
							#endregion

							#region << ***Create field***  Entity: user_file Field Name: caption >>
							{
								InputTextField textboxField = new InputTextField();
								textboxField.Id = new Guid("6796c578-22f4-4b07-8568-99f9d6600294");
								textboxField.Name = "caption";
								textboxField.Label = "Caption";
								textboxField.PlaceholderText = "";
								textboxField.Description = "";
								textboxField.HelpText = "";
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
									var response = entMan.CreateField(new Guid("5c666c54-9e76-4327-ac7a-55851037810c"), textboxField, false);
									if (!response.Success)
										throw new Exception("System error 10060. Entity: user_file Field: caption Message:" + response.Message);
								}
							}
							#endregion

							#region << ***Create field***  Entity: user_file Field Name: height >>
							{
								InputNumberField numberField = new InputNumberField();
								numberField.Id = new Guid("a7a06f28-5893-4890-a8a7-fd794c741cf9");
								numberField.Name = "height";
								numberField.Label = "Height";
								numberField.PlaceholderText = "";
								numberField.Description = "";
								numberField.HelpText = "";
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
									var response = entMan.CreateField(new Guid("5c666c54-9e76-4327-ac7a-55851037810c"), numberField, false);
									if (!response.Success)
										throw new Exception("System error 10060. Entity: user_file Field: height Message:" + response.Message);
								}
							}
							#endregion

							#region << ***Create field***  Entity: user_file Field Name: name >>
							{
								InputTextField textboxField = new InputTextField();
								textboxField.Id = new Guid("cc2730d3-7711-4d8a-bdc2-1d11c3eae5c2");
								textboxField.Name = "name";
								textboxField.Label = "Name";
								textboxField.PlaceholderText = "";
								textboxField.Description = "";
								textboxField.HelpText = "";
								textboxField.Required = true;
								textboxField.Unique = false;
								textboxField.Searchable = true;
								textboxField.Auditable = false;
								textboxField.System = true;
								textboxField.DefaultValue = "file-name";
								textboxField.MaxLength = null;
								textboxField.EnableSecurity = false;
								textboxField.Permissions = new FieldPermissions();
								textboxField.Permissions.CanRead = new List<Guid>();
								textboxField.Permissions.CanUpdate = new List<Guid>();
								//READ
								//UPDATE
								{
									var response = entMan.CreateField(new Guid("5c666c54-9e76-4327-ac7a-55851037810c"), textboxField, false);
									if (!response.Success)
										throw new Exception("System error 10060. Entity: user_file Field: name Message:" + response.Message);
								}
							}
							#endregion

							#region << ***Create field***  Entity: user_file Field Name: size >>
							{
								InputNumberField numberField = new InputNumberField();
								numberField.Id = new Guid("6a66fbd8-fb5a-4e48-882f-b760475bf2f0");
								numberField.Name = "size";
								numberField.Label = "Size";
								numberField.PlaceholderText = "";
								numberField.Description = "";
								numberField.HelpText = "";
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
									var response = entMan.CreateField(new Guid("5c666c54-9e76-4327-ac7a-55851037810c"), numberField, false);
									if (!response.Success)
										throw new Exception("System error 10060. Entity: user_file Field: size Message:" + response.Message);
								}
							}
							#endregion

							#region << ***Create field***  Entity: user_file Field Name: type >>
							{
								InputSelectField dropdownField = new InputSelectField();
								dropdownField.Id = new Guid("e856b229-ab8c-440c-8b6d-f817cc2776f0");
								dropdownField.Name = "type";
								dropdownField.Label = "Type";
								dropdownField.PlaceholderText = "";
								dropdownField.Description = "";
								dropdownField.HelpText = "";
								dropdownField.Required = true;
								dropdownField.Unique = false;
								dropdownField.Searchable = true;
								dropdownField.Auditable = false;
								dropdownField.System = true;
								dropdownField.DefaultValue = "image";
								dropdownField.Options = new List<SelectFieldOption>
								{
									new SelectFieldOption() { Key = "image", Value = "image"},
									new SelectFieldOption() { Key = "document", Value = "document"},
									new SelectFieldOption() { Key = "audio", Value = "audio"},
									new SelectFieldOption() { Key = "video", Value = "video"},
									new SelectFieldOption() { Key = "other", Value = "other"}
								};
								dropdownField.EnableSecurity = false;
								dropdownField.Permissions = new FieldPermissions();
								dropdownField.Permissions.CanRead = new List<Guid>();
								dropdownField.Permissions.CanUpdate = new List<Guid>();
								//READ
								//UPDATE
								{
									var response = entMan.CreateField(new Guid("5c666c54-9e76-4327-ac7a-55851037810c"), dropdownField, false);
									if (!response.Success)
										throw new Exception("System error 10060. Entity: user_file Field: type Message:" + response.Message);
								}
							}
							#endregion

							#region << ***Create field***  Entity: user_file Field Name: width >>
							{
								InputNumberField numberField = new InputNumberField();
								numberField.Id = new Guid("c2b8fee6-81a4-4cb0-adac-f19f734f6380");
								numberField.Name = "width";
								numberField.Label = "Width";
								numberField.PlaceholderText = "";
								numberField.Description = "";
								numberField.HelpText = "";
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
									var response = entMan.CreateField(new Guid("5c666c54-9e76-4327-ac7a-55851037810c"), numberField, false);
									if (!response.Success)
										throw new Exception("System error 10060. Entity: user_file Field: width Message:" + response.Message);
								}
							}
							#endregion

							#region << ***Create field***  Entity: user_file Field Name: path >>
							{
								InputFileField fileField = new InputFileField();
								fileField.Id = new Guid("3f4e8056-6e94-4304-8fd7-8f151c81bc19");
								fileField.Name = "path";
								fileField.Label = "File";
								fileField.PlaceholderText = "";
								fileField.Description = "";
								fileField.HelpText = "";
								fileField.Required = true;
								fileField.Unique = true;
								fileField.Searchable = true;
								fileField.Auditable = false;
								fileField.System = false;
								fileField.DefaultValue ="no-file-error.txt";
								fileField.EnableSecurity = false;
								fileField.Permissions = new FieldPermissions();
								fileField.Permissions.CanRead = new List<Guid>();
								fileField.Permissions.CanUpdate = new List<Guid>();
								//READ
								//UPDATE
								{
									var response = entMan.CreateField(new Guid("5c666c54-9e76-4327-ac7a-55851037810c"), fileField, false);
									if (!response.Success)
										throw new Exception("System error 10060. Entity: user_file Field: path Message:" + response.Message);
								}
							}
							#endregion

					}

					new DbSystemSettingsRepository().Save(new DbSystemSettings { Id = systemSettings.Id, Version = systemSettings.Version });

					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					var exception = ex;
					connection.RollbackTransaction();
					throw;
				}

			}

			//recMan.ConvertNtoNRelations();
		}

		public void InitializeBackgroundJobs()
		{
			JobManagerSettings settings = new JobManagerSettings();
			settings.DbConnectionString = Settings.ConnectionString;
			settings.Enabled = Settings.EnableBackgroungJobs;

			JobManager.Initialize(settings);
			ScheduleManager.Initialize(settings);
		}

		public void StartBackgroundJobProcess()
		{
			JobManager.Current.ProcessJobsAsync();
			ScheduleManager.Current.ProcessSchedulesAsync();
		}

		private void CheckCreateSystemTables()
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				bool entitiesTableExists = false;
				var command = connection.CreateCommand("SELECT EXISTS (  SELECT 1 FROM   information_schema.tables  WHERE  table_schema = 'public' AND table_name = 'entities' ) ");
				using (var reader = command.ExecuteReader())
				{
					reader.Read();
					entitiesTableExists = reader.GetBoolean(0);
					reader.Close();
				}

				if (!entitiesTableExists)
				{
					command = connection.CreateCommand("CREATE TABLE public.entities(  id uuid NOT NULL, \"json\"  json NOT NULL,  CONSTRAINT entities_pkey	PRIMARY KEY (id)) WITH(	OIDS = FALSE  )");
					command.ExecuteNonQuery();
				}

				bool relationsTableExists = false;
				command = connection.CreateCommand("SELECT EXISTS (  SELECT 1 FROM   information_schema.tables  WHERE  table_schema = 'public' AND table_name = 'entity_relations' ) ");
				using (var reader = command.ExecuteReader())
				{
					reader.Read();
					relationsTableExists = reader.GetBoolean(0);
					reader.Close();
				}

				if (!relationsTableExists)
				{
					command = connection.CreateCommand("CREATE TABLE public.entity_relations(  id uuid NOT NULL, \"json\"  json NOT NULL,  CONSTRAINT entity_relations_pkey	PRIMARY KEY (id)) WITH(	OIDS = FALSE  )");
					command.ExecuteNonQuery();
				}


				bool settingsTableExists = false;
				command = connection.CreateCommand("SELECT EXISTS (  SELECT 1 FROM   information_schema.tables  WHERE  table_schema = 'public' AND table_name = 'system_settings' ) ");
				using (var reader = command.ExecuteReader())
				{
					reader.Read();
					settingsTableExists = reader.GetBoolean(0);
					reader.Close();
				}

				if (!settingsTableExists)
				{
					command = connection.CreateCommand("CREATE TABLE public.system_settings (  id uuid NOT NULL,  version  integer NOT NULL, CONSTRAINT system_settings_pkey	PRIMARY KEY(id)) WITH(	OIDS = FALSE  )");
					command.ExecuteNonQuery();
				}

				bool filesTableExists = false;
				command = connection.CreateCommand("SELECT EXISTS (  SELECT 1 FROM   information_schema.tables  WHERE  table_schema = 'public' AND table_name = 'files' ) ");
				using (var reader = command.ExecuteReader())
				{
					reader.Read();
					filesTableExists = reader.GetBoolean(0);
					reader.Close();
				}

				if (!filesTableExists)
				{
					const string filesTableSql = @"CREATE TABLE public.files (
					  id           uuid NOT NULL,
					  object_id    numeric(18) NOT NULL,
					  filepath     text NOT NULL,
					  created_on   timestamp WITHOUT TIME ZONE NOT NULL,
					  modified_on  timestamp WITHOUT TIME ZONE NOT NULL,
					  created_by   uuid,
					  modified_by  uuid,
					  /* Keys */
					  CONSTRAINT files_pkey
						PRIMARY KEY (id), 
					  CONSTRAINT udx_filepath
						UNIQUE (filepath), 
					  CONSTRAINT udx_object_id
						UNIQUE (object_id)
					) WITH (
						OIDS = FALSE
					  )";

					command = connection.CreateCommand(filesTableSql);
					command.ExecuteNonQuery();

					DbRepository.CreateIndex("idx_filepath", "files", "filepath", true);
				}

				//drop unique constraint for object id - to support FS storage (object id is 0 for all files stored on file system)
				if (!filesTableExists)
				{
					DbRepository.DropUniqueConstraint("udx_object_id", "files");
				}


				bool jobsTableExists = false;
				command = connection.CreateCommand("SELECT EXISTS (  SELECT 1 FROM   information_schema.tables  WHERE  table_schema = 'public' AND table_name = 'jobs' ) ");
				using (var reader = command.ExecuteReader())
				{
					reader.Read();
					jobsTableExists = reader.GetBoolean(0);
					reader.Close();
				}

				if (!jobsTableExists)
				{
					const string jobTableSql = @"CREATE TABLE public.jobs (
					  id	uuid NOT NULL,
					  type_id	uuid NOT NULL,
					  type_name text NOT NULL,
					  assembly text NOT NULL,
					  complete_class_name text NOT NULL,
					  method_name text NOT NULL,
					  attributes text,
					  status	integer NOT NULL,
					  priority	integer NOT NULL,
					  started_on	timestamp WITH TIME ZONE,
					  finished_on	timestamp WITH TIME ZONE,
					  aborted_by	uuid,
					  canceled_by	uuid,
					  error_message text,
					  schedule_plan_id	uuid,
					  created_on   timestamp WITH TIME ZONE NOT NULL,
					  last_modified_on  timestamp WITH TIME ZONE NOT NULL,
					  created_by   uuid,
					  last_modified_by  uuid,
					  /* Keys */
					  CONSTRAINT jobs_pkey
						PRIMARY KEY (id)
					) WITH (
						OIDS = FALSE
					  )";

					command = connection.CreateCommand(jobTableSql);
					command.ExecuteNonQuery();
				}

				bool schedulePlanTableExists = false;
				command = connection.CreateCommand("SELECT EXISTS (  SELECT 1 FROM   information_schema.tables  WHERE  table_schema = 'public' AND table_name = 'schedule_plan' ) ");
				using (var reader = command.ExecuteReader())
				{
					reader.Read();
					schedulePlanTableExists = reader.GetBoolean(0);
					reader.Close();
				}

				if (!schedulePlanTableExists)
				{
					const string schedulePlanTableSql = @"CREATE TABLE public.schedule_plan (
					  id	uuid NOT NULL,
					  name text NOT NULL,
					  type	integer NOT NULL,
					  start_date	timestamp WITH TIME ZONE,
					  end_date	timestamp WITH TIME ZONE,
					  schedule_days json,
					  interval_in_minutes integer,
					  start_timespan	integer,
					  end_timespan	integer,
					  last_trigger_time	timestamp WITH TIME ZONE,
					  next_trigger_time	timestamp WITH TIME ZONE,
					  job_type_id	uuid NOT NULL,
					  job_attributes text,
					  enabled	boolean NOT NULL,
					  last_started_job_id	uuid,
					  created_on   timestamp WITH TIME ZONE NOT NULL,
					  last_modified_on  timestamp WITH TIME ZONE NOT NULL,
					  last_modified_by  uuid,
					  /* Keys */
					  CONSTRAINT schedule_plan_pkey
						PRIMARY KEY (id)
					) WITH (
						OIDS = FALSE
					  )";

					command = connection.CreateCommand(schedulePlanTableSql);
					command.ExecuteNonQuery();
				}

				//added result column into system table jobs
				bool jobsResultColumnExists = false;
				command = connection.CreateCommand("SELECT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema='public' AND table_name='jobs' AND column_name='result')");
				using (var reader = command.ExecuteReader())
				{
					reader.Read();
					jobsResultColumnExists = reader.GetBoolean(0);
					reader.Close();
				}

				if( !jobsResultColumnExists)
				{
					command = connection.CreateCommand("ALTER TABLE public.jobs  ADD COLUMN result text");
					command.ExecuteNonQuery();
				}
			}
		}

		#region << tests >>

		private void EntityTests()
		{
			Debug.WriteLine("==== START ENTITY TESTS====");

			var entityManager = new EntityManager();

			InputEntity inputEntity = new InputEntity();
			//entity.Id = new Guid("C5050AC8-5967-4CE1-95E7-A79B054F9D14");
			inputEntity.Id = Guid.NewGuid();
			inputEntity.Name = "goro_test";
			inputEntity.Label = "Goro Test";
			inputEntity.LabelPlural = "Goro Tests";
			inputEntity.System = true;

			List<Guid> allowedRoles = new List<Guid>();
			allowedRoles.Add(new Guid("F42EBA3B-6433-752B-6C34-B322A7B4CE7D"));
			inputEntity.RecordPermissions = new RecordPermissions();
			inputEntity.RecordPermissions.CanRead = allowedRoles;
			inputEntity.RecordPermissions.CanCreate = allowedRoles;
			inputEntity.RecordPermissions.CanUpdate = allowedRoles;
			inputEntity.RecordPermissions.CanDelete = allowedRoles;

			try
			{
				Entity entity = inputEntity.MapTo<Entity>();

				EntityResponse response = entityManager.CreateEntity(inputEntity);

				InputTextField field = new InputTextField();
				field.Id = Guid.NewGuid();
				field.Name = "text_field";
				field.Label = "Text field";
				field.PlaceholderText = "Text field placeholder text";
				field.Description = "Text field description";
				field.HelpText = "Text field help text";
				field.Required = true;
				field.Unique = true;
				field.Searchable = true;
				field.Auditable = true;
				field.System = true;
				field.DefaultValue = "";

				field.MaxLength = 200;

				FieldResponse fieldResponse = entityManager.CreateField(entity.Id, field, false);

				//inputEntity.Label = "GoroTest_edited";
				//inputEntity.PluralLabel = "Goro Tests - edited";

				//Expando obj = new Expando();
				//obj["Label"] = "GoroTest_edited";
				//obj["PluralLabel"] = "Goro Tests - edited";

				//response = entityManager.PartialUpdateEntity(entity.Id.Value, obj);

				//field.Label = "TextField_edited";

				InputField fieldObj = new InputTextField();
				fieldObj.Label = "TextField_edited";

				//fieldResponse = entityManager.PartialUpdateField(entity.Id, field.Id.Value, fieldObj);

				//fieldResponse = entityManager.DeleteField(entity.Id.Value, field.Id.Value);

				//List<Field> fields = CreateTestFieldCollection(entity);
				////FieldResponse fieldResponse = entityManager.CreateField(entity.Id.Value, fields[0]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[1]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[2]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[3]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[4]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[5]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[6]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[7]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[8]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[9]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[10]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[11]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[12]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[13]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[14]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[15]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[16]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[17]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[18]);
				//fieldResponse = entityManager.CreateField(entity.Id.Value, fields[19]);

				//EntityResponse entityResponse = entityManager.ReadEntity(entity.Id.Value);
				//entity = entityResponse.Object;

				//List<RecordsList> recordsLists = CreateTestViewCollection(entity);

				//RecordsListResponse recordsListsResponse = entityManager.CreateRecordsList(entity.Id.Value, recordsLists[0]);

				//recordsLists[0].Label = "Edited View";

				//recordsListsResponse = entityManager.UpdateRecordsList(entity.Id.Value, recordsLists[0]);

				//List<RecordView> recordViewList = CreateTestFormCollection(entity);

				//RecordViewResponse recordViewResponse = entityManager.CreateRecordView(entity.Id.Value, recordViewList[0]);

				//recordViewList[0].Label = "Edited Form";

				//recordViewResponse = entityManager.CreateRecordView(entity.Id.Value, recordViewList[0]);

				EntityListResponse entityListResponse = entityManager.ReadEntities();

				EntityResponse resultEntity = entityManager.ReadEntity(entity.Id);

				response = entityManager.DeleteEntity(entity.Id);

			}
			catch (StorageException e)
			{
				Debug.WriteLine(e);
			}

			Debug.WriteLine("==== END ENTITY TESTS====");
		}

		private List<Field> CreateTestFieldCollection(Entity entity)
		{
			List<Field> fields = new List<Field>();

			AutoNumberField autoNumberField = new AutoNumberField();

			autoNumberField.Id = Guid.NewGuid();
			autoNumberField.Name = "AutoNumberField";
			autoNumberField.Label = "AutoNumber field";
			autoNumberField.PlaceholderText = "AutoNumber field placeholder text";
			autoNumberField.Description = "AutoNumber field description";
			autoNumberField.HelpText = "AutoNumber field help text";
			autoNumberField.Required = true;
			autoNumberField.Unique = true;
			autoNumberField.Searchable = true;
			autoNumberField.Auditable = true;
			autoNumberField.System = true;
			autoNumberField.DefaultValue = 0;

			autoNumberField.DisplayFormat = "A{0000}";
			autoNumberField.StartingNumber = 10;

			fields.Add(autoNumberField);

			CheckboxField checkboxField = new CheckboxField();

			checkboxField.Id = Guid.NewGuid();
			checkboxField.Name = "CheckboxField";
			checkboxField.Label = "Checkbox field";
			checkboxField.PlaceholderText = "Checkbox field placeholder text";
			checkboxField.Description = "Checkbox field description";
			checkboxField.HelpText = "Checkbox field help text";
			checkboxField.Required = true;
			checkboxField.Unique = true;
			checkboxField.Searchable = true;
			checkboxField.Auditable = true;
			checkboxField.System = true;
			checkboxField.DefaultValue = false;

			fields.Add(checkboxField);

			CurrencyField currencyField = new CurrencyField();

			currencyField.Id = Guid.NewGuid();
			currencyField.Name = "CurrencyField";
			currencyField.Label = "Currency field";
			currencyField.PlaceholderText = "Currency field placeholder text";
			currencyField.Description = "Currency field description";
			currencyField.HelpText = "Currency field help text";
			currencyField.Required = true;
			currencyField.Unique = true;
			currencyField.Searchable = true;
			currencyField.Auditable = true;
			currencyField.System = true;
			currencyField.DefaultValue = 0;

			currencyField.MinValue = 1;
			currencyField.MaxValue = 35;
			currencyField.Currency = new CurrencyType();
			currencyField.Currency.Name = "US Dollar";
			currencyField.Currency.NamePlural = "US Dollars";
			currencyField.Currency.Symbol = "$";
			currencyField.Currency.SymbolNative = "$";
			currencyField.Currency.SymbolPlacement = CurrencySymbolPlacement.Before;
			currencyField.Currency.Code = "USD";
			currencyField.Currency.DecimalDigits = 2;
			currencyField.Currency.Rounding = 0;

			fields.Add(currencyField);

			DateField dateField = new DateField();

			dateField.Id = Guid.NewGuid();
			dateField.Name = "DateField";
			dateField.Label = "Date field";
			dateField.PlaceholderText = "Date field placeholder text";
			dateField.Description = "Date field description";
			dateField.HelpText = "Date field help text";
			dateField.Required = true;
			dateField.Unique = true;
			dateField.Searchable = true;
			dateField.Auditable = true;
			dateField.System = true;
			dateField.DefaultValue = DateTime.MinValue;

			dateField.Format = "dd MMM yyyy";

			fields.Add(dateField);

			DateTimeField dateTimeField = new DateTimeField();

			dateTimeField.Id = Guid.NewGuid();
			dateTimeField.Name = "DateTimeField";
			dateTimeField.Label = "DateTime field";
			dateTimeField.PlaceholderText = "DateTime field placeholder text";
			dateTimeField.Description = "DateTime field description";
			dateTimeField.HelpText = "DateTime field help text";
			dateTimeField.Required = true;
			dateTimeField.Unique = true;
			dateTimeField.Searchable = true;
			dateTimeField.Auditable = true;
			dateTimeField.System = true;
			dateTimeField.DefaultValue = DateTime.MinValue;

			dateTimeField.Format = "dd MMM yyyy HH:mm";

			fields.Add(dateTimeField);

			EmailField emailField = new EmailField();

			emailField.Id = Guid.NewGuid();
			emailField.Name = "EmailField";
			emailField.Label = "Email field";
			emailField.PlaceholderText = "Email field placeholder text";
			emailField.Description = "Email field description";
			emailField.HelpText = "Email field help text";
			emailField.Required = true;
			emailField.Unique = true;
			emailField.Searchable = true;
			emailField.Auditable = true;
			emailField.System = true;
			emailField.DefaultValue = "";

			emailField.MaxLength = 255;

			fields.Add(emailField);

			FileField fileField = new FileField();

			fileField.Id = Guid.NewGuid();
			fileField.Name = "FileField";
			fileField.Label = "File field";
			fileField.PlaceholderText = "File field placeholder text";
			fileField.Description = "File field description";
			fileField.HelpText = "File field help text";
			fileField.Required = true;
			fileField.Unique = true;
			fileField.Searchable = true;
			fileField.Auditable = true;
			fileField.System = true;
			fileField.DefaultValue = "";

			fields.Add(fileField);

			//FormulaField formulaField = new FormulaField();

			//formulaField.Id = Guid.NewGuid();
			//formulaField.Name = "Formula field";
			//formulaField.Label = "Formula field";
			//formulaField.PlaceholderText = "Formula field placeholder text";
			//formulaField.Description = "Formula field description";
			//formulaField.HelpText = "Formula field help text";
			//formulaField.Required = true;
			//formulaField.Unique = true;
			//formulaField.Searchable = true;
			//formulaField.Auditable = true;
			//formulaField.System = true;

			//formulaField.ReturnType = Api.FormulaFieldReturnType.Number;
			//formulaField.FormulaText = "2 + 5";
			//formulaField.DecimalPlaces = 2;

			//fields.Add(formulaField);

			HtmlField htmlField = new HtmlField();

			htmlField.Id = Guid.NewGuid();
			htmlField.Name = "HtmlField";
			htmlField.Label = "Html field";
			htmlField.PlaceholderText = "Html field placeholder text";
			htmlField.Description = "Html field description";
			htmlField.HelpText = "Html field help text";
			htmlField.Required = true;
			htmlField.Unique = true;
			htmlField.Searchable = true;
			htmlField.Auditable = true;
			htmlField.System = true;
			htmlField.DefaultValue = "";

			fields.Add(htmlField);

			ImageField imageField = new ImageField();

			imageField.Id = Guid.NewGuid();
			imageField.Name = "ImageField";
			imageField.Label = "Image field";
			imageField.PlaceholderText = "Image field placeholder text";
			imageField.Description = "Image field description";
			imageField.HelpText = "Image field help text";
			imageField.Required = true;
			imageField.Unique = true;
			imageField.Searchable = true;
			imageField.Auditable = true;
			imageField.System = true;
			imageField.DefaultValue = "";

			fields.Add(imageField);

			MultiLineTextField multiLineTextField = new MultiLineTextField();

			multiLineTextField.Id = Guid.NewGuid();
			multiLineTextField.Name = "MultiLineTextField";
			multiLineTextField.Label = "MultiLineText field";
			multiLineTextField.PlaceholderText = "MultiLineText field placeholder text";
			multiLineTextField.Description = "MultiLineText field description";
			multiLineTextField.HelpText = "MultiLineText field help text";
			multiLineTextField.Required = true;
			multiLineTextField.Unique = true;
			multiLineTextField.Searchable = true;
			multiLineTextField.Auditable = true;
			multiLineTextField.System = true;
			multiLineTextField.DefaultValue = "";

			multiLineTextField.MaxLength = 500;
			multiLineTextField.VisibleLineNumber = 10;

			fields.Add(multiLineTextField);

			MultiSelectField multiSelectField = new MultiSelectField();

			multiSelectField.Id = Guid.NewGuid();
			multiSelectField.Name = "MultiSelectField";
			multiSelectField.Label = "MultiSelect field";
			multiSelectField.PlaceholderText = "MultiSelect field placeholder text";
			multiSelectField.Description = "MultiSelect field description";
			multiSelectField.HelpText = "MultiSelect field help text";
			multiSelectField.Required = true;
			multiSelectField.Unique = true;
			multiSelectField.Searchable = true;
			multiSelectField.Auditable = true;
			multiSelectField.System = true;
			multiSelectField.DefaultValue = new string[] { "itemKey1", "itemKey4" };

			multiSelectField.Options = new List<MultiSelectFieldOption>();
			multiSelectField.Options.Add(new MultiSelectFieldOption("itemKey1", "itemValue1"));
			multiSelectField.Options.Add(new MultiSelectFieldOption("itemKey2", "itemValue2"));
			multiSelectField.Options.Add(new MultiSelectFieldOption("itemKey3", "itemValue3"));
			multiSelectField.Options.Add(new MultiSelectFieldOption("itemKey4", "itemValue4"));
			multiSelectField.Options.Add(new MultiSelectFieldOption("itemKey5", "itemValue5"));
			multiSelectField.Options.Add(new MultiSelectFieldOption("itemKey6", "itemValue6"));

			fields.Add(multiSelectField);

			NumberField numberField = new NumberField();

			numberField.Id = Guid.NewGuid();
			numberField.Name = "NumberField";
			numberField.Label = "Number field";
			numberField.PlaceholderText = "Number field placeholder text";
			numberField.Description = "Number field description";
			numberField.HelpText = "Number field help text";
			numberField.Required = true;
			numberField.Unique = true;
			numberField.Searchable = true;
			numberField.Auditable = true;
			numberField.System = true;
			numberField.DefaultValue = 0;

			numberField.MinValue = 1;
			numberField.MaxValue = 100;
			numberField.DecimalPlaces = 3;

			fields.Add(numberField);

			PasswordField passwordField = new PasswordField();

			passwordField.Id = Guid.NewGuid();
			passwordField.Name = "PasswordField";
			passwordField.Label = "Password field";
			passwordField.PlaceholderText = "Password field placeholder text";
			passwordField.Description = "Password field description";
			passwordField.HelpText = "Password field help text";
			passwordField.Required = true;
			passwordField.Unique = true;
			passwordField.Searchable = true;
			passwordField.Auditable = true;
			passwordField.System = true;

			passwordField.MaxLength = 24;
			passwordField.MinLength = 6;

			fields.Add(passwordField);

			PercentField percentField = new PercentField();

			percentField.Id = Guid.NewGuid();
			percentField.Name = "PercentField";
			percentField.Label = "Percent field";
			percentField.PlaceholderText = "Percent field";
			percentField.Description = "Percent field description";
			percentField.HelpText = "Percent field help text";
			percentField.Required = true;
			percentField.Unique = true;
			percentField.Searchable = true;
			percentField.Auditable = true;
			percentField.System = true;
			percentField.DefaultValue = 0;

			percentField.MinValue = 1;
			percentField.MaxValue = 100;
			percentField.DecimalPlaces = 3;

			fields.Add(percentField);

			PhoneField phoneField = new PhoneField();

			phoneField.Id = Guid.NewGuid();
			phoneField.Name = "PhoneField";
			phoneField.Label = "Phone field";
			phoneField.PlaceholderText = "Phone field";
			phoneField.Description = "Phone field description";
			phoneField.HelpText = "Phone field help text";
			phoneField.Required = true;
			phoneField.Unique = true;
			phoneField.Searchable = true;
			phoneField.Auditable = true;
			phoneField.System = true;
			phoneField.DefaultValue = "";

			phoneField.Format = "{0000}-{000}-{000}";
			phoneField.MaxLength = 10;

			fields.Add(phoneField);

			GuidField primaryKeyField = new GuidField();

			primaryKeyField.Id = Guid.NewGuid();
			primaryKeyField.Name = "PrimaryKeyField";
			primaryKeyField.Label = "PrimaryKey field";
			primaryKeyField.PlaceholderText = "PrimaryKey field placeholder text";
			primaryKeyField.Description = "PrimaryKey field description";
			primaryKeyField.HelpText = "PrimaryKey field help text";
			primaryKeyField.Required = true;
			primaryKeyField.Unique = true;
			primaryKeyField.Searchable = true;
			primaryKeyField.Auditable = true;
			primaryKeyField.System = true;
			primaryKeyField.DefaultValue = Guid.Empty;

			fields.Add(primaryKeyField);

			SelectField selectField = new SelectField();

			selectField.Id = Guid.NewGuid();
			selectField.Name = "SelectField";
			selectField.Label = "Select field";
			selectField.PlaceholderText = "Select field placeholder text";
			selectField.Description = "Select field description";
			selectField.HelpText = "Select field help text";
			selectField.Required = true;
			selectField.Unique = true;
			selectField.Searchable = true;
			selectField.Auditable = true;
			selectField.System = true;
			selectField.DefaultValue = "itemKey2";

			selectField.Options = new List<SelectFieldOption>();
			selectField.Options.Add(new SelectFieldOption("itemKey1", "itemValue1"));
			selectField.Options.Add(new SelectFieldOption("itemKey2", "itemValue2"));
			selectField.Options.Add(new SelectFieldOption("itemKey3", "itemValue3"));
			selectField.Options.Add(new SelectFieldOption("itemKey4", "itemValue4"));
			selectField.Options.Add(new SelectFieldOption("itemKey5", "itemValue5"));
			selectField.Options.Add(new SelectFieldOption("itemKey6", "itemValue6"));

			fields.Add(selectField);

			TextField textField = new TextField();

			textField.Id = Guid.NewGuid();
			textField.Name = "TextField";
			textField.Label = "Text field";
			textField.PlaceholderText = "Text field placeholder text";
			textField.Description = "Text field description";
			textField.HelpText = "Text field help text";
			textField.Required = true;
			textField.Unique = true;
			textField.Searchable = true;
			textField.Auditable = true;
			textField.System = true;
			textField.DefaultValue = "";

			textField.MaxLength = 200;

			UrlField urlField = new UrlField();

			urlField.Id = Guid.NewGuid();
			urlField.Name = "UrlField";
			urlField.Label = "Url field";
			urlField.PlaceholderText = "Url field placeholder text";
			urlField.Description = "Url field description";
			urlField.HelpText = "Url field help text";
			urlField.Required = true;
			urlField.Unique = true;
			urlField.Searchable = true;
			urlField.Auditable = true;
			urlField.System = true;
			urlField.DefaultValue = "";

			urlField.MaxLength = 200;
			urlField.OpenTargetInNewWindow = true;

			fields.Add(urlField);

			return fields;
		}

		private List<RecordList> CreateTestViewCollection(Entity entity)
		{
			List<RecordList> recordsLists = new List<RecordList>();

			//RecordList firstRecordList = new RecordList();

			//firstRecordList.Id = Guid.NewGuid();
			//firstRecordList.Name = "SearchPopupviewname";
			//firstRecordList.Label = "Search Popup view label";
			//firstRecordList.Type = Api.RecordsListTypes.SearchPopup;

			//firstRecordList.Filters = new List<RecordListFilter>();

			//RecordListFilter filter = new RecordListFilter();
			//filter.EntityId = entity.Id;
			//filter.FieldId = entity.Fields[1].Id;
			//filter.Operator = Api.FilterOperatorTypes.Equals;
			//filter.Value = "false";

			//firstRecordList.Filters.Add(filter);

			//firstRecordList.Fields = new List<RecordListField>();

			//RecordListField field1 = new RecordListField();
			//field1.EntityId = entity.Id;
			//field1.Id = entity.Fields[3].Id;
			//field1.Position = 1;

			//firstRecordList.Fields.Add(field1);

			//RecordListField field2 = new RecordListField();
			//field2.EntityId = entity.Id;
			//field2.Id = entity.Fields[10].Id;
			//field2.Position = 2;

			//firstRecordList.Fields.Add(field2);

			//recordsLists.Add(firstRecordList);
			return recordsLists;
		}
		/*
        private List<RecordView> CreateTestFormCollection(Entity entity)
        {
            List<RecordView> recordViewList = new List<RecordView>();

            RecordView recordView = new RecordView();

            recordView.Id = Guid.NewGuid();
            recordView.Name = "record_view_name";
            recordView.Label = "Record view label";

            recordView.Fields = new List<RecordViewField>();

            RecordViewField field1 = new RecordViewField();

            field1.Id = entity.Fields[1].Id;
            field1.EntityId = entity.Id;
            field1.Column = Api.RecordViewColumns.Left;
            field1.Position = 1;

            recordView.Fields.Add(field1);

            RecordViewField field2 = new RecordViewField();

            field2.Id = entity.Fields[5].Id;
            field2.EntityId = entity.Id;
            field2.Column = Api.RecordViewColumns.Right;
            field2.Position = 2;

            recordView.Fields.Add(field2);

            recordViewList.Add(recordView);

            return recordViewList;
        }
        */
#endregion
			}
		}