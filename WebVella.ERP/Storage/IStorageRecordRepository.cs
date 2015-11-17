using System;
using System.Collections.Generic;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage
{
    public interface IStorageRecordRepository : IStorageRepository
    {
        void Create(string entityName, IEnumerable<KeyValuePair<string, object>> recordData);

        IEnumerable<KeyValuePair<string, object>> Update(string entityName, IEnumerable<KeyValuePair<string, object>> recordData);

        IEnumerable<KeyValuePair<string, object>> Delete(string entityName, Guid id);

        IEnumerable<IEnumerable<KeyValuePair<string, object>>> Find(string entityName, QueryObject query, QuerySortObject[] sort, int? skip, int? limit);

        long Count(string entityName, QueryObject query);

        IEnumerable<KeyValuePair<string, object>> Find(string entityName, Guid id);

        IStorageTransaction CreateTransaction();

        void CreateRecordField(string entityName, string fieldName, object value);

		void RemoveRecordField(string entityName, string fieldName);

		void CreateAutoNumberRecordField(string entityName, string fieldName, decimal initialValue);

		decimal GetAutoNumberRecordFieldMaxValue(string entityName, string fieldName);
	}
}