using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class LookupRelationField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.LookupRelationField; } }

        [JsonProperty(PropertyName = "relatedEntityId")]
        public Guid? RelatedEntityId { get; set; }
    }

    public class LookupRelationFieldMeta : LookupRelationField
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

		public LookupRelationFieldMeta(Guid entityId, string entityName, LookupRelationField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			RelatedEntityId = field.RelatedEntityId;
		}
	}
}