using Newtonsoft.Json;

namespace WebVella.Erp.Database
{
	public class DbNumberField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public decimal? DefaultValue { get; set; }

		[JsonProperty(PropertyName = "min_value")]
		public decimal? MinValue { get; set; }

		[JsonProperty(PropertyName = "max_value")]
		public decimal? MaxValue { get; set; }

		[JsonProperty(PropertyName = "decimal_places")]
		public byte DecimalPlaces { get; set; }
    }
}