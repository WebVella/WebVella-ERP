using System;
using System.Collections.Generic;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage
{
    public interface IStorageRecordViewField
    {
        Guid Id { get; set; }

        Guid EntityId { get; set; }

        RecordViewColumns Column { get; set; }

        int Position { get; set; }
    }

    public interface IStorageRecordView
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string Label { get; set; }

        IList<IStorageRecordViewField> Fields { get; set; }
    }
}