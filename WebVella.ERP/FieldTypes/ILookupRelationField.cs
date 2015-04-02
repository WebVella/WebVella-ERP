using System;

namespace WebVella.ERP.Core
{
    public interface ILookupRelationField : IField
    {
        new string DefaultValue { get; set; }

        string Value { get; set; }
    }
}