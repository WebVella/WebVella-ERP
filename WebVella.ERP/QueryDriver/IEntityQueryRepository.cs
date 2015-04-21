using System.Collections.Generic;
using WebVella.ERP.QueryDriver;
using WebVella.ERP.Storage;

namespace WebVella.ERP.QueryDriver
{
    public interface IEntityQueryRepository : IStorageRepository
    {
        IEnumerable<EntityQueryResultDoc> Execute(string entityName, List<string> fields, EntityQueryDoc query, EntityQuerySortDoc[] sort, int page, int pageSize);
    }
}