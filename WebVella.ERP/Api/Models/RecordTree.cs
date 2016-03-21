using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
	public class InputRecordTree
	{
		public InputRecordTree() {
			NodeWeightFieldId = null;
		}


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

		[JsonProperty(PropertyName = "nodeWeightFieldId")]
		public Guid? NodeWeightFieldId { get; set; }

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
			NodeWeightFieldId = null;
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

		[JsonProperty(PropertyName = "nodeWeightFieldId")]
		public Guid? NodeWeightFieldId { get; set; }

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

		[JsonProperty(PropertyName = "weight")]
		public int? Weight { get; set; }

		[JsonProperty(PropertyName = "parentId")]
		public Guid? ParentId { get; set; }

		[JsonProperty(PropertyName = "recordId")]
		public Guid RecordId { get; set; }
	}

	public class RecordTreeRecordResponse : BaseResponseModel
	{
		[JsonProperty(PropertyName = "object")]
		public RecordTreeRecord Object { get; set; }
	}

	public class RecordTreeRecord
	{
		[JsonProperty(PropertyName = "data")]
		public List<ResponseTreeNode> Data { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public RecordTree Meta { get; set; }
	}

	public class ResponseTreeNode
	{
		[JsonProperty(PropertyName = "recordId")]
		public Guid RecordId { get; set; }

		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[JsonProperty(PropertyName = "parentId")]
		public Guid? ParentId { get; set; }

		[JsonProperty(PropertyName = "weight")]
		public int? Weight { get; set; }

		[JsonProperty(PropertyName = "nodes")]
		public List<ResponseTreeNode> Nodes{ get; set; }
	}

	public class RelationTreeItem
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return "treeFromRelation"; } }

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

		[JsonProperty(PropertyName = "relationId")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "relationName")]
		public string RelationName { get; set; }

		[JsonProperty(PropertyName = "meta")]
		public RecordTree Meta { get; set; }

		[JsonProperty(PropertyName = "treeId")]
		public Guid TreeId { get; set; }

		[JsonProperty(PropertyName = "treeName")]
		public string TreeName { get; set; }

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