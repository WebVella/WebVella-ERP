using System;
using WebVella.ERP;
using WebVella.ERP.QueryDriver;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoStorageService : IStorageService
    {
        public IEntityQueryRepository GetEntityQueryRepository()
        {
            return new MongoEntityQueryRepository();
        }

        public IStorageEntityRepository GetEntityRepository()
        {
            return new MongoEntityRepository();
        }
    }

   
}