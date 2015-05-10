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

    public class LookupRelationFieldMeta : LookupRelationField, IFieldMeta
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "parentFieldName")]
        public string ParentFieldName { get; set; }

        public LookupRelationFieldMeta(Guid entityId, string entityName, LookupRelationField field, string parentFieldName = null)
        {
            EntityId = entityId;
			EntityName = entityName;
			RelatedEntityId = field.RelatedEntityId;
            ParentFieldName = parentFieldName;
        }
	}
}