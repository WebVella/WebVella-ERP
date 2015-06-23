using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
    public enum RecordViewType
    {
        Details
    }

    public enum RecordViewItemType
    {
        Html,
        Field,
		FieldFromRelation,
        List,
        View
    }

    public class RecordView
    {
        public RecordView()
        {
            Id = Guid.NewGuid();
            Name = "";
            Label = "";
            Default = false;
            System = false;
            Weight = 1;
            CssClass = "";
            Type = RecordViewType.Details;
            Regions = new List<RecordViewRegion>();
            Sidebar = new RecordViewSidebar();
        }

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "default")]
        public bool? Default { get; set; }

        [JsonProperty(PropertyName = "system")]
        public bool? System { get; set; }

        [JsonProperty(PropertyName = "weight")]
        public decimal? Weight { get; set; }

        [JsonProperty(PropertyName = "cssClass")]
        public string CssClass { get; set; }

        [JsonProperty(PropertyName = "type")]
        public RecordViewType Type { get; set; }

        [JsonProperty(PropertyName = "regions")]
        public List<RecordViewRegion> Regions { get; set; }

        [JsonProperty(PropertyName = "sidebar")]
        public RecordViewSidebar Sidebar { get; set; }

    }

    ////////////////////////
    public class RecordViewSidebar
    {
        public RecordViewSidebar()
        {
            Render = false;
            CssClass = "";
            Lists = new List<RecordViewSidebarList>();
        }

        [JsonProperty(PropertyName = "render")]
        public bool Render { get; set; }

        [JsonProperty(PropertyName = "cssClass")]
        public string CssClass { get; set; }

        [JsonProperty(PropertyName = "lists")]
        public List<RecordViewSidebarList> Lists { get; set; }
    }

    ////////////////////////
    public class RecordViewSidebarList
    {
        public RecordViewSidebarList()
        {
        }

        [JsonProperty(PropertyName = "entityId")]
        public Guid EntityId { get; set; }

        [JsonProperty(PropertyName = "listId")]
        public Guid ListId { get; set; }

        [JsonProperty(PropertyName = "relationId")]
        public Guid RelationId { get; set; }
    }

    ////////////////////////
    public class RecordViewRegion
    {
        public RecordViewRegion()
        {
            Name = "";
            Render = true;
            CssClass = "";
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "render")]
        public bool Render { get; set; }

        [JsonProperty(PropertyName = "cssClass")]
        public string CssClass { get; set; }

        [JsonProperty(PropertyName = "sections")]
        public List<RecordViewSection> Sections { get; set; }
    }

    ////////////////////////
    public class RecordViewSection
    {

        public RecordViewSection()
        {
			Id = Guid.NewGuid();
			Name = "";
            Label = "";
            CssClass = "";
            ShowLabel = true;
            Collapsed = false;
            Weight = 1;
            TabOrder = "left-right";
            Rows = new List<RecordViewRow>();
        }

		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "cssClass")]
        public string CssClass { get; set; }

        [JsonProperty(PropertyName = "showLabel")]
        public bool ShowLabel { get; set; }

        [JsonProperty(PropertyName = "collapsed")]
        public bool Collapsed { get; set; }

        [JsonProperty(PropertyName = "weight")]
        public decimal? Weight { get; set; }

        [JsonProperty(PropertyName = "tabOrder")]
        public string TabOrder { get; set; }

        [JsonProperty(PropertyName = "rows")]
        public List<RecordViewRow> Rows { get; set; }

    }

    ////////////////////////
    public class RecordViewRow
    {
        public RecordViewRow()
        {
			Id = Guid.NewGuid();
			Weight = 1;
            Columns = new List<RecordViewColumn>();
        }

		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "weight")]
        public decimal? Weight { get; set; }

        [JsonProperty(PropertyName = "columns")]
        public List<RecordViewColumn> Columns { get; set; }

    }

    ////////////////////////
    public class RecordViewColumn
    {
        public RecordViewColumn()
        {
            Items = new List<RecordViewItemBase>();
			GridColCount = 0;
        }

        [JsonProperty(PropertyName = "items")]
        public List<RecordViewItemBase> Items { get; set; }

		[JsonProperty(PropertyName = "gridColCount")]
		public int GridColCount { get; set; }
	}



    ////////////////////////
    public abstract class RecordViewItemBase
    {
    }

    public class RecordViewFieldItem : RecordViewItemBase
    {
        [JsonProperty(PropertyName = "type")]
        public static RecordViewItemType ItemType { get { return RecordViewItemType.Field; } }

        [JsonProperty(PropertyName = "fieldId")]
        public Guid FieldId { get; set; }
    }

    public class RecordViewListItem : RecordViewItemBase
    {
        [JsonProperty(PropertyName = "type")]
        public static RecordViewItemType ItemType { get { return RecordViewItemType.List; } }

        [JsonProperty(PropertyName = "listId")]
        public Guid ListId { get; set; }
    }

    public class RecordViewViewItem : RecordViewItemBase
    {
        [JsonProperty(PropertyName = "type")]
        public static RecordViewItemType ItemType { get { return RecordViewItemType.View; } }

        [JsonProperty(PropertyName = "viewId")]
        public Guid ViewId { get; set; }
    }

    public class RecordViewRelationFieldItem : RecordViewItemBase
    {
        [JsonProperty(PropertyName = "type")]
        public static RecordViewItemType ItemType { get { return RecordViewItemType.FieldFromRelation; } }

        [JsonProperty(PropertyName = "relationId")]
        public Guid RelationId { get; set; }

        [JsonProperty(PropertyName = "fieldId")]
        public Guid FieldId { get; set; }
    }

    public class RecordViewHtmlItem : RecordViewItemBase
    {
        [JsonProperty(PropertyName = "type")]
        public static RecordViewItemType ItemType { get { return RecordViewItemType.Html; } }

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }


    public class RecordViewCollection
    {
        [JsonProperty(PropertyName = "recordViews")]
        public List<RecordView> RecordViews { get; set; }
    }

    public class RecordViewResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public RecordView Object { get; set; }
    }

    public class RecordViewCollectionResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public RecordViewCollection Object { get; set; }
    }
}