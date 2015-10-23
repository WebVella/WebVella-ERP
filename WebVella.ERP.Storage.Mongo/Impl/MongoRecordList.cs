using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;
using WebVella.ERP.Api;
using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage.Mongo
{
    [BsonIgnoreExtraElements]
    public class MongoRecordList : IStorageRecordList
	{
		[BsonElement("id")]
		public Guid Id { get; set; }

		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("label")]
		public string Label { get; set; }

		[BsonElement("default")]
		public bool? Default { get; set; }

		[BsonElement("system")]
		public bool? System { get; set; }

		[BsonElement("weight")]
		public decimal? Weight { get; set; }

		[BsonElement("cssClass")]
		public string CssClass { get; set; }

        [BsonElement("iconName")]
        public string IconName { get; set; }

        [BsonElement("viewNameOverride")]
        public string ViewNameOverride { get; set; }

        [BsonElement("visibleColumnsCount")]
        public int VisibleColumnsCount { get; set; }

        [BsonElement("type")]
		public RecordListType Type { get; set; }

        [BsonElement("pageSize")]
		public int PageSize { get; set; }

		[BsonElement("columns")]
		public List<IStorageRecordListItemBase> Columns { get; set; }

		[BsonElement("query")]
		public IStorageRecordListQuery Query { get; set; }

		[BsonElement("sorts")]
		public List<IStorageRecordListSort> Sorts { get; set; }

        [BsonElement("relationOptions")]
        public List<IStorageEntityRelationOptions> RelationOptions { get; set; }
    }

	public class MongoRecordListQuery : IStorageRecordListQuery
	{
		[BsonElement("queryType")]
		public QueryType QueryType { get; set; }

		[BsonElement("fieldName")]
		public string FieldName { get; set; }

		[BsonElement("fieldValue")]
		public string FieldValue { get; set; }

		[BsonElement("subQueries")]
		public List<IStorageRecordListQuery> SubQueries { get; set; }
	}

	public class MongoRecordListSort : IStorageRecordListSort
	{
		[BsonElement("fieldName")]
		public string FieldName { get; set; }

		[BsonElement("sortType")]
		public QuerySortType SortType { get; set; }
	}

	public abstract class MongoRecordListItemBase : IStorageRecordListItemBase
	{
		[BsonElement("entityId")]
		public Guid EntityId { get; set; }
	}

	public class MongoRecordListFieldItem : MongoRecordListItemBase, IStorageRecordListFieldItem
	{
		[BsonElement("fieldId")]
		public Guid FieldId { get; set; }
	}

	public class MongoRecordListRelationFieldItem : MongoRecordListItemBase, IStorageRecordListRelationFieldItem
	{
		[BsonElement("relationId")]
		public Guid RelationId { get; set; }

		[BsonElement("fieldId")]
		public Guid FieldId { get; set; }

		[BsonElement("fieldLabel")]
		public string FieldLabel { get; set; }

		[BsonElement("fieldPlaceholder")]
		public string FieldPlaceholder { get; set; }

		[BsonElement("fieldHelpText")]
		public string FieldHelpText { get; set; }

		[BsonElement("fieldRequired")]
		public bool FieldRequired { get; set; }

		[BsonElement("fieldLookupList")]
		public string FieldLookupList { get; set; }
	}

	public class MongoRecordListViewItem : MongoRecordListItemBase, IStorageRecordListViewItem
	{
		[BsonElement("viewId")]
		public Guid ViewId { get; set; }
	}

	public class MongoRecordListRelationViewItem : MongoRecordListItemBase, IStorageRecordListRelationViewItem
	{
		[BsonElement("relationId")]
		public Guid RelationId { get; set; }

		[BsonElement("viewId")]
		public Guid ViewId { get; set; }
        
		[BsonElement("fieldLabel")]
		public string FieldLabel { get; set; }

		[BsonElement("fieldPlaceholder")]
		public string FieldPlaceholder { get; set; }

		[BsonElement("fieldHelpText")]
		public string FieldHelpText { get; set; }

		[BsonElement("fieldRequired")]
		public bool FieldRequired { get; set; }

		[BsonElement("fieldLookupList")]
		public string FieldLookupList { get; set; }

		[BsonElement("fieldManageView")]
		public string FieldManageView { get; set; }
	}

	public class MongoRecordListListItem : MongoRecordListItemBase, IStorageRecordListListItem
	{
		[BsonElement("listId")]
		public Guid ListId { get; set; }
	}

	public class MongoRecordListRelationListItem : MongoRecordListItemBase, IStorageRecordListRelationListItem
	{
		[BsonElement("relationId")]
		public Guid RelationId { get; set; }

		[BsonElement("listId")]
		public Guid ListId { get; set; }

		[BsonElement("fieldLabel")]
		public string FieldLabel { get; set; }

		[BsonElement("fieldPlaceholder")]
		public string FieldPlaceholder { get; set; }

		[BsonElement("fieldHelpText")]
		public string FieldHelpText { get; set; }

		[BsonElement("fieldRequired")]
		public bool FieldRequired { get; set; }

		[BsonElement("fieldLookupList")]
		public string FieldLookupList { get; set; }

		[BsonElement("fieldManageView")]
		public string FieldManageView { get; set; }
	}

	public class MongoRecordListRelationTreeItem : MongoRecordListItemBase, IStorageRecordListRelationTreeItem
	{
		[BsonElement("relationId")]
		public Guid RelationId { get; set; }

		[BsonElement("treeId")]
		public Guid TreeId { get; set; }

		[BsonElement("fieldLabel")]
		public string FieldLabel { get; set; }

		[BsonElement("fieldPlaceholder")]
		public string FieldPlaceholder { get; set; }

		[BsonElement("fieldHelpText")]
		public string FieldHelpText { get; set; }

		[BsonElement("fieldRequired")]
		public bool FieldRequired { get; set; }
	}
}