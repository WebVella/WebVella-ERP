using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class PhoneField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.PhoneField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }

        public PhoneField()
        {
        }

        public PhoneField(Field field) : base(field)
        {
        }

        public PhoneField(InputField field) : base(field)
        {
            foreach (var property in field.GetProperties())
            {
                switch (property.Key.ToLower())
                {
                    case "defaultvalue":
                        DefaultValue = (string)property.Value;
                        break;
                    case "format":
                        Format = (string)property.Value;
                        break;
                    case "MaxLength":
                        MaxLength = (int?)property.Value;
                        break;
                }
            }
        }
    }

    public class PhoneFieldMeta : PhoneField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public PhoneFieldMeta(Guid entityId, string entityName, PhoneField field, string parentFieldName = null) : base(field)
        {
            EntityId = entityId;
			EntityName = entityName;
			Format = field.Format;
			MaxLength = field.MaxLength;
            ParentFieldName = parentFieldName;
        }
	}
}