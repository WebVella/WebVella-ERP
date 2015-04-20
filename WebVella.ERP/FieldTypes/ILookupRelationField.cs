using System;

namespace WebVella.ERP.Core
{
    public interface ILookupRelationField : IField
    {
        new string DefaultValue { get; set; }

        Guid Value { get; set; }
    }
}