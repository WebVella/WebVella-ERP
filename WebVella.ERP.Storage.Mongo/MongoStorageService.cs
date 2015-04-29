namespace WebVella.ERP.Storage.Mongo
{
    public class MongoStorageService : IStorageService
    {
        public IStorageRecordRepository GetRecordRepository()
        {
            return new MongoRecordRepository();
        }

        public IStorageEntityRepository GetEntityRepository()
        {
            return new MongoEntityRepository();
        }
    }

   
}