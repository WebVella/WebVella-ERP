using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebVella.ERP.Plugins
{
	public struct PluginLibraries
	{
		[JsonProperty(PropertyName = "css")]
		public List<string> Css { get; private set; }
		
		[JsonProperty(PropertyName = "js")]
		public List<string> Js { get; private set; }
	}

	public struct PluginModule
	{
		[JsonProperty(PropertyName = "wvApp_injects")]
		public List<string> WVAppInjects { get; private set; }

		[JsonProperty(PropertyName = "Css")]
		public List<string> Css { get; private set; }

		[JsonProperty(PropertyName = "Js")]
		public List<string> Js { get; private set; }
	}

	[Serializable]
	public class Plugin
    {
		[JsonProperty(PropertyName = "name")]
		public string Name { get; private set; }
		
		[JsonProperty(PropertyName = "url")]
		public string Url { get; private set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; private set; }

		[JsonProperty(PropertyName = "version")]
		public int Version { get; private set; }

		[JsonProperty(PropertyName = "company")]
		public string Company { get; private set; }

		[JsonProperty(PropertyName = "company_url")]
		public string CompanyUrl { get; private set; }

		[JsonProperty(PropertyName = "author")]
		public string Author { get; private set; }

		[JsonProperty(PropertyName = "repository")]
		public string Repository { get; private set; }

		[JsonProperty(PropertyName = "license")]
		public string License { get; private set; }

		[JsonProperty(PropertyName = "settings_url")]
		public string SettingsUrl { get; private set; }

		[JsonProperty(PropertyName = "icon_url")]
		public string IconUrl { get; private set; }

		[JsonProperty(PropertyName = "load_priority")]
		public int LoadPriority { get; private set; }

		[JsonProperty(PropertyName = "libraries")]
		public PluginLibraries Libraries { get; private set; }

		[JsonProperty(PropertyName = "module")]
		public PluginModule Module { get; private set; }

		[JsonIgnore]
		internal List<Assembly> Assemblies { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
