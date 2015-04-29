using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
    public class View
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public ViewTypes Type { get; set; }

        public IList<ViewFilter> Filters { get; set; }

        public IList<ViewField> Fields { get; set; }
    }

    public class ViewFilter
    {
        public Guid? EntityId { get; set; }

        public Guid? FieldId { get; set; }

        public FilterOperatorTypes Operator { get; set; }

        public string Value { get; set; }
    }

    public class ViewField
    {
        public Guid? Id { get; set; }

        public Guid? EntityId { get; set; }

        public int? Position { get; set; }
    }
}