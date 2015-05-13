using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class UrlField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.UrlField; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }

        [JsonProperty(PropertyName = "openTargetInNewWindow")]
        public bool? OpenTargetInNewWindow { get; set; }

        public UrlField()
        {
        }

        public UrlField(Field field) : base(field)
        {
        }

        public UrlField(InputField field) : base(field)
        {
            foreach (var property in field.GetProperties())
            {
                switch (property.Key.ToLower())
                {
                    case "defaultvalue":
                        DefaultValue = (string)property.Value;
                        break;
                    case "maxlength":
                        MaxLength = (int?)property.Value;
                        break;
                    case "opentargetinnewwindow":
                        OpenTargetInNewWindow = (bool?)property.Value;
                        break;
                }
            }
        }
    }

    public class UrlFieldMeta : UrlField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public UrlFieldMeta(Guid entityId, string entityName, UrlField field, string parentFieldName = null ) : base(field)
        {
            EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			MaxLength = field.MaxLength;
			OpenTargetInNewWindow = field.OpenTargetInNewWindow;
            ParentFieldName = parentFieldName;
        }
	}
}
