
using System;

namespace WebVella.ERP.Core
{
    public interface IMasterDetailsRelationshipField : IField
    {
        new IRelationFieldValue DefaultValue { get; set; }
    }
}