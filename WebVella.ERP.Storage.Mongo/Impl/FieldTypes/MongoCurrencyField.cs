using System;

namespace WebVella.ERP.Core
{
    public class MongoCurrencyField : MongoBaseField, ICurrencyField
    {
        public new INumberFieldValue DefaultValue { get; set; }
    }
}