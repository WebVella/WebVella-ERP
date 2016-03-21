using Newtonsoft.Json;

namespace WebVella.ERP.Database
{
	public class DbEmailField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "max_length")]
		public int? MaxLength { get; set; }
    }
}