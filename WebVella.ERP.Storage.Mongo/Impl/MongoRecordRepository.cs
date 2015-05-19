using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using WebVella.ERP.Api.Models;
using System;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoRecordRepository : IStorageRecordRepository
    {
        private const string RECORD_COLLECTION_PREFIX = "rec_";

        public void Create(string entityName, IEnumerable<KeyValuePair<string, object>> recordData)
        {
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(RECORD_COLLECTION_PREFIX + entityName);
            BsonDocument doc = new BsonDocument();
            foreach (var record in recordData)
            {
                var value = record.Value == null ? BsonNull.Value : BsonValue.Create(record.Value);
                doc[ProcessQueryIDFieldName(record.Key)] = value;
            }
            mongoCollection.Insert(doc);
        }


        public IEnumerable<KeyValuePair<string, object>> Find(string entityName, Guid id)
        {
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(RECORD_COLLECTION_PREFIX + entityName);
            var doc = mongoCollection.FindOne(Query.EQ("_id", id));
            if (doc == null)
                return null;

            List<KeyValuePair<string, object>> record = new List<KeyValuePair<string, object>>();
            foreach (var fieldName in doc.Names)
            {
                if (fieldName == "_id")
                    record.Add(new KeyValuePair<string, object>("id", BsonTypeMapper.MapToDotNetValue(doc["_id"])));
                else
                    record.Add(new KeyValuePair<string, object>(fieldName, BsonTypeMapper.MapToDotNetValue(doc[fieldName])));
            }

            return record;
        }

        public IEnumerable<IEnumerable<KeyValuePair<string, object>>> Find(string entityName, QueryObject query, QuerySortObject[] sort, int? skip, int? limit)
        {
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(RECORD_COLLECTION_PREFIX + entityName);
            var mongoQuery = ConvertQuery(query);
            var cursor = mongoCollection.Find(mongoQuery);

            if (sort != null && sort.Length > 0)
            {
                SortByBuilder sortBy = null;
                foreach (var s in sort)
                {
                    if (s.SortType == QuerySortType.Ascending)
                        sortBy = sortBy == null ? SortBy.Ascending(s.FieldName) : sortBy.Ascending(s.FieldName);
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
            foreach (BsonDocument doc in cursor)
            {
                List<KeyValuePair<string, object>> record = new List<KeyValuePair<string, object>>();
                foreach (var fieldName in doc.Names)
                {
                    if (fieldName == "_id")
                        record.Add(new KeyValuePair<string, object>("id", BsonTypeMapper.MapToDotNetValue(doc["_id"]) ) );
                    else
                        record.Add(new KeyValuePair<string, object>(fieldName, BsonTypeMapper.MapToDotNetValue(doc[fieldName] )));
                }
                result.Add(record);
            }
            return result;
        }

        private IMongoQuery ConvertQuery(QueryObject query)
        {
            if (query == null)
                return Query.Null;

            var value = query.FieldValue == null ? BsonNull.Value : BsonValue.Create(query.FieldValue);

            switch (query.QueryType)
            {
                case QueryType.EQ:
                    return Query.EQ(ProcessQueryIDFieldName(query.FieldName), value);
                case QueryType.NOT:
                    return Query.Not(Query.EQ(ProcessQueryIDFieldName(query.FieldName), value));
                case QueryType.LT:
                    return Query.LT(ProcessQueryIDFieldName(query.FieldName), value);
                case QueryType.LTE:
                    return Query.LTE(ProcessQueryIDFieldName(query.FieldName), value);
                case QueryType.GT:
                    return Query.GT(ProcessQueryIDFieldName(query.FieldName), value);
                case QueryType.GTE:
                    return Query.GTE(ProcessQueryIDFieldName(query.FieldName), value);
                case QueryType.CONTAINS:
                    {
                        var regex = new BsonRegularExpression( string.Format( ".*{0}.*", value ), "i" ); // contains, ignore case
                        return Query.Matches(ProcessQueryIDFieldName(query.FieldName), regex );
                    }
                case QueryType.STARTSWITH:
                    {
                        var regex = new BsonRegularExpression(string.Format("^{0}", value), "i"); // starts with, ignore case
                        return Query.Matches(ProcessQueryIDFieldName(query.FieldName), regex);
                    }

                case QueryType.AND:
                    {
                        List<IMongoQuery> queries = new List<IMongoQuery>();
                        foreach (var q in query.SubQueries)
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
                    throw new Exception("Not supported query type");
            }
        }

        private string ProcessQueryIDFieldName(string fieldName)
        {
            if (fieldName == "id" || fieldName == "Id")
                return "_id";

            return fieldName;
        }
    }
}