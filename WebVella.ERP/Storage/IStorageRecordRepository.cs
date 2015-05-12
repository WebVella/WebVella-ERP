using System;
using System.Collections.Generic;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage
{
    public interface IStorageRecordRepository : IStorageRepository
    {
        void Create(string entityName, IEnumerable<KeyValuePair<string, object>> recordData);

        IEnumerable<IEnumerable<KeyValuePair<string, object>>> Find(string entityName, QueryObject query, QuerySortObject[] sort, int? skip, int? limit);

        IEnumerable<KeyValuePair<string, object>> Find(string entityName, Guid id);
    }
}