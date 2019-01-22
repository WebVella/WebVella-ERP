//using System;
//using Newtonsoft.Json;
//using System.Linq;

//namespace WebVella.Erp.Api.Models
//{
//	public class InputTreeSelectField : InputField
//	{
//		[JsonProperty(PropertyName = "fieldType")]
//		public static FieldType FieldType { get { return FieldType.TreeSelectField; } }

//		[JsonProperty(PropertyName = "relatedEntityId")]
//		public Guid RelatedEntityId { get; set; }

//		[JsonProperty(PropertyName = "relationId")]
//		public Guid RelationId { get; set; }

//		[JsonProperty(PropertyName = "selectedTreeId")]
//		public Guid SelectedTreeId { get; set; }

//		[JsonProperty(PropertyName = "selectionType")]
//		public string SelectionType { get; set; }

//		[JsonProperty(PropertyName = "selectionTarget")]
//		public string SelectionTarget { get; set; }

//	}

//	[Serializable]
//	public class TreeSelectField : Field
//    {
//        [JsonProperty(PropertyName = "fieldType")]
//        public static FieldType FieldType { get { return FieldType.TreeSelectField; } }

//		[JsonProperty(PropertyName = "relatedEntityId")]
//		public Guid RelatedEntityId { get; set; }

//		[JsonProperty(PropertyName = "relationId")]
//		public Guid RelationId { get; set; }

//		[JsonProperty(PropertyName = "selectedTreeId")]
//		public Guid SelectedTreeId { get; set; }

//		[JsonProperty(PropertyName = "selectionType")]
//		public string SelectionType { get; set; }

//		[JsonProperty(PropertyName = "selectionTarget")]
//		public string SelectionTarget { get; set; }

//	}
//}
 