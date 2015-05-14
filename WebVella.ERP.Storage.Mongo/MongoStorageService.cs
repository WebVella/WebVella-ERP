namespace WebVella.ERP.Storage.Mongo
{
    public class MongoStorageService : IStorageService
    {
        public IStorageObjectFactory GetObjectFactory()
        {
            return new MongoStorageObjectFactory();
        }

        public IStorageSystemSettingsRepository GetSystemSettingsRepository()
        {
            return new MongoSystemSettingsRepository();
        }

        public IStorageRecordRepository GetRecordRepository()
        {
            return new MongoRecordRepository();
        }

        public IStorageEntityRepository GetEntityRepository()
        {
            return new MongoEntityRepository();
        }

        public IStorageEntityRelationRepository GetEntityRelationRepository()
        {
            return new MongoEntityRelationRepository();
        }
    }

   
}