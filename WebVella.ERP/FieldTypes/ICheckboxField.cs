using System;

namespace WebVella.ERP.Core
{
    public interface ICheckboxField : IField
    {
        new IBooleanFieldValue DefaultValue { get; set; }
    }
}