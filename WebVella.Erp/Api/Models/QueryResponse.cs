using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Api.Models
{
    public class QueryResponse : BaseResponseModel
    {
		public QueryResponse() {
			Object = new QueryResult();
		}

		[JsonProperty(PropertyName = "object")]
        public QueryResult Object { get; set; }
	}
}
