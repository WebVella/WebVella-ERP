using System;

namespace WebVella.ERP.Api.Models
{
    public class ImageField : Field
    {
        public static FieldType FieldType { get { return FieldType.ImageField; } }

        public string DefaultValue { get; set; }
    }

    public class ImageFieldMeta : ImageField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }

		public ImageFieldMeta(Guid entityId, string entityName, ImageField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue= field.DefaultValue;
		}
	}
}