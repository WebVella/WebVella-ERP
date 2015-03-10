using WebVella.ERP;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoStorageService : IStorageService
    {
        public IEntityRepository GetEntityRepository()
        {
            return new MongoEntityRepository();
        }
    }

   
}