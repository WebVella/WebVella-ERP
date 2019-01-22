using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Database
{
    public class DbGuidField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public Guid? DefaultValue { get; set; }

		[JsonProperty(PropertyName = "generate_new_id")]
		public bool? GenerateNewId { get; set; }
    }
}