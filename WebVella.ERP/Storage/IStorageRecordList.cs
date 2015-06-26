using System;
using System.Collections.Generic;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage
{

    public interface IStorageRecordListFilter
    {
        Guid EntityId { get; set; }

        Guid FieldId { get; set; }

        FilterOperatorTypes Operator { get; set; }

        string Value { get; set; }
    }

    public interface IStorageRecordListField
    {
        Guid Id { get; set; }

        Guid EntityId { get; set; }

        int Position { get; set; }
    }

    public interface IStorageRecordList
	{
        Guid Id { get; set; }

        string Name { get; set; }

        string Label { get; set; }

        RecordsListTypes Type { get; set; }

        IList<IStorageRecordListFilter> Filters { get; set; }

        IList<IStorageRecordListField> Fields { get; set; }
    }
}
