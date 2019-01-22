using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Api.Models
{
	[Serializable]
	public class EntityQuery
    {
		public string EntityName { get; set; }
		public string Fields { get; set; }
		public QueryObject Query { get; set; }
		public QuerySortObject[] Sort { get; set; }
		public int? Skip { get; set; }
		public int? Limit { get; set; }
		[JsonIgnore]
		public List<KeyValuePair<string,string>> OverwriteArgs { get; set; }

		public EntityQuery(string entityName, string fields = "*", QueryObject query = null, 
			QuerySortObject[] sort = null, int? skip = null, int? limit = null, List<KeyValuePair<string, string>> overwriteArgs = null )
		{
			if (string.IsNullOrWhiteSpace(entityName))
				throw new ArgumentException("Invalid entity name.");

			if (string.IsNullOrWhiteSpace(fields))
                fields = "*";

            EntityName = entityName;
			Fields = fields;
			Query = query;
			Sort = sort;
			Skip = skip;
			Limit = limit;
			OverwriteArgs = overwriteArgs;
		}

		#region <=== Static Methods ===>

		public static QueryObject QueryEQ(string fieldName, object value)
		{
			if (string.IsNullOrWhiteSpace(fieldName))
				throw new ArgumentNullException("fieldName");

			return new QueryObject { QueryType = QueryType.EQ, FieldName = fieldName, FieldValue = value };
		}

		public static QueryObject QueryNOT(string fieldName, object value)
		{
			return new QueryObject { QueryType = QueryType.NOT, FieldName = fieldName, FieldValue = value };
		}

		public static QueryObject QueryLT(string fieldName, object value)
		{
			return new QueryObject { QueryType = QueryType.LT, FieldName = fieldName, FieldValue = value };
		}

		public static QueryObject QueryLTE(string fieldName, object value)
		{
			return new QueryObject { QueryType = QueryType.LTE, FieldName = fieldName, FieldValue = value };
		}

		public static QueryObject QueryGT(string fieldName, object value)
		{
			return new QueryObject { QueryType = QueryType.GT, FieldName = fieldName, FieldValue = value };
		}

		public static QueryObject QueryGTE(string fieldName, object value)
		{
			return new QueryObject { QueryType = QueryType.GTE, FieldName = fieldName, FieldValue = value };
		}
        
		public static QueryObject QueryAND(params QueryObject[] queries)
		{
			foreach (var query in queries)
			{
				if (query == null)
					throw new ArgumentException("Queries contains null values.");
			}

			return new QueryObject { QueryType = QueryType.AND, SubQueries = new List<QueryObject>(queries) };
		}

		public static QueryObject QueryOR(params QueryObject[] queries)
		{
			foreach (var query in queries)
			{
				if (query == null)
					throw new ArgumentException("Queries contains null values.");
			}

			return new QueryObject { QueryType = QueryType.OR, SubQueries = new List<QueryObject>(queries) };
		}

        public static QueryObject QueryContains(string fieldName, object value)
        {
            return new QueryObject { QueryType = QueryType.CONTAINS, FieldName = fieldName, FieldValue = value };
        }

        public static QueryObject QueryStartsWith(string fieldName, object value)
        {
            return new QueryObject { QueryType = QueryType.STARTSWITH, FieldName = fieldName, FieldValue = value };
        }

        public static QueryObject QueryRegex(string fieldName, object value, QueryObjectRegexOperator op = QueryObjectRegexOperator.MatchCaseSensitive )
        {
            return new QueryObject { QueryType = QueryType.REGEX, FieldName = fieldName, FieldValue = value, RegexOperator = op };
        }

		public static QueryObject QueryFTS(string fieldName, object value, string language = null )
		{
			return new QueryObject { QueryType = QueryType.FTS, FieldName = fieldName, FieldValue = value, FtsLanguage = language };
		}

		public static QueryObject Related(string relationName, string direction = "origin-target")
		{
			return new QueryObject { QueryType = QueryType.RELATED, FieldName = relationName, FieldValue = direction };
		}

		public static QueryObject NotRelated(string relationName, string direction = "origin-target")
		{
			return new QueryObject { QueryType = QueryType.NOTRELATED, FieldName = relationName, FieldValue = direction };
		}

		#endregion
	}
}