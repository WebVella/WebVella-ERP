using System;

namespace WebVella.ERP.Api.Models
{
    public class DateField : Field
    {
        public static FieldType FieldType { get { return FieldType.DateField; } }

        public DateTime? DefaultValue { get; set; }

        public string Format { get; set; }
    }

    public class DateFieldMeta : DateField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }

		public DateFieldMeta(Guid entityId, string entityName, DateField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			Format = field.Format;
		}
	}
}