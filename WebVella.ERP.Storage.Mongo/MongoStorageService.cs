using System;
using WebVella.ERP;
using WebVella.ERP.QueryDriver;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoStorageService : IStorageService
    {
        public IStorageQueryRepository GetQueryRepository()
        {
            return new MongoQueryRepository();
        }

        public IStorageEntityRepository GetEntityRepository()
        {
            return new MongoEntityRepository();
        }
    }

   
}