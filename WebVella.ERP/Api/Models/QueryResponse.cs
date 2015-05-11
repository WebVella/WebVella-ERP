using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Api.Models
{
    public class QueryResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public QueryResult Object { get; set; }
	}

	public class SingleQueryResponse : BaseResponseModel
	{
		[JsonProperty(PropertyName = "object")]
		public SingleQueryResult Object { get; set; }
	}
}
