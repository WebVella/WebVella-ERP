using System;

namespace WebVella.ERP.Api.Models
{
    public class PrimaryKeyField : Field
    {
        public Guid? DefaultValue { get; set; }
    }
}