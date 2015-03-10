using System;

namespace WebVella.ERP.Core
{
    public interface IPercentField : IField
    {
        new INumberFieldValue DefaultValue { get; set; }
    }
}