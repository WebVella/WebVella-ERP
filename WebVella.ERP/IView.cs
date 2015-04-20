using System;
using System.Collections.Generic;

namespace WebVella.ERP
{
    public enum ViewTypes
    {
        SearchPopup = 1,
        List,
        Custom
    }

    public enum FilterOperatorTypes
    {
        Equals = 1,
        NotEqualTo,
        StartsWith,
        Contains,
        DoesNotContain,
        LessThan,
        GreaterThan,
        LessOrEqual,
        GreaterOrEqual,
        Includes,
        Excludes,
        Within
    }

    public interface IViewFilter
    {
        Guid LeftEntityId { get; set; }

        Guid LeftFieldId { get; set; }

        FilterOperatorTypes Operator { get; set; }

        Guid RightEntityId { get; set; }

        Guid RightFieldId { get; set; }
    }

    public interface IViewField
    {
        Guid Id { get; set; }

        Guid EntityId { get; set; }

        int Position { get; set; }
    }

    public interface IView
	{
        Guid Id { get; set; }

        string Name { get; set; }

        string DisplayName { get; set; }

        ViewTypes Type { get; set; }

        IList<IViewFilter> Filter { get; set; }

        IList<IViewField> Fields { get; set; }
    }
}
