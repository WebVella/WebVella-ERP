using System;
using System.Collections.Generic;
using WebVella.ERP.Api;

namespace WebVella.ERP.Storage
{
    public interface IStorageFormField
    {
        Guid Id { get; set; }

        Guid EntityId { get; set; }

        FormColumns Column { get; set; }

        int Position { get; set; }
    }

    public interface IStorageForm
    {
        string Name { get; set; }

        string DisplayName { get; set; }

        IList<IStorageFormField> Fields { get; set; }
    }
}