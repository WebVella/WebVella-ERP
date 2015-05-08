using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;

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

    public class ViewList
    {
        public Guid Offset { get; set; }

        public List<View> Views { get; set; }
    }

    public class ViewResponse : BaseResponseModel
    {
        public View Object { get; set; }
    }

    public class ViewListResponse : BaseResponseModel
    {
        public ViewList Object { get; set; }
    }
}