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

        public PercentField(Field field) : base(field)
        {
        }

        public PercentField(InputField field) : base(field)
        {
            foreach (var property in field.GetProperties())
            {
                switch (property.Key.ToLower())
                {
                    case "defaultvalue":
                        DefaultValue = Convert.ToDecimal(property.Value);
                        break;
                    case "minvalue":
                        MinValue = Convert.ToDecimal(property.Value);
                        break;
                    case "maxvalue":
                        MaxValue = Convert.ToDecimal(property.Value);
                        break;
                    case "decimalplaces":
                        DecimalPlaces = Convert.ToByte(property.Value);
                        break;
                }
            }
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

        public PercentFieldMeta(Guid entityId, string entityName, PercentField field, string parentFieldName = null) : base(field)
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