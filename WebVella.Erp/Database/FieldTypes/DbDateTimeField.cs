using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Database
{
	public class DbDateTimeField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public DateTime? DefaultValue { get; set; }

		[JsonProperty(PropertyName = "format")]
		public string Format { get; set; }

		[JsonProperty(PropertyName = "use_current_time_as_default_value")]
		public bool UseCurrentTimeAsDefaultValue { get; set; }
    }
}