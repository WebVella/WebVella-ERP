
using System;

namespace WebVella.ERP.Core
{
    public interface IMultiSelectField : IField
    {
        new ITextArrayFieldValue DefaultValue { get; set; }
    }
}