using System;

namespace WebVella.ERP.Core
{
    public class MongoPrimaryKeyField : MongoBaseField, IPrimaryKeyField
    {
        public new Guid DefaultValue { get; set; }

        public Guid Value { get; set; }
    }
}