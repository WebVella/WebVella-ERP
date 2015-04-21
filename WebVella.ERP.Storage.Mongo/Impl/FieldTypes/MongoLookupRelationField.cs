using System;

namespace WebVella.ERP.Core
{
    public class MongoLookupRelationField : MongoBaseField, ILookupRelationField
    {
        public new string DefaultValue { get; set; }

        public Guid Value { get; set; }
    }
}