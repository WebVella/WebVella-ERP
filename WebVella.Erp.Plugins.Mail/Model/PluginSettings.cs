using Newtonsoft.Json;

namespace WebVella.Erp.Plugins.Mail.Model
{
	internal class PluginSettings
	{
		[JsonProperty(PropertyName = "version")]
		public int Version { get; set; }
	}
}
