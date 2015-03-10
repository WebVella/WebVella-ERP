using System;

namespace WebVella.ERP
{
    public abstract class MongoBaseField : IField
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string PlaceholderText { get; set; }

        public string Description { get; set; }

        public string HelpText { get; set; }

        public bool Required { get; set; }

        public bool Unique { get; set; }

        public virtual IFieldValue DefaultValue { get; set; }
    }
}