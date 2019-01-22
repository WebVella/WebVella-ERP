using Newtonsoft.Json;

namespace WebVella.Erp.Database
{
	public class DbFileField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public string DefaultValue { get; set; }
    }
}