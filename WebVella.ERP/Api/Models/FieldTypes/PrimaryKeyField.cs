using System;

namespace WebVella.ERP.Api.Models
{
    public class PrimaryKeyField : Field
    {
        public new Guid? DefaultValue { get; set; }
    }
}