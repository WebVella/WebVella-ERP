using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
	public class MultiSelectField : Field
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.MultiSelectField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public IEnumerable<string> DefaultValue { get; set; }

        [JsonProperty(PropertyName = "options")]
        public IDictionary<string, string> Options { get; set; }
	}

	public class MultiSelectFieldMeta : MultiSelectField
	{
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

		public MultiSelectFieldMeta(Guid entityId, string entityName, MultiSelectField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			Options = field.Options;
		}
	}
}