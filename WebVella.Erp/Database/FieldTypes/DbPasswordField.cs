using Newtonsoft.Json;

namespace WebVella.Erp.Database
{
	public class DbPasswordField : DbBaseField
    {
		[JsonProperty(PropertyName = "max_length")]
		public int? MaxLength { get; set; }

		[JsonProperty(PropertyName = "min_length")]
		public int? MinLength { get; set; }

		[JsonProperty(PropertyName = "encrypted")]
		public bool Encrypted { get; set; }
    }
}