using System;

namespace WebVella.ERP.Api.Models
{
    public class MasterDetailsRelationshipField : Field
    {
        public static FieldType FieldType { get { return FieldType.MasterDetailsRelationshipField; } }

        public Guid? RelatedEntityId { get; set; }
    }

    public class MasterDetailsRelationshipFieldMeta : MasterDetailsRelationshipField
    {
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }
    }
}