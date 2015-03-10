using System;

namespace WebVella.ERP.Core
{
    public interface IPhoneField : IField
    {
        new ITextFieldValue DefaultValue { get; set; }
    }
}