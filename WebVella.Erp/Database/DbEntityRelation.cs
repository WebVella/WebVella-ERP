using Newtonsoft.Json;
using System;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Database
{
    public class DbEntityRelation : DbDocumentBase
    {
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "system")]
		public bool System { get; set; }

		[JsonProperty(PropertyName = "relation_type")]
		public EntityRelationType RelationType { get; set; }

		[JsonProperty(PropertyName = "origin_entity_id")]
		public Guid OriginEntityId { get; set; }

		[JsonProperty(PropertyName = "origin_field_id")]
		public Guid OriginFieldId { get; set; }

		[JsonProperty(PropertyName = "target_entity_id")]
		public Guid TargetEntityId { get; set; }

		[JsonProperty(PropertyName = "target_field_id")]
		public Guid TargetFieldId { get; set; }
    }
}