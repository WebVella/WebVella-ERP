using System.Collections.Generic;

namespace WebVella.Erp.Api.Models
{
	public class SearchResultList : List<SearchResult>
	{
		public int TotalCount { get; set; } = 0;
	}
}
