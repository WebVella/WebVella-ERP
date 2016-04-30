using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Database;
using WebVella.ERP.Plugins;
using WebVella.ERP.Project.Models;

namespace WebVella.ERP.Project
{
	[PluginStartup]
	public class Startup
	{
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
				//Here we need to initialize or update the environment based on the plugin requirements.
				//The default place for the plugin data is the "plugin_data" entity -> the "data" text field, which is used to store stringified JSON
				//containing the plugin settings or version

				//1.Get the current ERP database version and checks
				if (systemSettings.Version > 0)
				{
					//Do something if database version is not what you expect
				}
				//2.Get the current plugin settings from the database
				var currentPluginSettings = new PluginSettings();
				QueryObject pluginDataQueryObject = EntityQuery.QueryEQ("name", "webvella-project");
				var pluginDataQuery = new EntityQuery("plugin_data", "*", pluginDataQueryObject);
				var pluginDataQueryResponse = recMan.Find(pluginDataQuery);
				if (!pluginDataQueryResponse.Success)
					throw new Exception("plugin 'webvella-project' failed to get its settings due to: " + pluginDataQueryResponse.Message);

				if (pluginDataQueryResponse.Object == null || !pluginDataQueryResponse.Object.Data.Any() || pluginDataQueryResponse.Object.Data[0]["data"] == DBNull.Value)
				{
					//plugin was not installed
					currentPluginSettings.Version = 20160430;
					string json = JsonConvert.SerializeObject(currentPluginSettings);
					var settingsEntityRecord = new EntityRecord();
					settingsEntityRecord["id"] = Guid.NewGuid();
					settingsEntityRecord["name"] = "webvella-project";
					settingsEntityRecord["data"] = json;
					var settingsSaveReponse = recMan.CreateRecord("plugin_data", settingsEntityRecord);
					if (!settingsSaveReponse.Success)
						throw new Exception("plugin 'webvella-project' failed to save its settings in the database due to: " + pluginDataQueryResponse.Message);
				}
				else
				{
					string json = (string)((List<EntityRecord>)pluginDataQueryResponse.Object.Data)[0]["data"];
					currentPluginSettings = JsonConvert.DeserializeObject<PluginSettings>(json);
				}

				//3. Run methods based on the 
			}
		}
	}
}
