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

        public SelectField(Field field) : base(field)
        {
        }

        public SelectField(InputField field) : base(field)
        {
            foreach (var property in field.GetProperties())
            {
                switch (property.Key.ToLower())
                {
                    case "defaultvalue":
                        DefaultValue = Convert.ToString(property.Value);
                        break;
                    case "options":
                        Options = (IDictionary<string, string>)property.Value;
                        break;
                }
            }
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

        public SelectFieldMeta(Guid entityId, string entityName, SelectField field, string parentFieldName = null) : base(field)
        {
            EntityId = entityId;
            EntityName = entityName;
            DefaultValue = field.DefaultValue;
            Options = field.Options;
            ParentFieldName = parentFieldName;
        }
    }
}