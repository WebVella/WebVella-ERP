using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models
{
    public enum EntityRelationType
    {
        /// <summary>
        /// 1. Origin field should be an unique, required Guid field
        /// 2. Target field should be an unique, required Guid field
        /// 3. Target field record values should match one origin record field values
        /// </summary>
        OneToOne = 1,

        /// <summary>
        /// 1. Origin field should be an unique,required Guid field
        /// 2. Target field should be a Guid field 
        /// 3. Target field record values should match atleast one origin record field values or null if field value is not required
        /// </summary>
        OneToMany = 2,

        /// <summary>
        /// 1. Origin field should be an unique, required Guid field
        /// 2. Target field should be an unique, required Guid field
        /// </summary>
        ManyToMany = 3
    }

    public class EntityRelation : IStorageEntityRelation
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "system")]
        public bool System { get; set; }

        [JsonProperty(PropertyName = "relationType")]
        public EntityRelationType RelationType { get; set; }

        [JsonProperty(PropertyName = "originEntityId")]
        public Guid OriginEntityId { get; set; }

        [JsonProperty(PropertyName = "originFieldId")]
        public Guid OriginFieldId { get; set; }

        [JsonProperty(PropertyName = "targetEntityId")]
        public Guid TargetEntityId { get; set; }

        [JsonProperty(PropertyName = "targetFieldId")]
        public Guid TargetFieldId { get; set; }
    }

    public class EntityRelationOptionsItem
    {
        [JsonProperty(PropertyName = "type")]
        public static string ItemType { get { return "relationOptions"; } }

        [JsonProperty(PropertyName = "relationId")]
        public Guid? RelationId { get; set; }

        [JsonProperty(PropertyName = "relationName")]
        public string RelationName { get; set; }

        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }
    }


    public class EntityRelationResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public EntityRelation Object { get; set; }
    }

    public class EntityRelationListResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public List<EntityRelation> Object { get; set; }
    }

    public class InputEntityRelationRecordUpdateModel
    {
        [JsonProperty(PropertyName = "relationName")]
        public string RelationName { get; set; }

        [JsonProperty(PropertyName = "originFieldRecordId")]
        public Guid OriginFieldRecordId { get; set; }

        [JsonProperty(PropertyName = "attachTargetFieldRecordIds")]
        public List<Guid> AttachTargetFieldRecordIds { get; set; }

        [JsonProperty(PropertyName = "detachTargetFieldRecordIds")]
        public List<Guid> DetachTargetFieldRecordIds { get; set; }
    }
}
