using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoMasterDetailsRelationshipField : MongoBaseField, IStorageMasterDetailsRelationshipField
    {
        public new string DefaultValue { get; set; }

        public Guid Value { get; set; }
    }
}