using System;

namespace WebVella.ERP.Core
{
    public interface IUrlField : IField
    {
        new string DefaultValue { get; set; }

        int MaxLength { get; set; }

        bool OpenTargetInNewWindow { get; set; }

        string Value { get; set; }
    }
}
