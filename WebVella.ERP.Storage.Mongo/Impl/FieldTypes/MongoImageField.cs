using System;

namespace WebVella.ERP.Core
{
    public class MongoImageField : MongoBaseField, IImageField
    {
        public new string DefaultValue { get; set; }

        public string TargetEntityType { get; set; }

        public string RelationshipName { get; set; }

        public string Value { get; set; }
    }
}