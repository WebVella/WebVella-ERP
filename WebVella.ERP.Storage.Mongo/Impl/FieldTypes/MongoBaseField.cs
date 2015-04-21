using System;
using WebVella.ERP.Storage;


namespace WebVella.ERP
{
    public abstract class MongoBaseField : IStorageField
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string PlaceholderText { get; set; }

        public string Description { get; set; }

        public string HelpText { get; set; }

        public bool Required { get; set; }

        public bool Unique { get; set; }

        public bool Searchable { get; set; }

        public bool Auditable { get; set; }

        public bool System { get; set; }

        public virtual object DefaultValue { get; set; }
    }
}