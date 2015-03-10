using System;

namespace WebVella.ERP.Core
{
    public class MongoLookupRelationField : MongoBaseField, ILookupRelationField
    {
        public new IRelationFieldValue DefaultValue { get; set; }
    }
}