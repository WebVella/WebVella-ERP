using Newtonsoft.Json;

namespace WebVella.Erp.Database
{
	public class DbPhoneField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "format")]
		public string Format { get; set; }

		[JsonProperty(PropertyName = "max_length")]
		public int? MaxLength { get; set; }
    }
}