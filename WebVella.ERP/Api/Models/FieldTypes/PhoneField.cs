using System;

namespace WebVella.ERP.Api.Models
{
    public class PhoneField : Field
    {
        public static FieldType FieldType { get { return FieldType.PhoneField; } }

        public string DefaultValue { get; set; }

        public string Format { get; set; }

        public int? MaxLength { get; set; }
    }

    public class PhoneFieldMeta : PhoneField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }

		public PhoneFieldMeta(Guid entityId, string entityName, PhoneField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			Format = field.Format;
			MaxLength = field.MaxLength;
		}
	}
}