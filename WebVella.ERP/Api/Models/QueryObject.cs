using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
	public class QueryObject
	{
		public QueryType QueryType { get; set; }
		public string FieldName { get; set; }
		public object FieldValue { get; set; }
		public List<QueryObject> SubQueries { get; set; }
	}
}