using System;

namespace WebVella.ERP.Core
{
    public interface IPrimaryKeyField : IField
    {
        new Guid DefaultValue { get; set; }

        Guid Value { get; set; }
    }
}