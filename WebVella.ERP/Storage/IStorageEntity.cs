using System;
using System.Collections.Generic;

namespace WebVella.ERP.Storage
{
    public interface IStorageEntity : IStorageObject
    {
        string Name { get; set; }

        string Label { get; set; }

        string LabelPlural { get; set; }

        bool System { get; set; }

        string IconName { get; set; }

        decimal Weight { get; set; }

        IStorageRecordPermissions RecordPermissions { get; set; }

        List<IStorageField> Fields { get; set; }

        List<IStorageRecordList> RecordLists { get; set; }

        List<IStorageRecordView> RecordViews { get; set; }

		List<IStorageRecordTree> RecordTrees { get; set; }
	}

    public interface IStorageRecordPermissions
    {
        List<Guid> CanRead { get; set; }

        List<Guid> CanCreate { get; set; }

        List<Guid> CanUpdate { get; set; }

        List<Guid> CanDelete { get; set; }
    }

    public interface IStorageFieldPermissions
    {
        List<Guid> CanRead { get; set; }

        List<Guid> CanUpdate { get; set; }
    }
}