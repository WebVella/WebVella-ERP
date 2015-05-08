using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Core
{
    public class MongoPrimaryKeyField : MongoBaseField, IStoragePrimaryKeyField
    {
        public Guid DefaultValue { get; set; }
    }
}