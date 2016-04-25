using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
	public class InputEntity
	{
		[JsonProperty(PropertyName = "id")]
		public Guid? Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[JsonProperty(PropertyName = "labelPlural")]
		public string LabelPlural { get; set; }

		[JsonProperty(PropertyName = "system")]
		public bool? System { get; set; }

		[JsonProperty(PropertyName = "iconName")]
		public string IconName { get; set; }

		[JsonProperty(PropertyName = "weight")]
		public decimal? Weight { get; set; }

		[JsonProperty(PropertyName = "recordPermissions")]
		public RecordPermissions RecordPermissions { get; set; }
	}

	[Serializable]
	public class Entity
	{
		public Entity()
		{
			RecordViews = new List<RecordView>();
			RecordLists = new List<RecordList>();
		}

		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[JsonProperty(PropertyName = "labelPlural")]
		public string LabelPlural { get; set; }

		[JsonProperty(PropertyName = "system")]
		public bool System { get; set; }

		[JsonProperty(PropertyName = "iconName")]
		public string IconName { get; set; }

		[JsonProperty(PropertyName = "weight")]
		public decimal Weight { get; set; }

		[JsonProperty(PropertyName = "recordPermissions")]
		public RecordPermissions RecordPermissions { get; set; }

		[JsonProperty(PropertyName = "fields")]
		public List<Field> Fields { get; set; }

		[JsonProperty(PropertyName = "recordLists")]
		public List<RecordList> RecordLists { get; set; }

		[JsonProperty(PropertyName = "recordViews")]
		public List<RecordView> RecordViews { get; set; }

		[JsonProperty(PropertyName = "recordTrees")]
		public List<RecordTree> RecordTrees { get; set; }

		[JsonProperty(PropertyName = "hash")]
		public string Hash { get; internal set; }
	}

	[Serializable]
	public class RecordPermissions
	{
		[JsonProperty(PropertyName = "canRead")]
		public List<Guid> CanRead { get; set; }

		[JsonProperty(PropertyName = "canCreate")]
		public List<Guid> CanCreate { get; set; }

		[JsonProperty(PropertyName = "canUpdate")]
		public List<Guid> CanUpdate { get; set; }

		[JsonProperty(PropertyName = "canDelete")]
		public List<Guid> CanDelete { get; set; }

		public RecordPermissions()
		{
			CanRead = new List<Guid>();
			CanCreate = new List<Guid>();
			CanUpdate = new List<Guid>();
			CanDelete = new List<Guid>();
		}
	}

	public class EntityList
	{
		[JsonProperty(PropertyName = "entities")]
		public List<Entity> Entities { get; set; }

		public EntityList()
		{
			Entities = new List<Entity>();
		}
	}

	public class EntityResponse : BaseResponseModel
	{
		[JsonProperty(PropertyName = "object")]
		public Entity Object { get; set; }
	}

	public class EntityListResponse : BaseResponseModel
	{
		[JsonProperty(PropertyName = "object")]
		public EntityList Object { get; set; }
	}

	public class EntityLibraryItemsResponse : BaseResponseModel
	{
		[JsonProperty(PropertyName = "object")]
		public List<object> Object { get; set; }
	}
}