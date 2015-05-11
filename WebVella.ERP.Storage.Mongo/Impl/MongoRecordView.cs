using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoRecordView : IStorageRecordView
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public IList<IStorageRecordViewField> Fields { get; set; }

        public MongoRecordView()
        {
            Fields = new List<IStorageRecordViewField>();
        }
    }

    public class MongoRecordViewField : IStorageRecordViewField
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public FormColumns Column { get; set; }

        public int Position { get; set; }
    }
}