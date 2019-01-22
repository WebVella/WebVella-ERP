using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebVella.Erp.Database
{
	public class DbMultiSelectField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public IEnumerable<string> DefaultValue { get; set; }

		[JsonProperty(PropertyName = "options")]
		public IList<DbSelectOption> Options { get; set; }
    }

}