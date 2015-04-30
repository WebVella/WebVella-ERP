using System;

namespace WebVella.ERP.Api.Models
{
    public class MasterDetailsRelationshipField : Field
    {
        public Guid? RelatedEntityId { get; set; }
    }
}