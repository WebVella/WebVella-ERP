using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class PhoneField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.PhoneField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }
    }

    public class PhoneFieldMeta : PhoneField
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

		public PhoneFieldMeta(Guid entityId, string entityName, PhoneField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			Format = field.Format;
			MaxLength = field.MaxLength;
		}
	}
}