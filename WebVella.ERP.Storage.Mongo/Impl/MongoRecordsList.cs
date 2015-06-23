using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;
using WebVella.ERP.Api;
using MongoDB.Bson.Serialization.Attributes;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoRecordsList : IStorageRecordsList
    {
		[BsonElement("id")]
		public Guid Id { get; set; }

		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("label")]
		public string Label { get; set; }

		[BsonElement("type")]
		public RecordsListTypes Type { get; set; }

		[BsonElement("filters")]
		public IList<IStorageRecordsListFilter> Filters { get; set; }

		[BsonElement("fields")]
		public IList<IStorageRecordsListField> Fields { get; set; }

        public MongoRecordsList()
        {
            Filters = new List<IStorageRecordsListFilter>();
            Fields = new List<IStorageRecordsListField>();
        }
    }

    public class MongoRecordsListFilter : IStorageRecordsListFilter
    {
		[BsonElement("entityId")]
		public Guid EntityId { get; set; }

		[BsonElement("fieldId")]
		public Guid FieldId { get; set; }

		[BsonElement("operator")]
		public FilterOperatorTypes Operator { get; set; }

		[BsonElement("value")]
		public string Value { get; set; }
    }

    public class MongoRecordsListField : IStorageRecordsListField
    {
		[BsonElement("id")]
		public Guid Id { get; set; }

		[BsonElement("entityId")]
		public Guid EntityId { get; set; }

		[BsonElement("position")]
		public int Position { get; set; }
    }
}