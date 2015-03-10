using System;

namespace WebVella.ERP.Core
{
    public class MongoMultiSelectField : MongoBaseField, IMultiSelectField
    {
        public new ITextArrayFieldValue DefaultValue { get; set; }
    }
}