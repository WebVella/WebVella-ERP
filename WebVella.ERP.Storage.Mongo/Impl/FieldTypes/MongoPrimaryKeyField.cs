using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoPrimaryKeyField : MongoBaseField, IStoragePrimaryKeyField
    {
        public Guid DefaultValue { get; set; }
    }
}