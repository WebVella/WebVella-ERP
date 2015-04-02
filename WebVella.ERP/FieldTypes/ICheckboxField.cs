using System;

namespace WebVella.ERP.Core
{
    public interface ICheckboxField : IField
    {
        new bool DefaultValue { get; set; }

        bool Value { get; set; }
    }
}