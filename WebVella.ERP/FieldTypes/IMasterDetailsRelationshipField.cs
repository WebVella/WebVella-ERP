
using System;

namespace WebVella.ERP.Core
{
    public interface IMasterDetailsRelationshipField : IField
    {
        new string DefaultValue { get; set; }

        string Value { get; set; }
    }
}