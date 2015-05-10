using System;

namespace WebVella.ERP.Api.Models
{
    public interface IFieldMeta
    {
        Guid EntityId { get; set; }

        string EntityName { get; set; }

        string ParentFieldName { get; set; }
    }
}
