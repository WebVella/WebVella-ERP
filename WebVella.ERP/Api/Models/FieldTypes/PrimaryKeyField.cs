using System;

namespace WebVella.ERP.Api.Models
{
    public class PrimaryKeyField : Field
    {
        public static FieldType FieldType { get { return FieldType.PrimaryKeyField; } }

        public Guid? DefaultValue { get; set; }
    }

    public class PrimaryKeyFieldMeta : PrimaryKeyField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }

		public PrimaryKeyFieldMeta(Guid entityId, string entityName, PrimaryKeyField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
		}
	}
}