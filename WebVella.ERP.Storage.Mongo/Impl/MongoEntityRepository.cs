using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEntityRepository : IStorageEntityRepository
    {

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
            return MongoStaticContext.Context.Entities.Get().ToList<IStorageEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStorageEntity Read(Guid id)
        {
            return MongoStaticContext.Context.Entities.SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStorageEntity Read(string name)
        {
            return MongoStaticContext.Context.Entities.SingleOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public bool Create(IStorageEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var mongoEntity = entity as MongoEntity;

            if (mongoEntity == null)
                throw new Exception("The specified entity is not mongo storage object.");

            return MongoStaticContext.Context.Entities.Create(mongoEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public bool Update(IStorageEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var mongoEntity = entity as MongoEntity;

            if (mongoEntity == null)
                throw new Exception("The specified entity is not mongo storage object.");

            return MongoStaticContext.Context.Entities.Update(mongoEntity);
        }

        /// <summary>
        /// Deletes entity document by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            var transaction = MongoStaticContext.Context.CreateTransaction();
            try
            {
                //remove value storage
                var entity = Read(id);
                string relationCollectionName = RECORD_COLLECTION_PREFIX + entity.Name;
                if (MongoStaticContext.Context.Database.CollectionExists(relationCollectionName))
                    MongoStaticContext.Context.Database.DropCollection(relationCollectionName);

                var relRepo = new MongoEntityRelationRepository();
                var entityRelations = relRepo.Read().Where(x => x.OriginEntityId == entity.Id || x.TargetEntityId == entity.Id).ToList();
                foreach ( var relation in entityRelations)
                    relRepo.Delete(relation.Id);
             
                var result = MongoStaticContext.Context.Entities.Delete(Query.EQ("_id", id));
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Saves entity document
        /// </summary>
        /// <param name="entity"></param>
        public bool Save(IStorageEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var mongoEntity = entity as MongoEntity;

            if (mongoEntity == null)
                throw new Exception("The specified entity is not mongo storage object.");

            return MongoStaticContext.Context.Entities.Save(mongoEntity);
        }
    }
}