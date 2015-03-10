using System;

namespace WebVella.ERP.Core
{
    public interface IMultiLineTextField : IField
    {
        new ITextFieldValue DefaultValue { get; set; }
    }
}