using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class EmailField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.EmailField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }
    }

    public class EmailFieldMeta : EmailField
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

		public EmailFieldMeta(Guid entityId, string entityName, EmailField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			MaxLength = field.MaxLength;
		}
	}
}