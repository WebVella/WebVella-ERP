using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;
using WebVella.ERP.Api;
using MongoDB.Bson.Serialization.Attributes;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoRecordList : IStorageRecordList
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
		public IList<IStorageRecordListFilter> Filters { get; set; }

		[BsonElement("fields")]
		public IList<IStorageRecordListField> Fields { get; set; }

        public MongoRecordList()
        {
            Filters = new List<IStorageRecordListFilter>();
            Fields = new List<IStorageRecordListField>();
        }
    }

    public class MongoRecordListFilter : IStorageRecordListFilter
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

    public class MongoRecordListField : IStorageRecordListField
    {
		[BsonElement("id")]
		public Guid Id { get; set; }

		[BsonElement("entityId")]
		public Guid EntityId { get; set; }

		[BsonElement("position")]
		public int Position { get; set; }
    }
}