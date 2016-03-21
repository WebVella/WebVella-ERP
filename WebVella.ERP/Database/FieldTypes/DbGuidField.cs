using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Database
{
    public class DbGuidField : DbBaseField
    {
		[JsonProperty(PropertyName = "default_value")]
		public Guid? DefaultValue { get; set; }

		[JsonProperty(PropertyName = "generate_new_id")]
		public bool? GenerateNewId { get; set; }
    }
}