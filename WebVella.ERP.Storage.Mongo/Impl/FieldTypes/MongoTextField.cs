using System;

namespace WebVella.ERP.Core
{
    public class MongoTextField : MongoBaseField, ITextField
    {
        public new string DefaultValue { get; set; }

        public int MaxLength { get; set; }

        public string Value { get; set; }
    }
}