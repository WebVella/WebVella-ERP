using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoRecordsList : IStorageRecordsList
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public RecordsListTypes Type { get; set; }

        public IList<IStorageRecordsListFilter> Filters { get; set; }

        public IList<IStorageRecordsListField> Fields { get; set; }

        public MongoRecordsList()
        {
            Filters = new List<IStorageRecordsListFilter>();
            Fields = new List<IStorageRecordsListField>();
        }
    }

    public class MongoRecordsListFilter : IStorageRecordsListFilter
    {
        public Guid EntityId { get; set; }

        public Guid FieldId { get; set; }

        public FilterOperatorTypes Operator { get; set; }

        public string Value { get; set; }
    }

    public class MongoRecordsListField : IStorageRecordsListField
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public int Position { get; set; }
    }
}