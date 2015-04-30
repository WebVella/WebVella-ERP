using System;

namespace WebVella.ERP.Api.Models
{
    public class PasswordField : Field
    {
        public new string DefaultValue { get; set; }

        public int? MaxLength { get; set; }

        public PasswordFieldMaskTypes MaskType { get; set; }

        public char? MaskCharacter { get; set; }
    }
}