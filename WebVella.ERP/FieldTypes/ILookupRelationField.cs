using System;

namespace WebVella.ERP.Core
{
    public interface ILookupRelationField : IField
    {
        new IRelationFieldValue DefaultValue { get; set; }
    }
}