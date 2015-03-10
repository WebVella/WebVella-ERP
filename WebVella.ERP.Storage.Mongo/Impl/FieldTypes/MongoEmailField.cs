using System;

namespace WebVella.ERP.Core
{
    public class MongoEmailField : MongoBaseField, IEmailField
    {
        public new ITextFieldValue DefaultValue { get; set; }
    }
}