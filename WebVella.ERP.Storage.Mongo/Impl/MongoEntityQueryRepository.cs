using System.Collections.Generic;
using WebVella.ERP.QueryDriver;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEntityQueryRepository : IEntityQueryRepository
    {
        public IEnumerable<EntityQueryResultDoc> Execute(string entityName, List<string> fields, EntityQueryDoc query, EntityQuerySortDoc[] sort, int page, int pageSize)
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}