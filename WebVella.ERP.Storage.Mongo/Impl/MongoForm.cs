using System;
using System.Collections.Generic;

namespace WebVella.ERP.Storage.Mongo.Impl
{
    public class MongoForm : IForm
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public IList<IFormField> Fields { get; set; }
    }

    public class FormField : IFormField
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public FormColumns Column { get; set; }

        public int Position { get; set; }
    }
}