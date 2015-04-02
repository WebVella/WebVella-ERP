using System;

namespace WebVella.ERP.Core
{
    public class MongoPasswordField : MongoBaseField, IPasswordField
    {
        public new string DefaultValue { get; set; }

        public int MaxLength { get; set; }

        public MaskTypes MaskType { get; set; }

        public char MaskCharacter { get; set; }

        public string Value { get; set; }
    }
}