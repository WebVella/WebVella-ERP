using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Api.Models
{
    public class QueryResponse : BaseResponseModel
    {
		public QueryResult Object { get; set; }
	}
}
