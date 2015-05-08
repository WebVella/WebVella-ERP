using System;

namespace WebVella.ERP.Api.Models
{
    public class PasswordField : Field
    {
        public static FieldType FieldType { get { return FieldType.PasswordField; } }

        public int? MaxLength { get; set; }

        bool Encrypted { get; set; }

        public PasswordFieldMaskTypes MaskType { get; set; }

        public char? MaskCharacter { get; set; }
    }

    public class PasswordFieldMeta : PasswordField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }
    }
}