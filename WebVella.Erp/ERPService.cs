using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Database;
using WebVella.Erp.Hooks;
using WebVella.Erp.Jobs;

namespace WebVella.Erp
{
	public class ErpService : IErpService
	{
		public List<ErpPlugin> Plugins { get; set; } = new List<ErpPlugin>();
		public List<ErpJob> Jobs { get; set; } = new List<ErpJob>();

		public void InitializeSystemEntities()
		{
			FieldResponse fieldResponse = null;
			EntityManager entMan = new EntityManager();
			EntityRelationManager rm = new EntityRelationManager();
			RecordManager recMan = new RecordManager(null, true);

			using (var connection = DbContext.Current.CreateConnection())
			{
				//setup necessary extensions
				DbRepository.CreatePostgresqlExtensions();
				//setup casts
				DbRepository.CreatePostgresqlCasts();

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

					if (currentVersion < 1)
					{
						systemSettings.Version = 1;

						List<Guid> allowedRoles = new List<Guid>();
						allowedRoles.Add(SystemIds.AdministratorRoleId);

						#region << create user entity >>
						{

							var systemItemIdDictionary = new Dictionary<string, Guid>();
							systemItemIdDictionary["id"] = new Guid("24cdb49a-cb1f-4a38-bc99-a6137ade9949");

							InputEntity userEntity = new InputEntity();
							userEntity.Id = SystemIds.UserEntityId;
							userEntity.Name = "user";
							userEntity.Label = "User";
							userEntity.LabelPlural = "Users";
							userEntity.System = true;
							userEntity.Color = "#f44336";
							userEntity.IconName = "fa fa-user";
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
							var response = entMan.CreateEntity(userEntity, systemItemIdDictionary);

							#region <--- created_on --->
							{
								InputDateTimeField createdOn = new InputDateTimeField();
								createdOn.Id = new Guid("6fda5e6b-80e6-4d8a-9e2a-d983c3694e96");
								createdOn.Name = "created_on";
								createdOn.Label = "Created On";
								createdOn.PlaceholderText = "";
								createdOn.Description = "";
								createdOn.HelpText = "";
								createdOn.Required = true;
								createdOn.Unique = false;
								createdOn.Searchable = true;
								createdOn.Auditable = true;
								createdOn.System = true;
								createdOn.DefaultValue = null;
								createdOn.Format = "dd MMM yyyy HH:mm:ss";
								createdOn.UseCurrentTimeAsDefaultValue = true;

								var createResponse = entMan.CreateField(SystemIds.UserEntityId, createdOn, false);
								if (!createResponse.Success)
									throw new Exception("System error 10060. Entity: user. Field: created_on" + " Message:" + createResponse.Message);
							}
							#endregion

							#region <--- first_name --->
							{
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

								var createResponse = entMan.CreateField(SystemIds.UserEntityId, firstName, false);
								if (!createResponse.Success)
									throw new Exception("System error 10060. Entity: user. Field: first_name" + " Message:" + createResponse.Message);
							}
							#endregion

							#region <--- last_name --->
							{
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

								var createResponse = entMan.CreateField(SystemIds.UserEntityId, lastName, false);
								if (!createResponse.Success)
									throw new Exception("System error 10060. Entity: user. Field: last_name" + " Message:" + createResponse.Message);
							}
							#endregion

							#region <--- username --->
							{
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

								var createResponse = entMan.CreateField(SystemIds.UserEntityId, userName, false);
								if (!createResponse.Success)
									throw new Exception("System error 10060. Entity: user. Field: userName" + " Message:" + createResponse.Message);
							}
							#endregion

							#region <--- email --->
							{
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

								var createResponse = entMan.CreateField(SystemIds.UserEntityId, email, false);
								if (!createResponse.Success)
									throw new Exception("System error 10060. Entity: user. Field: email" + " Message:" + createResponse.Message);
							}
							#endregion

							#region <--- password --->
							{
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

								var createResponse = entMan.CreateField(SystemIds.UserEntityId, password, false);
								if (!createResponse.Success)
									throw new Exception("System error 10060. Entity: user. Field: password" + " Message:" + createResponse.Message);
							}
							#endregion

							#region <--- last_logged_in --->
							{
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

								var createResponse = entMan.CreateField(SystemIds.UserEntityId, lastLoggedIn, false);
								if (!createResponse.Success)
									throw new Exception("System error 10060. Entity: user. Field: lastLoggedIn" + " Message:" + createResponse.Message);
							}
							#endregion

							#region <--- enabled --->
							{
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

								var createResponse = entMan.CreateField(SystemIds.UserEntityId, enabledField, false);
								if (!createResponse.Success)
									throw new Exception("System error 10060. Entity: user. Field: enabled" + " Message:" + createResponse.Message);
							}
							#endregion

							#region <--- verified --->
							{
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

								var createResponse = entMan.CreateField(SystemIds.UserEntityId, verifiedUserField, false);
								if (!createResponse.Success)
									throw new Exception("System error 10060. Entity: user. Field: verified" + " Message:" + createResponse.Message);
							}

							#endregion

							#region <--- preferences --->
							{
								InputTextField preferences = new InputTextField();
								preferences.Id = new Guid("29d46dac-b477-48f8-9f3a-22d7e95ae1cc");
								preferences.Name = "preferences";
								preferences.Label = "Preferences";
								preferences.PlaceholderText = "";
								preferences.Description = "Preferences json field.";
								preferences.HelpText = "";
								preferences.Required = true;
								preferences.Unique = false;
								preferences.Searchable = false;
								preferences.Auditable = false;
								preferences.System = true;
								preferences.DefaultValue = "{}";

								var createResponse = entMan.CreateField(SystemIds.UserEntityId, preferences, false);
								if (!createResponse.Success)
									throw new Exception("System error 10060. Entity: user. Field: preferences" + " Message:" + createResponse.Message);
							}

							#endregion

							#region <---  image --- >
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

								var createResponse = entMan.CreateField(SystemIds.UserEntityId, imageField, false);
								if (!createResponse.Success)
									throw new Exception("System error 10060. Entity: user. Field: image" + " Message:" + createResponse.Message);
							}
							#endregion
						}

						#endregion

						#region << create role entity >>

						{
							var systemItemIdDictionary = new Dictionary<string, Guid>();
							systemItemIdDictionary["id"] = new Guid("0c5679f4-a290-4923-ad2b-d304cbc79937");

							InputEntity roleEntity = new InputEntity();
							roleEntity.Id = SystemIds.RoleEntityId;
							roleEntity.Name = "role";
							roleEntity.Label = "Role";
							roleEntity.LabelPlural = "Roles";
							roleEntity.System = true;
							roleEntity.Color = "#f44336";
							roleEntity.IconName = "fa fa-key";
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
							var response = entMan.CreateEntity(roleEntity, systemItemIdDictionary);

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
							user["created_on"] = new DateTime(2010, 10, 10);
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
							user["created_on"] = new DateTime(2010, 10, 10);
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

							QueryResponse result = recMan.CreateRecord("role", adminRole);
							if (!result.Success)
								throw new Exception("CREATE ADMINITRATOR ROLE RECORD:" + result.Message);
						}

						{
							EntityRecord regularRole = new EntityRecord();
							regularRole["id"] = SystemIds.RegularRoleId;
							regularRole["name"] = "regular";
							regularRole["description"] = "";

							QueryResponse result = recMan.CreateRecord("role", regularRole);
							if (!result.Success)
								throw new Exception("CREATE REGULAR ROLE RECORD:" + result.Message);
						}

						{
							EntityRecord guestRole = new EntityRecord();
							guestRole["id"] = SystemIds.GuestRoleId;
							guestRole["name"] = "guest";
							guestRole["description"] = "";

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

						#region << create user_file entity >>
						{

							#region << ***Create entity*** Entity name: user_file >>
							{
								#region << entity >>
								{
									var systemFieldIdDictionary = new Dictionary<string, Guid>();
									systemFieldIdDictionary["id"] = new Guid("14369619-fe7b-423f-bf60-3e7f8b35b840");

									var entity = new InputEntity();
									entity.Id = new Guid("5c666c54-9e76-4327-ac7a-55851037810c");
									entity.Name = "user_file";
									entity.Label = "User File";
									entity.LabelPlural = "User Files";
									entity.System = true;
									entity.IconName = "fa fa-file";
									entity.Color = "#f44336";
									//entity.Weight = (decimal)100.0;
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
										var response = entMan.CreateEntity(entity, systemFieldIdDictionary);
										if (!response.Success)
											throw new Exception("System error 10050. Entity: user_file creation Message: " + response.Message);
									}
								}
								#endregion
							}
							#endregion

							#region << ***Create field***  Entity: user_file Field Name: created_on >>
							{

								InputDateTimeField createdOn = new InputDateTimeField();

								createdOn.Id = new Guid("7bc7c1a2-93aa-40bc-8374-fd350cdc7fac");
								createdOn.Name = "created_on";
								createdOn.Label = "Created On";
								createdOn.PlaceholderText = "";
								createdOn.Description = "";
								createdOn.HelpText = "";
								createdOn.Required = true;
								createdOn.Unique = false;
								createdOn.Searchable = true;
								createdOn.Auditable = true;
								createdOn.System = true;
								createdOn.DefaultValue = null;

								createdOn.Format = "dd MMM yyyy HH:mm:ss";
								createdOn.UseCurrentTimeAsDefaultValue = true;
								createdOn.EnableSecurity = false;
								createdOn.Permissions = new FieldPermissions();
								createdOn.Permissions.CanRead = new List<Guid>();
								createdOn.Permissions.CanUpdate = new List<Guid>();

								{
									var response = entMan.CreateField(new Guid("5c666c54-9e76-4327-ac7a-55851037810c"), createdOn, false);
									if (!response.Success)
										throw new Exception("System error 10060. Entity: user_file Field: created_on:" + response.Message);
								}
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
								dropdownField.Options = new List<SelectOption>
								{
									new SelectOption() { Value = "image", Label = "image"},
									new SelectOption() { Value = "document", Label = "document"},
									new SelectOption() { Value = "audio", Label = "audio"},
									new SelectOption() { Value = "video", Label = "video"},
									new SelectOption() { Value = "other", Label = "other"}
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
								fileField.DefaultValue = "no-file-error.txt";
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
						#endregion
					}
					if (currentVersion < 2)
					{
						systemSettings.Version = 2;
						UpdateSitemapNodeTable1();
					}

					if (currentVersion < 3)
					{
						systemSettings.Version = 3;
						UpdateSitemapNodeTable2();
					}

					new DbSystemSettingsRepository(DbContext.Current).Save(new DbSystemSettings { Id = systemSettings.Id, Version = systemSettings.Version });

					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					var exception = ex;
					connection.RollbackTransaction();
					throw;
				}

			}
		}

		public void InitializePlugins(IServiceProvider serviceProvider)
		{
			foreach (ErpPlugin plugin in Plugins)
				plugin.Initialize(serviceProvider);

			JobManager.Current.RegisterJobTypes(this);
			HookManager.RegisterHooks(this);
		}

		public void SetAutoMapperConfiguration()
		{
			foreach (ErpPlugin plugin in Plugins)
				plugin.SetAutoMapperConfiguration(ErpAutoMapperConfiguration.MappingExpressions);
		}

		public void InitializeBackgroundJobs(List<JobType> additionalJobTypes = null)
		{
			JobManagerSettings settings = new JobManagerSettings();
			settings.DbConnectionString = ErpSettings.ConnectionString;
			settings.Enabled = ErpSettings.EnableBackgroundJobs;

			JobManager.Initialize(settings, additionalJobTypes);
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


				bool systemSearchTableExists = false;
				command = connection.CreateCommand("SELECT EXISTS (  SELECT 1 FROM   information_schema.tables  WHERE  table_schema = 'public' AND table_name = 'system_search' ) ");
				using (var reader = command.ExecuteReader())
				{
					reader.Read();
					systemSearchTableExists = reader.GetBoolean(0);
					reader.Close();
				}

				if (!systemSearchTableExists)
				{
					const string filesTableSql = @"CREATE TABLE public.system_search (
  id UUID NOT NULL,
  entities TEXT DEFAULT ''::text NOT NULL,
  apps TEXT DEFAULT ''::text NOT NULL,
  records TEXT DEFAULT ''::text NOT NULL,
  content TEXT DEFAULT ''::text NOT NULL,
  snippet TEXT DEFAULT ''::text NOT NULL,
  url TEXT DEFAULT ''::text NOT NULL,
  aux_data TEXT DEFAULT ''::text NOT NULL,
  ""timestamp"" TIMESTAMP(0) WITH TIME ZONE NOT NULL,
  stem_content TEXT DEFAULT ''::text NOT NULL,
  CONSTRAINT system_search_pkey PRIMARY KEY(id)
) 
WITH(oids = false); ";

					command = connection.CreateCommand(filesTableSql);
					command.ExecuteNonQuery();

					command = connection.CreateCommand("CREATE INDEX system_search_fts_idx ON system_search USING gin( to_tsvector( 'english', stem_content) )");
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

					DbRepository.CreateIndex("idx_filepath", "files", "filepath", null, true);
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
					  complete_class_name text NOT NULL,
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

				if (!jobsResultColumnExists)
				{
					command = connection.CreateCommand("ALTER TABLE public.jobs  ADD COLUMN result text");
					command.ExecuteNonQuery();
				}

				bool systemLogTableExists = false;
				command = connection.CreateCommand("SELECT EXISTS (  SELECT 1 FROM   information_schema.tables  WHERE  table_schema = 'public' AND table_name = 'system_log' ) ");
				using (var reader = command.ExecuteReader())
				{
					reader.Read();
					systemLogTableExists = reader.GetBoolean(0);
					reader.Close();
				}

				if (!systemLogTableExists)
				{
					command = connection.CreateCommand(@"
CREATE TABLE public.system_log (
  id UUID NOT NULL,
  created_on TIMESTAMP WITH TIME ZONE DEFAULT '2011-11-11 02:11:11+02'::timestamp with time zone NOT NULL,
  type INTEGER DEFAULT 1 NOT NULL,
  message TEXT DEFAULT 'message'::text NOT NULL,
  source TEXT DEFAULT 'source'::text NOT NULL,
  details TEXT,
  notification_status INTEGER DEFAULT 1 NOT NULL,
  CONSTRAINT system_log_pkey PRIMARY KEY(id)
) 
WITH (oids = false);

CREATE INDEX idx_system_log_created_on ON public.system_log
  USING btree (created_on);

CREATE INDEX idx_system_log_message ON public.system_log
  USING btree (message COLLATE pg_catalog.""default"");

CREATE INDEX idx_system_log_notification_status ON public.system_log
    USING btree(notification_status);

CREATE INDEX idx_system_log_source ON public.system_log
  USING btree(source COLLATE pg_catalog.""default"");

CREATE INDEX idx_system_log_type ON public.system_log
  USING btree(type);
");
					command.ExecuteNonQuery();
				}

				bool pluginDataTableExists = false;
				command = connection.CreateCommand("SELECT EXISTS (  SELECT 1 FROM   information_schema.tables  WHERE  table_schema = 'public' AND table_name = 'plugin_data' ) ");
				using (var reader = command.ExecuteReader())
				{
					reader.Read();
					pluginDataTableExists = reader.GetBoolean(0);
					reader.Close();
				}

				if (!pluginDataTableExists)
				{
					command = connection.CreateCommand(@"
CREATE TABLE plugin_data(
  id UUID NOT NULL,
  name TEXT DEFAULT ''::text NOT NULL,
  data TEXT DEFAULT ''::text,
  CONSTRAINT idx_u_plugin_data_name UNIQUE(name),
  CONSTRAINT plugin_data_pkey PRIMARY KEY(id)
)
WITH(oids = false);");
					command.ExecuteNonQuery();
				}


				bool appTableExists = false;
				command = connection.CreateCommand("SELECT EXISTS (  SELECT 1 FROM   information_schema.tables  WHERE  table_schema = 'public' AND table_name = 'app' ) ");
				using (var reader = command.ExecuteReader())
				{
					reader.Read();
					appTableExists = reader.GetBoolean(0);
					reader.Close();
				}

				if (!appTableExists)
				{
					const string sql = @"
CREATE TABLE public.app (
    id uuid NOT NULL,
    name text DEFAULT ''::text NOT NULL,
    label text NOT NULL,
    description text,
    icon_class text,
    author text,
    color text,
    weight integer DEFAULT '-1'::integer NOT NULL,
    access uuid[]
)
WITH (oids = false);

CREATE TABLE public.app_sitemap_area (
    id uuid NOT NULL,
    name text DEFAULT ''::text NOT NULL,
    label text,
    label_translations text,
    description text,
    description_translations text,
    icon_class text,
    weight integer DEFAULT '-1'::integer NOT NULL,
    color text,
    show_group_names boolean DEFAULT false NOT NULL,
    access_roles uuid[] NOT NULL,
    app_id uuid NOT NULL
)
WITH (oids = false);

CREATE TABLE public.app_sitemap_area_group (
    id uuid NOT NULL,
    area_id uuid NOT NULL,
    weight integer DEFAULT '-1'::integer NOT NULL,
    name text NOT NULL,
    label text,
    label_translations text,
    render_roles uuid[] NOT NULL
)
WITH (oids = false);

CREATE TABLE public.app_sitemap_area_node (
    id uuid NOT NULL,
    area_id uuid NOT NULL,
    name text NOT NULL,
    label text,
    label_translations text,
    icon_class text,
    url text,
    weight integer NOT NULL,
    access_roles uuid[] NOT NULL,
    type integer NOT NULL,
    entity_id uuid
)
WITH (oids = false);

CREATE TABLE public.app_page (
    id uuid NOT NULL,
    name text NOT NULL,
    label text,
    icon_class text,
    system boolean DEFAULT false,
    type integer NOT NULL,
    weight integer DEFAULT '-1'::integer NOT NULL,
    label_translations text,
	razor_body text,
    area_id uuid,
    node_id uuid,
    app_id uuid,
    entity_id uuid,
    is_razor_body boolean DEFAULT false NOT NULL
)
WITH (oids = false);

CREATE TABLE public.app_page_body_node (
    id uuid NOT NULL,
    parent_id uuid,
    node_id uuid,
	page_id uuid NOT NULL,
    weight integer DEFAULT '-1'::integer NOT NULL,
    component_name text,
    options text
)
WITH (oids = false);


ALTER TABLE public.app_page_body_node
    ADD COLUMN container_id text;

CREATE INDEX idx_app_page_body_node_page_id ON app_page_body_node USING btree (page_id);
CREATE INDEX idx_app_page_app_id ON app_page USING btree (app_id);
CREATE INDEX fki_app_page_body_node_parent_id ON app_page_body_node USING btree (parent_id);
CREATE INDEX fki_app_page_area_id ON app_page USING btree (area_id);
CREATE INDEX fki_app_page_node_id ON app_page USING btree (node_id);
CREATE INDEX fki_app_page_entity_id ON app_page USING btree (entity_id);

ALTER TABLE ONLY app_page_body_node
    ADD CONSTRAINT app_page_body_node_pkey
    PRIMARY KEY (id);

ALTER TABLE ONLY app
    ADD CONSTRAINT app_pkey
    PRIMARY KEY (id);

ALTER TABLE ONLY app_sitemap_area
    ADD CONSTRAINT app_sitemap_area_pkey
    PRIMARY KEY (id);

ALTER TABLE ONLY app_sitemap_area_group
    ADD CONSTRAINT app_sitemap_area_group_pkey
    PRIMARY KEY (id);

ALTER TABLE ONLY app_sitemap_area_node
    ADD CONSTRAINT app_sitemap_area_node_pkey
    PRIMARY KEY (id);

ALTER TABLE ONLY app_page
    ADD CONSTRAINT app_page_pkey
    PRIMARY KEY (id);

ALTER TABLE ONLY app_page_body_node
    ADD CONSTRAINT fkey_app_page_body_node_parent_id
    FOREIGN KEY (parent_id) REFERENCES app_page_body_node(id);

ALTER TABLE ONLY app_page_body_node
    ADD CONSTRAINT fkey_app_page_body_node_page_id
    FOREIGN KEY (page_id) REFERENCES app_page(id);

ALTER TABLE ONLY app_sitemap_area
    ADD CONSTRAINT fkey_app_id
    FOREIGN KEY (app_id) REFERENCES app(id);

ALTER TABLE ONLY app_sitemap_area_group
    ADD CONSTRAINT fkey_area_id
    FOREIGN KEY (area_id) REFERENCES app_sitemap_area(id);

ALTER TABLE ONLY app_sitemap_area_node
    ADD CONSTRAINT fkey_area_id
    FOREIGN KEY (area_id) REFERENCES app_sitemap_area(id);

ALTER TABLE ONLY app_page
    ADD CONSTRAINT fkey_app_id
    FOREIGN KEY (app_id) REFERENCES app(id);

ALTER TABLE ONLY app_page
    ADD CONSTRAINT fkey_area_id
    FOREIGN KEY (area_id) REFERENCES app_sitemap_area(id);

ALTER TABLE ONLY app_page
    ADD CONSTRAINT fkey_node_id
    FOREIGN KEY (node_id) REFERENCES app_sitemap_area_node(id);

ALTER TABLE ONLY app
    ADD CONSTRAINT ux_app_name
    UNIQUE (name);	

ALTER TABLE public.app_page
	ADD COLUMN layout text NOT NULL DEFAULT '';	
	
CREATE TABLE public.data_source (
  id UUID NOT NULL,
  name TEXT NOT NULL,
  description TEXT NOT NULL,
  weight INTEGER NOT NULL,
  eql_text TEXT NOT NULL,
  sql_text TEXT NOT NULL,
  parameters_json TEXT NOT NULL,
  fields_json TEXT NOT NULL,
  entity_name TEXT NOT NULL,
  CONSTRAINT data_source_pkey PRIMARY KEY(id),
  CONSTRAINT ux_data_source_name UNIQUE(name)
) 
WITH (oids = false);


CREATE TABLE public.app_page_data_source (
  parameters TEXT NOT NULL,
  name TEXT NOT NULL,
  id UUID NOT NULL,
  page_id UUID NOT NULL,
  data_source_id UUID NOT NULL,
  CONSTRAINT app_page_data_source_pkey PRIMARY KEY(id),
  CONSTRAINT app_page_data_uxc_name_page_id UNIQUE(name, page_id)
) 
WITH (oids = false);

ALTER TABLE public.app_page_data_source
  ADD CONSTRAINT fkey_page_id FOREIGN KEY (page_id)
    REFERENCES public.app_page(id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
    NOT DEFERRABLE;

CREATE INDEX fki_app_page_data_fkc_page_id ON public.app_page_data_source
  USING btree (page_id);";

					command = connection.CreateCommand(sql);
					command.ExecuteNonQuery();
				}
			}
		}

		private void UpdateSitemapNodeTable1()
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				const string updateTable = @"ALTER TABLE public.app_sitemap_area_node 
                  ADD COLUMN entity_list_pages uuid[] NOT NULL DEFAULT array[]::uuid[],
                  ADD COLUMN entity_create_pages uuid[] NOT NULL DEFAULT array[]::uuid[],
                  ADD COLUMN entity_details_pages uuid[] NOT NULL DEFAULT array[]::uuid[],
                  ADD COLUMN entity_manage_pages uuid[] NOT NULL DEFAULT array[]::uuid[];";

				var command = connection.CreateCommand(updateTable);
				command.ExecuteNonQuery();
			}
		}

		private void UpdateSitemapNodeTable2()
		{
			using (var connection = DbContext.Current.CreateConnection())
			{
				const string updateTable = @"ALTER TABLE public.app_sitemap_area_node 
                  ADD COLUMN parent_id uuid DEFAULT NULL;

						ALTER TABLE ONLY public.app_sitemap_area_node
						ADD CONSTRAINT fkey_app_sitemap_area_node_parent_id
						FOREIGN KEY (parent_id) REFERENCES app_sitemap_area_node(id);";

				var command = connection.CreateCommand(updateTable);
				command.ExecuteNonQuery();
			}
		}
	}
}
