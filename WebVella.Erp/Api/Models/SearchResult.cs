using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Api.Models
{
	public class SearchResult
	{
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "entities")]
		public List<Guid> Entities { get; set; } = new List<Guid>();

		[JsonProperty(PropertyName = "apps")]
		public List<Guid> Apps { get; set; } = new List<Guid>();

		[JsonProperty(PropertyName = "records")]
		public List<Guid> Records { get; set; } = new List<Guid>();

		[JsonProperty(PropertyName = "content")]
		public string Content { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "stem_content")]
		public string StemContent { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "snippet")]
		public string Snippet { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "url")]
		public string Url { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "aux_data")]
		public string AuxData { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "timestamp")]
		public DateTime Timestamp { get; set; }
	}
}
