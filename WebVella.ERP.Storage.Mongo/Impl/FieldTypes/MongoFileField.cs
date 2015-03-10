using System;

namespace WebVella.ERP.Core
{
    public class MongoFileField : MongoBaseField, IFileField
    {
        public new IFileFieldValue DefaultValue { get; set; }
    }
}