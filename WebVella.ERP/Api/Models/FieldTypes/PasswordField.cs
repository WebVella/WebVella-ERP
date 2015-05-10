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
        public bool Encrypted { get; set; }

        [JsonProperty(PropertyName = "maskType")]
        public PasswordFieldMaskTypes MaskType { get; set; }

        [JsonProperty(PropertyName = "maskCharacter")]
        public char? MaskCharacter { get; set; }
    }

    public class PasswordFieldMeta : PasswordField
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

		public PasswordFieldMeta(Guid entityId, string entityName, PasswordField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			Encrypted = field.Encrypted;
			MaskType = field.MaskType;
			MaskCharacter = field.MaskCharacter;
		}
	}
}