using System;

namespace WebVella.ERP.Core
{
    public class MongoMasterDetailsRelationshipField : MongoBaseField, IMasterDetailsRelationshipField
    {
        public new string DefaultValue { get; set; }

        public string Value { get; set; }
    }
}