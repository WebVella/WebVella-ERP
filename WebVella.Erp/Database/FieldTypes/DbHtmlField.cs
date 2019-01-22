using Newtonsoft.Json;

namespace WebVella.Erp.Database
{
	public class DbHtmlField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public string DefaultValue { get; set; }
    }
}