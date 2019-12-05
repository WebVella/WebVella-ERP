using Newtonsoft.Json;
using System;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;
using WebVella.Erp.Plugins.SDK.Model;

namespace WebVella.Erp.Plugins.SDK
{
	public partial class SdkPlugin : ErpPlugin
	{
		private const int WEBVELLA_SDK_INIT_VERSION = 20181001;
		private static string WEBVELLA_SDK_APP_NAME = "sdk";
		private static Guid WEBVELLA_SDK_APP_ID = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
		private static Guid WEBVELLA_SDK_APP_AREA_DESIGN_ID = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
		private static Guid WEBVELLA_SDK_APP_AREA_ACCESS_ID = new Guid("c5c4cefc-1402-4a8b-9867-7f2a059b745d");
		private static Guid WEBVELLA_SDK_APP_AREA_SERVER_ID = new Guid("fee72214-f1c4-4ed5-8bda-35698dc11528");

		public void ProcessPatches()
		{
			using (SecurityContext.OpenSystemScope())
			{

				var entMan = new EntityManager();
				var relMan = new EntityRelationManager();
				var recMan = new RecordManager();
				var storeSystemSettings = DbContext.Current.SettingsRepository.Read();
				var systemSettings = new SystemSettings(storeSystemSettings);

				//Create transaction
				using (var connection = DbContext.Current.CreateConnection())
				{
					try
					{
						connection.BeginTransaction();

						//Here we need to initialize or update the environment based on the plugin requirements.
						//The default place for the plugin data is the "plugin_data" entity -> the "data" text field, which is used to store stringified JSON
						//containing the plugin settings or version

						//TODO: Develop a way to check for installed plugins
						#region << 1.Get the current ERP database version and checks for other plugin dependencies >>

						if (systemSettings.Version > 0)
						{
							//Do something if database version is not what you expect
						}

						//This plugin needs the webvella-sdk plugin to be installed, so we will check this here
						//var installedPlugins = new PluginService().Plugins;
						//var corePluginFound = false;
						//foreach (var plugin in installedPlugins)
						//{
						//	if (plugin.Name == "webvella-core")
						//	{
						//		corePluginFound = true;
						//		break;
						//	}
						//}

						//if (!corePluginFound)
						//	throw new Exception("'webvella-sdk' plugin is required for the 'webvella-sdk' to operate");

						#endregion

						#region << 2.Get the current plugin settings from the database >>

						var currentPluginSettings = new PluginSettings() { Version = WEBVELLA_SDK_INIT_VERSION };
						string jsonData = GetPluginData();
						if (!string.IsNullOrWhiteSpace(jsonData))
							currentPluginSettings = JsonConvert.DeserializeObject<PluginSettings>(jsonData);

						#endregion

						#region << 3. Run methods based on the current installed version of the plugin >>

						//this patch creates SDK application
						//duplicate this IF for next patches
						if (currentPluginSettings.Version < 20181215)
						{
							try
							{
								currentPluginSettings.Version = 20181215;
								Patch20181215(entMan, relMan, recMan);
							}
							catch (Exception ex)
							{
								var exception = ex;
								throw ex;
							}
						}

						//this patch creates SDK application
						//duplicate this IF for next patches
						if (currentPluginSettings.Version < 20190227)
						{
							try
							{
								currentPluginSettings.Version = 20190227;
								Patch20190227(entMan, relMan, recMan);
							}
							catch (Exception ex)
							{
								var exception = ex;
								throw ex;
							}
						}

						if (currentPluginSettings.Version < 20191205)
						{
							try
							{
								currentPluginSettings.Version = 20191205;
								Patch20191205(entMan, relMan, recMan);
							}
							catch (Exception ex)
							{
								var exception = ex;
								throw ex;
							}
						}

						#endregion


						SavePluginData(JsonConvert.SerializeObject(currentPluginSettings));

						connection.CommitTransaction();
						//connection.RollbackTransaction();
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
