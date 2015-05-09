using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Api.Models
{
    public class Role : EntityRecord
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
