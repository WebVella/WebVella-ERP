using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Database;
using WebVella.ERP.Plugins;
using WebVella.ERP.Project.Models;
using WebVella.ERP.Utilities;

namespace WebVella.ERP.Project
{
	[PluginStartup]
	public partial class Startup
	{
		//System elements	
		// Check the SystemIds for lot's of helpful constants you may need, e.g. SystemIds.UserEntityId

		//Code snippets
		//Check out the CodeSnippets.txt file in WebVella.ERP.Web > Docs folder for code pieces on how to create or update some elements


		private bool createSampleRecords = false; //To be enabled requires the CRM sample records
		//Constants
		private static Guid WEBVELLA_PROJECT_PLUGIN_ID = new Guid("2a7bc24a-da6a-48f0-a0c7-7156a8ac69bd");
		private static string WEBVELLA_PROJECT_PLUGIN_NAME = "webvella-project";
		private static Guid PROJECT_ENTITY_ID = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
		private static string PROJECT_ENTITY_NAME = "wv_project";
		private static Guid MILESTONE_ENTITY_ID = new Guid("d691b634-016c-46ef-8ba8-8c3328797497");
		private static string MILESTONE_ENTITY_NAME = "wv_milestone";
		private static Guid TASK_ENTITY_ID = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
		private static string TASK_ENTITY_NAME = "wv_task";
		private static Guid BUG_ENTITY_ID = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
		private static string BUG_ENTITY_NAME = "wv_bug";
		private static Guid ACTIVITY_ENTITY_ID = new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0");
		private static string ACTIVITY_ENTITY_NAME = "wv_project_activity";
		private static Guid TIMELOG_ENTITY_ID = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
		private static string TIMELOG_ENTITY_NAME = "wv_timelog";
		private static Guid ATTACHMENT_ENTITY_ID = new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73");
		private static string ATTACHMENT_ENTITY_NAME = "wv_project_attachment";
		private static Guid COMMENT_ENTITY_ID = new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99");
		private static string COMMENT_ENTITY_NAME = "wv_project_comment";
		private static Guid PROJECT_ADMIN_AREA_ID = new Guid("5b131255-46fc-459d-bbb5-923a4bdfc006");
		private static Guid PROJECT_WORKPLACE_AREA_ID = new Guid("205877a1-242c-41bf-a080-49ea01d4f519");
		private static Guid CREATE_TASK_WORKPLACE_AREA_ID = new Guid("aacf2d40-9b03-43d5-aba8-8c8cb3a0133e");
		private static Guid REPORT_BUG_WORKPLACE_AREA_ID = new Guid("c57fc703-4546-4bfa-9448-29151e21d6ae");
		private static Guid PROJECT_RELATION_USER_1_N_PROJECT_OWNER_ID = new Guid("0cad07c3-73bd-4c1f-a5d6-552256f679a4");
		private static Guid PROJECT_RELATION_CUSTOMER_1_N_PROJECT_ID = new Guid("d7f1ec35-9f59-4d75-b8a2-554c7eaeab11");
		private static Guid PROJECT_RELATION_ROLE_N_N_PROJECT_TEAM_ID = new Guid("4860a4b6-d07e-416f-b548-60491607e93f");
		private static Guid PROJECT_RELATION_ROLE_N_N_PROJECT_CUSTOMER_ID = new Guid("e6d75feb-3c8f-410b-9ff4-54ef8489dc2f");
		//webvella-crm plugin constants
		private static Guid CUSTOMER_ENTITY_ID = new Guid("90bcdb47-2cde-4137-a412-0198348fecc0");
		//private static string CUSTOMER_ENTITY_NAME = "wv_customer";
		private static Guid CUSTOMER_RECORD_ID = new Guid("fb06213f-7632-495b-bb8d-ed5ff07dc515");
		private static Guid CUSTOMER_USER_ID = new Guid("307fe376-a1c6-495e-a7c0-2a78797565f2");
		private static Guid CUSTOMER_ROLE_ID = new Guid("27745245-09bd-4adb-8831-3870bcae46fe");


		public void Start(PluginStartArguments pluginStartArgs)
		{
			//initialize static context
			StaticContext.Initialize(pluginStartArgs.Plugin, pluginStartArgs.ServiceProvider);

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
						var crmPluginFound = false;
						foreach (var plugin in installedPlugins)
						{
							switch (plugin.Name)
							{
								case "webvella-crm":
									crmPluginFound = true;
									break;
								default:
									break;
							}
						}

						if (!crmPluginFound)
							throw new Exception("'webvella-crm' plugin is required for the 'webvella-project' to operate");

						#endregion

						#region << 2.Get the current plugin settings from the database >>
						var currentPluginSettings = new PluginSettings();
						QueryObject pluginDataQueryObject = EntityQuery.QueryEQ("name", WEBVELLA_PROJECT_PLUGIN_NAME);
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
								settingsEntityRecord["id"] = WEBVELLA_PROJECT_PLUGIN_ID;
								settingsEntityRecord["name"] = WEBVELLA_PROJECT_PLUGIN_NAME;
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
								throw ex;
							}
						}

						if (currentPluginSettings.Version < 20160613)
						{
							try
							{
								currentPluginSettings.Version = 20160613;
								Patch160613(entMan, relMan, recMan, createSampleRecords);
							}
							catch (Exception ex)
							{
								throw ex;
							}
						}

						if (currentPluginSettings.Version < 20160627)
						{
							try
							{
								currentPluginSettings.Version = 20160627;
								Patch160627(entMan, relMan, recMan, createSampleRecords);
							}
							catch (Exception ex)
							{
								throw ex;
							}
						}

						if (currentPluginSettings.Version < 20160707)
						{
							try
							{
								currentPluginSettings.Version = 20160707;
								Patch160707(entMan, relMan, recMan, createSampleRecords);
							}
							catch (Exception ex)
							{
								throw ex;
							}
						}

						if (currentPluginSettings.Version < 20161118)
						{
							try
							{
								currentPluginSettings.Version = 20161118;
								Patch161118(entMan, relMan, recMan, createSampleRecords);
							}
							catch (Exception ex)
							{
								throw ex;
							}
						}

						if (currentPluginSettings.Version < 20161119)
						{
							try
							{
								currentPluginSettings.Version = 20161119;
								Patch161119(entMan, relMan, recMan, createSampleRecords);
							}
							catch (Exception ex)
							{
								var exception = ex;
								throw ex;
							}
						}

						if (currentPluginSettings.Version < 20170119)
						{
							try
							{
								currentPluginSettings.Version = 20170119;
								Patch170119(entMan, relMan, recMan, createSampleRecords);
							}
							catch (Exception ex)
							{
								throw ex;
							}
						}

						if (currentPluginSettings.Version < 20170328)
						{
							try
							{
								currentPluginSettings.Version = 20170328;
								Patch170328(entMan, relMan, recMan, createSampleRecords);
							}
							catch (Exception ex)
							{
								throw ex;
							}
						}

						if (currentPluginSettings.Version < 20170502)
						{
							try
							{
								currentPluginSettings.Version = 20170502;
								Patch20170502(entMan, relMan, recMan, createSampleRecords);
							}
							catch (Exception ex)
							{
								throw ex;
							}
						}

						#endregion

						#region << 4. Save needed changes to the plugin setting data >>
						{
							string json = JsonConvert.SerializeObject(currentPluginSettings);
							var settingsEntityRecord = new EntityRecord();
							settingsEntityRecord["id"] = WEBVELLA_PROJECT_PLUGIN_ID;
							settingsEntityRecord["name"] = WEBVELLA_PROJECT_PLUGIN_NAME;
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
