using System;

namespace WebVella.ERP.Core
{
    public class MongoPhoneField : MongoBaseField, IPhoneField
    {
        public new string DefaultValue { get; set; }

        public string Format { get; set; }

        public int MaxLength { get; set; }

        public string Value { get; set; }
    }
}