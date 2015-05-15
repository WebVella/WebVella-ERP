using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoGuidField : MongoBaseField, IStorageGuidField
    {
        public Guid DefaultValue { get; set; }
        public bool GenerateNewId { get; set; }
    }
}