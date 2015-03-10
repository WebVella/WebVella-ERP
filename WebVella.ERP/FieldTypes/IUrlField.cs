using System;

namespace WebVella.ERP.Core
{
    public interface IUrlField : IField
    {
        new ITextFieldValue DefaultValue { get; set; }
    }
}
