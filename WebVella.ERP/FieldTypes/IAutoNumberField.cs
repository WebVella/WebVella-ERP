using System;

namespace WebVella.ERP.Core
{
    public interface IAutoNumberField : IField
    {
        new INumberFieldValue DefaultValue { get; set; }
    }
}