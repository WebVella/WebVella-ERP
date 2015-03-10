using System;

namespace WebVella.ERP
{
    public interface IField
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string DisplayName { get; set; }

        string PlaceholderText { get; set; }

        string Description { get; set; }

        string HelpText { get; set; }

        bool Required { get; set; }

        bool Unique { get; set; }

        IFieldValue DefaultValue { get; set; }
    }
}