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
        internal const string RECORD_COLLECTION_PREFIX = "rec_";

        /// <summary>
        /// Create record
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="recordData"></param>
        public void Create(string entityName, IEnumerable<KeyValuePair<string, object>> recordData)
        {
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(RECORD_COLLECTION_PREFIX + entityName);

            BsonDocument doc = new BsonDocument();

            foreach (var record in recordData)
                doc[ProcessQueryIDFieldName(record.Key)] = ConvertObjectToBsonValue(record.Value);

            mongoCollection.Insert(doc);
        }

        /// <summary>
        /// Updates record
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="recordData"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, object>> Update(string entityName, IEnumerable<KeyValuePair<string, object>> recordData)
        {
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(RECORD_COLLECTION_PREFIX + entityName);

            Guid? id = null;
            foreach (var rec in recordData)
            {
                string fieldName = ProcessQueryIDFieldName(rec.Key);
                if (fieldName == "_id")
                    id = (Guid)ConvertObjectToBsonValue(rec.Value);
            }

            if (id == null)
                throw new StorageException("ID is missing. Cannot update records without ID specified.");


            var doc = mongoCollection.FindOne(Query.EQ("_id", id));
            if (doc == null)
                throw new StorageException("There is no document with such id to update.");

            foreach (var rec in recordData)
            {
                string fieldName = ProcessQueryIDFieldName(rec.Key);
                doc[fieldName] = ConvertObjectToBsonValue(rec.Value);
            }

            var updateSuccess = mongoCollection.Save(doc).DocumentsAffected > 0;
            if (!updateSuccess )
                throw new StorageException("Failed to update record.");

            List<KeyValuePair<string, object>> record = new List<KeyValuePair<string, object>>();
            foreach (var fieldName in doc.Names)
            {
                if (fieldName == "_id")
                    record.Add(new KeyValuePair<string, object>("id", BsonTypeMapper.MapToDotNetValue(doc["_id"])));
                else
                    record.Add(new KeyValuePair<string, object>(fieldName, ConvertBsonValueToObject(doc[fieldName])));
            }

            return record;
        }

        /// <summary>
        /// Deletes record
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, object>> Delete(string entityName, Guid id)
        {
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(RECORD_COLLECTION_PREFIX + entityName);

            var doc = mongoCollection.FindOne(Query.EQ("_id", id));
            if (doc == null)
                throw new StorageException("There is no document with such id to update.");

            mongoCollection.Remove(Query.EQ("_id", id));

            List<KeyValuePair<string, object>> record = new List<KeyValuePair<string, object>>();
            foreach (var fieldName in doc.Names)
            {
                if (fieldName == "_id")
                    record.Add(new KeyValuePair<string, object>("id", BsonTypeMapper.MapToDotNetValue(doc["_id"])));
                else
                    record.Add(new KeyValuePair<string, object>(fieldName, ConvertBsonValueToObject(doc[fieldName])));
            }

            return record;
        }

        /// <summary>
        /// Find record
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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
                    record.Add(new KeyValuePair<string, object>(fieldName, ConvertBsonValueToObject(doc[fieldName])));
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
                        record.Add(new KeyValuePair<string, object>("id", BsonTypeMapper.MapToDotNetValue(doc["_id"])));
                    else
                        record.Add(new KeyValuePair<string, object>(fieldName, ConvertBsonValueToObject(doc[fieldName])));
                }
                result.Add(record);
            }
            return result;
        }

        public long Count(string entityName, QueryObject query )
        {
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(RECORD_COLLECTION_PREFIX + entityName);
            var mongoQuery = ConvertQuery(query);
            return mongoCollection.Count(mongoQuery);
        }

        private IMongoQuery ConvertQuery(QueryObject query)
        {
            if (query == null)
                return Query.Null;

            BsonValue value = ConvertObjectToBsonValue(query.FieldValue);

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
                        var regex = new BsonRegularExpression(string.Format(".*{0}.*", value), "i"); // contains, ignore case
                        return Query.Matches(ProcessQueryIDFieldName(query.FieldName), regex);
                    }
                case QueryType.STARTSWITH:
                    {
                        var regex = new BsonRegularExpression(string.Format("^{0}", value), "i"); // starts with, ignore case
                        return Query.Matches(ProcessQueryIDFieldName(query.FieldName), regex);
                    }
                case QueryType.REGEX:
                    {
                        var regex = new BsonRegularExpression(string.Format("{0}", value));
                        return Query.Matches(ProcessQueryIDFieldName(query.FieldName), regex);
                    }
				case QueryType.RELATED:
					{
						return  Query.And( Query.Not( Query.Size(query.FieldName, 0) ), 
											Query.Not( Query.EQ( query.FieldName, BsonNull.Value ) ));
					}
				case QueryType.NOTRELATED:
					{
						return Query.Or( Query.Size(query.FieldName, 0), 
										Query.EQ(query.FieldName, BsonNull.Value), 
										Query.Not( Query.Exists( query.FieldName)) );
					}
				case QueryType.AND:
                    {
                        List<IMongoQuery> queries = new List<IMongoQuery>();
                        if (query.SubQueries.Count == 1)
                            return ConvertQuery(query.SubQueries[0]);
                        else
                        {
                            foreach (var q in query.SubQueries)
                                queries.Add(ConvertQuery(q));
                            return Query.And(queries);
                        }
                    }
                case QueryType.OR:
                    {
                        List<IMongoQuery> queries = new List<IMongoQuery>();
                        if (query.SubQueries.Count == 1)
                            return ConvertQuery(query.SubQueries[0]);
                        else
                        {
                            foreach (var q in query.SubQueries)
                                queries.Add(ConvertQuery(q));
                            return Query.Or(queries);
                        }
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

        private BsonValue ConvertObjectToBsonValue(object value)
        {
            if (MongoTypeConvertor.IsNumber(value))
                return value == null ? BsonNull.Value : BsonValue.Create(MongoTypeConvertor.ToLong(value));
            else
                return value == null ? BsonNull.Value : BsonValue.Create(value);

        }

        private object ConvertBsonValueToObject( BsonValue bsonValue)
        {
            var value = BsonTypeMapper.MapToDotNetValue(bsonValue);
            if (value is long)
                return MongoTypeConvertor.LongToDecimal(value);

            return value;

        }

        public IStorageTransaction CreateTransaction()
        {
			return  MongoStaticContext.Context.CreateTransaction(false);
        }

        public void CreateRecordField(string entityName, string fieldName, object value)
        {
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(RECORD_COLLECTION_PREFIX + entityName);
            mongoCollection.Update(Query.Null, MongoDB.Driver.Builders.Update.Set(fieldName, ConvertObjectToBsonValue(value)), UpdateFlags.Multi);
        }

        public void RemoveRecordField(string entityName, string fieldName)
        {
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(RECORD_COLLECTION_PREFIX + entityName);
            mongoCollection.Update(Query.Null, MongoDB.Driver.Builders.Update.Unset(fieldName), UpdateFlags.Multi);
        }

    }
}