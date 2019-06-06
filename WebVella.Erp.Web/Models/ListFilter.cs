using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class ListFilter
	{
		[JsonProperty(PropertyName = "queryType")]
		public QueryType QueryType { get; set; }

		[JsonProperty(PropertyName = "query_key")]
		public string QueryKey { get; set; }

		[JsonProperty(PropertyName = "is_general_search")]
		public bool IsGeneralSearch { get; set; } = false;
	}


}
