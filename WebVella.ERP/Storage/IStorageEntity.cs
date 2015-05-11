using System;
using System.Collections.Generic;

namespace WebVella.ERP.Storage
{
    public interface IStorageEntity : IStorageObject
    {
        string Name { get; set; }

        string Label { get; set; }

        string PluralLabel { get; set; }

        bool System { get; set; }

        string IconName { get; set; }

        decimal Weight { get; set; }

        IStorageEntityPermissions Permissions { get; set; }

        List<IStorageField> Fields { get; set; }

        List<IStorageRecordsList> RecordsLists { get; set; }

        List<IStorageRecordView> RecordViewList { get; set; }
    }

    public interface IStorageEntityPermissions
    {
        List<Guid> CanRead { get; set; }

        List<Guid> CanUpdate { get; set; }

        List<Guid> CanDelete { get; set; }
    }
}