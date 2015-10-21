using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WebVella.ERP.Storage.Mongo
{
	public class MongoRecordTree : IStorageRecordTree
	{
		public MongoRecordTree()
		{
			Id = Guid.NewGuid();
			Name = "";
			Label = "";
			Default = false;
			System = false;
			Weight = 1;
			CssClass = "";
			IconName = "";
			RootNodes = new List<Guid>();
			NodeProperties = new List<Guid>();
        }

		[BsonElement("id")]
		public Guid Id { get; set; }

		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("label")]
		public string Label { get; set; }

		[BsonElement("default")]
		public bool Default { get; set; }

		[BsonElement("system")]
		public bool System { get; set; }

		[BsonElement("weight")]
		public decimal? Weight { get; set; }

		[BsonElement("cssClass")]
		public string CssClass { get; set; }

		[BsonElement("iconName")]
		public string IconName { get; set; }

		[BsonElement("relationId")]
		public Guid RelationId { get; set; }

		[BsonElement("depthLimit")]
		public int DepthLimit { get; set; }

		[BsonElement("nodeParentIdFieldId")]
		public Guid NodeParentIdFieldId { get; set; }

		[BsonElement("nodeIdFieldId")]
		public Guid NodeIdFieldId { get; set; }

		[BsonElement("nodeNameFieldId")]
		public Guid NodeNameFieldId { get; set; }

		[BsonElement("nodeLabelFieldId")]
		public Guid NodeLabelFieldId { get; set; }

		[BsonElement("rootNodes")]
		public List<Guid> RootNodes { get; set; }

		[BsonElement("nodeProperties")]
		public List<Guid> NodeProperties { get; set; }
	}
}