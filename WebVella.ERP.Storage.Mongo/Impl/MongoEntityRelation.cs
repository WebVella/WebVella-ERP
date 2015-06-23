using MongoDB.Bson.Serialization.Attributes;
using System;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage.Mongo
{
    internal class MongoEntityRelation : MongoDocumentBase, IStorageEntityRelation
    {
		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("label")]
		public string Label { get; set; }

		[BsonElement("description")]
		public string Description { get; set; }

		[BsonElement("system")]
		public bool System { get; set; }

		[BsonElement("relationType")]
		public EntityRelationType RelationType { get; set; }

		[BsonElement("originEntityId")]
		public Guid OriginEntityId { get; set; }

		[BsonElement("originFieldId")]
		public Guid OriginFieldId { get; set; }

		[BsonElement("targetEntityId")]
		public Guid TargetEntityId { get; set; }

		[BsonElement("targetFieldId")]
		public Guid TargetFieldId { get; set; }
    }
}