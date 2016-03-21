using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebVella.ERP.Database
{
	public class DbMultiSelectField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public IEnumerable<string> DefaultValue { get; set; }

		[JsonProperty(PropertyName = "options")]
		public IList<DbMultiSelectFieldOption> Options { get; set; }
    }

    public class DbMultiSelectFieldOption
    {
		[JsonProperty(PropertyName = "key")]
		public string Key { get; set; }

		[JsonProperty(PropertyName = "value")]
		public string Value { get; set; }
    }
}