using System;

namespace WebVella.ERP
{
    public abstract class Field
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string PlaceholderText { get; set; }

        public string Description { get; set; }

        public string HelpText { get; set; }

        public bool? Required { get; set; }

        public bool? Unique { get; set; }

        public bool? Searchable { get; set; }

        public bool? Auditable { get; set; }

        public bool? System { get; set; }

        public virtual object DefaultValue { get; set; }
    }
}