using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class PercentField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.PercentField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public decimal? DefaultValue { get; set; }

        [JsonProperty(PropertyName = "minValue")]
        public decimal? MinValue { get; set; }

        [JsonProperty(PropertyName = "maxValue")]
        public decimal? MaxValue { get; set; }

        [JsonProperty(PropertyName = "decimalPlaces")]
        public byte? DecimalPlaces { get; set; }

        public PercentField()
        {
        }

        public PercentField(InputField field) : base(field)
        {
            DefaultValue = (decimal?)field["defaultValue"];
            MinValue = (decimal?)field["minValue"];
            MaxValue = (decimal?)field["maxValue"];
            DecimalPlaces = (byte?)field["decimalPlaces"];
        }
    }

    public class PercentFieldMeta : PercentField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public PercentFieldMeta(Guid entityId, string entityName, PercentField field, string parentFieldName = null)
        {
            EntityId = entityId;
			EntityName = entityName;
			MinValue = field.MinValue;
			MaxValue= field.MaxValue;
			DecimalPlaces = field.DecimalPlaces;
            ParentFieldName = parentFieldName;
        }
	}
}