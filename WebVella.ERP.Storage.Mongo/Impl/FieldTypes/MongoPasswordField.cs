using System;

namespace WebVella.ERP.Core
{
    public class MongoPasswordField : MongoBaseField, IPasswordField
    {
        public new ITextFieldValue DefaultValue { get; set; }
    }
}