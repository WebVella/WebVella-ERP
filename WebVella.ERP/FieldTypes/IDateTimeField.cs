using System;

namespace WebVella.ERP.Core
{
    public interface IDateTimeField : IField
    {
        new IDateTimeFieldValue DefaultValue { get; set; }
    }
}