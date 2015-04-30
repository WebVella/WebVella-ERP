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

        public string Label { get; set; }

        public ViewTypes Type { get; set; }

        public IList<IStorageViewFilter> Filters { get; set; }

        public IList<IStorageViewField> Fields { get; set; }

        public MongoView()
        {
            Filters = new List<IStorageViewFilter>();
            Fields = new List<IStorageViewField>();
        }
    }

    public class MongoViewFilter :  IStorageViewFilter
    {
        public Guid EntityId { get; set; }

        public Guid FieldId { get; set; }

        public FilterOperatorTypes Operator { get; set; }

        public string Value { get; set; }
    }

    public class MongoViewField :  IStorageViewField
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public int Position { get; set; }
    }
}