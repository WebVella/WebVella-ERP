using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class DateTimeField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.DateTimeField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public DateTime? DefaultValue { get; set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }
    }

    public class DateTimeFieldMeta : DateTimeField
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }
		
		public DateTimeFieldMeta(Guid entityId, string entityName, DateTimeField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			Format = field.Format;
		}
	}
}