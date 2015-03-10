using System;

namespace WebVella.ERP.Core
{
    public interface IPrimaryKeyField : IField
    {
        new IGuidFieldValue DefaultValue { get; set; }
    }
}