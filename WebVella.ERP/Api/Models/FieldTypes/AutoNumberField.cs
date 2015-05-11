using System;
using Newtonsoft.Json;

namespace WebVella.ERP.Api.Models
{
	public class AutoNumberField : Field
	{
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.AutoNumberField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public decimal? DefaultValue { get; set; }

        [JsonProperty(PropertyName = "displayFormat")]
        public string DisplayFormat { get; set; }

        [JsonProperty(PropertyName = "startingNumber")]
        public decimal? StartingNumber { get; set; }

        public AutoNumberField()
        {

        }

        public AutoNumberField(InputField field) : base(field)
        {
            DefaultValue = (decimal)field["defaultValue"];
            DisplayFormat = (string)field["displayFormat"];
            StartingNumber = (decimal)field["startingNumber"];
        }
    }

    public class AutoNumberFieldMeta : AutoNumberField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public AutoNumberFieldMeta(Guid entityId, string entityName, AutoNumberField field, string parentFieldName = null)
        {
            EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			DisplayFormat = field.DisplayFormat;
			StartingNumber = field.StartingNumber;
            ParentFieldName = parentFieldName;
        }
	}
}