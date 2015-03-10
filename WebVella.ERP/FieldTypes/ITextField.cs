using System;

namespace WebVella.ERP.Core
{
    public interface ITextField : IField
    {
        new ITextFieldValue DefaultValue { get; set; }
    }
}