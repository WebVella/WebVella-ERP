using System;

namespace WebVella.ERP.Core
{
    public interface ISelectField : IField
    {
        new ITextFieldValue DefaultValue { get; set; }
    }
}