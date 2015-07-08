using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoRecordView : IStorageRecordView
    {
        public MongoRecordView()
        {
            Id = Guid.NewGuid();
            Name = "";
            Label = "";
            Default = false;
            System = false;
            Weight = 1;
            CssClass = "";
            Type = RecordViewType.Details;
            Regions = new List<IStorageRecordViewRegion>();
            Sidebar = new MongoRecordViewSidebar();
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

        [BsonElement("type")]
        public RecordViewType Type { get; set; }

        [BsonElement("regions")]
        public List<IStorageRecordViewRegion> Regions { get; set; }

        [BsonElement("sidebar")]
        public IStorageRecordViewSidebar Sidebar { get; set; }

    }

    ////////////////////////
    public class MongoRecordViewSidebar : IStorageRecordViewSidebar
    {
        public MongoRecordViewSidebar()
        {
            Render = false;
            CssClass = "";
            Lists = new List<IStorageRecordViewSidebarList>();
        }

        [BsonElement("render")]
        public bool Render { get; set; }

        [BsonElement("cssClass")]
        public string CssClass { get; set; }

        [BsonElement("lists")]
        public List<IStorageRecordViewSidebarList> Lists { get; set; }
    }

    ////////////////////////
    public class MongoRecordViewSidebarList : IStorageRecordViewSidebarList
    {
        [BsonElement("entityId")]
        public Guid EntityId { get; set; }

        [BsonElement("listId")]
        public Guid ListId { get; set; }

        [BsonElement("relationId")]
        public Guid RelationId { get; set; }
    }

    ////////////////////////
    public class MongoRecordViewRegion : IStorageRecordViewRegion
    {
        public MongoRecordViewRegion()
        {
            Name = "";
            Render = true;
            CssClass = "";
        }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("render")]
        public bool Render { get; set; }

        [BsonElement("cssClass")]
        public string CssClass { get; set; }

        [BsonElement("sections")]
        public List<IStorageRecordViewSection> Sections { get; set; }
    }

    ////////////////////////
    public class MongoRecordViewSection : IStorageRecordViewSection
    {

        public MongoRecordViewSection()
        {
			Id = Guid.NewGuid();
			Name = "";
            Label = "";
            CssClass = "";
            ShowLabel = true;
            Collapsed = false;
            Weight = 1;
            TabOrder = "left-right";
            Rows = new List<IStorageRecordViewRow>();
        }

		[BsonElement("id")]
		public Guid Id { get; set; }

		[BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("label")]
        public string Label { get; set; }

        [BsonElement("cssClass")]
        public string CssClass { get; set; }

        [BsonElement("showLabel")]
        public bool ShowLabel { get; set; }

        [BsonElement("collapsed")]
        public bool Collapsed { get; set; }

        [BsonElement("weight")]
        public decimal? Weight { get; set; }

        [BsonElement("tabOrder")]
        public string TabOrder { get; set; }

        [BsonElement("rows")]
        public List<IStorageRecordViewRow> Rows { get; set; }

    }

    ////////////////////////
    public class MongoRecordViewRow : IStorageRecordViewRow
    {
        public MongoRecordViewRow()
        {
			Id = Guid.NewGuid();
			Weight = 1;
            Columns = new List<IStorageRecordViewColumn>();
        }

		[BsonElement("id")]
		public Guid Id { get; set; }

		[BsonElement("weight")]
        public decimal? Weight { get; set; }

        [BsonElement("columns")]
        public List<IStorageRecordViewColumn> Columns { get; set; }
    }

    ////////////////////////
    public class MongoRecordViewColumn : IStorageRecordViewColumn
    {
        public MongoRecordViewColumn()
        {
            Items = new List<IStorageRecordViewItemBase>();
			GridColCount = 0;
        }

        [BsonElement("items")]
        public List<IStorageRecordViewItemBase> Items { get; set; }

		[BsonElement("gridColCount")]
		public int GridColCount { get; set; }
	}



    ////////////////////////
    public abstract class MongoRecordViewItemBase : IStorageRecordViewItemBase
    {
    }

    public class MongoRecordViewFieldItem : MongoRecordViewItemBase, IStorageRecordViewFieldItem
    {
        [BsonElement("fieldId")]
        public Guid FieldId { get; set; }
    }

    public class MongoRecordViewListItem : MongoRecordViewItemBase, IStorageRecordViewListItem
    {
        [BsonElement("listId")]
        public Guid ListId { get; set; }
    }

    public class MongoRecordViewViewItem : MongoRecordViewItemBase, IStorageRecordViewViewItem
    {
        [BsonElement("viewId")]
        public Guid ViewId { get; set; }
    }

    public class MongoRecordViewHtmlItem : MongoRecordViewItemBase, IStorageRecordViewHtmlItem
    {
        [BsonElement("tag")]
        public string Tag { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }
    }

    public class MongoRecordViewRelationFieldItem : MongoRecordViewItemBase, IStorageRecordViewRelationFieldItem
    {
        [BsonElement("fieldId")]
        public Guid FieldId { get; set; }

        [BsonElement("relationId")]
        public Guid RelationId { get; set; }
    }

    public class MongoRecordViewRelationViewItem : MongoRecordViewItemBase, IStorageRecordViewRelationViewItem
    {
        [BsonElement("viewId")]
        public Guid ViewId { get; set; }

        [BsonElement("relationId")]
        public Guid RelationId { get; set; }
    }

    public class MongoRecordViewRelationListItem : MongoRecordViewItemBase, IStorageRecordViewRelationListItem
    {
        [BsonElement("listId")]
        public Guid ListId { get; set; }

        [BsonElement("relationId")]
        public Guid RelationId { get; set; }
    }
}