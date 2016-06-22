using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebVella.ERP;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Crm.Models;
using WebVella.ERP.Database;
using WebVella.ERP.Plugins;
using WebVella.ERP.Utilities;

namespace WebVella.ERP.Crm
{
	[PluginStartup]
	public partial class Startup
	{
		//System elements	
		// Check the SystemIds for lot's of helpful constants you may need, e.g. SystemIds.UserEntityId

		//Code snippets
		//Check out the CodeSnippets.txt file in WebVella.ERP.Web > Docs folder for code pieces on how to create or update some elements		

		private bool createSampleRecords = false;
		//Constants
		private static Guid WEBVELLA_CRM_PLUGIN_ID = new Guid("7b44d010-a856-449f-b415-507efe869ccd");
		private static string WEBVELLA_CRM_PLUGIN_NAME = "webvella-crm";
		private static Guid CUSTOMER_ENTITY_ID = new Guid("90bcdb47-2cde-4137-a412-0198348fecc0");
		private static string CUSTOMER_ENTITY_NAME = "wv_customer";
		private static Guid CRM_ADMIN_AREA_ID = new Guid("0e3a02c4-64df-4b61-bae5-8b3cc847f129");

		public void Start(PluginStartArguments pluginStartArgs)
		{

			var entMan = new EntityManager();
			var relMan = new EntityRelationManager();
			var recMan = new RecordManager();
			var storeSystemSettings = DbContext.Current.SettingsRepository.Read();
			var systemSettings = new SystemSettings(storeSystemSettings);

			using (SecurityContext.OpenSystemScope())
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
						var corePluginFound = false;
						foreach (var plugin in installedPlugins)
						{
							if (plugin.Name == "webvella-core")
							{
								corePluginFound = true;
								break;
							}
						}

						if (!corePluginFound)
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
							try
							{
								currentPluginSettings.Version = 20160430;
								Patch160430(entMan,relMan,recMan,createSampleRecords);
							}
							catch (Exception ex)
							{
								var exception = ex;
								throw ex;
							}

						}

						if (currentPluginSettings.Version < 20160610)
						{
							try
							{
								currentPluginSettings.Version = 20160610;
								Patch160610(entMan, relMan, recMan, createSampleRecords);
							}
							catch (Exception ex)
							{
								var exception = ex;
								throw ex;
							}

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
