using System;

namespace WebVella.ERP.Core
{
    public interface IEmailField : IField
    {
        new ITextFieldValue DefaultValue { get; set; }
    }
}