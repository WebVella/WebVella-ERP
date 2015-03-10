using System;

namespace WebVella.ERP.Core
{
    public class MongoCheckboxField : MongoBaseField, ICheckboxField
    {
        public new IBooleanFieldValue DefaultValue { get; set; }
    }
}