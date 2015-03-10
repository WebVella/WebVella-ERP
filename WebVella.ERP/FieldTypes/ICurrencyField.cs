using System;

namespace WebVella.ERP.Core
{
    public interface ICurrencyField : IField
    {
        new INumberFieldValue DefaultValue { get; set; }
    }
}