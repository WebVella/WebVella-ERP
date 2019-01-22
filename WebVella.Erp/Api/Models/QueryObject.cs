using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Api.Models
{
	[Serializable]
	public class QueryObject
	{
        [JsonProperty(PropertyName = "queryType")]
        public QueryType QueryType { get; set; }

        [JsonProperty(PropertyName = "fieldName")]
        public string FieldName { get; set; }

        [JsonProperty(PropertyName = "fieldValue")]
        public object FieldValue { get; set; }

		[JsonProperty(PropertyName = "regexOperator")]
		public QueryObjectRegexOperator RegexOperator { get; set; }

		[JsonProperty(PropertyName = "ftsLanguage")]
		public string FtsLanguage { get; set; }

		[JsonProperty(PropertyName = "subQueries")]
        public List<QueryObject> SubQueries { get; set; }
	}

	public enum QueryObjectRegexOperator 
	{
		MatchCaseSensitive,
		MatchCaseInsensitive,
		DontMatchCaseSensitive,
		DontMatchCaseInsensitive,
	}
}