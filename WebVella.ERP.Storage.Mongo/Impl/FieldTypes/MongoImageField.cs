using System;

namespace WebVella.ERP.Core
{
    public class MongoImageField : MongoBaseField, IImageField
    {
        public new IFileFieldValue DefaultValue { get; set; }
    }
}