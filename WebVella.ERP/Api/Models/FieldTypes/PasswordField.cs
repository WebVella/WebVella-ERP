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

        public PasswordField(InputField field) : base(field)
        {
            MaxLength = (int?)field["maxLength"];
            Encrypted = (bool?)field["encrypted"];
            MaskType = (PasswordFieldMaskTypes)field["maskType"];
            MaskCharacter = (char?)field["maskCharacter"];
            foreach (var property in field.GetProperties())
            {
                switch (property.Key.ToLower())
                {
                    case "maxlength":
                        MaxLength = (int?)property.Value;
                        break;
                    case "encrypted":
                        Encrypted = (bool?)property.Value;
                        break;
                    case "masktype":
                        MaskType = (PasswordFieldMaskTypes)property.Value;
                        break;
                    case "maskcharacter":
                        MaskCharacter = (char?)property.Value;
                        break;
                }
            }
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