using System;

namespace WebVella.ERP.Core
{
    public interface IDateField : IField
    {
        new IDateTimeFieldValue DefaultValue { get; set; }
    }
}