using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebVella.Erp.Web.Models
{
	class PageVisit
	{

		[JsonProperty("created_on")]
		public DateTime CreatedOn { get; set; } = DateTime.MinValue;

		[JsonProperty("session_id")]
		public Guid SessionId { get; set; } = Guid.Empty;

		[JsonProperty("user_id")]
		public Guid? UserId { get; set; } = null;

		[JsonProperty("page_id")]
		public Guid? PageId { get; set; } = null;

		[JsonProperty("app_id")]
		public Guid? AppId { get; set; } = null;

		[JsonProperty("area_id")]
		public Guid? AreaId { get; set; } = null;

		[JsonProperty("node_id")]
		public Guid? NodeId { get; set; } = null;

		[JsonProperty("query")]
		public string Query { get; set; } = null;

		[JsonProperty("id_string")]
		public string IdString { get; set; } = null; //the name of the custom or the name of the entity etc.

		[JsonProperty("relation_id")]
		public Guid? RelationId { get; set; } = null;

		[JsonProperty("record_id")]
		public Guid? RecordId { get; set; } = null;

		[JsonProperty("related_record_id")]
		public Guid? RelatedRecordId { get; set; } = null;
	}
}
