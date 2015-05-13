using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class TextField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.TextField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }

        public TextField()
        {
        }

        public TextField(Field field) : base(field)
        {
        }

        public TextField(InputField field) : base(field)
        {
            foreach (var property in field.GetProperties())
            {
                switch (property.Key.ToLower())
                {
                    case "defaultvalue":
                        DefaultValue = Convert.ToString(property.Value);
                        break;
                    case "maxlength":
                        MaxLength = Convert.ToInt32(property.Value);
                        break;
                }
            }
        }
    }

    public class TextFieldMeta : TextField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public TextFieldMeta(Guid entityId, string entityName, TextField field, string parentFieldName = null) : base(field)
        {
            EntityId = entityId;
            EntityName = entityName;
            DefaultValue = field.DefaultValue;
            MaxLength = field.MaxLength;
            ParentFieldName = parentFieldName;
        }
    }
}