
using System;

namespace WebVella.ERP.Core
{
    public interface IHtmlField : IField
    {
        new ITextFieldValue DefaultValue { get; set; }
    }
}