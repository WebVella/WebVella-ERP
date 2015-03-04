using WebVella.ERP.Storage.Mongo;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEntityRepository : IEntityRepository
    {
        public IEntity Get()
        {
            return new MongoEntity { Name = "Sample Entity" };
        }
    }
}