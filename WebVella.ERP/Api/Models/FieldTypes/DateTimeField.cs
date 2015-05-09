using System;

namespace WebVella.ERP.Api.Models
{
    public class DateTimeField : Field
    {
        public static FieldType FieldType { get { return FieldType.DateTimeField; } }

        public DateTime? DefaultValue { get; set; }

        public string Format { get; set; }
    }

    public class DateTimeFieldMeta : DateTimeField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }
		
		public DateTimeFieldMeta(Guid entityId, string entityName, DateTimeField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			Format = field.Format;
		}
	}
}