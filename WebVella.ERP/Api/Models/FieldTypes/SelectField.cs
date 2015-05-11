using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
    public class SelectField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.SelectField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "options")]
        public IDictionary<string, string> Options { get; set; }

        public SelectField()
        {
        }

        public SelectField(InputField field) : base(field)
        {
            DefaultValue = (string)field["defaultValue"];
            Options = (IDictionary<string, string>)field["options"];
        }
    }

    public class SelectFieldMeta : SelectField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public SelectFieldMeta(Guid entityId, string entityName, SelectField field, string parentFieldName = null)
        {
            EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			Options = field.Options;
            ParentFieldName = parentFieldName;
        }
	}
}