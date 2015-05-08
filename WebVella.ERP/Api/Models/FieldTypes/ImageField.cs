using System;

namespace WebVella.ERP.Api.Models
{
    public class ImageField : Field
    {
        public static FieldType FieldType { get { return FieldType.ImageField; } }

        public string DefaultValue { get; set; }

        public string TargetEntityType { get; set; }

        public string RelationshipName { get; set; }
    }

    public class ImageFieldMeta : ImageField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }
    }
}