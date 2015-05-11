using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class FileField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.FileField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        public FileField()
        {
        }

        public FileField(InputField field) : base(field)
        {
            DefaultValue = (string)field["defaultValue"];
        }
    }

    public class FileFieldMeta : FileField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public FileFieldMeta(Guid entityId, string entityName, FileField field, string parentFieldName = null)
        {
            EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
            ParentFieldName = parentFieldName;
        }
	}
}