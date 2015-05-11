using System;
using System.Collections.Generic;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage
{

    public interface IStorageRecordsListFilter
    {
        Guid EntityId { get; set; }

        Guid FieldId { get; set; }

        FilterOperatorTypes Operator { get; set; }

        string Value { get; set; }
    }

    public interface IStorageRecordsListField
    {
        Guid Id { get; set; }

        Guid EntityId { get; set; }

        int Position { get; set; }
    }

    public interface IStorageRecordsList
	{
        Guid Id { get; set; }

        string Name { get; set; }

        string Label { get; set; }

        ViewTypes Type { get; set; }

        IList<IStorageRecordsListFilter> Filters { get; set; }

        IList<IStorageRecordsListField> Fields { get; set; }
    }
}
