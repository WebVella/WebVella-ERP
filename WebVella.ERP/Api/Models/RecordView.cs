using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
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
			Type = "";
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
		public Boolean Default { get; set; }

		[JsonProperty(PropertyName = "system")]
		public Boolean System { get; set; }

		[JsonProperty(PropertyName = "weight")]
		public decimal? Weight { get; set; }

		[JsonProperty(PropertyName = "cssClass")]
		public string CssClass { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

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
		public Boolean Render { get; set; }

		[JsonProperty(PropertyName = "cssClass")]
		public string CssClass { get; set; }

		[JsonProperty(PropertyName = "lists")]
		public List<RecordViewSidebarList> Lists { get; set; }
	}

	////////////////////////
	public class RecordViewSidebarList
	{
		public RecordViewSidebarList() {
			EntityId = Guid.NewGuid();
			ListId = Guid.NewGuid();
			RelationId = Guid.NewGuid();
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
		public Boolean Render { get; set; }

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
			Name = "";
			Label = "";
			CssClass = "";
			ShowLabel = true;
			Collapsed = false;
			Weight = 1;
			TabOrder = "left-right";
			Rows = new List<RecordViewRow>();
		}

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[JsonProperty(PropertyName = "cssClass")]
		public string CssClass { get; set; }

		[JsonProperty(PropertyName = "showLabel")]
		public Boolean ShowLabel { get; set; }

		[JsonProperty(PropertyName = "collapsed")]
		public Boolean Collapsed { get; set; }

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
			Weight = 1;
			Columns = new List<RecordViewColumn>();
		}

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
			Items = new List<RecordViewItem>();
		}

		[JsonProperty(PropertyName = "items")]
		public List<RecordViewItem> Items { get; set; }
	}

	////////////////////////
	public class RecordViewItem
	{
		public RecordViewItem()
		{
			Id = Guid.Empty;
			Type = "";
		}
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }
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