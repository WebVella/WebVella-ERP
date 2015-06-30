using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;
using WebVella.ERP.Utilities;

namespace WebVella.ERP.Api.Models
{
	public enum RecordListType
	{
		General
	}

	public enum RecordListItemType
	{
		Field,
		FieldFromRelation
	}

	#region << Input classes >>

	public class InputRecordList
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

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "recordsLimit")]
		public int? RecordsLimit { get; set; }

		[JsonProperty(PropertyName = "pageSize")]
		public int? PageSize { get; set; }

		[JsonProperty(PropertyName = "columns")]
		public List<InputRecordListItemBase> Columns { get; set; }

		[JsonProperty(PropertyName = "query")]
		public InputRecordListQuery Query { get; set; }

		[JsonProperty(PropertyName = "sorts")]
		public List<InputRecordListSort> Sorts { get; set; }

		public static InputRecordList Convert(JObject inputList)
		{
			InputRecordList list = JsonConvert.DeserializeObject<InputRecordList>(inputList.ToString(), new RecordListItemConverter());

			return list;
		}
	}

	public class InputRecordListQuery
	{
		[JsonProperty(PropertyName = "queryType")]
		public QueryType QueryType { get; set; }

		[JsonProperty(PropertyName = "fieldName")]
		public string FieldName { get; set; }

		[JsonProperty(PropertyName = "fieldValue")]
		public string FieldValue { get; set; }

		[JsonProperty(PropertyName = "subQueries")]
		public List<InputRecordListQuery> SubQueries { get; set; }
	}

	public class InputRecordListSort
	{
		[JsonProperty(PropertyName = "fieldName")]
		public string FieldName { get; set; }

		[JsonProperty(PropertyName = "sortType")]
		public string SortType { get; set; }
	}

	public class InputRecordListItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }
	}

	public class InputRecordListFieldItem : InputRecordListItemBase
	{
		[JsonProperty(PropertyName = "fieldId")]
		public Guid? FieldId { get; set; }
	}

	public class InputRecordListRelationFieldItem : InputRecordListItemBase
	{
		[JsonProperty(PropertyName = "relationId")]
		public Guid? RelationId { get; set; }

		[JsonProperty(PropertyName = "entityId")]
		public Guid? EntityId { get; set; }

		[JsonProperty(PropertyName = "fieldId")]
		public Guid? FieldId { get; set; }
	}

	#endregion

	#region << Default classes >>

	public class RecordList
	{
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
		public string Type { get; set; }

		[JsonProperty(PropertyName = "recordsLimit")]
		public int RecordsLimit { get; set; }

		[JsonProperty(PropertyName = "pageSize")]
		public int PageSize { get; set; }

		[JsonProperty(PropertyName = "columns")]
		public List<RecordListItemBase> Columns { get; set; }

		[JsonProperty(PropertyName = "query")]
		public RecordListQuery Query { get; set; }

		[JsonProperty(PropertyName = "sorts")]
		public List<RecordListSort> Sorts { get; set; }
	}

	public class RecordListQuery
	{
		[JsonProperty(PropertyName = "queryType")]
		public string QueryType { get; set; }

		[JsonProperty(PropertyName = "fieldName")]
		public string FieldName { get; set; }

		[JsonProperty(PropertyName = "fieldValue")]
		public string FieldValue { get; set; }

		[JsonProperty(PropertyName = "subQueries")]
		public List<RecordListQuery> SubQueries { get; set; }
	}

	public class RecordListSort
	{
		[JsonProperty(PropertyName = "fieldName")]
		public string FieldName { get; set; }

		[JsonProperty(PropertyName = "sortType")]
		public string SortType { get; set; }
	}

	public abstract class RecordListItemBase
	{
	}

	public class RecordListFieldItem : RecordListItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return Enum.GetName(typeof(RecordListItemType), RecordListItemType.Field).ToLower(); } }

		[JsonProperty(PropertyName = "fieldId")]
		public Guid FieldId { get; set; }

		[JsonProperty(PropertyName = "fieldName")]
		public string FieldName { get; set; }

		[JsonProperty(PropertyName = "fieldLabel")]
		public string FieldLabel { get; set; }
	}

	public class RecordListRelationFieldItem : RecordListItemBase
	{
		[JsonProperty(PropertyName = "type")]
		public static string ItemType { get { return Enum.GetName(typeof(RecordListItemType), RecordListItemType.FieldFromRelation).ToLower(); } }

		[JsonProperty(PropertyName = "relationId")]
		public Guid RelationId { get; set; }

		[JsonProperty(PropertyName = "entityId")]
		public Guid EntityId { get; set; }

		[JsonProperty(PropertyName = "entityName")]
		public string EntityName { get; set; }

		[JsonProperty(PropertyName = "entityLabel")]
		public string EntityLabel { get; set; }

		[JsonProperty(PropertyName = "fieldId")]
		public Guid FieldId { get; set; }

		[JsonProperty(PropertyName = "fieldName")]
		public string FieldName { get; set; }

		[JsonProperty(PropertyName = "fieldLabel")]
		public string FieldLabel { get; set; }
	}

	#endregion

	public class RecordListCollection
	{
		[JsonProperty(PropertyName = "recordLists")]
		public List<RecordList> RecordLists { get; set; }
	}

	public class RecordListResponse : BaseResponseModel
	{
		[JsonProperty(PropertyName = "object")]
		public RecordList Object { get; set; }
	}

	public class RecordListCollectionResponse : BaseResponseModel
	{
		[JsonProperty(PropertyName = "object")]
		public RecordListCollection Object { get; set; }
	}

	public class RecordListItemConverter : JsonCreationConverter<InputRecordListItemBase>
	{
		protected override InputRecordListItemBase Create(Type objectType, JObject jObject)
		{
			string type = jObject["type"].ToString().ToLower();

			if (type == "fieldfromrelation")
				if (objectType == typeof(InputRecordListRelationFieldItem))
					return new InputRecordListRelationFieldItem();

			return new InputRecordListFieldItem();
		}
	}
}