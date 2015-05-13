using System;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage
{
    public interface IStorageEntityRelation : IStorageObject
    {
        string Name { get; set; }

        string Label { get; set; }

        string Description { get; set; }

        bool System { get; set; }

        EntityRelationType RelationType { get; set; }

        Guid OriginEntityId { get; set; }

        Guid OriginFieldId { get; set; }

        Guid TargetEntityId { get; set; }

        Guid TargetFieldId { get; set; }
    }
}