
using System;

namespace WebVella.ERP.Core
{
    public interface IHtmlField : IField
    {
        new string DefaultValue { get; set; }

        string Value { get; set; }
    }
}