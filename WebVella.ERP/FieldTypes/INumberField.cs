using System;

namespace WebVella.ERP.Core
{
    public interface INumberField : IField
    {
        new INumberFieldValue DefaultValue { get; set; }
    }
}