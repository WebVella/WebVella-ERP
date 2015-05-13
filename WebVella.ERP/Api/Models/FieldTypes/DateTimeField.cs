using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class DateTimeField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.DateTimeField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public DateTime? DefaultValue { get; set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }

        [JsonProperty(PropertyName = "useCurrentTimeAsDefaultValue")]
        public bool? UseCurrentTimeAsDefaultValue { get; set; }

        public DateTimeField()
        {
        }

        public DateTimeField(Field field) : base(field)
        {
        }

        public DateTimeField(InputField field) : base(field)
        {
            foreach (var property in field.GetProperties())
            {
                switch (property.Key.ToLower())
                {
                    case "defaultvalue":
                        DefaultValue = Convert.ToDateTime(property.Value);
                        break;
                    case "format":
                        Format = Convert.ToString(property.Value);
                        break;
                    case "usecurrenttimeasdefaultvalue":
                        UseCurrentTimeAsDefaultValue = Convert.ToBoolean(property.Value);
                        break;
                }
            }
        }
    }

    public class DateTimeFieldMeta : DateTimeField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public DateTimeFieldMeta(Guid entityId, string entityName, DateTimeField field, string parentFieldName = null) : base(field)
        {
            EntityId = entityId;
            EntityName = entityName;
            DefaultValue = field.DefaultValue;
            Format = field.Format;
            UseCurrentTimeAsDefaultValue = field.UseCurrentTimeAsDefaultValue;
            ParentFieldName = parentFieldName;
        }
    }
}