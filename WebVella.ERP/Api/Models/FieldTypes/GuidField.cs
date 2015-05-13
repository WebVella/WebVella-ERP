using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class GuidField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.GuidField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public Guid? DefaultValue { get; set; }

        public GuidField()
        {
        }

        public GuidField(Field field) : base(field)
        {
        }

        public GuidField(InputField field) : base(field)
        {
            foreach (var property in field.GetProperties())
            {
                switch (property.Key.ToLower())
                {
                    case "defaultvalue":
                        DefaultValue = (Guid?)property.Value;
                        break;
                }
            }
        }
    }

    public class PrimaryKeyFieldMeta : GuidField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public PrimaryKeyFieldMeta(Guid entityId, string entityName, GuidField field, string parentFieldName = null) : base(field)
        {
            EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
            ParentFieldName = parentFieldName;
        }
	}
}