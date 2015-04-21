using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage.Mongo.Impl
{
    public class MongoForm : IStorageForm
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public IList<IStorageFormField> Fields { get; set; }
    }

    public class FormField : IStorageFormField
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public FormColumns Column { get; set; }

        public int Position { get; set; }
    }
}