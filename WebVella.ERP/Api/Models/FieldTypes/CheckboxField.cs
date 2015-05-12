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

        public CheckboxField()
        {
        }

        public CheckboxField(Field field) : base(field)
        {
        }

        public CheckboxField(InputField field) : base(field)
        {
            DefaultValue = (bool?)field["defaultValue"];
        }
    }

    public class CheckboxFieldMeta : CheckboxField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public CheckboxFieldMeta(Guid entityId, string entityName, CheckboxField field, string parentFieldName = null) : base(field)
        {
            EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
            ParentFieldName = parentFieldName;
        }
	}
}