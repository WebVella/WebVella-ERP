using System;

namespace WebVella.ERP.Core
{
    public class MongoHtmlField : MongoBaseField, IHtmlField
    {
        public new ITextFieldValue DefaultValue { get; set; }
    }
}