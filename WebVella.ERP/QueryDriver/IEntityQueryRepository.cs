using System.Collections.Generic;
using WebVella.ERP.QueryDriver;

namespace WebVella.ERP.QueryDriver
{
    public interface IEntityQueryRepository : IRepository
    {
        IEnumerable<EntityQueryResultDoc> Execute(string entityName, List<string> fields, EntityQueryDoc query, EntityQuerySortDoc[] sort, int page, int pageSize);
    }
}