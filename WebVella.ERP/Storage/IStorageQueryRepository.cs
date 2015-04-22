using System.Collections.Generic;
using WebVella.ERP.QueryDriver;

namespace WebVella.ERP.Storage
{
    public interface IStorageQueryRepository : IStorageRepository
    {
        IEnumerable<EntityQueryResultDoc> Execute(string entityName, List<string> fields, EntityQueryDoc query, EntityQuerySortDoc[] sort, int page, int pageSize);
    }
}