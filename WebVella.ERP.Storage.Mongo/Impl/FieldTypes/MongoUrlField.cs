using System;

namespace WebVella.ERP.Core
{
    public class MongoUrlField : MongoBaseField, IUrlField
    {
        public new ITextFieldValue DefaultValue { get; set; }
    }
}
