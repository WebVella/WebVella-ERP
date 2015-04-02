using System;

namespace WebVella.ERP.Core
{
    public class MongoFileField : MongoBaseField, IFileField
    {
        public new string DefaultValue { get; set; }

        public string Value { get; set; }
    }
}