using System;
using System.Collections.Generic;

namespace WebVella.ERP.Storage.Mongo.Impl
{
    public class MongoView : IView
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public ViewTypes Type { get; set; }

        public IList<IViewFilter> Filter { get; set; }

        public IList<IViewField> Fields { get; set; }
    }

    public class ViewFilter :  IViewFilter
    {
        public Guid LeftEntityId { get; set; }

        public Guid LeftFieldId { get; set; }

        public FilterOperatorTypes Operator { get; set; }

        public Guid RightEntityId { get; set; }

        public Guid RightFieldId { get; set; }
    }

    public class ViewField :  IViewField
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public int Position { get; set; }
    }
}