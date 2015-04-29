using System;

namespace WebVella.ERP.Api.Models
{
    public class ImageField : Field
    {
        public new string DefaultValue { get; set; }

        public string TargetEntityType { get; set; }

        public string RelationshipName { get; set; }
    }
}