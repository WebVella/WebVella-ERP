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

        public DateTimeField(InputField field) : base(field)
        {
            DefaultValue = (DateTime?)field["defaultValue"];
            Format = (string)field["format"];
            UseCurrentTimeAsDefaultValue = (bool?)field["useCurrentTimeAsDefaultValue"];
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

        public DateTimeFieldMeta(Guid entityId, string entityName, DateTimeField field, string parentFieldName = null)
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