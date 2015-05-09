using System;

namespace WebVella.ERP.Api.Models
{
    public class MultiLineTextField : Field
    {
        public static FieldType FieldType { get { return FieldType.MultiLineTextField; } }

        public string DefaultValue { get; set; }

        public int? MaxLength { get; set; }

        public int? VisibleLineNumber { get; set; }
    }

    public class MultiLineTextFieldMeta : MultiLineTextField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }

		public MultiLineTextFieldMeta(Guid entityId, string entityName, MultiLineTextField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			MaxLength = field.MaxLength;
			VisibleLineNumber = field.VisibleLineNumber;
		}
	}
}