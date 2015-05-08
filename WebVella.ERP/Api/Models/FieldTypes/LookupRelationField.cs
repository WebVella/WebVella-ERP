using System;

namespace WebVella.ERP.Api.Models
{
    public class LookupRelationField : Field
    {
        public static FieldType FieldType { get { return FieldType.LookupRelationField; } }

        public Guid? RelatedEntityId { get; set; }
    }

    public class LookupRelationFieldMeta : LookupRelationField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }

		public LookupRelationFieldMeta(Guid entityId, string entityName, LookupRelationField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			RelatedEntityId = field.RelatedEntityId;
		}
	}
}