using System;

namespace WebVella.ERP.Core
{
    public class MongoMasterDetailsRelationshipField : MongoBaseField, IMasterDetailsRelationshipField
    {
        public new IRelationFieldValue DefaultValue { get; set; }
    }
}