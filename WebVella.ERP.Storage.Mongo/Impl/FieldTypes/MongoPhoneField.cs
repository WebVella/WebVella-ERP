using System;

namespace WebVella.ERP.Core
{
    public class MongoPhoneField : MongoBaseField, IPhoneField
    {
        public new ITextFieldValue DefaultValue { get; set; }
    }
}