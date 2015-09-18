using System;

namespace WebVella.ERP.Storage
{
    public interface IStorageField
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string Label { get; set; }

        string PlaceholderText { get; set; }

        string Description { get; set; }

        string HelpText { get; set; }

        bool Required { get; set; }

        bool Unique { get; set; }

        bool Searchable { get; set; }

        bool Auditable { get; set; }

        bool System { get; set; }

        FieldPermissions Permissions { get; set; }

        bool EnableSecurity { get; set; }
    }
}