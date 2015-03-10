using System;

namespace WebVella.ERP.Core
{
    public class MongoMultiLineTextField : MongoBaseField, IMultiLineTextField
    {
        public new ITextFieldValue DefaultValue { get; set; }
    }
}