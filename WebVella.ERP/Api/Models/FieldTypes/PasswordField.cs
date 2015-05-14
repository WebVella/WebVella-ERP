using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class PasswordField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.PasswordField; } }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }

        [JsonProperty(PropertyName = "encrypted")]
        public bool? Encrypted { get; set; }

        [JsonProperty(PropertyName = "maskType")]
        public PasswordFieldMaskTypes MaskType { get; set; }

        [JsonProperty(PropertyName = "maskCharacter")]
        public char? MaskCharacter { get; set; }

        public PasswordField()
        {
        }

        public PasswordField(Field field) : base(field)
        {
        }
    }

    public class PasswordFieldMeta : PasswordField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public PasswordFieldMeta(Guid entityId, string entityName, PasswordField field, string parentFieldName = null) : base(field)
        {
            EntityId = entityId;
			EntityName = entityName;
			Encrypted = field.Encrypted;
			MaskType = field.MaskType;
			MaskCharacter = field.MaskCharacter;
            ParentFieldName = parentFieldName;
        }
	}
}