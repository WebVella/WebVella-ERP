using System;
using System.IO;

namespace WebVella.ERP.Core
{
    public interface IImageField : IField
    {
        new string DefaultValue { get; set; }

        string TargetEntityType { get; set; }

        string RelationshipName { get; set; }

        string Value { get; set; }
    }
}