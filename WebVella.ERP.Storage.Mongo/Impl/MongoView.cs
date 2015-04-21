using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage.Mongo.Impl
{
    public class MongoView : IStorageView
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public ViewTypes Type { get; set; }

        public IList<IStorageViewFilter> Filter { get; set; }

        public IList<IStorageViewField> Fields { get; set; }
    }

    public class MongoViewFilter :  IStorageViewFilter
    {
        public Guid LeftEntityId { get; set; }

        public Guid LeftFieldId { get; set; }

        public FilterOperatorTypes Operator { get; set; }

        public Guid RightEntityId { get; set; }

        public Guid RightFieldId { get; set; }
    }

    public class MongoViewField :  IStorageViewField
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public int Position { get; set; }
    }
}