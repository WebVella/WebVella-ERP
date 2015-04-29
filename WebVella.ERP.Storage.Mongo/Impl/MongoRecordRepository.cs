using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoRecordRepository : IStorageRecordRepository
    {
        public IEnumerable<IEnumerable<KeyValuePair<string, object>>> Find(string entityName, QueryObject query, QuerySortObject[] sort, int? skip, int? limit)
        {
			var mongoCollection = MongoStaticContext.Context.GetBsonCollection(entityName);
			var cursor = mongoCollection.Find(ConvertQuery(query));

			if ( sort != null && sort.Length > 0 )
			{
				SortByBuilder sortBy = null;
				foreach (var s in sort)
				{
					if (s.SortType == QuerySortType.Ascending)
						sortBy = sortBy == null  ? SortBy.Ascending(s.FieldName) : sortBy.Ascending(s.FieldName);
					else
						sortBy = sortBy == null ? SortBy.Descending(s.FieldName) : sortBy.Descending(s.FieldName);
				}
				cursor.SetSortOrder(sortBy);
			}

			if (skip.HasValue)
				cursor.SetSkip(skip.Value);

			if (limit.HasValue)
				cursor.SetLimit(limit.Value);

			List<List<KeyValuePair<string, object>>> result = new List<List<KeyValuePair<string, object>>>();
			foreach( BsonDocument doc in cursor )
			{
				List<KeyValuePair<string, object>> record = new List<KeyValuePair<string, object>>();
                foreach (var fieldName in doc.Names)
					record.Add(new KeyValuePair<string, object>(fieldName, doc[fieldName]));
				result.Add(record);
			}
			return result;
        }

		private IMongoQuery ConvertQuery(QueryObject query)
		{
			if (query == null)
				return Query.Null;

			switch (query.QueryType)
			{
				case QueryType.EQ:
					{
						var value = query.FieldValue == null ? BsonNull.Value.ToBsonDocument() : query.FieldValue.ToBsonDocument();
						return Query.EQ( query.FieldName, value );
					}
				case QueryType.NOT:
					{
						var value = query.FieldValue == null ? BsonNull.Value.ToBsonDocument() : query.FieldValue.ToBsonDocument();
						return Query.Not(Query.EQ(query.FieldName, value));
					}
				case QueryType.LT:
					{
						var value = query.FieldValue == null ? BsonNull.Value.ToBsonDocument() : query.FieldValue.ToBsonDocument();
						return Query.LT(query.FieldName, value);
					}
				case QueryType.LTE:
					{
						var value = query.FieldValue == null ? BsonNull.Value.ToBsonDocument() : query.FieldValue.ToBsonDocument();
						return Query.LTE(query.FieldName, value);
					}
				case QueryType.GT:
					{
						var value = query.FieldValue == null ? BsonNull.Value.ToBsonDocument() : query.FieldValue.ToBsonDocument();
						return Query.GT(query.FieldName, value);
					}
				case QueryType.GTE:
					{
						var value = query.FieldValue == null ? BsonNull.Value.ToBsonDocument() : query.FieldValue.ToBsonDocument();
						return Query.GTE(query.FieldName, value);
					}
				case QueryType.AND:
					{
						List<IMongoQuery> queries = new List<IMongoQuery>();
						foreach( var q in query.SubQueries )
							queries.Add(ConvertQuery(q));
						return Query.And(queries);
					}
				case QueryType.OR:
					{
						List<IMongoQuery> queries = new List<IMongoQuery>();
						foreach (var q in query.SubQueries)
							queries.Add(ConvertQuery(q));
						return Query.Or(queries);
					}
				default:
					throw new System.Exception("Not supported query type");
			}
		} 
    }
}