using Newtonsoft.Json;

namespace WebVella.Erp.Database
{
	public class DbUrlField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "max_length")]
		public int? MaxLength { get; set; }

		[JsonProperty(PropertyName = "open_target_in_new_window")]
		public bool OpenTargetInNewWindow { get; set; }
    }
}
