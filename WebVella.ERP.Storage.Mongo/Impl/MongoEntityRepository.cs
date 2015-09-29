using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEntityRepository : IStorageEntityRepository
    {
        private static object lockObject = new object();
        private static List<IStorageEntity> cachedEntities;
        private const string RECORD_COLLECTION_PREFIX = "rec_";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStorageEntity Empty()
        {
            return new MongoEntity();
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IStorageEntity> Read()
        {
            lock( lockObject )
            {
                if (cachedEntities == null)
                    cachedEntities = MongoStaticContext.Context.Entities.Get().ToList<IStorageEntity>();

                return cachedEntities;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStorageEntity Read(Guid id)
        {
            lock( lockObject )
            {
                if (cachedEntities == null)
                    cachedEntities = Read();

                return cachedEntities.SingleOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStorageEntity Read(string name)
        {
            lock (lockObject)
            {
                if (cachedEntities == null)
                    cachedEntities = Read();

                return cachedEntities.SingleOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public bool Create(IStorageEntity entity)
        {
            lock (lockObject)
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                var mongoEntity = entity as MongoEntity;

                if (mongoEntity == null)
                    throw new Exception("The specified entity is not mongo storage object.");

                cachedEntities = null;

                return MongoStaticContext.Context.Entities.Create(mongoEntity);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public bool Update(IStorageEntity entity)
        {
            lock (lockObject)
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                var mongoEntity = entity as MongoEntity;

                if (mongoEntity == null)
                    throw new Exception("The specified entity is not mongo storage object.");

                cachedEntities = null;

                return MongoStaticContext.Context.Entities.Update(mongoEntity);
            }
        }

        /// <summary>
        /// Deletes entity document by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            lock (lockObject)
            {
                var transaction = MongoStaticContext.Context.CreateTransaction();
                try
                {
                    //remove value storage
                    var entity = Read(id);
                    string relationCollectionName = RECORD_COLLECTION_PREFIX + entity.Name;
                    if (MongoStaticContext.Context.Database.CollectionExists(relationCollectionName))
                        MongoStaticContext.Context.Database.DropCollection(relationCollectionName);

                    var result = MongoStaticContext.Context.Entities.Delete(Query.EQ("_id", id));
                    transaction.Commit();

                    cachedEntities = null;

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
        /// Saves entity document
        /// </summary>
        /// <param name="entity"></param>
        public bool Save(IStorageEntity entity)
        {
            lock (lockObject)
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                var mongoEntity = entity as MongoEntity;

                if (mongoEntity == null)
                    throw new Exception("The specified entity is not mongo storage object.");

                cachedEntities = null;

                return MongoStaticContext.Context.Entities.Save(mongoEntity);
            }
        }
    }
}