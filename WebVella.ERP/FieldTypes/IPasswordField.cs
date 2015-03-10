
using System;

namespace WebVella.ERP.Core
{
    public interface IPasswordField : IField
    {
        new ITextFieldValue DefaultValue { get; set; }
    }
}