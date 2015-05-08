using System;

namespace WebVella.ERP.Api.Models
{
    public class PasswordField : Field
    {
        public int? MaxLength { get; set; }

        bool Encrypted { get; set; }

        public PasswordFieldMaskTypes MaskType { get; set; }

        public char? MaskCharacter { get; set; }
    }
}