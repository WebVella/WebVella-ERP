using System;

namespace WebVella.ERP.Core
{
    public interface IFileField : IField
    {
        new string DefaultValue { get; set; }

        string Value { get; set; }
    }
}