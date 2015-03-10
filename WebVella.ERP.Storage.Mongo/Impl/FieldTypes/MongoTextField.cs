using System;

namespace WebVella.ERP.Core
{
    public class MongoTextField : MongoBaseField, ITextField
    {
        public new ITextFieldValue DefaultValue { get; set; }
    }
}