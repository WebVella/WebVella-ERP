using System;

namespace WebVella.ERP.Core
{
    public interface IDateTimeField : IField
    {
        new DateTime DefaultValue { get; set; }

        string Format { get; set; }

        DateTime Value { get; set; }
    }
}