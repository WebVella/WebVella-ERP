using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoLookupRelationField : MongoBaseField, IStorageLookupRelationField
    {
        public new string DefaultValue { get; set; }

        public Guid Value { get; set; }
    }
}