using System;

namespace WebVella.ERP.Core
{
    public class MongoPrimaryKeyField : MongoBaseField, IPrimaryKeyField
    {
        public new IGuidFieldValue DefaultValue { get; set; }
    }
}