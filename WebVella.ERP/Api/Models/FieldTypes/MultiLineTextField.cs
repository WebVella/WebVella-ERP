using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class MultiLineTextField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.MultiLineTextField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }

        [JsonProperty(PropertyName = "visibleLineNumber")]
        public int? VisibleLineNumber { get; set; }

        public MultiLineTextField()
        {
        }

        public MultiLineTextField(Field field) : base(field)
        {
        }

        public MultiLineTextField(InputField field) : base(field)
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
                    case "visiblelinenumber":
                        VisibleLineNumber = Convert.ToInt32(property.Value);
                        break;
                }
            }
        }
    }

    public class MultiLineTextFieldMeta : MultiLineTextField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public MultiLineTextFieldMeta(Guid entityId, string entityName, MultiLineTextField field, string parentFieldName = null) : base(field)
        {
            EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			MaxLength = field.MaxLength;
			VisibleLineNumber = field.VisibleLineNumber;
            ParentFieldName = parentFieldName;
        }
	}
}