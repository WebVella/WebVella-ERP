using MongoDB.Bson.Serialization.Attributes;
using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoTreeSelectField : MongoBaseField, IStorageTreeSelectField
    {
		[BsonElement("relatedEntityId")]
		public Guid RelatedEntityId { get; set; }

		[BsonElement("relationId")]
		public Guid RelationId { get; set; }

		[BsonElement("selectedTreeId")]
		public Guid SelectedTreeId { get; set; }

		[BsonElement("selectionType")]
		public string SelectionType { get; set; }

		[BsonElement("selectionTarget")]
		public string SelectionTarget { get; set; }
	}
}
