using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class HtmlField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.HtmlField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        public HtmlField()
        {
        }

        public HtmlField(InputField field) : base(field)
        {
            DefaultValue = (string)field["defaultValue"];
        }
    }

    public class HtmlFieldMeta : HtmlField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public HtmlFieldMeta(Guid entityId, string entityName, HtmlField field, string parentFieldName = null)
        {
            EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
            ParentFieldName = parentFieldName;
        }
	}
}