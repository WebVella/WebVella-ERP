using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using WebVella.ERP.Storage;
using WebVella.ERP.Utilities;

namespace WebVella.ERP.Api.Models
{
	public enum RecordViewType
	{
		General = 0,
		Quick_View = 1,
		Create = 2,
		Quick_Create = 3
	}

	public enum RecordViewItemType
	{
		Html,
		Field,
		FieldFromRelation,
		List,
		View
	}

	#region << Input Classes >>

	public class InputRecordView
	{
		[JsonProperty(PropertyName = "id")]
		public Guid? Id { get; set; }

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

        [JsonProperty(PropertyName = "iconName")]
        public string IconName { get; set; }

        [JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "regions")]
		public List<InputRecordViewRegion> Regions { get; set; }

        [JsonProperty(PropertyName = "relationOptions")]
        public List<EntityRelationOptionsItem> RelationOptions { get; set; }

        [JsonProperty(PropertyName = "sidebar")]
		public InputRecordViewSidebar Sidebar { get; set; }

		public static InputRecordView Convert(JObject inputField)
		{
			JsonConverter itemConverter = new RecordViewItemConverter();
			JsonConverter sidebarItemConverter = new RecordViewSidebarItemConverter();
            InputRecordView view = JsonConvert.DeserializeObject<InputRecordView>(inputField.ToString(), new [] { itemConverter, sidebarItemConverter });

			return view;
		}
	}

    ////////////////////////
    public class InputRecordViewSidebar
	{
		[JsonProperty(PropertyName = "render")]
		public bool? Render { get; set; }

		[JsonProperty(PropertyName = "cssClass")]
		public string CssClass { get; set; }

		[JsonProperty(PropertyName = "items")]
		public List<InputRecordViewSidebarItemBase> Items { get; set; }
	}

	////////////////////////
	public class InputRecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "entityId")]
		public Guid? EntityId { get; set; }

		[JsonProperty(PropertyName = "entityName")]
		public string EntityName { get; set; }
	}

	public class InputRecordViewSidebarListItem : InputRecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "listId")]
		public Guid? ListId { get; set; }

		[JsonProperty(PropertyName = "listName")]
		public string ListName { get; set; }
	}

	public class InputRecordViewSidebarViewItem : InputRecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "viewId")]
		public Guid? ViewId { get; set; }

		[JsonProperty(PropertyName = "viewName")]
		public string ViewName { get; set; }
	}

	public class InputRecordViewSidebarRelationViewItem : InputRecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "relationId")]
		public Guid? RelationId { get; set; }

		[JsonProperty(PropertyName = "relationName")]
		public string RelationName { get; set; }

		[JsonProperty(PropertyName = "viewId")]
		public Guid? ViewId { get; set; }

		[JsonProperty(PropertyName = "viewName")]
		public string ViewName { get; set; }

        [JsonProperty(PropertyName = "fieldLabel")]
        public string FieldLabel { get; set; }

        [JsonProperty(PropertyName = "fieldPlaceholder")]
        public string FieldPlaceholder { get; set; }

        [JsonProperty(PropertyName = "fieldHelpText")]
        public string FieldHelpText { get; set; }

        [JsonProperty(PropertyName = "fieldRequired")]
        public bool FieldRequired { get; set; }

        [JsonProperty(PropertyName = "fieldLookupList")]
        public string FieldLookupList { get; set; }

		[JsonProperty(PropertyName = "fieldManageView")]
		public string FieldManageView { get; set; }
	}

	public class InputRecordViewSidebarRelationListItem : InputRecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "relationId")]
		public Guid? RelationId { get; set; }

		[JsonProperty(PropertyName = "relationName")]
		public string RelationName { get; set; }

		[JsonProperty(PropertyName = "listId")]
		public Guid? ListId { get; set; }

		[JsonProperty(PropertyName = "listName")]
		public string ListName { get; set; }

        [JsonProperty(PropertyName = "fieldLabel")]
        public string FieldLabel { get; set; }

        [JsonProperty(PropertyName = "fieldPlaceholder")]
        public string FieldPlaceholder { get; set; }

        [JsonProperty(PropertyName = "fieldHelpText")]
        public string FieldHelpText { get; set; }

        [JsonProperty(PropertyName = "fieldRequired")]
        public bool FieldRequired { get; set; }

        [JsonProperty(PropertyName = "fieldLookupList")]
        public string FieldLookupList { get; set; }

		[JsonProperty(PropertyName = "fieldManageView")]
		public string FieldManageView { get; set; }
	}

	////////////////////////
	public class InputRecordViewRegion
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "render")]
		public bool? Render { get; set; }

		[JsonProperty(PropertyName = "cssClass")]
		public string CssClass { get; set; }

		[JsonProperty(PropertyName = "sections")]
		public List<InputRecordViewSection> Sections { get; set; }
	}

	////////////////////////
	public class InputRecordViewSection
	{
		[JsonProperty(PropertyName = "id")]
		public Guid? Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[JsonProperty(PropertyName = "cssClass")]
		public string CssClass { get; set; }

		[JsonProperty(PropertyName = "showLabel")]
		public bool? ShowLabel { get; set; }

		[JsonProperty(PropertyName = "collapsed")]
		public bool? Collapsed { get; set; }

		[JsonProperty(PropertyName = "weight")]
		public decimal? Weight { get; set; }

		[JsonProperty(PropertyName = "tabOrder")]
		public string TabOrder { get; set; }

		[JsonProperty(PropertyName = "rows")]
		public List<InputRecordViewRow> Rows { get; set; }

	}

	////////////////////////
	public class InputRecordViewRow
	{
		[JsonProperty(PropertyName = "id")]
		public Guid? Id { get; set; }

		[JsonProperty(PropertyName = "weight")]
		public decimal? Weight { get; set; }

		[JsonProperty(PropertyName = "columns")]
		public List<InputRecordViewColumn> Columns { get; set; }
	}

	////////////////////////
	public class InputRecordViewColumn
	{
		//[JsonConverter(typeof(RecordViewItemConverter))]
		[JsonProperty(PropertyName = "items")]
		public List<InputRecordViewItemBase> Items { get; set; }

		[JsonProperty(PropertyName = "gridColCount")]
		public int? GridColCount { get; set; }
	}



	////////////////////////
	public class InputRecordViewItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "entityId")]
		public Guid? EntityId { get; set; }

		[JsonProperty(PropertyName = "entityName")]
		public string EntityName { get; set; }
	}

	public class InputRecordViewFieldItem : InputRecordViewItemBase
	{
		[JsonProperty(PropertyName = "fieldId")]
		public Guid? FieldId { get; set; }

		[JsonProperty(PropertyName = "fieldName")]
		public string FieldName { get; set; }
	}

	public class InputRecordViewListItem : InputRecordViewItemBase
	{
		[JsonProperty(PropertyName = "listId")]
		public Guid? ListId { get; set; }

		[JsonProperty(PropertyName = "listName")]
		public string ListName { get; set; }
	}

	public class InputRecordViewViewItem : InputRecordViewItemBase
	{
		[JsonProperty(PropertyName = "viewId")]
		public Guid? ViewId { get; set; }

		[JsonProperty(PropertyName = "viewName")]
		public string ViewName { get; set; }
	}

	public class InputRecordViewRelationFieldItem : InputRecordViewItemBase
	{
		[JsonProperty(PropertyName = "relationId")]
		public Guid? RelationId { get; set; }

		[JsonProperty(PropertyName = "relationName")]
		public string RelationName { get; set; }

		[JsonProperty(PropertyName = "fieldId")]
		public Guid? FieldId { get; set; }

		[JsonProperty(PropertyName = "fieldName")]
		public string FieldName { get; set; }

        [JsonProperty(PropertyName = "fieldLabel")]
        public string FieldLabel { get; set; }

        [JsonProperty(PropertyName = "fieldPlaceholder")]
        public string FieldPlaceholder { get; set; }

        [JsonProperty(PropertyName = "fieldHelpText")]
        public string FieldHelpText { get; set; }

        [JsonProperty(PropertyName = "fieldRequired")]
        public bool FieldRequired { get; set; }

		[JsonProperty(PropertyName = "fieldLookupList")]
		public string FieldLookupList { get; set; }
	}

	public class InputRecordViewRelationViewItem : InputRecordViewItemBase
	{
		[JsonProperty(PropertyName = "relationId")]
		public Guid? RelationId { get; set; }

		[JsonProperty(PropertyName = "relationName")]
		public string RelationName { get; set; }

		[JsonProperty(PropertyName = "viewId")]
		public Guid? ViewId { get; set; }

		[JsonProperty(PropertyName = "viewName")]
		public string ViewName { get; set; }

        [JsonProperty(PropertyName = "fieldLabel")]
        public string FieldLabel { get; set; }

        [JsonProperty(PropertyName = "fieldPlaceholder")]
        public string FieldPlaceholder { get; set; }

        [JsonProperty(PropertyName = "fieldHelpText")]
        public string FieldHelpText { get; set; }

        [JsonProperty(PropertyName = "fieldRequired")]
        public bool FieldRequired { get; set; }

        [JsonProperty(PropertyName = "fieldLookupList")]
        public string FieldLookupList { get; set; }

		[JsonProperty(PropertyName = "fieldManageView")]
		public string FieldManageView { get; set; }
	}

	public class InputRecordViewRelationListItem : InputRecordViewItemBase
	{
		[JsonProperty(PropertyName = "relationId")]
		public Guid? RelationId { get; set; }

		[JsonProperty(PropertyName = "relationName")]
		public string RelationName { get; set; }

		[JsonProperty(PropertyName = "listId")]
		public Guid? ListId { get; set; }

		[JsonProperty(PropertyName = "listName")]
		public string ListName { get; set; }

        [JsonProperty(PropertyName = "fieldLabel")]
        public string FieldLabel { get; set; }

        [JsonProperty(PropertyName = "fieldPlaceholder")]
        public string FieldPlaceholder { get; set; }

        [JsonProperty(PropertyName = "fieldHelpText")]
        public string FieldHelpText { get; set; }

        [JsonProperty(PropertyName = "fieldRequired")]
        public bool FieldRequired { get; set; }

        [JsonProperty(PropertyName = "fieldLookupList")]
        public string FieldLookupList { get; set; }

		[JsonProperty(PropertyName = "fieldManageView")]
		public string FieldManageView { get; set; }
	}

	public class InputRecordViewHtmlItem : InputRecordViewItemBase
	{
		[JsonProperty(PropertyName = "tag")]
		public string Tag { get; set; }

		[JsonProperty(PropertyName = "content")]
		public string Content { get; set; }
	}

	#endregion

	#region << Default Classes >>

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
            IconName = "";
			Type = Enum.GetName(typeof(RecordViewType), RecordViewType.General).ToLower();
			Regions = new List<RecordViewRegion>();
			Sidebar = new RecordViewSidebar();
            RelationOptions = new List<IStorageEntityRelationOptions>();
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

        [JsonProperty(PropertyName = "iconName")]
        public string IconName { get; set; }

        [JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "regions")]
		public List<RecordViewRegion> Regions { get; set; }

        [JsonProperty(PropertyName = "relationOptions")]
        public List<IStorageEntityRelationOptions> RelationOptions { get; set; }

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
			Items = new List<RecordViewSidebarItemBase>();
		}

		[JsonProperty(PropertyName = "render")]
		public bool Render { get; set; }

		[JsonProperty(PropertyName = "cssClass")]
		public string CssClass { get; set; }

		[JsonProperty(PropertyName = "items")]
		public List<RecordViewSidebarItemBase> Items { get; set; }
	}

	////////////////////////
	public class RecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "dataName")]
		public string DataName { get; set; }

		[JsonProperty(PropertyName = "entityId")]
		public Guid EntityId { get; set; }

		[JsonProperty(PropertyName = "entityLabel")]
		public string EntityLabel { get; set; }

		[JsonProperty(PropertyName = "entityName")]
		public string EntityName { get; set; }

		[JsonProperty(PropertyName = "entityLabelPlural")]
		public string EntityLabelPlural { get; set; }
	}

	public class RecordViewSidebarListItem : RecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return Enum.GetName(typeof(RecordViewItemType), RecordViewItemType.List).ToLower(); } }

		[JsonProperty(PropertyName = "listId")]
		public Guid ListId { get; set; }

		[JsonProperty(PropertyName = "listName")]
		public string ListName { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public RecordList Meta { get; set; }
	}

	public class RecordViewSidebarViewItem : RecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return Enum.GetName(typeof(RecordViewItemType), RecordViewItemType.View).ToLower(); } }

		[JsonProperty(PropertyName = "viewId")]
		public Guid ViewId { get; set; }

		[JsonProperty(PropertyName = "viewName")]
		public string ViewName { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public RecordView Meta { get; set; }
	}

	public class RecordViewSidebarRelationViewItem : RecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return "viewFromRelation"; } }

		[JsonProperty(PropertyName = "relationId")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "relationName")]
		public string RelationName { get; set; }

        [JsonProperty(PropertyName = "relationDirection")]
        public string RelationDirection { get; set; }

        [JsonProperty(PropertyName = "viewId")]
		public Guid ViewId { get; set; }

		[JsonProperty(PropertyName = "viewName")]
		public string ViewName { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public RecordView Meta { get; set; }

        [JsonProperty(PropertyName = "fieldLabel")]
        public string FieldLabel { get; set; }

        [JsonProperty(PropertyName = "fieldPlaceholder")]
        public string FieldPlaceholder { get; set; }

        [JsonProperty(PropertyName = "fieldHelpText")]
        public string FieldHelpText { get; set; }

        [JsonProperty(PropertyName = "fieldRequired")]
        public bool FieldRequired { get; set; }

        [JsonProperty(PropertyName = "fieldLookupList")]
        public string FieldLookupList { get; set; }

		[JsonProperty(PropertyName = "fieldManageView")]
		public string FieldManageView { get; set; }
	}

	public class RecordViewSidebarRelationListItem : RecordViewSidebarItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return "listFromRelation"; } }

		[JsonProperty(PropertyName = "relationId")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "relationName")]
		public string RelationName { get; set; }

        [JsonProperty(PropertyName = "relationDirection")]
        public string RelationDirection { get; set; }

        [JsonProperty(PropertyName = "listId")]
		public Guid ListId { get; set; }

		[JsonProperty(PropertyName = "listName")]
		public string ListName { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public RecordList Meta { get; set; }

        [JsonProperty(PropertyName = "fieldLabel")]
        public string FieldLabel { get; set; }

        [JsonProperty(PropertyName = "fieldPlaceholder")]
        public string FieldPlaceholder { get; set; }

        [JsonProperty(PropertyName = "fieldHelpText")]
        public string FieldHelpText { get; set; }

        [JsonProperty(PropertyName = "fieldRequired")]
        public bool FieldRequired { get; set; }

        [JsonProperty(PropertyName = "fieldLookupList")]
        public string FieldLookupList { get; set; }

		[JsonProperty(PropertyName = "fieldManageView")]
		public string FieldManageView { get; set; }
	}

	////////////////////////
	public class RecordViewRegion
	{
		public RecordViewRegion()
		{
			Name = "";
			Render = true;
			CssClass = "";
			Sections = new List<RecordViewSection>();
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
		[JsonProperty(PropertyName = "dataName")]
		public string DataName { get; set; }

		[JsonProperty(PropertyName = "entityId")]
		public Guid EntityId { get; set; }

		[JsonProperty(PropertyName = "entityLabel")]
		public string EntityLabel { get; set; }

		[JsonProperty(PropertyName = "entityName")]
		public string EntityName { get; set; }

		[JsonProperty(PropertyName = "entityLabelPlural")]
		public string EntityLabelPlural { get; set; }
	}

	public class RecordViewFieldItem : RecordViewItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return Enum.GetName(typeof(RecordViewItemType), RecordViewItemType.Field).ToLower(); } }

		[JsonProperty(PropertyName = "fieldId")]
		public Guid FieldId { get; set; }

		[JsonProperty(PropertyName = "fieldName")]
		public string FieldName { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public Field Meta { get; set; }
	}

	public class RecordViewListItem : RecordViewItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return Enum.GetName(typeof(RecordViewItemType), RecordViewItemType.List).ToLower(); } }

		[JsonProperty(PropertyName = "listId")]
		public Guid ListId { get; set; }

		[JsonProperty(PropertyName = "listName")]
		public string ListName { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public RecordList Meta { get; set; }
	}

	public class RecordViewViewItem : RecordViewItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return Enum.GetName(typeof(RecordViewItemType), RecordViewItemType.View).ToLower(); } }

		[JsonProperty(PropertyName = "viewId")]
		public Guid ViewId { get; set; }

		[JsonProperty(PropertyName = "viewName")]
		public string ViewName { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public RecordView Meta { get; set; }
	}

	public class RecordViewRelationFieldItem : RecordViewItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return "fieldFromRelation"; /*Enum.GetName(typeof(RecordViewItemType), RecordViewItemType.FieldFromRelation).ToLower();*/ } }

		[JsonProperty(PropertyName = "relationId")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "relationName")]
		public string RelationName { get; set; }

        [JsonProperty(PropertyName = "relationDirection")]
        public string RelationDirection { get; set; }

        [JsonProperty(PropertyName = "fieldId")]
		public Guid FieldId { get; set; }

		[JsonProperty(PropertyName = "fieldName")]
		public string FieldName { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public Field Meta { get; set; }

        [JsonProperty(PropertyName = "fieldLabel")]
        public string FieldLabel { get; set; }

        [JsonProperty(PropertyName = "fieldPlaceholder")]
        public string FieldPlaceholder { get; set; }

        [JsonProperty(PropertyName = "fieldHelpText")]
        public string FieldHelpText { get; set; }

        [JsonProperty(PropertyName = "fieldRequired")]
        public bool FieldRequired { get; set; }

        [JsonProperty(PropertyName = "fieldLookupList")]
        public string FieldLookupList { get; set; }
    }

	public class RecordViewRelationViewItem : RecordViewItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return "viewFromRelation"; } }

		[JsonProperty(PropertyName = "relationId")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "relationName")]
		public string RelationName { get; set; }

        [JsonProperty(PropertyName = "relationDirection")]
        public string RelationDirection { get; set; }

        [JsonProperty(PropertyName = "viewId")]
		public Guid ViewId { get; set; }

		[JsonProperty(PropertyName = "viewName")]
		public string ViewName { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public RecordView Meta { get; set; }

        [JsonProperty(PropertyName = "fieldLabel")]
        public string FieldLabel { get; set; }

        [JsonProperty(PropertyName = "fieldPlaceholder")]
        public string FieldPlaceholder { get; set; }

        [JsonProperty(PropertyName = "fieldHelpText")]
        public string FieldHelpText { get; set; }

        [JsonProperty(PropertyName = "fieldRequired")]
        public bool FieldRequired { get; set; }

        [JsonProperty(PropertyName = "fieldLookupList")]
        public string FieldLookupList { get; set; }

        [JsonProperty(PropertyName = "fieldManageView")]
        public string FieldManageView { get; set; }
    }

	public class RecordViewRelationListItem : RecordViewItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return "listFromRelation"; } }

		[JsonProperty(PropertyName = "relationId")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "relationName")]
		public string RelationName { get; set; }

        [JsonProperty(PropertyName = "relationDirection")]
        public string RelationDirection { get; set; }

        [JsonProperty(PropertyName = "listId")]
		public Guid ListId { get; set; }

		[JsonProperty(PropertyName = "listName")]
		public string ListName { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public RecordList Meta { get; set; }

        [JsonProperty(PropertyName = "fieldLabel")]
        public string FieldLabel { get; set; }

        [JsonProperty(PropertyName = "fieldPlaceholder")]
        public string FieldPlaceholder { get; set; }

        [JsonProperty(PropertyName = "fieldHelpText")]
        public string FieldHelpText { get; set; }

        [JsonProperty(PropertyName = "fieldRequired")]
        public bool FieldRequired { get; set; }

        [JsonProperty(PropertyName = "fieldLookupList")]
        public string FieldLookupList { get; set; }

        [JsonProperty(PropertyName = "fieldManageView")]
        public string FieldManageView { get; set; }
    }

	public class RecordViewHtmlItem : RecordViewItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return Enum.GetName(typeof(RecordViewItemType), RecordViewItemType.Html).ToLower(); } }

		[JsonProperty(PropertyName = "tag")]
		public string Tag { get; set; }

		[JsonProperty(PropertyName = "content")]
		public string Content { get; set; }
	}

	#endregion

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

	public class RecordViewRecordResponse : BaseResponseModel
	{
		[JsonProperty(PropertyName = "object")]
		public RecordViewRecord Object { get; set; }
	}

	public class RecordViewRecord
	{
		[JsonProperty(PropertyName = "data")]
		public object Data { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public RecordView Meta { get; set; }
	}

	public class RecordViewItemConverter : JsonCreationConverter<InputRecordViewItemBase>
	{
		protected override InputRecordViewItemBase Create(Type objectType, JObject jObject)
		{
			string type = jObject["type"].ToString().ToLower();

			if (type == "list")
				return new InputRecordViewListItem();
			if (type == "view")
				return new InputRecordViewViewItem();
			if (type == "fieldfromrelation")
				return new InputRecordViewRelationFieldItem();
			if (type == "viewfromrelation")
				return new InputRecordViewRelationViewItem();
			if (type == "listfromrelation")
				return new InputRecordViewRelationListItem();
			if (type == "html")
				return new InputRecordViewHtmlItem();

			return new InputRecordViewFieldItem();
		}
	}

	public class RecordViewSidebarItemConverter : JsonCreationConverter<InputRecordViewSidebarItemBase>
	{
		protected override InputRecordViewSidebarItemBase Create(Type objectType, JObject jObject)
		{
			string type = jObject["type"].ToString().ToLower();

			if (type == "view")
				return new InputRecordViewSidebarViewItem();
			if (type == "viewfromrelation")
				return new InputRecordViewSidebarRelationViewItem();
			if (type == "listfromrelation")
				return new InputRecordViewSidebarRelationListItem();

			return new InputRecordViewSidebarListItem();
		}
	}


}