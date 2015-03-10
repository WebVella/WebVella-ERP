using System;

namespace WebVella.ERP.Core
{
    public interface IImageField : IField
    {
        new IFileFieldValue DefaultValue { get; set; }
    }
}