using System.Collections.Generic;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage
{
    public interface IStorageRecordRepository : IStorageRepository
    {
		IEnumerable<IEnumerable<KeyValuePair<string, object>>> Find(string entityName, QueryObject query, QuerySortObject[] sort, int? skip, int? limit);
    }
}