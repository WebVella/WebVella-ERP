using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEntityRelationRepository : IStorageEntityRelationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStorageEntityRelation Empty()
        {
            return new MongoEntityRelation();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IStorageEntityRelation> Read()
        {
            return MongoStaticContext.Context.EntityRelations.Get().ToList<IStorageEntityRelation>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStorageEntityRelation Read(Guid id)
        {
            return MongoStaticContext.Context.EntityRelations.SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStorageEntityRelation Read(string name)
        {
            return MongoStaticContext.Context.EntityRelations.SingleOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public bool Create(IStorageEntityRelation entityRelation)
        {
            if (entityRelation == null)
                throw new ArgumentNullException("entityRelation");

            var mongoEntityRelation = entityRelation as MongoEntityRelation;

            if (mongoEntityRelation == null)
                throw new Exception("The specified entityRelation is not mongo storage object.");

            return MongoStaticContext.Context.EntityRelations.Create(mongoEntityRelation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public bool Update(IStorageEntityRelation entityRelation)
        {
            if (entityRelation == null)
                throw new ArgumentNullException("entityRelation");

            var mongoEntityRelation = entityRelation as MongoEntityRelation;

            if (mongoEntityRelation == null)
                throw new Exception("The specified entityRelation is not mongo storage object.");

            return MongoStaticContext.Context.EntityRelations.Update(mongoEntityRelation);
        }

        /// <summary>
        /// Deletes entity document by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            return MongoStaticContext.Context.EntityRelations.Delete(Query.EQ("_id", id));
        }

        /// <summary>
        /// Saves entity document
        /// </summary>
        /// <param name="entity"></param>
        public bool Save(IStorageEntityRelation entityRelation)
        {
            if (entityRelation == null)
                throw new ArgumentNullException("entityRelation");

            var mongoEntityRelation = entityRelation as MongoEntityRelation;

            if (mongoEntityRelation == null)
                throw new Exception("The specified entityRelation is not mongo storage object.");

            return MongoStaticContext.Context.EntityRelations.Save(mongoEntityRelation);
        }
    }
}