using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Api.Models
{
	public class SearchQuery
	{
		[JsonProperty(PropertyName = "search_type")]
		public SearchType SearchType { get; set; } = SearchType.Contains;

		[JsonProperty(PropertyName = "result_type")]
		public SearchResultType ResultType { get; set; } = SearchResultType.Compact;

		[JsonProperty(PropertyName = "text")]
		public string Text { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "entities")]
		public List<Guid> Entities { get; set; } = new List<Guid>();

		[JsonProperty(PropertyName = "apps")]
		public List<Guid> Apps { get; set; } = new List<Guid>();

		[JsonProperty(PropertyName = "records")]
		public List<Guid> Records { get; set; } = new List<Guid>();

		[JsonProperty(PropertyName = "skip")]
		public int? Skip { get; set; } = 0;

		[JsonProperty(PropertyName = "limit")]
		public int? Limit { get; set; } = 20;

	}
}
