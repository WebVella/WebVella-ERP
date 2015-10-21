using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
	public class InputRecordTree
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

		[JsonProperty(PropertyName = "cssClass")]
		public string CssClass { get; set; }

		[JsonProperty(PropertyName = "iconName")]
		public string IconName { get; set; }

		[JsonProperty(PropertyName = "relationId")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "depthLimit")]
		public int? DepthLimit { get; set; }

		[JsonProperty(PropertyName = "nodeParentIdFieldId")]
		public Guid? NodeParentIdFieldId { get; set; }

		[JsonProperty(PropertyName = "nodeIdFieldId")]
		public Guid? NodeIdFieldId { get; set; }

		[JsonProperty(PropertyName = "nodeNameFieldId")]
		public Guid? NodeNameFieldId { get; set; }

		[JsonProperty(PropertyName = "nodeLabelFieldId")]
		public Guid? NodeLabelFieldId { get; set; }

		[JsonProperty(PropertyName = "rootNodes")]
		public List<RecordTreeNode> RootNodes { get; set; }

		[JsonProperty(PropertyName = "nodeObjectProperties")]
		public List<Guid> NodeObjectProperties { get; set; }

		public static InputRecordTree Convert(JObject inputField)
		{
			return JsonConvert.DeserializeObject<InputRecordTree>(inputField.ToString());
		}
	}

	public class RecordTree
	{
		public RecordTree()
		{
			Id = Guid.NewGuid();
			Name = "";
			Label = "";
			Default = false;
			System = false;
			CssClass = "";
			IconName = "";
			RootNodes = new List<RecordTreeNode>();
			NodeObjectProperties = new List<Guid>();
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

		[JsonProperty(PropertyName = "cssClass")]
		public string CssClass { get; set; }

		[JsonProperty(PropertyName = "iconName")]
		public string IconName { get; set; }

		[JsonProperty(PropertyName = "relationId")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "depthLimit")]
		public int DepthLimit { get; set; }

		[JsonProperty(PropertyName = "nodeParentIdFieldId")]
		public Guid NodeParentIdFieldId { get; set; }

		[JsonProperty(PropertyName = "nodeIdFieldId")]
		public Guid NodeIdFieldId { get; set; }

		[JsonProperty(PropertyName = "nodeNameFieldId")]
		public Guid NodeNameFieldId { get; set; }

		[JsonProperty(PropertyName = "nodeLabelFieldId")]
		public Guid NodeLabelFieldId { get; set; }

		[JsonProperty(PropertyName = "rootNodes")]
		public List<RecordTreeNode> RootNodes { get; set; }

		[JsonProperty(PropertyName = "nodeObjectProperties")]
		public List<Guid> NodeObjectProperties { get; set; }

	}

	public class RecordTreeNode
	{
		[JsonProperty(PropertyName = "id")]
		public Guid? Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[JsonProperty(PropertyName = "parentId")]
		public Guid? ParentId { get; set; }

		[JsonProperty(PropertyName = "recordId")]
		public Guid RecordId { get; set; }


	}

	public class RecordTreeCollection
	{
		[JsonProperty(PropertyName = "recordTrees")]
		public List<RecordTree> RecordTrees { get; set; }
	}

	public class RecordTreeResponse : BaseResponseModel
	{
		[JsonProperty(PropertyName = "object")]
		public RecordTree Object { get; set; }
	}

	public class RecordTreeCollectionResponse : BaseResponseModel
	{
		[JsonProperty(PropertyName = "object")]
		public RecordTreeCollection Object { get; set; }
	}
}