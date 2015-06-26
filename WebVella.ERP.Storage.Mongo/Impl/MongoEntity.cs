using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Storage.Mongo
{
    internal class MongoEntity : MongoDocumentBase, IStorageEntity
    {
		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("label")]
		public string Label { get; set; }

		[BsonElement("labelPlural")]
		public string LabelPlural { get; set; }

		[BsonElement("system")]
		public bool System { get; set; }

		[BsonElement("iconName")]
		public string IconName { get; set; }

		[BsonElement("weight")]
		public decimal Weight { get; set; }

		[BsonElement("recordPermissions")]
		public IStorageRecordPermissions RecordPermissions { get; set; }

		[BsonElement("fields")]
		public List<IStorageField> Fields { get; set; }

		[BsonElement("recordLists")]
		public List<IStorageRecordList> RecordLists { get; set; }

		[BsonElement("recordViews")]
		public List<IStorageRecordView> RecordViews { get; set; }

        public MongoEntity()
        {
            Fields = new List<IStorageField>();
            RecordLists = new List<IStorageRecordList>();
            RecordViews = new List<IStorageRecordView>();
            RecordPermissions = new MongoRecordPermissions();
        }
    }

    public class MongoRecordPermissions : IStorageRecordPermissions
    {
		[BsonElement("canRead")]
		public List<Guid> CanRead { get; set; }

		[BsonElement("canCreate")]
		public List<Guid> CanCreate { get; set; }

		[BsonElement("canUpdate")]
		public List<Guid> CanUpdate { get; set; }

		[BsonElement("canDelete")]
		public List<Guid> CanDelete { get; set; }

        public MongoRecordPermissions()
        {
            CanRead = new List<Guid>();
            CanCreate = new List<Guid>();
            CanUpdate = new List<Guid>();
            CanDelete = new List<Guid>();
        }
    }
}