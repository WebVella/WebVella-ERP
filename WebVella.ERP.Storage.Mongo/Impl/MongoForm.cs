using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoForm : IStorageForm
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public IList<IStorageFormField> Fields { get; set; }

        public MongoForm()
        {
            Fields = new List<IStorageFormField>();
        }
    }

    public class MongoFormField : IStorageFormField
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public FormColumns Column { get; set; }

        public int Position { get; set; }
    }
}