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

        public PhoneField()
        {
        }

        public PhoneField(InputField field) : base(field)
        {
            DefaultValue = (string)field["defaultValue"];
            Format = (string)field["format"];
            MaxLength = (int?)field["maxLength"];
        }
    }

    public class PhoneFieldMeta : PhoneField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public PhoneFieldMeta(Guid entityId, string entityName, PhoneField field, string parentFieldName = null)
        {
            EntityId = entityId;
			EntityName = entityName;
			Format = field.Format;
			MaxLength = field.MaxLength;
            ParentFieldName = parentFieldName;
        }
	}
}