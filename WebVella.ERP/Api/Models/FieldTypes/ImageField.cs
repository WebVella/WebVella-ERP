using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class ImageField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.ImageField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        public ImageField()
        {
        }

        public ImageField(Field field) : base(field)
        {
        }
    }

    public class ImageFieldMeta : ImageField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public ImageFieldMeta(Guid entityId, string entityName, ImageField field, string parentFieldName = null) : base(field)
        {
            EntityId = entityId;
			EntityName = entityName;
			DefaultValue= field.DefaultValue;
            ParentFieldName = parentFieldName;
        }
	}
}