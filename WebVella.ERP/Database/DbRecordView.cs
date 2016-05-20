using System;
using System.Collections.Generic;
using WebVella.ERP.Api.Models;
using Newtonsoft.Json;

namespace WebVella.ERP.Database
{
    public class DbRecordView
    {
        public DbRecordView()
        {
            Id = Guid.NewGuid();
            Name = "";
            Label = "";
			Title = "";
            Default = false;
            System = false;
            Weight = 1;
            CssClass = "";
            IconName = "";
            Type = RecordViewType.General;
            Regions = new List<DbRecordViewRegion>();
            Sidebar = new DbRecordViewSidebar();
			DynamicHtmlTemplate = "";
			DataSourceUrl = "";

		}

		[JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

		[JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

		[JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

		[JsonProperty(PropertyName = "default")]
        public bool Default { get; set; }

		[JsonProperty(PropertyName = "system")]
        public bool System { get; set; }

		[JsonProperty(PropertyName = "weight")]
        public decimal? Weight { get; set; }

		[JsonProperty(PropertyName = "css_class")]
        public string CssClass { get; set; }

		[JsonProperty(PropertyName = "icon_name")]
        public string IconName { get; set; }

		[JsonProperty(PropertyName = "type")]
        public RecordViewType Type { get; set; }

		[JsonProperty(PropertyName = "regions")]
        public List<DbRecordViewRegion> Regions { get; set; }

		[JsonProperty(PropertyName = "sidebar")]
        public DbRecordViewSidebar Sidebar { get; set; }

		[JsonProperty(PropertyName = "relation_options")]
        public List<DbEntityRelationOptions> RelationOptions { get; set; }

		[JsonProperty(PropertyName = "dynamic_html_template")]
		public string DynamicHtmlTemplate { get; set; }

		[JsonProperty(PropertyName = "data_source_url")]
		public string DataSourceUrl { get; set; }

		[JsonProperty(PropertyName = "action_items")]
		public List<ActionItem> ActionItems { get; set; }

		[JsonProperty(PropertyName = "service_code")]
		public string ServiceCode { get; set; }

	}

    ////////////////////////
    public class DbRecordViewSidebar
    {
        public DbRecordViewSidebar()
        {
            Render = false;
            CssClass = "";
            Items = new List<DbRecordViewSidebarItemBase>();
        }

		[JsonProperty(PropertyName = "render")]
        public bool Render { get; set; }

		[JsonProperty(PropertyName = "css_class")]
        public string CssClass { get; set; }

		[JsonProperty(PropertyName = "items")]
        public List<DbRecordViewSidebarItemBase> Items { get; set; }
    }

	////////////////////////
	public abstract class DbRecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "entity_id")]
		public Guid EntityId { get; set; }
	}

	public class DbRecordViewSidebarListItem : DbRecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "list_id")]
		public Guid ListId { get; set; }
	}

	public class DbRecordViewSidebarViewItem : DbRecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "view_id")]
		public Guid ViewId { get; set; }
	}

	public class DbRecordViewSidebarRelationViewItem : DbRecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "view_id")]
		public Guid ViewId { get; set; }

		[JsonProperty(PropertyName = "relation_id")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "field_label")]
		public string FieldLabel { get; set; }

		[JsonProperty(PropertyName = "field_placeholder")]
		public string FieldPlaceholder { get; set; }

		[JsonProperty(PropertyName = "field_help_text")]
		public string FieldHelpText { get; set; }

		[JsonProperty(PropertyName = "field_required")]
		public bool FieldRequired { get; set; }

		[JsonProperty(PropertyName = "field_lookup_list")]
		public string FieldLookupList { get; set; }

		[JsonProperty(PropertyName = "field_manage_view")]
		public string FieldManageView { get; set; }
	}

	public class DbRecordViewSidebarRelationListItem : DbRecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "list_id")]
		public Guid ListId { get; set; }

		[JsonProperty(PropertyName = "relation_id")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "field_label")]
		public string FieldLabel { get; set; }

		[JsonProperty(PropertyName = "field_placeholder")]
		public string FieldPlaceholder { get; set; }

		[JsonProperty(PropertyName = "field_help_text")]
		public string FieldHelpText { get; set; }

		[JsonProperty(PropertyName = "field_required")]
		public bool FieldRequired { get; set; }

		[JsonProperty(PropertyName = "field_lookup_list")]
		public string FieldLookupList { get; set; }

		[JsonProperty(PropertyName = "field_manage_view")]
		public string FieldManageView { get; set; }
	}

	public class DbRecordViewSidebarRelationTreeItem : DbRecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "tree_id")]
		public Guid TreeId { get; set; }

		[JsonProperty(PropertyName = "relation_id")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "field_label")]
		public string FieldLabel { get; set; }

		[JsonProperty(PropertyName = "field_placeholder")]
		public string FieldPlaceholder { get; set; }

		[JsonProperty(PropertyName = "field_help_text")]
		public string FieldHelpText { get; set; }

		[JsonProperty(PropertyName = "field_required")]
		public bool FieldRequired { get; set; }
	}

	////////////////////////
	public class DbRecordViewRegion
    {
        public DbRecordViewRegion()
        {
            Name = "";
			Label = "";
            Render = true;
            CssClass = "";
			Weight = 10;
        }

		[JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

		[JsonProperty(PropertyName = "render")]
        public bool Render { get; set; }

		[JsonProperty(PropertyName = "css_class")]
        public string CssClass { get; set; }

		[JsonProperty(PropertyName = "weight")]
		public decimal? Weight { get; set; }

		[JsonProperty(PropertyName = "sections")]
        public List<DbRecordViewSection> Sections { get; set; }
    }

    ////////////////////////
    public class DbRecordViewSection
    {

        public DbRecordViewSection()
        {
			Id = Guid.NewGuid();
			Name = "";
            Label = "";
            CssClass = "";
            ShowLabel = true;
            Collapsed = false;
            Weight = 1;
            TabOrder = "left-right";
            Rows = new List<DbRecordViewRow>();
        }

		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

		[JsonProperty(PropertyName = "css_class")]
        public string CssClass { get; set; }

		[JsonProperty(PropertyName = "show_label")]
        public bool ShowLabel { get; set; }

		[JsonProperty(PropertyName = "collapsed")]
        public bool Collapsed { get; set; }

		[JsonProperty(PropertyName = "weight")]
        public decimal? Weight { get; set; }

		[JsonProperty(PropertyName = "tab_order")]
        public string TabOrder { get; set; }

		[JsonProperty(PropertyName = "rows")]
        public List<DbRecordViewRow> Rows { get; set; }

    }

    ////////////////////////
    public class DbRecordViewRow
    {
        public DbRecordViewRow()
        {
			Id = Guid.NewGuid();
			Weight = 1;
            Columns = new List<DbRecordViewColumn>();
        }

		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "weight")]
        public decimal? Weight { get; set; }

		[JsonProperty(PropertyName = "columns")]
        public List<DbRecordViewColumn> Columns { get; set; }
    }

    ////////////////////////
    public class DbRecordViewColumn
    {
        public DbRecordViewColumn()
        {
            Items = new List<DbRecordViewItemBase>();
			GridColCount = 0;
        }

		[JsonProperty(PropertyName = "items")]
        public List<DbRecordViewItemBase> Items { get; set; }

		[JsonProperty(PropertyName = "grid_col_count")]
		public int GridColCount { get; set; }
	}



    ////////////////////////
    public abstract class DbRecordViewItemBase
    {
		[JsonProperty(PropertyName = "entity_id")]
		public Guid EntityId { get; set; }
	}

    public class DbRecordViewFieldItem : DbRecordViewItemBase
    {
		[JsonProperty(PropertyName = "field_id")]
        public Guid FieldId { get; set; }
    }

    public class DbRecordViewListItem : DbRecordViewItemBase
    {
		[JsonProperty(PropertyName = "list_id")]
        public Guid ListId { get; set; }
    }

    public class DbRecordViewViewItem : DbRecordViewItemBase
    {
		[JsonProperty(PropertyName = "view_id")]
        public Guid ViewId { get; set; }
    }

    public class DbRecordViewHtmlItem : DbRecordViewItemBase
    {
		[JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

		[JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }

    public class DbRecordViewRelationFieldItem : DbRecordViewItemBase
    {
		[JsonProperty(PropertyName = "field_id")]
        public Guid FieldId { get; set; }

		[JsonProperty(PropertyName = "relation_id")]
        public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "field_label")]
		public string FieldLabel { get; set; }

		[JsonProperty(PropertyName = "field_placeholder")]
		public string FieldPlaceholder { get; set; }

		[JsonProperty(PropertyName = "field_help_text")]
		public string FieldHelpText { get; set; }

		[JsonProperty(PropertyName = "field_required")]
		public bool FieldRequired { get; set; }

		[JsonProperty(PropertyName = "field_lookup_list")]
		public string FieldLookupList { get; set; }
	}

    public class DbRecordViewRelationViewItem : DbRecordViewItemBase
    {
		[JsonProperty(PropertyName = "view_id")]
        public Guid ViewId { get; set; }

		[JsonProperty(PropertyName = "relation_id")]
        public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "field_label")]
		public string FieldLabel { get; set; }

		[JsonProperty(PropertyName = "field_placeholder")]
		public string FieldPlaceholder { get; set; }

		[JsonProperty(PropertyName = "field_help_text")]
		public string FieldHelpText { get; set; }

		[JsonProperty(PropertyName = "field_required")]
		public bool FieldRequired { get; set; }

		[JsonProperty(PropertyName = "field_lookup_list")]
		public string FieldLookupList { get; set; }

		[JsonProperty(PropertyName = "field_manage_view")]
		public string FieldManageView { get; set; }
	}

    public class DbRecordViewRelationListItem : DbRecordViewItemBase
    {
		[JsonProperty(PropertyName = "list_id")]
        public Guid ListId { get; set; }

		[JsonProperty(PropertyName = "relation_id")]
        public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "field_label")]
		public string FieldLabel { get; set; }

		[JsonProperty(PropertyName = "field_placeholder")]
		public string FieldPlaceholder { get; set; }

		[JsonProperty(PropertyName = "field_help_text")]
		public string FieldHelpText { get; set; }

		[JsonProperty(PropertyName = "field_required")]
		public bool FieldRequired { get; set; }

		[JsonProperty(PropertyName = "field_lookup_list")]
		public string FieldLookupList { get; set; }

		[JsonProperty(PropertyName = "field_manage_view")]
		public string FieldManageView { get; set; }
	}

	public class DbRecordViewRelationTreeItem : DbRecordViewItemBase
	{
		[JsonProperty(PropertyName = "tree_id")]
		public Guid TreeId { get; set; }

		[JsonProperty(PropertyName = "relation_id")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "field_label")]
		public string FieldLabel { get; set; }

		[JsonProperty(PropertyName = "field_placeholder")]
		public string FieldPlaceholder { get; set; }

		[JsonProperty(PropertyName = "field_help_text")]
		public string FieldHelpText { get; set; }

		[JsonProperty(PropertyName = "field_required")]
		public bool FieldRequired { get; set; }
	}
}