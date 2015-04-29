using System;

namespace WebVella.ERP.Api.Models
{
    public class LookupRelationField : Field
    {
        public Guid? RelatedEntityId { get; set; }
    }
}