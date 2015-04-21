using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEntityRepository : IStorageEntityRepository
    {
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
            return MongoStaticContext.Context.Entities.SingleOrDefault(x=>x.Id == id);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStorageEntity Read(string name)
        {
            return MongoStaticContext.Context.Entities.SingleOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant() );
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
        public bool Update( IStorageEntity entity)
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
           return MongoStaticContext.Context.Entities.Delete( Query.EQ("_id", id ) );
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