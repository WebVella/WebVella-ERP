using System;

namespace WebVella.ERP.Core
{
    public class MongoSelectField : MongoBaseField, ISelectField
    {
        public new ITextFieldValue DefaultValue { get; set; }
    }
}