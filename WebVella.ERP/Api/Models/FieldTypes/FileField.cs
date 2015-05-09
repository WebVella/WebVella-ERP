using System;

namespace WebVella.ERP.Api.Models
{
    public class FileField : Field
    {
        public static FieldType FieldType { get { return FieldType.FileField; } }

        public string DefaultValue { get; set; }
    }

    public class FileFieldMeta : FileField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }

		public FileFieldMeta(Guid entityId, string entityName, FileField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
		}
	}
}