using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class CheckboxField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.CheckboxField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public bool? DefaultValue { get; set; }
    }

    public class CheckboxFieldMeta : CheckboxField
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

		public CheckboxFieldMeta(Guid entityId, string entityName, CheckboxField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
		}
	}
}