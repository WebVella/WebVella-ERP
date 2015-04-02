using System;

namespace WebVella.ERP.Core
{
    public class MongoMultiLineTextField : MongoBaseField, IMultiLineTextField
    {
        public new string DefaultValue { get; set; }

        public int LineNumber { get; set; }

        public string Value { get; set; }
    }
}