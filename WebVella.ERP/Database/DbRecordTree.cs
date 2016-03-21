using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebVella.ERP.Database
{
	public class DbRecordTree
	{
		public DbRecordTree()
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
			NodeObjectProperties = new List<Guid>();
        }

		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

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

		[JsonProperty(PropertyName = "relation_id")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "depth_limit")]
		public int DepthLimit { get; set; }

		[JsonProperty(PropertyName = "node_parent_id_field_id")]
		public Guid NodeParentIdFieldId { get; set; }

		[JsonProperty(PropertyName = "node_id_field_id")]
		public Guid NodeIdFieldId { get; set; }

		[JsonProperty(PropertyName = "node_name_field_id")]
		public Guid NodeNameFieldId { get; set; }

		[JsonProperty(PropertyName = "node_label_field_id")]
		public Guid NodeLabelFieldId { get; set; }

		[JsonProperty(PropertyName = "node_weight_field_id")]
		public Guid? NodeWeightFieldId { get; set; }

		[JsonProperty(PropertyName = "root_nodes")]
		public List<Guid> RootNodes { get; set; }

		[JsonProperty(PropertyName = "node_object_properties")]
		public List<Guid> NodeObjectProperties { get; set; }
	}
}