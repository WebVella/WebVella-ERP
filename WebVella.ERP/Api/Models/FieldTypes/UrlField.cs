using System;

namespace WebVella.ERP.Api.Models
{
    public class UrlField : Field
    {
        public static FieldType FieldType { get { return FieldType.UrlField; } }

        public string DefaultValue { get; set; }

        public int? MaxLength { get; set; }

        public bool? OpenTargetInNewWindow { get; set; }
    }

    public class UrlFieldMeta : UrlField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }

		public UrlFieldMeta(Guid entityId, string entityName, UrlField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			MaxLength = field.MaxLength;
			OpenTargetInNewWindow = field.OpenTargetInNewWindow;
		}
	}
}
