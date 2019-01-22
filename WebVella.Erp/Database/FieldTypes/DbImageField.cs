using Newtonsoft.Json;

namespace WebVella.Erp.Database
{
	public class DbImageField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public string DefaultValue { get; set; }
    }
}