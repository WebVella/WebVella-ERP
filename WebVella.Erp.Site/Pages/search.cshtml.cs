using Microsoft.AspNetCore.Mvc;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Site.Pages
{
	public class SearchModel : BaseErpPageModel
	{
		[BindProperty]
		public string SearchText { get; set; }

		[BindProperty]
		public SearchResultList SearchResults { get; set; } = new SearchResultList();

		public void OnGet()
		{
			//SearchManager sm = new SearchManager();
			//for (int i = 0; i < 1000; i++)
			//{
			//	sm.AddToIndex($"http://test{i}", $"snippet{i}", $"content_a_{i} content_b_{i}");
			//}
		}

		public void OnPost()
		{
			SearchManager sm = new SearchManager();

			SearchQuery query = new SearchQuery();
			query.SearchType = SearchType.Fts;
			query.Text = SearchText;
			SearchResults = sm.Search(query);

		}
	}
}   