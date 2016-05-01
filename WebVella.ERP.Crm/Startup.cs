using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebVella.ERP;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Crm.Models;
using WebVella.ERP.Database;
using WebVella.ERP.Plugins;

namespace WebVella.Crm.Project
{
	[PluginStartup]
	public class Startup
	{
		//Constants
		private static Guid WEBVELLA_CRM_PLUGIN_ID = new Guid("7b44d010-a856-449f-b415-507efe869ccd");
		private static string WEBVELLA_CRM_PLUGIN_NAME = "webvella-crm";
		private static Guid CUSTOMER_ENTITY_ID = new Guid("90bcdb47-2cde-4137-a412-0198348fecc0");
		private static string CUSTOMER_ENTITY_NAME = "wv_customer";

		public void Start()
		{

			var entMan = new EntityManager();
			var relMan = new EntityRelationManager();
			var recMan = new RecordManager();
			var storeSystemSettings = DbContext.Current.SettingsRepository.Read();
			var systemSettings = new SystemSettings(storeSystemSettings);

			//Open scope with a user we will use for the operations further ahead
			var user = new SecurityManager().GetUser(SystemIds.FirstUserId);
			using (SecurityContext.OpenScope(user))
			{

				//Create transaction
				using (var connection = DbContext.Current.CreateConnection())
				{
					try
					{
						connection.BeginTransaction();
						//Here we need to initialize or update the environment based on the plugin requirements.
						//The default place for the plugin data is the "plugin_data" entity -> the "data" text field, which is used to store stringified JSON
						//containing the plugin settings or version

						#region << 1.Get the current ERP database version and checks for other plugin dependencies >>

						if (systemSettings.Version > 0)
						{
							//Do something if database version is not what you expect
						}

						//This plugin needs the webvella-crm plugin to be installed, so we will check this here
						var installedPlugins = new PluginService().Plugins;
						var crmPluginFound = false;
						foreach (var plugin in installedPlugins)
						{
							if (plugin.Name == "webvella-core")
							{
								crmPluginFound = true;
								break;
							}
						}

						if (!crmPluginFound)
							throw new Exception("'webvella-core' plugin is required for the 'webvella-crm' to operate");

						#endregion

						#region << 2.Get the current plugin settings from the database >>
						var currentPluginSettings = new PluginSettings();
						QueryObject pluginDataQueryObject = EntityQuery.QueryEQ("name", WEBVELLA_CRM_PLUGIN_NAME);
						var pluginDataQuery = new EntityQuery("plugin_data", "*", pluginDataQueryObject);
						var pluginDataQueryResponse = recMan.Find(pluginDataQuery);
						if (!pluginDataQueryResponse.Success)
							throw new Exception("plugin 'webvella-project' failed to get its settings due to: " + pluginDataQueryResponse.Message);

						if (pluginDataQueryResponse.Object == null || !pluginDataQueryResponse.Object.Data.Any() || pluginDataQueryResponse.Object.Data[0]["data"] == DBNull.Value)
						{
							//plugin was not installed
							currentPluginSettings.Version = 20160429;
							{
								string json = JsonConvert.SerializeObject(currentPluginSettings);
								var settingsEntityRecord = new EntityRecord();
								settingsEntityRecord["id"] = WEBVELLA_CRM_PLUGIN_ID;
								settingsEntityRecord["name"] = WEBVELLA_CRM_PLUGIN_NAME;
								settingsEntityRecord["data"] = json;
								var settingsSaveReponse = recMan.CreateRecord("plugin_data", settingsEntityRecord);
								if (!settingsSaveReponse.Success)
									throw new Exception("plugin 'webvella-project' failed to save its settings in the database due to: " + pluginDataQueryResponse.Message);
							}
						}
						else
						{
							string json = (string)((List<EntityRecord>)pluginDataQueryResponse.Object.Data)[0]["data"];
							currentPluginSettings = JsonConvert.DeserializeObject<PluginSettings>(json);
						}
						#endregion

						#region << 3. Run methods based on the current installed version of the plugin >>
						if (currentPluginSettings.Version < 20160430)
						{
							currentPluginSettings.Version = 20160430;

							#region << wv_customer >>
							{
								#region << entity >>
								{
									InputEntity entity = new InputEntity();
									entity.Id = CUSTOMER_ENTITY_ID;
									entity.Name = CUSTOMER_ENTITY_NAME;
									entity.Label = "Customer";
									entity.LabelPlural = "Customers";
									entity.System = true;
									entity.IconName = "building-o";
									entity.Weight = 2;
									entity.RecordPermissions = new RecordPermissions();
									entity.RecordPermissions.CanCreate = new List<Guid>();
									entity.RecordPermissions.CanRead = new List<Guid>();
									entity.RecordPermissions.CanUpdate = new List<Guid>();
									entity.RecordPermissions.CanDelete = new List<Guid>();
									//Create
									entity.RecordPermissions.CanCreate.Add(SystemIds.AdministratorRoleId);
									entity.RecordPermissions.CanCreate.Add(SystemIds.RegularRoleId);
									//READ
									entity.RecordPermissions.CanRead.Add(SystemIds.AdministratorRoleId);
									entity.RecordPermissions.CanRead.Add(SystemIds.RegularRoleId);
									//UPDATE
									entity.RecordPermissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
									entity.RecordPermissions.CanUpdate.Add(SystemIds.RegularRoleId);
									//DELETE
									entity.RecordPermissions.CanDelete.Add(SystemIds.AdministratorRoleId);

									{
										var response = entMan.CreateEntity(entity);
										if (!response.Success)
											throw new Exception("System error 10050. Entity: " + CUSTOMER_ENTITY_NAME + " Field: entity creation" + " Message:" + response.Message);
									}
								}
								#endregion

								#region << name >>
								{
									InputTextField textboxField = new InputTextField();
									textboxField.Id = new Guid("7fb95d0f-ab59-421d-974d-ab357e28a1f9");
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
										var response = entMan.CreateField(CUSTOMER_ENTITY_ID, textboxField, false);
										if (!response.Success)
											throw new Exception("System error 10060. Entity: " + CUSTOMER_ENTITY_NAME + " Field: name" + " Message:" + response.Message);
									}
								}
								#endregion

							}
							#endregion
						}
						#endregion

						#region << 4. Save needed changes to the plugin setting data >>
						{
							string json = JsonConvert.SerializeObject(currentPluginSettings);
							var settingsEntityRecord = new EntityRecord();
							settingsEntityRecord["id"] = WEBVELLA_CRM_PLUGIN_ID;
							settingsEntityRecord["name"] = WEBVELLA_CRM_PLUGIN_NAME;
							settingsEntityRecord["data"] = json;
							var settingsUpdateReponse = recMan.UpdateRecord("plugin_data", settingsEntityRecord);
							if (!settingsUpdateReponse.Success)
								throw new Exception("plugin 'webvella-project' failed to update its settings in the database due to: " + pluginDataQueryResponse.Message);
						}
						#endregion

						connection.CommitTransaction();
					}
					catch (Exception ex)
					{
						connection.RollbackTransaction();
						throw ex;
					}
				}
			}
		}
	}
}
