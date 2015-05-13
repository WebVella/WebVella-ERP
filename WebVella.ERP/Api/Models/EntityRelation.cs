using Newtonsoft.Json;
using System;


namespace WebVella.ERP.Api.Models
{
    public enum EntityRelationType
    {
        OneToOne = 1,
        OneToMany = 2,
        ManyToMany = 3
    }

    public class EntityRelation
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
}
