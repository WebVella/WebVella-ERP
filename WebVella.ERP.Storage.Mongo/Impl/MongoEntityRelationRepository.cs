using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEntityRelationRepository : IStorageEntityRelationRepository
    {
        private static object lockObject = new object();
        private static List<IStorageEntityRelation> cachedRelations = null;
        private const string RELATION_COLLECTION_PREFIX = "rel_";

        /// <summary>
        /// Reads relations records
        /// </summary>
        /// <returns></returns>
        public List<IStorageEntityRelation> Read()
        {
            lock (lockObject)
            {
                if (cachedRelations == null)
                    cachedRelations = MongoStaticContext.Context.EntityRelations.Get().ToList<IStorageEntityRelation>();

                return cachedRelations;
            }
        }

        /// <summary>
        /// Read single relation record specified by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStorageEntityRelation Read(Guid id)
        {
            lock( lockObject)
            {
                if (cachedRelations == null)
                    cachedRelations = Read();

                return cachedRelations.SingleOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Read single relation record specified by its name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStorageEntityRelation Read(string name)
        {
            lock (lockObject)
            {
                if (cachedRelations == null)
                    cachedRelations = Read();

                return cachedRelations.SingleOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant());
            }
        }

        /// <summary>
        /// Creates entity relation
        /// </summary>
        /// <param name="entity"></param>
        public bool Create(IStorageEntityRelation entityRelation)
        {
            lock( lockObject )
            {
                if (entityRelation == null)
                    throw new ArgumentNullException("entityRelation");

                var mongoEntityRelation = entityRelation as MongoEntityRelation;

                if (mongoEntityRelation == null)
                    throw new Exception("The specified entityRelation is not mongo storage object.");

                cachedRelations = null;

                return MongoStaticContext.Context.EntityRelations.Create(mongoEntityRelation);
            }
        }

        /// <summary>
        /// Updates entity relation
        /// </summary>
        /// <param name="entity"></param>
        public bool Update(IStorageEntityRelation entityRelation)
        {
            lock( lockObject )
            {
                if (entityRelation == null)
                    throw new ArgumentNullException("entityRelation");

                var mongoEntityRelation = entityRelation as MongoEntityRelation;

                if (mongoEntityRelation == null)
                    throw new StorageException("The specified entityRelation is not mongo storage object.");

                cachedRelations = null;

                return MongoStaticContext.Context.EntityRelations.Update(mongoEntityRelation);
            }
        }

        /// <summary>
        /// Deletes entity relation 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            lock( lockObject )
            {
                var transaction = MongoStaticContext.Context.CreateTransaction();
                try
                {

                    //remove system collection for many to many collection
                    var relation = Read(id);
                    string relationCollectionName = RELATION_COLLECTION_PREFIX + relation.Name;
                    if (MongoStaticContext.Context.Database.CollectionExists(relationCollectionName))
                        MongoStaticContext.Context.Database.DropCollection(relationCollectionName);

                    var result = MongoStaticContext.Context.EntityRelations.Delete(Query.EQ("_id", id));
                    transaction.Commit();
                    cachedRelations = null;
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Saves entity relation
        /// </summary>
        /// <param name="entity"></param>
        public bool Save(IStorageEntityRelation entityRelation)
        {
            lock( lockObject )
            {
                if (entityRelation == null)
                    throw new ArgumentNullException("entityRelation");

                var mongoEntityRelation = entityRelation as MongoEntityRelation;

                if (mongoEntityRelation == null)
                    throw new StorageException("The specified entityRelation is not mongo storage object.");

                cachedRelations = null;

                return MongoStaticContext.Context.EntityRelations.Save(mongoEntityRelation);
            }
        }

        /// <summary>
        /// Created many to many relation record
        /// </summary>
        /// <param name="relationName"></param>
        /// <param name="originId"></param>
        /// <param name="targetId"></param>
        public void CreateManyToManyRecord(Guid relationId, Guid originId, Guid targetId)
        {
            var relation = Read(relationId);
            string relationCollectionName = RELATION_COLLECTION_PREFIX + relation.Name;
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(relationCollectionName);

            var query = Query.And(Query.EQ("relationId", relationId), Query.EQ("targetId", targetId), Query.EQ("originId", originId));
            bool recordExists = mongoCollection.Find(query).Count() > 0;

            if (recordExists)
                throw new StorageException("A record with same arguments already exists.");

            BsonDocument doc = new BsonDocument();
            doc["relationId"] = BsonValue.Create(relationId);
            doc["originId"] = BsonValue.Create(originId);
            doc["targetId"] = BsonValue.Create(targetId);

            mongoCollection.Insert(doc);
        }

        /// <summary>
        /// Deletes many to many relation record
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="originId"></param>
        /// <param name="targetId"></param>
        public void DeleteManyToManyRecord(Guid relationId, Guid originId, Guid targetId)
        {
            var relation = Read(relationId);
            string relationCollectionName = RELATION_COLLECTION_PREFIX + relation.Name;
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(relationCollectionName);
            var query = Query.And(Query.EQ("relationId", relationId), Query.EQ("targetId", targetId), Query.EQ("originId", originId));
            mongoCollection.Remove(query);
        }

        /// <summary>
        /// Reads list of ids asociated to target entity, filtered by relation and origin id
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="originId"></param>
        /// <returns></returns>
        public List<Guid> ReadManyToManyRecordByOrigin(Guid relationId, Guid originId)
        {
            var relation = Read(relationId);
            string relationCollectionName = RELATION_COLLECTION_PREFIX + relation.Name;
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(relationCollectionName);

            var query = Query.And(Query.EQ("relationId", relationId), Query.EQ("originId", originId));
            var records = mongoCollection.Find(query);
            return records.Select(x => (Guid)x["targetId"]).ToList();
        }

        /// <summary>
        /// Reads list of ids asociated to origin entity, filtered by relation and target id
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="originId"></param>
        /// <returns></returns>
        public List<Guid> ReadManyToManyRecordByTarget(Guid relationId, Guid targetId)
        {
            var relation = Read(relationId);
            string relationCollectionName = RELATION_COLLECTION_PREFIX + relation.Name;
            var mongoCollection = MongoStaticContext.Context.GetBsonCollection(relationCollectionName);

            var query = Query.And(Query.EQ("relationId", relationId), Query.EQ("targetId", targetId));
            var records = mongoCollection.Find(query);
            return records.Select(x => (Guid)x["originId"]).ToList();
        }
    }
}