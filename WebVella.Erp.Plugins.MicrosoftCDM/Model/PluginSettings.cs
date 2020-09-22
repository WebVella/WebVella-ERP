using Newtonsoft.Json;

namespace WebVella.Erp.Plugins.MicrosoftCDM.Model
{
	internal class PluginSettings
	{
		[JsonProperty(PropertyName = "version")]
		public int Version { get; set; }
	}
}
