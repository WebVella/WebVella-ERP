using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebVella.ERP.Database
{
	public class DbSelectField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public string DefaultValue { get; set; }

		[JsonProperty(PropertyName = "options")]
		public IList<DbSelectFieldOption> Options { get; set; }
    }

    public class DbSelectFieldOption
    {
		[JsonProperty(PropertyName = "key")]
		public string Key { get; set; }

		[JsonProperty(PropertyName = "value")]
		public string Value { get; set; }
    }
}