using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class MasterDetailsRelationshipField : Field
    {
        [JsonProperty(PropertyName = "fieldType")]
        public static FieldType FieldType { get { return FieldType.MasterDetailsRelationshipField; } }

        [JsonProperty(PropertyName = "relatedEntityId")]
        public Guid? RelatedEntityId { get; set; }
    }

    public class MasterDetailsRelationshipFieldMeta : MasterDetailsRelationshipField
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }
		
		public MasterDetailsRelationshipFieldMeta(Guid entityId, string entityName, MasterDetailsRelationshipField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			RelatedEntityId = field.RelatedEntityId;
		}
	}
}