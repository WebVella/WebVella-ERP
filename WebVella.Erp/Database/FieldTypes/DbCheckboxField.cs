using Newtonsoft.Json;


namespace WebVella.Erp.Database
{
    public class DbCheckboxField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public bool DefaultValue { get; set; }
    }
}