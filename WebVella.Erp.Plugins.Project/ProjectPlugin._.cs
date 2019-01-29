using Newtonsoft.Json;
using System;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.Project.Model;

namespace WebVella.Erp.Plugins.Project
{
	public partial class ProjectPlugin : ErpPlugin
	{
		private const int WEBVELLA_PROJECT_INIT_VERSION = 20190101;

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

						#endregion

						#region << 2.Get the current plugin settings from the database >>

						var currentPluginSettings = new PluginSettings() { Version = WEBVELLA_PROJECT_INIT_VERSION };
						string jsonData = GetPluginData();
						if (!string.IsNullOrWhiteSpace(jsonData))
							currentPluginSettings = JsonConvert.DeserializeObject<PluginSettings>(jsonData);

						#endregion

						#region << 3. Run methods based on the current installed version of the plugin >>

						//duplicate this IF for next patches
						{
							var patchVersion = 20190123;
							if (currentPluginSettings.Version < patchVersion)
							{
								try
								{
									currentPluginSettings.Version = patchVersion;
									Patch20190123(entMan, relMan, recMan);
								}
								catch (ValidationException ex)
								{
									var exception = ex;
									throw ex;
								}
								catch (Exception ex)
								{
									var exception = ex;
									throw ex;
								}
							}
						}
						//duplicate this IF for next patches
						{
							var patchVersion = 20190124;
							if (currentPluginSettings.Version < patchVersion)
							{
								try
								{
									currentPluginSettings.Version = patchVersion;
									Patch20190124(entMan, relMan, recMan);
								}
								catch (ValidationException ex)
								{
									var exception = ex;
									throw ex;
								}
								catch (Exception ex)
								{
									var exception = ex;
									throw ex;
								}
							}
						}
						{
							var patchVersion = 20190125;
							if (currentPluginSettings.Version < patchVersion)
							{
								try
								{
									currentPluginSettings.Version = patchVersion;
									Patch20190125(entMan, relMan, recMan);
								}
								catch (ValidationException ex)
								{
									var exception = ex;
									throw ex;
								}
								catch (Exception ex)
								{
									var exception = ex;
									throw ex;
								}
							}
						}

						//Patch 20190126
						{
							var patchVersion = 20190126;
							if (currentPluginSettings.Version < patchVersion)
							{
								try
								{
									currentPluginSettings.Version = patchVersion;
									Patch20190126(entMan, relMan, recMan);
								}
								catch (ValidationException ex)
								{
									var exception = ex;
									throw ex;
								}
								catch (Exception ex)
								{
									var exception = ex;
									throw ex;
								}
							}
						}

						//Patch 20190127
						{
							var patchVersion = 20190127;
							if (currentPluginSettings.Version < patchVersion)
							{
								try
								{
									currentPluginSettings.Version = patchVersion;
									Patch20190127(entMan, relMan, recMan);
								}
								catch (ValidationException ex)
								{
									var exception = ex;
									throw ex;
								}
								catch (Exception ex)
								{
									var exception = ex;
									throw ex;
								}
							}
						}

						//Patch 20190128
						{
							var patchVersion = 20190128;
							if (currentPluginSettings.Version < patchVersion)
							{
								try
								{
									currentPluginSettings.Version = patchVersion;
									Patch20190128(entMan, relMan, recMan);
								}
								catch (ValidationException ex)
								{
									var exception = ex;
									throw ex;
								}
								catch (Exception ex)
								{
									var exception = ex;
									throw ex;
								}
							}
						}
						#endregion


						SavePluginData(JsonConvert.SerializeObject(currentPluginSettings));

						connection.CommitTransaction();
						//connection.RollbackTransaction();
					}
					catch (ValidationException ex)
					{
						connection.RollbackTransaction();
						throw ex;
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
