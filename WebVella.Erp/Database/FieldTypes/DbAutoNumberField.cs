
using Newtonsoft.Json;

namespace WebVella.Erp.Database
{
    public class DbAutoNumberField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public decimal? DefaultValue { get; set; }

		[JsonProperty(PropertyName = "display_format")]
		public string DisplayFormat { get; set; }

		[JsonProperty(PropertyName = "starting_number")]
		public decimal? StartingNumber { get; set; }
    }
}