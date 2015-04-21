using System;
using System.Collections.Generic;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage
{

    public interface IStorageViewFilter
    {
        Guid LeftEntityId { get; set; }

        Guid LeftFieldId { get; set; }

        FilterOperatorTypes Operator { get; set; }

        Guid RightEntityId { get; set; }

        Guid RightFieldId { get; set; }
    }

    public interface IStorageViewField
    {
        Guid Id { get; set; }

        Guid EntityId { get; set; }

        int Position { get; set; }
    }

    public interface IStorageView
	{
        Guid Id { get; set; }

        string Name { get; set; }

        string DisplayName { get; set; }

        ViewTypes Type { get; set; }

        IList<IStorageViewFilter> Filter { get; set; }

        IList<IStorageViewField> Fields { get; set; }
    }
}
